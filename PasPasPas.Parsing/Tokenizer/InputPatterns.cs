using PasPasPas.Api;
using PasPasPas.Infrastructure.Input;
using System.Collections.Generic;
using System.Text;
using System;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     manually group token group values and their character classes
    /// </summary>
    public class InputPatternAndClass {

        /// <summary>
        ///     combination of input pattern and character class
        /// </summary>
        /// <param name="chrClass">character class</param>
        /// <param name="value">group value (tokenizer)</param>
        public InputPatternAndClass(CharacterClass chrClass, InputPattern value) {

            if (chrClass == null)
                throw new ArgumentNullException(nameof(chrClass));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            CharClass = chrClass;
            GroupValue = value;
        }

        /// <summary>
        ///     character class
        /// </summary>
        public CharacterClass CharClass { get; }

        /// <summary>
        ///     tokenizer group        
        /// </summary>
        public InputPattern GroupValue { get; }
    }

    /// <summary>
    ///     storage for input patterns, used by the tokenizer
    /// </summary>
    public class InputPatterns {

        /// <summary>
        ///     simple input patterns
        /// </summary>
        private readonly IDictionary<char, InputPattern> simplePatterns =
            new Dictionary<char, InputPattern>();

        /// <summary>
        ///     list of complex input patterns
        /// </summary>
        private readonly IList<InputPatternAndClass> complexPatterns =
            new List<InputPatternAndClass>();

        /// <summary>
        ///     add a simple pattern based upon one character
        /// </summary>
        /// <param name="prefix">prefix</param>
        /// <param name="tokenValue">resulting token value</param>
        /// <returns></returns>
        public InputPattern AddPattern(char prefix, int tokenValue) {
            var result = new InputPattern(prefix, tokenValue);
            simplePatterns.Add(prefix, result);
            return result;
        }

        /// <summary>
        ///     add a complex pattern
        /// </summary>
        /// <param name="prefix">character prefix</param>
        /// <param name="tokenValue">tokenizer</param>
        /// <returns></returns>
        public InputPattern AddPattern(CharacterClass prefix, PatternContinuation tokenValue) {

            if (prefix == null)
                throw new ArgumentNullException(nameof(prefix));

            if (tokenValue == null)
                throw new ArgumentNullException(nameof(tokenValue));

            var result = new InputPattern(prefix, tokenValue);
            var prefixedCharecterClass = prefix as PrefixedCharacterClass;

            if (prefixedCharecterClass == null)
                complexPatterns.Add(new InputPatternAndClass(prefix, result));
            else
                simplePatterns.Add(prefixedCharecterClass.Prefix, result);

            return result;
        }

        /// <summary>
        ///     add a simple pattern
        /// </summary>
        /// <param name="prefix">pattern prefix</param>
        /// <param name="tokenValue">pattern continuation</param>
        /// <returns>created pattern</returns>
        public InputPattern AddPattern(char prefix, PatternContinuation tokenValue) {
            var result = new InputPattern(new SingleCharClass(prefix), tokenValue);
            simplePatterns.Add(prefix, result);
            return result;
        }


        /// <summary>
        ///     test if a input pattern matches
        /// </summary>
        /// <param name="valueToMatch">char to match</param>
        /// <param name="tokenGroup">pattern</param>
        /// <returns></returns>
        public bool Match(char valueToMatch, out InputPattern tokenGroup) {
            if (simplePatterns.TryGetValue(valueToMatch, out tokenGroup))
                return true;

            foreach (var pattern in complexPatterns) {
                if (pattern.CharClass.Matches(valueToMatch)) {
                    tokenGroup = pattern.GroupValue;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     fetch the next token from the input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PascalToken FetchNextToken(StackedFileReader input) {

            if (input.AtEof)
                throw new InvalidOperationException();

            var file = input.CurrentInputFile;
            bool switchedInput = false;
            char c = input.FetchChar(out switchedInput);
            InputPattern tokenGroup;

            if (Match(c, out tokenGroup)) {
                return FetchTokenByGroup(input, c, tokenGroup);
            }

            return null; // GenerateUndefinedToken(c, file);
        }

        /// <summary>
        ///     fetch a token for this group
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="prefix"></param>
        /// <param name="tokenGroup"></param>
        /// <returns></returns>
        public static PascalToken FetchTokenByGroup(StackedFileReader inputStream, char prefix, InputPattern tokenGroup) {
            StringBuilder input = new StringBuilder(100);
            bool switchedInput = false;
            input.Append(prefix);
            while (input.Length < tokenGroup.Length && (!inputStream.AtEof) && (!switchedInput)) {
                input.Append(inputStream.FetchChar(out switchedInput));
            }

            int tokenLength;
            var file = inputStream.CurrentInputFile;
            var tokenKind = tokenGroup.Match(input, out tokenLength);

            for (int inputIndex = input.Length - 1; inputIndex >= tokenLength; inputIndex--) {
                inputStream.PutbackChar(file, input[inputIndex]); ;
            }
            input.Length = tokenLength;

            return tokenKind.Tokenize(inputStream, input);
        }
    }
}
