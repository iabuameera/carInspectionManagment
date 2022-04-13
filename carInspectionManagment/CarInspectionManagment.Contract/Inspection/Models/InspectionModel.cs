using System;
using System.ComponentModel.DataAnnotations;

namespace CarInspectionManagment.Contract.Inspection.Models
{
    public class InspectionModel
    {
        [Required]
        public DateTime DateOfCreation { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Vinnumber { get; set; }
        [Required]
        public string Reason { get; set; }
    }
}
