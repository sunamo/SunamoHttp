namespace SunamoHttp._sunamo.SunamoFileIO;

/// <summary>
/// Text file helper methods
/// </summary>
internal class TF
{
    /// <summary>
    /// Writes all bytes to the specified file
    /// </summary>
    /// <param name="path">The file path</param>
    /// <param name="bytes">The bytes to write</param>
    internal static void WriteAllBytes(string path, byte[] bytes)
    {
        File.WriteAllBytes(path, bytes);
    }
}