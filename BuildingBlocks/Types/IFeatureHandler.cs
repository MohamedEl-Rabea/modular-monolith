namespace WT.B2C.API.BuildingBlocks.Types;

public interface IFeatureHandler<TInput, TOutputDto>
{
    Task<FeatureResult<TOutputDto>> Handle(TInput createAuctionInputDto, CancellationToken cancellationToken);
}