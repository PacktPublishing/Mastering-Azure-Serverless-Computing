using System;
using System.Data.Common;
using System.Reflection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace MonitoringAzureFunctions
{
    public static class MonitoringFunctions
    {
        [FunctionName("TimerTriggerFunction")]
        public static void Run([TimerTrigger("0 */2 * * * *")]TimerInfo myTimer, ILogger log)
        {
            var executionTimestamp = DateTime.Now;
            log.LogInformation($"C# Timer trigger function executed at: {executionTimestamp}");

            log.LogTrace($"Is past due: {myTimer.IsPastDue}");
            log.LogTrace($"Schedule: {myTimer.Schedule}");
            log.LogTrace($"Schedule Status Last: {myTimer.ScheduleStatus.Last}");
            log.LogTrace($"Schedule Status Next: {myTimer.ScheduleStatus.Next}");
            log.LogTrace($"Schedule Status LastUpdated: {myTimer.ScheduleStatus.LastUpdated}");

        }
    }
}
