// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoHttp._sunamo;

internal static class StringExtensions
{
internal static string FromSpace160To32(this string input)
{
return Regex.Replace(input, @"\p{Z}", " ");
}
}