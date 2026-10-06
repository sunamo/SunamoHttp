namespace SunamoHttp.Args;

public class DownloadOrReadArgs : GetResponseArgs
{
    public bool ForceDownload { get; set; } = false;
}
