using CarInspectionManagment.Contract.Inspection.Resource;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using CarInspectionManagment.Contract.Filters;
using CarInspectionManagment.Contract.Models;
using CarInspectionManagment.Business.Managers;
using Lib.AspNetCore.ServerTiming;
using Lib.AspNetCore.ServerTiming.Http.Headers;
using CarInspectionManagment.Contract.Inspection.Models;

namespace CarInspectionManagment.Api.Controllers
{
    // [Route("api/[controller]")]
    [ApiController]
    public class CarInspectionController : BaseController
    {
        private readonly IInspectionManager _inspectionManager;
        private readonly IServerTiming _serverTiming;
        public CarInspectionController(IInspectionManager inspectionManager, IServerTiming serverTiming)
        {
            _inspectionManager = inspectionManager;
            _serverTiming = serverTiming;
        }
        [HttpGet]
        [Route("GetAllInspections", Name = "GetAllInspections")]
        [ProducesResponseType(typeof(ResourceCollection<InspectionResource>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllInspections([FromQuery]InspectionListFilter filter)
        {
            var chronometer = new Stopwatch();
            chronometer.Start();
             var resources = await _inspectionManager.GetInspectionsAsync(filter);
          
            chronometer.Stop();
            _serverTiming.Metrics.Add(new ServerTimingMetric("action", chronometer.ElapsedMilliseconds, "Action execution"));

            var result = new ResourceCollection<InspectionResource>(resources, resources.Count);
            return Ok(result);
        }

        /// <summary>
        /// Get a Car Inspection by id.
        /// </summary>
        /// <param name="id">Inspection id.</param>
        /// <remarks>Get a Car Inspection by id.</remarks>
        /// <respone code="404">Inspection is not found.</respone>
        /// <response code="500">Server error.</response>
        /// <returns></returns>
        [HttpGet]
        [Route("GetInspectionById/{id:min(1)}", Name = "GetInspectionById")]
        [ProducesResponseType(typeof(InspectionResource), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetInspectionById(int id)
        {
            var resource = await _inspectionManager.GetInspectionByIdAsync(id);
            return Ok(resource);
        }


        /// <summary>
        /// Create a Car Inspection.
        /// </summary>
        /// <remarks>Create a Inspection.</remarks>
        /// <param name="model">Inspection model.</param>
        /// <response code="400">The model is not valid.</response>
        /// <response code="500">Server error.</response>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateInspection", Name = "CreateInspection")]
        [ProducesResponseType(typeof(InspectionResource), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateInspection([FromBody] InspectionModel model)
        {
            // Create it
            var resource = await _inspectionManager.CreateInspectionAsync(model);

            // Return
            return Created
            (
                Url.Link
                ("GetInspectionById",
                    new
                    {
                        id = resource.Id
                    }),
                resource);
        }



        /// <summary>
        /// Update an existing Car Inspection.
        /// </summary>
        /// <remarks>Update an existing Car Inspection.</remarks>
        /// <param name="id">The Inspection id.</param>
        /// <param name="model">The Inspection model.</param>
        /// <response code="400">The model is not valid.</response>
        /// <response code="404">Car Inspection is not found.</response>
        /// <response code="500">Server error.</response>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateInspection/{id:min(1)}", Name = "UpdateInspection")]
        [ProducesResponseType(typeof(InspectionResource), (int)HttpStatusCode.Created)]
      
        public async Task<IActionResult> UpdateInspection(int id, [FromBody] InspectionModel model)
        {
            // Update it
            var entity = await _inspectionManager.UpdateInspectionAsync(id, model);

            // Return
            return Ok(entity);
        }



        /// <summary>
        /// Delete a Car Inspection.
        /// </summary>
        /// <remarks>Delete a Car Inspection.</remarks>
        /// <param name="id">Inspection id.</param>
        /// <response code="404">Not found.</response>
        /// <response code="500">Server error.</response>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteInspection/{id:min(1)}", Name = "DeleteInspection")]
        public async Task<IActionResult> DeleteInspection(int id)
        {
            await _inspectionManager.DeleteInspectionAsync(id);

            // Return
            return NoContent();
        }
    }
}
