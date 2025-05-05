using Shouldly;
using TrackEasy.Application.Operators.CreateOperator;
using TrackEasy.Application.Operators.DeleteOperator;
using TrackEasy.Application.Operators.FindOperator;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.IntegrationTests.Operators;

public class DeleteOperatorCommandHandler(DatabaseFixture databaseFixture) : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task DeleteOperator_ValidId_ShouldDeleteOperator()
    {
        // Arrange
        var createCommand = new CreateOperatorCommand("Test Operator", "TO");
        var operatorId = await Sender.Send(createCommand);
        
        var deleteCommand = new DeleteOperatorCommand(operatorId);

        // Act
        await Sender.Send(deleteCommand);

        // Assert
        var @operator = await Sender.Send(new FindOperatorQuery(operatorId));
        @operator.ShouldBeNull();
    }
    
    [Fact]
    public async Task DeleteOperator_OperatorNotFound_ShouldThrowValidationException()
    {
        // Arrange
        var deleteCommand = new DeleteOperatorCommand(Guid.NewGuid());

        // Act & Assert
        var exception = await Should.ThrowAsync<TrackEasyException>(async () =>
            await Sender.Send(deleteCommand));

        exception.Code.ShouldBe(SharedCodes.EntityNotFound);
        exception.Message.ShouldContain("does not exists");
    }
}