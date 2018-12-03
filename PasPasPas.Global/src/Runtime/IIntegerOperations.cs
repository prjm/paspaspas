﻿namespace PasPasPas.Globals.Runtime {

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
        ///     one value
        /// </summary>
        ITypeReference One { get; }

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
        ///     <c>hi</c> function: return the upper byte from a 16 bit value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        ITypeReference Hi(ITypeReference typeReference);

        /// <summary>
        ///     <c>chr</c> function: return a character for a given integer value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        ITypeReference Chr(ITypeReference typeReference);

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
        ///     absolute value of a value
        /// </summary>
        /// <param name="value">value to increment</param>
        /// <returns>incremented value</returns>
        ITypeReference Abs(ITypeReference value);

    }
}
