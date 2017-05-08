﻿using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     procedurual type
    /// </summary>
    public class ProcedureType : StandardSyntaxTreeBase {
        public ProcedureType(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        ///     procedure reference
        /// </summary>
        public ProcedureReference ProcedureReference { get; set; }

        /// <summary>
        ///     procedure type
        /// </summary>
        public ProcedureTypeDefinition ProcedureRefType { get; set; }

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