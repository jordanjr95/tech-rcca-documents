using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Files.Filters
{
    public class FormFileSwaggerFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                return;

            foreach (var parameter in operation.Parameters)
            {
                var parameterDescription = context.ApiDescription.ParameterDescriptions.FirstOrDefault(x => x.Name == parameter.Name);
                if (parameterDescription == null)
                    continue;

                var isFile = parameterDescription.ModelMetadata?.ModelType == typeof(IFormFile);
                if (!isFile)
                    continue;

                parameter.In = Microsoft.OpenApi.Models.ParameterLocation.Header;
                parameter.Schema = new OpenApiSchema
                {
                    Type = "object",
                    Properties =
                {
                    ["FileName"] = new OpenApiSchema { Type = "string", Description = "File Name" },
                    ["ContentType"] = new OpenApiSchema { Type = "string", Description = "File Content Type" },
                    ["ContentDisposition"] = new OpenApiSchema { Type = "string", Description = "File Content Disposition" },
                    ["Length"] = new OpenApiSchema { Type = "integer", Description = "File Length" },
                },
                    Required = new HashSet<string> { "FileName", "ContentType", "ContentDisposition", "Length" }
                };
            }
        }
    }

}
