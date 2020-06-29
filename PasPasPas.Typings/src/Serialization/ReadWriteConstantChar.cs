using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Serialization {

    internal static class CharKindHelper {

        internal static byte ToByte(this CharTypeKind kind)
            => (byte)kind;

        internal static CharTypeKind ToCharTypeKind(this byte typeKind)
            => (CharTypeKind)typeKind;

    }

    internal partial class TypeReader {

        internal IValue ReadCharValue() {
            var kind = ReadByte().ToCharTypeKind();

            switch (kind) {

                case CharTypeKind.AnsiChar:
                    return Types.Runtime.Chars.ToAnsiCharValue(ReadByte());

                case CharTypeKind.WideChar:
                    return Types.Runtime.Chars.ToWideCharValue((char)ReadUshort());

            }

            throw new InvalidOperationException();

        }

    }

    internal partial class TypeWriter {

        internal void WriteCharValue(ICharValue charValue) {
            var kind = charValue.Kind;
            WriteByte(kind.ToByte());

            switch (kind) {
                case CharTypeKind.AnsiChar:
                    WriteByte(charValue.AsAnsiChar);
                    return;

                case CharTypeKind.WideChar:
                    WriteUShort(charValue.AsWideChar);
                    return;

            }

            throw new InvalidOperationException();
        }

    }
}
