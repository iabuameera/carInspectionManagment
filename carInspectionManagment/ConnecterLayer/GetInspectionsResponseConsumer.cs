using CarInspectionManagment.Business.Managers;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConncterLayer
{
    public class GetInspectionsResponseConsumer : IConsumer<GetInspectionsResponse>
    {
        public Task Consume(ConsumeContext<GetInspectionsResponse> context)
        {
            var inspections = context.Message.Inspections;
            Console.WriteLine("Received inspections:");
            foreach (var inspection in inspections)
            {
                Console.WriteLine($"- id:{inspection.Id}, VINNO:{inspection.Vinnumber},Reason:{inspection.Reason}");
            }
            return Task.CompletedTask;
        }
    }
}
