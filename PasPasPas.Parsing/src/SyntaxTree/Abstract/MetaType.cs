﻿#nullable disable
using PasPasPas.Globals.Parsing;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     special type
    /// </summary>
    public class MetaType : TypeSpecificationBase, IExpression, IExpressionTarget {

        /// <summary>
        ///     type kind
        /// </summary>
        public MetaTypeKind Kind { get; set; }
            = MetaTypeKind.Undefined;

        /// <summary>
        ///     subexpression
        /// </summary>
        /// <remarks>used for string length / string code page</remarks>
        public IExpression Value { get; set; }

        /// <summary>
        ///     creates a new meta type definition
        /// </summary>
        public MetaType()
            => Fragments = new SyntaxPartCollection<GenericNameFragment>();

        /// <summary>
        ///     name fragments
        /// </summary>
        public ISyntaxPartCollection<GenericNameFragment> Fragments { get; }

        /// <summary>
        ///     add a a fragment
        /// </summary>
        /// <param name="fragment"></param>
        public void AddFragment(GenericNameFragment fragment)
            => Fragments.Add(fragment);

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            if (Value != default)
                AcceptPart(this, Value, visitor);
            AcceptPart(this, Fragments, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     scoped referenced name
        /// </summary>
        /// <returns></returns>
        public ScopedName AsScopedName {
            get {
                var parts = new string[Fragments.Count];
                for (var i = 0; i < Fragments.Count; i++)
                    parts[i] = Fragments[i].Name;
                return new ScopedName(parts);
            }
        }

        /// <summary>
        ///     <c>true</c> if this reference is constant
        /// </summary>
        public bool IsConstant { get; set; }
    }
}
