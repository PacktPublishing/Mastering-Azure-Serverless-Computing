using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MoneyCalculatorFunctions.Entities;
using MoneyCalculatorFunctions.Services;
using Newtonsoft.Json;

namespace MoneyCalculatorFunctions
{
    public class MortgageFunctions
    {
        private readonly IMortgageCalculator mortgageCalculator;

        public MortgageFunctions(IMortgageCalculator mortgageCalculator)
        {
            if (mortgageCalculator == null)
                throw new ArgumentNullException(nameof(mortgageCalculator));

            this.mortgageCalculator = mortgageCalculator;
        }

        [FunctionName(FunctionNames.MortgageCalculatorFunction)]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Table("executionsTable", Connection = "StorageAccount")] ICollector<ExecutionRow> outputTable,
            ILogger log)
        {
            log.LogInformation($"{FunctionNames.MortgageCalculatorFunction} start");

            decimal loan;
            string queryLoan = req.Query["loan"];
            if (queryLoan == null || !decimal.TryParse(queryLoan, out loan))
            {
                return new BadRequestObjectResult("Loan not valid");
            }

            double interest;
            string queryInterest = req.Query["interest"];
            if (queryInterest == null || !double.TryParse(queryInterest, out interest))
            {
                return new BadRequestObjectResult("Annual Interest not valid");
            }

            uint nPayments;
            string querynPayments = req.Query["nPayments"];
            if (querynPayments == null || !uint.TryParse(querynPayments, out nPayments))
            {
                return new BadRequestObjectResult("Number of payments not valid");
            }

            var calculatorResult = await this.mortgageCalculator.CalculateMontlyRateAsync(loan, interest, nPayments);

            var executionRow = new ExecutionRow(DateTime.Now)
            {
                AnnualInterest = interest,
                ErrorCode = calculatorResult.Error?.Code,
                MonthlyRate = calculatorResult.Result,
                MortgageLoan = loan,
                NumberOfPayments = nPayments,
                Result = calculatorResult.Succeed
            };

            outputTable.Add(executionRow);

            if (calculatorResult.Succeed)
            {
                return new OkObjectResult(calculatorResult.Result);
            }

            return new BadRequestObjectResult(calculatorResult.Error.Message);
        }
    }
}
