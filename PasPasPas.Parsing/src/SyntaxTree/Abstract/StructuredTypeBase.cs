﻿using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     structured type definition
    /// </summary>
    public abstract class StructuredTypeBase : TypeSpecificationBase, IDeclaredSymbolTarget, INamedTypeDeclaration {

        /// <summary>
        ///     packed type
        /// </summary>
        public bool PackedType { get; set; }

        /// <summary>
        ///     declared symbols
        /// </summary>
        public DeclaredSymbolCollection Symbols { get; }
            = new DeclaredSymbolCollection();

        /// <summary>
        ///     type name
        /// </summary>
        public string Name { get; set; }
            = string.Empty;
    }
}
