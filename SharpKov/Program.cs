using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using SharpKov.Utils;

namespace SharpKov
{
    /// <summary>
    /// C# fork of Pyrkov
    /// </summary>
    class Program
    {
        // TODO Logging
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
                    markov.AddSentence(tweet);
                }

                // either post a tweet or write X tweets to a file
                var writeFile = config.GetBoolValue(ConfigKeys.Preferences_Local);
                var forceLastWord = config.GetBoolValue(ConfigKeys.Preferences_ForceLastWord);

                if (writeFile)
                {
                    var generatedTweets = new List<string>();

                    for (var i = 0; i < 1000; i++)
                    {
                        generatedTweets.Add(markov.GenerateSentence(220, forceLastWord));
                    }

                    var filePath = Directory.GetCurrentDirectory() + @"\tweets.txt";
                    File.AppendAllLines(filePath, generatedTweets);

                    Console.WriteLine($"Generated tweets written at {filePath}.");
                }
                else
                {
                    twitter.PostTweet(markov.GenerateSentence(250, forceLastWord));
                }
            }
        }
    }
}
