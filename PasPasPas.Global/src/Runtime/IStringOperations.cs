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
        IOldTypeReference Concat(IOldTypeReference value1, IOldTypeReference value2);

        /// <summary>
        ///     convert a string to a runtime value object
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        IOldTypeReference ToUnicodeString(string text);

        /// <summary>
        ///     convert a string to a runtime value object
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        IOldTypeReference ToAnsiString(string text);

        /// <summary>
        ///     convert a string to a runtime value object
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        IOldTypeReference ToShortString(string text);

        /// <summary>
        ///     get an empty string
        /// </summary>
        /// <returns></returns>
        IOldTypeReference EmptyString { get; }

    }
}

