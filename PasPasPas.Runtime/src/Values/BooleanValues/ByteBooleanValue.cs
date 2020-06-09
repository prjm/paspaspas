using System.Globalization;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.BooleanValues {

    /// <summary>
    ///     constant byte boolean (8 bit)
    /// </summary>
    internal class ByteBooleanValue : BooleanValueBase {

        /// <summary>
        ///     create a new byte boolean
        /// </summary>
        /// <param name="byteBoolValue">boolean value</param>
        /// <param name="typeDefinition">type def</param>
        internal ByteBooleanValue(byte byteBoolValue, ITypeDefinition typeDefinition) : base(typeDefinition, BooleanTypeKind.ByteBool)
            => Value = byteBoolValue;

        /// <summary>
        ///     boolean value
        /// </summary>
        public override bool AsBoolean
            => Value != 0;

        /// <summary>
        ///     boolean value
        /// </summary>
        public byte Value { get; }

        /// <summary>
        ///     get the value of this boolean
        /// </summary>
        public override uint AsUint
            => Value;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(IValue? other)
            => other is ByteBooleanValue b && b.Value == Value;

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
        ///     get the value string
        /// </summary>
        /// <returns></returns>
        public override string GetValueString()
            => Value.ToString(CultureInfo.InvariantCulture);
    }
}
