using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace SharpKov.Utils
{
    public class Twitter
    {
        //private readonly string _patternUsername = @"(\.?@[a-zA-Z0-9_ ]{1,15})"; // ref.: https://support.twitter.com/articles/20065832#error
        //private ITwitterCredentials _auth;
        private readonly Logging _log;
        private readonly IAuthenticatedUser _user;

        public Twitter(Config config, Logging log)
        {
            _user = User.GetAuthenticatedUser(Auth.CreateCredentials(config.ConsumerKey, config.ConsumerSecret, config.AccessToken, config.AccessSecret));
            _log = log;
        }

        public int GetRemainingRequests()
        {
            _log.WriteIn();
            int rateLimitCount = 1;

            // WORKAROUND FOR LINUX (NullReferenceException on RateLimit.GetCredentialsRateLimits, WHY THE FUCK IS IT HAPPENING)
            try
            {
                var rateLimit = RateLimit.GetCredentialsRateLimits(_user.Credentials);
                rateLimitCount = rateLimit.StatusesHomeTimelineLimit.Remaining;
                _log.Write($"Remaining requests: {rateLimitCount}");
            }
            catch (Exception e)
            {
                _log.Write(e);
            }

            return rateLimitCount;
        }

        public List<string> GetStatuses()
        {
            _log.WriteIn();

            var result = new List<string>();
            long? sinceId = -1;

            var homeParams = new HomeTimelineParameters()
            {
                MaximumNumberOfTweetsToRetrieve = 200
            };
            homeParams.AddCustomQueryParameter("tweet_mode", "extended"); // Avoid truncated tweets

            while (this.GetRemainingRequests() > 0)
            {
                if (sinceId != -1) homeParams.SinceId = (long) sinceId;

                _log.Write($"GetHomeTimeLine with sinceId: {sinceId}");

                var statuses = _user.GetHomeTimeline(homeParams);

                _log.Write($"Did we get something? {statuses != null}");
                if (statuses == null) break; // couldn't fetch more

                foreach (var tweet in statuses)
                {
                    var text = tweet.RetweetedTweet?.FullText ?? tweet.FullText;

                    text = this.CleanTweet(text);
                    if(this.IsNiceTweet(text)) { result.Add(text); }

                    sinceId = tweet.Id;
                }
            }

            return result;
        }

        public bool IsNiceTweet(string tweet)
        {
            if (tweet.StartsWith("http://") || tweet.StartsWith("https://") || tweet.StartsWith("www") || string.IsNullOrWhiteSpace(tweet))
                return false;

            return true;
        }

        public string CleanTweet(string tweet)
        {
            //_log.Write("[Twitter] CleanTweet", this);
            while (tweet.StartsWith('@') || tweet.StartsWith(".@"))
            {
                tweet = string.Join(" ", tweet.Split().Skip(1));
            }
            return tweet.Trim();
        }

        public void PostTweet(string tweet)
        {
            _log.WriteIn();
            _log.Write($"Trying to post a tweet...");
            _user.PublishTweet(tweet);
        }

        public void HelloWorld()
        {
            _log.WriteIn();
            _user.PublishTweet("Hello world ♥");
        }
    }
}
