using Microsoft.AspNetCore.Http;

namespace WT.Customers.Portal.BuildingBlocks.Extensions;

public static class FormFileExtensions
{
    public static async Task<byte[]> GetBytesAsync(this IFormFile file)
    {
        if (file == null || file.Length == 0)
            return [];

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}