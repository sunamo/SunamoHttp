namespace RunnerHttp;

using SunamoHttp;
using SunamoHttp.Tests;
using SunamoPlatformUwpInterop.AppData;

internal class Program
{
    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();


    }

    static async Task MainAsync()
    {
        HttpRequestHelperTests t = new HttpRequestHelperTests();
        //await t.DownloadOrReadTest();
        await t.DownloadTest();
    }
}