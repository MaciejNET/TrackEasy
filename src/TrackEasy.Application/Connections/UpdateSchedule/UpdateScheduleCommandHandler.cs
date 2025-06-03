using TrackEasy.Domain.Connections;
using TrackEasy.Domain.Stations;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Connections.UpdateSchedule;

internal sealed class UpdateScheduleCommandHandler(IConnectionRepository connectionRepository, IStationRepository stationRepository)
    : ICommandHandler<UpdateScheduleCommand>
{
    public async Task Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
    {
        var connection = await connectionRepository.FindByIdAsync(request.Id, cancellationToken);
        
        if (connection is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Connection with ID {request.Id} was not found.");
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
        connection.UpdateSchedule(schedule, connectionStations);
        
        await connectionRepository.SaveChangesAsync(cancellationToken);
    }
}