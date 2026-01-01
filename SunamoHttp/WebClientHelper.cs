namespace SunamoHttp;

/// <summary>
/// Helper class for WebClient operations
/// </summary>
public class WebClientHelper
{
    private static SunamoWebClient sunamoWebClient = new SunamoWebClient();

    /// <summary>
    /// Gets the response text from the specified address
    /// </summary>
    /// <param name="address">The URL address</param>
    /// <param name="httpRequestData">The HTTP request configuration data</param>
    /// <returns>The response text with spaces normalized</returns>
    public static string GetResponseText(string address, HttpRequestData httpRequestData)
    {
        sunamoWebClient.HttpRequestData = httpRequestData;
        return sunamoWebClient.DownloadString(address).FromSpace160To32();
    }

    /// <summary>
    /// Gets the response bytes from the specified address
    /// </summary>
    /// <param name="address">The URL address</param>
    /// <returns>The response as byte array</returns>
    public static byte[] GetResponseBytes(string address)
    {
        return sunamoWebClient.DownloadData(address);
    }

    private static WebClient webClient = new WebClient();

    /// <summary>
    /// Gets the response stream from the specified address
    /// </summary>
    /// <param name="address">The URL address</param>
    /// <returns>The response stream</returns>
    public static Stream GetResponseStream(string address)
    {
        return webClient.OpenRead(address);
    }
}