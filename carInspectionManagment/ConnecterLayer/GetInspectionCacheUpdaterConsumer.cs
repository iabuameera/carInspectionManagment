using CarInspectionManagment.Business.Infrastructure.Message;
using CarInspectionManagment.Business.Managers.ViewCache;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ConncterLayer
{
    public class GetInspectionCacheUpdaterConsumer : IConsumer<GetInspectionsResponse>
    {

        private readonly ICarInspectionViewCacheManager _carInspectionViewCacheManager;
        private readonly ILogger<GetInspectionCacheUpdaterConsumer> _logger;
        public GetInspectionCacheUpdaterConsumer(ICarInspectionViewCacheManager carInspectionViewCacheManager, ILogger<GetInspectionCacheUpdaterConsumer> logger)
        {
            _carInspectionViewCacheManager = carInspectionViewCacheManager;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<GetInspectionsResponse> context)
        {

            try
            {
                await _carInspectionViewCacheManager.GetInspectionsAsync();
                var inspections = context.Message.Inspections;

                var response = new GetInspectionsResponse
                {
                    Inspections = inspections,
                };

                await context.RespondAsync<GetInspectionsResponse>(new
                {
                    Inspections = response,
                });
            }
            catch (Exception ex)
             {
                _logger.LogError(ex, "Error occurred while processing GetInspectionsResponse message.");
             }

        }


    }
}
