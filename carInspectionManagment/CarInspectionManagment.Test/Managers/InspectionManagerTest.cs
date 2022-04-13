using CarInspectionManagment.Business.Infrastructure;
using CarInspectionManagment.Business.Managers;
using CarInspectionManagment.Business.Managers.ViewCache;
using CarInspectionManagment.Business.Persistence.Entities;
using CarInspectionManagment.Business.Persistence.Repositories;
using CarInspectionManagment.Contract;
using CarInspectionManagment.Contract.Filters;
using CarInspectionManagment.Contract.Inspection.Models;
using CarInspectionManagment.Contract.Inspection.Resource;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CarInspectionManagment.Test.Managers
{
    public class InspectionManagerTest
    {

        private readonly Mock<IInspectionRepository> _repository;
        private readonly Mock<ICarInspectionViewCacheManager> _carInspection;

        public InspectionManagerTest()
        {
            _repository = new Mock<IInspectionRepository>();
            _carInspection = new Mock<ICarInspectionViewCacheManager>();

        }

        [Fact]
        public async Task CarInspectionManagerkManager_GetCarInspection()
        {
            // Arrange
            var expectedEntities = GetCarInspectionResource();

            _repository.Setup(r => r.LoadinspectionsAsync(It.IsAny<InspectionListFilter>()))
                .Returns(Task.FromResult(expectedEntities));

            // Act
            var carInspectionRepository = GetManager();
            await carInspectionRepository.GetInspectionsAsync(It.IsAny<InspectionListFilter>());

            // Assert
            _repository.Verify(m => m.LoadinspectionsAsync(It.IsAny<InspectionListFilter>()), Times.Once);
        }


        [Fact]
        public async Task CarInspectionManager_GetCarInspectionsByIdAsync()
        {
            // Arrange
            int carInspectionId = 1;
            var expectedEntity = GetCarInspectionResource()
                .FirstOrDefault(e => e.Id == carInspectionId);

            _repository.Setup(r => r.LoadInspectionByIdAsync(carInspectionId))
                .Returns(Task.FromResult(expectedEntity));

            // Act
            var CarInspectionRepository = GetManager();
            var state = await CarInspectionRepository.GetInspectionByIdAsync(carInspectionId);

            // Assert
            Assert.Equal(carInspectionId, state.Id);
            _repository.Verify(m => m.LoadInspectionByIdAsync(carInspectionId), Times.Once);
        }

        [Fact]
        public async Task CarInspectionManager_GetCarInspectionByIdAsync_Failed_NotFound()
        {
            // Arrange
            var id = 999;

            _repository.Setup(r => r.LoadInspectionByIdAsync(id))
                .Returns(Task.FromResult<CarInspection>(null));
            var CarInspectionRepository = GetManager();

            // Act
            var ex = await Assert.ThrowsAnyAsync<ItemNotFoundException>
           (async () => {
               await CarInspectionRepository.GetInspectionByIdAsync(id);
           });

            //Assert
            ex.GetErrorString("Id")
                .Should().BeEquivalentTo(ValidationMessages.GetNotFoundMessage("CarInspection", id.ToString()));
             
        }


        [Fact]
        public async Task CarInspectionManager_CreateAsync()
        {
            // Arrange
            int CarInspectionId = 1;
            var expectedEntity = GetCarInspectionResource()
                .FirstOrDefault(e => e.Id == CarInspectionId);

            _repository.Setup(r => r.LoadInspectionByIdAsync(CarInspectionId))
                .Returns(Task.FromResult(expectedEntity));
            _repository
                .Setup(w => w.CreateInspectionAsync(It.IsAny<CarInspection>()))
                .Returns(Task.CompletedTask);
            var model = GetValidModel();

            // Act
            var manager = GetManager();
            var state = await manager.CreateInspectionAsync(model);

            // Assert
            Assert.Equal(model.Vinnumber, state.Vinnumber);
            Assert.Equal(model.Reason, state.Reason);
            Assert.Equal(model.DateOfCreation, state.DateOfCreation);
          

            _repository.Verify(m => m.CreateInspectionAsync(It.IsAny<CarInspection>()), Times.Once);
        }



        [Fact]
        public async Task CarInspectionManager_UpdateAsync()
        {
            // Arrange
            int id = 1;
            var expectedEntity = GetCarInspectionResource()
                .FirstOrDefault(e => e.Id == id);

            _repository.Setup(r => r.LoadInspectionByIdAsync(id))
                .Returns(Task.FromResult(expectedEntity));
            _repository
                .Setup(w => w.UpdateInspectionAsync(It.IsAny<CarInspection>()))
                .Returns(Task.CompletedTask);
            var model = GetValidModel();

            // Act
            var manager = GetManager();
            var state = await manager.UpdateInspectionAsync(id, model);

            // Assert
            Assert.Equal(id, state.Id);
            _repository.Verify(m => m.UpdateInspectionAsync(It.IsAny<CarInspection>()), Times.Once);
        }

        [Fact]
        public async Task CarInspectionManager_UpdateAsync_Failed_NotFound()
        {
            // Arrange
            var id = 999;
            _repository.Setup(r => r.LoadInspectionByIdAsync(id))
                .Returns(Task.FromResult<CarInspection>(null));

            var manager = GetManager();

            var model = new InspectionModel
            {
               Reason= "checkIn",
               Vinnumber="1569-852-963"
            };
            // Act
            var ex = await Assert.ThrowsAnyAsync<ItemNotFoundException>
            (async () =>
            {
                await manager.UpdateInspectionAsync(id, model);
            });
            //Assert
            ex.GetErrorString("Id")
                .Should().BeEquivalentTo(
              
                    $"CarInspection with Id: {id} is not found."
                );
        }


        [Fact]
        public async Task CarInspectionManager_DeleteAsync()
        {
            // Arrange
            int id = 1;
            var expectedEntity = GetCarInspectionResource()
                .FirstOrDefault(e => e.Id == id);

            _repository.Setup(r => r.LoadInspectionByIdAsync(id))
                .Returns(Task.FromResult(expectedEntity));

            // Act
            var manager = GetManager();
            await manager.DeleteInspectionAsync(id);

            // Assert
            _repository.Verify(m => m.DeleteInspection(expectedEntity));
        }


        [Fact]
        public async Task BankManager_DeleteAsync_Failed_NotFound()
        {
            // Arrange
            var id = 50;

            _repository.Setup(r => r.LoadInspectionByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<CarInspection>(null));

            var bankRepository = GetManager();
            // Act
            var ex = await Assert.ThrowsAnyAsync<ItemNotFoundException>
           (async () =>
           {
               await bankRepository.DeleteInspectionAsync(id);
           });
            //Assert
            ex.GetErrorString("Id")
                .Should().BeEquivalentTo(
                    $"CarInspection with Id: {id} is not found."
                );
        }

        private static InspectionModel GetValidModel()
        {
            return new InspectionModel
            {
                DateOfCreation = DateTime.Now,
                Reason = "CheckIn",
                Vinnumber = "159-789-852"
            };
        }

        private InspectionManager GetManager()
        {
            return new InspectionManager(
                _repository.Object,
                _carInspection.Object
                
                );
        }
        private List<CarInspection> GetCarInspectionResource()
        {

            return new List<CarInspection> {

            new CarInspection{DateOfCreation= DateTime.Now,Reason="CheckIn", Id=1,Vinnumber="123-456-987"  },
                new CarInspection{DateOfCreation= DateTime.Now,Reason="CheckOut", Id=2,Vinnumber="123-456-987" }
            };
        }
    }
}
