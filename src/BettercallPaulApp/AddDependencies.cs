using BettercallPaulApp.Models;
using BettercallPaulApp.Parsers;
using BettercallPaulApp.Readers;
using BettercallPaulApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BettercallPaulApp;

public static class AddDependencies
{
    public static ServiceProvider ConfigureServices()
    {
        return new ServiceCollection()
            .AddSingleton<IReader, CsvReader>()
            .AddTransient<IParser<DailyWeatherEntry>, WeatherParser>()
            .AddTransient<IParser<Country>, CountryParser>()
            .AddScoped<WeatherService>()
            .AddScoped<CountryService>()
            .BuildServiceProvider();
    }
}