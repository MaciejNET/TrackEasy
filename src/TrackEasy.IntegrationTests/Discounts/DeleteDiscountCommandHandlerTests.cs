using Shouldly;
using TrackEasy.Application.Discounts.CreateDiscount;
using TrackEasy.Application.Discounts.DeleteDiscount;
using TrackEasy.Application.Discounts.FindDiscount;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.IntegrationTests.Discounts;

public class DeleteDiscountCommandHandlerTests(DatabaseFixture databaseFixture) : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task DeleteDiscount_ValidDataProvided_ShouldDeleteDiscount()
    {
        // Arrange
        const string name = "Student";
        var command = new CreateDiscountCommand(name, 51);
        var id = await Sender.Send(command);

        var deleteCommand = new DeleteDiscountCommand(id);

        // Act
        await Sender.Send(deleteCommand);

        // Assert
        var discount = await Sender.Send(new FindDiscountQuery(id));
        discount.ShouldBeNull();
    }
    
    [Fact]
    public async Task DeleteDiscount_InvalidDataProvided_ShouldThrowException()
    {
        // Arrange
        var deleteCommand = new DeleteDiscountCommand(Guid.NewGuid());

        // Act
        var act = async () => await Sender.Send(deleteCommand);

        // Assert
        await act.ShouldThrowAsync<TrackEasyException>();
    }
}