using BettercallPaulApp.Parsers;
using FluentAssertions;

namespace BettercaulPaulApp.Tests.Unit
{
    public class CountryParserTests
    {
        private readonly CountryParser _sut = new();

        [Fact]
        public void Parse_ShouldReturnListOfCountries_WhenValidDataIsProvided()
        {
            // Arrange
            var csvData = new List<string[]>
            {
                new[] { "Austria", "Vienna", "1995", "8926000", "83855", "447718", "0.922", "19" },
                new[] { "Belgium", "Brussels", "Founder", "11566041", "30528", "517609", "0.931", "21" }
            };

            // Act
            var result = _sut.Parse(csvData);

            // Assert
            result
                .Should()
                .HaveCount(2);
        }

        [Fact]
        public void Parse_ShouldThrowInvalidOperationException_WhenDataIsEmpty()
        {
            var result = _sut.Invoking(s => s.Parse(new List<string[]>()));

            // Assert
            result
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("No valid country data found.");
        }

        [Fact]
        public void Parse_ShouldThrowFormatException_WhenDataHasInvalidNumbers()
        {
            // Arrange
            var csvData = new List<string[]>
            {
                new[] { "Germany", "Berlin", "Founder", "invalid_number", "357386", "3863344", "0.947", "96" }
            };

            // Act
            var result = _sut.Invoking(s => s.Parse(csvData));

            // Assert
            result
                .Should()
                .Throw<FormatException>()
                .WithMessage("Error parsing CSV data*");
        }

        [Fact]
        public void Parse_ShouldCorrectlyHandleEuropeanFormattedNumbers()
        {
            // Arrange
            var csvData = new List<string[]>
            {
                new[] { "France", "Paris", "Founder", "4.036.355,00", "632833", "2707074", "0.901", "79" }
            };

            // Act
            var result = _sut.Parse(csvData);

            // Assert
            result
                .Should()
                .ContainSingle(c => c.Name == "France");
        }
    }
}