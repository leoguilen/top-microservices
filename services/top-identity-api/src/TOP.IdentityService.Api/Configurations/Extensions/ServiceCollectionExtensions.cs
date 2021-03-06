using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Text.Json;
using TOP.IdentityService.Domain.Models;
using TOP.IdentityService.WebApi.Filters;

namespace TOP.IdentityService.Api.Configurations.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddControllersWithJsonOptions(this IServiceCollection services)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            JsonConvert.DefaultSettings = () => jsonSettings;

            services.AddControllers(o =>
                {
                    o.Filters.Add<ContextFilter>();
                    o.Filters.Add<ExceptionFilter>();
                })
                .ConfigureApiBehaviorOptions(o => o.InvalidModelStateResponseFactory = ctx =>
                {
                    var errors = ctx.ModelState.Keys
                        .Select(x => new
                        {
                            Key = x,
                            Values = ctx.ModelState[x],
                            Error = ctx.ModelState[x]?.Errors == null ? null : string.Join("|", ctx.ModelState[x]?.Errors.Select(e => e.ErrorMessage))
                        })
                        .Select(x => new InnerError
                        {
                            Title = $"validation error on field {x.Key}".Replace(" Data.", " "),
                            Detail = string.IsNullOrWhiteSpace(x.Error) ? "Must be a valid value" : x.Error,
                            Status = ((int)HttpStatusCode.BadRequest).ToString()
                        });

                    return new BadRequestObjectResult(new Error { Errors = errors });
                })
                .AddJsonSerializerOptions()
                .AddNewtonsoftJson(jsonOpt => 
                {
                    jsonOpt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    jsonOpt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            return services;
        }

        private static IMvcBuilder AddJsonSerializerOptions(this IMvcBuilder mvcBuilder)
            => mvcBuilder.AddJsonOptions(opt => 
            {
                opt.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                opt.JsonSerializerOptions.IgnoreNullValues = true;
            });
    }
}
