// variables names: ok
using Microsoft.Extensions.Logging;
using SunamoHttp.Args;
#if !NET8_0
using SunamoPlatformUwpInterop._public.SunamoEnums.Enums;
using SunamoPlatformUwpInterop.AppData;
using SunamoTest;
#endif
using System.Threading.Tasks;

namespace SunamoHttp.Tests;

/// <summary>
/// Tests for HttpRequestHelper class
/// </summary>
public class HttpRequestHelperTests
{
#if !NET8_0
    private readonly ILogger logger = TestLogger.Instance;
#else
#pragma warning disable CS0414 // Field is assigned but never used - only used in non-NET8.0 builds
    private readonly ILogger? logger = null;
#pragma warning restore CS0414
#endif

    /// <summary>
    /// Tests the DownloadOrRead functionality
    /// </summary>
    [Fact]
    public async Task DownloadOrReadTest()
    {
#if !NET8_0
        AppData.ci.CreateAppFoldersIfDontExists(new SunamoPlatformUwpInterop.Args.CreateAppFoldersIfDontExistsArgs { AppName = "SunamoHttp.Tests" });

        var html = await HttpRequestHelper.DownloadOrRead(TestLogger.Instance, AppData.ci.GetFolder(AppFolders.Cache), @"https://reality.idnes.cz/s/prodej/domy/okres-kutna-hora/?s-qc%5BusableAreaMin%5D=60&s-qc%5BusableAreaMax%5D=70", new DownloadOrReadArgs { ForceDownload = false });
#endif
        await Task.CompletedTask;
    }

    /// <summary>
    /// Tests the Download functionality
    /// </summary>
    [Fact]
    public async Task DownloadTest()
    {
#if !NET8_0
        if (logger != null)
        {
            var html = await HttpRequestHelper.Download(logger, null, "https://www.cooljobs.eu/cz/java/38016", null, @"DownloadTest.html");
        }
#endif
        await Task.CompletedTask;
    }
}