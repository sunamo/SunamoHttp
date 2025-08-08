using SunamoHttp;
using SunamoHttp.Tests;
using SunamoPlatformUwpInterop.AppData;

namespace RunnerHttp;

internal class Program
{
    static void Main()
    {
        MainAsync(args).GetAwaiter().GetResult();


    }

    static async Task MainAsync(string[] args)
    {
        HttpRequestHelperTests t = new HttpRequestHelperTests();
        //await t.DownloadOrReadTest();
        await t.DownloadTest();
    }
}
