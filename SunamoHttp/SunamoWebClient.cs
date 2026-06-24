namespace SunamoHttp;

public class SunamoWebClient : WebClient
{
    public HttpRequestData? HttpRequestData { get; set; } = null;

    public SunamoWebClient()
    {
        base.Encoding = Encoding.UTF8;
    }

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
