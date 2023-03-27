using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;

namespace UploadTest;

/// <summary>
/// Helper para rotinas de upload de arquivo
/// </summary>
public static class MultipartRequestHelper
{
    /// <remarks>
    /// Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
    /// The spec at https://tools.ietf.org/html/rfc2046#section-5.1 states that 70 characters is a reasonable limit.
    /// </remarks>
    private static string GetBoundary(MediaTypeHeaderValue contentType)
    {
        var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

        if (string.IsNullOrWhiteSpace(boundary))
            throw new ValidationException("Missing content-type boundary.");

        return boundary;
    }

    private static bool IsMultipartContentType(string? contentType)
    {
        return !string.IsNullOrEmpty(contentType)
               && contentType.Contains("multipart/", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Obtém a stream referente ao primeiro arquivo do upload
    /// </summary>
    public static async Task<Stream> GetUploadedFileAsync(HttpRequest request)
    {
        if (!IsMultipartContentType(request.ContentType))
            throw new ValidationException("A requisição não é multipart.");

        var boundary = GetBoundary(MediaTypeHeaderValue.Parse(request.ContentType));
        var reader = new MultipartReader(boundary, request.Body);

        // note: this is for a single file, you could also process multiple files
        var section = await reader.ReadNextSectionAsync();

        if (section == null)
            throw new ValidationException("Nenhuma seção de multipart definida.");

        return section.Body;
    }
}
