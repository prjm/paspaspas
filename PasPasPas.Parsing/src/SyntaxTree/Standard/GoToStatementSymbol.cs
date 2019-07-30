using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     goto statement
    /// </summary>
    public class GoToStatementSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new goto statement
        /// </summary>
        /// <param name="gotoSymbol"></param>
        public GoToStatementSymbol(Terminal gotoSymbol)
            => GotoSymbol = gotoSymbol;

        /// <summary>
        ///     create a new goto symbol
        /// </summary>
        /// <param name="gotoSymbol"></param>
        /// <param name="label"></param>
        public GoToStatementSymbol(Terminal gotoSymbol, LabelSymbol label) {
            GotoSymbol = gotoSymbol;
            GoToLabel = label;
        }

        /// <summary>
        ///     create a new goto symbol
        /// </summary>
        /// <param name="gotoSymbol"></param>
        /// <param name="openParen"></param>
        /// <param name="exitExpression"></param>
        /// <param name="closeParen"></param>
        public GoToStatementSymbol(Terminal gotoSymbol, Terminal openParen, ExpressionSymbol exitExpression, Terminal closeParen) {
            GotoSymbol = gotoSymbol;
            OpenParen = openParen;
            ExitExpression = exitExpression;
            CloseParen = closeParen;
        }

        /// <summary>
        ///     break statement
        /// </summary>
        public bool Break
            => GotoSymbol.GetSymbolKind() == TokenKind.Break;


        /// <summary>
        ///     continue statement
        /// </summary>
        public bool Continue
            => GotoSymbol.GetSymbolKind() == TokenKind.Continue;

        /// <summary>
        ///     exit statement
        /// </summary>
        public bool Exit
            => GotoSymbol.GetSymbolKind() == TokenKind.Exit;

        /// <summary>
        ///     exit expression
        /// </summary>
        public ExpressionSymbol ExitExpression { get; }

        /// <summary>
        ///     goto label
        /// </summary>
        public LabelSymbol GoToLabel { get; }

        /// <summary>
        ///     goto symbol
        /// </summary>
        public Terminal GotoSymbol { get; }

        /// <summary>
        ///     open paren
        /// </summary>
        public Terminal OpenParen { get; }

        /// <summary>
        ///     close paren
        /// </summary>
        public Terminal CloseParen { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, GotoSymbol, visitor);
            AcceptPart(this, GoToLabel, visitor);
            AcceptPart(this, OpenParen, visitor);
            AcceptPart(this, ExitExpression, visitor);
            AcceptPart(this, CloseParen, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => GotoSymbol.GetSymbolLength() +
                GoToLabel.GetSymbolLength() +
                OpenParen.GetSymbolLength() +
                ExitExpression.GetSymbolLength() +
                CloseParen.GetSymbolLength();
    }
}