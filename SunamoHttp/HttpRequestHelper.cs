namespace SunamoHttp;

public static partial class HttpRequestHelper
{
    /// <summary>
    /// Download or read worker.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="path">The path.</param>
    /// <param name="uri">The uri.</param>
    /// <param name="args">The args.</param>
    /// <returns>The download or read worker result.</returns>
    public static async Task<string> DownloadOrReadWorker(ILogger logger, string path, string uri, DownloadOrReadArgs? args = null)
    {
        if (args == null)
        {
            args = new DownloadOrReadArgs();
        }
        if (!FS.ExistsFile(path) || args.ForceDownload)
        {
            await Download(logger, args, uri, null, path);
        }
        return File.ReadAllText(path).FromSpace160To32();
    }

    // WARNING: Switched parameter order - A2 and A1
    /// <summary>
    /// Download or read.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="appDataCachePath">The app data cache path.</param>
    /// <param name="uri">The uri.</param>
    /// <param name="args">The args.</param>
    /// <returns>The download or read result.</returns>
    public static async Task<string> DownloadOrRead(ILogger logger, string appDataCachePath, string uri, DownloadOrReadArgs? args = null)
    {
        if (args == null)
        {
            args = new DownloadOrReadArgs();
        }
        var uriFileName = UH.GetFileName(uri);
        var sanitizedFileName = FS.ReplaceInvalidFileNameChars(uriFileName);
        sanitizedFileName = FS.Combine(appDataCachePath, SH.AppendIfDontEndingWith(sanitizedFileName, AllExtensions.html));
        return await DownloadOrReadWorker(logger, sanitizedFileName, uri, args);
    }

    /// <summary>
    /// Exists page.
    /// </summary>
    /// <param name="url">The url.</param>
    /// <returns>The exists page result.</returns>
    public static bool ExistsPage(string url)
    {
        try
        {
            HttpWebRequest? request = WebRequest.Create(url) as HttpWebRequest;
            if (request != null)
            {
                request.Method = "HEAD";
                HttpWebResponse? response = request.GetResponse() as HttpWebResponse;
                if (response != null)
                {
                    response.Close();
                    return (response.StatusCode == HttpStatusCode.OK);
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Is not found.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="args">The args.</param>
    /// <param name="uri">The uri.</param>
    /// <returns>The is not found result.</returns>
    public static bool IsNotFound(ILogger logger, GetResponseArgs? args, object uri)
    {
        HttpWebResponse? response;
        GetResponseText(logger, args, uri.ToString() ?? string.Empty, HttpMethod.Get, null, out response);
        return HttpResponseHelper.IsNotFound(response);
    }

    /// <summary>
    /// Some error.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="args">The args.</param>
    /// <param name="uri">The uri.</param>
    /// <returns>The some error result.</returns>
    public static bool SomeError(ILogger logger, GetResponseArgs? args, object uri)
    {
        HttpWebResponse? response;
        GetResponseText(logger, args, uri.ToString() ?? string.Empty, HttpMethod.Get, null, out response);
        return HttpResponseHelper.SomeError(response);
    }

    /// <summary>
    /// Download all.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="args">The args.</param>
    /// <param name="uris">The uris.</param>
    /// <param name="dontHaveAllowedExtension">The dont have allowed extension.</param>
    /// <param name="folder2">The folder 2.</param>
    /// <param name="collisionOption">The collision option.</param>
    /// <param name="ext">The ext.</param>
    public static async Task DownloadAll(ILogger logger, GetResponseArgs? args, List<string> uris, Func<string, bool>? dontHaveAllowedExtension, string folder2, FileMoveCollisionOptionHttp collisionOption, string? ext = null)
    {
        if (collisionOption != FileMoveCollisionOptionHttp.Overwrite)
        {
            ThrowEx.Custom("Is allowed only Overwrite. Due to deps FS.MoveFile is not possible to use.");
        }
        foreach (var item in uris)
        {
            var tempPath = FS.GetTempFilePath();
            await Download(logger, args, item, dontHaveAllowedExtension, tempPath);
            var to = FS.Combine(folder2, Path.GetFileName(item) + ext);
#if NET48 || NETSTANDARD2_0
            if (File.Exists(to)) File.Delete(to);
            File.Move(tempPath, to);
#else
            File.Move(tempPath, to, true);
#endif
        }
    }

    /// <summary>
    /// Download.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="args">The args.</param>
    /// <param name="uri">The uri.</param>
    /// <param name="dontHaveAllowedExtension">The dont have allowed extension.</param>
    /// <param name="folder2">The folder 2.</param>
    /// <param name="fileName">The file name.</param>
    /// <param name="ext">The ext.</param>
    /// <returns>The download result.</returns>
    public static async Task<bool> Download(ILogger logger, GetResponseArgs? args, string uri, Func<string, bool>? dontHaveAllowedExtension, string folder2, string fileName, string? ext = null)
    {
        if (dontHaveAllowedExtension != null)
        {
            if (ext != null && dontHaveAllowedExtension(ext))
            {
                ext += ".jpeg";
            }
        }
        if (string.IsNullOrWhiteSpace(ext))
        {
            ext = FS.GetExtension(uri);
            ext = SHParts.RemoveAfterFirst(ext, "?");
        }
        fileName = SHParts.RemoveAfterFirst(fileName, "?");
        string path = FS.Combine(folder2, fileName + ext);
        FS.CreateFoldersPsysicallyUnlessThere(folder2);
        if (!FS.ExistsFile(path) || FS.GetFileSize(path) == 0)
        {
            var count = await GetResponseBytes(logger, args, uri, HttpMethod.Get);
            TF.WriteAllBytes(path, count);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Download.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="args">The args.</param>
    /// <param name="uri">The uri.</param>
    /// <param name="dontHaveAllowedExtension">The dont have allowed extension.</param>
    /// <param name="path">The path.</param>
    /// <returns>The download result.</returns>
    public static async Task<bool> Download(ILogger logger, GetResponseArgs? args, string uri, Func<string, bool>? dontHaveAllowedExtension, string path)
    {
        string folderPath, fileName, ext;
        FS.GetPathAndFileNameWithoutExtension(path, out folderPath, out fileName, out ext);
        return await Download(logger, args, uri, dontHaveAllowedExtension, folderPath, fileName, Path.GetExtension(path));
    }

    /// <summary>
    /// Gets or sets the progress bar.
    /// </summary>
    public static IProgressBarHttp? ProgressBar { get; set; } = null;

    /// <summary>
    /// Download.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="args">The args.</param>
    /// <param name="uri">The uri.</param>
    /// <param name="dontHaveAllowedExtension">The dont have allowed extension.</param>
    /// <param name="folder2">The folder 2.</param>
    /// <param name="fileName">The file name.</param>
    /// <param name="timeoutInMs">The timeout in ms.</param>
    /// <param name="ext">The ext.</param>
    /// <returns>The download result.</returns>
    public static async Task<bool> Download(ILogger logger, GetResponseArgs? args, string uri, Func<string, bool>? dontHaveAllowedExtension, string folder2, string fileName, int timeoutInMs, string? ext = null)
    {
        if (dontHaveAllowedExtension != null)
        {
            if (ext != null && dontHaveAllowedExtension(ext))
            {
                ext += ".jpeg";
            }
        }
        if (string.IsNullOrWhiteSpace(ext))
        {
            ext = Path.GetExtension(uri);
            ext = SHParts.RemoveAfterFirst(ext, "?");
        }
        fileName = SHParts.RemoveAfterFirst(fileName, "?");
        string path = Path.Combine(folder2, fileName + ext);
        FS.CreateFoldersPsysicallyUnlessThere(folder2);
        if (!File.Exists(path) || new FileInfo(path).Length == 0)
        {
            var count = await GetResponseBytes(logger, args, uri, HttpMethod.Get, timeoutInMs);
            if (count.Length != 0)
            {
#if NET48 || NETSTANDARD2_0
                File.WriteAllBytes(path, count);
#else
                await File.WriteAllBytesAsync(path, count);
#endif
                return true;
            }
        }
        return false;
    }

    static string ShortPathFromUri(string text)
    {
        var fileNameWithoutExtension = UH.GetFileNameWithoutExtension(text);
        var qs = new Uri(text).Query;
        StringBuilder stringBuilder = new StringBuilder();
        var queryParameters = qs.Split('&');
        foreach (var item in queryParameters)
        {
            stringBuilder.Append(item.Split('=')[1] + ",");
        }
        text = FS.ReplaceInvalidFileNameChars(fileNameWithoutExtension + stringBuilder.ToString());
        return text;
    }

    /// <summary>
    /// Before testing ip address.
    /// </summary>
    /// <param name="ipAddress">The ip address.</param>
    /// <returns>The before testing ip address result.</returns>
    public static string BeforeTestingIpAddress(string ipAddress)
    {
        if (ipAddress == "::1")
        {
            ipAddress = "127.0.0.1";
        }
        return ipAddress;
    }
}
