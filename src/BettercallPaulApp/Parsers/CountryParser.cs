using System.Globalization;
using BettercallPaulApp.Models;

namespace BettercallPaulApp.Parsers;

public class CountryParser : IParser<Country>
{
    public IEnumerable<Country> Parse(IEnumerable<string[]> csvData)
    {
        try
        {
            var countries = csvData
                .Where(row => row.Length >= 8)
                .Select(row =>
                    new Country
                    {
                        Name = row[0],
                        Capital = row[1],
                        Population = ParseNumber(row[3]),
                        Area = ParseNumber(row[4]),
                        GDP = ParseNumber(row[5]),
                        HDI = double.Parse(row[6], CultureInfo.InvariantCulture),
                    })
                .ToList();

            if (countries.Count == 0)
            {
                throw new InvalidOperationException("No valid country data found.");
            }

            return countries;
        }
        catch (FormatException ex)
        {
            throw new FormatException($"Error parsing CSV data: {ex.Message}", ex);
        }
    }

    private static double ParseNumber(string input)
    {
        return double.Parse(input.Replace(".", "").Replace(",", "."), CultureInfo.InvariantCulture);
    }
}