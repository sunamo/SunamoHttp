namespace SunamoHttp._sunamo.SunamoFileExtensions.Attributes;

internal class TypeOfExtensionAttribute : Attribute
{
    internal TypeOfExtensionAttribute(TypeOfExtension toe)
    {
        Type = toe;
    }

    internal TypeOfExtension Type { get; set; }
}