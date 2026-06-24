namespace SunamoHttp._sunamo;

internal class SHSplit
{
    internal static List<string> Split(string text, params string[] delimiters) =>
        text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();
}
