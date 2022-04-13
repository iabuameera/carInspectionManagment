using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarInspectionManagment.Contract.Models
{
    public class AggregateGroup
    {
        public string AggregateGroupName { get; set; }
        public IEnumerable<GroupItem> Items { get; set; }
    }
}
