namespace SunamoHttp;

/// <summary>
/// Helper class for HttpClient operations
/// If you want a replacement for the HttpRequestHelper class, use this
/// </summary>
public class HttpClientHelper
{
    /// <summary>
    /// Gets or sets the shared HttpClient instance
    /// </summary>
    public static HttpClient HttpClientInstance { get; set; } = new HttpClient();

    private HttpClientHelper()
    {
    }

    /// <summary>
    /// Gets or sets the last HTTP response message
    /// </summary>
    public static HttpResponseMessage HttpResponseMessage { get; set; } = null;

    /// <summary>
    /// Gets the response text from the specified address
    /// Return always HttpResponseMessage, can't return HttpWebResponse
    /// Same url:
    /// HttpClientHelper.GetResponseText - Exception: The remote server returned an error: (400) Bad Request., response is null
    /// HttpClientHelper.GetResponseText - really xml, exists response
    /// Pros: Better is HttpClientHelper because I can parse error
    /// Cons: HttpClientHelper.GetResponseText not return HttpWebResponse object, only HttpResponseMessage
    /// </summary>
    /// <param name="address">The URL address</param>
    /// <param name="method">The HTTP method</param>
    /// <param name="httpRequestData">The HTTP request configuration data</param>
    /// <returns>The response text</returns>
    public static
#if ASYNC
async Task<string>
#else
string
#endif
GetResponseText(string address, HttpMethod method, HttpRequestData httpRequestData = null)
    {
        HttpResponseMessage =
#if ASYNC
            await
#endif
            GetResponse(address, method, httpRequestData);
        return await GetResponseText(HttpResponseMessage);
    }

    /// <summary>
    /// Gets the response text from the HTTP response message
    /// </summary>
    /// <param name="response">The HTTP response message</param>
    /// <returns>The response text with spaces normalized</returns>
    private static async Task<string> GetResponseText(HttpResponseMessage response)
    {
        string responseText = "";
        using (response)
        {
            // Must be await, not AsyncHelper, not .Result, otherwise will be frozen
            responseText = (await response.Content.ReadAsStringAsync()).FromSpace160To32();
        }
        return responseText;
    }

    /// <summary>
    /// Gets the response stream from the specified address
    /// </summary>
    /// <param name="address">The URL address</param>
    /// <param name="method">The HTTP method</param>
    /// <param name="httpRequestData">The HTTP request configuration data</param>
    /// <returns>The response stream</returns>
    public static
#if ASYNC
async Task<Stream>
#else
  Stream
#endif
GetResponseStream(string address, HttpMethod method, HttpRequestData httpRequestData)
    {
        HttpResponseMessage response =
#if ASYNC
            await
#endif
            GetResponse(address, method, httpRequestData);
        using (response)
        {
#if ASYNC
            return
#if ASYNC
await
#endif
response.Content.ReadAsStreamAsync();
#else
            return response.Content.ReadAsStreamAsync().Result;
#endif
        }
    }

    /// <summary>
    /// Gets the HTTP response from the specified address
    /// Return always HttpResponseMessage, can't return HttpWebResponse
    /// </summary>
    /// <param name="address">The URL address</param>
    /// <param name="method">The HTTP method</param>
    /// <param name="httpRequestData">The HTTP request configuration data (can be null)</param>
    /// <returns>The HTTP response message</returns>
    public static
#if ASYNC
    async Task<HttpResponseMessage>
#else
    HttpResponseMessage
#endif
        GetResponse(string address, HttpMethod method, HttpRequestData httpRequestData = null)
    {
        if (httpRequestData == null)
        {
            httpRequestData = new HttpRequestData();
        }
        SetHttpHeaders(httpRequestData, HttpClientInstance);
        string addressCopy = address;

        HttpContent httpContent = httpRequestData.Content;
        HttpResponseMessage response = null;
        if (method == HttpMethod.Get)
        {
            response =
#if ASYNC
                await HttpClientInstance.GetAsync(address);
#else
                HttpClientInstance.GetAsync(address).Result;
#endif
        }
        else if (method == HttpMethod.Post)
        {
            var responseTask = HttpClientInstance.PostAsync(address, httpContent);
#if ASYNC
            response = await responseTask;
#else
            response = responseTask.Result;
#endif
        }

        return response;
    }

    /// <summary>
    /// Sets HTTP headers on the HttpClient instance
    /// </summary>
    /// <param name="httpRequestData">The HTTP request configuration data</param>
    /// <param name="httpClient">The HttpClient instance</param>
    private static void SetHttpHeaders(HttpRequestData httpRequestData, HttpClient httpClient)
    {
        if (httpClient == null)
        {
            httpClient = new HttpClient();
        }

        if (httpRequestData.Accept != null)
        {
            httpClient.DefaultRequestHeaders.Add(HttpKnownHeaderNames.Accept, httpRequestData.Accept);
        }
        if (httpRequestData.KeepAlive.HasValue)
        {
            httpClient.DefaultRequestHeaders.Add(HttpKnownHeaderNames.KeepAlive, httpRequestData.KeepAlive.ToString());
        }
        if (httpRequestData != null)
        {
            foreach (var item in httpRequestData.Headers)
            {
                httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
            }
        }
    }
}
