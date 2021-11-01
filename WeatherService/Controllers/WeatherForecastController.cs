using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WeatherService.Business;
using WeatherService.Models;
using WeatherService.Repository;

namespace WeatherService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherForecastBusiness _weatherForecastBusiness;
        private readonly ILogger<WeatherForecastController> _logger;
        public WeatherForecastController(IWeatherForecastBusiness weatherForecastBusiness, ILogger<WeatherForecastController> logger)
        {
            _weatherForecastBusiness = weatherForecastBusiness;
            _logger = logger;
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
               var cityResponseList = await _weatherForecastBusiness.GetWeatherDataBasedId(cityList);
                return Ok(new
                {
                    APISuccess = true,
                    Data = cityResponseList
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new 
                {
                    APISuccess = false,
                    ErrorMessage = $"Error getting weather from OpenWeather: {ex.Message}"
                }); ;
            }
        }
    }
}
