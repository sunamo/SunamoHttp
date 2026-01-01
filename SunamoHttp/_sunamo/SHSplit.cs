namespace SunamoHttp._sunamo;

/// <summary>
/// String split helper methods
/// </summary>
internal class SHSplit
{
    /// <summary>
    /// Splits the input string by the specified delimiters
    /// </summary>
    /// <param name="text">The text to split</param>
    /// <param name="delimiters">The delimiter strings</param>
    /// <returns>A list of split strings with empty entries removed</returns>
    internal static List<string> Split(string text, params string[] delimiters)
    {
        return text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();
    }
}