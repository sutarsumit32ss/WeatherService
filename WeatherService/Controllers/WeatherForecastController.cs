using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WeatherService.Models;

namespace WeatherService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IOptions<AppOptions> _options;
        private readonly ILogger<WeatherForecastController> _logger;
        public WeatherForecastController(IOptions<AppOptions> options, ILogger<WeatherForecastController> logger)
        {
            _options = options;
            _logger = logger;
            WeatherAPIHelper.WeatherKey = _options.Value.OpenWeatherApiKey;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> GetAllCitiWeatherData(List<City> cityList)
        {
            if (cityList is null || cityList.Count == 0)
            {
                return Ok(new
                {
                    APISuccess = false,
                    ErrorMessage = $"No city available to get the data",
                }); ;
            }

            try
            {
                List<Task<OpenWeatherResponse>> listOfTasks = new List<Task<OpenWeatherResponse>>();
                cityList.ToList().ForEach(city =>
                {
                    listOfTasks.Add(WeatherAPIHelper.GetWeatherDataBasedIdAsync(city));

                });

                var cityResponseListResult = await Task.WhenAll(listOfTasks);
                var cityResponseList = new List<CityResponse>();
                cityResponseListResult.ToList().ForEach(result =>
                {
                    cityResponseList.Add(new CityResponse()
                    {
                        Success = string.IsNullOrEmpty(result.ErrorMessage) ? true : false,
                        Temp = result.Main == null ? "" : result.Main.Temp,
                        Summary = result.Weather == null ? "" : string.Join(",", result.Weather.Select(x => x.Main)),
                        City = result.Name,
                        CityId = result.Id,
                        ErrorMessage = result.ErrorMessage,
                    });
                });
                return Ok(new
                {
                    APISuccess = true,
                    Data = cityResponseList
                });
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message, null);
                return BadRequest(new 
                {
                    APISuccess = false,
                    ErrorMessage = $"Error getting weather from OpenWeather: {ex.Message}"
                }); ;
            }
        }
    }
}
