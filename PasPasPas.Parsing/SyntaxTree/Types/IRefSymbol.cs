using PasPasPas.Global.Runtime;

namespace PasPasPas.Parsing.SyntaxTree.Types {

    /// <summary>
    ///     symbol by reference
    /// </summary>
    public interface IRefSymbol {

        /// <summary>
        ///     get the type of this symbol
        /// </summary>
        ITypeReference TypeInfo { get; }

    }
}
