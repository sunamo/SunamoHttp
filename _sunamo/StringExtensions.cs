namespace SunamoHttp._sunamo;

internal static class StringExtensions
{
public static string FromSpace160To32(this string input)
{
return Regex.Replace(input, @"\p{Z}", " ");
}
}