using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Listeners;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Azure.WebJobs.Host.Triggers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeatherMap.Services;

namespace Extensions.Triggers
{
    public class WeatherTriggerBinding : ITriggerBinding
    {
        public Type TriggerValueType => typeof(WeatherPayload);

        public IReadOnlyDictionary<string, Type> BindingDataContract { get; } = new Dictionary<string, Type>();

        private readonly WeatherTriggerAttribute _attribute;
        private readonly ParameterInfo _parameter;
        private readonly INameResolver _nameResolver;
        private readonly IWeatherService _weatherService;

        private readonly Task<ITriggerData> _emptyBindingDataTask =
            Task.FromResult<ITriggerData>(new TriggerData(null, new Dictionary<string, object>()));

        public WeatherTriggerBinding(ParameterInfo parameter, INameResolver nameResolver,
            IWeatherService weatherService, WeatherTriggerAttribute attribute)
        {
            this._parameter = parameter;
            this._nameResolver = nameResolver;
            this._attribute = attribute;
            this._weatherService = weatherService;
        }
        public Task<ITriggerData> BindAsync(object value, ValueBindingContext context)
        { 
            return _emptyBindingDataTask;
        }

        public Task<IListener> CreateListenerAsync(ListenerFactoryContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return Task.FromResult<IListener>(new WeatherTriggerListener(context.Executor,
                this._weatherService, this._attribute));
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            return new WeatherTriggerParameterDescriptor()
            {
                Name = _parameter.Name,
                Type = "WeatherTrigger",
                CityName = _attribute.CityName,
                TemperatureThreshold = _attribute.TemperatureThreshold
            };
        }
    }
}
