namespace TrackEasy.Application.Operators.FindCoach;

public sealed record CoachDetailsDto(Guid Id, string Code, IEnumerable<int> SeatsNumbers);