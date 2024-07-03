using CarInspectionManagment.Contract.Inspection.Resource;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarInspectionManagment.Business.Managers.ViewCache
{
    public interface ICarInspectionViewCacheManager
    {
        Task DeleteCarInspectAsync(InspectionResource resource);
        Task<List<InspectionResource>> GetInspectionsAsync();
        Task SetCarInspectAsync(InspectionResource resource);
    }

    public class CarInspectionViewCacheManager : ICarInspectionViewCacheManager
    {

        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _multiplexer;

        public CarInspectionViewCacheManager(IDistributedCache distributedCache, IConnectionMultiplexer multiplexer)
        {
            _distributedCache = distributedCache;
            _multiplexer = multiplexer;


            _multiplexer.GetDatabase();

        }

        public async Task<List<InspectionResource>> GetInspectionsAsync()
        {
            var result = new List<InspectionResource>();
            var endPoint = _multiplexer.GetEndPoints().First();
            RedisKey[] keys = _multiplexer.GetServer(endPoint).Keys(pattern: "*").ToArray();

            if (keys != null && keys.Length != 0)
            {
                foreach (var k in keys)
                {
                    var carInspectionRedis = await _distributedCache.GetStringAsync(k);

                    result.Add(JsonConvert.DeserializeObject<InspectionResource>(carInspectionRedis));

                }
            }

            return result;
        }


        public async Task SetCarInspectAsync(InspectionResource resource)
        {
            string carInspect = JsonConvert.SerializeObject(resource);
            var key = $"Inspection_key_{resource.Vinnumber}_{resource.DateOfCreation}_{resource.Reason}";
            await _distributedCache.SetStringAsync(key, carInspect);

        }

        public async Task DeleteCarInspectAsync(InspectionResource resource)
        {
            await _distributedCache.RemoveAsync($"Insinspection_{resource.Id}");
        }

    }
}
