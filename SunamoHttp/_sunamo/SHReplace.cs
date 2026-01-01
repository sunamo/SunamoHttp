namespace SunamoHttp._sunamo;

/// <summary>
/// String replacement helper methods
/// </summary>
internal class SHReplace
{
    /// <summary>
    /// Replaces the first occurrence of the specified pattern
    /// </summary>
    /// <param name="input">The input text</param>
    /// <param name="what">The pattern to find</param>
    /// <param name="replacement">The replacement text</param>
    /// <returns>The text with the first occurrence replaced</returns>
    internal static string ReplaceOnce(string input, string what, string replacement)
    {
        return new Regex(what).Replace(input, replacement, 1);
    }
}