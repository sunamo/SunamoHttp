namespace SunamoHttp._sunamo.SunamoUri;

internal class UH
{
    internal static string GetFileName(string uri)
    {
        return Path.GetFileName(uri);
    }

    internal static string GetFileNameWithoutExtension(string uri)
    {
        return Path.GetFileNameWithoutExtension(GetFileName(uri));
    }
}
