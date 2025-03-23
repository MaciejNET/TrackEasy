using MediatR;
using TrackEasy.Application.Stations.GetCountries;

namespace TrackEasy.Api.Endpoints;

public static class StationsEndpoints
{
    public static void MapStationsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/stations").WithTags("Stations");

        group.MapGet("/countries", async (ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new GetCountriesQuery();
                var countries = await sender.Send(query, cancellationToken);
                return Results.Ok(countries);
            })
            .WithName("GetCountries")
            .Produces<IEnumerable<CountryDto>>()
            .WithDescription("Get all countries.")
            .WithOpenApi();
    }
}