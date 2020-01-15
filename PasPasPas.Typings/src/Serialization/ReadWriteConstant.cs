using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Serialization {

    internal partial class TypeReader {

        /// <summary>
        ///     read a constant value
        /// </summary>
        /// <returns></returns>
        public ITypeReference ReadConstant() {
            var typeId = ReadInt();
            var typeDef = Types.GetTypeByIdOrUndefinedType(typeId);

            if (typeDef is IIntegralType intType)
                return ReadIntValue(intType);

            else
                throw new InvalidOperationException();

        }

    }

    internal partial class TypeWriter {


        /// <summary>
        ///     write a constant value
        /// </summary>
        /// <param name="value"></param>
        public void WriteConstant(ITypeReference value) {
            WriteInt(value.TypeId);

            if (value is IIntegerValue intValue)
                WriteIntValue(intValue);

            else
                throw new InvalidOperationException();

        }

    }
}
