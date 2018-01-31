using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SharpKov.Utils
{
    public class Markov
    {
        private Logging _log;
        private Dictionary<string, List<string>> _wordChains = new Dictionary<string, List<string>>();
        private WordsList _firstWords = new WordsList();
        private WordsList _lastWords = new WordsList();

        public Markov(Logging log, string startString = "")
        {
            this._log = log;
            _log.WriteIn();

            if (!string.IsNullOrWhiteSpace(startString))
            {
                this.AddSentence(startString);
            }

        }

        #region Database

        public void AddSentence(string text)
        {
            // TODO add newlines handling, Split is eating them atm
            if (string.IsNullOrWhiteSpace(text)) return;

            var splitted = text.Split();
            var firstWord = splitted[0];
            var lastWord = splitted[splitted.Length - 1];

            // Add the reference of the first word
            if (!_firstWords.Contains(firstWord)) _firstWords.Add(firstWord);

            // Add the reference of the last word
            if (!_lastWords.Contains(lastWord)) _lastWords.Add(lastWord);

            // Add words to the dictionnary
            var currentWord = "";
            var nextWord = "";

            for (var i = 0; i < splitted.Length; i++)
            {
                currentWord = splitted[i];

                if (i + 1 >= splitted.Length)
                {
                    nextWord = "";
                }
                else
                {
                    nextWord = splitted[i + 1];
                }

                this.AddChainedWords(currentWord, nextWord);
            }

            // Last word of the string is followed by an EOL
            this.AddChainedWords(currentWord, "");
        }

        private void AddChainedWords(string key, string value)
        {
            key = StringHelper.NormalizeString(key);
            value = value.Trim();

            if (this._wordChains.ContainsKey(key))
            {
                _wordChains[key].Add(value);
            }
            else
            {
                this._wordChains.Add(key, new List<string>() { value });
            }
        }

        private string GetRandomValues(string key)
        {
            var random = new Random();

            if (!this._wordChains.ContainsKey(key)) return "";

            var keyValue = this._wordChains[key];
            return keyValue[random.Next(0, keyValue.Count)];
        }

        #endregion


        #region Generation

        public string GenerateSentence(int maxChar = 500, bool forceLastWord = false)
        {
            _log.WriteIn();
            _log.Write($"Sentence parameters: maxChar = {maxChar}, forceLastWord = {forceLastWord}");
            var result = "";

            while (true)
            {
                // Get the first word randomly
                var randomKey = this._firstWords.GetRandomValue();
                var currentWord = randomKey;
                var nextWord = "";
                result += currentWord + " ";

                // Generate the rest of the sentence
                while (true)
                {
                    //// Are we exceeding the characters limit?
                    //if (result.Length > maxChar) break;

                    nextWord = this.GetRandomValues(currentWord);

                    // Do we met the ending conditions? (either no more word or exceeding the char limit
                    if (nextWord == "" || (result.Length + nextWord.Length) > maxChar)
                    {
                        if (!forceLastWord || this._lastWords.Contains(currentWord))
                        {
                            //result += nextWord;
                            return result;
                        }

                        // We didn't match the criterias => retrying
                        result = "";
                        break;
                    }

                    result += nextWord + " ";
                    currentWord = nextWord;
                }
            }
        }

        #endregion


        #region Private

        private class WordsList
        {
            private List<string> _wordsList = new List<string>();

            public bool Contains(string word)
            {
                word = StringHelper.NormalizeString(word);

                foreach (var _word in _wordsList)
                {
                    if(_word.Equals(word)) return true;
                }

                return false;
            }

            public void Add(string word)
            {
                this._wordsList.Add(StringHelper.NormalizeString(word));
            }

            public string GetRandomValue()
            {
                var number = new Random().Next(0, this._wordsList.Count - 1);

                return this._wordsList[number];
            }
        }

        #endregion
    }
}
