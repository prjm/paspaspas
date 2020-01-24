using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     integer calculator
    /// </summary>
    public interface IIntegerOperations :
        IArithmeticOperations, ILogicalOperations, IRelationalOperations,
        IIntegerConverter {

        /// <summary>
        ///     invalid integer
        /// </summary>
        IOldTypeReference Invalid { get; }

        /// <summary>
        ///     integer overflow
        /// </summary>
        IOldTypeReference Overflow { get; }

        /// <summary>
        ///     zero value
        /// </summary>
        IValue Zero { get; }

        /// <summary>
        ///     one value
        /// </summary>
        IOldTypeReference One { get; }

        /// <summary>
        ///     integer division
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        IOldTypeReference Divide(IOldTypeReference dividend, IOldTypeReference divisor);

        /// <summary>
        ///     integer remainder
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        IOldTypeReference Modulo(IOldTypeReference dividend, IOldTypeReference divisor);

        /// <summary>
        ///     swap high and low bit
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="types">type registry</param>
        /// <returns></returns>
        IOldTypeReference Swap(IOldTypeReference parameter, ITypeRegistry types);

        /// <summary>
        ///     <c>hi</c> function: return the upper byte from a 16 bit value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        IOldTypeReference Hi(IOldTypeReference typeReference);

        /// <summary>
        ///     <c>chr</c> function: return a character for a given integer value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        IOldTypeReference Chr(IOldTypeReference typeReference);

        /// <summary>
        ///     <c>ki</c> function
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        IOldTypeReference Lo(IOldTypeReference typeReference);

        /// <summary>
        ///     shift left
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        IOldTypeReference Shl(IOldTypeReference firstOperand, IOldTypeReference secondOperand);

        /// <summary>
        ///     shift right
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        IOldTypeReference Shr(IOldTypeReference firstOperand, IOldTypeReference secondOperand);

        /// <summary>
        ///     increment a value
        /// </summary>
        /// <param name="value">value to increment</param>
        /// <returns>incremented value</returns>
        IValue Increment(IValue value);

        /// <summary>
        ///     absolute value of a value
        /// </summary>
        /// <param name="value">value to increment</param>
        /// <returns>incremented value</returns>
        IOldTypeReference Abs(IOldTypeReference value);

    }
}
