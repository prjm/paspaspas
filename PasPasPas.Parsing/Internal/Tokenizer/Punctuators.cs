using PasPasPas.Api;
using PasPasPas.Infrastructure.Input;
using System.Collections.Generic;
using System.Text;

namespace PasPasPas.Internal.Tokenizer {

    /// <summary>
    ///     manually group token group values andvtheircharacter cxlasses
    /// </summary>
    public class PuntcuatorAndClass {

        /// <summary>
        ///     combination of punctuator and character class
        /// </summary>
        /// <param name="chrClass">character class</param>
        /// <param name="value">group value (tokenizer)</param>
        public PuntcuatorAndClass(CharacterClass chrClass, PunctuatorGroup value) {
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
        public PunctuatorGroup GroupValue { get; }
    }

    /// <summary>
    ///     storage for punctuator groups
    /// </summary>
    public class Punctuators {

        /// <summary>
        ///     punctuators
        /// </summary>
        private readonly IDictionary<char, PunctuatorGroup> punctuators =
            new Dictionary<char, PunctuatorGroup>();

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
        protected PunctuatorGroup AddPunctuator(char prefix, int tokenValue) {
            var result = new PunctuatorGroup(prefix, tokenValue);
            punctuators.Add(prefix, result);
            return result;
        }

        /// <summary>
        ///     add a punctuator
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="tokenValue"></param>
        /// <returns></returns>
        protected PunctuatorGroup AddPunctuator(CharacterClass prefix, TokenGroupValue tokenValue) {
            var result = new PunctuatorGroup(prefix, tokenValue);

            if (prefix.Prefix == '\0')
                classPunctuators.Add(new PuntcuatorAndClass(prefix, result));
            else
                punctuators.Add(prefix.Prefix, result);

            return result;
        }

        /// <summary>
        ///     add a punctuator
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="tokenValue"></param>
        /// <returns></returns>
        protected PunctuatorGroup AddPunctuator(char prefix, TokenGroupValue tokenValue) {
            var result = new PunctuatorGroup(new SingleCharClass(prefix), tokenValue);
            punctuators.Add(prefix, result);
            return result;
        }


        /// <summary>
        ///     test if a punctuator group matches
        /// </summary>
        /// <param name="valueToMatch">char to match</param>
        /// <param name="tokenGroup">token group</param>
        /// <returns></returns>
        public bool Match(char valueToMatch, out PunctuatorGroup tokenGroup) {
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
        public static PascalToken FetchTokenByGroup(StackedFileReader inputStream, char prefix, PunctuatorGroup tokenGroup) {
            StringBuilder input = new StringBuilder(100);
            input.Append(prefix);
            while (input.Length < tokenGroup.Length && (!inputStream.AtEof)) {
                input.Append(inputStream.FetchChar());
            }

            int tokenLength;
            var tokenKind = tokenGroup.Match(input, out tokenLength);

            for (int inputIndex = input.Length - 1; inputIndex >= tokenLength; inputIndex--) {
                inputStream.PutbackChar(input[inputIndex]); ;
            }
            input.Length = tokenLength;

            return tokenKind.WithPrefix(inputStream, input);
        }
    }
}
