namespace TrackEasy.Api.Endpoints;

public class StationsEndpoints : IEndpoints
{
    public static void MapEndpoints(RouteGroupBuilder rootGroup)
    {
        var group = rootGroup.MapGroup("/stations").WithTags("Stations");
    }
}