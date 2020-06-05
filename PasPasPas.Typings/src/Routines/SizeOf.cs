#nullable disable
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     <pre>SizeOf</pre> routine
    /// </summary>
    public class SizeOf : IntrinsicRoutine, IUnaryRoutine {

        /// <summary>
        ///     constant routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     routine name <pre>SizeOf</pre>
        /// </summary>
        public override string Name
            => KnownNames.SizeOf;

        /// <summary>
        ///     procedure kind
        /// </summary>
        public RoutineKind Kind
            => RoutineKind.Function;

        /// <summary>
        ///     routine id
        /// </summary>
        public override IntrinsicRoutineId RoutineId
            => IntrinsicRoutineId.SizeOf;

        /// <summary>
        ///     check parameters
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeSymbol parameter)
            => true;

        /// <summary>
        ///     execute a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IValue ExecuteCall(IValue parameter)
            => ExecuteCall(parameter.TypeDefinition);

        /// <summary>
        ///     execute a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IValue ExecuteCall(ITypeDefinition parameter)
            => Integers.ToScaledIntegerValue(parameter.TypeSizeInBytes);

        /// <summary>
        ///     resolve type definition
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIntrinsicInvocationResult ResolveCall(ITypeSymbol parameter)
            => MakeResult(ExecuteCall(parameter.TypeDefinition), parameter);
    }
}
