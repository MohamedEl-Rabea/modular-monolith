using WT.B2C.API.BuildingBlocks.Types;

namespace WT.B2C.API.BuildingBlocks.FeatureBehaviors;

public delegate Task<FeatureResult<TOutput>> FeatureHandlerDelegate<in TInput, TOutput>(
    TInput? request,
    CancellationToken cancellationToken
);

public interface IFeatureBehavior<TInput, TOutput>
{
    Task<FeatureResult<TOutput>> Handle(
        TInput? request,
        FeatureHandlerDelegate<TInput, TOutput> next,
        CancellationToken cancellationToken
    );
}
