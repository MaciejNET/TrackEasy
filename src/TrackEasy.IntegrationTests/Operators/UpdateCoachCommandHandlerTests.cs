using TrackEasy.Application.Operators.AddCoach;
using TrackEasy.Application.Operators.CreateOperator;
using TrackEasy.Application.Operators.FindCoach;
using TrackEasy.Application.Operators.GetCoaches;
using TrackEasy.Application.Operators.UpdateCoach;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.IntegrationTests.Operators;

public class UpdateCoachCommandHandlerTests(DatabaseFixture databaseFixture) : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task UpdateCoach_ValidRequest_ShouldUpdateCoach()
    {
        var createOperatorCommand = new CreateOperatorCommand("Test Operator", "TO");
        var operatorId = await Sender.Send(createOperatorCommand);
        
        var addCoachCommand = new AddCoachCommand(operatorId, "Test Coach", [1, 2, 3]);
        await Sender.Send(addCoachCommand);
        
        var coaches = await Sender.Send(new GetCoachesQuery(operatorId, null, 1, 10));
        var coachId = coaches.Items.FirstOrDefault()!.Id;
        
        var updateCoachCommand = new UpdateCoachCommand(coachId, operatorId, "Updated Coach", [4, 5, 6]);
        await Sender.Send(updateCoachCommand);
        
        var coach = await Sender.Send(new FindCoachQuery(coachId, operatorId));
        Assert.NotNull(coach);
        Assert.Equal("Updated Coach", coach.Code);
        Assert.Equal(3, coach.SeatsNumbers.Count());
        Assert.Contains(4, coach.SeatsNumbers);
    }
    
    [Fact]
    public async Task UpdateCoach_OperatorNotFound_ShouldThrowException()
    {
        var updateCoachCommand = new UpdateCoachCommand(Guid.NewGuid(), Guid.NewGuid(), "Updated Coach", [4, 5, 6]);
        
        await Assert.ThrowsAsync<TrackEasyException>(() => Sender.Send(updateCoachCommand));
    }
}