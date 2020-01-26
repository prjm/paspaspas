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
        ///     one value
        /// </summary>
        IValue One { get; }

        /// <summary>
        ///     integer division
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        IValue Divide(IValue dividend, IValue divisor);

        /// <summary>
        ///     integer remainder
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        IValue Modulo(IValue dividend, IValue divisor);

        /// <summary>
        ///     swap high and low bit
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="types">type registry</param>
        /// <returns></returns>
        IValue Swap(IValue parameter, ITypeRegistry types);

        /// <summary>
        ///     <c>hi</c> function: return the upper byte from a 16 bit value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        IValue Hi(IValue typeReference);

        /// <summary>
        ///     <c>chr</c> function: return a character for a given integer value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        IValue Chr(IValue typeReference);

        /// <summary>
        ///     <c>ki</c> function
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        IValue Lo(IValue typeReference);

        /// <summary>
        ///     shift left
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        IValue Shl(IValue firstOperand, IValue secondOperand);

        /// <summary>
        ///     shift right
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        IValue Shr(IValue firstOperand, IValue secondOperand);

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
        IValue Abs(IValue value);

    }
}
