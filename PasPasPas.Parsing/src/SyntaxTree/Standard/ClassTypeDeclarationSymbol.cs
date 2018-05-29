using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class type declaration
    /// </summary>
    public class ClassTypeDeclarationSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     class declaration
        /// </summary>
        public SyntaxPartBase ClassDef { get; set; }

        /// <summary>
        ///     class helper
        /// </summary>
        public SyntaxPartBase ClassHelper { get; set; }

        /// <summary>
        ///     class of declaration
        /// </summary>
        public SyntaxPartBase ClassOf { get; set; }

        /// <summary>
        ///     interface definition
        /// </summary>
        public SyntaxPartBase InterfaceDef { get; set; }

        /// <summary>
        ///     object declaration
        /// </summary>
        public SyntaxPartBase ObjectDecl { get; set; }

        /// <summary>
        ///     record declaration
        /// </summary>
        public SyntaxPartBase RecordDecl { get; set; }

        /// <summary>
        ///     record helper
        /// </summary>
        public SyntaxPartBase RecordHelper { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, ClassOf, visitor);
            AcceptPart(this, ClassHelper, visitor);
            AcceptPart(this, ClassDef, visitor);
            AcceptPart(this, InterfaceDef, visitor);
            AcceptPart(this, ObjectDecl, visitor);
            AcceptPart(this, RecordDecl, visitor);
            AcceptPart(this, RecordHelper, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => ClassOf.Length +
                ClassHelper.Length +
                ClassDef.Length +
                InterfaceDef.Length +
                ObjectDecl.Length +
                RecordDecl.Length +
                RecordHelper.Length;

    }
}