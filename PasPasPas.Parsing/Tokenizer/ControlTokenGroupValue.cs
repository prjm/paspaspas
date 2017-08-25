using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {
    /// <summary>
    ///     token group value for control characters
    /// </summary>
    public class ControlTokenGroupValue : CharacterClassTokenGroupValue {


        /// <summary>
        ///     token group value for control characters
        /// </summary>
        public ControlTokenGroupValue() : base(TokenKind.ControlChar, 0) {
            //..
        }

        /// <summary>
        ///     test if the charcater is a control char
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override bool MatchesClass(char input)
            => (!char.IsWhiteSpace(input)) && (char.IsControl(input));

    }

}
