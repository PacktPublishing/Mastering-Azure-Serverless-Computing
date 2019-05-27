using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Extensions.Triggers;
using Extensions.Bindings;

namespace Functions
{
    public static class WeatherFunctions
    {

        [FunctionName(nameof(MilanWeatherCheck))]
        public static void MilanWeatherCheck(
           [WeatherTrigger("Milan,IT", 0.1)] WeatherPayload req,
           ILogger log)
        {
            var message = $"{req.CityName} [{req.CurrentTemperature}] at {req.Timestamp}";
            log.LogWarning(message);
        }

        [FunctionName(nameof(RomeWeatherCheck))]
        public static async Task RomeWeatherCheck(
            [WeatherTrigger("Rome,IT", 0.1)] WeatherPayload req,
            [TwitterBinding] IAsyncCollector<string> tweetMessages,
            ILogger log)
        {
            var message = $"{req.CityName} [{req.CurrentTemperature}] at {req.Timestamp}";
            log.LogWarning(message);
            await tweetMessages.AddAsync(message);
        }

        //[FunctionName(nameof(PeriodicTweet))]
        //public static async Task PeriodicTweet(
        //    [TimerTrigger("0 */5 * * * *")] TimerInfo timer,
        //    [TwitterBinding()] TwitterBinder twitter,
        //    ILogger log)
        //{
        //    // .....
        //}

        //[FunctionName(nameof(PeriodicTweets))]
        //public static async Task PeriodicTweets(
        //    [TimerTrigger("0 */5 * * * *")] TimerInfo timer,
        //    [TwitterBinding] IAsyncCollector<string> tweetMessages,
        //    ILogger log)
        //{
        //    // .....
        //}
    }
}
