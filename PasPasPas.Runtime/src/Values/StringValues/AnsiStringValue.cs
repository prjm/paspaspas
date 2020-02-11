using System.Text;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values.CharValues;
using PasPasPas.Runtime.Values.Other;

namespace PasPasPas.Runtime.Values.StringValues {

    /// <summary>
    ///     ANSI string values
    /// </summary>
    public class AnsiStringValue : StringValueBase, IStringValue {

        private readonly string data;

        /// <summary>
        ///     create a new ANSI string value
        /// </summary>
        /// <param name="text"></param>
        /// <param name="typeId">type id</param>
        public AnsiStringValue(ITypeDefinition typeId, string text) : base(typeId, StringTypeKind.AnsiString) {
            var buffer = new StringBuilder(text.Length);
            for (var i = 0; i < text.Length; i++) {
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
        ///    get a char at
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
        ///     number of char elements
        /// </summary>
        public override int NumberOfCharElements
            => data.Length;
    }
}
