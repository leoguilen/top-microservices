using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace TOP.ProfileService.Api.Configurations.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            _provider.ApiVersionDescriptions
                .ToList()
                .ForEach(x =>
                {
                    var info = new OpenApiInfo
                    {
                        Title = "Profile - API",
                        Version = x.ApiVersion.ToString()
                    };

                    options.SwaggerDoc(x.GroupName, info);
                });
        }
    }
}
