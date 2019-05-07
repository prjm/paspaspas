using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.Parsing {

    /// <summary>
    ///     interface for parser that convert literal values
    ///     to constant values
    /// </summary>
    public interface IILiteralParser : IEnvironmentItem {

        /// <summary>
        ///     parse a given literal to a constant value
        /// </summary>
        /// <param name="input">input value</param>
        /// <returns>parsed literal value as constant value</returns>
        ITypeReference Parse(string input);

    }
}
