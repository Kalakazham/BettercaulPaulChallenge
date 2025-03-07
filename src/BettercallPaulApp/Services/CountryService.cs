using BettercallPaulApp.Models;

namespace BettercallPaulApp.Services;

public class CountryService
{
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