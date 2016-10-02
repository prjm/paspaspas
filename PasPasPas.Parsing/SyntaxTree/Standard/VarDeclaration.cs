﻿namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     variable declaration
    /// </summary>
    public class VarDeclaration : SyntaxPartBase {

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributes Attributes { get; set; }

        /// <summary>
        ///     hints
        /// </summary>
        public HintingInformationList Hints { get; set; }

        /// <summary>
        ///     var names
        /// </summary>
        public IdentifierList Identifiers { get; set; }

        /// <summary>
        ///     var types
        /// </summary>
        public TypeSpecification TypeDeclaration { get; set; }

        /// <summary>
        ///     var values
        /// </summary>
        public VarValueSpecification ValueSpecification { get; set; }

    }
}