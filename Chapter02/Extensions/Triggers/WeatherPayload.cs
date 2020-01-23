using System;

namespace Extensions.Triggers
{
    public class WeatherPayload
    {
        public string CityName { get; set; }

        public double CurrentTemperature { get; set; }

        public double? LastTemperature { get; set; }

        public DateTimeOffset Timestamp { get; internal set; }
    }
}