using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using TOP.IdentityService.Domain.Constants;
using TOP.IdentityService.Domain.Interfaces.Domain.Holders;
using TOP.IdentityService.Domain.Interfaces.Infra;

namespace TOP.IdentityService.WebApi.Filters
{
    public class ContextFilter : IActionFilter
    {
        private readonly IRequestContextHolder _requestContextHolder;
        private readonly ILogWriter _logWriter;

        public ContextFilter(IRequestContextHolder requestContextHolder, ILogWriter logWriter)
        {
            _requestContextHolder = requestContextHolder;
            _logWriter = logWriter;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // not used
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var headers = context.HttpContext.Request.Headers;
            var correlationId = headers[HttpHeaders.CorrelationId];
            var parsed = Guid.TryParse(correlationId, out var guid) ? guid : Guid.NewGuid();

            _logWriter.CorrelationId = parsed;
            _requestContextHolder.CorrelationId = parsed;
            _requestContextHolder.Object = ReadBody(context);
        }

        private static object ReadBody(ActionExecutingContext context)
        {
            var parameters = context.ActionDescriptor.Parameters;
            var bodyParameter = ResolveBodyParameterName(parameters);

            return context.ActionArguments.TryGetValue(bodyParameter, out var body) ? body : default;
        }

        private static string ResolveBodyParameterName(IList<ParameterDescriptor> parameters)
        {
            return parameters.FirstOrDefault(p => 
            {
                var type = p.ParameterType;
                return type.Name.EndsWith("Command");
            })?.Name;
        }
    }
}
