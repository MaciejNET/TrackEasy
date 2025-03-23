using Shouldly;
using TrackEasy.Application.DiscountCodes.CreateDiscountCode;
using TrackEasy.Application.DiscountCodes.FindDiscountCode;
using TrackEasy.Application.DiscountCodes.UpdateDiscountCode;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.IntegrationTests.DiscountCodes;

public class UpdateDiscountCodeCommandHandlerTests(DatabaseFixture databaseFixture)
    : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task UpdateDiscountCode_ValidDataProvided_ShouldUpdateDiscountCode()
    {
        // Arrange
        const string code = "SUMMER2023";
        var createCommand =
            new CreateDiscountCodeCommand(code, 20, new DateTime(2025, 3, 25), new DateTime(2025, 3, 27));
        await Sender.Send(createCommand);

        var discountCode = await Sender.Send(new FindDiscountCodeQuery(code));
        discountCode.ShouldNotBeNull();

        var updateCommand =
            new UpdateDiscountCodeCommand(discountCode.Id, 30, new DateTime(2025, 3, 26), new DateTime(2025, 3, 28));

        // Act
        await Sender.Send(updateCommand);

        // Assert
        var updatedDiscountCode = await Sender.Send(new FindDiscountCodeQuery(code));
        updatedDiscountCode.ShouldNotBeNull();
        updatedDiscountCode.Percentage.ShouldBe(30);
        updatedDiscountCode.From.ShouldBe(new DateTime(2025, 3, 26));
        updatedDiscountCode.To.ShouldBe(new DateTime(2025, 3, 28));
    }

    [Fact]
    public async Task UpdateDiscountCode_InvalidIdProvided_ShouldThrowException()
    {
        // Arrange
        var updateCommand =
            new UpdateDiscountCodeCommand(Guid.NewGuid(), 30, new DateTime(2025, 3, 26), new DateTime(2025, 3, 28));

        // Act
        var act = async () => await Sender.Send(updateCommand);

        // Assert
        await act.ShouldThrowAsync<TrackEasyException>();
    }
}