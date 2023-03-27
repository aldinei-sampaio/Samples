using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UploadTest;

/// <summary>
/// Filtro para desabilitar o binding automático do corpo da request
/// </summary>
/// <remarks>
/// Isto é recomendado no caso de endpoints de upload de arquivo, para
/// impedir que o asp.net efetue o download do arquivo inteiro antes
/// de executar o endpoint. Usando este atributo no método, o asp.net
/// não toca no corpo da requisição, da qual pode ser extraída a
/// stream para os dados do arquivo através do método
/// <see cref="MultipartRequestHelper.GetUploadedFileAsync(HttpRequest)"/>
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class DisableFormValueModelBindingAttribute : Attribute, IResourceFilter
{
    /// <summary>
    /// Remove da lista os providers que normalmente fariam o parsing
    /// do corpo da request
    /// </summary>
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        var factories = context.ValueProviderFactories;
        factories.RemoveType<FormValueProviderFactory>();
        factories.RemoveType<FormFileValueProviderFactory>();
        factories.RemoveType<JQueryFormValueProviderFactory>();
    }

    /// <summary>
    /// </summary>
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
    }
}
