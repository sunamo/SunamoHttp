namespace SunamoHttp.Args;

/// <summary>
/// Arguments for download or read operations
/// </summary>
public class DownloadOrReadArgs : GetResponseArgs
{
    /// <summary>
    /// Gets or sets a value indicating whether to force download
    /// </summary>
    public bool ForceDownload { get; set; } = false;
}