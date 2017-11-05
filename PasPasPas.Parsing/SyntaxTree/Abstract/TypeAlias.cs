using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     type alias
    /// </summary>
    public class TypeAlias : TypeSpecificationBase {

        /// <summary>
        ///     name fragements
        /// </summary>
        public ISyntaxPartList<GenericNameFragment> Fragments { get; }

        public TypeAlias()
            => Fragments = new SyntaxPartCollection<GenericNameFragment>(this);

        /// <summary>
        ///     add a afragment
        /// </summary>
        /// <param name="fragment"></param>
        public void AddFragment(GenericNameFragment fragment)
            => Fragments.Add(fragment);

        /// <summary>
        ///     <c>true</c> if the aliased type is considered as new type
        /// </summary>
        public bool IsNewType { get; set; }

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (var fragment in Fragments)
                    yield return fragment;
            }
        }

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
