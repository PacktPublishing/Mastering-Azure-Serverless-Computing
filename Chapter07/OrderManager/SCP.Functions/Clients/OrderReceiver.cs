using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrderManager.Core;
using OrderManager.Core.Rest;
using OrderManager.Core.Entities;

namespace OrderManager.Functions
{
    public static class OrderReceiver
    {
        [FunctionName(FunctionNames.OrderReceiverFunction)]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestMessage req,
            [OrchestrationClient] DurableOrchestrationClient starter,
            ILogger log)
        {
            var jsonContent = await req.Content.ReadAsStringAsync();
            string instanceId = null;
            OrderDto orderDto = null;

            orderDto = JsonConvert.DeserializeObject<OrderDto>(jsonContent);
            if (orderDto != null && orderDto.IsValid())
            {
                var order = orderDto.ToOrder();
                instanceId = await starter.StartNewAsync(FunctionNames.OrderWorkflowFunction, order.Id, order);
                log.LogInformation($"Order received {orderDto} - started orchestration with ID = '{instanceId}'.");
            }
            else
            {
                log.LogError($"Order not valid - {orderDto}");
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest) { ReasonPhrase = "Order not valid" };
            }

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
