using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     <pre>pred</pre> or <pre>succ</pre> routine mode
    /// </summary>
    public enum PredSuccMode {

        /// <summary>
        ///     undefined mode
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     <pre>pred</pre>
        /// </summary>
        Pred = 1,

        /// <summary>
        ///     <pre>succ</pre>
        /// </summary>
        Succ = 2

    }

    /// <summary>
    ///     increment values
    /// </summary>
    public class PredOrSucc : IntrinsicRoutine, IUnaryRoutine {

        /// <summary>
        ///     crate a <pre>pred</pre> or <pre>succ</pre> routine
        /// </summary>
        /// <param name="mode"></param>
        public PredOrSucc(PredSuccMode mode) {
            if (mode == PredSuccMode.Undefined)
                throw new System.ArgumentOutOfRangeException(nameof(mode));

            Mode = mode;
        }

        /// <summary>
        ///     <pre>pred</pre>
        /// </summary>
        public bool Pred
            => Mode == PredSuccMode.Pred;

        /// <summary>
        ///     <pre>succ</pre>
        /// </summary>
        public bool Succ
            => Mode == PredSuccMode.Succ;

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => Pred ? "Pred" : "Succ";

        /// <summary>
        ///     routine mode
        /// </summary>
        public PredSuccMode Mode { get; }

        /// <summary>
        ///     <c>true</c> constant routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     check for ordinal parameters
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeReference parameter)
            => parameter.IsOrdinal();

        /// <summary>
        ///     execute the call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ExecuteCall(ITypeReference parameter) {
            var ordinalType = TypeRegistry.GetTypeByIdOrUndefinedType(parameter.TypeId) as IOrdinalType;

            if (parameter.IsSubrangeValue(out var subrangeValue))
                return Types.MakeSubrangeValue(parameter.TypeId, ExecuteCall(subrangeValue.Value));

            if (Pred) {

                if (parameter.IsIntegralValue(out var intValue))
                    return Integers.Subtract(intValue, Integers.One);

                if (parameter.IsAnsiCharValue(out var charValue))
                    return Chars.ToAnsiCharValue(parameter.TypeId, unchecked((byte)(charValue.AsAnsiChar - 1)));

                if (parameter.IsWideCharValue(out charValue))
                    return Chars.ToWideCharValue(parameter.TypeId, unchecked((char)(charValue.AsWideChar - 1)));

                if (parameter.IsBooleanValue(out var boolValue)) {
                    var mask = 1u << ((int)ordinalType.BitSize - 1);
                    return Booleans.ToBoolean(ordinalType.BitSize, (boolValue.AsUint - 1u) & mask);
                }

                if (parameter.IsEnumValue(out var enumValue))
                    return Types.MakeEnumValue(enumValue.TypeId, ExecuteCall(enumValue.Value));

            }

            if (Succ) {

                if (parameter.IsIntegralValue(out var intValue))
                    return Integers.Add(intValue, Integers.One);

                if (parameter.IsAnsiCharValue(out var charValue))
                    return Chars.ToAnsiCharValue(parameter.TypeId, unchecked((byte)(1 + charValue.AsAnsiChar)));

                if (parameter.IsWideCharValue(out charValue))
                    return Chars.ToWideCharValue(parameter.TypeId, unchecked((char)(1 + charValue.AsWideChar)));

                if (parameter.IsBooleanValue(out var boolValue)) {
                    var mask = 1u << ((int)ordinalType.BitSize - 1);
                    return Booleans.ToBoolean(ordinalType.BitSize, (1u + boolValue.AsUint) & mask);
                }

                if (parameter.IsEnumValue(out var enumValue))
                    return Types.MakeEnumValue(enumValue.TypeId, ExecuteCall(enumValue.Value));

            }

            return RuntimeException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ResolveCall(ITypeReference parameter)
            => MakeTypeInstanceReference(parameter.TypeId);
    }
}
