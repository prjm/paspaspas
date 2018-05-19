﻿using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     interface definition
    /// </summary>
    public class InterfaceDefinition : StandardSyntaxTreeBase {

        /// <summary>
        ///     <c>true</c> if dispinterface
        /// </summary>
        public bool DisplayInterface { get; set; }

        /// <summary>
        ///     guid declaration
        /// </summary>
        public InterfaceGuid Guid { get; set; }

        /// <summary>
        ///     interface items
        /// </summary>
        public InterfaceItems Items { get; set; }

        /// <summary>
        ///     parent interface
        /// </summary>
        public ParentClass ParentInterface { get; set; }

        /// <summary>
        ///     <c>true</c> for forward declarations
        /// </summary>
        public bool ForwardDeclaration { get; set; }

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