﻿#nullable disable
using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     list of namespace names
    /// </summary>
    public class NamespaceNameListSymbol : VariableLengthSyntaxTreeBase<NamespaceNameSymbol> {

        /// <summary>
        ///     create a new namespace name list
        /// </summary>
        /// <param name="items"></param>
        public NamespaceNameListSymbol(ImmutableArray<NamespaceNameSymbol> items) : base(items) { }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ItemLength;

    }
}