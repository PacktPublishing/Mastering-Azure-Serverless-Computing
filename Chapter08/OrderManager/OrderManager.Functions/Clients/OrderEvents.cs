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
    public static class OrderEvents
    {
        [FunctionName(FunctionNames.OrderConfirmedFunction)]
        public static async Task<HttpResponseMessage> OrderConfirmed(
            [HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequestMessage req,
            [OrchestrationClient] DurableOrchestrationClient starter,
            ILogger log)
        {
            return await SendEventToOrderAsync(req, starter, Events.OrderPaid, log);
        }

        [FunctionName(FunctionNames.OrderCancelledFunction)]
        public static async Task<HttpResponseMessage> OrderCancelled(
            [HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequestMessage req,
            [OrchestrationClient] DurableOrchestrationClient starter,
            ILogger log)
        {
            return await SendEventToOrderAsync(req, starter, Events.OrderCancelled, log);
        }

        private static async Task<HttpResponseMessage> SendEventToOrderAsync(HttpRequestMessage req,
            DurableOrchestrationClient starter,
            string orderEvent,
            ILogger log)
        {
            var jsonContent = await req.Content.ReadAsStringAsync();
            OrderEventDto orderEventDto = null;

            orderEventDto = JsonConvert.DeserializeObject<OrderEventDto>(jsonContent);
            if (orderEventDto != null && !string.IsNullOrWhiteSpace(orderEventDto.OrderId))
            {
                log.LogInformation($"SendEventToOrder {orderEvent} --> Order {orderEventDto.OrderId}!");
                await starter.RaiseEventAsync(orderEventDto.OrderId, orderEvent, null);
                return starter.CreateCheckStatusResponse(req, orderEventDto.OrderId);
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest) { ReasonPhrase = "Order not valid" };
        }
    }
}
