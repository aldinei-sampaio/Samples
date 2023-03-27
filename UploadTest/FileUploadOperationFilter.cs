using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace UploadTest;

/// <summary>
/// Filtro para o Swagger exibir upload para APIs sem usar IFormFile
/// </summary>
public sealed class FileUploadOperationFilter : IOperationFilter
{
    /// <summary>
    /// Aplica o filtro
    /// </summary>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileUploadAttribute = context.MethodInfo.GetCustomAttribute(typeof(FileUploadOperationAttribute), true) as FileUploadOperationAttribute;
        if (fileUploadAttribute != null)
        {
            var requestBody = operation.RequestBody;
            if (fileUploadAttribute.ClearOtherParameters)
            {
                operation.Parameters.Clear();
                requestBody = new OpenApiRequestBody();
            }
            requestBody ??= new OpenApiRequestBody();
            if (!requestBody.Content.TryGetValue("multipart/form-data", out var uploadFileMediaType))
            {
                requestBody.Content["multipart/form-data"] = uploadFileMediaType = new OpenApiMediaType();
            }
            uploadFileMediaType.Schema ??= new OpenApiSchema();
            uploadFileMediaType.Schema.Type = "object";
            if (uploadFileMediaType.Schema.Properties == null)
            {
                uploadFileMediaType.Schema.Properties = new Dictionary<string, OpenApiSchema>(StringComparer.OrdinalIgnoreCase);
            }
            uploadFileMediaType.Schema.Properties[fileUploadAttribute.FieldName] = new OpenApiSchema()
            {
                Description = fileUploadAttribute.Description,
                Type = "string",
                Format = "binary"
            };
            if (uploadFileMediaType.Schema.Required == null)
            {
                uploadFileMediaType.Schema.Required = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }
            uploadFileMediaType.Schema.Required.Add(fileUploadAttribute.FieldName);

            operation.RequestBody = requestBody;
        }
    }
}
