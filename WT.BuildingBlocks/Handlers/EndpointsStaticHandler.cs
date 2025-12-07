using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WT.Customers.Portal.BuildingBlocks.Extensions;
using WT.Customers.Portal.BuildingBlocks.Runtime;
using WT.Customers.Portal.BuildingBlocks.Types;

namespace WT.Customers.Portal.BuildingBlocks.Handlers;

public static class EndpointsStaticHandler
{
    public static async Task<IResult> HandleEndpoint<TOutput>(HttpContext ctx, CancellationToken cancellationToken)
    {
        return await Handle<NoInput, TOutput>(ctx, null, cancellationToken);
    }


    public static async Task<IResult> HandleEndpoint<TInput, TOutput>(HttpContext ctx, TInput request,
        CancellationToken cancellationToken)
    {
        return await Handle<TInput, TOutput>(ctx, request, cancellationToken);
    }

    private static async Task<IResult> Handle<TInput, TOutput>(HttpContext ctx, TInput? request,
        CancellationToken cancellationToken)
    {
        var contextAccessor = ctx.RequestServices.GetRequiredService<IExecutionContextAccessor>();
        try
        {
            if (request != null)
            {
                var validationResult = await request.ValidateAsync(ctx.RequestServices);
                var errorsArray = validationResult.ToArray();
                if (errorsArray.Any())
                {
                    var errorResult = OperationResult<TOutput>.BadRequest(errorsArray);
                    errorResult.RequestId = contextAccessor.CorrelationId;
                    return Results.Json(errorResult, statusCode: StatusCodes.Status400BadRequest);
                }
            }

            var featureHandler = ctx.RequestServices.GetRequiredService<IFeatureHandler<TInput, TOutput>>();
            var result = await featureHandler.Handle(request, cancellationToken);
            result.RequestId = contextAccessor.CorrelationId;
            return Results.Json(result, statusCode: result.StatusCode);
        }
        catch (Exception ex)
        {
            var featureLogger = ctx.RequestServices.GetRequiredService<ILogger<IFeatureHandler<TInput, TOutput>>>();
            featureLogger.LogError(ex, ex.Message);
            var errorResult = OperationResult<TOutput>.ServerError();
            errorResult.RequestId = contextAccessor.CorrelationId;
            return Results.Json(errorResult, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}