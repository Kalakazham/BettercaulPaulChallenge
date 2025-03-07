using BettercallPaulApp.Models;
using BettercallPaulApp.Services;
using FluentAssertions;

namespace BettercaulPaulApp.Tests.Unit
{
    public class WeatherServiceTests
    {
        private readonly WeatherService _sut = new();

        [Fact]
        public void FindDayWithSmallestTemperatureSpread_ShouldReturnCorrectDay_WhenDataIsValid()
        {
            // Arrange
            var weatherData = new List<DailyWeatherEntry>
            {
                new() { Day = 1, MaxTemperature = 30, MinTemperature = 15 }, // Spread: 15
                new() { Day = 2, MaxTemperature = 28, MinTemperature = 16 }, // Spread: 12
                new() { Day = 3, MaxTemperature = 25, MinTemperature = 14 } // Spread: 11
            };

            // Act
            var result = _sut.FindDayWithSmallestTemperatureSpread(weatherData);

            // Assert
            result
                .Should()
                .Be(3);
        }

        [Fact]
        public void FindDayWithSmallestTemperatureSpread_ShouldThrowArgumentNullException_WhenWeatherDataIsNull()
        {
            // Act
            var result = _sut.Invoking(s => s.FindDayWithSmallestTemperatureSpread(null!));

            //Assert
            result
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Weather data cannot be null. (Parameter 'weatherData')");
        }

        [Fact]
        public void FindDayWithSmallestTemperatureSpread_ShouldThrowInvalidOperationException_WhenDataIsEmpty()
        {
            // Act
            var result = _sut.Invoking(s => s.FindDayWithSmallestTemperatureSpread(new List<DailyWeatherEntry>()));

            // Assert
            result
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Weather data is empty.");
        }
    }
}