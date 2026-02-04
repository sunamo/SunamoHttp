namespace SunamoHttp._sunamo.SunamoFileExtensions.Enums;

internal enum TypeOfExtension
{
    archive,
    image,
    source_code,
    documentText,
    documentBinary,
    database,

    /// <summary>
    ///     Verified that all extensions in AllExtension are text-based
    /// </summary>
    configText,

    /// <summary>
    ///     XML, JSON, mdf, ldf, sdf, etc.
    ///     Can't name data because is difficult search (exists also database)
    /// </summary>
    contentText,
    contentBinary,

    /// <summary>
    ///     Verified that all extensions in AllExtension are text-based
    ///     ini, etc.
    /// </summary>
    settingsText,

    /// <summary>
    ///     Verified that all extensions in AllExtension are text-based
    /// </summary>
    visual_studioText,
    executable,
    binary,

    /// <summary>
    ///     For resources, it probably wouldn't hurt if they were encoded in base64, but to be safe,
    ///     I classify them all as binary to avoid damaging them
    /// </summary>
    resource,

    /// <summary>
    ///     Verified that all extensions in AllExtension are text-based
    ///     sql, cmd, ps1, etc.
    /// </summary>
    script,
    font,
    multimedia,
    temporary,

    /// <summary>
    ///     Is used when extension isn't known
    ///     For other files, display their description from Windows
    /// </summary>
    other
}