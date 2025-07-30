using Microsoft.Extensions.Logging;
using SunamoHttp.Args;
using SunamoPlatformUwpInterop._public.SunamoEnums.Enums;
using SunamoPlatformUwpInterop.AppData;
using SunamoTest;
using System.Threading.Tasks;

namespace SunamoHttp.Tests;

public class HttpRequestHelperTests
{
    [Fact]
    public async Task DownloadOrReadTest()
    {
        AppData.ci.CreateAppFoldersIfDontExists(new SunamoPlatformUwpInterop.Args.CreateAppFoldersIfDontExistsArgs { AppName = "SunamoHttp.Tests" });

        var html = await HttpRequestHelper.DownloadOrRead(TestLogger.Instance, @"https://reality.idnes.cz/s/prodej/domy/okres-kutna-hora/?s-qc%5BusableAreaMin%5D=60&s-qc%5BusableAreaMax%5D=70", AppData.ci.GetFolder(AppFolders.Cache), new DownloadOrReadArgs { forceDownload = false });
    }

    ILogger logger = TestLogger.Instance;

    //[Fact]
    //public async Task DownloadOrReadTest()
    //{
    //    AppData.ci.CreateAppFoldersIfDontExists(new SunamoPlatformUwpInterop.Args.CreateAppFoldersIfDontExistsArgs { AppName = "SunamoHttp.Tests" });

    //    HttpRequestHelper.DownloadOrRead(logger, AppData.ci.GetFolder(AppFolders.Cache), uriListing, new DownloadOrReadArgs { forceDownload = true });
    //}

    [Fact]
    public async Task DownloadTest()
    {
        var html = await HttpRequestHelper.Download(TestLogger.Instance, null, "https://www.cooljobs.eu/cz/java/38016", null, @"DownloadTest.html");
    }
}