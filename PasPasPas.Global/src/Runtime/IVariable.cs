using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     interface for variables
    /// </summary>
    public interface IVariable : IRefSymbol {

        /// <summary>
        ///     symbol type
        /// </summary>
        ITypeReference SymbolType { get; }

        /// <summary>
        ///     visibility
        /// </summary>
        MemberVisibility Visibility { get; }

        /// <summary>
        ///     variable name
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     variable kind
        /// </summary>
        VariableKind Kind { get; }

        /// <summary>
        ///     class fields
        /// </summary>
        bool ClassItem { get; }
    }
}