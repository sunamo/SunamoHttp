namespace SunamoHttp;

public static partial class HttpRequestHelper
{
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

    public static bool IsNotFound(ILogger logger, GetResponseArgs? args, object uri)
    {
        HttpWebResponse? response;
        GetResponseText(logger, args, uri.ToString() ?? string.Empty, HttpMethod.Get, null, out response);
        return HttpResponseHelper.IsNotFound(response);
    }

    public static bool SomeError(ILogger logger, GetResponseArgs? args, object uri)
    {
        HttpWebResponse? response;
        GetResponseText(logger, args, uri.ToString() ?? string.Empty, HttpMethod.Get, null, out response);
        return HttpResponseHelper.SomeError(response);
    }

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
            FileCompat.Move(tempPath, to, true);
        }
    }

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

    public static async Task<bool> Download(ILogger logger, GetResponseArgs? args, string uri, Func<string, bool>? dontHaveAllowedExtension, string path)
    {
        string folderPath, fileName, ext;
        FS.GetPathAndFileNameWithoutExtension(path, out folderPath, out fileName, out ext);
        return await Download(logger, args, uri, dontHaveAllowedExtension, folderPath, fileName, Path.GetExtension(path));
    }

    public static IProgressBarHttp? ProgressBar { get; set; } = null;

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
                await FileAsync.WriteAllBytesAsync(path, count);
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

    public static string BeforeTestingIpAddress(string ipAddress)
    {
        if (ipAddress == "::1")
        {
            ipAddress = "127.0.0.1";
        }
        return ipAddress;
    }
}
