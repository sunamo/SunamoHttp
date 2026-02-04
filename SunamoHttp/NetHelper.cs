namespace SunamoHttp;

/// <summary>
/// Network helper utilities for HTTP operations
/// </summary>
public class NetHelper
{
    /// <summary>
    /// Posts files with form data to the specified address
    /// </summary>
    /// <param name="address">The URL address</param>
    /// <param name="method">The HTTP method</param>
    /// <param name="files">The files to upload</param>
    /// <param name="values">The form values</param>
    /// <param name="httpRequestData">The HTTP request configuration data</param>
    /// <returns>The response text</returns>
    public static
#if ASYNC
async Task<string>
#else
string
#endif
PostFiles(string address, HttpMethod method, IList<UploadFile> files, Dictionary<string, string> values, HttpRequestData httpRequestData)
    {
        var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
        httpRequestData.ContentType = "multipart/form-data; boundary=" + boundary;
        httpRequestData.KeepAlive = false;
        boundary = "--" + boundary;
        MemoryStream requestStream = new MemoryStream();
        var content = new StreamContent(requestStream);
        // Write the values
        foreach (string name in values.Keys)
        {
            var buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
            requestStream.Write(buffer, 0, buffer.Length);
            buffer = Encoding.ASCII.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", name, Environment.NewLine));
            requestStream.Write(buffer, 0, buffer.Length);
            buffer = Encoding.UTF8.GetBytes(values[name] + Environment.NewLine);
            requestStream.Write(buffer, 0, buffer.Length);
        }
        // Write the files
        foreach (var file in files)
        {
            var buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
            requestStream.Write(buffer, 0, buffer.Length);
            buffer = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}", file.Name, file.Filename, Environment.NewLine));
            requestStream.Write(buffer, 0, buffer.Length);
            buffer = Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}{1}", file.ContentType, Environment.NewLine));
            requestStream.Write(buffer, 0, buffer.Length);
            if (file.Stream != null)
            {
                file.Stream.CopyTo(requestStream);
            }
            buffer = Encoding.ASCII.GetBytes(Environment.NewLine);
            requestStream.Write(buffer, 0, buffer.Length);
        }
        var boundaryBuffer = Encoding.ASCII.GetBytes(boundary + "--");
        requestStream.Write(boundaryBuffer, 0, boundaryBuffer.Length);
        if (httpRequestData.ContentType != null)
        {
            content.Headers.Add("Content-Type", httpRequestData.ContentType);
        }
        httpRequestData.Content = content;
        var responseText =
#if ASYNC
await
#endif
HttpClientHelper.GetResponseText(address, method, httpRequestData);
        return responseText;
    }

    /// <summary>
    /// Sets HTTP headers on the request message
    /// </summary>
    /// <param name="httpRequestData">The HTTP request configuration data</param>
    /// <param name="httpRequestMessage">The HTTP request message</param>
    private static void SetHttpHeaders(HttpRequestData httpRequestData, HttpRequestMessage httpRequestMessage)
    {
        httpRequestMessage.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.157 Safari/537.36");
        if (httpRequestData.Accept != null)
        {
            httpRequestMessage.Headers.Add(HttpKnownHeaderNames.Accept, httpRequestData.Accept);
        }
        if (httpRequestData.KeepAlive.HasValue)
        {
            httpRequestMessage.Headers.Add(HttpKnownHeaderNames.KeepAlive, httpRequestData.KeepAlive.ToString());
        }
        if (httpRequestData != null)
        {
            foreach (var item in httpRequestData.Headers)
            {
                httpRequestMessage.Headers.Add(item.Key, item.Value);
            }
        }
    }
}