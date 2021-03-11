using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using TOP.ProfileService.Api.Configurations.Extensions;
using TOP.ProfileService.Api.Configurations.Swagger;
using TOP.ProfileService.Infra.CrossCutting.IoC.DependencyInjection;

namespace TOP.ProfileService.Api
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
            services
                .AddIoc(Configuration)
                .AddControllersWithJsonOptions()
                .AddResponseCompression()
                .AddJwt(Configuration)
                .AddRouting()
                .AddHealthChecks().Services
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
                .AddSwaggerGen(o => 
                { 
                    o.OperationFilter<SwaggerDefaultValues>();
                    o.ApplySecurityDefinition();
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureSwagger(Configuration, provider)
                .UseApiVersioning()
                .UseHttpsRedirection()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
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
