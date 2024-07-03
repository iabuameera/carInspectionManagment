using CarInspectionManagment.Contract.Inspection.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarInspectionManagment.Business.Infrastructure.Message
{
    public class CreateInspectionResponse
    {
        public InspectionResource Inspection { get; set; }
    }
}
