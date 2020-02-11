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
        IValue Concat(IValue value1, IValue value2);

        /// <summary>
        ///     convert a string to a runtime value object
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        IValue ToUnicodeString(string text);

        /// <summary>
        ///     convert a string to a runtime value object
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        IValue ToAnsiString(string text);

        /// <summary>
        ///     convert a string to a runtime value object
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        IValue ToShortString(string text);

        /// <summary>
        ///     get an empty string
        /// </summary>
        /// <returns></returns>
        IValue EmptyString { get; }

        /// <summary>
        ///     invalid string
        /// </summary>
        IValue Invalid { get; }

        /// <summary>
        ///     convert string value to a wide string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        IValue ToWideString(string text);
    }
}

