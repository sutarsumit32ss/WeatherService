namespace WeatherService.Models
{
    public class CityResponse
    {
        public bool Success { get; set; }
        public string Temp { get; set; }
        public string Summary { get; set; }
        public string City { get; set; }
        public string CityId { get; set; }
        public string ErrorMessage { get; set; }
    }

}
