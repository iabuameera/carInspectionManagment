using CarInspectionManagment.Contract.Inspection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarInspectionManagment.Business.Infrastructure.Message
{
    public class CreateInspection
    {
        public InspectionModel Model { get; set; }
    }
}
