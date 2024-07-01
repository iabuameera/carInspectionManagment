﻿using CarInspectionManagment.Contract.Filters;
using CarInspectionManagment.Contract.Inspection.Models;
using CarInspectionManagment.Contract.Inspection.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarInspectionManagment.Business.Managers
{
    public class GetInspections
    {
        public InspectionListFilter Filters { get; set; }
    }

    public class GetInspectionsResponse
    {
        public List<InspectionResource> Inspections { get; set; }
    }

    public class CreateInspection
    {
        public InspectionModel Model { get; set; }
    }

    public class CreateInspectionResponse
    {
        public InspectionResource Inspection { get; set; }
    }

}