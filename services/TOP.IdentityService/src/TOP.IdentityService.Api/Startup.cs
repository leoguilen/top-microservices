using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Net;
using TOP.IdentityService.Domain.Models;
using TOP.IdentityService.Infra.CrossCutting.IoC.DependencyInjection;
using TOP.IdentityService.WebApi.Configurations.Swagger;
using TOP.IdentityService.WebApi.Filters;

namespace TOP.IdentityService.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            JsonConvert.DefaultSettings = () => jsonSettings;

            services
                .AddIoc(Configuration)
                .AddHealthChecks().Services
                .AddControllers(o =>
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
                .AddNewtonsoftJson().Services
                .AddApiVersioning(o =>
                {
                    o.ReportApiVersions = true;
                    o.AssumeDefaultVersionWhenUnspecified = true;
                })
                .AddVersionedApiExplorer(o =>
                {
                    o.GroupNameFormat = "'v'VVV";
                    o.SubstituteApiVersionInUrl = true;
                })
                .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
                .AddSwaggerGen(o => { o.OperationFilter<SwaggerDefaultValues>(); })
                .AddSwaggerGenNewtonsoftSupport();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureSwagger(Configuration, provider)
                .UseHttpsRedirection()
                .UseRouting()
                .UseEndpoints(endpoint =>
                {
                    endpoint.MapControllers();
                    endpoint.MapHealthChecks("/healthcheck", new HealthCheckOptions
                    {
                        ResultStatusCodes =
                        {
                            [HealthStatus.Healthy] = StatusCodes.Status200OK,
                            [HealthStatus.Degraded] = StatusCodes.Status200OK,
                            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                        }
                    });
                });
        }
    }
}
