using BettercallPaulApp.Readers;
using FluentAssertions;

namespace BettercaulPaulApp.Tests.Unit
{
    public class CsvReaderTests
    {
        private readonly CsvReader _sut = new();
        private const string TestFilePath = "test_weather.csv";

        [Fact]
        public void ReadData_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
        {
            // Act
            var result = _sut.Invoking(reader => reader.ReadData("non_existent_file.csv"));

            // Assert
            result
                .Should()
                .Throw<FileNotFoundException>()
                .WithMessage("The file at 'non_existent_file.csv' was not found.");
        }

        [Fact]
        public void ReadData_ShouldThrowInvalidOperationException_WhenFileIsEmpty()
        {
            // Arrange
            File.WriteAllText(TestFilePath, "");

            // Act & Assert
            var result = _sut.Invoking(reader => reader.ReadData(TestFilePath));

            // Assert
            result
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage($"The file '{TestFilePath}' contains no data.");

            // Cleanup
            File.Delete(TestFilePath);
        }

        [Fact]
        public void ReadData_ShouldReturnParsedLines_WhenFileHasValidData()
        {
            // Arrange
            const string content = "Day,MxT,MnT\n1,30.5,15.2\n2,28.0,16.1";
            File.WriteAllText(TestFilePath, content);

            // Act
            var result = _sut.ReadData(TestFilePath);

            // Assert
            result.Should().BeEquivalentTo(new List<string[]>
            {
                new[] { "1", "30.5", "15.2" },
                new[] { "2", "28.0", "16.1" }
            });

            // Cleanup
            File.Delete(TestFilePath);
        }
    }
}