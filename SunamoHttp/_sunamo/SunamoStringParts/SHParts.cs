namespace SunamoHttp._sunamo.SunamoStringParts;

/// <summary>
/// String parts manipulation helper methods
/// </summary>
internal class SHParts
{
    /// <summary>
    /// Removes everything after the first occurrence of the specified character/string
    /// </summary>
    /// <param name="text">The input text</param>
    /// <param name="searchText">The character or string to search for</param>
    /// <returns>The text before the first occurrence, or original text if not found</returns>
    internal static string RemoveAfterFirst(string text, string searchText)
    {
        var index = text.IndexOf(searchText);
        if (index == -1 || index == text.Length - 1) return text;

        var result = text.Remove(index);
        return result;
    }
}