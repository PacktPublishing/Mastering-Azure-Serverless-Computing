using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrderManager.Core;
using OrderManager.Core.Entities;
using System.Threading;

namespace OrderManager.Functions.Orchetsrators
{
    public static class OrderWorkflow
    {
        [FunctionName(FunctionNames.OrderWorkflowFunction)]
        public static async Task Run([OrchestrationTrigger] DurableOrchestrationContext context,
            ILogger log)
        {
            log.LogInformation($"[START ORCHESTRATOR] --> {FunctionNames.OrderWorkflowFunction}");
            var order = context.GetInput<Order>();

            log.LogTrace($"Adding Order {order}");
            var addResult = await context.CallActivityWithRetryAsync<bool>(FunctionNames.AddOrderFunction,
                new RetryOptions(TimeSpan.FromSeconds(1), 10), order);

            if (addResult)
            {
                DateTime orderDeadline = context.CurrentUtcDateTime.AddMinutes(10);

                var orderPaidEvent = context.WaitForExternalEvent<bool>(Events.OrderPaid);
                var orderCancelledEvent = context.WaitForExternalEvent<bool>(Events.OrderCancelled);
                var cancelTimer= context.CreateTimer(orderDeadline,default(CancellationToken));

                var taskCompleted = await Task.WhenAny(orderPaidEvent, orderCancelledEvent, cancelTimer);
                if (taskCompleted == orderCancelledEvent && taskCompleted== cancelTimer)
                {
                    log.LogWarning($"Order Cancelled : {order}");
                    // Order cancelled
                }
                else if (taskCompleted == orderPaidEvent)
                {
                    log.LogTrace($"Order Paid : {order}");
                }


            }
        }
    }
}
