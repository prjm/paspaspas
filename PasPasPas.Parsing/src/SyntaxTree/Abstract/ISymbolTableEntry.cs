#nullable disable
namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     symbol table entry
    /// </summary>
    public interface ISymbolTableEntry {

        /// <summary>
        ///     symbol name
        /// </summary>
        string SymbolName { get; }

    }
}