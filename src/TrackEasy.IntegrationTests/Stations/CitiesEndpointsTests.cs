using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using TrackEasy.Application.Stations.CreateCity;
using TrackEasy.Domain.Stations;
using FluentAssertions;


namespace TrackEasy.IntegrationTests.Stations;

public class CitiesEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CitiesEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_ShouldCreateCity()
    {
        var command = new CreateCityCommand("Gdańsk", Country.PL);
        var response = await _client.PostAsJsonAsync("/cities", command);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Get_ShouldReturnCities()
    {
        var response = await _client.GetAsync("/cities");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
