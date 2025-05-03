using FluentValidation;
using TrackEasy.Domain.Coaches;
using TrackEasy.Domain.Managers;
using TrackEasy.Domain.Trains;
using TrackEasy.Domain.Users;
using TrackEasy.Shared.Domain.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Domain.Operators;

public sealed class Operator : AggregateRoot
{
    private readonly List<Train> _trains = [];
    private readonly List<Coach> _coaches = [];
    private readonly List<Manager> _managers = [];
    
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Code { get; private set; }
    public IReadOnlyList<Train> Trains => _trains.AsReadOnly();
    public IReadOnlyList<Coach> Coaches => _coaches.AsReadOnly();
    public IReadOnlyList<Manager> Managers => _managers.AsReadOnly();

    public static Operator Create(string name, string code)
    {
        var @operator = new Operator
        {
            Id = Guid.NewGuid(),
            Name = name,
            Code = code
        };
        
        new OperatorValidator().ValidateAndThrow(@operator);
        return @operator;
    }
    
    public void Update(string name, string code)
    {
        Name = name;
        Code = code;
        new OperatorValidator().ValidateAndThrow(this);
    }
    
    public void AddCoach(string code, IEnumerable<Seat> seats)
    {
        if (_coaches.Any(x => x.Code == code))
        {
            throw new TrackEasyException(Codes.CoachCodeAlreadyExists, $"Coach with code {code} already exists.");
        }
        var coach = Coach.Create(code, seats);
        _coaches.Add(coach);
        new OperatorValidator().ValidateAndThrow(this);
    }
    
    public void UpdateCoach(Guid coachId, string code, IEnumerable<Seat> seats)
    {
        var coach = _coaches.FirstOrDefault(x => x.Id == coachId);
        if (coach is null)
        {
            throw new TrackEasyException(Codes.CoachNotFound, $"Coach with ID {coachId} not found.");
        }
        coach.Update(code, seats);
        new OperatorValidator().ValidateAndThrow(this);
    }
    
    public void RemoveCoach(Guid coachId)
    {
        var coach = _coaches.FirstOrDefault(x => x.Id == coachId);
        if (coach is null)
        {
            throw new TrackEasyException(Codes.CoachNotFound, $"Coach with ID {coachId} not found.");
        }
        _coaches.Remove(coach);
        new OperatorValidator().ValidateAndThrow(this);
    }
    
    public void AddTrain(string name, IEnumerable<(Guid CoachId, int Number)> coaches)
    {
        if (_trains.Any(x => x.Name == name))
        {
            throw new TrackEasyException(Codes.TrainNameAlreadyExists, $"Train with name {name} already exists.");
        }
        var trainCoaches = new List<(Coach Coach, int Number)>();
        foreach (var (coachId, number) in coaches)
        {
            var coach = _coaches.FirstOrDefault(x => x.Id == coachId);
            if (coach is null)
            {
                throw new TrackEasyException(Codes.CoachNotFound, $"Coach with ID {coachId} not found.");
            }
            trainCoaches.Add((coach, number));
        }
        var train = Train.Create(name, trainCoaches);
        _trains.Add(train);
        new OperatorValidator().ValidateAndThrow(this);
    }

    public void UpdateTrain(Guid trainId, string name, IEnumerable<(Guid CoachId, int Number)> coaches)
    {
        var train = _trains.FirstOrDefault(x => x.Id == trainId);
        if (train is null)
        {
            throw new TrackEasyException(Codes.TrainNotFound, $"Train with ID {trainId} not found.");
        }
        var trainCoaches = new List<(Coach Coach, int Number)>();
        foreach (var (coachId, number) in coaches)
        {
            var coach = _coaches.FirstOrDefault(x => x.Id == coachId);
            if (coach is null)
            {
                throw new TrackEasyException(Codes.CoachNotFound, $"Coach with ID {coachId} not found.");
            }
            trainCoaches.Add((coach, number));
        }
        train.Update(name, trainCoaches);
        new OperatorValidator().ValidateAndThrow(this);
    }
    
    public void AddManager(string firstName, string lastName, string email, DateOnly dateOfBirth, TimeProvider timeProvider)
    {
        if (_managers.Any(x => x.User.Email == email))
        {
            throw new TrackEasyException(Codes.ManagerEmailAlreadyExists, $"Manager with email {email} already exists.");
        }
        var user = User.CreateManager(firstName, lastName, email, dateOfBirth, timeProvider);
        var manager = Manager.Create(user, this);
        _managers.Add(manager);
        new OperatorValidator().ValidateAndThrow(this);
    }
    
#pragma warning disable CS8618, CS9264
    private Operator() {}
#pragma warning restore CS8618, CS9264
}