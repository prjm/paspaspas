using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.CharValues {

    /// <summary>
    ///     base class for char values
    /// </summary>
    internal abstract class CharValueBase : RuntimeValueBase, ICharValue, IStringValue {

        /// <summary>
        ///     create a new char value
        /// </summary>
        /// <param name="typeDef"></param>
        /// <param name="kind"></param>
        protected CharValueBase(ITypeDefinition typeDef, CharTypeKind kind) : base(typeDef) {
            if (typeDef.BaseType != BaseType.Char)
                throw new ArgumentException(string.Empty, nameof(typeDef));

            if (!(typeDef is ICharType charType))
                throw new ArgumentException(string.Empty, nameof(typeDef));

            if (charType.Kind != kind)
                throw new ArgumentException(string.Empty, nameof(typeDef));
        }

        /// <summary>
        ///     char type kind
        /// </summary>
        public CharTypeKind Kind
            => (TypeDefinition as ICharType ?? throw new InvalidOperationException()).Kind;

        /// <summary>
        ///     get the boolean value
        /// </summary>
        public abstract char AsWideChar { get; }

        /// <summary>
        ///     get a char at
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public abstract IValue CharAt(int index);

        /// <summary>
        ///     convert this char to a string
        /// </summary>
        public string AsUnicodeString
            => new string(AsWideChar, 1);

        /// <summary>
        ///     length (in characters)
        /// </summary>
        public int NumberOfCharElements
            => 1;

        /// <summary>
        ///     ANSI char value
        /// </summary>
        public byte AsAnsiChar
            => unchecked((byte)AsWideChar);

        /// <summary>
        ///     test for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ICharValue other)
            => other.AsWideChar == AsWideChar;

        /// <summary>
        ///     get the ordinal value of a char
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public abstract IValue GetOrdinalValue(ITypeRegistry types);
    }
}