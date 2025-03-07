using BettercallPaulApp.Models;
using BettercallPaulApp.Parsers;
using BettercallPaulApp.Readers;
using BettercallPaulApp.Services;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BettercaulPaulApp.Tests.Unit
{
    public class MainServiceTests
    {
        private readonly MainService _sut;
        private readonly IReader _csvReader = Substitute.For<IReader>();
        private readonly IParser<DailyWeatherEntry> _weatherParser = Substitute.For<IParser<DailyWeatherEntry>>();
        private readonly IParser<Country> _countryParser = Substitute.For<IParser<Country>>();
        private readonly WeatherService _weatherService = Substitute.For<WeatherService>();
        private readonly CountryService _countryService = Substitute.For<CountryService>();

        public MainServiceTests()
        {
            _sut = new MainService(_csvReader, _weatherParser, _countryParser, _weatherService, _countryService);
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

        [Fact]
        public void ProcessCountryDataToFindHighestDensity_ShouldNotThrow_WhenValidData()
        {
            // Arrange
            var csvData = new List<string[]>
            {
                new[] { "Country1", "Capital1", "2000", "5000000", "10000", "30000", "0.9", "10" },
                new[] { "Country2", "Capital2", "2010", "2000000", "2000", "50000", "0.8", "8" }
            };

            var parsedData = new List<Country>
            {
                new()
                {
                    Name = "Country1", Capital = "Capital1", AccessionYear = "2000", Population = 5000000, Area = 10000,
                    GDP = 30000, HDI = 0.9, MEPs = 10
                },
                new()
                {
                    Name = "Country2", Capital = "Capital2", AccessionYear = "2010", Population = 2000000, Area = 2000,
                    GDP = 50000, HDI = 0.8, MEPs = 8
                }
            };

            _csvReader.ReadData(Arg.Any<string>(), Arg.Any<char>()).Returns(csvData);
            _countryParser.Parse(Arg.Any<IEnumerable<string[]>>()).Returns(parsedData);

            // Act
            var result = _sut.Invoking(s => s.ProcessCountryDataToFindHighestDensity());

            // Assert
            result
                .Should()
                .NotThrow();
        }

        [Fact]
        public void ProcessCountryDataToFindHighestDensity_ShouldThrowException_WhenEmptyFile()
        {
            // Arrange
            _csvReader.ReadData(Arg.Any<string>(), Arg.Any<char>()).Returns(new List<string[]>());
            _countryParser.Parse(Arg.Any<IEnumerable<string[]>>()).Returns(new List<Country>());

            // Act
            var result = _sut.Invoking(s => s.ProcessCountryDataToFindHighestDensity());

            // Assert
            result
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Country data is empty. Cannot determine the most dense country.");
        }
 
        [Fact]
        public void ProcessCountryDataToFindHighestDensity_ShouldThrowFileNotFoundException_WhenFileNotFound()
        {
            // Arrange
            _csvReader.ReadData(Arg.Any<string>(), Arg.Any<char>())
                .Throws(new FileNotFoundException("The file was not found."));

            // Act
            var result = _sut.Invoking(s => s.ProcessCountryDataToFindHighestDensity());

            // Assert
            result
                .Should()
                .Throw<FileNotFoundException>()
                .WithMessage("The file was not found.");
        }

        [Fact]
        public void ProcessCountryDataToFindHighestDensity_ShouldThrowFormatException_WhenCsvParsingFails()
        {
            // Arrange
            _csvReader.ReadData(Arg.Any<string>(), Arg.Any<char>())
                .Returns(new List<string[]> { new[] { "InvalidData" } });
            _countryParser.Parse(Arg.Any<IEnumerable<string[]>>())
                .Throws(new FormatException("Error parsing CSV data."));

            // Act
            var result = _sut.Invoking(s => s.ProcessCountryDataToFindHighestDensity());

            // Assert
            result
                .Should()
                .Throw<FormatException>()
                .WithMessage("Error parsing CSV data.");
        }
    }
}