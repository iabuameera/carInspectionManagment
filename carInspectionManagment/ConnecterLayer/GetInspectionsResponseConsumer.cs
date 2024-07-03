using CarInspectionManagment.Business.Managers;
using CarInspectionManagment.Business.Managers.ViewCache;
using CarInspectionManagment.Contract.Inspection.Resource;
using ConnecterLayer;
using MassTransit;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConncterLayer
{
    public class GetInspectionsResponseConsumer : IConsumer<GetInspectionsResponse>
    {

        private readonly ICarInspectionViewCacheManager _carInspectionViewCacheManager;

        public GetInspectionsResponseConsumer(ICarInspectionViewCacheManager carInspectionViewCacheManager)
        {
            _carInspectionViewCacheManager = carInspectionViewCacheManager;
        }

        public async Task Consume(ConsumeContext<GetInspectionsResponse> context)
        {
            var result = await _carInspectionViewCacheManager.GetInspectionsAsync();
            var inspections = context.Message.Inspections;
            var response = new GetInspectionsResponse();

            if (inspections.SequenceEqual(result))
            {
                response = new GetInspectionsResponse
                {
                    Inspections = result,
                };
            }
            else
            {
                response = new GetInspectionsResponse
                {
                    Inspections = inspections,
                };
            }

            await context.RespondAsync<GetInspectionsResponse>(new
            {
                 Inspections = response,
            });
        }


    }
}
