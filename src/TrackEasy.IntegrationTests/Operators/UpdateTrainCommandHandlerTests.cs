using Shouldly;
using TrackEasy.Application.Operators.AddCoach;
using TrackEasy.Application.Operators.AddTrain;
using TrackEasy.Application.Operators.CreateOperator;
using TrackEasy.Application.Operators.FindTrain;
using TrackEasy.Application.Operators.GetCoaches;
using TrackEasy.Application.Operators.UpdateTrain;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.IntegrationTests.Operators;

public class UpdateTrainCommandHandlerTests(DatabaseFixture databaseFixture) : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task UpdateTrain_ValidRequest_ShouldUpdateTrain()
    {
        var operatorId = await Sender.Send(new CreateOperatorCommand("Test Operator", "TO"));

        await Sender.Send(new AddCoachCommand(operatorId, "C11", [1, 2]));
        await Sender.Send(new AddCoachCommand(operatorId, "C21", [1, 2]));
        await Sender.Send(new AddCoachCommand(operatorId, "C31", [1, 2]));

        var coaches = await Sender.Send(new GetCoachesQuery(operatorId, null, 1, 10));
        var coach1 = coaches.Items.First(c => c.Code == "C11");
        var coach2 = coaches.Items.First(c => c.Code == "C21");
        var coach3 = coaches.Items.First(c => c.Code == "C31");

        var trainId = await Sender.Send(new AddTrainCommand(operatorId, "Express", [(coach1.Id, 1), (coach2.Id, 2)]));

        var updateCommand = new UpdateTrainCommand(operatorId, trainId, "Updated", [(coach2.Id, 1), (coach3.Id, 2)]);
        await Sender.Send(updateCommand);

        var train = await Sender.Send(new FindTrainQuery(operatorId, trainId));
        train.ShouldNotBeNull();
        train!.Name.ShouldBe("Updated");
        train.Coaches.Count().ShouldBe(2);
        train.Coaches.ShouldContain(c => c.Coach.Id == coach2.Id && c.Number == 1);
        train.Coaches.ShouldContain(c => c.Coach.Id == coach3.Id && c.Number == 2);
    }

    [Fact]
    public async Task UpdateTrain_OperatorNotFound_ShouldThrowException()
    {
        var command = new UpdateTrainCommand(Guid.NewGuid(), Guid.NewGuid(), "Express", []);

        await Assert.ThrowsAsync<TrackEasyException>(() => Sender.Send(command));
    }
}
