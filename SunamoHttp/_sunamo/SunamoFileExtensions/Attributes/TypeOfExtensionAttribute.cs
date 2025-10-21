// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoHttp._sunamo.SunamoFileExtensions.Attributes;

internal class TypeOfExtensionAttribute : Attribute
{
    internal TypeOfExtensionAttribute(TypeOfExtension toe)
    {
        Type = toe;
    }

    internal TypeOfExtension Type { get; set; }
}