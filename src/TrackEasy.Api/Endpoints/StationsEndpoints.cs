namespace TrackEasy.Api.Endpoints;

public static class StationsEndpoints
{
    public static void MapStationsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/stations").WithTags("Stations");
    }
}