using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using OrderManager.Core;
using OrderManager.Core.Entities;
using OrderManager.Functions.Extensions;

namespace OrderManager.Functions.Activities
{
    public static class AddOrder
    {
        [FunctionName(FunctionNames.AddOrderFunction)]
        public static async Task<bool> Run([ActivityTrigger] Order order,
            [Table(SourceNames.OrdersTable, Connection = "StorageAccount")] CloudTable giftTable,
            ILogger log)
        {
            log.LogInformation($"[START ACTIVITY] --> {FunctionNames.AddOrderFunction} for {order}");
            bool retVal = false;
            try
            {
                retVal = await giftTable.InsertAsync(order);
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"Error during adding order  {order}");
                retVal = false;
            }
            return retVal;
        }
    }
}
