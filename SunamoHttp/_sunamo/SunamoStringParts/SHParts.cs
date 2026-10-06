namespace SunamoHttp._sunamo.SunamoStringParts;

internal class SHParts
{
    internal static string RemoveAfterFirst(string text, string searchText)
    {
        var index = text.IndexOf(searchText);
        if (index == -1 || index == text.Length - 1) return text;

        var result = text.Remove(index);
        return result;
    }
}
