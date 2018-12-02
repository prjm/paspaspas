namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     calculator for string values
    /// </summary>
    public interface IStringOperations : IRelationalOperations {

        /// <summary>
        ///     concatenate two strings
        /// </summary>
        /// <param name="value1">first string</param>
        /// <param name="value2">second string</param>
        /// <returns>concatenated string</returns>
        ITypeReference Concat(ITypeReference value1, ITypeReference value2);

        /// <summary>
        ///     convert a string to a runtime value object
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        ITypeReference ToUnicodeString(string text);

        /// <summary>
        ///     get an empty string
        /// </summary>
        /// <returns></returns>
        ITypeReference GetEmptyString();
    }
}

