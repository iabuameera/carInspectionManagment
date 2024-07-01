using CarInspectionManagment.Business.Managers;
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
        public async Task Consume(ConsumeContext<CreateInspectionResponse> context)
        {
            var list = context.Message;
            Console.WriteLine($"Received list: {list.Inspection} ");
        }
    }
}
