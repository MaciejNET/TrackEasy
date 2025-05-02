using FluentValidation;
using TrackEasy.Domain.Operators;
using TrackEasy.Domain.Users;
using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Managers;

public sealed class Manager : AggregateRoot
{
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    public Guid OperatorId { get; private set; }
    public Operator Operator { get; private set; }

    public static Manager Create(User user, Operator @operator)
    {
        var manager = new Manager
        {
            UserId = user.Id,
            User = user,
            OperatorId = @operator.Id,
            Operator = @operator
        };

        new ManagerValidator().ValidateAndThrow(manager);
        return manager;
    }
    
#pragma warning disable CS8618
    private Manager() { }
#pragma warning restore CS8618
}