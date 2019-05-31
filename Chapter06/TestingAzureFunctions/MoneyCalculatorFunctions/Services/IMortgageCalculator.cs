using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MoneyCalculatorFunctions.Services
{
    public interface IMortgageCalculator
    {

        Task<CalculatorResult> CalculateMontlyRateAsync(decimal mortgageLoan, double annualInterest,
            uint numberOfPayments);
    }
}