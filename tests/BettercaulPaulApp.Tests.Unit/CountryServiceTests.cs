using BettercallPaulApp.Models;
using BettercallPaulApp.Services;
using FluentAssertions;

namespace BettercaulPaulApp.Tests.Unit
{
    public class CountryServiceTests
    {
        private readonly CountryService _sut = new();

        [Fact]
        public void FindCountryWithHighestPopulationDensity_ShouldReturnCorrectCountry_WhenDataIsValid()
        {
            // Arrange
            var countryData = new List<Country>
            {
                new() { Name = "CountryA", Population = 5000000, Area = 10000 },
                new() { Name = "CountryB", Population = 2000000, Area = 2000 }, // Highest Density (1000 per km²)
                new() { Name = "CountryC", Population = 3000000, Area = 5000 }
            };

            // Act
            var result = _sut.FindCountryWithHighestPopulationDensity(countryData);

            // Assert
            result
                .Should()
                .Be("CountryB");
        }

        [Fact]
        public void FindCountryWithHighestPopulationDensity_ShouldThrowArgumentNullException_WhenDataIsNull()
        {
            // Act
            var result = _sut.Invoking(s => s.FindCountryWithHighestPopulationDensity(null!));

            // Assert
            result
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Country data cannot be null. (Parameter 'countries')");
        }

        [Fact]
        public void FindCountryWithHighestPopulationDensity_ShouldThrowInvalidOperationException_WhenDataIsEmpty()
        {
            // Act
            var result = _sut.Invoking(s => s.FindCountryWithHighestPopulationDensity(new List<Country>()));

            // Assert
            result
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Country data is empty. Cannot determine the most dense country.");
        }
    }
}