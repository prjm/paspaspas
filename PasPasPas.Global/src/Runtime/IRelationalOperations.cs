#nullable disable
namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     provides relations operators
    /// </summary>
    public interface IRelationalOperations {

        /// <summary>
        ///     used boolean operations
        /// </summary>
        IBooleanOperations Booleans { get; }

        /// <summary>
        ///     <c>==</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue Equal(IValue left, IValue right);

        /// <summary>
        ///     <c>&lt;&gt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue NotEquals(IValue left, IValue right);

        /// <summary>
        ///     <c>&lt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue LessThen(IValue left, IValue right);

        /// <summary>
        ///     <c>&gt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue GreaterThen(IValue left, IValue right);

        /// <summary>
        ///     <c>&lt;=</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue LessThenOrEqual(IValue left, IValue right);

        /// <summary>
        ///     <c>&gt;=</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue GreaterThenEqual(IValue left, IValue right);

    }
}