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
            = string.Empty;

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
            = default!;

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
            = default!;

        public bool Equals(ITypeSymbol? other)
            => other is IVariable v &&
                TypeDefinition.Equals(v.TypeDefinition) &&
                KnownNames.SameIdentifier(Name, v.Name);
    }
}
