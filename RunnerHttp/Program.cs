using SunamoHttp;
using SunamoHttp.Tests;
using SunamoPlatformUwpInterop.AppData;

namespace RunnerHttp;

internal class Program
{
    static void Main(string[] args)
    {
        MainAsync(args).GetAwaiter().GetResult();


    }

    static async Task MainAsync(string[] args)
    {
        HttpRequestHelperSyncTests t = new HttpRequestHelperSyncTests();
        t.DownloadFile_CreatesFolderAndWritesFile();
        await Task.CompletedTask;
    }
}
