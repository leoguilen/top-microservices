using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace TOP.IdentityService.WebApi.Configurations.Swagger
{
    public static class SwaggerExtensions
    {
        public static IApplicationBuilder ConfigureSwagger(
            this IApplicationBuilder app,
            IConfiguration configuration,
            IApiVersionDescriptionProvider provider)
        {
            var useSwagger = configuration.GetValue<bool>("UseSwagger");

            if(useSwagger is false)
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
