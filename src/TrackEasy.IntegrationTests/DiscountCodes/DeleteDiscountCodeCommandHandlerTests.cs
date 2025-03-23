using Shouldly;
using TrackEasy.Application.DiscountCodes.CreateDiscountCode;
using TrackEasy.Application.DiscountCodes.DeleteDiscountCode;
using TrackEasy.Application.DiscountCodes.FindDiscountCode;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.IntegrationTests.DiscountCodes;

public class DeleteDiscountCodeCommandHandlerTests(DatabaseFixture databaseFixture) : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task DeleteDiscountCode_ValidIdProvided_ShouldDeleteDiscountCode()
    {
        // Arrange
        const string code = "SUMMER2023";
        var createCommand = new CreateDiscountCodeCommand(code, 20, new DateTime(2025, 3, 25), new DateTime(2025, 3, 27));
        await Sender.Send(createCommand);

        var discountCode = await Sender.Send(new FindDiscountCodeQuery(code));
        discountCode.ShouldNotBeNull();

        var deleteCommand = new DeleteDiscountCodeCommand(discountCode.Id);

        // Act
        await Sender.Send(deleteCommand);

        // Assert
        var deletedDiscountCode = await Sender.Send(new FindDiscountCodeQuery(code));
        deletedDiscountCode.ShouldBeNull();
    }

    [Fact]
    public async Task DeleteDiscountCode_InvalidIdProvided_ShouldThrowException()
    {
        // Arrange
        var deleteCommand = new DeleteDiscountCodeCommand(Guid.NewGuid());

        // Act
        var act = async () => await Sender.Send(deleteCommand);

        // Assert
        await act.ShouldThrowAsync<TrackEasyException>();
    }
}