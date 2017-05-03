using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     property declaration
    /// </summary>
    public class ClassProperty : StandardSyntaxTreeBase {

        /// <summary>
        ///     property access index
        /// </summary>
        public FormalParameters ArrayIndex { get; set; }

        /// <summary>
        ///     default flag (for dispinterface)
        /// </summary>
        public bool IsDefault { get; internal set; }

        /// <summary>
        ///     index of the property
        /// </summary>
        public Expression PropertyIndex { get; set; }

        /// <summary>
        ///     property name
        /// </summary>
        public Identifier PropertyName { get; set; }

        /// <summary>
        ///     property type
        /// </summary>
        public TypeName TypeName { get; set; }

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