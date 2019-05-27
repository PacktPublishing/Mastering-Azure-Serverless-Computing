using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.Host.Listeners;
using WeatherMap.Entities;
using WeatherMap.Services;

namespace Extensions.Triggers
{
    public class WeatherTriggerListener : IListener
    {

        private readonly ITriggeredFunctionExecutor _executor;
        private CancellationTokenSource _listenerStoppingTokenSource;

        private readonly IWeatherService _weatherService;
        private readonly WeatherTriggerAttribute _attribute;

        private Task _listenerTask;

        public WeatherTriggerListener(ITriggeredFunctionExecutor executor,
            IWeatherService weatherService, WeatherTriggerAttribute attribute)
        {
            this._executor = executor;
            this._weatherService = weatherService;
            this._attribute = attribute;
        }

        public void Cancel()
        {
            StopAsync(CancellationToken.None).Wait();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _listenerStoppingTokenSource = new CancellationTokenSource();
                var factory = new TaskFactory();
                var token = _listenerStoppingTokenSource.Token;
                _listenerTask = factory.StartNew(async () => await ListenerAction(token), token);
            }
            catch (Exception)
            {
                throw;
            }

            return _listenerTask.IsCompleted ? _listenerTask : Task.CompletedTask;
        }

        private async Task ListenerAction(CancellationToken token)
        {
            this._weatherService.ApiKey = this._attribute.ApiKey;
            var cityData = new CityInfo();
            double lastTemperature = 0;

            while (!token.IsCancellationRequested)
            {
                try
                {
                    cityData = await this._weatherService.GetCityInfoAsync(this._attribute.CityName);
                }
                catch (Exception)
                {
                    cityData = null;
                }

                if (cityData != null && 
                    Math.Abs(cityData.Temperature - lastTemperature) > this._attribute.TemperatureThreshold)
                {
                    var weatherPayload = new WeatherPayload()
                    {
                        CityName = this._attribute.CityName,
                        CurrentTemperature = cityData.Temperature,
                        Timestamp = cityData.Timestamp,
                        LastTemperature = lastTemperature
                    };

                    await _executor.TryExecuteAsync(new TriggeredFunctionData() { TriggerValue = weatherPayload }, token);

                    lastTemperature = cityData.Temperature;
                }

                await Task.Delay(TimeSpan.FromMinutes(1), token);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_listenerTask == null)
                return;

            try
            {
                _listenerStoppingTokenSource.Cancel();
            }
            finally
            {
                await Task.WhenAny(_listenerTask, Task.Delay(Timeout.Infinite, cancellationToken));

            }
        }
    }
}