using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherService.Models;

namespace WeatherService.Repository
{
    public interface IWeatherAPIHelperRepo
    {
        Task<OpenWeatherResponse> GetWeatherDataBasedIdAsync(City city);
    }
}
