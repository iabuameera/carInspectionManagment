using CarInspectionManagment.Business.Persistence.Entities;
using CarInspectionManagment.Contract.Inspection.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarInspectionManagment.Business
{
    public static class EntityToResourceExtensions
    {

        public static InspectionResource ToResource(this CarInspection entity)
        {
            if (entity == null) return null;

            return new InspectionResource
            {
                Id = entity.Id,
                DateOfCreation=entity.DateOfCreation,
                Reason=entity.Reason,
                Vinnumber=entity.Vinnumber
               
            };
        }
        public static InspectionResource ToInspectionResources(this InspectionResource entity)
        {
            if (entity == null) return null;

            return new InspectionResource
            {
                Id = entity.Id,
                DateOfCreation = entity.DateOfCreation,
                Reason = entity.Reason,
                Vinnumber = entity.Vinnumber

            };
        }
    }
}
