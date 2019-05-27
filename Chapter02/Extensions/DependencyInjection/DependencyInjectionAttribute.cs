using Microsoft.Azure.WebJobs.Description;
using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions.DependencyInjection
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter)]
    public class DependencyInjectionAttribute : Attribute
    {
        public DependencyInjectionAttribute(Type type)
        {
            Type = type;
        }
        public Type Type { get; }
    }
}
