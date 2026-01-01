namespace SunamoHttp._sunamo.SunamoFileExtensions.Attributes;

/// <summary>
/// Attribute for marking type of file extension
/// </summary>
internal class TypeOfExtensionAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeOfExtensionAttribute"/> class
    /// </summary>
    /// <param name="typeOfExtension">The type of extension</param>
    internal TypeOfExtensionAttribute(TypeOfExtension typeOfExtension)
    {
        Type = typeOfExtension;
    }

    /// <summary>
    /// Gets or sets the type of extension
    /// </summary>
    internal TypeOfExtension Type { get; set; }
}