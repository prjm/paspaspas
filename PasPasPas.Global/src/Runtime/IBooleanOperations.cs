#nullable disable
using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     provides logical operations and relational operations for boolean
    ///     values
    /// </summary>
    public interface IBooleanOperations : ILogicalOperations, IRelationalOperations {

        /// <summary>
        ///     value of <c>true</c>
        /// </summary>
        IBooleanValue TrueValue { get; }

        /// <summary>
        ///     value of <c>false</c>
        /// </summary>
        IBooleanValue FalseValue { get; }

        /// <summary>
        ///     invalid <c>boolean</c> value
        /// </summary>
        IValue Invalid { get; }

        /// <summary>
        ///     convert a boolean constant value to a value object
        /// </summary>
        /// <param name="value">boolean value</param>
        /// <param name="typeDef">type def</param>
        /// <returns><c>TrueValue</c> or <c>FalseValue</c></returns>
        IBooleanValue ToBoolean(bool value, ITypeDefinition typeDef);

        /// <summary>
        ///     convert a boolean constant value to an value object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IBooleanValue ToBoolean(bool value);

        /// <summary>
        ///     convert a byte value to a boolean value object
        /// </summary>
        /// <param name="unsignedValue"></param>
        /// <param name="typeDef"></param>
        /// <returns></returns>
        IBooleanValue ToByteBool(byte unsignedValue, ITypeDefinition typeDef);

        /// <summary>
        ///     convert a short value to a boolean value object
        /// </summary>
        /// <param name="unsignedValue"></param>
        /// <param name="typeDef"></param>
        /// <returns></returns>
        IBooleanValue ToWordBool(ushort unsignedValue, ITypeDefinition typeDef);

        /// <summary>
        ///     convert a integer value to a boolean value object
        /// </summary>
        /// <param name="unsignedValue"></param>
        /// <param name="typeDef"></param>
        /// <returns></returns>
        IBooleanValue ToLongBool(uint unsignedValue, ITypeDefinition typeDef);

        /// <summary>
        ///     convert a integer boolean value object to a boolean
        /// </summary>
        /// <param name="typeDef"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IBooleanValue ToBoolean(IBooleanType typeDef, uint value);
    }
}
