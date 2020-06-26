using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using SharpFloat.FloatingPoint;

namespace PasPasPas.Typings.Serialization {

    internal static class RealKindHelper {

        internal static byte ToByte(this RealTypeKind kind)
            => (byte)kind;

        internal static RealTypeKind ToRealTypeTypeKind(this byte typeKind)
            => (RealTypeKind)typeKind;

    }

    internal partial class TypeReader {

        internal IValue ReadRealValue() {
            var kind = ReadByte().ToRealTypeTypeKind();

            switch (kind) {

                case RealTypeKind.Extended:
                    var signif = ReadUlong();
                    var signExp = ReadUshort();
                    return Types.Runtime.RealNumbers.ToExtendedValue(new ExtF80(signExp, signif));

            }

            throw new InvalidOperationException();

        }

    }

    internal partial class TypeWriter {

        internal void WriteRealValue(IRealNumberValue realValue) {
            WriteByte(realValue.RealType.Kind.ToByte());
            var typeDef = realValue.RealType;
            switch (typeDef.TypeSizeInBytes) {
                case 10:
                    WriteUlong(realValue.AsExtended.signif);
                    WriteUShort(realValue.AsExtended.signExp);
                    break;
            }
        }

    }
}
