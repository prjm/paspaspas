#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     file type
    /// </summary>
    public class FileTypeSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new file type
        /// </summary>
        /// <param name="fileSymbol"></param>
        /// <param name="ofSymbol"></param>
        /// <param name="typeDefinition"></param>
        public FileTypeSymbol(Terminal fileSymbol, Terminal ofSymbol, TypeSpecificationSymbol typeDefinition) {
            FileSymbol = fileSymbol;
            OfSymbol = ofSymbol;
            TypeDefinition = typeDefinition;
        }

        /// <summary>
        ///     optional subtype
        /// </summary>
        public TypeSpecificationSymbol TypeDefinition { get; }

        /// <summary>
        ///     file symbol
        /// </summary>
        public Terminal FileSymbol { get; }

        /// <summary>
        ///     of symbol
        /// </summary>
        public Terminal OfSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, FileSymbol, visitor);
            AcceptPart(this, OfSymbol, visitor);
            AcceptPart(this, TypeDefinition, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => FileSymbol.GetSymbolLength() +
                OfSymbol.GetSymbolLength() +
                TypeDefinition.GetSymbolLength();
    }
}