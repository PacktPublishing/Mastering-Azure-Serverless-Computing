using System;

namespace WeatherMap.Entities
{
    public class CityInfo
    {
        public string CityCode { get; set; }

        public double Temperature { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}