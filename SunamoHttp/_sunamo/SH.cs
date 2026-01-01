namespace SunamoHttp._sunamo;

/// <summary>
/// String helper methods
/// </summary>
internal class SH
{
    /// <summary>
    /// Appends the specified text if the input doesn't already end with it
    /// </summary>
    /// <param name="text">The input text</param>
    /// <param name="append">The text to append if not already ending with</param>
    /// <returns>The text with the appended string if it wasn't already there</returns>
    internal static string AppendIfDontEndingWith(string text, string append)
    {
        if (text.EndsWith(append))
        {
            return text;
        }
        return text + append;
    }
}