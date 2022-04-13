using CarInspectionManagment.Business.Infrastructure.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;


namespace CarInspectionManagment.Business.Infrastructure
{
    public static class ModelExtensions
    {
        public static Dictionary<string, string> ValidateModel(
            this object model)
        {
            var context = model != null ? new ValidationContext(model) : null;
            var errors = GetModelErrors(model, context);

            var validatableObject = model as IValidatableObject;
            var result = validatableObject?.Validate(context).ToArray();

            if (result == null || !result.Any())
                return errors;

            foreach (var validationResult in result)
            {
                if (validationResult == null) continue;
                foreach (var memberName in validationResult.MemberNames)
                {
                    errors.Add(memberName, validationResult.ErrorMessage);
                }
            }

            return errors;
        }

        public static void EnsureValidModel(this object model)
        {
            var errors = model.ValidateModel();
            if (errors.Count > 0)
            {

                ExceptionManager.ThrowInvalidModelException(errors);
            }
        }

        private static Dictionary<string, string> GetModelErrors(object model, ValidationContext context)
        {
            var errors = new Dictionary<string, string>();
            if (model == null)
            {
                errors.Add("model", "Model is null.");
                return errors;
            }

            foreach (var property in model.GetType().GetProperties())
                foreach (var attribute in property.GetCustomAttributes(true))
                    ValidateAttribute(model, context, attribute, property, errors);

            return errors;
        }

        private static void ValidateAttribute(
            object model,
            ValidationContext context,
            object attribute,
            PropertyInfo property,
            Dictionary<string, string> errors)
        {
            if (attribute is ValidationAttribute validationAttribute)
            {
                if (attribute is IExtendedValidation)
                    ExtendedValidation(model, context, validationAttribute, property, errors);
                else if (!validationAttribute.IsValid(property.GetValue(model)))
                    errors.Add(property.Name, validationAttribute.FormatErrorMessage(property.Name));
            }
            else
            {
                if (!(attribute is Newtonsoft.Json.JsonIgnoreAttribute) && !(attribute is Newtonsoft.Json.JsonPropertyAttribute)
                    && !(attribute is DefaultValueAttribute) && !(attribute is TimestampAttribute))
                    throw new NotImplementedException($"Attribute {attribute} is not of type ValidationAttribute.");
            }
        }

        private static void ExtendedValidation(
            object model,
            ValidationContext context,
            ValidationAttribute validationAttribute,
            PropertyInfo property,
            Dictionary<string, string> errors)
        {
            var result = validationAttribute.GetValidationResult(property.GetValue(model), context);

            if (result == ValidationResult.Success)
                return;

            if (result != null)
                errors.Add(property.Name, result.ErrorMessage);
        }
    }

}
