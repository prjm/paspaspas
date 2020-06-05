#nullable disable
using System.Text;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using SharpFloat.FloatingPoint;

namespace PasPasPas.Runtime.Values.FloatValues {

    /// <summary>
    ///     extended value
    /// </summary>
    public class ExtendedValue : FloatValueBase {

        private readonly ExtF80 value;

        /// <summary>
        ///     create a new extend value
        /// </summary>
        /// <param name="typeDef"></param>
        /// <param name="extendedValue"></param>
        public ExtendedValue(ITypeDefinition typeDef, in ExtF80 extendedValue) : base(typeDef, RealTypeKind.Extended)
            => value = extendedValue;

        /// <summary>
        ///     test if the value is negative
        /// </summary>
        public override bool IsNegative
            => ExtF80.IsNegativeValue(value);

        /// <summary>
        ///     get the extended value
        /// </summary>
        public override ExtF80 AsExtended
            => value;

        /// <summary>
        ///     compare equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is IRealNumberValue floatValue)
                return floatValue.AsExtended == value;
            return false;
        }

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => ExtF80.GetHashCode(value);

        /// <summary>
        ///     get a formatted string of this value
        /// </summary>
        public string ToFormattedString {
            get {
                var builder = new StringBuilder();
                var _ = ExtF80.PrintFloat80(builder, value, PrintFloatFormat.PositionalFormat, 20);
                return builder.ToString();
            }
        }
    }
}
