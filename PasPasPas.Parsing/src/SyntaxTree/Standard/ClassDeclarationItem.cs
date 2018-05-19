﻿using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class declaration item
    /// </summary>
    public class ClassDeclarationItem : StandardSyntaxTreeBase, IStructuredTypeMember {

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributes Attributes { get; set; }

        /// <summary>
        ///     class-wide declaration
        /// </summary>
        public bool ClassItem { get; set; }

        /// <summary>
        ///     constant class section
        /// </summary>
        public ConstSection ConstSection { get; set; }

        /// <summary>
        ///     field declaration
        /// </summary>
        public ClassField FieldDeclaration { get; set; }

        /// <summary>
        ///     method declaration
        /// </summary>
        public ClassMethod MethodDeclaration { get; set; }

        /// <summary>
        ///     method resolution
        /// </summary>
        public MethodResolution MethodResolution { get; set; }

        /// <summary>
        ///     property declaration
        /// </summary>
        public ClassProperty PropertyDeclaration { get; set; }

        /// <summary>
        ///     strict declaration
        /// </summary>
        public bool Strict { get; set; }

        /// <summary>
        ///     type section
        /// </summary>
        public TypeSection TypeSection { get; set; }

        /// <summary>
        ///     visibility declaration
        /// </summary>
        public int Visibility { get; set; }
            = TokenKind.Undefined;


        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}