using Microsoft.Azure.WebJobs.Description;
using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions.Bindings
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public class TwitterBindingAttribute : Attribute
    {
        [AppSetting(Default = "Twitter.ConsumerKey")]
        public string ConsumerKey { get;  set; }

        [AppSetting(Default = "Twitter.ConsumerSecret")]
        public string ConsumerSecret { get;  set; }

        [AppSetting(Default = "Twitter.AccessToken")]
        public string AccessToken { get;  set; }

        [AppSetting(Default = "Twitter.AccessTokenSecret")]
        public string AccessTokenSecret { get;  set; }
    }
}
