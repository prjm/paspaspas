using SharpFloat.FloatingPoint;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     calculator for extended value
    /// </summary>
    public interface IRealNumberOperations :
        IArithmeticOperations, IRelationalOperations {

        /// <summary>
        ///     rounding mode
        /// </summary>
        RealNumberRoundingMode RoundingMode { get; set; }

        /// <summary>
        ///     invalid real number
        /// </summary>
        IValue Invalid { get; }

        /// <summary>
        ///     floating point division
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        IValue Divide(IValue dividend, IValue divisor);

        /// <summary>
        ///     convert a double to the appropriate runtime constant
        /// </summary>
        /// <param name="value">parsed value</param>
        /// <param name="typeId">type id</param>
        /// <returns>constant value</returns>
        IValue ToExtendedValue(int typeId, in ExtF80 value);

        /// <summary>
        ///     absolute value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        IValue Abs(IValue typeReference);

        /// <summary>
        ///     round a real number
        /// </summary>
        /// <param name="realValue"></param>
        /// <returns></returns>
        IValue Round(IValue realValue);

        /// <summary>
        ///     truncate a real number
        /// </summary>
        /// <param name="realNumberValue"></param>
        /// <returns></returns>
        IValue Trunc(IRealNumberValue realNumberValue);
    }
}
