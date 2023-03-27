namespace UploadTest;

/// <summary>
/// Define que o Swagger deve mostrar uma caixa para permitir upload de arquivo para um método de API
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class FileUploadOperationAttribute : Attribute
{
    /// <summary>
    /// Outros parâmetros devem ser removidos?
    /// </summary>
    public bool ClearOtherParameters { get; set; }

    /// <summary>
    /// Nome do campo
    /// </summary>
    public string FieldName { get; set; }

    /// <summary>
    /// Descrição do campo
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Construtor principal
    /// </summary>
    public FileUploadOperationAttribute()
    {
        FieldName = "uploadedFile";
        Description = "Upload File";
    }
}
