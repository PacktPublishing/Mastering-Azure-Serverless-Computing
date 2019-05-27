using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;
using Twitter;
using Twitter.Services;

namespace Extensions.Bindings
{
    public class TwitterBinder
    {
        private readonly TwitterBindingAttribute _attribute;
        private readonly ITwitterService _twitterService;
        private readonly INameResolver _nameResolver;

        public TwitterBinder(TwitterBindingAttribute attribute, 
            ITwitterService twitterService, INameResolver nameResolver)
        {
            this._attribute = attribute;
            this._twitterService = twitterService;
            this._nameResolver = nameResolver;

            this._twitterService.SetSettings(attribute);
        }
        
        public Task TweetAsync(string message)
        {
            return _twitterService.SendTweetAsync(message);
        }
    }
}