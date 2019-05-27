using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Functions
{
    public static class ServiceLocator
    {
        public static IServiceProvider DefaultProvider;

        public static TService GetService<TService>()
        {
            return DefaultProvider.GetRequiredService<TService>();
        }
    }
}
