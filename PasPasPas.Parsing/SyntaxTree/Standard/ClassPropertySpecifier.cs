﻿using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class property specifier
    /// </summary>
    public class ClassPropertySpecifier : StandardSyntaxTreeBase {
        public ClassPropertySpecifier(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        ///     default property
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        ///     default property expression
        /// </summary>
        public Expression DefaultProperty { get; set; }

        /// <summary>
        ///     dispinterface
        /// </summary>
        public ClassPropertyDispInterface PropertyDispInterface { get; set; }

        /// <summary>
        ///     read write accessor
        /// </summary>
        public ClassPropertyReadWrite PropertyReadWrite { get; set; }

        /// <summary>
        ///     stored property
        /// </summary>
        public bool IsStored { get; set; }

        /// <summary>
        ///     stored property expression
        /// </summary>
        public Expression StoredProperty { get; set; }

        /// <summary>
        ///     no default
        /// </summary>
        public bool NoDefault { get; set; }

        /// <summary>
        ///     implementing type name
        /// </summary>
        public NamespaceName ImplementsTypeId { get; set; }

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