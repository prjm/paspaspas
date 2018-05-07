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
        IValue Invalid { get; }

        /// <summary>
        ///     integer overflow
        /// </summary>
        IValue Overflow { get; }

        /// <summary>
        ///     zero value
        /// </summary>
        IValue Zero { get; }

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
        ///     cast an integral constant value to another type
        /// </summary>
        /// <param name="value">value to cast</param>
        /// <param name="typeId">target type id</param>
        /// <returns>casted value</returns>
        ITypeReference Cast(ITypeReference value, int typeId);

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
    }
}
