using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi;
using Aliencube.AzureFunctions.Extensions.OpenApi.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi;

namespace OpenAPIFunctions
{
    public static class OpenAPIFunctions
    {
        [FunctionName(nameof(RenderOpenApiDocument))]
        [OpenApiIgnore]
        public static async Task<IActionResult> RenderOpenApiDocument(
                    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "openapi")] HttpRequest req,
                    ILogger log)
        {
            var ver = OpenApiSpecVersion.OpenApi3_0;
            var ext = OpenApiFormat.Json;

            var settings = new AppSettings();
            var helper = new DocumentHelper();
            var document = new Document(helper);
            var result = await document.InitialiseDocument()
                                       .AddMetadata(settings.OpenApiInfo)
                                       .AddServer(req, settings.HttpSettings.RoutePrefix)
                                       .Build(Assembly.GetExecutingAssembly())
                                       .RenderAsync(ver, ext)
                                       .ConfigureAwait(false);
            var response = new ContentResult()
            {
                Content = result,
                ContentType = "application/json",
                StatusCode = (int)HttpStatusCode.OK
            };

            return response;
        }

        [FunctionName(nameof(RenderSwaggerUI))]
        [OpenApiIgnore]
        public static async Task<IActionResult> RenderSwaggerUI(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "openapi/ui")] HttpRequest req,
                ILogger log)
        {
            var settings = new AppSettings();
            var ui = new SwaggerUI();
            var result = await (await ui.AddMetadata(settings.OpenApiInfo)
                                 .AddServer(req, settings.HttpSettings.RoutePrefix)
                                 .BuildAsync())
                                 .RenderAsync("swagger", settings.SwaggerAuthKey)
                                 .ConfigureAwait(false);
            var response = new ContentResult()
            {
                Content = result,
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK
            };

            return response;
        }
    }
}
