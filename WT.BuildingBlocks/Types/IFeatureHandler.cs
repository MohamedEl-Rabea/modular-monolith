namespace WT.Customers.Portal.BuildingBlocks.Types;

public interface IFeatureHandler<TInput, TOutputDto>
{
    Task<OperationResult<TOutputDto>> Handle(TInput createAuctionInputDto, CancellationToken cancellationToken);
}