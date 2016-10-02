﻿namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     constant declaration
    /// </summary>
    public class ConstDeclaration : SyntaxPartBase {

        /// <summary>
        ///     user defined attributes
        /// </summary>
        public UserAttributes Attributes { get; set; }

        /// <summary>
        ///     additional hint for that constant
        /// </summary>
        public HintingInformationList Hint { get; set; }

        /// <summary>
        ///     identifier
        /// </summary>
        public Identifier Identifier { get; set; }

        /// <summary>
        ///     type specification
        /// </summary>
        public TypeSpecification TypeSpecification { get; set; }

        /// <summary>
        ///     expression
        /// </summary>
        public ConstantExpression Value { get; set; }


    }
}