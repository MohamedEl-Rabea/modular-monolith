namespace Sample.Service.Integrations.External;

public sealed class ExternalApiOptions
{
    public const string SectionName = "ExternalApi";

    public string BaseUrl { get; set; } = default!;
    public string ApiKey { get; set; } = default!;
}
