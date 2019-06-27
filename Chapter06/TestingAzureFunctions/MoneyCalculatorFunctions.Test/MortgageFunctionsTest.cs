using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs;
using MoneyCalculatorFunctions.Entities;
using MoneyCalculatorFunctions.Services;
using Moq;
using Xunit;

namespace MoneyCalculatorFunctions.Test
{
    public class MortgageFunctionsTest
    {
        #region [ Run ]

        public static IEnumerable<object[]> CalculationData =>
            new List<object[]>
            {
                new object[] { 100000m, 0.06D, 180, 843.86m },
                new object[] { 100000m, 0D, 180, 555.56m },
                new object[] { 100000m, 0D, 360, 277.78m },
                new object[] { 50000m, 0.05D, 24, 2193.57m }
            };

        [Theory]
        [MemberData(nameof(CalculationData))]
        public async Task Run_RightParametersInQueryString_Calculate(decimal mortgageLoan,
            double annualInterest, uint numberOfPayments, decimal rate)
        {
            #region ARRANGE
            var provider = CultureInfo.InvariantCulture;

            var request = new Mock<HttpRequest>();
            request.Setup(e => e.Query["loan"]).Returns(mortgageLoan.ToString(provider));
            request.Setup(e => e.Query["interest"]).Returns(annualInterest.ToString(provider));
            request.Setup(e => e.Query["nPayments"]).Returns(numberOfPayments.ToString(provider));

            var mortgageCalculator = new Mock<IMortgageCalculator>();
            mortgageCalculator
                .Setup(c => c.CalculateMontlyRateAsync(mortgageLoan, annualInterest, numberOfPayments))
                .ReturnsAsync(new CalculatorResult() { Result = rate });

            var target = new MortgageFunctions(mortgageCalculator.Object);

            var logger = new Mock<ILogger>();
            var table = new Mock<ICollector<ExecutionRow>>();
            table
                .Setup(t => t.Add(It.IsAny<ExecutionRow>()))
                .Verifiable();
            #endregion ARRANGE

            #region ACT
            var actual = await target.Run(request.Object, table.Object, logger.Object);
            #endregion ACT

            #region ASSERT
            Assert.IsType<OkObjectResult>(actual);
            var objResponse = actual as OkObjectResult;
            Assert.Equal(objResponse.Value, rate);
            table.Verify(t => t.Add(It.IsAny<ExecutionRow>()), Times.Once); 
            #endregion ASSERT
        }

        public static IEnumerable<object[]> WrongCalculationData =>
            new List<object[]>
            {
                new object[] { "", "0.06", "180"},
                new object[] { "value", "0.06", "180"},
                new object[] { null, "0.06", "180"},
                new object[] { "100000", "", "180"},
                new object[] { "100000", null, "180"},
                new object[] { "100000", "value", "180"},
                new object[] { "100000", "0.06", ""},
                new object[] { "100000", "0.06", null},
                new object[] { "100000", "0.06", "value"},
            };

        [Theory]
        [MemberData(nameof(WrongCalculationData))]
        public async Task Run_WrongParametersInQueryString_Calculate(string mortgageLoan,
            string annualInterest, string numberOfPayments)
        {
            #region ARRANGE
            var provider = CultureInfo.InvariantCulture;

            var request = new Mock<HttpRequest>();
            request.Setup(e => e.Query["loan"]).Returns(mortgageLoan);
            request.Setup(e => e.Query["interest"]).Returns(annualInterest);
            request.Setup(e => e.Query["nPayments"]).Returns(numberOfPayments);

            var mortgageCalculator = new Mock<IMortgageCalculator>();
            mortgageCalculator
                .Setup(c => c.CalculateMontlyRateAsync(It.IsAny<decimal>(), It.IsAny<double>(), It.IsAny<uint>()))
                .ReturnsAsync(new CalculatorResult() { Succeed = false });

            var target = new MortgageFunctions(mortgageCalculator.Object);

            var logger = new Mock<ILogger>();
            var table = new Mock<ICollector<ExecutionRow>>();
            #endregion ARRANGE

            #region ACT
            var actual = await target.Run(request.Object, table.Object, logger.Object);
            #endregion ACT

            #region ASSERT
            Assert.IsType<BadRequestObjectResult>(actual);
            table.Verify(t => t.Add(It.IsAny<ExecutionRow>()), Times.Never); 
            #endregion ASSERT
        } 


        #endregion [ Run ]
    }

}
