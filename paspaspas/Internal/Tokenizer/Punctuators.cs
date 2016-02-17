using PasPasPas.Api;
using PasPasPas.Api.Input;
using System.Collections.Generic;
using System.Text;

namespace PasPasPas.Internal.Tokenizer {

    /// <summary>
    ///     manually group token group values andtheircharacter cxlasses
    /// </summary>
    public class PuntcuatorAndClass {
        public PuntcuatorAndClass(CharacterClass chrClass, TokenGroupValue value) {
            ChrClass = chrClass;
            GroupValue = value;
        }

        public CharacterClass ChrClass { get; private set; }
        public TokenGroupValue GroupValue { get; private set; }
    }

    /// <summary>
    ///     storage for punctuator groups
    /// </summary>
    public class Punctuators {

        /// <summary>
        ///     punctuators
        /// </summary>
        private IDictionary<char, PunctuatorGroup> punctuators =
            new Dictionary<char, PunctuatorGroup>();

        /// <summary>
        ///     list of class punctuators
        /// </summary>
        private IList<PunctuatorGroup> classPunctuators =
            new List<PunctuatorGroup>();

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
                classPunctuators.Add(result);
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
        /// <param name="charToMatch">char to match</param>
        /// <param name="tokenGroup">token group</param>
        /// <returns></returns>
        public bool Match(char charToMatch, out PunctuatorGroup tokenGroup) {
            if (punctuators.TryGetValue(charToMatch, out tokenGroup))
                return true;

            foreach (var punctuator in classPunctuators) {
                if (punctuator.)
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
        public PascalToken FetchTokenByGroup(IParserInput inputStream, char prefix, PunctuatorGroup tokenGroup) {
            StringBuilder input = new StringBuilder(100);
            input.Append(prefix);
            while (input.Length < tokenGroup.Length && (!inputStream.AtEof)) {
                input.Append(inputStream.NextChar());
            }

            int tokenLength;
            var tokenKind = tokenGroup.Match(input, out tokenLength);

            for (int inputIndex = input.Length - 1; inputIndex >= tokenLength; inputIndex--) {
                inputStream.Putback(input[inputIndex]); ;
            }
            input.Length = tokenLength;

            return new PascalToken() { Kind = tokenKind.Token, Value = tokenKind.WithPrefix(inputStream, input) };
        }
    }
}
