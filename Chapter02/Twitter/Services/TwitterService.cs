using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;

namespace Twitter.Services
{
    public class TwitterService : ITwitterService
    {
        private TaskFactory taskFactory = new TaskFactory();
        
        public TwitterSettings Settings { get; set; }

        public Task<bool> SendTweetAsync(string tweetMessage)
        {
            var task = taskFactory.StartNew<bool>(() => SendTweetInternal(tweetMessage));
            return task;
        }

        private bool SendTweetInternal(string tweetMessage)
        {
            try
            {
                var creds = new TwitterCredentials(this.Settings?.ConsumerKey, this.Settings?.ConsumerSecret, 
                    this.Settings?.AccessToken, this.Settings?.AccessTokenSecret);

                var tweet = Auth.ExecuteOperationWithCredentials(creds, () =>
                        {
                            return Tweet.PublishTweet(tweetMessage);
                        });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
