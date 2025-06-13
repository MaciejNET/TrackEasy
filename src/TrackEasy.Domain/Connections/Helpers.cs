using FluentValidation;

namespace TrackEasy.Domain.Connections;

public static class Helpers
{
    public static IRuleBuilderOptions<T, IEnumerable<ConnectionStation>> ValidStations<T>(
            this IRuleBuilder<T, IEnumerable<ConnectionStation>> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                    .WithMessage("Stations collection cannot be empty.")
                .Must(stations => stations.Count() > 1)
                    .WithMessage("A connection must have at least two stations.")
                .Must(stations =>
                {
                    var sequenceNumbers = stations.Select(s => s.SequenceNumber)
                                                  .OrderBy(n => n)
                                                  .ToList();
                    return sequenceNumbers.SequenceEqual(Enumerable.Range(1, sequenceNumbers.Count));
                })
                    .WithMessage("Station sequence numbers must start from 1 and be in sequence.")
                .Must(stations =>
                {
                    var connectionStations = stations as ConnectionStation[] ?? stations.ToArray();
                    var stationsList = connectionStations.ToList();
                    for (var i = 1; i < connectionStations.Length; i++)
                    {
                        var previous = stationsList[i - 1];
                        var current = stationsList[i];
                        if (previous.DepartureTime.HasValue && current.ArrivalTime.HasValue &&
                            previous.DepartureTime > current.ArrivalTime)
                        {
                            return false;
                        }
                    }
                    return true;
                })
                    .WithMessage("Arrival and departure times must be in sequence.")
                .Must(stations =>
                    stations.Any(s => s.ArrivalTime == null && s.SequenceNumber == 1))
                    .WithMessage("There must be one station with a null arrival time and sequence number 1.")
                .Must(stations =>
                {
                    var connectionStations = stations as ConnectionStation[] ?? stations.ToArray();
                    var maxSequenceNumber = connectionStations.Max(s => s.SequenceNumber);
                    return connectionStations.Any(s => s.DepartureTime == null && s.SequenceNumber == maxSequenceNumber);
                })
                    .WithMessage("There must be one station with a null departure time and the maximum sequence number.");
        }
}