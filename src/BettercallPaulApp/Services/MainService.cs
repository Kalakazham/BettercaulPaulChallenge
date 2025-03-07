using BettercallPaulApp.Models;
using BettercallPaulApp.Parsers;
using BettercallPaulApp.Readers;

namespace BettercallPaulApp.Services;

public class MainService
{
    private readonly IReader _csvReader;
    private readonly IParser<DailyWeatherEntry> _weatherParser;
    private readonly WeatherService _weatherService;
    private readonly string _weatherFile;
    
    private const string ResourcesFolder = "Resources";
    private const string WeatherFileName = "weather.csv";

    public MainService(IReader csvReader, IParser<DailyWeatherEntry> weatherParser, WeatherService weatherService)
    {
        _csvReader = csvReader;
        _weatherParser = weatherParser;
        _weatherService = weatherService;
        var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../.."));
        _weatherFile = Path.Combine(root, ResourcesFolder, WeatherFileName);
    }

    public void ProcessWeatherDataToFindDayWithSmallestTemperatureSpread()
    {
        var csvData = _csvReader.ReadData(_weatherFile);
        var weatherData = _weatherParser.Parse(csvData);
        var smallestSpreadDay = _weatherService.FindDayWithSmallestTemperatureSpread(weatherData);
        Console.WriteLine($"Day with smallest temperature spread: {smallestSpreadDay}");
    }
}