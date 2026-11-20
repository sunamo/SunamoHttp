namespace SunamoHttp;

/// <summary>
/// Represents the sunamo web client type.
/// </summary>
public class SunamoWebClient : WebClient
{
    /// <summary>
    /// Gets or sets the http request data.
    /// </summary>
    public HttpRequestData? HttpRequestData { get; set; } = null;

    /// <summary>
    /// Sunamo web client.
    /// </summary>
    /// <returns>The sunamo web client result.</returns>
    public SunamoWebClient()
    {
        base.Encoding = Encoding.UTF8;
    }

    /// <summary>
    /// Get web request.
    /// </summary>
    /// <param name="uri">The uri.</param>
    /// <returns>The get web request result.</returns>
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
