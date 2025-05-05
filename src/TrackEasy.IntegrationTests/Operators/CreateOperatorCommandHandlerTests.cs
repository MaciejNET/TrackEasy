using Shouldly;
using TrackEasy.Application.Operators;
using TrackEasy.Application.Operators.CreateOperator;
using TrackEasy.Application.Operators.FindOperator;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.IntegrationTests.Operators;

public class CreateOperatorCommandHandlerTests(DatabaseFixture databaseFixture) : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task CreateOperator_ValidName_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreateOperatorCommand("Test Operator", "TO");

        // Act
        var id = await Sender.Send(command);

        // Assert
        var @operator = await Sender.Send(new FindOperatorQuery(id));
        @operator.ShouldNotBeNull();
        @operator!.Name.ShouldBe(command.Name);
        @operator.Code.ShouldBe(command.Code);
    }
    
    [Fact]
    public async Task CreateOperator_OperatorAlreadyExists_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreateOperatorCommand("Test Operator", "TO");
        await Sender.Send(command);

        // Act & Assert
        var exception = await Should.ThrowAsync<TrackEasyException>(async () =>
            await Sender.Send(command));

        exception.Code.ShouldBe(Codes.OperatorAlreadyExists);
        exception.Message.ShouldContain("already exists");
    }
    
    [Fact]
    public async Task CreateOperator_InvalidName_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreateOperatorCommand("", "TO");

        // Act & Assert
        await Should.ThrowAsync<FluentValidation.ValidationException>(async () =>
            await Sender.Send(command));
    }
}