using System;
using System.Collections.Generic;

#nullable disable

namespace CarInspectionManagment.Business.Persistence.Entities
{
    public partial class CarInspection
    {
        public int Id { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string Vinnumber { get; set; }
        public string Reason { get; set; }
    }
}
