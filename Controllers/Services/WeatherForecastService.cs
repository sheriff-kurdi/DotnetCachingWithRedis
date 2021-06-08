using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetWithRedis.Extensions;
using Microsoft.Extensions.Caching.Distributed;
namespace DotnetWithRedis.Controllers.Services
{
    public class WeatherForecastService
    {
        private WeatherForecast[] forecasts;
        private string loadLocation = "";
        private readonly IDistributedCache cache;

        public WeatherForecastService(IDistributedCache cache)
        {
            this.cache = cache;

        }
        public async Task<object>  LoadForecast()
        {
            forecasts = null;
            loadLocation = null;

            string recordKey = "WeatherForecast_" + DateTime.Now.ToString("yyyyMMdd_hhmm");

            forecasts = await cache.GetRecordAsync<WeatherForecast[]>(recordKey);

            if (forecasts is null)
            {
                forecasts = await GetForecastAsync(DateTime.Now);

                loadLocation = $"Loaded from API at { DateTime.Now }";

                await cache.SetRecordAsync(recordKey, forecasts);
            }
            else
            {
                loadLocation = $"Loaded from the cache at { DateTime.Now }";
            }

            return new {forecasts = forecasts, location = loadLocation};
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private async Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            var rng = new Random();
            await Task.Delay(1500);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray();
        }
    }
}