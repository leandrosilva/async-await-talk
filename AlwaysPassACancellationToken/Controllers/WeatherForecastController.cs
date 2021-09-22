using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlwaysPassACancellationToken.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        private async Task<IEnumerable<WeatherForecast>> GetWeatherForecastsAsync(CancellationToken cancellationToken = default)
        {
            var data = new List<WeatherForecast>();
            var rng = new Random();

            for (int i = 1; i <= 10; i++)
            {
                await Task.Delay(2000, cancellationToken); // oh gosh, this data retrieving is soooo freakin' expensive
                _logger.LogInformation($"Got record #{i}");

                data.Add(new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(i),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                });
            }

            return data;
        }

        [HttpGet("without-cancellation-token")]
        public async Task<IEnumerable<WeatherForecast>> GetWithoutCancellationToken()
        {
            _logger.LogInformation("Before expensive data retrieving");

            var data = await GetWeatherForecastsAsync();

            _logger.LogInformation("After expensive data retrieving");

            return data;
        }

        [HttpGet("with-cancellation-token")]
        public async Task<IEnumerable<WeatherForecast>> GetWithCancellationToken(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Before expensive data retrieving");

                var data = await GetWeatherForecastsAsync(cancellationToken);

                _logger.LogInformation("After expensive data retrieving");

                return data;
            }
            catch (TaskCanceledException e)
            {
                _logger.LogInformation($"Request was canceled: {e.Message}"); // no more expensive data retrieving for nothing
                return null;
            }
        }
    }
}
