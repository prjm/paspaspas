﻿namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     special type
    /// </summary>
    public class MetaType : TypeSpecificationBase {

        /// <summary>
        ///     type kind
        /// </summary>
        public MetaTypeKind Kind { get; set; }
            = MetaTypeKind.Undefined;

        /// <summary>
        ///     type name
        /// </summary>
        public SymbolName Name { get; set; }


    }
}