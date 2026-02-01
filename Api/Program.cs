using Asp.Versioning;
using Sample.Service;
using WT.B2C.API.BuildingBlocks.Cache;
using WT.B2C.API.BuildingBlocks.Data;
using WT.B2C.API.BuildingBlocks.FeatureBehaviors;
using WT.B2C.API.BuildingBlocks.Localization;
using WT.B2C.API.BuildingBlocks.Runtime;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCacheService(builder.Configuration);
builder.Services.AddDataServices(builder.Configuration);
builder.Services.AddFeatureBehaviors();
builder.Services.AddContextAccessor();
builder.Services.AddAppLocalization();
builder.Services.AddSampleModule(builder.Configuration);
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseRequestLocalization();
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/healthz");
app.MapSampleModule();

await app.UseDataSeedersAsync();
await app.UseCacheWarmupAsync();

app.Run();
