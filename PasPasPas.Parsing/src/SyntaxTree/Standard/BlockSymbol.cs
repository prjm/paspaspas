using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     file block
    /// </summary>
    public class BlockSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new block symbol
        /// </summary>
        /// <param name="body"></param>
        /// <param name="declarationSections"></param>
        public BlockSymbol(BlockBodySymbol body, Declarations declarationSections) {
            Body = body;
            DeclarationSections = declarationSections;
        }

        /// <summary>
        ///     block body
        /// </summary>
        public BlockBodySymbol Body { get; }

        /// <summary>
        ///     declarations
        /// </summary>
        public Declarations DeclarationSections { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, DeclarationSections, visitor);
            AcceptPart(this, Body, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => DeclarationSections.GetSymbolLength() +
               Body.GetSymbolLength();

    }
}