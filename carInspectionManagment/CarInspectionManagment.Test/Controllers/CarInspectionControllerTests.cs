using CarInspectionManagment.Api.Controllers;
using CarInspectionManagment.Business.Managers;
using CarInspectionManagment.Contract.Inspection.Resource;
using Lib.AspNetCore.ServerTiming;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Lib.AspNetCore.ServerTiming.Http.Headers;
using CarInspectionManagment.Contract.Filters;
using CarInspectionManagment.Contract.Inspection.Models;
using System.Net;
using Microsoft.AspNetCore.Routing;

namespace CarInspectionManagment.Test.Controllers
{
   public class CarInspectionControllerTests
    {

        private List<InspectionResource> Inspection { get; }
        private readonly Mock<IInspectionManager> _inspectionManager;
        private readonly Mock<IServerTiming> _serverTimingMock;
        private readonly CarInspectionController _carInspectionController;

        public CarInspectionControllerTests()
        {
            Inspection = new List<InspectionResource> { 
            
            new InspectionResource{DateOfCreation= DateTime.Now,Reason="CheckIn", Id=1,Vinnumber="123-456-987" },
                new InspectionResource{DateOfCreation= DateTime.Now,Reason="CheckOut", Id=2,Vinnumber="123-456-987" }
            };
            _inspectionManager = new Mock<IInspectionManager>();
            _serverTimingMock = new Mock<IServerTiming>();

            _carInspectionController = new CarInspectionController(_inspectionManager.Object, _serverTimingMock.Object);// GetBanksController(_bankManager, _serverTimingMock);

        }

        [Fact]
        public async Task GetInspectionById_Should_ReturnOk()
        {


            // Setup
            _inspectionManager
                .Setup(x => x.GetInspectionByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new InspectionResource()));




            // Act
            var result = await _carInspectionController.GetInspectionById(1);
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
            _inspectionManager.Verify(x => x.GetInspectionByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetAllInspections()
        {
            ServerTimingMetric mertics =
              new ServerTimingMetric("name", "description");

            var merticsArray = new[] { mertics };
            _serverTimingMock.Setup(x => x.Metrics).Returns(merticsArray.ToList());

            //Arrange
            _inspectionManager
                .Setup(x => x.GetInspectionsAsync(It.IsAny<InspectionListFilter>()))
                .Returns(Task.FromResult(Inspection));
           
            //Act
            var result = await _carInspectionController.GetAllInspections(It.IsAny<InspectionListFilter>());
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

        }


        [Fact]
        public async Task Create_CarInspection()
        {
            var dt = DateTime.Now;
            //Arrange
            _inspectionManager
                .Setup(x => x.CreateInspectionAsync(It.IsAny<InspectionModel>()))
                .Returns(Task.FromResult(new InspectionResource
                {
                   DateOfCreation= dt,
                   Id=3,
                   Reason="CheckOut",
                   Vinnumber="123-654-985"
                }));

            var model = new InspectionModel
            {
               Vinnumber= "123-654-985",
               
               Reason= "CheckOut",
               DateOfCreation= dt
            };
            //Act

            var defaultWebRoot = "http://localhost:50739/accounting";

            var routeData = new RouteData();
            routeData.Values.Add("controller", "v1/{customerSlug}/Banks");
            routeData.Values.Add("action", "");


            var urlHelper = new Mock<IUrlHelper>();
            urlHelper.Setup(o => o.Link(It.IsAny<String>(), It.IsAny<Object>())).Returns(defaultWebRoot);
            _carInspectionController.Url = urlHelper.Object;

            var result = await _carInspectionController.CreateInspection(model);
            //Assert
            Assert.NotNull(result);
            Assert.Equal((result as CreatedResult)?.StatusCode, (int)HttpStatusCode.Created);

        }

        [Fact]
        public async Task Update_Inspection()
        {

            //Arrange
            _inspectionManager
                .Setup(x => x.UpdateInspectionAsync(It.IsAny<int>(), It.IsAny<InspectionModel>()))
                .Returns(Task.FromResult(new InspectionResource
                {
                    Id = 3,
                   Vinnumber="123-951-756",
                   Reason="CheckIn"
                }));

            var model = new InspectionModel
            {
               Reason="CheckIn",
               Vinnumber="159-963-741",
               DateOfCreation=DateTime.Now
            };
            //Act
            var result = await _carInspectionController.UpdateInspection(It.IsAny<int>(), model);
            //Assert
            Assert.NotNull(result);
            Assert.Equal((result as OkObjectResult).StatusCode, (int)HttpStatusCode.OK);
            Assert.Equal(3, ((result as OkObjectResult).Value as InspectionResource).Id);

        }

        [Fact]
        public async Task Delete_Inspection()
        {

            //Act
            var result = await _carInspectionController.DeleteInspection(It.IsAny<int>());
            //Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
