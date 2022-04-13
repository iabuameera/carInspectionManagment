using CarInspectionManagment.Business.Persistence;
using CarInspectionManagment.Business.Persistence.Entities;
using CarInspectionManagment.Business.Persistence.Repositories;
using CarInspectionManagment.Contract.Filters;
using CarInspectionManagment.Contract.Inspection.Resource;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarInspectionManagment.Test.Repositories
{
   public class InspectionRepositoryTest
    {

        private readonly Mock<IRepository<CarInspection>> _repositoryMock;

        private readonly IInspectionRepository _repository;
        private List<CarInspection> _carInspection { get; }
        public InspectionRepositoryTest()
        {
            _carInspection = new List<CarInspection> {

            new CarInspection{DateOfCreation= DateTime.Now,Reason="CheckIn", Id=1,Vinnumber="123-456-987" },
                new CarInspection{DateOfCreation= DateTime.Now,Reason="CheckOut", Id=2,Vinnumber="123-456-987" }
            };

            _repositoryMock = new Mock<IRepository<CarInspection>>();
            _repository = new InspectionRepository(_repositoryMock.Object);
        }

     
        [Fact]
        public async Task CarInspectionRepository_LoadAllCarInspectionsAsync()
        {
            var filter = new InspectionListFilter();
            _repositoryMock.Setup(r => r.GetItemsAsync(It.IsAny<Expression<Func<CarInspection, bool>>[]>(), null, filter, typeof(InspectionResource), filter.OrderBy)).
                    Returns(Task.FromResult(_carInspection));

            await _repository.LoadinspectionsAsync(filter);
            _repositoryMock.Verify(x => x.GetItemsAsync(It.IsAny<Expression<Func<CarInspection, bool>>[]>(), null, filter, typeof(InspectionResource), filter.OrderBy), Times.Once);
        }
        [Fact]
        public async Task CarInspectionRepository_LoadAllCarInspectionsAsync_WithFilters()
        {
            var returnedEntities = _carInspection.Where(_ => _.Id == 1).ToList();
          



            var filter = new InspectionListFilter
            {
                Reason = "CheckIn",
                Vinnumber = "123-456-987"
            };

            _repositoryMock.Setup(r => r.GetItemsAsync(It.IsAny<Expression<Func<CarInspection, bool>>[]>(), null, filter, typeof(InspectionResource), filter.OrderBy)).
            Returns(Task.FromResult(returnedEntities));


            var entities = await _repository.LoadinspectionsAsync(filter);
            _repositoryMock.Verify(x => x.GetItemsAsync(It.IsAny<Expression<Func<CarInspection, bool>>[]>(), null, filter, typeof(InspectionResource), filter.OrderBy), Times.Once);

            Assert.Equal(returnedEntities.Count, entities.Count);
        }



        [Fact]
        public async Task CarInspectionRepository_CreateCarInspectionAsync()
        {
            _repositoryMock.Setup(a => a.Add(It.IsAny<CarInspection>(), null, null));

            await _repository.CreateInspectionAsync(new CarInspection
            {
                Id = 0
            });
            _repositoryMock.Verify(m => m.Add(It.IsAny<CarInspection>(), null, null));
            _repositoryMock.Verify(m => m.SaveAsync());
        }
        [Fact]
        public async Task CarInspectionRepository_UpdateCarInspectionAsync()
        {
            _repositoryMock.Setup(a => a.Update(It.IsAny<CarInspection>(), null, null));

            await _repository.UpdateInspectionAsync(new CarInspection
            {
                Id = 0
            });
            _repositoryMock.Verify(m => m.Update(It.IsAny<CarInspection>(), null, null));
            _repositoryMock.Verify(m => m.SaveAsync());
        }
        [Fact]
        public async Task CarInspectionRepository_DeleteCarInspectionAsync()
        {
            _repositoryMock.Setup(a => a.Delete(It.IsAny<CarInspection>()));

            await _repository.DeleteInspection(new CarInspection
            {
                Id = 1
            });
            _repositoryMock.Verify(m => m.Delete(It.IsAny<CarInspection>()));
            _repositoryMock.Verify(m => m.SaveAsync());
        }

    }
}
