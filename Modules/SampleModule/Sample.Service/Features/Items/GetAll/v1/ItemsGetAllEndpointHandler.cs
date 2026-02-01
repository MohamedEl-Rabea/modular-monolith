using Sample.Service.Data.Queries;
using WT.B2C.API.BuildingBlocks.Types;

namespace Sample.Service.Features.Items.GetAll.v1;

public class ItemsGetAllEndpointHandler(ISampleItemsQueries itemsQueries)
    : IFeatureHandler<ItemsGetAllInputDto, ItemsGetAllOutputDto>
{
    public async Task<FeatureResult<ItemsGetAllOutputDto>> Handle(
        ItemsGetAllInputDto input,
        CancellationToken cancellationToken)
    {
        var items = await itemsQueries.GetAllItems(cancellationToken);

        var result = items.Select(i => new ItemDto(
            i.Id,
            i.Code,
            i.Name,
            i.IsActive)).ToList();

        return FeatureResult<ItemsGetAllOutputDto>.Success(new ItemsGetAllOutputDto(result));
    }
}
