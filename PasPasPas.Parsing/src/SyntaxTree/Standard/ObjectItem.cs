﻿using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     object item
    /// </summary>
    public class ObjectItem : StandardSyntaxTreeBase, IStructuredTypeMember {

        /// <summary>
        ///     field declaration
        /// </summary>
        public ClassFieldSymbol FieldDeclaration { get; set; }

        /// <summary>
        ///     method declaration
        /// </summary>
        public ClassMethodSymbol MethodDeclaration { get; set; }

        /// <summary>
        ///     strict modifier
        /// </summary>
        public bool Strict { get; set; }

        /// <summary>
        ///     visibility
        /// </summary>
        public int Visibility { get; set; }
            = TokenKind.Undefined;

        /// <summary>
        ///     class item
        /// </summary>
        public bool ClassItem { get; set; }

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributes Attributes1 { get; set; }

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributes Attributes2 { get; set; }

        /// <summary>
        ///     properties
        /// </summary>
        public ClassPropertySymbol Property { get; set; }

        /// <summary>
        ///     type section
        /// </summary>
        public TypeSection TypeSection { get; set; }

        /// <summary>
        ///     const section
        /// </summary>
        public ConstSection ConstSection { get; set; }

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
