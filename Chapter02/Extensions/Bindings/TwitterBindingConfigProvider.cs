using Extensions.Triggers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Twitter.Services;

namespace Extensions.Bindings
{
    [Extension("Twitter")]
    public class TwitterBindingConfigProvider : IExtensionConfigProvider
    {
        private readonly INameResolver _nameResolver;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ITwitterService _twitterService;

        public TwitterBindingConfigProvider(INameResolver nameResolver, 
            ILoggerFactory loggerFactory, ITwitterService twitterService)
        {
            this._nameResolver = nameResolver;
            this._loggerFactory = loggerFactory;
            this._twitterService = twitterService;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            var bindingRule = context.AddBindingRule<TwitterBindingAttribute>();
            bindingRule.AddValidator(ValidateTwitterConfig);
            bindingRule.BindToCollector<OpenType>(typeof(TwitterBindingCollectorConverter), _nameResolver, _twitterService);
            bindingRule.BindToInput<TwitterBinder>(typeof(TwitterBindingConverter), _nameResolver, _twitterService);
        }


        private void ValidateTwitterConfig(TwitterBindingAttribute attribute, Type paramType)
        {
            if (string.IsNullOrEmpty(attribute.AccessToken))
                throw new InvalidOperationException($"Twitter AccessToken must be set either via the attribute property or via configuration.");

            if (string.IsNullOrEmpty(attribute.AccessTokenSecret))
                throw new InvalidOperationException($"Twitter AccessTokenSecret must be set either via the attribute property or via configuration.");

            if (string.IsNullOrEmpty(attribute.ConsumerKey))
                throw new InvalidOperationException($"Twitter ConsumerKey must be set either via the attribute property or via configuration.");

            if (string.IsNullOrEmpty(attribute.ConsumerSecret))
                throw new InvalidOperationException($"Twitter ConsumerSecret must be set either via the attribute property or via configuration.");
        }
    }
}
