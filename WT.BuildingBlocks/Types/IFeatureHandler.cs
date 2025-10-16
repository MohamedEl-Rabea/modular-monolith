namespace WT.Customers.Portal.BuildingBlocks.Integration.Types;

public interface IFeatureHandler<TInput, TOutputDto>
{
    Task<OperationResult<TOutputDto>> Handle(TInput createAuctionInputDto, CancellationToken cancellationToken);
}