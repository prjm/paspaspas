#nullable disable
using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Serialization {

    internal static class IntegralKindHelper {

        internal static byte ToByte(this IntegralTypeKind kind)
            => (byte)kind;

        internal static IntegralTypeKind ToIntegralTypeKind(this byte typeKind)
            => (IntegralTypeKind)typeKind;

    }

    internal partial class TypeReader {

        internal IValue ReadIntValue() {
            var kind = ReadByte().ToIntegralTypeKind();

            switch (kind) {

                case IntegralTypeKind.Byte:
                    return Types.Runtime.Integers.ToIntegerValue((sbyte)ReadByte());

                case IntegralTypeKind.ShortInt:
                    return Types.Runtime.Integers.ToIntegerValue(ReadByte());

            }

            throw new InvalidOperationException();

        }

    }

    internal partial class TypeWriter {

        internal void WriteIntValue(IIntegerValue intValue) {
            WriteByte(intValue.IntegralType.Kind.ToByte());
            var typeDef = intValue.TypeDefinition as IIntegralType;
            switch (typeDef.TypeSizeInBytes) {
                case 1:
                    WriteByte((byte)intValue.UnsignedValue);
                    break;
            }
        }

    }
}
