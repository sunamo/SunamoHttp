namespace SunamoHttp;

public class WebClientHelper
{
    static SunamoWebClient swc = new SunamoWebClient();

    public static string GetResponseText(string address, HttpMethod method, HttpRequestData hrd)
    {
        swc.hrd = hrd;
        return swc.DownloadString(address);
    }

    public static byte[] GetResponseBytes(string address, HttpMethod method)
    {
        return swc.DownloadData(address);
    }

    static WebClient wc = new WebClient();

    public static Stream GetResponseStream(string address, HttpMethod method)
    {
        return wc.OpenRead(address);
    }
}
