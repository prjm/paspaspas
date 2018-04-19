﻿namespace PasPasPas.Global.Runtime {

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

    }
}

