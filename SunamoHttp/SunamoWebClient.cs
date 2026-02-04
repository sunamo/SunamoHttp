namespace SunamoHttp;

/// <summary>
/// Custom WebClient with extended functionality for HTTP requests
/// </summary>
public class SunamoWebClient : WebClient
{
    /// <summary>
    /// Gets or sets the HTTP request data configuration
    /// </summary>
    public HttpRequestData? HttpRequestData { get; set; } = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="SunamoWebClient"/> class
    /// </summary>
    public SunamoWebClient()
    {
        base.Encoding = Encoding.UTF8;
    }

    /// <summary>
    /// Returns a <see cref="WebRequest"/> object for the specified resource
    /// </summary>
    /// <param name="uri">The URI to request</param>
    /// <returns>A new <see cref="WebRequest"/> object for the specified resource</returns>
    protected override WebRequest GetWebRequest(Uri uri)
    {
        WebRequest webRequest = base.GetWebRequest(uri);
        if (HttpRequestData != null)
        {
            webRequest.Timeout = HttpRequestData.TimeoutInS * 1000;
        }

        return webRequest;
    }
}