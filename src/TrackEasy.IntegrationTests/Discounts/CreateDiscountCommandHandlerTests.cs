using Shouldly;
using TrackEasy.Application.Discounts.CreateDiscount;
using TrackEasy.Application.Discounts.FindDiscount;

namespace TrackEasy.IntegrationTests.Discounts;

public class CreateDiscountCommandHandlerTests(DatabaseFixture databaseFixture) : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task CreateDiscount_ValidDataProvided_ShouldCreateDiscount()
    {
        // Arrange
        const string name = "Student";
        var command = new CreateDiscountCommand(name, 51);

        // Act
        var id = await Sender.Send(command);

        // Assert
        var discount = await Sender.Send(new FindDiscountQuery(id));
        discount.ShouldNotBeNull();
        discount.Name.ShouldBe(name);
        discount.Percentage.ShouldBe(51);
    }
    
    [Fact]
    public async Task CreateDiscount_InvalidDataProvided_ShouldThrowException()
    {
        // Arrange
        var command = new CreateDiscountCommand("", -10);

        // Act
        var act = async () => await Sender.Send(command);

        // Assert
        await act.ShouldThrowAsync<FluentValidation.ValidationException>();
    }
    
    [Fact]
    public async Task CreateDiscount_DiscountAlreadyExists_ShouldThrowException()
    {
        // Arrange
        const string name = "Student";
        var command = new CreateDiscountCommand(name, 51);
        await Sender.Send(command);

        // Act
        var act = async () => await Sender.Send(command);

        // Assert
        await act.ShouldThrowAsync<Shared.Exceptions.TrackEasyException>();
    }
}