using ConnecterLayer;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ConncterLayer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddSingleton<IBusControl>(prov => prov.GetRequiredService<IBusControl>());
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {

                cfg.Host(new Uri("rabbitmq://localhost:5672"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("create_queue", e =>
                {
                    e.Handler<CreateInspectionResponseConsumer>(ctx =>
                    {
                        return Console.Out.WriteLineAsync(ctx.Message.ToString());
                    });

                });
                cfg.ReceiveEndpoint("get_queue", e =>
                {
                    e.Handler<GetInspectionsResponseConsumer>(ctx =>
                    {
                        return Console.Out.WriteLineAsync(ctx.Message.ToString());
                    });

                });
            });
            bus.Start();
            //services.AddMassTransit(config =>
            //{
            //    config.AddConsumer<GetInspectionsResponseConsumer>();
            //    config.AddConsumer<CreateInspectionResponseConsumer>();

            //    config.UsingRabbitMq((context, cfg) =>
            //    {
            //        cfg.Host(new Uri("rabbitmq://localhost:5672"), h =>
            //        {
            //            h.Username("guest");
            //            h.Password("guest");
            //        });

            //        cfg.ReceiveEndpoint("creates_queue", e =>
            //        {
            //            e.ConfigureConsumer<CreateInspectionResponseConsumer>(context);
            //        });
            //        cfg.ReceiveEndpoint("gets_queue", e =>
            //        {
            //            e.ConfigureConsumer<GetInspectionsResponseConsumer>(context);
            //        });
            //    });

            //});
        }
    }
}
