using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MoneyCalculatorFunctions.Services
{
    public class MortgageCalculator : IMortgageCalculator
    {
        private ILogger log;

        public MortgageCalculator(ILogger log)
        {
            this.log = log;
        }

        public Task<CalculatorResult> CalculateMontlyRateAsync(decimal mortgageLoan, double annualInterest, uint numberOfPayments)
        {
            CalculatorResult result = null;

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
            return Task.FromResult(result);
        }

        private CalculatorResult ValidateInput(decimal mortgageLoan, double annualInterest, uint numberOfPayments)
        {
            var result = new CalculatorResult();

            if (mortgageLoan < 0)
            {
                result.Succeed = false;
                result.Error = new CalculatorError() { Code = MortgageCalculatorErrors.MortgageNotValid, Message = "Mortgage not valid" };
            }
            else if (annualInterest < 0)
            {
                result.Succeed = false;
                result.Error = new CalculatorError() { Code = MortgageCalculatorErrors.AnnualInterestNotValid, Message = "Annual interest not valid" };
            }

            return result;
        }
    }
}