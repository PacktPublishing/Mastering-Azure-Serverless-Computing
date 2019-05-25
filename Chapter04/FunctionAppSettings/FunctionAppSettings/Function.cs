using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace FunctionAppSettings
{
    public static class Function1
    {
        [FunctionName("GetConfigValue")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log,
            ExecutionContext context)
        {
            string key = req.Query["key"];

            log.LogInformation($"Retrieve config for {key}");

            if (key == null)
                return new BadRequestObjectResult("Please pass a key on the query string");

            var configValue = context.GetConfig(key);

            return new OkObjectResult(new { key, value = configValue });

        }

    }
}
