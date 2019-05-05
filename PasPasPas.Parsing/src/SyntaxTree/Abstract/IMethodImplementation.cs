﻿using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     common interface for method implementations
    /// </summary>
    public interface IMethodImplementation : ISyntaxPart {

        /// <summary>
        ///     anchor point
        /// </summary>
        SingleDeclaredSymbol Anchor { get; set; }

        /// <summary>
        ///     <c>true</c> if this a forward declaration
        /// </summary>
        bool IsForwardDeclaration { get; }

        /// <summary>
        ///     <c>true</c> if this is a exported method
        /// </summary>
        bool IsExportedMethod { get; }
    }
}