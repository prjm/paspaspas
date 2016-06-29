using PasPasPas.Api;
using PasPasPas.Infrastructure.Input;
using System.Collections.Generic;
using System.Text;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     manually group token group values andvtheircharacter cxlasses
    /// </summary>
    public class PuntcuatorAndClass {

        /// <summary>
        ///     combination of punctuator and character class
        /// </summary>
        /// <param name="chrClass">character class</param>
        /// <param name="value">group value (tokenizer)</param>
        public PuntcuatorAndClass(CharacterClass chrClass, InputPattern value) {
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
    ///     storage for punctuator groups
    /// </summary>
    public class Punctuators {

        /// <summary>
        ///     punctuators
        /// </summary>
        private readonly IDictionary<char, InputPattern> punctuators =
            new Dictionary<char, InputPattern>();

        /// <summary>
        ///     list of class punctuators
        /// </summary>
        private readonly IList<PuntcuatorAndClass> classPunctuators =
            new List<PuntcuatorAndClass>();

        /// <summary>
        ///     add a punctuator
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="tokenValue"></param>
        /// <returns></returns>
        protected InputPattern AddPunctuator(char prefix, int tokenValue) {
            var result = new InputPattern(prefix, tokenValue);
            punctuators.Add(prefix, result);
            return result;
        }

        /// <summary>
        ///     add a punctuator
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="tokenValue"></param>
        /// <returns></returns>
        protected InputPattern AddPunctuator(CharacterClass prefix, PatternContinuation tokenValue) {
            var result = new InputPattern(prefix, tokenValue);
            var prefixedCharecterClass = prefix as PrefixedCharacterClass;

            if (prefixedCharecterClass == null)
                classPunctuators.Add(new PuntcuatorAndClass(prefix, result));
            else
                punctuators.Add(prefixedCharecterClass.Prefix, result);

            return result;
        }

        /// <summary>
        ///     add a punctuator
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="tokenValue"></param>
        /// <returns></returns>
        protected InputPattern AddPunctuator(char prefix, PatternContinuation tokenValue) {
            var result = new InputPattern(new SingleCharClass(prefix), tokenValue);
            punctuators.Add(prefix, result);
            return result;
        }


        /// <summary>
        ///     test if a punctuator group matches
        /// </summary>
        /// <param name="valueToMatch">char to match</param>
        /// <param name="tokenGroup">token group</param>
        /// <returns></returns>
        public bool Match(char valueToMatch, out InputPattern tokenGroup) {
            if (punctuators.TryGetValue(valueToMatch, out tokenGroup))
                return true;

            foreach (var punctuator in classPunctuators) {
                if (punctuator.CharClass.Matches(valueToMatch)) {
                    tokenGroup = punctuator.GroupValue;
                    return true;
                }
            }

            return false;
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
                inputStream.PutbackChar(file.FilePath, input[inputIndex]); ;
            }
            input.Length = tokenLength;

            return tokenKind.Tokenize(inputStream, input);
        }
    }
}
