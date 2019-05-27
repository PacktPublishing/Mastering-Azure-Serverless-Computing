using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Extensions.DependencyInjection;
using Functions.MyServices;

namespace Functions
{
    public static class DIFunctions
    {
        [FunctionName(nameof(InjectMyServiceWithBinding))]
        public static async Task<IActionResult> InjectMyServiceWithBinding(
            [HttpTrigger(AuthorizationLevel.Function, "get",  Route = null)] HttpRequest req,
            [DependencyInjection(typeof(IMyService))] IMyService myService,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            var responseMessage = await myService.DoSomethingAsync(name);

            return responseMessage != null
                ? (ActionResult)new OkObjectResult(responseMessage)
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }

        [FunctionName(nameof(InjectMyServiceWithServiceLocator))]
        public static async Task<IActionResult> InjectMyServiceWithServiceLocator(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var myService = ServiceLocator.GetService<IMyService>();

            string name = req.Query["name"];

            var responseMessage = await myService.DoSomethingAsync(name);

            return responseMessage != null
                ? (ActionResult)new OkObjectResult(responseMessage)
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
