namespace SunamoHttp;

public class NetHelper
{
    public static
async Task<string>
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
            buffer = Encoding.ASCII.GetBytes($"Content-Disposition: form-data; name=\"{name}\"{Environment.NewLine}{Environment.NewLine}");
            requestStream.Write(buffer, 0, buffer.Length);
            buffer = Encoding.UTF8.GetBytes(values[name] + Environment.NewLine);
            requestStream.Write(buffer, 0, buffer.Length);
        }
        // Write the files
        foreach (var file in files)
        {
            var buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
            requestStream.Write(buffer, 0, buffer.Length);
            buffer = Encoding.UTF8.GetBytes($"Content-Disposition: form-data; name=\"{file.Name}\"; filename=\"{file.Filename}\"{Environment.NewLine}");
            requestStream.Write(buffer, 0, buffer.Length);
            buffer = Encoding.ASCII.GetBytes($"Content-Type: {file.ContentType}{Environment.NewLine}");
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
await
HttpClientHelper.GetResponseText(address, method, httpRequestData);
        return responseText;
    }

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
