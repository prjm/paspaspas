using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Serialization {

    internal static class StoredConstantKindHelper {

        internal static StoredConstantKind GetKindForConstant(IValue value)
            => value switch
            {
                IIntegerValue _ => StoredConstantKind.IntegerConstant,
                IRealNumberValue _ => StoredConstantKind.RealConstant,
                IBooleanValue _ => StoredConstantKind.BooleanConstant,
                ICharValue _ => StoredConstantKind.CharConstant,
                IStringValue _ => StoredConstantKind.StringConstant,
                _ => StoredConstantKind.Undefined,
            };

        internal static int ToInteger(this StoredConstantKind kind)
            => (int)kind;

        internal static StoredConstantKind ToStoredConstantKind(this int intValue)
            => (StoredConstantKind)intValue;


    }


    internal partial class TypeReader {

        /// <summary>
        ///     read a constant value
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public IValue ReadConstant(IStringRegistry strings)
            => ReadInt().ToStoredConstantKind() switch
            {
                StoredConstantKind.IntegerConstant => ReadIntValue(),
                StoredConstantKind.RealConstant => ReadRealValue(),
                StoredConstantKind.BooleanConstant => ReadBooleanValue(),
                StoredConstantKind.CharConstant => ReadCharValue(),
                StoredConstantKind.StringConstant => ReadStringValue(strings),
                _ => throw new InvalidOperationException(),
            };
    }

    internal partial class TypeWriter {

        /// <summary>
        ///     prepare the constant
        /// </summary>
        /// <param name="constant"></param>
        /// <param name="strings"></param>
        public void PrepareConstant(IValue constant, IStringRegistry strings) {

            if (constant is IStringValue value && value.Kind == StringTypeKind.UnicodeString)
                _ = strings[value.AsUnicodeString];
        }

        /// <summary>
        ///     write a constant value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="strings"></param>
        public void WriteConstant(IValue value, IStringRegistry strings) {
            var kind = StoredConstantKindHelper.GetKindForConstant(value);
            WriteInt(kind.ToInteger());

            switch (kind) {
                case StoredConstantKind.IntegerConstant:
                    WriteIntValue((IIntegerValue)value);
                    return;

                case StoredConstantKind.RealConstant:
                    WriteRealValue((IRealNumberValue)value);
                    return;

                case StoredConstantKind.BooleanConstant:
                    WriteBooleanValue((IBooleanValue)value);
                    return;

                case StoredConstantKind.CharConstant:
                    WriteCharValue((ICharValue)value);
                    return;

                case StoredConstantKind.StringConstant:
                    WriteStringValue((IStringValue)value, strings);
                    return;

            }

            throw new InvalidOperationException();
        }

    }
}
