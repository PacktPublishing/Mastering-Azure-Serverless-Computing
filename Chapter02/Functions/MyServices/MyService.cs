using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Functions.MyServices
{
    public class MyService : IMyService
    {
        public Task<string> DoSomethingAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;
            return Task.FromResult($"Hello, {name}!!");
        }
    }
}
