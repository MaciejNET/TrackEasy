using Shouldly;
using TrackEasy.Application.Operators.AddCoach;
using TrackEasy.Application.Operators.AddTrain;
using TrackEasy.Application.Operators.CreateOperator;
using TrackEasy.Application.Operators.FindTrain;
using TrackEasy.Application.Operators.GetCoaches;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.IntegrationTests.Operators;

public class AddTrainCommandHandlerTests(DatabaseFixture databaseFixture) : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task AddTrain_ValidRequest_ShouldAddTrain()
    {
        var operatorId = await Sender.Send(new CreateOperatorCommand("Test Operator", "TO"));

        await Sender.Send(new AddCoachCommand(operatorId, "C11", [1, 2]));
        await Sender.Send(new AddCoachCommand(operatorId, "C21", [1, 2]));

        var coaches = await Sender.Send(new GetCoachesQuery(operatorId, null, 1, 10));
        var coach1 = coaches.Items.First(c => c.Code == "C11");
        var coach2 = coaches.Items.First(c => c.Code == "C21");

        var command = new AddTrainCommand(operatorId, "Express", [(coach1.Id, 1), (coach2.Id, 2)]);
        var trainId = await Sender.Send(command);

        var train = await Sender.Send(new FindTrainQuery(operatorId, trainId));
        train.ShouldNotBeNull();
        train!.Name.ShouldBe("Express");
        train.Coaches.Count().ShouldBe(2);
        train.Coaches.ShouldContain(c => c.Coach.Id == coach1.Id && c.Number == 1);
        train.Coaches.ShouldContain(c => c.Coach.Id == coach2.Id && c.Number == 2);
    }

    [Fact]
    public async Task AddTrain_OperatorNotFound_ShouldThrowException()
    {
        var command = new AddTrainCommand(Guid.NewGuid(), "Express", []);

        await Assert.ThrowsAsync<TrackEasyException>(() => Sender.Send(command));
    }
}
