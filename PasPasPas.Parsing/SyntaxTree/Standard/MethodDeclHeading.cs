﻿using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     method declaration heading
    /// </summary>
    public class MethodDeclarationHeading : StandardSyntaxTreeBase {

        public MethodDeclarationHeading(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        ///     method kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     parameters
        /// </summary>
        public FormalParameterSection Parameters { get; set; }

        /// <summary>
        ///     result type
        /// </summary>
        public TypeSpecification ResultType { get; set; }

        /// <summary>
        ///     result type attributes
        /// </summary>
        public UserAttributes ResultTypeAttributes { get; set; }

        /// <summary>
        ///     method qualifier
        /// </summary>
        public IList<MethodDeclarationName> Qualifiers { get; }
            = new List<MethodDeclarationName>();

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