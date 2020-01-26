using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     routine parameters
    /// </summary>
    public class Variable : IVariable {

        /// <summary>
        ///     variable type
        /// </summary>
        public ITypeSymbol SymbolType { get; set; }

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
        ///     class item
        /// </summary>
        public bool ClassItem { get; set; }

        /// <summary>
        ///     variable kind
        /// </summary>
        public VariableKind Kind { get; set; }

    }
}
