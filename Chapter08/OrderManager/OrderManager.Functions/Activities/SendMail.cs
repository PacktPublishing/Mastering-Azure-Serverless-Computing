using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrderManager.Core;
using OrderManager.Core.Entities;
using SendGrid.Helpers.Mail;
using System.Diagnostics;

namespace OrderManager.Functions.Activities
{
    public static class SendMail
    {
        [FunctionName(FunctionNames.SendMailFunction)]
        public static bool Run([ActivityTrigger] Order order,
             [SendGrid(ApiKey = "SendGridApiKey")] out SendGridMessage message,
             IBinder invoiceBinder,
             ILogger log)
        {
            log.LogInformation($"[START ACTIVITY] --> {FunctionNames.SendMailFunction} for order: {order}");

            var invoiceFilename = order.GetInvoicePath(SourceNames.InvoicesContainer);
            log.LogInformation($"File Processed : {invoiceFilename}");
            log.LogInformation($"Order: {order}");
            log.LogInformation($"Customer mail: {order.CustomerMail}");
            using (var inputBlob = invoiceBinder.Bind<TextReader>(new BlobAttribute(invoiceFilename)))
            {
                message = CreateMailMessage(order, inputBlob);
            }
            return true;
        }

        private static SendGridMessage CreateMailMessage(Order order, TextReader inputBlob)
        {
            var message = new SendGridMessage()
            {
                Subject = TextUtility.GenerateMailSubject(order),
                From = new EmailAddress("order@ordermanager.com")
            };
            message.AddTo(new EmailAddress(order.CustomerMail));

            message.AddContent("text/plain", TextUtility.GenerateMailBody(order));

            if (order.State == OrderStatus.Paid)
            {
                var buffer = inputBlob.ReadBuffer();
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(buffer);
                var text = System.Convert.ToBase64String(plainTextBytes);

                message.AddAttachment(order.GetInvoicePath(SourceNames.InvoicesContainer),
                    text, "text/plain", "attachment", "Invoice File");
            }
            return message;
        }


    }
}
