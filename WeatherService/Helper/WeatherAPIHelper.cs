using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherService.Models;

namespace WeatherService
{
    public class WeatherAPIHelper
    {
        public static string WeatherKey;
        public static async Task<OpenWeatherResponse> GetWeatherDataBasedIdAsync(City city)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var response = await client.GetAsync($"/data/2.5/weather?id={city.CityId}&appid={WeatherKey}&units=metric");
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
