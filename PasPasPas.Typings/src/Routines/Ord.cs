using PasPasPas.Globals.Runtime;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     implementation of the <c>ord</c> routine
    /// </summary>
    public class Ord : IntrinsicRoutine, IUnaryRoutine {

        /// <summary>
        ///     constant routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     routine name <c>Ord</c>
        /// </summary>
        public override string Name
            => "Ord";

        /// <summary>
        ///     check parameter types
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeReference parameter)
            => parameter.IsOrdinal();

        /// <summary>
        ///     get the ordinal value
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ExecuteCall(ITypeReference parameter) {
            if (!parameter.IsOrdinalValue(out var value))
                return RuntimeException();

            return value.GetOrdinalValue(TypeRegistry);
        }

        public ITypeReference ResolveCall(ITypeReference parameter) => throw new System.NotImplementedException();
    }
}
