using Microsoft.Azure.WebJobs.Description;
using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions.Triggers
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter)]
    public class WeatherTriggerAttribute : Attribute
    {
        public WeatherTriggerAttribute(string cityName, double temperatureThreshold)
        {
            CityName = cityName;
            TemperatureThreshold = temperatureThreshold;
        }

        [AppSetting(Default = "Weather.ApiKey")]
        [AutoResolve]
        public string ApiKey { get; set; }

        public string CityName { get; internal set; }

        public double TemperatureThreshold { get; internal set; }
    }
}
