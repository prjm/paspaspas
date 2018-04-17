﻿using PasPasPas.Global.Runtime;

namespace PasPasPas.Parsing.Tokenizer.LiteralValues {

    /// <summary>
    ///     interface to convert real values
    /// </summary>
    public interface IRealConverter {

        /// <summary>
        ///     convert integer literals to one real literal
        /// </summary>
        /// <param name="value">real value</param>
        /// <returns>real literal value</returns>
        IValue Convert(string value);

    }
}
