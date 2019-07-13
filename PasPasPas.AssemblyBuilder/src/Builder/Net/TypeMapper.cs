using System;
using PasPasPas.Globals.Types;

namespace PasPasPas.AssemblyBuilder.Builder.Net {

    /// <summary>
    ///     type mappings
    /// </summary>
    public class TypeMapper {

        /// <summary>
        ///     map  a type to the target
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal Type Map(int value) {
            if (value == KnownTypeIds.NoType)
                return typeof(void);

            if (value == KnownTypeIds.ShortInt)
                return typeof(sbyte);

            if (value == KnownTypeIds.SmallInt)
                return typeof(short);

            if (value == KnownTypeIds.IntegerType)
                return typeof(int);

            if (value == KnownTypeIds.Int64Type)
                return typeof(long);

            if (value == KnownTypeIds.ByteType)
                return typeof(byte);

            if (value == KnownTypeIds.WordType)
                return typeof(ushort);

            if (value == KnownTypeIds.CardinalType)
                return typeof(uint);

            if (value == KnownTypeIds.Uint64Type)
                return typeof(ulong);



            throw new InvalidOperationException();
        }

        /// <summary>
        ///     map a type from the target
        /// </summary>
        /// <param name="returnType"></param>
        /// <returns></returns>
        internal int Map(Type returnType) {
            if (returnType == typeof(void))
                return KnownTypeIds.NoType;

            throw new InvalidOperationException();
        }
    }
}
