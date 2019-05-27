using Extensions.Bindings;
using System;
using System.Collections.Generic;
using System.Text;
using Twitter;
using Twitter.Services;

namespace Twitter.Services
{
    public static class TwitterServiceExtensions
    {
        public static void SetSettings(this ITwitterService service, TwitterBindingAttribute attribute)
        {
            if (service == null)
                throw new NullReferenceException(nameof(service));
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            var twitterSettings = new TwitterSettings();

            twitterSettings.ConsumerKey = attribute.ConsumerKey;
            twitterSettings.ConsumerSecret = attribute.ConsumerSecret;
            twitterSettings.AccessToken = attribute.AccessToken;
            twitterSettings.AccessTokenSecret = attribute.AccessTokenSecret;

            service.Settings = twitterSettings;
        }
    }
}
