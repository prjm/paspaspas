using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using SharpFloat.Helpers;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    /// <c>MulDivInt64</c> function
    /// </summary>
    public class MulDivInt64 : IntrinsicRoutine, IConstantVariadicRoutine {

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => "MulDivInt64";

        /// <summary>
        ///     procedure kind
        /// </summary>
        public RoutineKind Kind
            => RoutineKind.Function;

        /// <summary>
        ///     intrinsic routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     <c>muldiv64</c> routine id
        /// </summary>
        public override IntrinsicRoutineId RoutineId
            => IntrinsicRoutineId.MulDiv64;

        /// <summary>
        ///     check parameters
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool CheckParameter(Signature signature) {

            if (signature.Length != 3)
                return false;

            for (var i = 0; i < signature.Length; i++) {

                if (signature[i].TypeKind.IsIntegral() || IsSubrangeType(signature[i].TypeId, out var subrangeType) && subrangeType.BaseType.TypeKind.IsIntegral())
                    continue;

                return false;
            }

            return true;
        }

        /// <summary>
        ///     execute a call
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public IOldTypeReference ExecuteCall(Signature signature) {
            bool GetValue(int index, out IIntegerValue value) {
                if (signature[index].IsIntegralValue(out value))
                    return true;

                if (signature[index].IsSubrangeValue(out var subrangeValue) && subrangeValue.Value.IsIntegralValue(out value))
                    return true;

                value = default;
                return false;
            };

            if (!GetValue(0, out var value1))
                return RuntimeException();

            if (!GetValue(1, out var value2))
                return RuntimeException();

            if (!GetValue(2, out var value3))
                return RuntimeException();

            var result = (long)UInt128.Mul64To128DivTo64(value1.UnsignedValue, value2.UnsignedValue, value3.UnsignedValue);
            return Integers.ToScaledIntegerValue(result);
        }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public IOldTypeReference ResolveCall(Signature signature)
            => MakeTypeInstanceReference(KnownTypeIds.Int64Type);
    }
}
