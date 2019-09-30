using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace BindingExpressionsExample
{
    public static class BindingExpressionsExample
    {
        [FunctionName("LogQueueMessage")]
        public static void Run(
            [QueueTrigger("%queueappsetting%")]string myQueueItem,
            DateTimeOffset insertionTime,
            ILogger log)
        {
            log.LogInformation($"Message content: {myQueueItem}");
            log.LogInformation($"Created at: {insertionTime}");
        }
    }
}
