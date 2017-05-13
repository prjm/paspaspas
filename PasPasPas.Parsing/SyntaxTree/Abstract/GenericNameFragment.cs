using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     part of a generic ame
    /// </summary>
    public class GenericNameFragment : AbstractSyntaxPartBase, ITypeTarget {

        /// <summary>
        ///     symbol name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     symbol type
        /// </summary>
        public ISyntaxPartList<ITypeSpecification> TypeValues { get; }

        /// <summary>
        ///     create a new generic name fragment
        /// </summary>
        public GenericNameFragment()
            => TypeValues = new SyntaxPartCollection<ITypeSpecification>(this);

        /// <summary>
        ///     children
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (ITypeSpecification value in TypeValues)
                    yield return value;
            }
        }

        /// <summary>
        ///     type value
        /// </summary>
        public ITypeSpecification TypeValue {
            get => TypeValues.LastOrDefault();
            set => TypeValues.Add(value);
        }

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
