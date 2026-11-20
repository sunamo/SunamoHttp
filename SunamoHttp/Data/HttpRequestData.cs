namespace SunamoHttp.Data;

/// <summary>
/// Represents the http request data type.
/// </summary>
public class HttpRequestData
{
    /// <summary>
    /// Gets or sets the accept.
    /// </summary>
    public string? Accept { get; set; } = null;

    // Assign: StreamContent, ByteArrayContent, FormUrlEncodedContent, StringContent, MultipartContent, MultipartFormDataContent
    /// <summary>
    /// Gets or sets the content.
    /// </summary>
    public HttpContent? Content { get; set; } = null;

    /// <summary>
    /// Gets or sets the content type.
    /// </summary>
    public string? ContentType { get; set; } = null;

    /// <summary>
    /// Gets or sets the encoding post data.
    /// </summary>
    public Encoding? EncodingPostData { get; set; }

    /// <summary>
    /// Gets or sets the forced encoding.
    /// </summary>
    public Encoding? ForcedEncoding { get; set; } = null;

    /// <summary>
    /// Gets or sets the force encoding.
    /// </summary>
    public bool? ForceEncoding { get; set; } = false;

    /// <summary>
    /// Gets or sets the headers.
    /// </summary>
    public Dictionary<string, string> Headers { get; set; } = new();

    /// <summary>
    /// Gets or sets the keep alive.
    /// </summary>
    public bool? KeepAlive { get; set; } = null;

    /// <summary>
    /// Gets or sets the throw ex.
    /// </summary>
    public bool ThrowEx { get; set; } = true;

    /// <summary>
    /// Gets or sets the timeout in s.
    /// </summary>
    public int TimeoutInS { get; set; } = 60;
}
