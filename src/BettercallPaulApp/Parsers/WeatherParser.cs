using System.Globalization;
using BettercallPaulApp.Models;

namespace BettercallPaulApp.Parsers;

public class WeatherParser : IParser<DailyWeatherEntry>
{
    public IEnumerable<DailyWeatherEntry> Parse(IEnumerable<string[]> csvData)
    {
        try
        {
            var dailyWeatherEntries = csvData
                .Where(row => row.Length >= 3)
                .Select(row =>
                    new DailyWeatherEntry
                    {
                        Day = int.Parse(row[0]),
                        MaxTemperature = double.Parse(row[1], CultureInfo.InvariantCulture),
                        MinTemperature = double.Parse(row[2], CultureInfo.InvariantCulture)
                    }).ToList();

            if (dailyWeatherEntries.Count == 0)
            {
                throw new InvalidOperationException("No valid weather data found.");
            }

            return dailyWeatherEntries;
        }
        catch (FormatException ex)
        {
            throw new FormatException($"Error parsing CSV data: {ex.Message}", ex);
        }
    }
}