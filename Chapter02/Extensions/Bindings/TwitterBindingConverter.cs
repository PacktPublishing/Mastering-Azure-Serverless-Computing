using Microsoft.Azure.WebJobs;
using Twitter.Services;

namespace Extensions.Bindings
{
    public class TwitterBindingConverter : IConverter<TwitterBindingAttribute, TwitterBinder>
    {
        private readonly INameResolver _nameResolver;
        private readonly ITwitterService _twitterService;

        public TwitterBindingConverter(INameResolver nameResolver, ITwitterService twitterService)
        {
            _nameResolver = nameResolver;
            _twitterService = twitterService;
        }

        public TwitterBinder Convert(TwitterBindingAttribute attribute)
        {
            return new TwitterBinder(attribute, _twitterService, _nameResolver);
        }
    }
}