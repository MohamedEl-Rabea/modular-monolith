using System.Transactions;
using WT.B2C.API.BuildingBlocks.Types;

namespace WT.B2C.API.BuildingBlocks.FeatureBehaviors;

public class TransactionBehavior<TInput, TOutput> : IFeatureBehavior<TInput, TOutput>
{
    public async Task<FeatureResult<TOutput>> Handle(
        TInput? request,
        FeatureHandlerDelegate<TInput, TOutput> next,
        CancellationToken cancellationToken)
    {
        if (!typeof(ICommandInput).IsAssignableFrom(typeof(TInput)))
        {
            return await next(request, cancellationToken);
        }

        using var scope = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            },
            TransactionScopeAsyncFlowOption.Enabled
        );

        var result = await next(request, cancellationToken);

        if (result.Succeeded)
        {
            scope.Complete();
        }

        return result;
    }
}