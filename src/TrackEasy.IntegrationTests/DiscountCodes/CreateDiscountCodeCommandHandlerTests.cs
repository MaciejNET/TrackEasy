using FluentValidation;
using Shouldly;
using TrackEasy.Application.DiscountCodes.CreateDiscountCode;
using TrackEasy.Application.DiscountCodes.FindDiscountCode;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.IntegrationTests.DiscountCodes;

public class CreateDiscountCodeCommandHandlerTests(DatabaseFixture databaseFixture) : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task CreateDiscountCode_ValidDataProvided_ShouldCreateDiscountCode()
    {
        // Arrange
        const string code = "SUMMER2023";
        var command = new CreateDiscountCodeCommand(code, 20, new DateTime(2025, 3, 25), new DateTime(2025, 3, 27));

        // Act
        await Sender.Send(command);

        // Assert
        var discountCode = await Sender.Send(new FindDiscountCodeQuery(code));
        discountCode.ShouldNotBeNull();
        discountCode.Code.ShouldBe(code);
    }

    [Fact]
    public async Task CreateDiscountCode_InvalidDataProvided_ShouldThrowException()
    {
        // Arrange
        var command = new CreateDiscountCodeCommand("", -10, new DateTime(2025, 3, 22), new DateTime(2025, 3, 21));

        // Act
        var act = async () => await Sender.Send(command);

        // Assert
        await act.ShouldThrowAsync<ValidationException>();
    }
    
    [Fact]
    public async Task CreateDiscountCode_CodeAlreadyExists_ShouldThrowException()
    {
        // Arrange
        const string code = "SUMMER2023";
        var command = new CreateDiscountCodeCommand(code, 20, new DateTime(2025, 4, 25), new DateTime(2025, 4, 27));
        await Sender.Send(command);

        // Act
        var act = async () => await Sender.Send(command);

        // Assert
        await act.ShouldThrowAsync<TrackEasyException>();
    }
}