using TrackEasy.Application.Operators.AddCoach;
using TrackEasy.Application.Operators.CreateOperator;
using TrackEasy.Application.Operators.DeleteCoach;
using TrackEasy.Application.Operators.GetCoaches;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.IntegrationTests.Operators;

public class DeleteCoachCommandHandlerTests(DatabaseFixture databaseFixture) : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task DeleteCoach_ValidRequest_ShouldDeleteCoach()
    {
        var createOperatorCommand = new CreateOperatorCommand("Test Operator", "TO");
        var operatorId = await Sender.Send(createOperatorCommand);
        
        var addCoachCommand = new AddCoachCommand(operatorId, "Test Coach", [1, 2, 3]);
        await Sender.Send(addCoachCommand);
        
        var coaches = await Sender.Send(new GetCoachesQuery(operatorId, null, 1, 10));
        var coachId = coaches.Items.FirstOrDefault()!.Id;
        
        await Sender.Send(new DeleteCoachCommand(coachId, operatorId));
        
        coaches = await Sender.Send(new GetCoachesQuery(operatorId, null, 1, 10));
        Assert.Empty(coaches.Items);
    }
    
    [Fact]
    public async Task DeleteCoach_OperatorNotFound_ShouldThrowException()
    {
        var deleteCoachCommand = new DeleteCoachCommand(Guid.NewGuid(), Guid.NewGuid());
        
        await Assert.ThrowsAsync<TrackEasyException>(() => Sender.Send(deleteCoachCommand));
    }
}