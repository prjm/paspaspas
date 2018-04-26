using SharpFloat.FloatingPoint;

namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     calculator for extended value
    /// </summary>
    public interface IRealNumberOperations :
        IArithmeticOperations, IRelationalOperations {

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
        ITypeReference Divide(ITypeReference dividend, ITypeReference divisor);


        /// <summary>
        ///     convert a double to the appropriate runtime constant
        /// </summary>
        /// <param name="value">parsed value</param>
        /// <returns>constant value</returns>
        IValue ToExtendedValue(in ExtF80 value);


    }
}
