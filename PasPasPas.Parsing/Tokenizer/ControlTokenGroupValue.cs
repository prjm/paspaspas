using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {
    /// <summary>
    ///     token group value for control characters
    /// </summary>
    public class ControlTokenGroupValue : CharacterClassTokenGroupValue {


        /// <summary>
        ///     token group value for control characters
        /// </summary>
        public ControlTokenGroupValue() : base(TokenKind.ControlChar, new ControlCharacterClass(), 0) {
            //..
        }


    }

}
