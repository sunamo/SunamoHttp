// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy

namespace SunamoHttp;

public static partial class HttpRequestHelper
{
    /// <summary>
    /// In earlier time return ext
    /// Now return whether was downloaded
    /// </summary>
    /// <param name="path"></param>
    /// <param name="uri"></param>
    /// <returns></returns>
    public static async Task<string> DownloadOrReadWorker(ILogger logger, string path, string uri, DownloadOrReadArgs a = null)
    {
        if (a == null)
        {
            a = new DownloadOrReadArgs();
        }
        string html = null;
        if (!FS.ExistsFile(path) || a.forceDownload)
        {
            await Download(logger, a, uri, null, path);
        }
        html = File.ReadAllText(path).FromSpace160To32();
        return html;
    }
    /// <summary>
    /// As folder is use Cache
    /// 
    /// POZOR, přehodil jsem A2 a A1
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="uri"></param>
    public static async Task<string> DownloadOrRead(ILogger logger, string appDataCachePath, string uri, DownloadOrReadArgs a = null)
    {
        if (a == null)
        {
            a = new DownloadOrReadArgs();
        }
        var value = UH.GetFileName(uri);
        var fn = FS.ReplaceInvalidFileNameChars(value);
        fn = FS.Combine(appDataCachePath, SH.AppendIfDontEndingWith(fn, AllExtensions.html));
        return await DownloadOrReadWorker(logger, fn, uri);
    }
    public static bool ExistsPage(string url)
    {
        try
        {
            //Creating the HttpWebRequest
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //Setting the Request method HEAD, you can also use GET too.
            request.Method = "HEAD";
            //Getting the Web Response.
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //Returns TRUE if the Status code == 200
            response.Close();
            return (response.StatusCode == HttpStatusCode.OK);
        }
        catch
        {
            //Any exception will returns false.
            return false;
        }
    }
    public static bool IsNotFound(ILogger logger, GetResponseArgs a, object uri)
    {
        HttpWebResponse r;
        var test = GetResponseText(logger, a, uri.ToString(), HttpMethod.Get, null, out r);
        return HttpResponseHelper.IsNotFound(r);
    }
    public static bool SomeError(ILogger logger, GetResponseArgs a, object uri)
    {
        HttpWebResponse r;
        var test = GetResponseText(logger, a, uri.ToString(), HttpMethod.Get, null, out r);
        return HttpResponseHelper.SomeError(r);
    }
    /// <summary>
    /// A2 can be null (if dont have duplicated extension, set null)
    /// </summary>
    /// <param name="hrefs"></param>
    /// <param name="DontHaveAllowedExtension"></param>
    /// <param name="folder2"></param>
    /// <param name="co"></param>
    /// <param name="ext"></param>
    public static async Task DownloadAll(ILogger logger, GetResponseArgs a, List<string> hrefs, Func<string, bool> DontHaveAllowedExtension, string folder2, FileMoveCollisionOptionHttp co, string ext = null)
    {
        if (co != FileMoveCollisionOptionHttp.Overwrite)
        {
            ThrowEx.Custom("Is allowed only Overwrite. Due to deps FS.MoveFile is not possible to use.");
        }
        foreach (var item in hrefs)
        {
            var tempPath = FS.GetTempFilePath();
            await Download(logger, a, item, DontHaveAllowedExtension, tempPath);
            var to = FS.Combine(folder2, Path.GetFileName(item) + ext);
            File.Move(tempPath, to, true);
        }
    }
    /// <summary>
    /// A2 can be null (if dont have duplicated extension, set null)
    /// In earlier time return ext
    /// Now return whether was downloaded
    /// </summary>
    /// <param name = "href"></param>
    /// <param name = "DontHaveAllowedExtension"></param>
    /// <param name = "folder2"></param>
    /// <param name = "fn"></param>
    /// <param name = "ext"></param>
    public static async Task<bool> Download(ILogger logger, GetResponseArgs a, string href, Func<string, bool> DontHaveAllowedExtension, string folder2, string fn, string ext = null)
    {
        if (DontHaveAllowedExtension != null)
        {
            if (DontHaveAllowedExtension(ext))
            {
                ext += ".jpeg";
            }
        }
        if (string.IsNullOrWhiteSpace(ext))
        {
            ext = FS.GetExtension(href);
            ext = SHParts.RemoveAfterFirst(ext, "?");
        }
        fn = SHParts.RemoveAfterFirst(fn, "?");
        string path = FS.Combine(folder2, fn + ext);
        FS.CreateFoldersPsysicallyUnlessThere(folder2);
        if (!FS.ExistsFile(path) || FS.GetFileSize(path) == 0)
        {
            var count = await GetResponseBytes(logger, a, href, HttpMethod.Get);
            TF.WriteAllBytes(path, count);
            return true;
        }
        return false;
    }
    /// <summary>
    /// In earlier time return ext
    /// Now return whether was downloaded
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="DontHaveAllowedExtension"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static async Task<bool> Download(ILogger logger, GetResponseArgs a, string uri, Func<string, bool> DontHaveAllowedExtension, string path)
    {
        string parameter, fn, ext;
        FS.GetPathAndFileNameWithoutExtension(path, out parameter, out fn, out ext);
        return await Download(logger, a, uri, DontHaveAllowedExtension, parameter, fn, Path.GetExtension(path));
    }
    public static IProgressBarHttp clpb = null;
    /// <summary>
    /// A2 can be null (if dont have duplicated extension, set null)
    /// In earlier time return ext
    /// Now return whether was downloaded
    /// </summary>
    /// <param name = "href"></param>
    /// <param name = "DontHaveAllowedExtension"></param>
    /// <param name = "folder2"></param>
    /// <param name = "fn"></param>
    /// <param name = "ext"></param>
    public static async Task<bool> Download(ILogger logger, GetResponseArgs a, string href, Func<string, bool> DontHaveAllowedExtension, string folder2, string fn, int timeoutInMs, string ext = null)
    {
        // TODO: měl jsem tu arg , string fullPathForCompare
        // zkontrolovat zda se tu ta cesta skládá správně
        // přidání nového arg do prostřed to zurví spoustu dalšího kódu
        if (DontHaveAllowedExtension != null)
        {
            if (DontHaveAllowedExtension(ext))
            {
                ext += ".jpeg";
            }
        }
        if (string.IsNullOrWhiteSpace(ext))
        {
            ext = Path.GetExtension(href);
            ext = SHParts.RemoveAfterFirst(ext, "?");
        }
        fn = SHParts.RemoveAfterFirst(fn, "?");
        string path = Path.Combine(folder2, fn + ext);
        //if (path != fullPathForCompare)
        //{
        //    System.Diagnostics.Debugger.Break();
        //}
        FS.CreateFoldersPsysicallyUnlessThere(folder2);
        if (!File.Exists(path) || new FileInfo(path).Length == 0)
        {
            var count = await GetResponseBytes(logger, a, href, HttpMethod.Get, timeoutInMs);
            if (count.Length != 0)
            {
                await File.WriteAllBytesAsync(path, count);
                return true;
            }
        }
        return false;
    }
    static string ShortPathFromUri(string text)
    {
        #region Nefungovalo, furt to bylo příliš dlouhé
        //s = SHParts.KeepAfterFirst(text, "://");
        //s = SHParts.KeepAfterFirst(text, "www.");
        //// Abych ušetřil ještě nějaké místo, nebudu vkládat ani host
        //s = SHParts.KeepAfterFirst(text, "/");
        /*
         * Z nějakého důvodu, když to dekóduji, tak mi count# nedokáže zapsat soubor text tímto názvem
         * Ale VSCode to zvládne value pohodě: \\?\D:\Documents\sunamo\ConsoleApp1\Cache\sprodejdomycena-do-1000000moravskoslezsky-krajs-qc[usableAreaMin]=40&s-qc[ownership][0]=personal&s-qc[condition][0]=new&s-qc[condition][1]=good-condition&s-qc[condition][2]=maintained&s-qc[condition][3]=after-reconstruction.html
         * Píše to že část cesty neexistuje ale žádné \ ani / value tom není a D:\Documents\sunamo\ConsoleApp1\Cache\ existuje
         *
         * Kdybych měl text tím znovu problemy, udělat to že string se převede na hash
         * Případně že se zapíšou jen hodnoty parametrů, nikoliv jejich názvy, oddělené ,
         * */
        //s = UH.UrlDecode(text);
        //s = FS.ReplaceInvalidFileNameChars(text);
        #endregion
        #region Část kódu kretá se používala když jsem vracel fn
        var value = UH.GetFileNameWithoutExtension(text);
        var qs = new Uri(text).Query;
        StringBuilder stringBuilder = new StringBuilder();
        var parameter = qs.Split('&');
        foreach (var item in parameter)
        {
            stringBuilder.Append(item.Split('=')[1] + ",");
        }
        text = FS.ReplaceInvalidFileNameChars(value + stringBuilder.ToString());
        #endregion
        return text;
    }
    public static string BeforeTestingIpAddress(string vr)
    {
        if (vr == "::1")
        {
            vr = "127.0.0.1";
        }
        return vr;
    }
}