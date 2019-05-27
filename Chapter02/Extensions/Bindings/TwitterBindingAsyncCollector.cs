using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Twitter;
using Twitter.Services;

namespace Extensions.Bindings
{
    public class TwitterBindingAsyncCollector : IAsyncCollector<string>
    {
        private readonly TwitterBindingAttribute _attribute;
        private readonly INameResolver _nameResolver;
        private readonly ITwitterService _twitterService;

        private readonly List<string> _tweetsToSend = new List<string>();

        public TwitterBindingAsyncCollector(TwitterBindingAttribute attribute,
            ITwitterService twitterService, INameResolver nameResolver)
        {
            this._attribute = attribute;
            this._nameResolver = nameResolver;
            this._twitterService = twitterService;

            this._twitterService.SetSettings(attribute);
        }

        public Task AddAsync(string item, CancellationToken cancellationToken = default)
        {
            _tweetsToSend.Add(item);
            return Task.CompletedTask;
        }

        public async Task FlushAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in this._tweetsToSend)
            {
               await _twitterService.SendTweetAsync(item);
            }
            this._tweetsToSend.Clear();
        }
    }
}
