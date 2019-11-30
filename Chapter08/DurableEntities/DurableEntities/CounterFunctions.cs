using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureDayReloaded
{
    public class CounterFunctions
    {
        [FunctionName("DeleteCounter")]
        public async Task<HttpResponseMessage> DeleteCounter(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "Counter/{entityKey}")] HttpRequestMessage req,
            [DurableClient] IDurableEntityClient client,
            string entityKey)
        {
            var entityId = new EntityId("Counter", entityKey);

            await client.SignalEntityAsync(entityId, "Delete");

            return req.CreateResponse(HttpStatusCode.Accepted);
        }

        [FunctionName("GetCounter")]
        public static async Task<HttpResponseMessage> GetCounter(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Counter/{entityKey}")] HttpRequestMessage req,
            [DurableClient] IDurableEntityClient client,
            string entityKey)
        {
            var entityId = new EntityId("Counter", entityKey);

            var state = await client.ReadEntityStateAsync<Counter>(entityId);

            return req.CreateResponse(state);
        }

        [FunctionName("IncrementCounter")]
        public static async Task<HttpResponseMessage> IncrementCounter(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "Counter/{entityKey}/{increment?}")] HttpRequestMessage req,
            [DurableClient] IDurableEntityClient client,
            string entityKey,
            int? increment)
        {
            var entityId = new EntityId("Counter", entityKey);
            if (!increment.HasValue)
                increment = 1;

            await client.SignalEntityAsync(entityId, "Add", increment.Value);

            return req.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}
