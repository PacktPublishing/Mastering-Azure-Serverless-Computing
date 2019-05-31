#define FUNCSTARTUP
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MoneyCalculatorFunctions;
using MoneyCalculatorFunctions.Services;


#if FUNCSTARTUP
[assembly: FunctionsStartup(typeof(Startup))]
namespace MoneyCalculatorFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IMortgageCalculator, MortgageCalculator>();
        }
    }
}
#else
[assembly: WebJobsStartup(typeof(Startup))]
namespace MoneyCalculatorFunctions
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services.AddTransient<IMortgageCalculator, MortgageCalculator>();
        }
    }
}
#endif

