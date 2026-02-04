namespace SunamoHttp.Data;

/// <summary>
/// Represents a file to be uploaded via HTTP
/// </summary>
public class UploadFile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UploadFile"/> class
    /// </summary>
    public UploadFile()
    {
        ContentType = "application/octet-stream";
    }

    /// <summary>
    /// Gets or sets the form field name for the uploaded file
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the filename of the uploaded file
    /// </summary>
    public string? Filename { get; set; }

    /// <summary>
    /// Gets or sets the content type of the uploaded file
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// Gets or sets the stream containing the file data
    /// </summary>
    public Stream? Stream { get; set; }
}