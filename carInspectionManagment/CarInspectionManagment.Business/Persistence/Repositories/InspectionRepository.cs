using CarInspectionManagment.Business.Persistence.Entities;
using CarInspectionManagment.Contract.Filters;
using CarInspectionManagment.Contract.Inspection.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CarInspectionManagment.Business.Persistence.Repositories
{
    public interface IInspectionRepository
    {
        Task CreateInspectionAsync(CarInspection entity);
        Task DeleteInspection(CarInspection entity);
        Task<bool> ExistsAsync(Expression<Func<CarInspection, bool>> filters);
        Task<CarInspection> LoadInspectionByIdAsync(int id);
        Task<List<CarInspection>> LoadinspectionsAsync(InspectionListFilter filter);
        Task UpdateInspectionAsync(CarInspection entity);
    }

    public class InspectionRepository : IInspectionRepository
    {
        private readonly IRepository<CarInspection> _inspectionsRepository;

        public InspectionRepository(
  IRepository<CarInspection> inspectionsRepository)
        {
            _inspectionsRepository = inspectionsRepository;
        }

        public async Task<List<CarInspection>> LoadinspectionsAsync(InspectionListFilter filter)
        {
            var filters = GetFilters(filter);
            return await _inspectionsRepository.GetItemsAsync(filters, null, filter, typeof(InspectionResource), filter.OrderBy);
        }

        public async Task<CarInspection> LoadInspectionByIdAsync(int id)
        {
            InspectionListFilter filter = new InspectionListFilter { Id = id };
            var filters = GetFilters(new InspectionListFilter { Id=id});
            var r= await _inspectionsRepository.GetItemsAsync(filters, null, filter, typeof(InspectionResource), filter.OrderBy);

            return r.FirstOrDefault();
        }

        public async Task CreateInspectionAsync(CarInspection entity)
        {
            _inspectionsRepository.Add(entity);
            await _inspectionsRepository.SaveAsync();
        }

        public async Task UpdateInspectionAsync(CarInspection entity)
        {
            _inspectionsRepository.Update(entity);
            await _inspectionsRepository.SaveAsync();
        }

        public async Task DeleteInspection(CarInspection entity)
        {
            _inspectionsRepository.Delete(entity);
            await _inspectionsRepository.SaveAsync();
        }


        public async Task<bool> ExistsAsync(Expression<Func<CarInspection, bool>> filters)
        {
            return await _inspectionsRepository.ExistsAsync(filters);
        }
        private static Expression<Func<CarInspection, bool>>[] GetFilters(
            InspectionListFilter filter)
        {
            var filters = new List<Expression<Func<CarInspection, bool>>>();
            if (!string.IsNullOrWhiteSpace(filter?.Vinnumber))
                filters.Add(e => e.Vinnumber.Contains(filter.Vinnumber));
            if (!string.IsNullOrWhiteSpace(filter?.Reason))
                filters.Add(e => e.Reason.Contains(filter.Reason));

            if (filter.DateOfCreation.HasValue)
                filters.Add(e => e.DateOfCreation == filter.DateOfCreation);


            return filters.ToArray();
        }
    }



}
