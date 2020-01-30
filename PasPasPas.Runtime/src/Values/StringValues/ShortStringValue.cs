using System.Text;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values.CharValues;

namespace PasPasPas.Runtime.Values.StringValues {

    /// <summary>
    ///     short string values
    /// </summary>
    public class ShortStringValue : StringValueBase, IStringValue {

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
                return new SpecialValue(SpecialConstantKind.InvalidChar);

            var typeDef = TypeDefinition.DefiningUnit.TypeRegistry.SystemUnit.AnsiCharType;
            return new AnsiCharValue(typeDef, unchecked((byte)data[index]));
        }

        /// <summary>
        ///     number of char elements
        /// </summary>
        public override int NumberOfCharElements
            => data.Length;

    }
}
