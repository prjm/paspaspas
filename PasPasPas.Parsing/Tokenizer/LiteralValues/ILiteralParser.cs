using PasPasPas.Infrastructure.Common;

namespace PasPasPas.Parsing.Tokenizer.LiteralValues {

    /// <summary>
    ///     simple integer parser
    /// </summary>
    public interface IIntegerLiteralParser {

        /// <summary>
        ///     parse a given literal
        /// </summary>
        /// <param name="input">input</param>
        /// <returns>parsed literal value</returns>
        IValue Parse(string input);

    }
}
