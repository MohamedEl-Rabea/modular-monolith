namespace Sample.Service.Features.Items.GetAll.v1;

public record ItemsGetAllOutputDto(IReadOnlyList<ItemDto> Items);

public record ItemDto(int Id, string Code, string Name, bool IsActive);
