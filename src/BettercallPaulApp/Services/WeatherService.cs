using BettercallPaulApp.Models;

namespace BettercallPaulApp.Services;

public class WeatherService
{
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