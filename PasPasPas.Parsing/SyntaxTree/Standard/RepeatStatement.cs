﻿using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     repeat statement
    /// </summary>
    public class RepeatStatement : StandardSyntaxTreeBase {
        public RepeatStatement(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        ///     condition
        /// </summary>
        public Expression Condition { get; set; }

        /// <summary>
        ///     statement list
        /// </summary>
        public StatementList Statements { get; set; }

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