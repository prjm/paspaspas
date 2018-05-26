using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     simple field declaration
    /// </summary>
    public class ClassFieldSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     hints
        /// </summary>
        public ISyntaxPart Hint { get; set; }

        /// <summary>
        ///     names
        /// </summary>
        public IdentifierList Names { get; set; }

        /// <summary>
        ///     type declaration
        /// </summary>
        public TypeSpecification TypeDecl { get; set; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; set; }

        /// <summary>
        ///     colon
        /// </summary>
        public Terminal ColonSymbol { get; set; }


        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Names, visitor);
            AcceptPart(this, ColonSymbol, visitor);
            AcceptPart(this, TypeDecl, visitor);
            AcceptPart(this, Hint, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => Names.Length + ColonSymbol.Length + TypeDecl.Length + Hint.Length + Semicolon.Length;

    }
}