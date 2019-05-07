using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.Parsing {

    /// <summary>
    ///     simple integer parser
    /// </summary>
    public interface IILiteralParser : IEnvironmentItem {

        /// <summary>
        ///     parse a given literal to a constant value
        /// </summary>
        /// <param name="input">input</param>
        /// <returns>parsed literal value as constant value</returns>
        ITypeReference Parse(string input);

    }
}
