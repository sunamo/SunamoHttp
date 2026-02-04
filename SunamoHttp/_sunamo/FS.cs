namespace SunamoHttp._sunamo;

/// <summary>
/// File system helper methods
/// </summary>
internal class FS
{
    /// <summary>
    /// Creates all folders in the path physically unless they already exist
    /// </summary>
    /// <param name="path">The directory path to create</param>
    internal static void CreateFoldersPsysicallyUnlessThere(string path)
    {
        ThrowEx.IsNullOrEmpty("path", path);

        if (Directory.Exists(path))
        {
            return;
        }

        List<string> foldersToCreate = new List<string>
        {
            path
        };

        while (true)
        {
            var parentPath = Path.GetDirectoryName(path);
            if (parentPath == null)
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
        foreach (string item in foldersToCreate)
        {
            if (!Directory.Exists(item))
            {
                Directory.CreateDirectory(item);
            }
        }
    }

    /// <summary>
    /// Combines multiple path segments into a single path
    /// </summary>
    /// <param name="paths">The path segments to combine</param>
    /// <returns>The combined path</returns>
    internal static string Combine(params string[] paths)
    {
        return Path.Combine(paths);
    }

    /// <summary>
    /// Checks if a file exists at the specified path
    /// </summary>
    /// <param name="path">The file path to check</param>
    /// <returns>True if the file exists, false otherwise</returns>
    internal static bool ExistsFile(string path)
    {
        return FileMs.Exists(path);
    }

    /// <summary>
    /// Gets the size of the file in bytes
    /// </summary>
    /// <param name="filePath">The path to the file</param>
    /// <returns>The file size in bytes, or 0 if file doesn't exist or error occurs</returns>
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

    /// <summary>
    /// Gets the extension of the file
    /// </summary>
    /// <param name="path">The file path</param>
    /// <returns>The file extension including the dot</returns>
    internal static string GetExtension(string path)
    {
        return Path.GetExtension(path);
    }

    /// <summary>
    /// Gets the path, file name without extension, and extension from a file path
    /// </summary>
    /// <param name="filePath">The full file path</param>
    /// <param name="path">Output parameter for the directory path</param>
    /// <param name="file">Output parameter for the file name without extension</param>
    /// <param name="ext">Output parameter for the file extension</param>
    internal static void GetPathAndFileNameWithoutExtension(string filePath, out string path, out string file, out string ext)
    {
        path = Path.GetDirectoryName(filePath) + '\\';
        file = Path.GetFileNameWithoutExtension(filePath);
        ext = Path.GetExtension(filePath);
    }

    /// <summary>
    /// Gets a temporary file path
    /// </summary>
    /// <returns>A full path to a temporary file</returns>
    internal static string GetTempFilePath()
    {
        return Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetTempFileName());
    }

    /// <summary>
    /// Replaces all invalid file name characters with empty string
    /// </summary>
    /// <param name="fileName">The file name to sanitize</param>
    /// <returns>The file name with invalid characters removed</returns>
    internal static string ReplaceInvalidFileNameChars(string fileName)
    {
        return string.Concat(fileName.Split(Path.GetInvalidFileNameChars()));
    }
}