using System.Threading.Tasks;

namespace Twitter.Services
{
    public interface ITwitterService
    {
        TwitterSettings Settings { get; set; }
        Task<bool> SendTweetAsync(string tweetMessage);
    }
}