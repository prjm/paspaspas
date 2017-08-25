using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     token group for whitespace
    /// </summary>
    public class WhiteSpaceTokenGroupValue : CharacterClassTokenGroupValue {

        public WhiteSpaceTokenGroupValue() : base(TokenKind.WhiteSpace, 0) {
            //..
        }

        /// <summary>
        ///     test if the character is whitespace
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override bool MatchesClass(char input)
            => char.IsWhiteSpace(input);
    }

}