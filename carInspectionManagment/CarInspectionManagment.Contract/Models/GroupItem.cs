using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarInspectionManagment.Contract.Models
{
    public class GroupItem
    {
        public long? Total { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public string FilterValue { get; set; }
    }
}
