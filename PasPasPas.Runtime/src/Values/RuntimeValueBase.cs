using System.Diagnostics;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     base class for constant runtime values
    /// </summary>
    public abstract class RuntimeValueBase : IValue {

        /// <summary>
        ///     create a new runtime value
        /// </summary>
        /// <param name="typeDefinition"></param>
        protected RuntimeValueBase(ITypeDefinition typeDefinition) {
            TypeDefinition = typeDefinition;
            LogHistrogram();
        }

        [Conditional("DEBUG")]
        private void LogHistrogram() {
            if (Histograms.Enable)
                Histograms.Value(HistogramKeys.RuntimeValues, GetType().Name);
        }

        /// <summary>
        ///     type id
        /// </summary>
        public ITypeDefinition TypeDefinition { get; }

        /// <summary>
        ///     base type
        /// </summary>
        public BaseType BaseType
            => TypeDefinition.BaseType;

        /// <summary>
        ///     symbol type kind
        /// </summary>
        public SymbolTypeKind SymbolKind
            => SymbolTypeKind.Constant;
    }
}
