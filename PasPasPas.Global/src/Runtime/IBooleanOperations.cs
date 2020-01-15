﻿namespace PasPasPas.Globals.Runtime {

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
        IOldTypeReference Invalid { get; }

        /// <summary>
        ///     convert a boolean constant value to a value object
        /// </summary>
        /// <param name="value">boolean value</param>
        /// <param name="typeId">type id</param>
        /// <returns><c>TrueValue</c> or <c>FalseValue</c></returns>
        IBooleanValue ToBoolean(bool value, int typeId);

        /// <summary>
        ///     convert a boolean constant value to a value object
        /// </summary>
        /// <param name="bitSize"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IOldTypeReference ToBoolean(uint bitSize, uint value);

        /// <summary>
        ///     convert a byte constant value to a byte bool object
        /// </summary>
        /// <param name="value">byte value</param>
        /// <param name="typeId">type Id</param>
        IBooleanValue ToByteBool(byte value, int typeId);

        /// <summary>
        ///     convert a word constant value to a word bool object
        /// </summary>
        /// <param name="value">word bool value</param>
        /// <param name="typeId">type id</param>
        IBooleanValue ToWordBool(ushort value, int typeId);

        /// <summary>
        ///     convert a int constant value to a long bool object
        /// </summary>
        /// <param name="value">long bool value</param>
        /// <param name="typeId">type id</param>
        /// <returns>long bool value</returns>
        IBooleanValue ToLongBool(uint value, int typeId);

    }
}
