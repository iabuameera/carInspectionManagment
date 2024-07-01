using Autofac;
using Autofac.Extensions.DependencyInjection;
using CarInspectionManagment.Business.Infrastructure;
using CarInspectionManagment.Business.Managers;
using CarInspectionManagment.Business.Persistence.Repositories;
using ConncterLayer;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace carInspectionManagment
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public static IServiceProvider ServiceProvider;
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "carInspectionManagment", Version = "v1" });
            });
            services.AddServerTiming();
            services.AddDependencyInjection("User ID =cars;Password=1234;Server=localhost;Port=5432;Database=CarInspectionManagement ;Integrated Security=true;Pooling=true;");

            services.AddMassTransit(config =>
            {
                config.AddConsumer<GetInspectionsResponseConsumer>();
                config.AddConsumer<CreateInspectionResponseConsumer>();

                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri("rabbitmq://localhost:5672"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    //cfg.ReceiveEndpoint("creates_queue", e =>
                    //{
                    //    e.ConfigureConsumer<CreateInspectionResponseConsumer>(context);
                    //});
                    //cfg.ReceiveEndpoint("gets_queue", e =>
                    //{
                    //    e.ConfigureConsumer<GetInspectionsResponseConsumer>(context);
                    //});
                });

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "carInspectionManagment v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

 

  
}
