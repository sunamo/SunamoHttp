namespace SunamoHttp;

/// <summary>
/// Represents the web client helper type.
/// </summary>
public class WebClientHelper
{
    private static SunamoWebClient sunamoWebClient = new SunamoWebClient();

    /// <summary>
    /// Get response text.
    /// </summary>
    /// <param name="address">The address.</param>
    /// <param name="httpRequestData">The http request data.</param>
    /// <returns>The get response text result.</returns>
    public static string GetResponseText(string address, HttpRequestData? httpRequestData)
    {
        sunamoWebClient.HttpRequestData = httpRequestData;
        return sunamoWebClient.DownloadString(address).FromSpace160To32();
    }

    /// <summary>
    /// Get response bytes.
    /// </summary>
    /// <param name="address">The address.</param>
    /// <returns>The get response bytes result.</returns>
    public static byte[] GetResponseBytes(string address)
    {
        return sunamoWebClient.DownloadData(address);
    }

    private static WebClient webClient = new WebClient();

    /// <summary>
    /// Get response stream.
    /// </summary>
    /// <param name="address">The address.</param>
    /// <returns>The get response stream result.</returns>
    public static Stream GetResponseStream(string address)
    {
        return webClient.OpenRead(address);
    }
}
