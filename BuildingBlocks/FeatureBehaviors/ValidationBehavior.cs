using WT.B2C.API.BuildingBlocks.Extensions;
using WT.B2C.API.BuildingBlocks.Types;

namespace WT.B2C.API.BuildingBlocks.FeatureBehaviors;

public class ValidationBehavior<TInput, TOutput>(IServiceProvider sp) : IFeatureBehavior<TInput, TOutput>
{
    public async Task<FeatureResult<TOutput>> Handle(
        TInput? request,
        FeatureHandlerDelegate<TInput, TOutput> next,
        CancellationToken cancellationToken)
    {
        if (request is null)
            return await next(request, cancellationToken);

        var errors = (await request.ValidateAsync(sp)).ToArray();
        if (errors.Any())
        {
            return FeatureResult<TOutput>.BadRequest(errors);
        }

        return await next(request, cancellationToken);
    }
}
