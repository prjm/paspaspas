﻿using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     asm statement
    /// </summary>
    public class AsmStatement : StandardSyntaxTreeBase {

        /// <summary>
        ///     opcode
        /// </summary>
        public AsmOpCodeSymbol OpCode { get; set; }

        /// <summary>
        ///     lock / segment prefix
        /// </summary>
        public AsmPrefix Prefix { get; set; }

        /// <summary>
        ///     label
        /// </summary>
        public AsmLabelSymbol Label { get; set; }

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
