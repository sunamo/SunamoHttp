namespace SunamoHttp._sunamo.SunamoFileIO;

internal class TF
{
    internal static void WriteAllBytes(string path, byte[] bytes)
    {
        File.WriteAllBytes(path, bytes);
    }
}
