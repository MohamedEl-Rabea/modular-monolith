namespace WT.Customers.Portal.BuildingBlocks.Integration;

public class CommunicationOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public Authentication? Authentication { get; set; }
}

public class Authentication
{
    public BasicAuth? Basic { get; set; }
    public ApiKey? ApiKey { get; set; }
    public Bearer? Bearer { get; set; }
}

public class BasicAuth
{
    public string Value { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class ApiKey
{
    public string HeaderName { get; set; } = "x-api-key";
    public string Value { get; set; } = string.Empty;
}

public class Bearer
{
    public string Value { get; set; } = string.Empty;
}