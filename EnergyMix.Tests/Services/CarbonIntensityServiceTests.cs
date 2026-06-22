using EnergyMix.Api.DTOs.External;
using EnergyMix.Api.Exceptions;
using EnergyMix.Api.Interfaces;
using EnergyMix.Api.Services;
using Moq;

namespace EnergyMix.Tests.Services;

public class CarbonIntensityServiceTests
{
    [Fact]
    public async Task GetOptimalChargingWindowAsync_ShouldThrowInvalidHoursException_WhenHoursAreOutsideRange()
    {
        // Arrange
        var clientMock = new Mock<ICarbonIntensityClient>();

        var service = new CarbonIntensityService(clientMock.Object);

        // Act and Assert
        await Assert.ThrowsAsync<InvalidHoursException>(() => service.GetOptimalChargingWindowAsync(0));
    }
    
    [Fact]
    public async Task GetOptimalChargingWindowAsync_ShouldReturnBestChargingWindow()
    {
    // Arrange
    var start = new DateTime(2026, 1, 1, 0, 0, 0);

    var response = new GenerationResponse
    {
        Intervals =
        [
            new GenerationInterval
            {
                From = start,
                To = start.AddMinutes(30),
                GenerationMix =
                [
                    new GenerationMix { Fuel = "wind", Perc = 10 },
                    new GenerationMix { Fuel = "gas", Perc = 90 }
                ]
            },
            new GenerationInterval
            {
                From = start.AddMinutes(30),
                To = start.AddHours(1),
                GenerationMix =
                [
                    new GenerationMix { Fuel = "wind", Perc = 20 },
                    new GenerationMix { Fuel = "gas", Perc = 80 }
                ]
            },
            new GenerationInterval
            {
                From = start.AddHours(1),
                To = start.AddHours(1).AddMinutes(30),
                GenerationMix =
                [
                    new GenerationMix { Fuel = "wind", Perc = 90 },
                    new GenerationMix { Fuel = "gas", Perc = 10 }
                ]
            },
            new GenerationInterval
            {
                From = start.AddHours(1).AddMinutes(30),
                To = start.AddHours(2),
                GenerationMix =
                [
                    new GenerationMix { Fuel = "wind", Perc = 95 },
                    new GenerationMix { Fuel = "gas", Perc = 5 }
                ]
            }
        ]
    };

    var clientMock = new Mock<ICarbonIntensityClient>();

    clientMock
        .Setup(x => x.GetCarbonIntensityDataAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
        .ReturnsAsync(response);

    var service = new CarbonIntensityService(clientMock.Object);

    // Act
    var result = await service.GetOptimalChargingWindowAsync(1);

    // Assert
    Assert.Equal(start.AddHours(1), result.Start);
    Assert.Equal(start.AddHours(2), result.End);
    Assert.Equal(92.5m, result.AverageCleanEnergyPercentage);
    }
    

    
}