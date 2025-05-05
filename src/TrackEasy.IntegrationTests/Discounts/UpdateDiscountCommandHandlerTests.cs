using Shouldly;
using TrackEasy.Application.Discounts.CreateDiscount;
using TrackEasy.Application.Discounts.FindDiscount;
using TrackEasy.Application.Discounts.UpdateDiscount;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.IntegrationTests.Discounts;

public class UpdateDiscountCommandHandlerTests(DatabaseFixture databaseFixture) : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task UpdateDiscount_ValidDataProvided_ShouldUpdateDiscount()
    {
        // Arrange
        const string name = "Student";
        var command = new CreateDiscountCommand(name, 51);
        var id = await Sender.Send(command);

        var updateCommand = new UpdateDiscountCommand(id, "Student", 20);

        // Act
        await Sender.Send(updateCommand);

        // Assert
        var discount = await Sender.Send(new FindDiscountQuery(id));
        discount.ShouldNotBeNull();
        discount.Name.ShouldBe("Student");
        discount.Percentage.ShouldBe(20);
    }
    
    [Fact]
    public async Task UpdateDiscount_InvalidDataProvided_ShouldThrowException()
    {
        // Arrange
        var command = new CreateDiscountCommand("Student", 51);
        var id = await Sender.Send(command);

        var updateCommand = new UpdateDiscountCommand(id, "", -10);

        // Act
        var act = async () => await Sender.Send(updateCommand);

        // Assert
        await act.ShouldThrowAsync<FluentValidation.ValidationException>();
    }
    
    [Fact]
    public async Task UpdateDiscount_DiscountDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var updateCommand = new UpdateDiscountCommand(Guid.NewGuid(), "Student", 20);

        // Act
        var act = async () => await Sender.Send(updateCommand);

        // Assert
        await act.ShouldThrowAsync<TrackEasyException>();
    }
}