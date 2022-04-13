using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarInspectionManagment.Business.Persistence
{
    public interface ICorrelateBy<TIdentifier>
    {
        TIdentifier Id { get; set; }
    }
}
