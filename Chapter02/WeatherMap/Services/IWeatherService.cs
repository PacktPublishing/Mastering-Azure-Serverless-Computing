using System.Threading.Tasks;
using WeatherMap.Entities;

namespace WeatherMap.Services
{
    public interface IWeatherService
    {
        string ApiKey { get; set; }

        Task<CityInfo> GetCityInfoAsync(string cityCode);
    }
}