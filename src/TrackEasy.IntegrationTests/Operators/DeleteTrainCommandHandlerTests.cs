using TrackEasy.Application.Operators.AddCoach;
using TrackEasy.Application.Operators.AddTrain;
using TrackEasy.Application.Operators.CreateOperator;
using TrackEasy.Application.Operators.DeleteTrain;
using TrackEasy.Application.Operators.GetCoaches;
using TrackEasy.Application.Operators.GetTrains;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.IntegrationTests.Operators;

public class DeleteTrainCommandHandlerTests(DatabaseFixture databaseFixture) : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task DeleteTrain_ValidRequest_ShouldDeleteTrain()
    {
        var operatorId = await Sender.Send(new CreateOperatorCommand("Test Operator", "TO"));
        await Sender.Send(new AddCoachCommand(operatorId, "C1", [1, 2]));

        var coach = (await Sender.Send(new GetCoachesQuery(operatorId, null, 1, 10))).Items.First();
        var trainId = await Sender.Send(new AddTrainCommand(operatorId, "Express", [(coach.Id, 1)]));

        await Sender.Send(new DeleteTrainCommand(trainId, operatorId));

        var trains = await Sender.Send(new GetTrainsQuery(operatorId, null, 1, 10));
        Assert.Empty(trains.Items);
    }

    [Fact]
    public async Task DeleteTrain_OperatorNotFound_ShouldThrowException()
    {
        var command = new DeleteTrainCommand(Guid.NewGuid(), Guid.NewGuid());

        await Assert.ThrowsAsync<TrackEasyException>(() => Sender.Send(command));
    }
}
