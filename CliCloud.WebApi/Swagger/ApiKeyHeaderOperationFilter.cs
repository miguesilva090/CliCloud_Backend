using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CliCloud.WebApi.Swagger;

public sealed class ApiKeyHeaderOperationFilter : IOperationFilter
{
  public void Apply(OpenApiOperation operation, OperationFilterContext context)
  {
    operation.Parameters ??= new List<IOpenApiParameter>();

    if(operation.Parameters.Any(p => p.In == ParameterLocation.Header && p.Name == "X-API-Key"))
    return ;

    operation.Parameters.Add(new OpenApiParameter
    {
      Name = "X-API-Key",
      In = ParameterLocation.Header,
      Required = true, 
      Description = "API Key obrigatória",
      Schema = new OpenApiSchema { Type = JsonSchemaType.String}
    });
  }
}