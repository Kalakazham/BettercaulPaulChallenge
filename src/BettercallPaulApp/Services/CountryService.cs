using BettercallPaulApp.Models;
using BettercallPaulApp.Parsers;
using BettercallPaulApp.Readers;

namespace BettercallPaulApp.Services;

public class CountryService
{
    private readonly IReader _csvReader;
    private readonly IParser<Country> _countryParser;
    private readonly string _countryFile;

    private const string ResourcesFolder = "Resources";
    private const string CountryFileName = "countries.csv";

    public CountryService(IReader csvReader, IParser<Country> countryParser)
    {
        _csvReader = csvReader;
        _countryParser = countryParser;
        var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../.."));
        _countryFile = Path.Combine(root, ResourcesFolder, CountryFileName);
    }

    public void ProcessCountryDataToFindHighestDensity()
    {
        var csvData = _csvReader.ReadData(_countryFile, ';');
        var countryData = _countryParser.Parse(csvData);
        var mostDenseCountry = FindCountryWithHighestPopulationDensity(countryData);
        Console.WriteLine($"Country with highest population density: {mostDenseCountry}");
    }

    public string FindCountryWithHighestPopulationDensity(IEnumerable<Country> countries)
    {
        if (countries == null)
        {
            throw new ArgumentNullException(nameof(countries), "Country data cannot be null.");
        }

        var mostDenseCountry = countries.MaxBy(x => x.Population / x.Area);

        if (mostDenseCountry == null)
        {
            throw new InvalidOperationException("Country data is empty. Cannot determine the most dense country.");
        }

        return mostDenseCountry.Name;
    }
}