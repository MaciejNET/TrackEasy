using FluentValidation;
using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Operators;

public sealed class Operator : AggregateRoot
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Code { get; private set; }

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
    
#pragma warning disable CS8618, CS9264
    private Operator() {}
#pragma warning restore CS8618, CS9264
}