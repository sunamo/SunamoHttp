namespace SunamoHttp._sunamo.SunamoFileExtensions.Enums;

internal enum TypeOfExtension
{
    archive,
    image,
    source_code,
    documentText,
    documentBinary,
    database,

    // Verified that all extensions in AllExtension are text-based
    configText,

    // XML, JSON, mdf, ldf, sdf, etc.
    // Can't name data because is difficult search (exists also database)
    contentText,
    contentBinary,

    // Verified that all extensions in AllExtension are text-based
    // ini, etc.
    settingsText,

    // Verified that all extensions in AllExtension are text-based
    visual_studioText,
    executable,
    binary,

    // For resources, it probably wouldn't hurt if they were encoded in base64, but to be safe,
    // I classify them all as binary to avoid damaging them
    resource,

    // Verified that all extensions in AllExtension are text-based
    // sql, cmd, ps1, etc.
    script,
    font,
    multimedia,
    temporary,

    // Is used when extension isn't known
    // For other files, display their description from Windows
    other
}
