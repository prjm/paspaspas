using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     helper class to detect types for literal values
    /// </summary>
    public static class LiteralValuesHelper {

        /// <summary>
        ///     determine the type for a given literal value
        /// </summary>
        /// <param name="literalValue">literal value</param>
        /// <returns>type id</returns>
        public static int GetTypeFor(object literalValue) {

            if (literalValue is byte)
                return KnownTypeIds.ByteType;
            else if (literalValue is sbyte)
                return KnownTypeIds.ShortInt;
            else if (literalValue is ushort)
                return KnownTypeIds.WordType;
            else if (literalValue is short)
                return KnownTypeIds.SmallInt;
            else if (literalValue is int)
                return KnownTypeIds.IntegerType;
            else if (literalValue is uint)
                return KnownTypeIds.CardinalType;
            else if (literalValue is ulong)
                return KnownTypeIds.UInt64Type;
            else if (literalValue is long)
                return KnownTypeIds.Int64Type;
            else if (literalValue is bool)
                return KnownTypeIds.BooleanType;
            else if (literalValue is char)
                return KnownTypeIds.WideCharType;
            else if (literalValue is string)
                return KnownTypeIds.StringType;
            else if (literalValue is double)
                return KnownTypeIds.Extended;

            return KnownTypeIds.ErrorType;
        }
    }
}
