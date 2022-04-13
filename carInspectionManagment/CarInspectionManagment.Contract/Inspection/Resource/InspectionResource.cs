using CarInspectionManagment.Contract.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarInspectionManagment.Contract.Inspection.Resource
{
   public class InspectionResource
    {
        [IncludeInOrderBy]
        public int Id { get; set; }
        [IncludeInOrderBy]
        public DateTime DateOfCreation { get; set; }
        [IncludeInOrderBy]
        public string Vinnumber { get; set; }
        [IncludeInOrderBy]
        public string Reason { get; set; }
    }
}
