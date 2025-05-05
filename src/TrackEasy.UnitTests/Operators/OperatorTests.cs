using FluentValidation;
using Microsoft.Extensions.Time.Testing;
using Shouldly;
using TrackEasy.Domain.Operators;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.UnitTests.Operators;

public class OperatorTests
{
    private readonly FakeTimeProvider _fakeTimeProvider = new();

    private static Operator CreateValidOperator() =>
        Operator.Create("Test Operator", "TO");

    private static IEnumerable<int> ValidSeatNumbers(int count = 5) => Enumerable.Range(1, count);

    [Fact]
    public void Create_Should_CreateOperator_For_ValidData()
    {
        // Arrange
        const string validName = "Valid Operator Name";
        const string validCode = "VON";

        // Act
        var @operator = Operator.Create(validName, validCode);

        // Assert
        @operator.ShouldNotBeNull();
        @operator.Id.ShouldNotBe(Guid.Empty);
        @operator.Name.ShouldBe(validName);
        @operator.Code.ShouldBe(validCode);
        @operator.Trains.ShouldBeEmpty();
        @operator.Coaches.ShouldBeEmpty();
        @operator.Managers.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("AB")] // Too short
    [InlineData("This name is definitely longer than the fifty characters limit allowed")] // Too long
    public void Create_Should_ThrowValidationException_For_InvalidName(string invalidName)
    {
        // Arrange
        const string validCode = "VC";

        // Act & Assert
        Should.Throw<ValidationException>(() =>
            Operator.Create(invalidName, validCode));
    }

    [Theory]
    [InlineData("")]
    [InlineData("A")] // Too short
    [InlineData("ABCD")] // Too long
    public void Create_Should_ThrowValidationException_For_InvalidCode(string invalidCode)
    {
        // Arrange
        const string validName = "Valid Name";

        // Act & Assert
        Should.Throw<ValidationException>(() =>
            Operator.Create(validName, invalidCode));
    }

    [Fact]
    public void Update_Should_UpdateNameAndCode_For_ValidData()
    {
        // Arrange
        var @operator = CreateValidOperator();
        const string newName = "Updated Operator Name";
        const string newCode = "UON";

        // Act
        @operator.Update(newName, newCode);

        // Assert
        @operator.Name.ShouldBe(newName);
        @operator.Code.ShouldBe(newCode);
    }

    [Theory]
    [InlineData("")]
    [InlineData("XY")] // Too short
    [InlineData("This updated name is also way too long for the fifty character limit")] // Too long
    [InlineData(null)]
    public void Update_Should_ThrowValidationException_For_InvalidName(string invalidName)
    {
        // Arrange
        var @operator = CreateValidOperator();
        const string validCode = "VC";

        // Act & Assert
        Should.Throw<ValidationException>(() =>
            @operator.Update(invalidName, validCode));
    }

    [Theory]
    [InlineData("")]
    [InlineData("Z")] // Too short
    [InlineData("WXYZ")] // Too long
    [InlineData(null)]
    public void Update_Should_ThrowValidationException_For_InvalidCode(string invalidCode)
    {
        // Arrange
        var @operator = CreateValidOperator();
        const string validName = "Valid Name";

        // Act & Assert
        Should.Throw<ValidationException>(() =>
            @operator.Update(validName, invalidCode));
    }

    [Fact]
    public void AddCoach_Should_AddCoach_For_ValidData()
    {
        // Arrange
        var @operator = CreateValidOperator();
        const string coachCode = "C-001";
        var seatNumbers = ValidSeatNumbers(10);

        // Act
        @operator.AddCoach(coachCode, seatNumbers);

        // Assert
        @operator.Coaches.Count.ShouldBe(1);
        var addedCoach = @operator.Coaches[0];
        addedCoach.Code.ShouldBe(coachCode);
        addedCoach.Seats.Count.ShouldBe(10);
        addedCoach.OperatorId.ShouldBe(@operator.Id);
    }

    [Fact]
    public void AddCoach_Should_ThrowTrackEasyException_For_DuplicateCode()
    {
        // Arrange
        var @operator = CreateValidOperator();
        const string coachCode = "C-001";
        @operator.AddCoach(coachCode, ValidSeatNumbers(5)); // Add first coach

        // Act & Assert
        var ex = Should.Throw<TrackEasyException>(() =>
            @operator.AddCoach(coachCode, ValidSeatNumbers(10))); // Try adding another with same code

        ex.Code.ShouldBe(Codes.CoachCodeAlreadyExists);
    }

    [Fact]
    public void UpdateCoach_Should_UpdateCoach_For_ValidData()
    {
        // Arrange
        var @operator = CreateValidOperator();
        const string initialCode = "IC-01";
        @operator.AddCoach(initialCode, ValidSeatNumbers(5));
        var coachToUpdate = @operator.Coaches[0];
        const string newCode = "UC-02";
        var newSeatNumbers = ValidSeatNumbers(8);

        // Act
        @operator.UpdateCoach(coachToUpdate.Id, newCode, newSeatNumbers);

        // Assert
        coachToUpdate.Code.ShouldBe(newCode);
        coachToUpdate.Seats.Count.ShouldBe(8);
    }

    [Fact]
    public void UpdateCoach_Should_ThrowTrackEasyException_When_CoachNotFound()
    {
        // Arrange
        var @operator = CreateValidOperator();
        var nonExistentCoachId = Guid.NewGuid();

        // Act & Assert
        var ex = Should.Throw<TrackEasyException>(() =>
            @operator.UpdateCoach(nonExistentCoachId, "NewCode", ValidSeatNumbers()));

        ex.Code.ShouldBe(Codes.CoachNotFound);
    }

    [Fact]
    public void AddTrain_Should_AddTrain_For_ValidData()
    {
        // Arrange
        var @operator = CreateValidOperator();
        @operator.AddCoach("CO1", ValidSeatNumbers(5));
        @operator.AddCoach("CO2", ValidSeatNumbers(10));
        var coach1 = @operator.Coaches.First(c => c.Code == "CO1");
        var coach2 = @operator.Coaches.First(c => c.Code == "CO2");
        const string trainName = "Express 100";
        var coachesInfo = new List<(Guid CoachId, int Number)>
        {
            (coach1.Id, 1),
            (coach2.Id, 2)
        };

        // Act
        @operator.AddTrain(trainName, coachesInfo);

        // Assert
        @operator.Trains.Count.ShouldBe(1);
        var addedTrain = @operator.Trains[0];
        addedTrain.Name.ShouldBe(trainName);
        addedTrain.Coaches.Count.ShouldBe(2);
        addedTrain.Coaches.ShouldContain(tc => tc.Coach.Id == coach1.Id && tc.Number == 1);
        addedTrain.Coaches.ShouldContain(tc => tc.Coach.Id == coach2.Id && tc.Number == 2);
    }

    [Fact]
    public void AddTrain_Should_ThrowTrackEasyException_For_DuplicateName()
    {
        // Arrange
        var @operator = CreateValidOperator();
        @operator.AddCoach("CO1", ValidSeatNumbers(5));
        var coach1 = @operator.Coaches[0];
        const string trainName = "Express 100";
        @operator.AddTrain(trainName, new List<(Guid CoachId, int Number)> { (coach1.Id, 1) }); // Add first train

        // Act & Assert
        var ex = Should.Throw<TrackEasyException>(() =>
            @operator.AddTrain(trainName,
                new List<(Guid CoachId, int Number)> { (coach1.Id, 2) })); // Try adding another with same name

        ex.Code.ShouldBe(Codes.TrainNameAlreadyExists);
    }

    [Fact]
    public void AddTrain_Should_ThrowTrackEasyException_When_CoachNotFound()
    {
        // Arrange
        var @operator = CreateValidOperator();
        var nonExistentCoachId = Guid.NewGuid();
        const string trainName = "Express 200";
        var coachesInfo = new List<(Guid CoachId, int Number)> { (nonExistentCoachId, 1) };

        // Act & Assert
        var ex = Should.Throw<TrackEasyException>(() =>
            @operator.AddTrain(trainName, coachesInfo));

        ex.Code.ShouldBe(Codes.CoachNotFound);
    }

    [Fact]
    public void AddTrain_Should_ThrowValidationException_When_TrainValidationFails()
    {
        // Arrange
        var @operator = CreateValidOperator();
        @operator.AddCoach("CO1", ValidSeatNumbers(5));
        var coach1 = @operator.Coaches[0];
        const string invalidTrainName = ""; // Invalid name for Train
        var coachesInfo = new List<(Guid CoachId, int Number)> { (coach1.Id, 1) };

        // Act & Assert
        Should.Throw<ValidationException>(() =>
            @operator.AddTrain(invalidTrainName, coachesInfo));
    }

    [Fact]
    public void UpdateTrain_Should_UpdateTrain_For_ValidData()
    {
        // Arrange
        var @operator = CreateValidOperator();
        @operator.AddCoach("CO1", ValidSeatNumbers(5));
        @operator.AddCoach("CO2", ValidSeatNumbers(10));
        @operator.AddCoach("CO3", ValidSeatNumbers(8));
        var coach1 = @operator.Coaches.First(c => c.Code == "CO1");
        var coach2 = @operator.Coaches.First(c => c.Code == "CO2");
        var coach3 = @operator.Coaches.First(c => c.Code == "CO3");
        @operator.AddTrain("Old Train", new List<(Guid CoachId, int Number)> { (coach1.Id, 1) });
        var trainToUpdate = @operator.Trains[0];
        const string newName = "New Express";
        var newCoachesInfo = new List<(Guid CoachId, int Number)>
        {
            (coach2.Id, 1),
            (coach3.Id, 2)
        };

        // Act
        @operator.UpdateTrain(trainToUpdate.Id, newName, newCoachesInfo);

        // Assert
        trainToUpdate.Name.ShouldBe(newName);
        trainToUpdate.Coaches.Count.ShouldBe(2);
        trainToUpdate.Coaches.ShouldContain(tc => tc.Coach.Id == coach2.Id && tc.Number == 1);
        trainToUpdate.Coaches.ShouldContain(tc => tc.Coach.Id == coach3.Id && tc.Number == 2);
        trainToUpdate.Coaches.ShouldNotContain(tc => tc.Coach.Id == coach1.Id);
    }

    [Fact]
    public void UpdateTrain_Should_ThrowTrackEasyException_When_TrainNotFound()
    {
        // Arrange
        var @operator = CreateValidOperator();
        var nonExistentTrainId = Guid.NewGuid();
        @operator.AddCoach("CO1", ValidSeatNumbers(5));
        var coach1 = @operator.Coaches[0];

        // Act & Assert
        var ex = Should.Throw<TrackEasyException>(() =>
            @operator.UpdateTrain(nonExistentTrainId, "New Name",
                new List<(Guid CoachId, int Number)> { (coach1.Id, 1) }));

        ex.Code.ShouldBe(Codes.TrainNotFound);
    }

    [Fact]
    public void UpdateTrain_Should_ThrowTrackEasyException_When_CoachNotFound()
    {
        // Arrange
        var @operator = CreateValidOperator();
        @operator.AddCoach("CO1", ValidSeatNumbers(5));
        var coach1 = @operator.Coaches[0];
        @operator.AddTrain("Old Train", new List<(Guid CoachId, int Number)> { (coach1.Id, 1) });
        var trainToUpdate = @operator.Trains[0];
        var nonExistentCoachId = Guid.NewGuid();
        var newCoachesInfo = new List<(Guid CoachId, int Number)> { (nonExistentCoachId, 1) };

        // Act & Assert
        var ex = Should.Throw<TrackEasyException>(() =>
            @operator.UpdateTrain(trainToUpdate.Id, "New Name", newCoachesInfo));

        ex.Code.ShouldBe(Codes.CoachNotFound);
    }
    
    [Fact]
    public void AddManager_Should_AddManager_For_ValidData()
    {
        // Arrange
        var @operator = CreateValidOperator();
        const string firstName = "John";
        const string lastName = "Doe";
        const string email = "john.doe@example.com";
        var dateOfBirth = new DateOnly(1990, 1, 1);
        var expectedCreationTime = DateTime.UtcNow;
        _fakeTimeProvider.SetUtcNow(expectedCreationTime);
    
        // Act
        var manager = @operator.AddManager(firstName, lastName, email, dateOfBirth, _fakeTimeProvider);
    
        // Assert
        @operator.Managers.Count.ShouldBe(1);
        var addedManager = @operator.Managers[0];
        addedManager.ShouldBe(manager);
        addedManager.OperatorId.ShouldBe(@operator.Id);
        addedManager.User.FirstName.ShouldBe(firstName);
        addedManager.User.LastName.ShouldBe(lastName);
        addedManager.User.Email.ShouldBe(email);
        addedManager.User.DateOfBirth.ShouldBe(dateOfBirth);
    }

    [Fact]
    public void AddManager_Should_ThrowTrackEasyException_For_DuplicateEmail()
    {
        // Arrange
        var @operator = CreateValidOperator();
        const string email = "duplicate@example.com";
        @operator.AddManager("First", "Manager", email, new DateOnly(1985, 5, 5), _fakeTimeProvider); // Add first manager
    
        // Act & Assert
        var ex = Should.Throw<TrackEasyException>(() =>
            @operator.AddManager("Second", "Manager", email, new DateOnly(1995, 10, 10), _fakeTimeProvider)); // Try adding another with same email
    
        ex.Code.ShouldBe(Codes.ManagerEmailAlreadyExists);
    }

    [Theory]
    [InlineData("", "LastName", "valid@email.com", 1990, 1, 1)] // Empty First Name
    [InlineData("FirstName", "", "valid@email.com", 1990, 1, 1)] // Empty Last Name
    [InlineData("FirstName", "LastName", "invalid-email", 1990, 1, 1)] // Invalid Email
    [InlineData("FN", "LastName", "valid@email.com", 1990, 1, 1)] // Too short First Name
    [InlineData("FirstName", "LN", "valid@email.com", 1990, 1, 1)] // Too short Last Name
    public void AddManager_Should_ThrowValidationException_For_InvalidUserData(
        string firstName, string lastName, string email, int year, int month, int day)
    {
        // Arrange
        var @operator = CreateValidOperator();
        var dateOfBirth = new DateOnly(year, month, day);
    
        // Act & Assert
        Should.Throw<ValidationException>(() =>
            @operator.AddManager(firstName, lastName, email, dateOfBirth, _fakeTimeProvider));
    }
}