namespace PasPasPas.Parsing.Tokenizer.LiteralValues {

    /// <summary>
    ///     simple integer parser
    /// </summary>
    public interface ILiteralParser {
        object Parse(string input);
    }
}
