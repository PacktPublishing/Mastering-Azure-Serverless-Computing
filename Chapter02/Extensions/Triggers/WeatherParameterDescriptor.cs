using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Host.Protocols;

namespace Extensions.Triggers
{
    internal class WeatherTriggerParameterDescriptor : TriggerParameterDescriptor
    {
        public string CityName { get; internal set; }

        public double TemperatureThreshold { get; internal set; }

        public override string GetTriggerReason(IDictionary<string, string> arguments)
        {
            return base.GetTriggerReason(arguments);
        }
    }
}