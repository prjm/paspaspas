﻿namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     unit block
    /// </summary>
    public class UnitBlock : SyntaxPartBase {

        /// <summary>
        ///     initializarion
        /// </summary>
        public UnitInitialization Initialization { get; set; }

        /// <summary>
        ///     main block
        /// </summary>
        public CompoundStatement MainBlock { get; set; }

    }
}