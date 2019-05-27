using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Extensions.Triggers;
using Extensions.Bindings;
using Extensions;
using Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Azure.WebJobs
{
    public static class WebJobBuilderExtensions
    {
        public static IWebJobsBuilder UseWeatherTrigger(this IWebJobsBuilder builder)
        {
            if (builder == null)
                throw new NullReferenceException(nameof(builder));

            builder.AddExtension<WeatherTriggerConfigProvider>();

            // Add here all the configuration code for the extension

            return builder;
        }

        public static IWebJobsBuilder UseTwitterBinding(this IWebJobsBuilder builder)
        {
            if (builder == null)
                throw new NullReferenceException(nameof(builder));

            builder.AddExtension<TwitterBindingConfigProvider>();

            return builder;
        }

        public static IWebJobsBuilder UseDependencyInjection(this IWebJobsBuilder builder)
        {
            if (builder == null)
                throw new NullReferenceException(nameof(builder));

            var serviceProvider = builder.Services.BuildServiceProvider();

            builder.AddExtension(new DependencyInjectionConfigProvider(serviceProvider));

            return builder;
        }
    }
}
