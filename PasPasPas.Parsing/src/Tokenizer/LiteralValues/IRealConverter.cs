using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Parsing.Tokenizer.LiteralValues {

    /// <summary>
    ///     interface to convert real values
    /// </summary>
    public interface IRealConverter : IEnvironmentItem {

        /// <summary>
        ///     convert integer literals to one real literal
        /// </summary>
        /// <param name="value">real value</param>
        /// <returns>real literal value</returns>
        ITypeReference Convert(string value);

    }
}
