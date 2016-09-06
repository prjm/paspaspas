﻿namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     property declaration
    /// </summary>
    public class ClassProperty : SyntaxPartBase {

        /// <summary>
        ///     property access index
        /// </summary>
        public FormalParameters ArrayIndex { get; set; }

        /// <summary>
        ///     index of the property
        /// </summary>
        public Expression PropertyIndex { get; set; }

        /// <summary>
        ///     property name
        /// </summary>
        public PascalIdentifier PropertyName { get; set; }

        /// <summary>
        ///     property type
        /// </summary>
        public NamespaceName TypeName { get; set; }

    }
}