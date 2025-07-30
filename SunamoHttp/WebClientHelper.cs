namespace SunamoHttp;

public class WebClientHelper
{
    static SunamoWebClient swc = new SunamoWebClient();

    public static string GetResponseText(string address, HttpRequestData hrd)
    {
        swc.hrd = hrd;
        return swc.DownloadString(address).FromSpace160To32();
    }

    public static byte[] GetResponseBytes(string address)
    {
        return swc.DownloadData(address);
    }

    static WebClient wc = new WebClient();

    public static Stream GetResponseStream(string address)
    {
        return wc.OpenRead(address);
    }
}
