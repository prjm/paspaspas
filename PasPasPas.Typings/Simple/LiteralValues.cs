using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     helper class to detect types for literal values
    /// </summary>
    public static class LiteralValues {

        public static int GetTypeFor(object literalValue) {

            if (literalValue is byte)
                return TypeIds.ByteType;

            return TypeIds.ErrorType;
        }
    }
}
