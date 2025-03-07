using BettercallPaulApp.Models;
using BettercallPaulApp.Parsers;
using BettercallPaulApp.Readers;
using BettercallPaulApp.Services;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BettercaulPaulApp.Tests.Unit
{
    public class WeatherServiceTests
    {
        private readonly WeatherService _sut;
        private readonly IReader _csvReader = Substitute.For<IReader>();
        private readonly IParser<DailyWeatherEntry> _weatherParser = Substitute.For<IParser<DailyWeatherEntry>>();

        public WeatherServiceTests()
        {
            _sut = new WeatherService(_csvReader, _weatherParser);
        }

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

        [Fact]
        public void ProcessWeatherDataToFindDayWithSmallestTemperatureSpread_ShouldNotThrow_WhenValidData()
        {
            // Arrange
            var csvData = new List<string[]>
            {
                new[] { "1", "30.5", "15.2" },
                new[] { "2", "28.0", "16.1" }
            };

            var parsedData = new List<DailyWeatherEntry>
            {
                new() { Day = 1, MaxTemperature = 30, MinTemperature = 15 },
                new() { Day = 2, MaxTemperature = 28, MinTemperature = 16 }
            };

            _csvReader.ReadData(Arg.Any<string>()).Returns(csvData);
            _weatherParser.Parse(Arg.Any<IEnumerable<string[]>>()).Returns(parsedData);

            // Act
            var result = _sut.Invoking(s => s.ProcessWeatherDataToFindDayWithSmallestTemperatureSpread());

            // Assert
            result
                .Should()
                .NotThrow();
        }

        [Fact]
        public void
            ProcessWeatherDataToFindDayWithSmallestTemperatureSpread_ShouldThrowException_WhenEmptyFile()
        {
            // Arrange
            _csvReader.ReadData(Arg.Any<string>()).Returns(new List<string[]>());
            _weatherParser.Parse(Arg.Any<IEnumerable<string[]>>()).Returns(new List<DailyWeatherEntry>());

            // Act
            var result = _sut.Invoking(s => s.ProcessWeatherDataToFindDayWithSmallestTemperatureSpread());

            // Assert
            result
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Weather data is empty.");
        }

        [Fact]
        public void
            ProcessWeatherDataToFindDayWithSmallestTemperatureSpread_ShouldThrowFileNotFoundException_WhenFileNotFound()
        {
            // Arrange
            _csvReader.ReadData(Arg.Any<string>()).Throws(new FileNotFoundException("The file was not found."));

            // Act
            var result = _sut.Invoking(s => s.ProcessWeatherDataToFindDayWithSmallestTemperatureSpread());

            //Assert
            result
                .Should()
                .Throw<FileNotFoundException>()
                .WithMessage("The file was not found.");
        }

        [Fact]
        public void
            ProcessWeatherDataToFindDayWithSmallestTemperatureSpread_ShouldThrowFormatException_WhenCsvParsingFails()
        {
            // Arrange
            _csvReader.ReadData(Arg.Any<string>()).Returns(new List<string[]> { new[] { "InvalidData" } });
            _weatherParser.Parse(Arg.Any<IEnumerable<string[]>>())
                .Throws(new FormatException("Error parsing CSV data."));

            // Act
            var result = _sut.Invoking(s => s.ProcessWeatherDataToFindDayWithSmallestTemperatureSpread());

            //Assert
            result
                .Should()
                .Throw<FormatException>()
                .WithMessage("Error parsing CSV data.");
        }
    }
}