using System.Threading.Tasks;

namespace MoneyCalculatorFunctions.Services
{
    public interface IMortgageCalculator
    {
        Task<CalculatorResult> CalculateMontlyRateAsync(decimal mortgageLoan, double annualInterest, 
            uint numberOfPayments);
    }
}