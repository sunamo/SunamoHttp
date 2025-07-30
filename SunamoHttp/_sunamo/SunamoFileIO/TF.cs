namespace SunamoHttp._sunamo.SunamoFileIO;

internal class TF
{

    internal static void WriteAllBytes(string path, byte[] c)
    {
        File.WriteAllBytes(path, c);
    }
}