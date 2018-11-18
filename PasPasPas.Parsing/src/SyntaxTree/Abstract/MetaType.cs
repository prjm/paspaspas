using System.Collections.Generic;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     special type
    /// </summary>
    public class MetaType : TypeSpecificationBase, IExpressionTarget {

        /// <summary>
        ///     type kind
        /// </summary>
        public MetaTypeKind Kind { get; set; }
            = MetaTypeKind.Undefined;

        /// <summary>
        ///     subexpression
        /// </summary>
        /// <remarks>used for string length / string codepage</remarks>
        public IExpression Value { get; set; }

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (Value != null)
                    yield return Value;
                foreach (var fragment in Fragments)
                    yield return fragment;
            }
        }

        /// <summary>
        ///     creates a new meta type definition
        /// </summary>
        public MetaType()
            => Fragments = new SyntaxPartCollection<GenericNameFragment>(this);

        /// <summary>
        ///     name fragments
        /// </summary>
        public ISyntaxPartList<GenericNameFragment> Fragments { get; }

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
            AcceptParts(this, visitor);
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
