using CarInspectionManagment.Business.Infrastructure;
using CarInspectionManagment.Business.Managers.ViewCache;
using CarInspectionManagment.Business.Persistence.Entities;
using CarInspectionManagment.Business.Persistence.Repositories;
using CarInspectionManagment.Contract;
using CarInspectionManagment.Contract.Filters;
using CarInspectionManagment.Contract.Inspection.Models;
using CarInspectionManagment.Contract.Inspection.Resource;
using MassTransit;
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
        private readonly ICarInspectionViewCacheManager _carInspectionViewCacheManager;
        private readonly IPublishEndpoint _publishEndpoint;
        public InspectionManager(IInspectionRepository inspectionRepository, ICarInspectionViewCacheManager carInspectionViewCacheManager, IPublishEndpoint publishEndpoint)
        {
            _inspectionRepository = inspectionRepository;
            _carInspectionViewCacheManager = carInspectionViewCacheManager;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<List<InspectionResource>> GetInspectionsAsync(InspectionListFilter filters)
        {
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
            await _carInspectionViewCacheManager.SetCarInspectAsync(resource);

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
            await _carInspectionViewCacheManager.SetCarInspectAsync(resource);
            return resource;
        }

        public async Task DeleteInspectionAsync(int id)
        {
            var entity = await _inspectionRepository.LoadInspectionByIdAsync(id);
            CheckIfEntityNotExists(entity, id);
            
            await _inspectionRepository.DeleteInspection(entity);
            var resource = entity.ToResource();
           await _carInspectionViewCacheManager.DeleteCarInspectAsync(resource);
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
