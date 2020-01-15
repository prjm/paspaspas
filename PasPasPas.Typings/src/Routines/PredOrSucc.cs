using System;
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
        ///     procedure kind
        /// </summary>
        public RoutineKind Kind
            => RoutineKind.Function;

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
        ///     routine id
        /// </summary>
        public override IntrinsicRoutineId RoutineId {
            get {
                if (Mode == PredSuccMode.Pred)
                    return IntrinsicRoutineId.Pred;
                else if (Mode == PredSuccMode.Succ)
                    return IntrinsicRoutineId.Succ;
                throw new InvalidOperationException();
            }
        }

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
        public ITypeReference ExecuteCall(ITypeReference parameter)
            => StaticExecuteCall(TypeRegistry, parameter, Pred);

        /// <summary>
        ///     execute the call
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="pred"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static ITypeReference StaticExecuteCall(ITypeRegistry types, ITypeReference parameter, bool pred) {

            var ordinalType = types.GetTypeByIdOrUndefinedType(parameter.TypeId) as IOrdinalType;

            if (parameter.IsSubrangeValue(out var subrangeValue))
                return types.Runtime.Types.MakeSubrangeValue(parameter.TypeId, PredOrSucc.StaticExecuteCall(types, subrangeValue.Value, pred));

            if (pred) {

                if (parameter.IsIntegralValue(out var intValue))
                    return types.Runtime.Integers.Subtract(intValue, types.Runtime.Integers.One);

                if (parameter.IsAnsiCharValue(out var charValue))
                    return types.Runtime.Chars.ToAnsiCharValue(parameter.TypeId, unchecked((byte)(charValue.AsAnsiChar - 1)));

                if (parameter.IsWideCharValue(out charValue))
                    return types.Runtime.Chars.ToWideCharValue(parameter.TypeId, unchecked((char)(charValue.AsWideChar - 1)));

                if (parameter.IsBooleanValue(out var boolValue)) {
                    var mask = 1u << ((int)ordinalType.BitSize - 1);
                    return types.Runtime.Booleans.ToBoolean(ordinalType.BitSize, (boolValue.AsUint - 1u) & mask);
                }

                if (parameter.IsEnumValue(out var enumValue))
                    return types.Runtime.Types.MakeEnumValue(enumValue.TypeId, StaticExecuteCall(types, enumValue.Value, pred));

            }

            if (!pred) {

                if (parameter.IsIntegralValue(out var intValue))
                    return types.Runtime.Integers.Add(intValue, types.Runtime.Integers.One);

                if (parameter.IsAnsiCharValue(out var charValue))
                    return types.Runtime.Chars.ToAnsiCharValue(parameter.TypeId, unchecked((byte)(1 + charValue.AsAnsiChar)));

                if (parameter.IsWideCharValue(out charValue))
                    return types.Runtime.Chars.ToWideCharValue(parameter.TypeId, unchecked((char)(1 + charValue.AsWideChar)));

                if (parameter.IsBooleanValue(out var boolValue)) {
                    var mask = 1u << ((int)ordinalType.BitSize - 1);
                    return types.Runtime.Booleans.ToBoolean(ordinalType.BitSize, (1u + boolValue.AsUint) & mask);
                }

                if (parameter.IsEnumValue(out var enumValue))
                    return types.Runtime.Types.MakeEnumValue(enumValue.TypeId, StaticExecuteCall(types, enumValue.Value, pred));

            }

            return types.Runtime.Types.MakeErrorTypeReference();
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
