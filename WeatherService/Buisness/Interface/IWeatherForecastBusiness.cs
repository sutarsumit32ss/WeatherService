using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherService.Models;

namespace WeatherService.Business
{
    public interface IWeatherForecastBusiness
    {
        Task<List<CityResponse>> GetWeatherDataBasedId(List<City> cityList);
    }
}
