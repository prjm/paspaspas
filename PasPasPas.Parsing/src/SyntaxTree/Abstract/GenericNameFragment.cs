using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     part of a generic name
    /// </summary>
    public class GenericNameFragment : AbstractSyntaxPartBase, ITypeTarget {

        /// <summary>
        ///     symbol name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     symbol type
        /// </summary>
        public ISyntaxPartCollection<ITypeSpecification> TypeValues { get; }

        /// <summary>
        ///     create a new generic name fragment
        /// </summary>
        public GenericNameFragment()
            => TypeValues = new SyntaxPartCollection<ITypeSpecification>();

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
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, TypeValues, visitor);
            visitor.EndVisit(this);
        }
    }
}
