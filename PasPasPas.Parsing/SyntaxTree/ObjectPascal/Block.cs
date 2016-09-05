﻿namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     file block
    /// </summary>
    public class Block : SyntaxPartBase {

        /// <summary>
        ///     block body
        /// </summary>
        public BlockBody Body { get; set; }

        /// <summary>
        ///     declarations
        /// </summary>
        public Declarations DeclarationSections { get; set; }

    }
}