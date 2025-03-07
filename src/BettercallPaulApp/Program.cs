using BettercallPaulApp;
using BettercallPaulApp.Services;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = AddDependencies.ConfigureServices();
var mainService = serviceProvider.GetRequiredService<MainService>();
mainService.ProcessWeatherDataToFindDayWithSmallestTemperatureSpread();
