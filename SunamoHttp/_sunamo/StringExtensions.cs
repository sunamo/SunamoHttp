namespace SunamoHttp._sunamo;

/// <summary>
/// String extension methods for HTTP operations
/// </summary>
internal static class StringExtensions
{
    /// <summary>
    /// Replaces all Unicode space characters (including non-breaking space 160) with regular space (32)
    /// </summary>
    /// <param name="input">The input string</param>
    /// <returns>The string with normalized spaces</returns>
    internal static string FromSpace160To32(this string input)
    {
        return Regex.Replace(input, @"\p{Z}", " ");
    }
}