using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Triggers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WeatherMap.Services;

namespace Extensions.Triggers
{
    public class WeatherTriggerBindingProvider : ITriggerBindingProvider
    {
        private readonly INameResolver _nameResolver;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IWeatherService _weatherService;

        public WeatherTriggerBindingProvider(INameResolver nameResolver, 
            ILoggerFactory loggerFactory, IWeatherService weatherService)
        {
            this._nameResolver = nameResolver;
            this._loggerFactory = loggerFactory;
            this._weatherService = weatherService;
        }

        public Task<ITriggerBinding> TryCreateAsync(TriggerBindingProviderContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var parameter = context.Parameter;

            var triggerAttribute = parameter.GetCustomAttribute<WeatherTriggerAttribute>(inherit: false);
            if (triggerAttribute is null)
                return Task.FromResult<ITriggerBinding>(null);

            triggerAttribute.ApiKey = GetTriggerAttributeApiKey(triggerAttribute);

            return Task.FromResult<ITriggerBinding>(
                new WeatherTriggerBinding(parameter, _nameResolver, _weatherService, triggerAttribute));
        }

        private string GetTriggerAttributeApiKey(WeatherTriggerAttribute triggerAttribute)
        {
            if (string.IsNullOrEmpty(triggerAttribute.ApiKey))
            {
                var apiKey = _nameResolver.Resolve("Weather.ApiKey");

                if (string.IsNullOrEmpty(apiKey))
                    throw new InvalidOperationException("ApiKey is mandatory");

                return apiKey;
            }

            return triggerAttribute.ApiKey;
        }
    }
}