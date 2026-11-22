namespace SunamoHttp.Args;

/// <summary>
/// Represents the download or read args type.
/// </summary>
public class DownloadOrReadArgs : GetResponseArgs
{
    /// <summary>
    /// Gets or sets the force download.
    /// </summary>
    public bool ForceDownload { get; set; } = false;
}
