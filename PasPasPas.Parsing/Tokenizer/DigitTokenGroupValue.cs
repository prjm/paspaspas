using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {
    /// <summary>
    ///     token group value for digits
    /// </summary>
    public class DigitTokenGroupValue : CharacterClassTokenGroupValue {

        /// <summary>
        ///     create a new digital token group value
        /// </summary>
        public DigitTokenGroupValue() : base(TokenKind.Integer, 0) {
            //..
        }

        /// <summary>
        ///     matches a digit
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override bool MatchesClass(char input)
            => ('0' <= input) && (input <= '9');


    }

}

