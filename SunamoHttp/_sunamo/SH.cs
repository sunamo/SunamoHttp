namespace SunamoHttp._sunamo;

internal class SH
{
    internal static string AppendIfDontEndingWith(string text, string append)
    {
        if (text.EndsWith(append))
        {
            return text;
        }
        return text + append;
    }

}