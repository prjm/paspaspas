using PasPasPas.Globals.Runtime;

namespace PasPasPas.Typings.Routines {


    /// <summary>
    ///     type specification for the <code>High</code> routine
    /// </summary>
    public class High : IntrinsicRoutine, IUnaryRoutine {

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => "High";

        /// <summary>
        ///     constant routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     check parameter types
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeReference parameter) {

            var typeKind = parameter.TypeKind;

            if (typeKind.IsType())
                typeKind = TypeRegistry.GetTypeKindOf(parameter.TypeId);

            if (typeKind.IsOrdinal())
                return true;

            if (typeKind.IsShortString())
                return true;

            if (typeKind.IsArray())
                return true;

            return false;
        }

        /// <summary>
        ///     execute a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ExecuteCall(ITypeReference parameter) {

            if (IsOrdinalType(parameter.TypeId, out var ordinalType))
                return ordinalType.HighestElement;

            if (IsShortStringType(parameter.TypeId, out var shortStringType))
                return shortStringType.Size;

            if (IsArrayType(parameter.TypeId, out var arrayType) &&  //
                arrayType.IndexTypes.Length > 0 & //
                IsOrdinalType(arrayType.IndexTypes[0], out var ordinalIndexType))
                return ordinalIndexType.HighestElement;

            return RuntimeException();
        }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ResolveCall(ITypeReference parameter)
            => ExecuteCall(parameter);
    }
}
