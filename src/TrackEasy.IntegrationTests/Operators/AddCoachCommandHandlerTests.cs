using TrackEasy.Application.Operators.AddCoach;
using TrackEasy.Application.Operators.CreateOperator;
using TrackEasy.Application.Operators.FindCoach;
using TrackEasy.Application.Operators.GetCoaches;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.IntegrationTests.Operators;

public class AddCoachCommandHandlerTests(DatabaseFixture databaseFixture) : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task AddCoach_ValidRequest_ShouldAddCoach()
    {
        var createOperatorCommand = new CreateOperatorCommand("Test Operator", "TO");
        var operatorId = await Sender.Send(createOperatorCommand);
        
        var addCoachCommand = new AddCoachCommand(operatorId, "Test Coach", [1, 2, 3]);
        await Sender.Send(addCoachCommand);
        
        var coaches = await Sender.Send(new GetCoachesQuery(operatorId, null, 1, 10));
        var coachId = coaches.Items.FirstOrDefault()!.Id;
        
        var coach = await Sender.Send(new FindCoachQuery(coachId, operatorId));
        Assert.NotNull(coach);
        Assert.Equal("Test Coach", coach.Code);
        Assert.Equal(3, coach.SeatsNumbers.Count());
    }
    
    [Fact]
    public async Task AddCoach_OperatorNotFound_ShouldThrowException()
    {
        var addCoachCommand = new AddCoachCommand(Guid.NewGuid(), "Test Coach", [1, 2, 3]);
        
        await Assert.ThrowsAsync<TrackEasyException>(() => Sender.Send(addCoachCommand));
    }
}