﻿namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     value of an enumeration
    /// </summary>
    public interface IEnumeratedValue : IValue {

        /// <summary>
        ///     constant value
        /// </summary>
        IIntegerValue Value { get; }

        /// <summary>
        ///     enumerated value name
        /// </summary>
        string Name { get; }
    }
}
