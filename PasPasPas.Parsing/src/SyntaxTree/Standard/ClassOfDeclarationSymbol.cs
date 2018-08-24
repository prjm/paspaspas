using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class of declaration
    /// </summary>
    public class ClassOfDeclarationSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new class of symbol
        /// </summary>
        /// <param name="classSymbol"></param>
        /// <param name="ofSymbol"></param>
        /// <param name="typeName"></param>
        public ClassOfDeclarationSymbol(Terminal classSymbol, Terminal ofSymbol, TypeNameSymbol typeName) {
            ClassSymbol = classSymbol;
            OfSymbol = ofSymbol;
            TypeRef = typeName;
        }
        /// <summary>
        ///     type name
        /// </summary>
        public TypeNameSymbol TypeRef { get; }

        /// <summary>
        ///     class symbol
        /// </summary>
        public Terminal ClassSymbol { get; }

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
            AcceptPart(this, ClassSymbol, visitor);
            AcceptPart(this, OfSymbol, visitor);
            AcceptPart(this, TypeRef, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ClassSymbol.GetSymbolLength() +
                OfSymbol.GetSymbolLength() +
                TypeRef.GetSymbolLength();

    }
}