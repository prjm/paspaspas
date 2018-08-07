using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     string type
    /// </summary>
    public class StringType : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new string type
        /// </summary>
        /// <param name="stringType"></param>
        public StringType(Terminal stringType)
            => StringSymbol = stringType;

        /// <summary>
        ///     create a new string type
        /// </summary>
        /// <param name="stringSymbol"></param>
        /// <param name="openParen"></param>
        /// <param name="codePage"></param>
        /// <param name="closeParen"></param>
        public StringType(Terminal stringSymbol, Terminal openParen, ConstantExpressionSymbol codePage, Terminal closeParen) {
            StringSymbol = stringSymbol;
            OpenParen = openParen;
            CodePage = codePage;
            CloseParen = closeParen;
        }

        /// <summary>
        ///     code page
        /// </summary>
        public ConstantExpressionSymbol CodePage { get; }

        /// <summary>
        ///     kind of the string
        /// </summary>
        public int Kind
            => StringSymbol.GetSymbolKind();

        /// <summary>
        ///     string length
        /// </summary>
        public ExpressionSymbol StringLength { get; }

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
            AcceptPart(OpenParen, StringSymbol, visitor);
            AcceptPart(CodePage, StringSymbol, visitor);
            AcceptPart(CloseParen, StringSymbol, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => StringSymbol.GetSymbolLength() +
               OpenParen.GetSymbolLength() +
               CodePage.GetSymbolLength() +
               CloseParen.GetSymbolLength();



    }
}