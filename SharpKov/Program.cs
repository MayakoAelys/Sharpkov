using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SharpKov.Utils;

namespace SharpKov
{
    /// <summary>
    /// C# fork of Pyrkov
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var config = new Config();

            if (!config.IsValid())
            {
                Console.WriteLine("Invalid configuration file. Please check the values");
                return;
            }
            
            //var foo = config.GetValue(ConfigKeys.Auth_ConsumerKey);
            var twitter = new Twitter(config);

            if (twitter.GetRemainingRequests() > 0)
            {
                var markov = new Markov();
                var timeline = twitter.GetStatuses();

                if (timeline.Count == 0)
                {
                    Console.WriteLine("Couldn't fetch any tweet, exiting...");
                    return;
                }

                foreach (var tweet in timeline)
                {
                    //markov.AddSentence(tweet);
                }
            }
        }
    }
}
