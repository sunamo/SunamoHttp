// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoHttp._sunamo.SunamoUri;

internal class UH
{
    internal static string GetFileName(string uri)
    {
        return Path.GetFileName(uri);
    }

    internal static string GetFileNameWithoutExtension(string p)
    {
        return Path.GetFileNameWithoutExtension(GetFileName(p));
    }
}