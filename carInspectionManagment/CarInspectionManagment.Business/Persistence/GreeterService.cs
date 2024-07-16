using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Grpc.Core;
using System.Collections.Generic;
using CarInspectionManagment.Business.Managers;


namespace CarInspectionManagment.Business.Persistence
{
    public class GreeterService : CarService.CarServiceBase
    {

        private readonly InspectionManager _inspectionManager;
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<CarResponse> GetInspections(Id id, ServerCallContext context)
        {
            var cars = _inspectionManager.GetInspectionByIdAsync(id);
            CarResponse response = new CarResponse();
            response.Cars.AddRange(_cars);
            return Task.FromResult(response);
        }
    }
}
