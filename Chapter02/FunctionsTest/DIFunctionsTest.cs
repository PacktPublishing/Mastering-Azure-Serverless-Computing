using Functions;
using Functions.MyServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FunctionsTest
{
    public class UnitTest1
    {
        #region [ InjectMyServiceWithBinding ]
        [Fact]
        public async Task InjectMyServiceWithBinding_RequestWithoutName_ReturnBadRequestObjectResult()
        {
            var request = new Mock<HttpRequest>();
            request.Setup(e => e.Query.ContainsKey("name")).Returns(false);
            request.Setup(e => e.Query["name"]).Returns((string)null);

            var myService = new Mock<IMyService>();
            myService.Setup(s => s.DoSomethingAsync(null))
                .ReturnsAsync((string)null);

            var logger = new Mock<ILogger>();

            var actual = await DIFunctions.InjectMyServiceWithBinding(request.Object, myService.Object, logger.Object);

            Assert.IsType<BadRequestObjectResult>(actual);
        }

        [Fact]
        public async Task InjectMyServiceWithBinding_RequestWithName_ReturnOkObjectResult()
        {
            var name = "Mastering Serverless";
            var responseMessage = $"Hello, {name}!";

            var request = new Mock<HttpRequest>();
            request.Setup(e => e.Query.ContainsKey("name")).Returns(true);
            request.Setup(e => e.Query["name"]).Returns(name);

            var myService = new Mock<IMyService>();
            myService.Setup(s => s.DoSomethingAsync(name))
                .ReturnsAsync(responseMessage);

            var logger = new Mock<ILogger>();

            var actual = await DIFunctions.InjectMyServiceWithBinding(request.Object, myService.Object, logger.Object);

            Assert.IsType<OkObjectResult>(actual);
            var response = actual as OkObjectResult;
            Assert.Equal(response.Value, responseMessage);
        }
        #endregion [ InjectMyServiceWithBinding ]

        #region [ InjectMyServiceWithServiceLocator ]
        [Fact]
        public async Task InjectMyServiceWithServiceLocator_RequestWithoutName_ReturnBadRequestObjectResult()
        {
            var request = new Mock<HttpRequest>();
            request.Setup(e => e.Query.ContainsKey("name")).Returns(false);
            request.Setup(e => e.Query["name"]).Returns((string)null);

            var myService = new Mock<IMyService>();
            myService.Setup(s => s.DoSomethingAsync(null))
                .ReturnsAsync((string)null);

            var logger = new Mock<ILogger>();

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(s => s.GetService(typeof(IMyService)))
                .Returns(myService.Object);

            ServiceLocator.DefaultProvider = serviceProvider.Object;

            var actual = await DIFunctions.InjectMyServiceWithServiceLocator(request.Object, logger.Object);

            Assert.IsType<BadRequestObjectResult>(actual);
        }

        [Fact]
        public async Task InjectMyServiceWithServiceLocator_RequestWithName_ReturnOkObjectResult()
        {
            var name = "Mastering Serverless";
            var responseMessage = $"Hello, {name}!";

            var request = new Mock<HttpRequest>();
            request.Setup(e => e.Query.ContainsKey("name")).Returns(true);
            request.Setup(e => e.Query["name"]).Returns(name);

            var myService = new Mock<IMyService>();
            myService.Setup(s => s.DoSomethingAsync(name))
                .ReturnsAsync(responseMessage);

            var logger = new Mock<ILogger>();

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(s => s.GetService(typeof(IMyService)))
                .Returns(myService.Object);

            ServiceLocator.DefaultProvider = serviceProvider.Object;


            var actual = await DIFunctions.InjectMyServiceWithServiceLocator(request.Object, logger.Object);

            Assert.IsType<OkObjectResult>(actual);
            var response = actual as OkObjectResult;
            Assert.Equal(response.Value, responseMessage);
        }
        #endregion [ InjectMyServiceWithServiceLocator ]
    }
}
