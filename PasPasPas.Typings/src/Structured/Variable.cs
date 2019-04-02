using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     routine parameters
    /// </summary>
    public class Variable : IRefSymbol {

        /// <summary>
        ///     parameter type
        /// </summary>
        public ITypeReference SymbolType { get; set; }

        /// <summary>
        ///     parameter name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId
            => SymbolType != null ? SymbolType.TypeId : KnownTypeIds.ErrorType;

        /// <summary>
        ///     member visibility
        /// </summary>
        public MemberVisibility Visibility { get; set; }
            = MemberVisibility.Public;
    }
}
