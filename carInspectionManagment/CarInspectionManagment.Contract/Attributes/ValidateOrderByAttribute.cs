using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CarInspectionManagment.Contract.Attributes
{
    public class ValidateOrderByAttribute : ValidationAttribute
    {
        private readonly Type _type;
        private readonly OrderByAttributeType _orderByType;

        public ValidateOrderByAttribute(Type type, OrderByAttributeType orderByType = OrderByAttributeType.SqlServer)
        {
            _type = type;
            _orderByType = orderByType;
        }


        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            //split the orderby string to get the column names to orderby
            var columnsList = value.ToString()
                .Split(new[]
                {
                    "+",
                    "-",
                    ",+",
                    ",-",
                    ","
                }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            var invalidColumns = new StringBuilder();

            //check for each column if it is a valid column in the resource type
            
            CheckInvalidColumnsForSql(columnsList, invalidColumns);


            //set the error message to show the invalid columns and valid columns
            if (invalidColumns.Length > 0)
            {
                string orderByColumns;

                orderByColumns = string.Join(", ", _type.GetProperties()
                        .Where(p => IsDefined(p, typeof(IncludeInOrderByAttribute)))
                        .Select(p => p.Name));
                if (orderByColumns.Length == 0)
                {
                    ErrorMessage = "There is no specific column for order by .";
                }
                else if (orderByColumns.Length > 0)
                {
                    ErrorMessage = $"{invalidColumns}: is/are invalid column(s) to order by. Please use one of these properties: {orderByColumns}";
                }
                return false;
            }

            return true;
        }

        private void CheckInvalidColumnsForSql(System.Collections.Generic.List<string> columnsList, StringBuilder invalidColumns)
        {
            foreach (var columnName in columnsList)
            {
                if (!_type.GetProperties()
                    .Any(p => string.Equals(p.Name, columnName, StringComparison.CurrentCultureIgnoreCase)
                              && IsDefined(p, typeof(IncludeInOrderByAttribute))))
                {
                    if (invalidColumns.Length > 0)
                    {
                        invalidColumns.Append(", ");
                    }
                    invalidColumns.Append(columnName.Trim());
                }
            }
        }

   
        protected static string FirstCharacterToLower(string str)
        {
            if (string.IsNullOrEmpty(str) || char.IsLower(str, 0))
            {
                return str;
            }
            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }

        protected string GetProperOrderByFieldName(string fieldName)
        {
            var correctFieldName = _type.GetProperties()
                        .FirstOrDefault(p => string.Equals(p.Name, fieldName.ToLower(), StringComparison.CurrentCultureIgnoreCase));
            if (correctFieldName != null)
            {
                return FirstCharacterToLower(correctFieldName.Name);
            }
            return fieldName;
        }
    }

    public enum OrderByAttributeType
    {
       
        SqlServer
    }
}
