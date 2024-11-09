namespace SunamoHttp;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

partial class HttpRequestHelper
{
    static void LogDownload(ILogger logger, string address)
    {
        if (logger != null)
        {
            logger.LogTrace("Downloading " + address);
        }
    }

    /// <summary>
    /// If return empty array, SharedAlgorithms.lastError contains HttpError
    /// </summary>
    /// <param name = "address"></param>
    public static async Task<byte[]> GetResponseBytes(ILogger logger, GetResponseArgs a, string address, HttpMethod method, int timeoutInMs = 30000)
    {
        if (a == null)
        {
            a = new GetResponseArgs();
        }

        LogDownload(logger, address);

        var request = (HttpWebRequest)WebRequest.Create(address);
        request.CookieContainer = a.Cookies;

        #region MyRegion
        /*
jsem nastavil protože běz něj nejde 

        i tak pak dostávám:
        'Transfer-Encoding: chunked' header can not be used when content object is not specified.

        mělo by to být nějak kvůli seznamka.cz ale tu už dnes nepotřeubji, dělám to přes Selenium
        takže zakomentuji oba dva
         */
        //request.SendChunked = true;

        //request.TransferEncoding = "UTF-8"; 
        #endregion

        request.Method = method.Method;
        request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11";
        WebResponse r = null;
        int times = 5;
        r = await SharedAlgorithms.RepeatAfterTimeXTimesAsync(times, timeoutInMs, new Func<Task<WebResponse>>(request.GetResponseAsync));
        if (EqualityComparer<WebResponse>.Default.Equals(r, default(WebResponse)))
        {
            //var before = ThrowEx.FullNameOfExecutedCode(type, Exceptions.CallingMethod());
            //ThisApp.Warning(Exceptions.RepeatAfterTimeXTimesFailed(before, times, timeoutInMs, address, SharedAlgorithms.lastError));
            return new byte[0];
        }
        else
        {
            HttpWebResponse response = (HttpWebResponse)r;
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
    /// Is not async coz t.Result
    /// </summary>
    /// <param name="address"></param>
    public async static Task<string> GetResponseTextAsync(ILogger logger, GetResponseArgs a, string address)
    {
        if (a == null)
        {
            a = new GetResponseArgs();
        }

        LogDownload(logger, address);

        var request = (HttpWebRequest)WebRequest.CreateHttp(address);
        request.CookieContainer = a.Cookies;
        request.Timeout = int.MaxValue;
        request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11";
        var t = request.GetResponseAsync();
        using (var response = (HttpWebResponse)t.Result)
        {
            Encoding encoding = null;
            if (response.CharacterSet == "")
            {
                //encoding = Encoding.UTF8;
            }
            else
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
                string vr = reader.ReadToEnd();
                return vr;
            }
        }
    }

    /// <summary>
    /// A3 can be null
    /// </summary>
    /// <param name="address"></param>
    /// <param name="method"></param>
    /// <param name="hrd"></param>
    public static string GetResponseText(ILogger logger, GetResponseArgs a, string address, HttpMethod method, HttpRequestData hrd)
    {
        HttpWebResponse response;
        return GetResponseText(logger, a, address, method, hrd, out response);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name = "address"></param>
    public static Stream GetResponseStream(ILogger logger, GetResponseArgs a, string address, HttpMethod method)
    {
        if (a == null)
        {
            a = new GetResponseArgs();
        }

        LogDownload(logger, address);

        var request = (HttpWebRequest)WebRequest.Create(address);
        request.CookieContainer = a.Cookies;
        request.Method = method.Method;
        HttpWebResponse response = null;
        try
        {
            response = (HttpWebResponse)request.GetResponse();
        }
        catch (System.Exception ex)
        {
            return null;
        }
        return response.GetResponseStream();
    }

    public static string GetResponseText(ILogger logger, GetResponseArgs a, string address, HttpMethod method, HttpRequestData hrd, out HttpWebResponse response)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
        return GetResponseText(logger, a, request, method, hrd, out response);
    }

    /// <summary>
    /// A3 can be null
    /// Dont forger Dispose on A4
    /// </summary>
    /// <param name = "address"></param>
    /// <param name = "method"></param>
    /// <param name = "hrd"></param>
    public static string GetResponseText(ILogger logger, GetResponseArgs a, HttpWebRequest request, HttpMethod method, HttpRequestData hrd, out HttpWebResponse response)
    {
        if (a == null)
        {
            a = new GetResponseArgs();
        }

        LogDownload(logger, request.RequestUri.ToString());

        response = null;
        if (hrd == null)
        {
            hrd = new HttpRequestData();
        }
        var address = request.Address.ToString();
        int dex = address.IndexOf('?');
        string adressCopy = address;
        if (method.Method.ToUpper() == "POST")
        {
            if (dex != -1)
            {
                address = address.Substring(0, dex);
            }
        }
        // Cant create new instance, in A1 can be setted up property which is not allowed in Headers
        //request.Address = address;
        string result = null;
        request.CookieContainer = a.Cookies;
        request.Method = method.Method;
        if (method == HttpMethod.Post)
        {
            string query = adressCopy.Substring(dex + 1);
            Encoding encoder = null;
            if (hrd.encodingPostData == null)
            {
                encoder = new ASCIIEncoding();
            }
            else
            {
                encoder = hrd.encodingPostData;
            }
            byte[] data = encoder.GetBytes(query);
            request.ContentType = "application/x-www-urlencoded";
            request.ContentLength = data.Length;
            request.GetRequestStream().Write(data, 0, data.Length);
        }
        //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11";
        request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.157 Safari/537.36";
        if (hrd.contentType != null)
        {
            request.ContentType = hrd.contentType;
        }
        if (hrd.accept != null)
        {
            request.Accept = hrd.accept;
        }
        if (hrd != null)
        {
            foreach (var item in hrd.headers)
            {
                request.Headers.Add(item.Key, item.Value);
            }
        }
        try
        {
            response = (HttpWebResponse)request.GetResponse();
            Encoding encoding = null;
            if (response.CharacterSet == "")
            {
                //encoding = Encoding.UTF8;
            }
            else
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
                result = reader.ReadToEnd();
            }
        }
        catch (System.Exception ex)
        {
            result = Exceptions.TextOfExceptions(ex);
        }
        return result;
    }
}