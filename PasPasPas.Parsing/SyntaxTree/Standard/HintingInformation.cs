﻿using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     additinal hinting information
    /// </summary>
    public class HintingInformation : StandardSyntaxTreeBase {
        public HintingInformation(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        ///     hint for deprecation
        /// </summary>
        public bool Deprecated { get; set; }

        /// <summary>
        ///     comment for deprecation
        /// </summary>
        public QuotedString DeprecatedComment { get; set; }

        /// <summary>
        ///     hint for experimental
        /// </summary>
        public bool Experimental { get; set; }

        /// <summary>
        ///     hint for library
        /// </summary>
        public bool Library { get; set; }

        /// <summary>
        ///     hint for platform
        /// </summary>
        public bool Platform { get; set; }

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