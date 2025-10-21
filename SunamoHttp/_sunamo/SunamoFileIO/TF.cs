// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoHttp._sunamo.SunamoFileIO;

internal class TF
{

    internal static void WriteAllBytes(string path, byte[] c)
    {
        File.WriteAllBytes(path, c);
    }
}