﻿using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     set section part
    /// </summary>
    public class SetSectnPart : StandardSyntaxTreeBase {

        public SetSectnPart(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        ///     continuation
        /// </summary>
        public int Continuation { get; set; }

        /// <summary>
        ///     set expression
        /// </summary>
        public Expression SetExpression { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(this, startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }


    }
}