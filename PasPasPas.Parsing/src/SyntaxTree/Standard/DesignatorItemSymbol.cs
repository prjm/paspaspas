using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     designator item
    /// </summary>
    public class DesignatorItemSymbol : VariableLengthSyntaxTreeBase<ParameterSymbol> {

        /// <summary>
        ///     dereference symbol
        /// </summary>
        /// <param name="dereference"></param>
        public DesignatorItemSymbol(Terminal dereference) : base(ImmutableArray<ParameterSymbol>.Empty)
            => Dereference = dereference;

        /// <summary>
        ///     index expression
        /// </summary>
        /// <param name="dotSymbol"></param>
        /// <param name="subitem"></param>
        /// <param name="genericSuffix"></param>
        /// <param name="openBraces"></param>
        /// <param name="indexExpression"></param>
        /// <param name="closeBraces"></param>
        public DesignatorItemSymbol(Terminal dotSymbol, IdentifierSymbol subitem, GenericSuffixSymbol genericSuffix, Terminal openBraces, ExpressionList indexExpression, Terminal closeBraces) : base(ImmutableArray<ParameterSymbol>.Empty) {
            DotSymbol = dotSymbol;
            Subitem = subitem;
            SubitemGenericType = genericSuffix;
            OpenBraces = openBraces;
            IndexExpression = indexExpression;
            CloseBraces = closeBraces;
        }

        /// <summary>
        ///     create a new designator item
        /// </summary>
        /// <param name="dotSymbol"></param>
        /// <param name="subitem"></param>
        /// <param name="genericSuffix"></param>
        /// <param name="openParen"></param>
        /// <param name="immutableArray"></param>
        /// <param name="closeParen"></param>
        /// <param name="paramList"></param>
        public DesignatorItemSymbol(Terminal dotSymbol, IdentifierSymbol subitem, GenericSuffixSymbol genericSuffix, Terminal openParen, ImmutableArray<ParameterSymbol> immutableArray, Terminal closeParen, bool paramList) : base(immutableArray) {
            DotSymbol = dotSymbol;
            Subitem = subitem;
            SubitemGenericType = genericSuffix;
            OpenParen = openParen;
            CloseParen = closeParen;
            ParameterList = paramList;
        }

        /// <summary>
        ///     create a new designator item
        /// </summary>
        /// <param name="dotSymbol"></param>
        /// <param name="subitem"></param>
        /// <param name="openParen"></param>
        /// <param name="children"></param>
        /// <param name="closeParen"></param>
        public DesignatorItemSymbol(Terminal dotSymbol, IdentifierSymbol subitem, Terminal openParen, ConstantExpressionSymbol children, Terminal closeParen) : this(dotSymbol) {
            DotSymbol = dotSymbol;
            Subitem = subitem;
            OpenParen = openParen;
            ConstantExpression = children;
            CloseParen = closeParen;
        }

        /// <summary>
        ///     dereference
        /// </summary>
        public Terminal Dereference { get; }

        /// <summary>
        ///     index expression
        /// </summary>
        public ExpressionList IndexExpression { get; }

        /// <summary>
        ///     close braces
        /// </summary>
        public Terminal CloseBraces { get; }

        /// <summary>
        ///     dot symbol
        /// </summary>
        public Terminal DotSymbol { get; }

        /// <summary>
        ///     subitem
        /// </summary>
        public IdentifierSymbol Subitem { get; }

        /// <summary>
        ///     generic type of the subitem
        /// </summary>
        public GenericSuffixSymbol SubitemGenericType { get; }

        /// <summary>
        ///     open braces
        /// </summary>
        public Terminal OpenBraces { get; }

        /// <summary>
        ///     open paren
        /// </summary>
        public Terminal OpenParen { get; }

        /// <summary>
        ///     close paren
        /// </summary>
        public Terminal CloseParen { get; }

        /// <summary>
        ///     <true>if a parameter list</true>
        /// </summary>
        public bool ParameterList { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Dereference, visitor);
            AcceptPart(this, DotSymbol, visitor);
            AcceptPart(this, Subitem, visitor);
            AcceptPart(this, SubitemGenericType, visitor);
            AcceptPart(this, OpenBraces, visitor);
            AcceptPart(this, IndexExpression, visitor);
            AcceptPart(this, CloseBraces, visitor);
            AcceptPart(this, OpenParen, visitor);
            AcceptPart(this, ConstantExpression, visitor);
            AcceptPart(this, visitor);
            AcceptPart(this, CloseParen, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Dereference.GetSymbolLength() +
                DotSymbol.GetSymbolLength() +
                Subitem.GetSymbolLength() +
                SubitemGenericType.GetSymbolLength() +
                OpenBraces.GetSymbolLength() +
                IndexExpression.GetSymbolLength() +
                CloseBraces.GetSymbolLength() +
                OpenParen.GetSymbolLength() +
                ConstantExpression.GetSymbolLength() +
                ItemLength +
                CloseParen.GetSymbolLength();

        /// <summary>
        ///     constant expression
        /// </summary>
        public ConstantExpressionSymbol ConstantExpression { get; private set; }
    }
}