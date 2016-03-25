using PasPasPas.Api;
using PasPasPas.Parsing.Tokenizer;

namespace PasPasPas.Internal.Tokenizer {

    /// <summary>
    ///     provides tokenizer helpers delphi compiler directives
    /// </summary>
    public class PreprocessorPunctuators : Punctuators {

        /// <summary>
        ///     create the preprocessor punctuators
        /// </summary>
        public PreprocessorPunctuators() {
            AddPunctuator('+', PascalToken.Plus);
            AddPunctuator('-', PascalToken.Minus);
            AddPunctuator(new WhitspaceCharacterClass(), new WhiteSpaceTokenGroupValue());
            AddPunctuator(new IdentifierCharacterClass(), new IdentifierTokenGroupValue(CompilerDirectiveTokenizer.Keywords));
            AddPunctuator(new NumberCharacterClass(), new NumberTokenGroupValue());
            AddPunctuator('$', new HexNumberTokenValue());
            AddPunctuator(new ControlCharacterClass(), new ControlTokenGroupValue());
            AddPunctuator('\'', new QuotedStringTokenValue());
        }
    }
}