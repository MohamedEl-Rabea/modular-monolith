using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WT.B2C.API.BuildingBlocks.Runtime;
using WT.B2C.API.BuildingBlocks.Types;

namespace WT.B2C.API.BuildingBlocks.FeatureBehaviors;

public static class EndpointsStaticHandler
{
    public static async Task<IResult> HandleEndpoint<TInput, TOutput>(HttpContext ctx, TInput request,
        CancellationToken cancellationToken)
    {
        return await Handle<TInput, TOutput>(ctx, request, cancellationToken);
    }

    public static Task<IResult> HandleGet<TInput, TOutput>(
        HttpContext ctx,
        [AsParameters] [NotNull] TInput request,
        CancellationToken cancellationToken)
        => Handle<TInput, TOutput>(ctx, request, cancellationToken);

    private static async Task<IResult> Handle<TInput, TOutput>(
        HttpContext ctx,
        TInput request,
        CancellationToken cancellationToken)
    {
        var contextAccessor = ctx.RequestServices.GetRequiredService<IExecutionContextAccessor>();

        try
        {
            var handler = ctx.RequestServices.GetRequiredService<IFeatureHandler<TInput, TOutput>>();
            FeatureHandlerDelegate<TInput, TOutput> pipeline = (req, ct) => handler.Handle(req, ct);
            var behaviors = ctx.RequestServices
                .GetServices<IFeatureBehavior<TInput, TOutput>>()
                .Reverse()
                .ToArray();

            foreach (var behavior in behaviors)
            {
                var next = pipeline;
                pipeline = (req, ct) => behavior.Handle(req, next, ct);
            }

            var result = await pipeline(request, cancellationToken);
            result.RequestId = contextAccessor.CorrelationId;
            return Results.Json(result, statusCode: result.StatusCode);
        }
        catch (Exception ex)
        {
            var featureLogger = ctx.RequestServices.GetRequiredService<ILogger<IFeatureHandler<TInput, TOutput>>>();
            featureLogger.LogError(ex, ex.Message);
            var errorResult = FeatureResult<TOutput>.ServerError();
            errorResult.RequestId = contextAccessor.CorrelationId;
            return Results.Json(errorResult, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}