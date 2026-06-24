namespace SunamoHttp;

public class HttpClientHelper
{
    public static HttpClient HttpClientInstance { get; set; } = new HttpClient();

    public static HttpResponseMessage? HttpResponseMessage { get; set; } = null;

    private HttpClientHelper()
    {
    }


    public static
async Task<string>
GetResponseText(string address, HttpMethod method, HttpRequestData? httpRequestData = null)
    {
        HttpResponseMessage =
            await
            GetResponse(address, method, httpRequestData)!;
        return
            await GetResponseText(HttpResponseMessage);
    }

    private static async Task<string> GetResponseText(HttpResponseMessage response)
    {
        string responseText;
        using (response)
        {
            // Must be await, not AsyncHelper, not .Result, otherwise will be frozen
            responseText = (await response.Content.ReadAsStringAsync()).FromSpace160To32();
        }
        return responseText;
    }

    public static
async Task<Stream>
GetResponseStream(string address, HttpMethod method, HttpRequestData httpRequestData)
    {
        HttpResponseMessage response =
            await
            GetResponse(address, method, httpRequestData);
        using (response)
        {
            return
await
response.Content.ReadAsStreamAsync();
        }
    }

    public static
    async Task<HttpResponseMessage>
        GetResponse(string address, HttpMethod method, HttpRequestData? httpRequestData = null)
    {
        if (httpRequestData == null)
        {
            httpRequestData = new HttpRequestData();
        }
        SetHttpHeaders(httpRequestData, HttpClientInstance);

        HttpContent? httpContent = httpRequestData.Content;
        HttpResponseMessage response;
        if (method == HttpMethod.Get)
        {
            response =
                await HttpClientInstance.GetAsync(address);
        }
        else if (method == HttpMethod.Post)
        {
            var responseTask = HttpClientInstance.PostAsync(address, httpContent);
            response = await responseTask;
        }
        else
        {
            throw new NotSupportedException($"HTTP method {method} is not supported");
        }

        return response;
    }

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
