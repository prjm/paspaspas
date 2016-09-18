﻿namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     raise statemenmt
    /// </summary>
    public class RaiseStatement : SyntaxPartBase {

        /// <summary>
        ///     at part
        /// </summary>
        public DesignatorStatement At { get; set; }

        /// <summary>
        ///     raise part
        /// </summary>
        public DesignatorStatement Raise { get; set; }

    }
}