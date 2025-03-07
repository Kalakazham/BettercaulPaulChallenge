using BettercallPaulApp.Models;
using BettercallPaulApp.Parsers;
using FluentAssertions;

namespace BettercaulPaulApp.Tests.Unit
{
    public class WeatherParserTests
    {
        private readonly WeatherParser _sut = new();

        [Fact]
        public void Parse_ShouldReturnValidWeatherEntries_WhenDataIsCorrect()
        {
            // Arrange
            var csvData = new List<string[]>
            {
                new[] { "1", "30.5", "15.2" },
                new[] { "2", "28.0", "16.1" }
            };

            // Act
            var result = _sut.Parse(csvData);

            // Assert
            result.Should().BeEquivalentTo(new List<DailyWeatherEntry>
            {
                new() { Day = 1, MaxTemperature = 30.5, MinTemperature = 15.2 },
                new() { Day = 2, MaxTemperature = 28.0, MinTemperature = 16.1 }
            });
        }

        [Fact]
        public void Parse_ShouldThrowInvalidOperationException_WhenNoValidRows()
        {
            // Act
            var result = _sut.Invoking(parser => parser.Parse(new List<string[]>()));

            // Assert
            result
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("No valid weather data found.");
        }

        [Fact]
        public void Parse_ShouldThrowInvalidOperationException_WhenRowHasInsufficientColumns()
        {
            // Arrange
            var csvData = new List<string[]>
            {
                new[] { "1", "30.5" }
            };

            // Act & Assert
            var result = _sut.Invoking(parser => parser.Parse(csvData));

            // Assert
            result
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("No valid weather data found.");
        }

        [Fact]
        public void Parse_ShouldThrowFormatException_WhenRowHasInvalidNumericValues()
        {
            // Arrange
            var csvData = new List<string[]>
            {
                new[] { "1", "InvalidMax", "15.2" }, // MaxTemperature is not a valid number
                new[] { "2", "28.0", "InvalidMin" } // MinTemperature is not a valid number
            };

            // Act & Assert
            var result = _sut.Invoking(parser => parser.Parse(csvData));

            //Assert
            result
                .Should()
                .Throw<FormatException>()
                .WithMessage("Error parsing CSV data:*");
        }
    }
}