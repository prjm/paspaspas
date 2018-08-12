﻿using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     guid declaration
    /// </summary>
    public class InterfaceGuid : StandardSyntaxTreeBase {

        /// <summary>
        ///     guid for this interface
        /// </summary>
        public QuotedString Id { get; set; }

        /// <summary>
        ///     named guid for this interface
        /// </summary>
        public IdentifierSymbol IdIdentifier { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}