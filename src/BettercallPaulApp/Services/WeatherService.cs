using BettercallPaulApp.Models;
using BettercallPaulApp.Parsers;
using BettercallPaulApp.Readers;

namespace BettercallPaulApp.Services;

public class WeatherService
{
    private readonly IReader _csvReader;
    private readonly IParser<DailyWeatherEntry> _weatherParser;
    private readonly string _weatherFile;

    private const string ResourcesFolder = "Resources";
    private const string WeatherFileName = "weather.csv";

    public WeatherService(IReader csvReader, IParser<DailyWeatherEntry> weatherParser)
    {
        _csvReader = csvReader;
        _weatherParser = weatherParser;
        var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../.."));
        _weatherFile = Path.Combine(root, ResourcesFolder, WeatherFileName);
    }

    public void ProcessWeatherDataToFindDayWithSmallestTemperatureSpread()
    {
        var csvData = _csvReader.ReadData(_weatherFile);
        var weatherData = _weatherParser.Parse(csvData);
        var smallestSpreadDay = FindDayWithSmallestTemperatureSpread(weatherData);
        Console.WriteLine($"Day with smallest temperature spread: {smallestSpreadDay}");
    }

    public int FindDayWithSmallestTemperatureSpread(IEnumerable<DailyWeatherEntry> weatherData)
    {
        if (weatherData == null)
        {
            throw new ArgumentNullException(nameof(weatherData), "Weather data cannot be null.");
        }

        var smallestSpreadEntry = weatherData.MinBy(x => x.MaxTemperature - x.MinTemperature);

        if (smallestSpreadEntry == null)
        {
            throw new InvalidOperationException("Weather data is empty.");
        }

        return smallestSpreadEntry.Day;
    }
}