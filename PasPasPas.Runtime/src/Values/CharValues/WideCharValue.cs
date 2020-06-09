using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values.Other;

namespace PasPasPas.Runtime.Values.CharValues {

    /// <summary>
    ///     wide char (word) value
    /// </summary>
    internal class WideCharValue : CharValueBase {

        /// <summary>
        ///     create a new wide char value
        /// </summary>
        /// <param name="character"></param>
        /// <param name="typeDef">type id</param>
        internal WideCharValue(ITypeDefinition typeDef, char character) : base(typeDef, CharTypeKind.WideChar)
            => Value = character;

        /// <summary>
        ///     char value
        /// </summary>
        public override char AsWideChar
            => Value;

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
        ///     get this value as string
        /// </summary>
        /// <returns></returns>
        public override string GetValueString()
            => new string(Value, 1);

        public override bool Equals(IValue? other)
            => other is WideCharValue c && c.Value == Value;

        /// <summary>
        ///     char value
        /// </summary>
        public char Value { get; }
    }
}
