using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Serialization {

    internal static class BooleanKindHelper {

        internal static byte ToByte(this BooleanTypeKind kind)
            => (byte)kind;

        internal static BooleanTypeKind ToBooleanTypeKind(this byte typeKind)
            => (BooleanTypeKind)typeKind;

    }

    internal partial class TypeReader {

        internal IValue ReadBooleanValue() {
            var kind = ReadByte().ToBooleanTypeKind();

            switch (kind) {

                case BooleanTypeKind.Boolean:
                    return Types.Runtime.Booleans.ToBoolean(ReadByte() != 0);

            }

            throw new InvalidOperationException();

        }

    }

    internal partial class TypeWriter {

        internal void WriteBooleanValue(IBooleanValue booleanValue) {
            var kind = booleanValue.Kind;
            WriteByte(kind.ToByte());
            var typeDef = booleanValue.TypeDefinition;
            switch (kind) {
                case BooleanTypeKind.Boolean:
                    WriteByte(booleanValue.AsBoolean ? (byte)0xff : (byte)0x00);
                    break;
            }
        }

    }
}
