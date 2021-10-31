using System.Collections.Generic;

namespace WeatherService
{
    public class OpenWeatherResponse
    {
        public string Name { get; set; }

        public IEnumerable<WeatherDescription> Weather { get; set; }

        public Main Main { get; set; }

        public string Id { get; set; }
        public string ErrorMessage { get; set; }
    }
}
