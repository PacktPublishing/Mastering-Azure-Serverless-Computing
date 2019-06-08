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
    public static class FinalizeOrder
    {
        //[FunctionName(FunctionNames.FinalizeOrderFunction)]
        //public static async Task<Order> Run([ActivityTrigger] OrderStateChange orderStateChange,
        //    [Table(SourceNames.OrdersTable, Connection = "StorageAccount")] CloudTable orderTable,
        //    ILogger log)
        //{
        //    log.LogInformation($"[START ACTIVITY] --> {FunctionNames.FinalizeOrderFunction} for OrderUd={orderStateChange.OrderId}");
        //    bool retVal = true;
        //    Order order = null;

        //    (order, retVal) = await GetOrderFromTableAsync(orderStateChange, orderTable, log);

        //    if (retVal)
        //    {
        //        if (order != null)
        //        {
        //            order.State = orderStateChange.NewOrderState;
        //            if (!await orderTable.InsertOrReplaceAsync(order))
        //            {
        //                log.LogError($"Error during Updating Order {orderStateChange.OrderId}");
        //            }
        //        }
        //        else
        //        {
        //            log.LogWarning($"The Order {orderStateChange.OrderId} doesn't exist in the storage");
        //            order = null;
        //        }
        //    }

        //    return order;
        //}

        private static async Task<(Order order, bool result)> GetOrderFromTableAsync(OrderStateChange orderStateChange,
            CloudTable orderTable, ILogger log)
        {
            var result = true;
            Order order = null;
            try
            {
                order = await orderTable.GetOrderByIdAsync(orderStateChange.OrderId);
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"Error during retriving Order {orderStateChange.OrderId}");
                result = false;
            }

            return (order, result);
        }
    }
}
