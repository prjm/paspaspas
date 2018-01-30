using System;
using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values.Int {


    /// <summary>
    ///     calculator helper for integers
    /// </summary>
    public class IntegerCalculator : IIntegerCalculator {

        private IValue invalidInteger
            = new SpecialValue(SpecialConstantKind.InvalidInteger);

        /// <summary>
        ///     calculate the sum of two integers
        /// </summary>
        /// <param name="augend">first operand</param>
        /// <param name="addend">second operand</param>
        /// <returns>sum</returns>
        public IValue Add(IValue augend, IValue addend) {
            if (augend is IntegerValueBase intAugend && addend is IntegerValueBase intAddend)
                return IntegerValueBase.AddAndScale(intAugend, intAddend);
            else
                return invalidInteger;
        }

        /// <summary>
        ///     calculate the bitwise and of two integers
        /// </summary>
        /// <param name="firstOperand">first operand</param>
        /// <param name="secondOperand">second operand</param>
        /// <returns>bitwise and</returns>
        public IValue And(IValue firstOperand, IValue secondOperand) {
            if (firstOperand is IntegerValueBase firstInt && secondOperand is IntegerValueBase secondInt)
                return IntegerValueBase.AndAndScale(firstInt, secondInt);
            else
                return invalidInteger;
        }

        /// <summary>
        ///     calculate an integer division
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public IValue Divide(IValue dividend, IValue divisor) {
            if (dividend is IntegerValueBase intDividend && divisor is IntegerValueBase intDivisor)
                if (intDivisor.SignedValue == 0)
                    return new SpecialValue(SpecialConstantKind.DivisionByZero);
                else
                    return IntegerValueBase.DivideAndScale(intDividend, intDivisor);
            else
                return invalidInteger;
        }

        /// <summary>
        ///     calculate an integer remainder
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public IValue Modulo(IValue dividend, IValue divisor) {
            if (dividend is IntegerValueBase intDividend && divisor is IntegerValueBase intDivisor)
                if (intDivisor.SignedValue == 0)
                    return new SpecialValue(SpecialConstantKind.DivisionByZero);
                else
                    return IntegerValueBase.ModuloAndScale(intDividend, intDivisor);
            else
                return invalidInteger;
        }

        /// <summary>
        ///     multiply two integer values
        /// </summary>
        /// <param name="multiplicand">multiplicand</param>
        /// <param name="intMultiplier">multiplier</param>
        /// <returns></returns>
        public IValue Multiply(IValue multiplicand, IValue intMultiplier) {
            if (multiplicand is IntegerValueBase intMultiplicand && intMultiplier is IntegerValueBase secondInt)
                return IntegerValueBase.MultiplyAndScale(intMultiplicand, secondInt);
            else
                return invalidInteger;
        }

        /// <summary>
        ///     negate a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IValue Negate(IValue number) {
            if (number is IntegerValueBase intNumber)
                return IntegerValueBase.Negate(intNumber);
            else
                return invalidInteger;
        }

        /// <summary>
        ///     invert a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IValue Not(IValue number) {
            if (number is IntegerValueBase intNumber)
                return IntegerValueBase.Not(intNumber);
            else
                return invalidInteger;
        }

        /// <summary>
        ///     compute a bitwise or
        /// </summary>
        /// <param name="firstOperand">first operand</param>
        /// <param name="secondOperand">second operand</param>
        /// <returns></returns>
        public IValue Or(IValue firstOperand, IValue secondOperand) {
            if (firstOperand is IntegerValueBase firstInt && secondOperand is IntegerValueBase secondInt)
                return IntegerValueBase.OrAndScale(firstInt, secondInt);
            else
                return invalidInteger;
        }

        /// <summary>
        ///     compute a left shift
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        public IValue Shl(IValue firstOperand, IValue secondOperand) {
            if (firstOperand is IntegerValueBase firstInt && secondOperand is IntegerValueBase secondInt)
                return IntegerValueBase.ShlAndScale(firstInt, secondInt);
            else
                return invalidInteger;
        }

        /// <summary>
        ///     compute a right shift
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        public IValue Shr(IValue firstOperand, IValue secondOperand) {
            if (firstOperand is IntegerValueBase firstInt && secondOperand is IntegerValueBase secondInt)
                return IntegerValueBase.ShrAndScale(firstInt, secondInt);
            else
                return invalidInteger;
        }

        /// <summary>
        ///     subtract two numbers
        /// </summary>
        /// <param name="minuend"></param>
        /// <param name="subtrahend"></param>
        /// <returns></returns>
        public IValue Subtract(IValue minuend, IValue subtrahend) {
            if (minuend is IntegerValueBase intMinuend && subtrahend is IntegerValueBase intSubtrahend)
                return IntegerValueBase.SubtractAndScale(intMinuend, intSubtrahend);
            else
                return invalidInteger;
        }

        /// <summary>
        ///     xor two integers
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        public IValue Xor(IValue firstOperand, IValue secondOperand) {
            if (firstOperand is IntegerValueBase firstInt && secondOperand is IntegerValueBase secondInt)
                return IntegerValueBase.XorAndScale(firstInt, secondInt);
            else
                return invalidInteger;
        }
    }
}
