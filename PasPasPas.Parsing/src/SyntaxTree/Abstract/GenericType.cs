﻿using PasPasPas.Globals.Runtime;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     generic type
    /// </summary>
    public class GenericTypeNameCollection : SymbolTableBaseCollection<GenericConstraint>, ISymbolTableEntry, ITypedSyntaxNode {

        /// <summary>
        ///     type name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     symbol name
        /// </summary>
        public string SymbolName
            => Name?.CompleteName;

        /// <summary>
        ///     type inf
        /// </summary>
        public ITypeReference TypeInfo { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}