using BettercallPaulApp;
using BettercallPaulApp.Services;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = AddDependencies.ConfigureServices();

var weatherService = serviceProvider.GetRequiredService<WeatherService>();
weatherService.ProcessWeatherDataToFindDayWithSmallestTemperatureSpread();

var countryService = serviceProvider.GetRequiredService<CountryService>();
countryService.ProcessCountryDataToFindHighestDensity();