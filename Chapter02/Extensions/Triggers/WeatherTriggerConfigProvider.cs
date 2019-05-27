using Extensions.Triggers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using WeatherMap.Services;

namespace Extensions.Triggers
{
    [Extension("Weather")]
    public class WeatherTriggerConfigProvider : IExtensionConfigProvider
    {
        private readonly INameResolver _nameResolver;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IWeatherService _weatherService;

        public WeatherTriggerConfigProvider(INameResolver nameResolver,
            ILoggerFactory loggerFactory, IWeatherService weatherService)
        {
            this._nameResolver = nameResolver;
            this._loggerFactory = loggerFactory;
            this._weatherService = weatherService;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            var triggerAttributeBindingRule = context.AddBindingRule<WeatherTriggerAttribute>();
            triggerAttributeBindingRule.BindToTrigger<WeatherPayload>(
                new WeatherTriggerBindingProvider(this._nameResolver, this._loggerFactory, this._weatherService));

        }
    }
}
