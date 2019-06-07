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
    public static class GenerateInvoice
    {
        [FunctionName(FunctionNames.GenerateInvoiceFunction)]
        [StorageAccount("StorageAccount")]
        public static Task<string> Run([ActivityTrigger] Order order,
            IBinder outputBinder,
            ILogger log)
        {
            log.LogInformation($"[START ACTIVITY] --> {FunctionNames.GenerateInvoiceFunction} for order: {order}");
            var fileName = order.GetInvoicePath(SourceNames.InvoicesContainer);
            using (var outputBlob = outputBinder.Bind<TextWriter>(new BlobAttribute(fileName)))
            {
                outputBlob.WriteLine($"Invoice generated on {DateTime.Now} for the order {order.Id} of {order.CreationTimestamp}");
                outputBlob.WriteLine($"");
                outputBlob.WriteLine($"Customer : {order.Customer}");
                outputBlob.WriteLine($"Email: {order.CustomerMail}");
                outputBlob.WriteLine($"");
                outputBlob.WriteLine($"Amount : {order.Amount}€");
            }

            return Task.FromResult(fileName);
        }
    }
}
