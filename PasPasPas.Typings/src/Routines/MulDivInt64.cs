#nullable disable
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
            => KnownNames.MulDivInt64;

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
        public bool CheckParameter(ISignature signature) {

            if (signature.Count != 3)
                return false;

            foreach (var param in signature) {

                if (param.GetBaseType() == BaseType.Integer || param.HasSubrangeType(out var subrangeType) && subrangeType.SubrangeOfType.BaseType == BaseType.Integer)
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
        public IValue ExecuteCall(ISignature signature) {
            bool GetValue(int index, out IIntegerValue value) {
                if (signature[index] is IIntegerValue) {
                    value = signature[index] as IIntegerValue;
                    return true;
                }

                if (signature[index] is ISubrangeValue subrangeValue && subrangeValue.GetBaseType() == BaseType.Integer) {
                    value = subrangeValue.WrappedValue as IIntegerValue;
                    return true;
                }
                value = default;
                return false;
            };

            if (!GetValue(0, out var value1))
                return Integers.Invalid;

            if (!GetValue(1, out var value2))
                return Integers.Invalid;

            if (!GetValue(2, out var value3))
                return Integers.Invalid;

            var result = (long)UInt128.Mul64To128DivTo64(value1.UnsignedValue, value2.UnsignedValue, value3.UnsignedValue);
            return Integers.ToScaledIntegerValue(result);
        }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public IIntrinsicInvocationResult ResolveCall(ISignature signature)
            => MakeResult(TypeRegistry.SystemUnit.Int64Type.Reference, signature);
    }
}
