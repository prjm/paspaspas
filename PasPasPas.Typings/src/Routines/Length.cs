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
            => "Length";

        /// <summary>
        ///     procedure kind
        /// </summary>
        public ProcedureKind Kind
            => ProcedureKind.Function;

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
        public bool CheckParameter(ITypeReference parameter) {

            var typeKind = parameter.TypeKind;

            if (IsSubrangeType(parameter.TypeId, out var subrangeType))
                typeKind = subrangeType.BaseType.TypeKind;


            if (typeKind.IsString())
                return true;

            if (typeKind.IsChar())
                return true;

            if (typeKind.IsArray())
                return true;

            return false;
        }

        /// <summary>
        ///     get the length of a string value
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ExecuteCall(ITypeReference parameter) {

            if (parameter.IsStringValue(out var stringValue))
                return Integers.ToScaledIntegerValue(stringValue.NumberOfCharElements);

            if (parameter.IsChar())
                return Integers.ToScaledIntegerValue(1);

            if (parameter.IsArrayValue(out var arrayValue))
                return Integers.ToScaledIntegerValue(arrayValue.Values.Length);

            if (parameter.IsSubrangeValue(out var subrangeValue))
                return ExecuteCall(subrangeValue.Value);

            return RuntimeException();
        }

        /// <summary>
        ///   resolve the call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ResolveCall(ITypeReference parameter)
            => MakeTypeInstanceReference(KnownTypeIds.IntegerType);
    }
}
