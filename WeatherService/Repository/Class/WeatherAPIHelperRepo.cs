using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherService.Models;
using WeatherService.Repository;

namespace WeatherService
{
    public class WeatherAPIHelperRepo : IWeatherAPIHelperRepo
    {
        private readonly IOptions<AppOptions> _options;
        public WeatherAPIHelperRepo(IOptions<AppOptions> options)
        {
            _options = options;
        }
   
        public async Task<OpenWeatherResponse> GetWeatherDataBasedIdAsync(City city)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var response = await client.GetAsync($"/data/2.5/weather?id={city.CityId}&appid={_options.Value.OpenWeatherApiKey}&units=metric");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawWeather = JsonConvert.DeserializeObject<OpenWeatherResponse>(stringResult);
                    rawWeather.ErrorMessage = "";
                    return rawWeather;
                }
                catch (HttpRequestException httpRequestException)
                {
                    var rawWeather = new OpenWeatherResponse();
                    rawWeather.Id = city.CityId;
                    rawWeather.Name = city.CityName;
                    rawWeather.ErrorMessage = httpRequestException.Message;
                    return rawWeather;
                }
            }
        }
    }
}
