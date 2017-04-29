﻿using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     rrti control directive
    /// </summary>
    public class RttiControl : CompilerDirectiveBase {

        /// <summary>
        ///     fields visibility
        /// </summary>
        public RttiForVisibility Fields { get; set; }

        /// <summary>
        ///     methods visibilty
        /// </summary>
        public RttiForVisibility Methods { get; set; }

        /// <summary>
        ///     properties visibility
        /// </summary>
        public RttiForVisibility Properties { get; set; }


        /// <summary>
        ///     selected rtti mode
        /// </summary>
        public RttiGenerationMode Mode { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }


    }
}
