using CarInspectionManagment.Contract.Inspection.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnecterLayer
{
    public interface IInspectionResponseService
    {
        void SetInspectionResponse(List<InspectionResource> response);
        List<InspectionResource> GetInspectionResponse();
        void SetCreateInspectionResponse(InspectionResource response);
        InspectionResource GetCreateInspectionResponse();
    }

    public class InspectionResponseService : IInspectionResponseService
    {
        private InspectionResource _createResponse;
        private List<InspectionResource> _getResponse;

        public void SetCreateInspectionResponse(InspectionResource response)
        {
            _createResponse = response;
        }

        public InspectionResource GetCreateInspectionResponse()
        {
            return _createResponse;
        }

        public void SetInspectionResponse(List<InspectionResource> response)
        {
            _getResponse = response;
        }

        public List<InspectionResource> GetInspectionResponse()
        {
            return _getResponse;
        }
    }

}
