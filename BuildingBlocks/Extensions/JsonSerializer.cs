using System.Text.Json;

namespace WT.B2C.API.BuildingBlocks.Extensions;

public static class JsonSerializer
{
    public static bool TryDeserialize<T>(string? input, out T? result)
    {
        result = default;

        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        try
        {
            result = System.Text.Json.JsonSerializer.Deserialize<T>(input);
            return result is not null;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}
