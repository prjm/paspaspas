using System.Diagnostics;
using PasPasPas.Globals.Runtime;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     base class for runtime values
    /// </summary>
    public abstract class RuntimeValueBase : IOldTypeReference {

        /// <summary>
        ///     create a new runtime value
        /// </summary>
        /// <param name="typeId"></param>
        protected RuntimeValueBase(int typeId) {
            TypeId = typeId;
            LogHistrogram();
        }

        [Conditional("DEBUG")]
        private void LogHistrogram() {
            if (Histograms.Enable)
                Histograms.Value(HistogramKeys.RuntimeValues, GetType().Name);
        }


        /// <summary>
        ///     reference kind
        /// </summary>
        public TypeReferenceKind ReferenceKind
            => TypeReferenceKind.ConstantValue;


        /// <summary>
        ///     internal type format
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => InternalTypeFormat;

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId { get; }

        /// <summary>
        ///     type format
        /// </summary>
        public abstract string InternalTypeFormat { get; }

        /// <summary>
        ///     type kind
        /// </summary>
        public abstract CommonTypeKind TypeKind { get; }
    }
}
