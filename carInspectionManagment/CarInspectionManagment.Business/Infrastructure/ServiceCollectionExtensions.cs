using CarInspectionManagment.Business.Managers;
using CarInspectionManagment.Business.Managers.ViewCache;
using CarInspectionManagment.Business.Persistence;
using CarInspectionManagment.Business.Persistence.Entities;
using CarInspectionManagment.Business.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CarInspectionManagment.Business.Infrastructure
{
    

    public static class ServiceCollectionExtensions
    {
        public static void AddDependencyInjection(this IServiceCollection services, string sqlConnectionString)
        {

           // services.AddDbContext<CarInspectionManagementContext>(options => options.UseSqlServer(connectionString));
          

            services.AddDbContext<CarInspectionManagementContext>(options => options.UseNpgsql(sqlConnectionString));

            //register Repository
            services.AddScoped<IInspectionRepository, InspectionRepository>();

            //Register Managers
            services.AddScoped<IInspectionManager, InspectionManager>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddSingleton<ICarInspectionViewCacheManager, CarInspectionViewCacheManager>();

            string redisConectionString = "localhost:6379,password=GHStn8E4AGw+nONOBCRHZ+jpJJvOyTHcOFc3OijM1qM=";
            services.AddSingleton<IConnectionMultiplexer>(
                  ConnectionMultiplexer.Connect(redisConectionString));
            services.AddStackExchangeRedisCache(options => options.Configuration = redisConectionString);

         


        }


    }
}
