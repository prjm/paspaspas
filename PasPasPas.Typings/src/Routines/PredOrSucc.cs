using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     <c>pred</c> or <c>succ</c> routine mode
    /// </summary>
    public enum PredSuccMode {

        /// <summary>
        ///     undefined mode
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     <c>pred</c>
        /// </summary>
        Pred = 1,

        /// <summary>
        ///     <c>succ</c>
        /// </summary>
        Succ = 2

    }

    /// <summary>
    ///     increment values
    /// </summary>
    public class PredOrSucc : IntrinsicRoutine, IUnaryRoutine {

        /// <summary>
        ///     crate a <c>pred</c> or <c>succ</c> routine
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
        ///     <c>pred</c>
        /// </summary>
        public bool Pred
            => Mode == PredSuccMode.Pred;

        /// <summary>
        ///     <c>succ</c>
        /// </summary>
        public bool Succ
            => Mode == PredSuccMode.Succ;

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => Pred ? KnownNames.Pred : KnownNames.Succ;

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
        public bool CheckParameter(ITypeSymbol parameter)
            => parameter.TypeDefinition is IOrdinalType;

        /// <summary>
        ///     execute the call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IValue ExecuteCall(IValue parameter)
            => StaticExecuteCall(TypeRegistry, parameter, Pred);

        /// <summary>
        ///     execute the call
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="pred"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static IValue StaticExecuteCall(ITypeRegistry types, ITypeSymbol parameter, bool pred) {

            var ordinalType = parameter.TypeDefinition as IOrdinalType;

            if (parameter is ISubrangeValue subrangeValue)
                return types.Runtime.Types.MakeSubrangeValue(parameter.TypeDefinition, PredOrSucc.StaticExecuteCall(types, subrangeValue.WrappedValue, pred));

            if (ordinalType is null)
                return types.Runtime.Integers.Invalid;

            if (pred) {

                if (parameter is IIntegerValue intValue)
                    return types.Runtime.Integers.Subtract(intValue, types.Runtime.Integers.One);

                if (parameter is ICharValue charValue)
                    if (charValue.Kind == CharTypeKind.AnsiChar)
                        return types.Runtime.Chars.ToAnsiCharValue(parameter.TypeDefinition, unchecked((byte)(charValue.AsAnsiChar - 1)));
                    else if (charValue.Kind == CharTypeKind.WideChar)
                        return types.Runtime.Chars.ToWideCharValue(parameter.TypeDefinition, unchecked((char)(charValue.AsWideChar - 1)));

                if (parameter is IBooleanValue boolValue) {
                    return types.Runtime.Booleans.ToBoolean(boolValue.TypeDefinition as IBooleanType, boolValue.AsUint == uint.MinValue ? uint.MinValue : (boolValue.Kind == BooleanTypeKind.Boolean ? 0 : boolValue.AsUint - 1));
                }

                if (parameter is IEnumeratedValue enumValue)
                    return types.Runtime.MakeEnumValue(enumValue.TypeDefinition, StaticExecuteCall(types, enumValue.Value, pred) as IIntegerValue, string.Empty);

            }

            if (!pred) {

                if (parameter is IIntegerValue intValue)
                    return types.Runtime.Integers.Add(intValue, types.Runtime.Integers.One);

                if (parameter is ICharValue charValue)
                    if (charValue.Kind == CharTypeKind.AnsiChar)
                        return types.Runtime.Chars.ToAnsiCharValue(parameter.TypeDefinition, unchecked((byte)(1 + charValue.AsAnsiChar)));
                    else if (charValue.Kind == CharTypeKind.WideChar)
                        return types.Runtime.Chars.ToWideCharValue(parameter.TypeDefinition, unchecked((char)(1 + charValue.AsWideChar)));

                if (parameter is IBooleanValue boolValue) {
                    return types.Runtime.Booleans.ToBoolean(boolValue.TypeDefinition as IBooleanType, boolValue.AsUint == uint.MaxValue ? uint.MaxValue : boolValue.AsUint + 1);
                }

                if (parameter is IEnumeratedValue enumValue)
                    return types.Runtime.MakeEnumValue(enumValue.TypeDefinition, StaticExecuteCall(types, enumValue.Value, pred) as IIntegerValue, string.Empty);

            }

            return types.Runtime.Integers.Invalid;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIntrinsicInvocationResult ResolveCall(ITypeSymbol parameter)
            => MakeResult(parameter, parameter);
    }
}
