namespace SunamoHttp;

public class SunamoWebClient : WebClient
{
    public HttpRequestData hrd = null;

    public SunamoWebClient()
    {

    }

    protected override WebRequest GetWebRequest(Uri uri)
    {
        WebRequest w = base.GetWebRequest(uri);
        w.Timeout = hrd.timeoutInS * 1000;
        return w;
    }
}
