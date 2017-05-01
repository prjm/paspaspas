using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     type alias
    /// </summary>
    public class TypeAlias : TypeSpecificationBase {

        private IList<GenericNameFragment> fragments
                = new List<GenericNameFragment>();

        /// <summary>
        ///     name fragements
        /// </summary>
        public IList<GenericNameFragment> Fragments
            => fragments;

        /// <summary>
        ///     add a afragment
        /// </summary>
        /// <param name="fragment"></param>
        public void AddFragment(GenericNameFragment fragment) {
            fragments.Add(fragment);
        }

        /// <summary>
        ///     <c>true</c> if the aliased type is considered as new type
        /// </summary>
        public bool IsNewType { get; set; }

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (GenericNameFragment fragment in fragments)
                    yield return fragment;
            }
        }

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
