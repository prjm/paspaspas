using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     make a new invocation result
    /// </summary>
    public class InvocationResult : ITypeReference {

        /// <summary>
        ///     create a new invocation result
        /// </summary>
        /// <param name="resultType"></param>
        /// <param name="signature"></param>
        /// <param name="routine"></param>
        /// <param name="resultKind"></param>
        public InvocationResult(int resultType, CommonTypeKind resultKind, Signature signature, IRefSymbol routine) {
            ResultType = resultType;
            ResultKind = resultKind;
            Signature = signature;
            Routine = routine;
        }

        /// <summary>
        ///     result type
        /// </summary>
        public int TypeId
            => ResultType;

        /// <summary>
        ///     internal type format
        /// </summary>
        public string InternalTypeFormat
            => $"#{ResultType}";

        /// <summary>
        ///     invocation result
        /// </summary>
        public TypeReferenceKind ReferenceKind
            => TypeReferenceKind.InvocationResult;

        /// <summary>
        ///     result type kind
        /// </summary>
        public CommonTypeKind TypeKind
            => ResultKind;

        /// <summary>
        ///     result type
        /// </summary>
        public int ResultType { get; }

        /// <summary>
        ///     result kind
        /// </summary>
        public CommonTypeKind ResultKind { get; }

        /// <summary>
        ///     signature
        /// </summary>
        public Signature Signature { get; }

        /// <summary>
        ///     referenced routine
        /// </summary>
        public IRefSymbol Routine { get; }
    }
}