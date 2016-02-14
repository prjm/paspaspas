namespace PasPasPas.Internal.Parser {

    /// <summary>
    ///     preprocessor
    /// </summary>
    public class Preprocessor : ParserBase {

        /// <summary>
        ///     fetch the next token (and filter preprocessor tokens)
        /// </summary>
        protected override void FetchNextToken() {
            do {
                base.FetchNextToken();
            } while (IsPreprocessorToken());
        }

        protected override bool LookAhead(int num, params int[] tokenKind) {
            return base.LookAhead(num, tokenKind);
        }

        private bool IsPreprocessorToken() => false;

    }
}
