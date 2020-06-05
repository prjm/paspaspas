#nullable disable
namespace PasPasPas.Parsing.Tokenizer.LiteralValues {

    /// <summary>
    ///     integer literal parser kind
    /// </summary>
    public enum LiteralParserKind {

        /// <summary>
        ///     undefined parser
        /// </summary>
        Undefined,

        /// <summary>
        ///     parse ints
        /// </summary>
        IntegerNumbers,

        /// <summary>
        ///     parse hex numbers
        /// </summary>
        HexNumbers

    }
}
