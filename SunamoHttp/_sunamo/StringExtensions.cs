namespace SunamoHttp._sunamo;

internal static class StringExtensions
{
    internal static string FromSpace160To32(this string input) =>
        Regex.Replace(input, @"\p{Z}", " ");
}
