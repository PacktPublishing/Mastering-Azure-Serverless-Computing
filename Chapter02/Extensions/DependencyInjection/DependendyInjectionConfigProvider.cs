using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherMap.Services;

namespace Extensions.DependencyInjection
{
    [Extension("DependencyInjection")]
    public class DependencyInjectionConfigProvider : IExtensionConfigProvider
    {
        private readonly ServiceProvider _serviceProvider;

        public DependencyInjectionConfigProvider(ServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            context.AddBindingRule<DependencyInjectionAttribute>()
                .BindToInput<dynamic>((a,c) => ServiceBuilder(a,c));

        }


        private Task<object> ServiceBuilder(DependencyInjectionAttribute attribute, ValueBindingContext context)
        {
            var serviceInstance = _serviceProvider.GetRequiredService(attribute.Type);

            return Task.FromResult<dynamic>(serviceInstance);
        }
    }
}
