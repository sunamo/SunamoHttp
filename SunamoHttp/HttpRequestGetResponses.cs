namespace SunamoHttp;

partial class HttpRequestHelper
{
    /// <summary>
    /// Logs the download operation
    /// </summary>
    /// <param name="logger">The logger instance</param>
    /// <param name="address">The URL address being downloaded</param>
    static void LogDownload(ILogger logger, string address)
    {
        if (logger != null)
        {
            logger.LogTrace("Downloading " + address);
        }
    }

    /// <summary>
    /// Gets the response as byte array from the specified address
    /// If return empty array, SharedAlgorithms.lastError contains HttpError
    /// </summary>
    /// <param name="logger">The logger instance</param>
    /// <param name="args">The response retrieval arguments</param>
    /// <param name="address">The URL address</param>
    /// <param name="method">The HTTP method</param>
    /// <param name="timeoutInMs">The timeout in milliseconds (default 30000)</param>
    /// <returns>The response bytes, or empty array on error</returns>
    public static async Task<byte[]> GetResponseBytes(ILogger logger, GetResponseArgs args, string address, HttpMethod method, int timeoutInMs = 30000)
    {
        if (args == null)
        {
            args = new GetResponseArgs();
        }
        LogDownload(logger, address);
        var request = (HttpWebRequest)WebRequest.Create(address);
        request.CookieContainer = args.Cookies;
        request.Method = method.Method;
        request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11";
        WebResponse webResponse = null;
        int times = 5;
        webResponse = await SharedAlgorithms.RepeatAfterTimeXTimesAsync(times, timeoutInMs, new Func<Task<WebResponse>>(request.GetResponseAsync));
        if (EqualityComparer<WebResponse>.Default.Equals(webResponse, default(WebResponse)))
        {
            return new byte[0];
        }
        else
        {
            HttpWebResponse response = (HttpWebResponse)webResponse;
            using (response)
            {
                Encoding encoding = null;
                if (response.CharacterSet == "")
                {
                    encoding = Encoding.UTF8;
                }
                else
                {
                    encoding = Encoding.GetEncoding(response.CharacterSet);
                }
                using (var responseStream = response.GetResponseStream())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        responseStream.CopyTo(ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        using (var reader = new StreamReader(ms, encoding))
                        {
                            using (BinaryReader br = new BinaryReader(reader.BaseStream))
                            {
                                return br.ReadBytes((int)ms.Length);
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Gets the response text asynchronously from the specified address
    /// Is not async because of temp.Result
    /// </summary>
    /// <param name="logger">The logger instance</param>
    /// <param name="args">The response retrieval arguments</param>
    /// <param name="address">The URL address</param>
    /// <returns>The response text</returns>
    public async static Task<string> GetResponseTextAsync(ILogger logger, GetResponseArgs args, string address)
    {
        if (args == null)
        {
            args = new GetResponseArgs();
        }
        LogDownload(logger, address);
        var request = (HttpWebRequest)WebRequest.CreateHttp(address);
        request.CookieContainer = args.Cookies;
        request.Timeout = int.MaxValue;
        request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11";
        var responseTask = request.GetResponseAsync();
        using (var response = (HttpWebResponse)responseTask.Result)
        {
            Encoding encoding = null;
            if (response.CharacterSet != "")
            {
                encoding = Encoding.GetEncoding(response.CharacterSet);
            }
            using (var responseStream = response.GetResponseStream())
            {
                StreamReader reader = null;
                if (encoding == null)
                {
                    reader = new StreamReader(responseStream, true);
                }
                else
                {
                    reader = new StreamReader(responseStream, encoding);
                }
                string responseText = reader.ReadToEnd().FromSpace160To32();
                return responseText;
            }
        }
    }

    /// <summary>
    /// Gets the response text from the specified address
    /// </summary>
    /// <param name="logger">The logger instance</param>
    /// <param name="args">The response retrieval arguments</param>
    /// <param name="address">The URL address</param>
    /// <param name="method">The HTTP method</param>
    /// <param name="httpRequestData">The HTTP request configuration data (can be null)</param>
    /// <returns>The response text</returns>
    public static string GetResponseText(ILogger logger, GetResponseArgs args, string address, HttpMethod method, HttpRequestData httpRequestData)
    {
        HttpWebResponse response;
        return GetResponseText(logger, args, address, method, httpRequestData, out response);
    }

    /// <summary>
    /// Gets the response stream from the specified address
    /// </summary>
    /// <param name="logger">The logger instance</param>
    /// <param name="args">The response retrieval arguments</param>
    /// <param name="address">The URL address</param>
    /// <param name="method">The HTTP method</param>
    /// <returns>The response stream, or null on error</returns>
    public static Stream GetResponseStream(ILogger logger, GetResponseArgs args, string address, HttpMethod method)
    {
        if (args == null)
        {
            args = new GetResponseArgs();
        }
        LogDownload(logger, address);
        var request = (HttpWebRequest)WebRequest.Create(address);
        request.CookieContainer = args.Cookies;
        request.Method = method.Method;
        HttpWebResponse response = null;
        try
        {
            response = (HttpWebResponse)request.GetResponse();
        }
        catch (System.Exception)
        {
            return null;
        }
        return response.GetResponseStream();
    }

    /// <summary>
    /// Gets the response text from the specified address with output response
    /// </summary>
    /// <param name="logger">The logger instance</param>
    /// <param name="args">The response retrieval arguments</param>
    /// <param name="address">The URL address</param>
    /// <param name="method">The HTTP method</param>
    /// <param name="httpRequestData">The HTTP request configuration data (can be null)</param>
    /// <param name="response">The HTTP web response object (output parameter)</param>
    /// <returns>The response text</returns>
    public static string GetResponseText(ILogger logger, GetResponseArgs args, string address, HttpMethod method, HttpRequestData httpRequestData, out HttpWebResponse response)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
        return GetResponseText(logger, args, request, method, httpRequestData, out response);
    }

    /// <summary>
    /// Gets the response text from the specified HTTP request
    /// Don't forget to Dispose the response parameter
    /// </summary>
    /// <param name="logger">The logger instance</param>
    /// <param name="args">The response retrieval arguments (can be null)</param>
    /// <param name="request">The HTTP web request</param>
    /// <param name="method">The HTTP method</param>
    /// <param name="httpRequestData">The HTTP request configuration data (can be null)</param>
    /// <param name="response">The HTTP web response object (output parameter)</param>
    /// <returns>The response text</returns>
    public static string GetResponseText(ILogger logger, GetResponseArgs args, HttpWebRequest request, HttpMethod method, HttpRequestData httpRequestData, out HttpWebResponse response)
    {
        if (args == null)
        {
            args = new GetResponseArgs();
        }
        LogDownload(logger, request.RequestUri.ToString());
        response = null;
        if (httpRequestData == null)
        {
            httpRequestData = new HttpRequestData();
        }
        var address = request.Address.ToString();
        int queryStartIndex = address.IndexOf('?');
        string addressCopy = address;
        if (method.Method.ToUpper() == "POST")
        {
            if (queryStartIndex != -1)
            {
                address = address.Substring(0, queryStartIndex);
            }
        }
        string result = null;
        request.CookieContainer = args.Cookies;
        request.Method = method.Method;
        if (method == HttpMethod.Post)
        {
            string query = addressCopy.Substring(queryStartIndex + 1);
            Encoding encoder = null;
            if (httpRequestData.EncodingPostData == null)
            {
                encoder = new ASCIIEncoding();
            }
            else
            {
                encoder = httpRequestData.EncodingPostData;
            }
            byte[] data = encoder.GetBytes(query);
            request.ContentType = "application/x-www-urlencoded";
            request.ContentLength = data.Length;
            request.GetRequestStream().Write(data, 0, data.Length);
        }
        request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.157 Safari/537.36";
        if (httpRequestData.ContentType != null)
        {
            request.ContentType = httpRequestData.ContentType;
        }
        if (httpRequestData.Accept != null)
        {
            request.Accept = httpRequestData.Accept;
        }
        if (httpRequestData != null)
        {
            foreach (var item in httpRequestData.Headers)
            {
                request.Headers.Add(item.Key, item.Value);
            }
        }
        try
        {
            response = (HttpWebResponse)request.GetResponse();
            Encoding encoding = null;
            if (response.CharacterSet != "")
            {
                encoding = Encoding.GetEncoding(response.CharacterSet);
            }
            using (var responseStream = response.GetResponseStream())
            {
                StreamReader reader = null;
                if (encoding == null)
                {
                    reader = new StreamReader(responseStream, true);
                }
                else
                {
                    reader = new StreamReader(responseStream, encoding);
                }
                result = reader.ReadToEnd().FromSpace160To32();
            }
        }
        catch (System.Exception ex)
        {
            result = Exceptions.TextOfExceptions(ex);
        }
        return result;
    }
}
