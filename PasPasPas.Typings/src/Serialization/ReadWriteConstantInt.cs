using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Serialization {

    internal partial class TypeReader {

        internal IOldTypeReference ReadIntValue(IIntegralType typeDef) {

            switch (typeDef.TypeSizeInBytes) {
                case 1:
                    if (typeDef.IsSigned)
                        return Types.Runtime.Integers.ToIntegerValue((sbyte)ReadByte());
                    else
                        return Types.Runtime.Integers.ToIntegerValue(ReadByte());
            }

            throw new InvalidOperationException();

        }

    }

    internal partial class TypeWriter {

        internal void WriteIntValue(IIntegerValue intValue) {

            var typeDef = RegisteredTypes.GetTypeByIdOrUndefinedType(intValue.TypeId) as IIntegralType;
            switch (typeDef.TypeSizeInBytes) {
                case 1:
                    WriteByte((byte)intValue.UnsignedValue);
                    break;
            }
        }

    }
}
