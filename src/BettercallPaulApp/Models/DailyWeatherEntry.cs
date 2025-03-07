namespace BettercallPaulApp.Models;

public class DailyWeatherEntry
{
    public int Day { get; init; }
    public double MaxTemperature { get; init; }
    public double MinTemperature { get; init; }
    
    //There are more properties in the csv file, which are irrelevant for now
}