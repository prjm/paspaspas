using System;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Typings.Serialization {

    internal static class StoredConstantKindHelper {

        internal static StoredConstantKind GetKindForConstant(IValue value) {

            switch (value) {
                case IIntegerValue _:
                    return StoredConstantKind.IntegerConstant;
            }

            return StoredConstantKind.Undefined;
        }

        internal static int ToInteger(this StoredConstantKind kind)
            => (int)kind;

        internal static StoredConstantKind ToStoredConstantKind(this int intValue)
            => (StoredConstantKind)intValue;


    }


    internal partial class TypeReader {

        /// <summary>
        ///     read a constant value
        /// </summary>
        /// <returns></returns>
        public IValue ReadConstant() {
            var typeId = ReadInt().ToStoredConstantKind();

            switch (typeId) {
                case StoredConstantKind.IntegerConstant:
                    return ReadIntValue();
            }

            throw new InvalidOperationException();
        }
    }

    internal partial class TypeWriter {


        /// <summary>
        ///     write a constant value
        /// </summary>
        /// <param name="value"></param>
        public void WriteConstant(IValue value) {
            var kind = StoredConstantKindHelper.GetKindForConstant(value);
            WriteInt(kind.ToInteger());

            switch (kind) {
                case StoredConstantKind.IntegerConstant:
                    WriteIntValue(value as IIntegerValue);
                    return;
            }

            throw new InvalidOperationException();
        }

    }
}
