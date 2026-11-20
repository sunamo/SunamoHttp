namespace SunamoHttp.Data;

/// <summary>
/// Represents the upload file type.
/// </summary>
public class UploadFile
{
    /// <summary>
    /// Initializes a new instance of the upload file class.
    /// </summary>
    public UploadFile()
    {
        ContentType = "application/octet-stream";
    }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the filename.
    /// </summary>
    public string? Filename { get; set; }

    /// <summary>
    /// Gets or sets the content type.
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// Gets or sets the stream.
    /// </summary>
    public Stream? Stream { get; set; }
}
