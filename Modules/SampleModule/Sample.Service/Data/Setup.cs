using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Contracts;
using Sample.Service.Data.Cache;
using Sample.Service.Data.Context;
using Sample.Service.Data.Queries;
using Sample.Service.Models;
using Sample.Service.Seeds;
using WT.B2C.API.BuildingBlocks.Cache;
using WT.B2C.API.BuildingBlocks.Data.Seeds;

namespace Sample.Service.Data;

internal static class Setup
{
    internal static void AddModuleData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SampleDbContext>(options =>
        {
            var cs = configuration.GetConnectionString("DefaultConnection");
            options.UseMySql(cs, new MySqlServerVersion(new Version(8, 0, 21)));
        });

        services.AddScoped<ICacheManager<SampleItem>, SampleItemsCacheManager>();
        services.AddScoped<ISampleItemsQueries, SampleItemsQueries>();
        services.AddScoped<ISampleModuleApi, SampleModuleApi>();
        services.AddSingleton<IDataSeeder, SampleItemsSeeder>();

        // Register cache managers for warmup
        services.AddScoped<ICacheWarmup, SampleItemsCacheManager>();
    }
}
