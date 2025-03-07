using BettercallPaulApp.Models;
using BettercallPaulApp.Parsers;
using BettercallPaulApp.Readers;

namespace BettercallPaulApp.Services;

public class MainService
{
    private readonly IReader _csvReader;
    private readonly IParser<DailyWeatherEntry> _weatherParser;
    private readonly IParser<Country> _countryParser;
    private readonly WeatherService _weatherService;
    private readonly CountryService _countryService;
    private readonly string _weatherFile;
    private readonly string _countryFile;

    private const string ResourcesFolder = "Resources";
    private const string WeatherFileName = "weather.csv";
    private const string CountryFileName = "countries.csv";

    public MainService(IReader csvReader, IParser<DailyWeatherEntry> weatherParser, IParser<Country> countryParser,
        WeatherService weatherService, CountryService countryService)
    {
        _csvReader = csvReader;
        _weatherParser = weatherParser;
        _countryParser = countryParser;
        _weatherService = weatherService;
        _countryService = countryService;
        var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../.."));
        _weatherFile = Path.Combine(root, ResourcesFolder, WeatherFileName);
        _countryFile = Path.Combine(root, ResourcesFolder, CountryFileName);
    }

    public void ProcessWeatherDataToFindDayWithSmallestTemperatureSpread()
    {
        var csvData = _csvReader.ReadData(_weatherFile);
        var weatherData = _weatherParser.Parse(csvData);
        var smallestSpreadDay = _weatherService.FindDayWithSmallestTemperatureSpread(weatherData);
        Console.WriteLine($"Day with smallest temperature spread: {smallestSpreadDay}");
    }

    public void ProcessCountryDataToFindHighestDensity()
    {
        var csvData = _csvReader.ReadData(_countryFile, ';');
        var countryData = _countryParser.Parse(csvData);
        var mostDenseCountry = _countryService.FindCountryWithHighestPopulationDensity(countryData);
        Console.WriteLine($"Country with highest population density: {mostDenseCountry}");
    }
}