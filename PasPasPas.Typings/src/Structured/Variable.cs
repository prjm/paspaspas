using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     routine parameters
    /// </summary>
    internal class Variable : IVariable {

        /// <summary>
        ///     variable name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     member visibility
        /// </summary>
        public MemberVisibility Visibility { get; set; }
            = MemberVisibility.Public;

        /// <summary>
        ///     variable kind
        /// </summary>
        public VariableKind Kind { get; set; }

        /// <summary>
        ///     type definition
        /// </summary>
        public ITypeDefinition TypeDefinition { get; set; }

        /// <summary>
        ///     symbol type
        /// </summary>
        public SymbolTypeKind SymbolKind
            => SymbolTypeKind.Variable;

        /// <summary>
        ///     flags
        /// </summary>
        public VariableFlags Flags { get; set; }

        /// <summary>
        ///     initial value of the variable
        /// </summary>
        public IValue InitialValue { get; set; }
    }
}
