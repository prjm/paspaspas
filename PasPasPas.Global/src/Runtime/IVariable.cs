using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     interface for variables
    /// </summary>
    public interface IVariable {

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

        /// <summary>
        ///     variable type
        /// </summary>
        ITypeDefinition TypeDefinition { get; }

    }
}