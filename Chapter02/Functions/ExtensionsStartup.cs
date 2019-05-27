using Extensions.Triggers;
using Functions;
using Functions.MyServices;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Twitter.Services;
using WeatherMap.Services;

[assembly: WebJobsStartup(typeof(ExtensionsStartup))]

public class ExtensionsStartup : IWebJobsStartup
{
    public void Configure(IWebJobsBuilder builder)
    {
        builder.UseWeatherTrigger();
        builder.UseTwitterBinding();

        builder.Services.AddTransient<IWeatherService, WeatherService>();
        builder.Services.AddTransient<ITwitterService, TwitterService>();

        builder.Services.AddScoped<IMyService, MyService>();

        builder.UseDependencyInjection();

        ServiceLocator.DefaultProvider = builder.Services.BuildServiceProvider();
    }
}

