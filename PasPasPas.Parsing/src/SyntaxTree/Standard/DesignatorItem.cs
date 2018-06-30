using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     designator item
    /// </summary>
    public class DesignatorItem : VariableLengthSyntaxTreeBase<Parameter> {

        /// <summary>
        ///     dereference symbol
        /// </summary>
        /// <param name="dereference"></param>
        public DesignatorItem(Terminal dereference) : base(ImmutableArray<Parameter>.Empty)
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
        public DesignatorItem(Terminal dotSymbol, Identifier subitem, GenericSuffix genericSuffix, Terminal openBraces, ExpressionList indexExpression, Terminal closeBraces) : base(ImmutableArray<Parameter>.Empty) {
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
        public DesignatorItem(Terminal dotSymbol, Identifier subitem, GenericSuffix genericSuffix, Terminal openParen, ImmutableArray<Parameter> immutableArray, Terminal closeParen) : base(immutableArray) {
            DotSymbol = dotSymbol;
            Subitem = subitem;
            SubitemGenericType = genericSuffix;
            OpenParen = openParen;
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
        public Identifier Subitem { get; }

        /// <summary>
        ///     generic type of the subitem
        /// </summary>
        public GenericSuffix SubitemGenericType { get; }

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

            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => DotSymbol.GetSymbolLength() +
                Subitem.GetSymbolLength() +
                SubitemGenericType.GetSymbolLength() +
                OpenBraces.GetSymbolLength() +
                IndexExpression.GetSymbolLength() +
                CloseBraces.GetSymbolLength() +
                OpenParen.GetSymbolLength() +
                ItemLength +
                CloseParen.GetSymbolLength();

    }
}