using CarInspectionManagment.Contract.Attributes;
using CarInspectionManagment.Contract.Inspection.Resource;
using CarInspectionManagment.Contract.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace CarInspectionManagment.Contract.Filters
{
    public  class InspectionListFilter: ListFilter
    {
        public DateTime? DateOfCreation { get; set; }
        public string Vinnumber { get; set; }
        public string Reason { get; set; }

        /// <summary>
        /// Order By: Reason, Vinnumber
        /// </summary>
        [ValidateOrderBy(typeof(InspectionResource), OrderByAttributeType.SqlServer)]
     
        public override string OrderBy { get; set; }
    }
}
