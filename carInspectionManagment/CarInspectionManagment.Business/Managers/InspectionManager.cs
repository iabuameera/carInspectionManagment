using CarInspectionManagment.Business.Infrastructure;
using CarInspectionManagment.Business.Infrastructure.Message;
using CarInspectionManagment.Business.Managers.ViewCache;
using CarInspectionManagment.Business.Persistence.Entities;
using CarInspectionManagment.Business.Persistence.Repositories;
using CarInspectionManagment.Contract;
using CarInspectionManagment.Contract.Filters;
using CarInspectionManagment.Contract.Inspection.Models;
using CarInspectionManagment.Contract.Inspection.Resource;
using MassTransit;
using MassTransit.Clients;
using MassTransit.Transports;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarInspectionManagment.Business.Managers
{
    public interface IInspectionManager
    {
        Task<InspectionResource> CreateInspectionAsync(InspectionModel model);
        Task DeleteInspectionAsync(int id);
        Task<InspectionResource> GetInspectionByIdAsync(int id);
        Task<List<InspectionResource>> GetInspectionsAsync(InspectionListFilter filters);
        Task<InspectionResource> UpdateInspectionAsync(int id, InspectionModel model);
    }

    public class InspectionManager : IInspectionManager
    {
        private readonly IInspectionRepository _inspectionRepository;

        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRequestClient<GetInspectionsResponse> _client;
        public InspectionManager(IInspectionRepository inspectionRepository, IRequestClient<GetInspectionsResponse> client, IPublishEndpoint publishEndpoint)
        {
            _inspectionRepository = inspectionRepository;
            _client = client;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<List<InspectionResource>> GetInspectionsAsync(InspectionListFilter filters)
        {
            // publish message to get from cache
            var key = $"Inspection_key_{filters.Vinnumber}_{filters.DateOfCreation}_{filters.Reason}";

            var response = await _client.GetResponse<GetInspectionsResponse>(key);

            if (response.Message.Inspections != null && response.Message.Inspections.Any())
            {
                return response.Message.Inspections.Select(ins => ins.ToInspectionResources()).ToList();
            }

            var entities = await _inspectionRepository.LoadinspectionsAsync(filters);
            var result = entities.Select(_ => _.ToResource()).ToList();

            await _publishEndpoint.Publish(new GetInspectionsResponse { Inspections = result });


            return result;
      
         
        }

        public async Task<InspectionResource> GetInspectionByIdAsync(int id)
        {
            var entity = await _inspectionRepository.LoadInspectionByIdAsync(id);
            CheckIfEntityNotExists(entity, id);
            return entity.ToResource();
        }

        public async Task<InspectionResource> CreateInspectionAsync(InspectionModel model)
        {
            model.EnsureValidModel();
            var entity = new CarInspection
            {
                DateOfCreation = model.DateOfCreation,
                Vinnumber = model.Vinnumber,
                Reason = model.Reason
            };

            await _inspectionRepository.CreateInspectionAsync(entity);
            var resource = entity.ToResource();
            await _publishEndpoint.Publish(new CreateInspectionResponse { Inspection = resource });

            return resource;
        }
        public async Task<InspectionResource> UpdateInspectionAsync(int id, InspectionModel model)
        {
            var entity = await _inspectionRepository.LoadInspectionByIdAsync(id);
            CheckIfEntityNotExists(entity, id);
            model.EnsureValidModel();
            entity.Vinnumber = model.Vinnumber;
            entity.Reason = model.Reason;
            entity.DateOfCreation = model.DateOfCreation;

            await _inspectionRepository.UpdateInspectionAsync(entity);
            var resource = entity.ToResource();
            return resource;
        }

        public async Task DeleteInspectionAsync(int id)
        {
            var entity = await _inspectionRepository.LoadInspectionByIdAsync(id);
            CheckIfEntityNotExists(entity, id);
            
            await _inspectionRepository.DeleteInspection(entity);
            var resource = entity.ToResource();
        }

        

     

        private void CheckIfEntityNotExists(CarInspection entity, int id)
        {
            if (entity == null)
            {
                ExceptionManager.ThrowItemNotFoundException("Id", ValidationMessages.GetNotFoundMessage("CarInspection", id.ToString()));
            }
        }
    }
}
