using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     <c>swap</c> function
    /// </summary>
    public class Swap : IntrinsicRoutine, IUnaryRoutine {


        /// <summary>
        ///     name of the function
        /// </summary>
        public override string Name
            => "Swap";

        /// <summary>
        ///     constant routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     procedure kind
        /// </summary>
        public ProcedureKind Kind
            => ProcedureKind.Function;

        /// <summary>
        ///     check parameter types
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeReference parameter) {
            if (parameter.IsIntegral())
                return true;

            if (IsSubrangeType(parameter.TypeId, out var subrangeType))
                return subrangeType.BaseType.TypeKind.IsIntegral();

            return false;
        }

        /// <summary>
        ///     execute a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ExecuteCall(ITypeReference parameter) {

            if (parameter.IsIntegral())
                return Integers.Swap(parameter, TypeRegistry);

            if (parameter.IsSubrangeValue(out var value))
                return ExecuteCall(value.Value);

            return RuntimeException();
        }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ResolveCall(ITypeReference parameter)
            => MakeTypeInstanceReference(parameter.TypeId);
    }
}
