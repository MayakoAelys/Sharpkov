using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace SharpKov.Utils
{

    public class Logging
    {
        private bool _doLog { get; }

        public Logging(bool doLog)
        {
            _doLog = doLog;
        }

        #region Public methods

        /// <summary>
        /// Log "Entering method"
        /// </summary>
        public void WriteIn(
            [CallerFilePath] string path = @"C:\Unknown.cs",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string name = "Unknown method")
        {
            if (!_doLog) return;
            Console.WriteLine("");
            Console.WriteLine($"{FormatPrefix(path, line, name)} - Entering method");
        }

        public void Write(
            string message,
            [CallerFilePath] string path = @"C:\Unknown.cs", 
            [CallerLineNumber] int line = 0, 
            [CallerMemberName] string name = "Unknown method")
        {
            if (!_doLog) return;
            Console.WriteLine($"{FormatPrefix(path, line, name)} {message}");
        }

        public void Write(
            Exception ex,
            [CallerFilePath] string path = @"C:\Unknown.cs",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string name = "Unknown method")
        {
            if (!_doLog) return;
            Console.WriteLine($"{FormatPrefix(path, line, name)} {ex.Message}");
        }

        public void Test([CallerFilePath] string path = @"C:\Unknown.cs", [CallerLineNumber] int line = 0, [CallerMemberName] string name = "Unknown method")
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.ffff}] {GetFileName(path)}:{line} ({name}) > Test message");
        }

        #endregion


        #region Private methods

        /// <summary>
        /// return $"{GetFileName(path)}:{line} ({name}) >";
        /// </summary>
        private string FormatPrefix(string path, int line, string name)
        {
            return $"[{DateTime.Now:HH:mm:ss.ffff}] {GetFileName(path)}:{line} ({name}) >";
        }


        private string GetFileName(string path)
        {
            var splittedPath = path.Split('\\');
            return splittedPath[splittedPath.Length - 1];
        }

        #endregion
    }
}
