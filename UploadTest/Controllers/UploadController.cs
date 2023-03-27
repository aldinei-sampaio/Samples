using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UploadTest.Controllers;

[ApiController]
public class UploadController : Controller
{
    private const long MaxUploadedFileSize = 4L * 1024L * 1024L * 1024L; // 4GB

    private const string RootPath = @"D:\Dados";


    [RequestSizeLimit(MaxUploadedFileSize)]
    [RequestFormLimits(MultipartBodyLengthLimit = MaxUploadedFileSize)]
    [FileUploadOperation(FieldName = "file", Description = "O arquivo de dados a ser armazenado")]
    [DisableFormValueModelBinding]
    [HttpPost("/{id}")]
    public async Task<ActionResult> Upload([Required] Guid id)
    {
        var sourceStream = await MultipartRequestHelper.GetUploadedFileAsync(Request);
        var targetFilePath = Path.Combine(RootPath, id.ToString());
        using var stream = new FileStream(targetFilePath, FileMode.Create, FileAccess.Write);
        await sourceStream.CopyToAsync(stream);
        return Ok();
    }
}
