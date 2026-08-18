namespace SunamoHttp;

/// <summary>
/// Helper class for HTTP request operations
/// </summary>
public static partial class HttpRequestHelper
{
    /// <summary>
    /// Downloads or reads a file from cache
    /// In earlier time return ext
    /// Now return whether was downloaded
    /// </summary>
    /// <param name="logger">The logger instance</param>
    /// <param name="path">The local file path</param>
    /// <param name="uri">The URI to download from</param>
    /// <param name="args">The download or read arguments</param>
    /// <returns>The HTML content</returns>
    public static async Task<string> DownloadOrReadWorker(ILogger logger, string path, string uri, DownloadOrReadArgs? args = null)
    {
        if (args == null)
        {
            args = new DownloadOrReadArgs();
        }
        string? html = null;
        if (!FS.ExistsFile(path) || args.ForceDownload)
        {
            await Download(logger, args, uri, null, path);
        }
        html = File.ReadAllText(path).FromSpace160To32();
        return html;
    }

    /// <summary>
    /// Downloads or reads content from cache folder
    /// WARNING: Switched parameter order - A2 and A1
    /// </summary>
    /// <param name="logger">The logger instance</param>
    /// <param name="appDataCachePath">The cache folder path</param>
    /// <param name="uri">The URI to download from</param>
    /// <param name="args">The download or read arguments</param>
    /// <returns>The downloaded or cached content</returns>
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
    /// Checks if a page exists at the specified URL
    /// </summary>
    /// <param name="url">The URL to check</param>
    /// <returns>True if the page exists (HTTP 200 OK), false otherwise</returns>
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
    /// Checks if the resource at the specified URI was not found
    /// </summary>
    /// <param name="logger">The logger instance</param>
    /// <param name="args">The response retrieval arguments</param>
    /// <param name="uri">The URI to check</param>
    /// <returns>True if resource was not found, false otherwise</returns>
    public static bool IsNotFound(ILogger logger, GetResponseArgs? args, object uri)
    {
        HttpWebResponse? response;
        var test = GetResponseText(logger, args, uri.ToString() ?? string.Empty, HttpMethod.Get, null, out response);
        return HttpResponseHelper.IsNotFound(response);
    }

    /// <summary>
    /// Checks if there was an error accessing the specified URI
    /// </summary>
    /// <param name="logger">The logger instance</param>
    /// <param name="args">The response retrieval arguments</param>
    /// <param name="uri">The URI to check</param>
    /// <returns>True if there was an error, false otherwise</returns>
    public static bool SomeError(ILogger logger, GetResponseArgs? args, object uri)
    {
        HttpWebResponse? response;
        var test = GetResponseText(logger, args, uri.ToString() ?? string.Empty, HttpMethod.Get, null, out response);
        return HttpResponseHelper.SomeError(response);
    }

    /// <summary>
    /// Downloads all files from the specified hrefs
    /// </summary>
    /// <param name="logger">The logger instance</param>
    /// <param name="args">The response retrieval arguments</param>
    /// <param name="uris">The list of URIs to download</param>
    /// <param name="dontHaveAllowedExtension">Function to check if extension is not allowed (can be null)</param>
    /// <param name="folder2">The destination folder</param>
    /// <param name="collisionOption">The file move collision option</param>
    /// <param name="ext">The file extension (can be null)</param>
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
#if NET48
            if (File.Exists(to)) File.Delete(to);
            File.Move(tempPath, to);
#else
            File.Move(tempPath, to, true);
#endif
        }
    }

    /// <summary>
    /// Downloads a file from the specified href
    /// In earlier time return ext
    /// Now return whether was downloaded
    /// </summary>
    /// <param name="logger">The logger instance</param>
    /// <param name="args">The response retrieval arguments</param>
    /// <param name="uri">The URI to download from</param>
    /// <param name="dontHaveAllowedExtension">Function to check if extension is not allowed (can be null)</param>
    /// <param name="folder2">The destination folder</param>
    /// <param name="fileName">The file name</param>
    /// <param name="ext">The file extension (can be null)</param>
    /// <returns>True if file was downloaded, false if already exists</returns>
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
    /// Downloads a file to the specified path
    /// In earlier time return ext
    /// Now return whether was downloaded
    /// </summary>
    /// <param name="logger">The logger instance</param>
    /// <param name="args">The response retrieval arguments</param>
    /// <param name="uri">The URI to download from</param>
    /// <param name="dontHaveAllowedExtension">Function to check if extension is not allowed (can be null)</param>
    /// <param name="path">The destination file path</param>
    /// <returns>True if file was downloaded, false if already exists</returns>
    public static async Task<bool> Download(ILogger logger, GetResponseArgs? args, string uri, Func<string, bool>? dontHaveAllowedExtension, string path)
    {
        string folderPath, fileName, ext;
        FS.GetPathAndFileNameWithoutExtension(path, out folderPath, out fileName, out ext);
        return await Download(logger, args, uri, dontHaveAllowedExtension, folderPath, fileName, Path.GetExtension(path));
    }

    /// <summary>
    /// Gets or sets the progress bar for HTTP operations
    /// </summary>
    public static IProgressBarHttp? ProgressBar { get; set; } = null;

    /// <summary>
    /// Downloads a file from the specified href with timeout
    /// In earlier time return ext
    /// Now return whether was downloaded
    /// </summary>
    /// <param name="logger">The logger instance</param>
    /// <param name="args">The response retrieval arguments</param>
    /// <param name="uri">The URI to download from</param>
    /// <param name="dontHaveAllowedExtension">Function to check if extension is not allowed (can be null)</param>
    /// <param name="folder2">The destination folder</param>
    /// <param name="fileName">The file name</param>
    /// <param name="timeoutInMs">The timeout in milliseconds</param>
    /// <param name="ext">The file extension (can be null)</param>
    /// <returns>True if file was downloaded, false if already exists</returns>
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
#if NET48
                File.WriteAllBytes(path, count);
#else
                await File.WriteAllBytesAsync(path, count);
#endif
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Creates a short path from URI for caching purposes
    /// </summary>
    /// <param name="text">The URI text</param>
    /// <returns>The shortened path suitable for file names</returns>
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
    /// Normalizes IP address before testing (converts ::1 to 127.0.0.1)
    /// </summary>
    /// <param name="ipAddress">The IP address to normalize</param>
    /// <returns>The normalized IP address</returns>
    public static string BeforeTestingIpAddress(string ipAddress)
    {
        if (ipAddress == "::1")
        {
            ipAddress = "127.0.0.1";
        }
        return ipAddress;
    }
}