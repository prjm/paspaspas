﻿using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     single statement
    /// </summary>
    public class Statement : StandardSyntaxTreeBase {

        /// <summary>
        ///     label
        /// </summary>
        public SyntaxPartBase Label { get; set; }

        /// <summary>
        ///     statement part
        /// </summary>
        public SyntaxPartBase Part { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Label, visitor);
            AcceptPart(this, Part, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => Label.Length + ColonSymbol.Length + Part.Length;

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; set; }
    }
}