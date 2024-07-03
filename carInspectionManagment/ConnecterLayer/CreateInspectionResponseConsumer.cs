using CarInspectionManagment.Business.Managers;
using CarInspectionManagment.Business.Managers.ViewCache;
using ConnecterLayer;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConncterLayer
{

    public class CreateInspectionResponseConsumer : IConsumer<CreateInspectionResponse>
    {

        private readonly IInspectionResponseService _inspectionResponseService;
        private readonly ICarInspectionViewCacheManager _carInspectionViewCacheManager;
        public CreateInspectionResponseConsumer(IInspectionResponseService inspectionResponseService, ICarInspectionViewCacheManager carInspectionViewCacheManager)
        {
            _inspectionResponseService = inspectionResponseService;
            _carInspectionViewCacheManager = carInspectionViewCacheManager;
        }

        public Task Consume(ConsumeContext<CreateInspectionResponse> context)
        {
            var inspectionDetails = context.Message.Inspection;

            //  _inspectionResponseService.SetCreateInspectionResponse(inspectionDetails);
            var resource = context.Message.Inspection;
            _carInspectionViewCacheManager.SetCarInspectAsync(resource);
            return Task.CompletedTask;
        }

    }
}
