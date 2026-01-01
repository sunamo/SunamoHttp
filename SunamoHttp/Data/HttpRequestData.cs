namespace SunamoHttp.Data;

/// <summary>
/// HTTP request configuration data
/// </summary>
public class HttpRequestData
{
    /// <summary>
    /// Gets or sets the Accept header value
    /// </summary>
    public string Accept { get; set; } = null;

    /// <summary>
    /// Gets or sets the HTTP content for the request
    /// Assign: StreamContent, ByteArrayContent, FormUrlEncodedContent, StringContent, MultipartContent, MultipartFormDataContent
    /// </summary>
    public HttpContent Content { get; set; } = null;

    /// <summary>
    /// Gets or sets the Content-Type header value
    /// </summary>
    public string ContentType { get; set; } = null;

    /// <summary>
    /// Gets or sets the encoding for POST data
    /// </summary>
    public Encoding EncodingPostData { get; set; }

    /// <summary>
    /// Gets or sets the forced encoding for response
    /// </summary>
    public Encoding ForcedEncoding { get; set; } = null;

    /// <summary>
    /// Gets or sets a value indicating whether to force specific encoding
    /// </summary>
    public bool? ForceEncoding { get; set; } = false;

    /// <summary>
    /// Gets or sets the custom HTTP headers
    /// </summary>
    public Dictionary<string, string> Headers { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether to keep the connection alive
    /// </summary>
    public bool? KeepAlive { get; set; } = null;

    /// <summary>
    /// Gets or sets a value indicating whether to throw exceptions on errors
    /// </summary>
    public bool ThrowEx { get; set; } = true;

    /// <summary>
    /// Gets or sets the timeout in seconds
    /// </summary>
    public int TimeoutInS { get; set; } = 60;
}