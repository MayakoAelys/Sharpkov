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
        private readonly string _patternUsername = @"(\.?@[a-zA-Z0-9_ ]{1,15})"; // ref.: https://support.twitter.com/articles/20065832#error
        private ITwitterCredentials _auth;

        public Twitter(Config config)
        {
            _auth = Auth.SetUserCredentials(config.ConsumerKey, config.ConsumerSecret, config.AccessToken, config.AccessSecret);
            
        }

        public int GetRemainingRequests()
        {
            var rateLimit = RateLimit.GetCurrentCredentialsRateLimits();
            return rateLimit.StatusesHomeTimelineLimit.Limit;
        }

        public List<string> GetStatuses()
        {
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

                var statuses = Timeline.GetHomeTimeline(homeParams);

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
            while (tweet.StartsWith('@') || tweet.StartsWith(".@"))
            {
                tweet = string.Join(" ", tweet.Split().Skip(1));
            }
            return tweet.Trim();
        }

        public void PostTweet(string tweet)
        {
            Tweet.PublishTweet(tweet);
        }

        public void HelloWorld()
        {
            Tweet.PublishTweet("Hello world ♥");
        }
    }
}
