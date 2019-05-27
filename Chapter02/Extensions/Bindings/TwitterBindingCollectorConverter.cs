using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Twitter.Services;

namespace Extensions.Bindings
{
    public class TwitterBindingCollectorConverter : IConverter<TwitterBindingAttribute, IAsyncCollector<string>>
    {
        private readonly INameResolver _nameResolver;
        private readonly ITwitterService _twitterService;

        public TwitterBindingCollectorConverter(INameResolver nameResolver, ITwitterService twitterService)
        {
            _nameResolver = nameResolver;
            _twitterService = twitterService;
        }

        public IAsyncCollector<string> Convert(TwitterBindingAttribute attribute)
        {
            return new TwitterBindingAsyncCollector(attribute, _twitterService, _nameResolver);
        }
    }
}
