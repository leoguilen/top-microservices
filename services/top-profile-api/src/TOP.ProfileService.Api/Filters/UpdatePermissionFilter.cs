using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace TOP.ProfileService.Api.Filters
{
    public class UpdatePermissionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // not used
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var loggedUserId = GetUserIdByAccessToken(httpContext);
            var requestUserId = (string)httpContext.Request.RouteValues["userId"];

            if (loggedUserId != requestUserId)
            {
                context.Result = new ObjectResult("You don't have permission to execute this action");
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }

        private static string GetUserIdByAccessToken(HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }

            return httpContext.User.Claims.Single(x =>
                x.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}
