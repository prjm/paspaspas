using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     type specification for the <c>chr</c> routine
    /// </summary>
    public class Chr : IntrinsicRoutine, IUnaryRoutine {

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => "Chr";

        /// <summary>
        ///     constant routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     check if the parameter matches
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeReference parameter) {
            if (parameter.TypeKind.IsIntegral())
                return true;

            if (parameter.TypeKind == CommonTypeKind.SubrangeType && parameter is ISubrangeValue value)
                return CheckParameter(value.Value);

            return false;
        }

        /// <summary>
        ///     execute call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ExecuteCall(ITypeReference parameter) {

            if (parameter.TypeKind.IsIntegral())
                return TypeRegistry.Runtime.Integers.Chr(parameter);

            if (parameter.TypeKind == CommonTypeKind.SubrangeType && parameter is ISubrangeValue value)
                return MakeSubrangeValue(parameter.TypeId, ExecuteCall(value.Value));

            return RuntimeException();
        }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ResolveCall(ITypeReference parameter)
            => TypeRegistry.Runtime.Types.MakeReference(KnownTypeIds.CharType, TypeRegistry.GetTypeKindOf(KnownTypeIds.CharType));
    }
}