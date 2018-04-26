namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     provides relations operators
    /// </summary>
    public interface IRelationalOperations {

        /// <summary>
        ///     <c>==</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        ITypeReference Equal(ITypeReference left, ITypeReference right);

        /// <summary>
        ///     <c>&lt;&gt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        ITypeReference NotEquals(ITypeReference left, ITypeReference right);

        /// <summary>
        ///     <c>&lt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        ITypeReference LessThen(ITypeReference left, ITypeReference right);

        /// <summary>
        ///     <c>&gt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        ITypeReference GreaterThen(ITypeReference left, ITypeReference right);

        /// <summary>
        ///     <c>&lt;=</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        ITypeReference LessThenOrEqual(ITypeReference left, ITypeReference right);

        /// <summary>
        ///     <c>&gt;=</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        ITypeReference GreaterThenEqual(ITypeReference left, ITypeReference right);

    }
}