#nullable disable
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     <c>length</c> of strings or arrays
    /// </summary>
    public class Length : IntrinsicRoutine, IUnaryRoutine {

        /// <summary>
        ///     routine name <c>Length</c>
        /// </summary>
        public override string Name
            => KnownNames.Length;

        /// <summary>
        ///     procedure kind
        /// </summary>
        public RoutineKind Kind
            => RoutineKind.Function;

        /// <summary>
        ///     constant routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     <c>length</c> routine id
        /// </summary>
        public override IntrinsicRoutineId RoutineId
            => IntrinsicRoutineId.Length;

        /// <summary>
        ///     check parameter
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeSymbol parameter) {

            var typeKind = parameter.GetBaseType();

            if (parameter.TypeDefinition.IsSubrangeType(out var subrangeType))
                typeKind = subrangeType.SubrangeOfType.BaseType;

            if (typeKind == BaseType.String)
                return true;

            if (typeKind == BaseType.Char)
                return true;

            if (typeKind == BaseType.Array)
                return true;

            return false;
        }

        /// <summary>
        ///     get the length of a string value
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IValue ExecuteCall(IValue parameter) {

            if (parameter is IStringValue stringValue)
                return Integers.ToScaledIntegerValue(stringValue.NumberOfCharElements);

            if (parameter is ICharValue)
                return Integers.ToScaledIntegerValue(1);

            if (parameter is IArrayValue arrayValue)
                return Integers.ToScaledIntegerValue(arrayValue.Values.Length);

            if (parameter is ISubrangeValue subrangeValue)
                return ExecuteCall(subrangeValue.WrappedValue);

            return Integers.Invalid; ;
        }

        /// <summary>
        ///   resolve the call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIntrinsicInvocationResult ResolveCall(ITypeSymbol parameter)
            => MakeResult(TypeRegistry.SystemUnit.IntegerType.Reference, parameter);
    }
}
