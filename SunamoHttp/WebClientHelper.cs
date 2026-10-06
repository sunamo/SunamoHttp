namespace SunamoHttp;

public class WebClientHelper
{
    private static SunamoWebClient sunamoWebClient = new SunamoWebClient();

    public static string GetResponseText(string address, HttpRequestData? httpRequestData)
    {
        sunamoWebClient.HttpRequestData = httpRequestData;
        return sunamoWebClient.DownloadString(address).FromSpace160To32();
    }

    public static byte[] GetResponseBytes(string address)
    {
        return sunamoWebClient.DownloadData(address);
    }

    private static WebClient webClient = new WebClient();

    public static Stream GetResponseStream(string address)
    {
        return webClient.OpenRead(address);
    }
}
