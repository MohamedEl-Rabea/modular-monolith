using FluentAssertions;
using Moq;
using Sample.Service.Data.Queries;
using Sample.Service.Features.Items.GetAll.v1;
using Sample.Service.Models;

namespace Sample.Service.Tests.Features.Items.GetAll;

public class ItemsGetAllEndpointHandlerTests
{
    private Mock<ISampleItemsQueries> _itemsQueriesMock = null!;
    private ItemsGetAllEndpointHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        _itemsQueriesMock = new Mock<ISampleItemsQueries>();
        _handler = new ItemsGetAllEndpointHandler(_itemsQueriesMock.Object);
    }

    [Test]
    public async Task Handle_Should_Return_Items()
    {
        var items = new List<SampleItem>
        {
            new() { Id = 1, Code = "S1", Name = "Sample 1", IsActive = true }
        };

        _itemsQueriesMock
            .Setup(x => x.GetAllItems(It.IsAny<CancellationToken>()))
            .ReturnsAsync(items);

        var result = await _handler.Handle(new ItemsGetAllInputDto(), CancellationToken.None);

        result.Succeeded.Should().BeTrue();
        result.Data!.Items.Should().HaveCount(1);
    }
}
