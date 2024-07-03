using CarInspectionManagment.Business.Infrastructure.Message;
using CarInspectionManagment.Business.Managers;
using CarInspectionManagment.Business.Managers.ViewCache;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConncterLayer
{

    public class CreateInspectionCacheUpdaterConsumer : IConsumer<CreateInspectionResponse>
    {

        private readonly ICarInspectionViewCacheManager _carInspectionViewCacheManager;
        private readonly ILogger<CreateInspectionCacheUpdaterConsumer> _logger;

        public CreateInspectionCacheUpdaterConsumer(ICarInspectionViewCacheManager carInspectionViewCacheManager, ILogger<CreateInspectionCacheUpdaterConsumer> logger)
        {
            _carInspectionViewCacheManager = carInspectionViewCacheManager;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CreateInspectionResponse> context)
        {
            try
            {
                var inspectionDetails = context.Message.Inspection;

                if (inspectionDetails != null)
                {
                    await _carInspectionViewCacheManager.SetCarInspectAsync(inspectionDetails);
                }
                else
                {
                    _logger.LogWarning("Received a CreateInspectionResponse message with null inspection details.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing CreateInspectionResponse message.");
            }
        }

    }
}
