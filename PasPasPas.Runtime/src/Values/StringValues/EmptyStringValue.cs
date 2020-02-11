using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values.Other;

namespace PasPasPas.Runtime.Values.StringValues {

    /// <summary>
    ///     constant empty string value
    /// </summary>
    public class EmptyStringValue : StringValueBase {

        /// <summary>
        ///     create a new empty string value
        /// </summary>
        /// <param name="typeDef"></param>
        public EmptyStringValue(ITypeDefinition typeDef) : base(typeDef, StringTypeKind.ShortString) { }

        /// <summary>
        ///     get the empty string
        /// </summary>
        public override string AsUnicodeString
            => string.Empty;


        /// <summary>
        ///     number of chars
        /// </summary>
        public override int NumberOfCharElements
            => 0;

        /// <summary>
        ///     char at index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override IValue CharAt(int index)
            => new ErrorValue(SystemUnit.ErrorType, SpecialConstantKind.InvalidChar);
    }
}