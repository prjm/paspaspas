﻿using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     namespace name with generuc suffix
    /// </summary>
    public class GenericNamespaceName : StandardSyntaxTreeBase {

        /// <summary>
        ///     generic part
        /// </summary>
        public GenericSuffix GenericPart { get; set; }

        /// <summary>
        ///     namespace name
        /// </summary>
        public NamespaceName Name { get; set; }

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
