using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SharpKov.Utils;
using Microsoft.Extensions.Configuration;

namespace SharpKov.Utils
{
    public class Config
    {
        private static IConfigurationRoot _configuration;

        public string ConsumerKey { get; }
        public string ConsumerSecret { get; }
        public string AccessToken { get; }
        public string AccessSecret { get; }

        public Config()
        {
            // TODO handle the missing config file
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Config/appSettings.json");

            _configuration = builder.Build();

            if (this.IsValid())
            {
                ConsumerKey    = this.GetValue(ConfigKeys.Auth_ConsumerKey);
                ConsumerSecret = this.GetValue(ConfigKeys.Auth_ConsumerSecret);
                AccessToken    = this.GetValue(ConfigKeys.Auth_AccessToken);
                AccessSecret   = this.GetValue(ConfigKeys.Auth_AccessSecret);
            }
        }

        public string GetValue(string key)
        {
            return _configuration[key];
        }

        public bool GetBoolValue(string key)
        {
            bool.TryParse(_configuration[key], out var value);
            return value;
        }

        /// <summary>
        /// Check if we have the required values in the config file
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            bool dummy;

            return
                // Auth
                !string.IsNullOrWhiteSpace(_configuration[ConfigKeys.Auth_ConsumerKey])          &&
                !string.IsNullOrWhiteSpace(_configuration[ConfigKeys.Auth_ConsumerSecret])       &&
                !string.IsNullOrWhiteSpace(_configuration[ConfigKeys.Auth_AccessToken])          &&
                !string.IsNullOrWhiteSpace(_configuration[ConfigKeys.Auth_AccessSecret])         &&
                
                // Pref
                bool.TryParse(_configuration[ConfigKeys.Preferences_Local], out dummy)           &&
                bool.TryParse(_configuration[ConfigKeys.Preferences_ForceLastWord], out dummy)   &&
                bool.TryParse(_configuration[ConfigKeys.Preferences_Logging], out dummy)         &&
                bool.TryParse(_configuration[ConfigKeys.Preferences_TestMode], out dummy);
        }
    }
}
