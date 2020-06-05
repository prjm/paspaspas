#nullable disable
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     <c>trunc</c> routine
    /// </summary>
    public class Trunc : IntrinsicRoutine, IUnaryRoutine {

        /// <summary>
        ///     constant routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => KnownNames.Trunc;

        /// <summary>
        ///     procedure kind
        /// </summary>
        public RoutineKind Kind
            => RoutineKind.Function;

        /// <summary>
        ///     routine id
        /// </summary>
        public override IntrinsicRoutineId RoutineId
            => IntrinsicRoutineId.Trunc;

        /// <summary>
        ///     check parameter
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeSymbol parameter) {
            if (parameter.HasNumericType())
                return true;

            if (parameter.HasSubrangeType(out var value))
                return value.SubrangeOfType.IsNumericType();

            return false;
        }

        /// <summary>
        ///     execute the call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IValue ExecuteCall(IValue parameter) {

            if (parameter is IIntegerValue)
                return parameter;

            if (parameter is ISubrangeValue)
                return parameter;

            if (parameter is IRealNumberValue realNumberValue)
                return RealNumbers.Trunc(realNumberValue);

            return Integers.Invalid;
        }

        /// <summary>
        ///     resolve the call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIntrinsicInvocationResult ResolveCall(ITypeSymbol parameter) {

            if (parameter.GetBaseType() == BaseType.Integer)
                return MakeResult(parameter.TypeDefinition.Reference, parameter);

            if (parameter.HasSubrangeType(out var _))
                return MakeResult(parameter.TypeDefinition.Reference, parameter);

            return MakeResult(TypeRegistry.SystemUnit.Int64Type.Reference, parameter);
        }
    }
}
