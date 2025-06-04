using TrackEasy.Domain.Connections;
using TrackEasy.Domain.Operators;
using TrackEasy.Domain.Shared;
using TrackEasy.Domain.Stations;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Connections.CreateConnection;

internal sealed class CreateConnectionCommandHandler(
    IConnectionRepository connectionRepository,
    IOperatorRepository operatorRepository,
    IStationRepository stationRepository)
    : ICommandHandler<CreateConnectionCommand, Guid>
{
    public async Task<Guid> Handle(CreateConnectionCommand request, CancellationToken cancellationToken)
    {
        var @operator = await operatorRepository.FindAsync(request.OperatorId, cancellationToken);
        
        if (@operator is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Operator with ID {request.OperatorId} was not found.");
        }
        
        var train = @operator.Trains.FirstOrDefault(x => x.Id == request.TrainId);
        
        if (train is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Train with ID {request.TrainId} was not found for operator {@operator.Name}.");
        }

        List<ConnectionStation> connectionStations = [];
        foreach (var connectionStation in request.ConnectionStations)
        {
            var station = await stationRepository.GetByIdAsync(connectionStation.StationId, cancellationToken);
            
            if (station is null)
            {
                throw new TrackEasyException(SharedCodes.EntityNotFound, $"Station with ID {connectionStation.StationId} was not found.");
            }
            
            connectionStations.Add(ConnectionStation.Create(station, connectionStation.ArrivalTime,
                connectionStation.DepartureTime, connectionStation.SequenceNumber));
        }

        var schedule = new Schedule(request.Schedule.ValidFrom, request.Schedule.ValidTo, request.Schedule.DaysOfWeek);
        var money = new Money(request.PricePerKilometer.Amount, request.PricePerKilometer.Currency);
        
        var connection = Connection.Create(request.Name, @operator, money, train, schedule, connectionStations, request.NeedsSeatReservation);
        connectionRepository.Add(connection);
        await connectionRepository.SaveChangesAsync(cancellationToken);
        
        return connection.Id;
    }
}