﻿#nullable disable
using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {


    /// <summary>
    ///     a list of expressions
    /// </summary>
    public class ExpressionList : VariableLengthSyntaxTreeBase<ExpressionSymbol> {

        /// <summary>
        ///     create a new expression list
        /// </summary>
        /// <param name="items"></param>
        public ExpressionList(ImmutableArray<ExpressionSymbol> items) : base(items) {
        }

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
        public override int Length => ItemLength;
    }
}