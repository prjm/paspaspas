using PasPasPas.Globals.Runtime;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     type specification for the <code>Abs</code> routine
    /// </summary>
    public class Abs : IntrinsicRoutine, IUnaryRoutine {

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => "Abs";

        /// <summary>
        ///     check parameter type kind
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeReference parameter) {
            if (parameter.IsNumerical())
                return true;

            if (parameter.IsSubrangeValue(out var value))
                return CheckParameter(value.Value);

            return false;
        }

        /// <summary>
        ///     check if this routine is constant
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ResolveCall(ITypeReference parameter)
            => parameter;

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ExecuteCall(ITypeReference parameter) {

            if (parameter.TypeKind.IsIntegral())
                return Integers.Abs(parameter);

            if (parameter.TypeKind == CommonTypeKind.RealType)
                return RealNumbers.Abs(parameter);

            if (parameter.TypeKind == CommonTypeKind.SubrangeType && parameter is ISubrangeValue value)
                return MakeSubrangeValue(parameter.TypeId, ExecuteCall(value.Value));

            return RuntimeException();
        }
    }
}
