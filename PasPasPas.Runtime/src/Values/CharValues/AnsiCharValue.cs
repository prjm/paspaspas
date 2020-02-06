using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.CharValues {

    /// <summary>
    ///     ANSI char value
    /// </summary>
    public class AnsiCharValue : CharValueBase {

        /// <summary>
        ///     crate a new char value
        /// </summary>
        /// <param name="charValue"></param>
        /// <param name="typeDef">type id</param>
        public AnsiCharValue(ITypeDefinition typeDef, byte charValue) : base(typeDef, CharTypeKind.AnsiChar)
            => Value = charValue;

        /// <summary>
        ///     convert this value to a a wide char
        /// </summary>
        public override char AsWideChar
            => (char)Value;

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => Value;

        /// <summary>
        ///     ordinal value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public override IValue GetOrdinalValue(ITypeRegistry types)
            => types.Runtime.Integers.ToScaledIntegerValue(Value);

        /// <summary>
        ///     char value
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override IValue CharAt(int index) {
            if (index < 0 || index >= 1)
                return new ErrorValue(SystemUnit.ErrorType, SpecialConstantKind.InvalidChar);

            return this;
        }

        /// <summary>
        ///     char value
        /// </summary>
        public byte Value { get; }
    }
}
