namespace SunamoHttp._sunamo.SunamoUri;

/// <summary>
/// URI helper methods
/// </summary>
internal class UH
{
    /// <summary>
    /// Gets the file name from URI
    /// </summary>
    /// <param name="uri">The URI string</param>
    /// <returns>The file name</returns>
    internal static string GetFileName(string uri)
    {
        return Path.GetFileName(uri);
    }

    /// <summary>
    /// Gets the file name without extension from URI
    /// </summary>
    /// <param name="uri">The URI string</param>
    /// <returns>The file name without extension</returns>
    internal static string GetFileNameWithoutExtension(string uri)
    {
        return Path.GetFileNameWithoutExtension(GetFileName(uri));
    }
}