using System.Text;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values.CharValues;
using PasPasPas.Runtime.Values.Other;

namespace PasPasPas.Runtime.Values.StringValues {

    /// <summary>
    ///     short string values
    /// </summary>
    internal class ShortStringValue : StringValueBase, IStringValue {

        private readonly string data;

        /// <summary>
        ///     create a new ANSI string value
        /// </summary>
        /// <param name="text"></param>
        /// <param name="typeDef">type id</param>
        public ShortStringValue(ITypeDefinition typeDef, string text) : base(typeDef, StringTypeKind.ShortString) {
            var buffer = new StringBuilder(text.Length);
            for (var i = 0; i < text.Length && i <= 255; i++) {
                var c = text[i];
                buffer.Append(c <= 255 ? c : '?');
            }
            data = buffer.ToString();
        }

        /// <summary>
        ///     string data
        /// </summary>
        public override string AsUnicodeString
            => data;

        /// <summary>
        ///     char value at
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override IValue CharAt(int index) {
            if (index < 0 || index >= data.Length)
                return new ErrorValue(SystemUnit.ErrorType, SpecialConstantKind.InvalidChar);

            var typeDef = TypeDefinition.DefiningUnit.TypeRegistry.SystemUnit.AnsiCharType;
            return new AnsiCharValue(typeDef, unchecked((byte)data[index]));
        }

        /// <summary>
        ///     get the value of this string
        /// </summary>
        /// <returns></returns>
        public override string GetValueString()
            => data;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(IValue? other)
            => other is ShortStringValue s && string.Equals(s.data, data, System.StringComparison.Ordinal);

        /// <summary>
        ///     number of char elements
        /// </summary>
        public override int NumberOfCharElements
            => data.Length;

    }
}
