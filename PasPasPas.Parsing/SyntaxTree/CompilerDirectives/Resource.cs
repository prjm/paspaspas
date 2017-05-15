﻿using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     resource reference
    /// </summary>
    public class Resource : CompilerDirectiveBase {

        /// <summary>
        ///     file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///     resource file
        /// </summary>
        public string RcFile { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }
    }
}
