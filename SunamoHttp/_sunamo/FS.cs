namespace SunamoHttp._sunamo;

internal class FS
{
    internal static void CreateFoldersPsysicallyUnlessThere(string path)
    {
        ThrowEx.IsNullOrEmpty("path", path);

        if (Directory.Exists(path))
        {
            return;
        }

        var foldersToCreate = new List<string>
        {
            path
        };

        while (true)
        {
            var parentPath = Path.GetDirectoryName(path);
            if (parentPath is null)
            {
                break;
            }
            path = parentPath;

            // TODO: This doesn't work for UWP/UAP apps because they don't have access to the entire disk.
            // Need to determine what UWP/UAP is and how to get/verify any folder on the disk in it
            if (Directory.Exists(path))
            {
                break;
            }

            foldersToCreate.Add(path);
        }

        foldersToCreate.Reverse();
        foreach (var item in foldersToCreate)
        {
            if (!Directory.Exists(item))
            {
                Directory.CreateDirectory(item);
            }
        }
    }

    internal static string Combine(params string[] paths) => Path.Combine(paths);

    internal static bool ExistsFile(string path) => FileMs.Exists(path);

    internal static long GetFileSize(string filePath)
    {
        FileInfo? fileInfo = null;
        try
        {
            fileInfo = new FileInfo(filePath);
        }
        catch (Exception)
        {
            // For example, file name is too long
            return 0;
        }
        if (fileInfo?.Exists == true)
        {
            return fileInfo.Length;
        }
        return 0;
    }

    internal static string GetExtension(string path) => Path.GetExtension(path);

    internal static void GetPathAndFileNameWithoutExtension(string filePath, out string path, out string file, out string ext)
    {
        path = Path.GetDirectoryName(filePath) + '\\';
        file = Path.GetFileNameWithoutExtension(filePath);
        ext = Path.GetExtension(filePath);
    }

    internal static string GetTempFilePath() => Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetTempFileName());

    internal static string ReplaceInvalidFileNameChars(string fileName) => string.Concat(fileName.Split(Path.GetInvalidFileNameChars()));
}
