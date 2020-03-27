using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     type specification for the <c>chr</c> routine
    /// </summary>
    public class Chr : IntrinsicRoutine, IUnaryRoutine {

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => KnownNames.Chr;

        /// <summary>
        ///     constant routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     procedure kind
        /// </summary>
        public RoutineKind Kind
            => RoutineKind.Function;

        /// <summary>
        ///     <c>chr</c> routine id
        /// </summary>
        public override IntrinsicRoutineId RoutineId
            => IntrinsicRoutineId.Chr;

        /// <summary>
        ///     check if the parameter matches
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeSymbol parameter) {
            if (parameter.GetBaseType() == BaseType.Integer)
                return true;

            if (parameter.HasSubrangeType(out var subrangeType))
                return CheckParameter(subrangeType.SubrangeOfType);

            return false;
        }

        /// <summary>
        ///     execute call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IValue ExecuteCall(IValue parameter) {

            if (parameter is IIntegerValue integer)
                return TypeRegistry.Runtime.Integers.Chr(integer);

            if (parameter is ISubrangeValue subrange)
                return ExecuteCall(subrange.WrappedValue);

            return Runtime.Chars.Invalid;
        }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIntrinsicInvocationResult ResolveCall(ITypeSymbol parameter)
            => MakeResult(TypeRegistry.SystemUnit.WideCharType, parameter);
    }
}