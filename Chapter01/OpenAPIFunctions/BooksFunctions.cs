using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Aliencube.AzureFunctions.Extensions.OpenApi.Attributes;
using Microsoft.OpenApi.Models;
using System.Net;
using Models;
using Aliencube.AzureFunctions.Extensions.OpenApi;
using System.Reflection;
using Microsoft.OpenApi;
using Repository;
using System.Linq;
using System.Collections.Generic;

namespace OpenAPIFunctions
{
    public static class BooksFunctions
    {
        [FunctionName(nameof(GetBooks))]
        [OpenApiOperation("books", Description = "Retrieves a list of books filtered by the title")]
        [OpenApiParameter("title", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
        [OpenApiResponseBody(HttpStatusCode.OK, "application/json", typeof(IEnumerable<BookModel>), Description ="The books that contains the filter in the title")]
        public static IActionResult GetBooks(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "books")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"Call {nameof(GetBooks)}");

            var books = BooksRepository.Books;
            if (req.Query.ContainsKey("title"))
            {
                var titleFilter = req.Query["title"];
                books = books.Where(b => b.Title.Contains(titleFilter));
            }

            return (ActionResult)new OkObjectResult(books);
        }

    }
}
