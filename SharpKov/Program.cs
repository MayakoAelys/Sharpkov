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
    /// build: dotnet publish -c Release (-r debian-x64)
    /// </summary>
    class Program
    {
        private static Logging _log;
        private static Config _config;

        static void Main(string[] args)
        {
            _config = new Config();
            _log = new Logging(_config.GetBoolValue(ConfigKeys.Preferences_Logging));

            if (!_config.IsValid())
            {
                _log.Write("Invalid configuration file. Please compare the structure with the ./Config/appSettings.default.json file.");
                return;
            }

            if (_config.GetBoolValue(ConfigKeys.Preferences_TestMode)) TestMode();
            else TwitterMode();

            _log.Write("Exiting...");
        }

        private static void TestMode()
        {
            throw new NotImplementedException();
        }

        private static void TwitterMode()
        {
            var twitter = new Twitter(_config, _log);

            // TODO split this into Twitter.Functions()
            if (twitter.GetRemainingRequests() > 0)
            {
                var markov = new Markov(_log);
                var timeline = twitter.GetStatuses();

                if (timeline.Count == 0)
                {
                    _log.Write("Couldn't fetch any tweet, exiting...");
                    return;
                }

                foreach (var tweet in timeline)
                {
                    markov.AddSentence(tweet);
                }

                // either post a tweet or write X tweets to a file
                var writeFile = _config.GetBoolValue(ConfigKeys.Preferences_Local);
                var forceLastWord = _config.GetBoolValue(ConfigKeys.Preferences_ForceLastWord);

                if (writeFile)
                {
                    var generatedTweets = new List<string>();

                    for (var i = 0; i < 1000; i++)
                    {
                        generatedTweets.Add(markov.GenerateSentence(220, forceLastWord));
                    }

                    var filePath = Directory.GetCurrentDirectory() + @"\tweets.txt";
                    File.AppendAllLines(filePath, generatedTweets);

                    _log.Write($"Generated tweets written at {filePath}.");
                }
                else
                {
                    twitter.PostTweet(markov.GenerateSentence(250, forceLastWord));
                }
            }
        }
    }
}
