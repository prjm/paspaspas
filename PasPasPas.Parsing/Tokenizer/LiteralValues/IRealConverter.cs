namespace PasPasPas.Parsing.Tokenizer.LiteralValues {

    /// <summary>
    ///     interface to convert real values
    /// </summary>
    public interface IRealConverter {
        object Convert(object digits, object decimals, bool minus, object exponent);
    }
}
