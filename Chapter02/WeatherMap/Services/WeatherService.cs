using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherMap.Entities;
using WeatherMap.Internals;

namespace WeatherMap.Services
{
    public class WeatherService : IWeatherService
    {
        private string baseUri = "https://api.openweathermap.org/data/2.5/weather";
        
        public string ApiKey { get; set; }

        private Uri GetApiUrl(string cityCode)
        {
            return new Uri($"{baseUri}?q={cityCode}&appId={ApiKey}&units=metric");
        }


        public async Task<CityInfo> GetCityInfoAsync(string cityCode)
        {
            string response = null;
            using (var client = new HttpClient())
            {
                response = await client.GetStringAsync(GetApiUrl(cityCode));
            }

            var responseObj = JsonConvert.DeserializeObject<WeatherData>(response);

            CityInfo city = null;

            if (responseObj.cod == 200)
            {
                var cityData = responseObj.main;

                city = new CityInfo()
                {
                    CityCode = cityCode,
                    Temperature = cityData.temp,
                    Timestamp = responseObj.dt.ToUtcDateTimeOffset()
                };
            }

            return city;
        }
    }

}
