using FluentAssertions;
using Moq;
using TrackEasy.Application.Stations.CreateCity;
using TrackEasy.Domain.Stations;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Tests;

public class CreateCityCommandHandlerTests
{
    private readonly Mock<ICityRepository> _repositoryMock = new();
    private readonly CreateCityCommandHandler _handler;

    public CreateCityCommandHandlerTests()
    {
        _handler = new CreateCityCommandHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenCityAlreadyExists()
    {
        var command = new CreateCityCommand("Warsaw", Country.PL);
        _repositoryMock.Setup(r => r.ExistsAsync(command.Name, default)).ReturnsAsync(true);

        var act = async () => await _handler.Handle(command, default);

        await act.Should().ThrowAsync<TrackEasyException>()
            .WithMessage("*already exists*");
    }

    [Fact]
    public async Task Handle_ShouldAddCity_WhenCityDoesNotExist()
    {
        var command = new CreateCityCommand("Warsaw", Country.PL);
        _repositoryMock.Setup(r => r.ExistsAsync(command.Name, default)).ReturnsAsync(false);

        await _handler.Handle(command, default);

        _repositoryMock.Verify(r => r.Add(It.Is<City>(c => c.Name == "Warsaw" && c.Country == Country.PL)), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }
}
