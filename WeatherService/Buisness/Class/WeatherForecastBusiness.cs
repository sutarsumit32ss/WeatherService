using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherService.Models;
using WeatherService.Repository;

namespace WeatherService.Business
{
    public class WeatherForecastBusiness : IWeatherForecastBusiness
    {
        private readonly IWeatherAPIHelperRepo _weatherAPIHelperRepo;
        private readonly ILogger<WeatherForecastBusiness> _logger;
        public WeatherForecastBusiness(IWeatherAPIHelperRepo weatherAPIHelperRepo, ILogger<WeatherForecastBusiness> logger)
        {
            _weatherAPIHelperRepo = weatherAPIHelperRepo;
            _logger = logger;
        }
        public async Task<List<CityResponse>> GetWeatherDataBasedId(List<City> cityList)
        {
            try
            {
                List<Task<OpenWeatherResponse>> listOfTasks = new List<Task<OpenWeatherResponse>>();
                cityList.ToList().ForEach(city =>
                {
                    listOfTasks.Add(_weatherAPIHelperRepo.GetWeatherDataBasedIdAsync(city));

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
                return cityResponseList;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message, null);
                throw;
            }
        }
    }
}
