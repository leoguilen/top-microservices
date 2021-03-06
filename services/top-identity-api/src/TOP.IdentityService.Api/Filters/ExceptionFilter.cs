using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;
using TOP.IdentityService.Domain.Exceptions;
using TOP.IdentityService.Domain.Interfaces.Domain.Holders;
using TOP.IdentityService.Domain.Interfaces.Infra;
using TOP.IdentityService.Domain.Models;

namespace TOP.IdentityService.WebApi.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly IRequestContextHolder _requestContextHolder;
        private readonly ILogWriter _logWriter;

        public ExceptionFilter(IRequestContextHolder requestContextHolder, ILogWriter logWriter)
        {
            _requestContextHolder = requestContextHolder;
            _logWriter = logWriter;
        }

        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            
            Error res;

            switch (ex)
            {
                case FluentValidation.ValidationException validationEx:
                    {
                        res = new Error
                        {
                            Errors = validationEx.Errors.Select(err => 
                                new InnerError 
                                { 
                                    Title = "validation failed", 
                                    Detail = err.ErrorMessage, 
                                    Status = ((int)HttpStatusCode.BadRequest).ToString()
                                })
                        };
                    };
                    break;
                default:
                    {
                        res = new Error
                        {
                            Errors = new InnerError[]
                            {
                                new InnerError
                                {
                                    Title = ex.GetType().Name,
                                    Detail = ex.Message,
                                    Status = ((int)HttpStatusCode.InternalServerError).ToString()
                                }
                            }
                        };
                    }
                    break;
            }

            LogException(ex, res);

            context.ExceptionHandled = true;
            context.Result = new ObjectResult(res);
            context.HttpContext.Response.StatusCode = res.StatusCode;
        }

        private void LogException(Exception ex, Error error)
        {
            var message = error?.Errors?.FirstOrDefault()?.Detail ?? ex.Message;
            var data = _requestContextHolder.Object;

            _logWriter.Error(message, data, ex, ex.TargetSite?.Name);
        }
    }
}
