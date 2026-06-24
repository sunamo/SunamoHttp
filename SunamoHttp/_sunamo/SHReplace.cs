namespace SunamoHttp._sunamo;

internal class SHReplace
{
    internal static string ReplaceOnce(string input, string what, string replacement) =>
        new Regex(what).Replace(input, replacement, 1);
}
