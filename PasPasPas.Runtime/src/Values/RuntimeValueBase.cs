using System.Diagnostics;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     base class for constant runtime values
    /// </summary>
    internal abstract class RuntimeValueBase : IValue {

        /// <summary>
        ///     create a new runtime value
        /// </summary>
        /// <param name="typeDefinition"></param>
        protected RuntimeValueBase(ITypeDefinition typeDefinition) {
            TypeDefinition = typeDefinition;
            LogHistogram();
        }

        [Conditional("DEBUG")]
        private void LogHistogram() {
            if (Histograms.Enable)
                Histograms.Value(HistogramKeys.RuntimeValues, GetType().Name);
        }

        /// <summary>
        ///     retrieve a value string
        /// </summary>
        /// <returns></returns>
        public string ToValueString()
            => Runtime.GetValueString(this);

        /// <summary>
        ///     get a value string
        /// </summary>
        /// <returns></returns>
        public abstract string GetValueString();

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public abstract bool Equals(IValue? other);

        /// <summary>
        ///     type id
        /// </summary>
        public ITypeDefinition TypeDefinition { get; }

        /// <summary>
        ///     system unit
        /// </summary>
        public ISystemUnit SystemUnit
            => TypeDefinition.DefiningUnit.TypeRegistry.SystemUnit;

        /// <summary>
        ///     base type
        /// </summary>
        protected BaseType BaseType
            => TypeDefinition.BaseType;

        /// <summary>
        ///     symbol type kind
        /// </summary>
        public SymbolTypeKind SymbolKind
            => SymbolTypeKind.Constant;

        /// <summary>
        ///     access the runtime object
        /// </summary>
        protected IRuntimeValueFactory Runtime
            => TypeDefinition.DefiningUnit.TypeRegistry.Runtime;

        /// <summary>
        ///     default equals mechanism
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj) {
            if (ReferenceEquals(obj, this))
                return true;

            if (obj is IValue value)
                return Equals(value);

            return false;
        }

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public abstract override int GetHashCode();

        /// <summary>
        ///     value of this constant
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => GetValueString();

        public bool Equals(ITypeSymbol? other)
            => Equals(other as IValue);
    }
}
