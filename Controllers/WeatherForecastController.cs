using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetWithRedis.Controllers.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotnetWithRedis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
         private readonly WeatherForecastService weatherForecastService;

        public WeatherForecastController(WeatherForecastService weatherForecastService)
        {
            this.weatherForecastService = weatherForecastService;
        }

        [HttpGet]
        public async Task<object> Get()
        {
            return await weatherForecastService.LoadForecast();
        }
    }
}
