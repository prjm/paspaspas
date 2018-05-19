﻿using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     fields
    /// </summary>
    public class StructureFields : AbstractSyntaxPartBase, ITypeTarget {

        /// <summary>
        ///     list of fields
        /// </summary>
        public ISyntaxPartList<StructureField> Fields { get; }

        /// <summary>
        ///     create a new set of fields of a structured type
        /// </summary>
        public StructureFields()
            => Fields = new SyntaxPartCollection<StructureField>(this);

        /// <summary>
        ///     enumerate all parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (var field in Fields)
                    yield return field;
                if (TypeValue != null)
                    yield return TypeValue;
            }
        }

        /// <summary>
        ///     field type
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     visibility
        /// </summary>
        public MemberVisibility Visibility { get; set; }
            = MemberVisibility.Public;

        /// <summary>
        ///     hints
        /// </summary>
        public SymbolHints Hints { get; set; }

        /// <summary>
        ///     <c>true</c> for class variables
        /// </summary>
        public bool ClassItem { get; set; }

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