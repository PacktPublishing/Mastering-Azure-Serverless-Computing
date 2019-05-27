using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Functions.MyServices
{
    public interface IMyService
    {
        Task<string> DoSomethingAsync(string name);
    }
}
