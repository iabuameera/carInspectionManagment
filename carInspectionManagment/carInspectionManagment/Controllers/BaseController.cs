using CarInspectionManagment.Business.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CarInspectionManagment.Api.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    public abstract class BaseController : Controller
    {
    

        protected BaseController()
        {
        
        }

        public override Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            TrimTrailingSpacesOfStringFields(context);

            if (ModelState.IsValid) return base.OnActionExecutionAsync(context, next);

            RemoveDuplicatedErrorMessages(ModelState);
            context.Result = BadRequest(ModelState);

            return base.OnActionExecutionAsync(context, next);
        }

        private static void RemoveDuplicatedErrorMessages(ModelStateDictionary modelStateDictionary)
        {
            foreach (var modelState in modelStateDictionary)
            {
                var knownValues = new HashSet<string>();
                for (var i = 0; i < modelState.Value.Errors.Count; i++)
                    //Check if we have seen the error message before by trying to add it to the HashSet
                    if (!knownValues.Add(modelState.Value.Errors[i].ErrorMessage))
                        modelStateDictionary[modelState.Key].Errors.RemoveAt(i);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //   context.HttpContext.Response.Headers.Add("Cache-Control", "max-age=0");
            if (context.Exception != null)
            {
                if (context.Exception.GetType() == typeof(InvalidOperationException))
                {


                    var errors = ((ValidateModelException)context.Exception).Errors;
                    context.Result = BadRequest(errors);
                }
               
                else if (context.Exception.GetType() == typeof(ConflictException))
                {
                    context.Result = Conflict(((ConflictException)context.Exception).Errors);
                }
                else if (context.Exception.GetType() == typeof(ItemNotFoundException))
                {
                    context.Result = NotFound(((ItemNotFoundException)context.Exception).Errors);
                }

                else if (context.Exception.GetType() == typeof(InvalidModelException))
                {
                    context.Result = BadRequest(((InvalidModelException)context.Exception).Errors);
                }
                else
                {
                    context.Result = Content(HttpStatusCode.InternalServerError.ToString(), context.Exception?.InnerException?.Message ?? context.Exception?.Message);
                }

                context.Exception = null;


            }
            else
            {
                context.HttpContext.Response.Headers.Add("Cache-Control", "max-age=0");
            }
        }

        private static void TrimTrailingSpacesOfStringFields(ActionExecutingContext context)
        {
            if (!context.ActionArguments.TryGetValue("model", out var value)) return;

            var stringProperties = value.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string));

            foreach (var stringProperty in stringProperties)
            {
                var currentValue = (string)stringProperty.GetValue(value, null);
                if (!string.IsNullOrEmpty(currentValue))
                    stringProperty.SetValue(value, currentValue.Trim(), null);
            }
        }
    }
}
