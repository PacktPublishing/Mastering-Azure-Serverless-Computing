using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MoneyCalculatorFunctions.Services
{
    public class MortgageCalculator : IMortgageCalculator
    {
        private ILogger log;

        public MortgageCalculator(ILoggerFactory factory)
        {
            if (factory != null)
            {
                this.log = factory.CreateLogger<MortgageCalculator>();
            }
        }

        public Task<CalculatorResult> CalculateMontlyRateAsync(decimal mortgageLoan, double annualInterest, uint numberOfPayments)
        {
            this.log?.LogTrace($"--> Enter {nameof(CalculateMontlyRateAsync)}", mortgageLoan, annualInterest,
                numberOfPayments);
            CalculatorResult result = null;

            var sw = Stopwatch.StartNew();

            result = ValidateInput(mortgageLoan, annualInterest, numberOfPayments);

            if (result.Succeed)
            {
                double rate = 0;
                if (annualInterest == 0)
                {
                    rate = (double)mortgageLoan / numberOfPayments;
                }
                else
                {
                    var montlyInterest = annualInterest / 12;
                    var k = Math.Pow(1 + montlyInterest, numberOfPayments);
                    rate = (double)mortgageLoan * (montlyInterest * k / (k - 1));
                }

                result.Result = (decimal)Math.Round(rate, 2, MidpointRounding.AwayFromZero);
            }

            sw.Stop();

            this.log?.LogMetric($"{nameof (CalculateMontlyRateAsync)} Duration", sw.ElapsedMilliseconds);

            this.log?.LogTrace($"<-- Exit {nameof(CalculateMontlyRateAsync)}", mortgageLoan, annualInterest,
                numberOfPayments, result);

            return Task.FromResult(result);
        }

        private CalculatorResult ValidateInput(decimal mortgageLoan, double annualInterest, uint numberOfPayments)
        {
            var result = new CalculatorResult();

            if (mortgageLoan < 0)
            {
                result.Succeed = false;
                result.Error = new CalculatorError() { Code = MortgageCalculatorErrors.MortgageNotValid, Message = "Mortgage not valid" };
                this.log?.LogError($"{nameof(CalculateMontlyRateAsync)} Mortgage not valid", mortgageLoan);
            }
            else if (annualInterest < 0)
            {
                result.Succeed = false;
                result.Error = new CalculatorError() { Code = MortgageCalculatorErrors.AnnualInterestNotValid, Message = "Annual interest not valid" };
                this.log?.LogError($"{nameof(CalculateMontlyRateAsync)} Annual interest not valid", annualInterest);
            }

            return result;
        }
    }
}