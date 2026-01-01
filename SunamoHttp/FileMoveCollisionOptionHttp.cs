// variables names: ok
namespace SunamoHttp;

/// <summary>
/// Options for handling file collisions when moving or downloading files
/// </summary>
public enum FileMoveCollisionOptionHttp
{
    /// <summary>
    /// Add a serial number to the filename to make it unique
    /// </summary>
    AddSerie,
    /// <summary>
    /// Add the file size to the filename to make it unique
    /// </summary>
    AddFileSize,
    /// <summary>
    /// Overwrite the existing file
    /// </summary>
    Overwrite,
    /// <summary>
    /// Discard the incoming file and keep the existing one
    /// </summary>
    DiscardFrom,
    /// <summary>
    /// Keep the larger file (by size)
    /// </summary>
    LeaveLarger,
    /// <summary>
    /// Do not manipulate the file, leave it as is
    /// </summary>
    DontManipulate
}