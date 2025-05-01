using FluentValidation;

namespace TrackEasy.Domain.Coaches;

public sealed record Seat
{
    public int Number { get; private set; }

    internal static Seat Create(int number)
    {
        var seat = new Seat
        {
            Number = number
        };

        new SeatValidator().ValidateAndThrow(seat);        
        return seat;
    }
}