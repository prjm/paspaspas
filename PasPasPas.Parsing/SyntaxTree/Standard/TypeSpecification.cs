﻿using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     a type specification
    /// </summary>
    public class TypeSpecification : StandardSyntaxTreeBase {
        public TypeSpecification(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        ///     pointer type
        /// </summary>
        public PointerType PointerType { get; set; }

        /// <summary>
        ///     procedure type
        /// </summary>
        public ProcedureType ProcedureType { get; set; }

        /// <summary>
        ///     simple type
        /// </summary>
        public SimpleType SimpleType { get; set; }

        /// <summary>
        ///     string type
        /// </summary>
        public StringType StringType { get; set; }

        /// <summary>
        ///     structured type
        /// </summary>
        public StructType StructuredType { get; set; }

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