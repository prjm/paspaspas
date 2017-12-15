using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     helper class to detect types for literal values
    /// </summary>
    public static class LiteralValues {

        /// <summary>
        ///     determine the type for a given literal value
        /// </summary>
        /// <param name="literalValue">literal value</param>
        /// <returns>type id</returns>
        public static int GetTypeFor(object literalValue) {

            if (literalValue is byte)
                return TypeIds.ByteType;
            else if (literalValue is sbyte)
                return TypeIds.ShortInt;
            else if (literalValue is ushort)
                return TypeIds.WordType;
            else if (literalValue is short)
                return TypeIds.SmallInt;
            else if (literalValue is int)
                return TypeIds.IntegerType;
            else if (literalValue is uint)
                return TypeIds.CardinalType;
            else if (literalValue is ulong)
                return TypeIds.Uint64Type;
            else if (literalValue is long)
                return TypeIds.Int64Type;
            else if (literalValue is bool)
                return TypeIds.BooleanType;
            else if (literalValue is char)
                return TypeIds.WideCharType;
            else if (literalValue is string)
                return TypeIds.StringType;
            else if (literalValue is double)
                return TypeIds.Extended;

            return TypeIds.ErrorType;
        }
    }
}
