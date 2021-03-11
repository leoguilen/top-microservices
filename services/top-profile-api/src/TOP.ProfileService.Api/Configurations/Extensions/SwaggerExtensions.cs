using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace TOP.ProfileService.Api.Configurations.Extensions
{
    public static class SwaggerExtensions
    {
        public static void ApplySecurityDefinition(this SwaggerGenOptions genOptions)
        {
            var security = new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            };

            genOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
            genOptions.AddSecurityRequirement(security);
        }

        public static IApplicationBuilder ConfigureSwagger(
            this IApplicationBuilder app,
            IConfiguration configuration,
            IApiVersionDescriptionProvider provider)
        {
            var useSwagger = configuration.GetValue<bool>("UseSwagger");

            if (useSwagger is false)
            {
                return app;
            }

            return app
                .UseSwagger()
                .UseSwaggerUI(o => provider.ApiVersionDescriptions
                    .ToList()
                    .ForEach(d =>
                        o.SwaggerEndpoint($"/swagger/{d.GroupName}/swagger.json", d.GroupName.ToString())));
        }
    }
}
