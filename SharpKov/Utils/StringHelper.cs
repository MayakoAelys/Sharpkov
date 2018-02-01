using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Parameters;

namespace SharpKov.Utils
{
    public static class StringHelper
    {
        public static string NormalizeString(string str)
        {
            return str.Trim().ToLowerInvariant();
        }
    }
}
