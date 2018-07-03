using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class declaration
    /// </summary>
    public class ClassDeclarationSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     sealed class
        /// </summary>
        public bool Abstract { get; set; }

        /// <summary>
        ///     items of a class declaration
        /// </summary>
        public ClassDeclarationItemsSymbol ClassItems { get; set; }

        /// <summary>
        ///     parent class
        /// </summary>
        public ParentClass ClassParent { get; set; }

        /// <summary>
        ///     forward declaration
        /// </summary>
        public bool ForwardDeclaration { get; set; }

        /// <summary>
        ///     abstract class
        /// </summary>
        public bool Sealed { get; set; }


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