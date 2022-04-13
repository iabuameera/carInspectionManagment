using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarInspectionManagment.Contract
{
    public static class ValidationMessages
    {
        public static string GetNotFoundMessage(string domainName, string id)
        {
            return $"{domainName} with Id: {id} is not found.";
        }

        public static string GetAlreadyExistsMessage(string fieldName, string value)
        {
            return $"{fieldName} {value} already exists.";
        }

        public static string GetNotEmptyMessage(string fieldName)
        {
            return $"{fieldName} can not be empty.";
        }

        public static string GetAlreadyDeletedMessage(string domainName, string id)
        {
            return $"{domainName} with Id: {id} is already deleted.";
        }
    }
}
