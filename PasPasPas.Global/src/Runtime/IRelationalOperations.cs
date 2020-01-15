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
        IOldTypeReference Equal(IOldTypeReference left, IOldTypeReference right);

        /// <summary>
        ///     <c>&lt;&gt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IOldTypeReference NotEquals(IOldTypeReference left, IOldTypeReference right);

        /// <summary>
        ///     <c>&lt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IOldTypeReference LessThen(IOldTypeReference left, IOldTypeReference right);

        /// <summary>
        ///     <c>&gt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IOldTypeReference GreaterThen(IOldTypeReference left, IOldTypeReference right);

        /// <summary>
        ///     <c>&lt;=</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IOldTypeReference LessThenOrEqual(IOldTypeReference left, IOldTypeReference right);

        /// <summary>
        ///     <c>&gt;=</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IOldTypeReference GreaterThenEqual(IOldTypeReference left, IOldTypeReference right);

    }
}