namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     integer calculator
    /// </summary>
    public interface IIntegerOperations :
        IArithmeticOperations, ILogicalOperations, IRelationalOperations,
        IIntegerConverter {

        /// <summary>
        ///     invalid integer
        /// </summary>
        ITypeReference Invalid { get; }

        /// <summary>
        ///     integer overflow
        /// </summary>
        ITypeReference Overflow { get; }

        /// <summary>
        ///     zero value
        /// </summary>
        ITypeReference Zero { get; }

        /// <summary>
        ///     integer division
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        ITypeReference Divide(ITypeReference dividend, ITypeReference divisor);

        /// <summary>
        ///     integer remainder
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        ITypeReference Modulo(ITypeReference dividend, ITypeReference divisor);

        /// <summary>
        ///     shift left
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        ITypeReference Shl(ITypeReference firstOperand, ITypeReference secondOperand);

        /// <summary>
        ///     shift right
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        ITypeReference Shr(ITypeReference firstOperand, ITypeReference secondOperand);

        /// <summary>
        ///     increment a value
        /// </summary>
        /// <param name="value">value to increment</param>
        /// <returns>incremented value</returns>
        ITypeReference Increment(ITypeReference value);

        /// <summary>
        ///     cast an integer value to another value
        /// </summary>
        /// <param name="value">integer to cast</param>
        /// <param name="typeId">target type</param>
        /// <returns>casted value or error</returns>
        ITypeReference Cast(ITypeReference value, int typeId);
    }
}
