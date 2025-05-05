using Shouldly;
using TrackEasy.Application.Operators.CreateOperator;
using TrackEasy.Application.Operators.FindOperator;
using TrackEasy.Application.Operators.UpdateOperator;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.IntegrationTests.Operators;

public class UpdateOperatorCommandHandlerTests(DatabaseFixture databaseFixture) : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task UpdateOperator_ValidData_ShouldUpdateOperator()
    {
        // Arrange
        var createCommand = new CreateOperatorCommand("Test Operator", "TO");
        var operatorId = await Sender.Send(createCommand);
        
        var updateCommand = new UpdateOperatorCommand(operatorId, "Updated Operator", "UO");

        // Act
        await Sender.Send(updateCommand);

        // Assert
        var @operator = await Sender.Send(new FindOperatorQuery(operatorId));
        @operator.ShouldNotBeNull();
        @operator!.Name.ShouldBe(updateCommand.Name);
        @operator.Code.ShouldBe(updateCommand.Code);
    }
    
    [Fact]
    public async Task UpdateOperator_OperatorNotFound_ShouldThrowValidationException()
    {
        // Arrange
        var updateCommand = new UpdateOperatorCommand(Guid.NewGuid(), "Updated Operator", "UO");

        // Act & Assert
        var exception = await Should.ThrowAsync<TrackEasyException>(async () =>
            await Sender.Send(updateCommand));

        exception.Code.ShouldBe(SharedCodes.EntityNotFound);
        exception.Message.ShouldContain("does not exists");
    }
}