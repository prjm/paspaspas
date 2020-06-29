using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Serialization {

    internal static class StringKindHelper {

        internal static byte ToByte(this StringTypeKind kind)
            => (byte)kind;

        internal static StringTypeKind ToStringTypeKind(this byte typeKind)
            => (StringTypeKind)typeKind;

    }

    internal partial class TypeReader {

        internal IValue ReadStringValue(IStringRegistry strings) {
            var kind = ReadByte().ToStringTypeKind();

            switch (kind) {

                case StringTypeKind.UnicodeString:
                    var data = strings[ReadUint()];
                    return Types.Runtime.Strings.ToUnicodeString(data);

            }

            throw new InvalidOperationException();

        }

    }

    internal partial class TypeWriter {

        internal void WriteStringValue(IStringValue stringValue, IStringRegistry strings) {
            WriteByte(stringValue.Kind.ToByte());
            switch (stringValue.Kind) {
                case StringTypeKind.UnicodeString:
                    var data = strings[stringValue.AsUnicodeString];
                    WriteUint(data);
                    break;
            }
        }

    }
}
