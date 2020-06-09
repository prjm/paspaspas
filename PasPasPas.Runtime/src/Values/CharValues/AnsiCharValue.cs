using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values.Other;

namespace PasPasPas.Runtime.Values.CharValues {

    /// <summary>
    ///     ANSI char value
    /// </summary>
    internal class AnsiCharValue : CharValueBase {

        /// <summary>
        ///     crate a new char value
        /// </summary>
        /// <param name="charValue"></param>
        /// <param name="typeDef">type id</param>
        internal AnsiCharValue(ITypeDefinition typeDef, byte charValue) : base(typeDef, CharTypeKind.AnsiChar)
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

        public override string GetValueString()
            => new string((char)Value, 1);

        public override bool Equals(IValue? other)
            => other is AnsiCharValue c && c.Value == Value;

        /// <summary>
        ///     char value
        /// </summary>
        public byte Value { get; }
    }
}
