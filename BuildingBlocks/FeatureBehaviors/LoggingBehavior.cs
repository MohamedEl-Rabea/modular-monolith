using Microsoft.Extensions.Logging;
using WT.B2C.API.BuildingBlocks.Types;

namespace WT.B2C.API.BuildingBlocks.FeatureBehaviors;

public class LoggingBehavior<TInput, TOutput>(ILogger<LoggingBehavior<TInput, TOutput>> logger)
    : IFeatureBehavior<TInput, TOutput>
{
    public async Task<FeatureResult<TOutput>> Handle(
        TInput? request,
        FeatureHandlerDelegate<TInput, TOutput> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TInput).Name;
        logger.LogInformation("Handling {RequestName}", requestName);

        var sw = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            var response = await next(request, cancellationToken);
            sw.Stop();

            logger.LogInformation(
                "Handled {RequestName} with status {StatusCode} in {Elapsed} ms",
                requestName,
                response.StatusCode,
                sw.ElapsedMilliseconds
            );

            return response;
        }
        catch (Exception ex)
        {
            sw.Stop();
            logger.LogError(ex,
                "Exception while handling {RequestName} after {Elapsed} ms",
                requestName,
                sw.ElapsedMilliseconds);

            throw;
        }
    }
}