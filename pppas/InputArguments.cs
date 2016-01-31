using System;
using System.Collections.Generic;

namespace Pppas {

    /// <summary>
    ///     helper class to parse input arguments
    /// </summary>
    public class InputArguments {

        /// <summary>
        ///       prefix for arguments
        /// </summary>
        public const string DefaultKeyLeadingPattern = "-";

        /// <summary>
        ///     parsed arguments
        /// </summary>
        private Dictionary<string, string> parsedArguments =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     key leading pattern
        /// </summary>
        private readonly string keyPattern;

        /// <summary>
        ///     get the value of an argument
        /// </summary>
        /// <param name="key">argument name</param>
        /// <returns>argument value</returns>
        public string this[string key]
        {
            get
            {
                return GetValue(key);
            }
            set
            {
                if (key != null)
                    parsedArguments[key] = value;
            }
        }

        /// <summary>
        ///     argumnet name leading pattern
        /// </summary>
        public string KeyLeadingPattern
            => keyPattern;


        /// <summary>
        ///     create a new input arguments parser
        /// </summary>
        /// <param name="args">command line arguments</param>
        /// <param name="keyLeadingPattern">key leading pattern</param>
        public InputArguments(string[] args, string keyLeadingPattern) {
            keyPattern = !string.IsNullOrWhiteSpace(keyLeadingPattern) ? keyLeadingPattern : DefaultKeyLeadingPattern;

            if (args != null && args.Length > 0)
                Parse(args);
        }

        /// <summary>
        ///     create a new input arguments parser
        /// </summary>
        /// <param name="args"></param>
        public InputArguments(string[] args) : this(args, null) {
        }

        /// <summary>
        ///     tests if a given key is present
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key) {
            string adjustedKey;
            return ContainsKey(key, out adjustedKey);
        }

        /// <summary>
        ///     gets the key witout the key identieifer
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetPeeledKey(string key)
            => IsKey(key) ? key.Substring(keyPattern.Length) : key;

        /// <summary>
        ///     gets the key with the key identifier
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetDecoratedKey(string key)
            => !IsKey(key) ? (keyPattern + key) : key;

        /// <summary>
        ///     tests if a given string is a key
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool IsKey(string str)
            => (!string.IsNullOrWhiteSpace(str)) && str.StartsWith(keyPattern, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        ///     parse command line arguments
        /// </summary>
        /// <param name="args"></param>
        protected void Parse(string[] args) {
            for (int i = 0; i < args.Length; i++) {
                if (string.IsNullOrWhiteSpace(args[i])) continue;

                string key = null;
                string val = null;

                if (IsKey(args[i])) {
                    key = args[i];

                    if (i + 1 < args.Length && !IsKey(args[i + 1])) {
                        val = args[i + 1];
                        i++;
                    }
                }
                else
                    val = args[i];

                // adjustment
                if (key == null) {
                    key = val;
                    val = null;
                }
                parsedArguments[key] = val;
            }
        }

        /// <summary>
        ///     gets the value of an argument, if pressent
        /// </summary>
        /// <param name="key">key name</param>
        /// <returns>key value or <c>null</c> if the key value was not present</returns>
        protected string GetValue(string key) {
            string adjustedKey;
            if (ContainsKey(key, out adjustedKey))
                return parsedArguments[adjustedKey];

            return null;
        }

        /// <summary>
        ///     check if a key exists 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="adjustedKey"></param>
        /// <returns></returns>
        protected virtual bool ContainsKey(string key, out string adjustedKey) {
            adjustedKey = key;

            if (parsedArguments.ContainsKey(key))
                return true;

            if (IsKey(key)) {
                string peeledKey = GetPeeledKey(key);
                if (parsedArguments.ContainsKey(peeledKey)) {
                    adjustedKey = peeledKey;
                    return true;
                }
                return false;
            }

            string decoratedKey = GetDecoratedKey(key);
            if (parsedArguments.ContainsKey(decoratedKey)) {
                adjustedKey = decoratedKey;
                return true;
            }
            return false;
        }
    }
}
