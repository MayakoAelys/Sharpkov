using System;
using System.Collections;
using System.Collections.Generic;
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
            IEnumerable<ITweet> statuses;
            List<string> result = new List<string>();
            long? sinceId = -1;
            string text;
            var passes = 0;

            var homeParams = new HomeTimelineParameters()
            {
                MaximumNumberOfTweetsToRetrieve = 200
            };
            homeParams.AddCustomQueryParameter("tweet_mode", "extended");

            while (this.GetRemainingRequests() > 0)
            {
                if (sinceId != -1) homeParams.SinceId = (long) sinceId;

                statuses = Timeline.GetHomeTimeline(homeParams);

                if (statuses == null) break; // couldn't fetch more

                foreach (var tweet in statuses)
                {
                    text = tweet.RetweetedTweet?.FullText ?? tweet.FullText;

                    text = this.CleanTweet(text);
                    if(this.IsNiceTweet(text)) { result.Add(text); }

                    sinceId = tweet.Id;
                    // TODO check how to set SinceID to null when we can't fetch more tweets
                }

                passes++;

                //return result;
            }

            return result;

        }

        public bool IsNiceTweet(string text)
        {
            // TODO
            return true;
        }

        public string CleanTweet(string text)
        {
            // TODO
            return text;
        }

        public void PostTweet()
        {
            throw new NotImplementedException();
        }

        public void HelloWorld()
        {
            Tweet.PublishTweet("Hello world ♥");
        }
    }
}
