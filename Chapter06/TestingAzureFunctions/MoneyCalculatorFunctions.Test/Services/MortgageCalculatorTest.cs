using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MoneyCalculatorFunctions.Services;
using Moq;
using Xunit;

namespace MoneyCalculatorFunctions.Test.Services
{
    public class MortgageCalculatorTest
    {
        #region [ CalculateMontlyRateAsync ]

        [Fact]
        public async Task CalculateMontlyRateAsync_MortgageLessThanZero_Error()
        {
            decimal mortgageLoan = -100;
            double annualInterest = 0;
            uint numberOfPayments = 10;

            var logFactoryMock = new Mock<ILoggerFactory>();
            var logMock = new Mock<ILogger>();

            logFactoryMock
                .Setup(e => e.CreateLogger(nameof(MortgageCalculator)))
                .Returns(logMock.Object);

            var target = new MortgageCalculator(logFactoryMock.Object);

            var actual = await target.CalculateMontlyRateAsync(mortgageLoan, annualInterest, numberOfPayments);

            Assert.False(actual.Succeed);
            Assert.NotNull(actual.Error);
            Assert.Equal(actual.Error.Code, MortgageCalculatorErrors.MortgageNotValid);
        }

        [Fact]
        public async Task CalculateMontlyRateAsync_AnnualInterestLessThanZero_Error()
        {
            decimal mortgageLoan = 100000;
            double annualInterest = -10;
            uint numberOfPayments = 10;

            var logFactoryMock = new Mock<ILoggerFactory>();
            var logMock = new Mock<ILogger>();

            logFactoryMock
                .Setup(e => e.CreateLogger(nameof(MortgageCalculator)))
                .Returns(logMock.Object);

            var target = new MortgageCalculator(logFactoryMock.Object);

            var actual = await target.CalculateMontlyRateAsync(mortgageLoan, annualInterest, numberOfPayments);

            Assert.False(actual.Succeed);
            Assert.NotNull(actual.Error);
            Assert.Equal(actual.Error.Code, MortgageCalculatorErrors.AnnualInterestNotValid);
        }

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
        public async Task CalculateMontlyRateAsync_ParametersOk_Calculate(decimal mortgageLoan,
            double annualInterest, uint numberOfPayments, decimal rate)
        {
            var logFactoryMock = new Mock<ILoggerFactory>();
            var logMock = new Mock<ILogger>();

            logFactoryMock
                .Setup(e => e.CreateLogger(nameof(MortgageCalculator)))
                .Returns(logMock.Object);

            var target = new MortgageCalculator(logFactoryMock.Object);

            var actual = await target.CalculateMontlyRateAsync(mortgageLoan, annualInterest, numberOfPayments);

            Assert.True(actual.Succeed);
            Assert.Null(actual.Error);
            Assert.Equal(actual.Result.Value, rate);
        }

        #endregion [ CalculateMontlyRateAsync ]
    }

}
