using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Azure.WebJobs
{
    public static class ExecutionContextExtensions
    {
        public static string GetConfig(this ExecutionContext context, string keyName)
        {
            if (string.IsNullOrWhiteSpace(keyName))
                throw new ArgumentException(nameof(keyName));

            var config = GetConfigurations(context);
            var pair = config.FirstOrDefault(k => k.Key == keyName);
            if (pair.Equals(default(KeyValuePair<string, string>)))
                return null;
            return pair.Value;
        }

        public static IEnumerable<KeyValuePair<string, string>> GetConfigurations(this ExecutionContext context)
        {
            if (context == null)
                throw new NullReferenceException(nameof(context));

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            return config.AsEnumerable();
        }
    }
}
