using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzureDayReloaded
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Counter
    {
        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("lastUpdate")]
        public DateTime? LastUpdate { get; set; }

        public void Add(int amount)
        {
            this.Value += amount;
            this.LastUpdate = DateTime.UtcNow;
        }

        public Task Reset()
        {
            this.Value = 0;
            this.LastUpdate = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        public Task<int> GetValue()
        {
            return Task.FromResult(this.Value);
        }

        public void Delete()
        {
            Entity.Current.DeleteState();
        }

        [FunctionName(nameof(Counter))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx)
        {
            ctx.DispatchAsync<Counter>();

            return Task.CompletedTask;
        }

    }
}
