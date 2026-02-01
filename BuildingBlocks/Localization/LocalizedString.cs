using System.Globalization;

namespace WT.B2C.API.BuildingBlocks.Localization;

public sealed record LocalizedText(
    string? En,
    string? Ar)
{
    public string Value =>
        CultureInfo.CurrentUICulture.TwoLetterISOLanguageName switch
        {
            "ar" => Ar ?? En ?? string.Empty,
            _ => En ?? Ar ?? string.Empty
        };

    public string AllLanguages => $"{En}-{Ar}";
    
    public override string ToString() => Value;

    public static implicit operator string(LocalizedText localized)
        => localized.Value;

    public bool IsEmpty =>
        string.IsNullOrWhiteSpace(En) &&
        string.IsNullOrWhiteSpace(Ar);
}