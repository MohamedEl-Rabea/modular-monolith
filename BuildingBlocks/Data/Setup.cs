using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WT.B2C.API.BuildingBlocks.Data.Queries;
using WT.B2C.API.BuildingBlocks.Data.Repositories;
using WT.B2C.API.BuildingBlocks.Data.Seeds;

namespace WT.B2C.API.BuildingBlocks.Data;

public static class Setup
{
    public static void AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped(typeof(IQueryBuilderCreator<>), typeof(QueryBuilderCreator<>));
        services.AddSingleton<AppSeederRunner>();
    }
    
    public static async Task UseDataSeedersAsync(this WebApplication app)
    {
        await app.Services.GetRequiredService<AppSeederRunner>().EnsureSeededAsync();
    }
}