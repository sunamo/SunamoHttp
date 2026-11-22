namespace SunamoHttp.Tests;

/// <summary>
/// Tests of the synchronous members without a logger, against a local listener.
/// </summary>
public class HttpRequestHelperSyncTests
{
    /// <summary>
    /// Starts a local listener that answers one request with the User-Agent it received and returns its address.
    /// </summary>
    private static string StartEchoUserAgentServer(out System.Threading.Tasks.Task serverTask)
    {
        var tcpListener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Loopback, 0);
        tcpListener.Start();
        var port = ((System.Net.IPEndPoint)tcpListener.LocalEndpoint).Port;
        tcpListener.Stop();

        var prefix = $"http://localhost:{port}/";
        var listener = new System.Net.HttpListener();
        listener.Prefixes.Add(prefix);
        listener.Start();
        serverTask = System.Threading.Tasks.Task.Run(() =>
        {
            var context = listener.GetContext();
            var body = Encoding.UTF8.GetBytes("UA=" + context.Request.UserAgent + ";žluť");
            context.Response.ContentType = "text/plain; charset=utf-8";
            context.Response.ContentLength64 = body.Length;
            context.Response.OutputStream.Write(body, 0, body.Length);
            context.Response.Close();
            // The listener is stopped only after the client had time to read the body; stopping at once resets the connection.
            System.Threading.Thread.Sleep(500);
            listener.Stop();
        });
        return prefix;
    }

    /// <summary>
    /// The text overload without a logger sends the default User-Agent and decodes the body.
    /// </summary>
    [Fact]
    public void GetResponseText_WithoutLogger_SendsUserAgent()
    {
        var address = StartEchoUserAgentServer(out var serverTask);

        var text = HttpRequestHelper.GetResponseText(address, HttpMethod.Get);

        serverTask.Wait(5000);
        Assert.Equal("UA=" + HttpRequestHelper.DefaultUserAgent + ";žluť", text);
    }

    /// <summary>
    /// DownloadFile creates the missing folder, writes the file and honours the extension filter.
    /// </summary>
    [Fact]
    public void DownloadFile_CreatesFolderAndWritesFile()
    {
        var folder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "SunamoHttpTests", Guid.NewGuid().ToString("N"), "nested");
        try
        {
            Assert.False(HttpRequestHelper.DownloadFile("http://127.0.0.1:9/never-called", folder, "photo", ".exe", extension => extension == ".exe"));
            Assert.False(System.IO.Directory.Exists(folder));

            var address = StartEchoUserAgentServer(out var serverTask);
            Assert.True(HttpRequestHelper.DownloadFile(address, folder, "photo", ".jpg", extension => extension == ".exe"));
            serverTask.Wait(5000);

            var path = System.IO.Path.Combine(folder, "photo.jpg");
            Assert.True(System.IO.File.Exists(path));
            Assert.StartsWith("UA=" + HttpRequestHelper.DefaultUserAgent, System.IO.File.ReadAllText(path));
        }
        finally
        {
            var root = System.IO.Path.GetDirectoryName(folder)!;
            if (System.IO.Directory.Exists(root))
            {
                System.IO.Directory.Delete(root, true);
            }
        }
    }
}
