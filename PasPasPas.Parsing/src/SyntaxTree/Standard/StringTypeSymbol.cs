#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     string type
    /// </summary>
    public class StringTypeSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new string type
        /// </summary>
        /// <param name="stringType"></param>
        public StringTypeSymbol(Terminal stringType)
            => StringSymbol = stringType;

        /// <summary>
        ///     create a new string type
        /// </summary>
        /// <param name="stringSymbol"></param>
        /// <param name="openParen"></param>
        /// <param name="codePage"></param>
        /// <param name="closeParen"></param>
        public StringTypeSymbol(Terminal stringSymbol, Terminal openParen, ConstantExpressionSymbol codePage, Terminal closeParen) {
            StringSymbol = stringSymbol;
            OpenParen = openParen;
            StringLength = codePage;
            CloseParen = closeParen;
        }

        /// <summary>
        ///     code page
        /// </summary>
        public ConstantExpressionSymbol StringLength { get; }

        /// <summary>
        ///     kind of the string
        /// </summary>
        public int Kind
            => StringSymbol.GetSymbolKind();

        /// <summary>
        ///     string type
        /// </summary>
        public Terminal StringSymbol { get; }

        /// <summary>
        ///     open paren
        /// </summary>
        public Terminal OpenParen { get; set; }

        /// <summary>
        ///     close paren
        /// </summary>
        public Terminal CloseParen { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, StringSymbol, visitor);
            AcceptPart(this, OpenParen, visitor);
            AcceptPart(this, StringLength, visitor);
            AcceptPart(this, CloseParen, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => StringSymbol.GetSymbolLength() +
               OpenParen.GetSymbolLength() +
               StringLength.GetSymbolLength() +
               CloseParen.GetSymbolLength();



    }
}