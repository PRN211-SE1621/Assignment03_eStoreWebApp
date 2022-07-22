using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

using BusinessObject;

namespace eStore.Filters
{
    public class AdminOnlyFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string jsonUser = context.HttpContext.Session.GetString("User");
            string role = context.HttpContext.Session.GetString("Role");
            if(jsonUser == null || role == null || role != "ADMIN")
            {
                context.Result = new NotFoundResult();
            }    
        }
    }
}
