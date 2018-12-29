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
        ITypeReference Invalid { get; }

        /// <summary>
        ///     floating point division
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        ITypeReference Divide(ITypeReference dividend, ITypeReference divisor);

        /// <summary>
        ///     convert a double to the appropriate runtime constant
        /// </summary>
        /// <param name="value">parsed value</param>
        /// <returns>constant value</returns>
        ITypeReference ToExtendedValue(in ExtF80 value);

        /// <summary>
        ///     absolute value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        ITypeReference Abs(ITypeReference typeReference);

        /// <summary>
        ///     round a real number
        /// </summary>
        /// <param name="realValue"></param>
        /// <returns></returns>
        ITypeReference Round(ITypeReference realValue);

        /// <summary>
        ///     truncate a real number
        /// </summary>
        /// <param name="realNumberValue"></param>
        /// <returns></returns>
        ITypeReference Trunc(IRealNumberValue realNumberValue);
    }
}
