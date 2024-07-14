namespace SunamoHttp.Code;

/// <summary>
/// Pokud chceš náhradu za třídu HttpRequestHelper, použij
/// </summary>
public class HttpClientHelper
{
    public static HttpClient hc = new HttpClient();
    private HttpClientHelper()
    {
    }
    public static HttpResponseMessage httpResponseMessage = null;
    /// <summary>
    /// Return always HttpResponseMessage, can't return HttpWebResponse
    /// Same url:
    /// HttpClientHelper.GetResponseText - Exception: The remote server returned an error: (400) Bad Request., response is null
    /// HttpClientHelper.GetResponseText - really xml, exists response
    /// Pros: Better is HttpClientHelper because I can parse error
    /// Cons: HttpClientHelper.GetResponseText not return HttpWebResponse object, only HttpResponseMessage
    /// </summary>
    /// <param name="address"></param>
    /// <param name="method"></param>
    /// <param name="hrd"></param>
    /// <returns></returns>
    public static
#if ASYNC
async Task<string>
#else
string
#endif
GetResponseText(string address, HttpMethod method, HttpRequestData hrd = null)
    {
        httpResponseMessage =
#if ASYNC
            await
#endif
            GetResponse(address, method, hrd);
        return GetResponseText(httpResponseMessage);
    }
    private static string GetResponseText(HttpResponseMessage response)
    {
        string vr = "";
        using (response)
        {
            // Must be await, not AsyncHelper, not .Result, otherwise will be frozen
            //vr =  AsyncHelper.ci.GetResult<string>( response.Content.ReadAsStringAsync());
            vr = response.Content.ReadAsStringAsync().Result;
        }
        return vr;
    }
    public static
#if ASYNC
async Task<Stream>
#else
  Stream
#endif
GetResponseStream(string address, HttpMethod method, HttpRequestData hrd)
    {
        HttpResponseMessage response =
#if ASYNC
            await
#endif
            GetResponse(address, method, hrd);
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
    /// A3 can be null
    /// Return always HttpResponseMessage, can't return HttpWebResponse
    /// </summary>
    /// <param name="address"></param>
    /// <param name="method"></param>
    /// <param name="hrd"></param>
    /// <returns></returns>
    public static
#if ASYNC
    async Task<HttpResponseMessage>
#else
    HttpResponseMessage
#endif
        GetResponse(string address, HttpMethod method, HttpRequestData hrd = null)
    {
        if (hrd == null)
        {
            hrd = new HttpRequestData();
        }
        SetHttpHeaders(hrd, hc);
        string adressCopy = address;
        #region Do samostatné metody pokud bych to někdy potřeboval, post neznamená že požadavek nemůže mít query string
        #endregion
        HttpContent httpContent = hrd.content;
        HttpResponseMessage response = null;
        if (method == HttpMethod.Get)
        {
            response =
#if ASYNC
                await hc.GetAsync(address);
#else
                hc.GetAsync(address).Result;
#endif
        }
        else if (method == HttpMethod.Post)
        {
            var resp = hc.PostAsync(address, httpContent);
#if ASYNC
            response = await resp;
#else
            response = resp.Result;
#endif
        }
        else
        {
        }
        //HttpResponseMessage response = responseTask.Result;
        return response;
    }
    static Type type = typeof(HttpClientHelper);
    private static void SetHttpHeaders(HttpRequestData hrd, HttpClient hc)
    {
        hc = new HttpClient();
        //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11";
        hc.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.157 Safari/537.36");
        if (hrd.accept != null)
        {
            hc.DefaultRequestHeaders.Add(HttpKnownHeaderNames.Accept, hrd.accept);
        }
        if (hrd.keepAlive.HasValue)
        {
            hc.DefaultRequestHeaders.Add(HttpKnownHeaderNames.KeepAlive, hrd.keepAlive.ToString());
        }
        if (hrd != null)
        {
            foreach (var item in hrd.headers)
            {
                hc.DefaultRequestHeaders.Add(item.Key, item.Value);
            }
        }
    }
}