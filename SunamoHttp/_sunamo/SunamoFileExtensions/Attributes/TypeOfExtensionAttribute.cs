namespace SunamoHttp._sunamo.SunamoFileExtensions.Attributes;

internal class TypeOfExtensionAttribute : Attribute
{
    internal TypeOfExtensionAttribute(TypeOfExtension typeOfExtension)
    {
        Type = typeOfExtension;
    }

    internal TypeOfExtension Type { get; set; }
}
