using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Azure.WebJobs
{
    public static class ExecutionContextExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> GetConfigurations(this ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            return config.AsEnumerable();
        }

        public static string GetValue(this ExecutionContext context, string key)
        {
            var configurations = context.GetConfigurations();
            var keyPair = configurations.FirstOrDefault(c => c.Key == key);
            if (keyPair.Key != null)
                return keyPair.Value;
            return null;
        }
    }
}
