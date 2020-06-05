#nullable disable
using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     external directive
    /// </summary>
    public class ExternalDirectiveSymbol : VariableLengthSyntaxTreeBase<ExternalSpecifierSymbol> {

        /// <summary>
        ///     external directives
        /// </summary>
        /// <param name="directive"></param>
        /// <param name="externalExpression"></param>
        /// <param name="items"></param>
        /// <param name="semicolon"></param>
        public ExternalDirectiveSymbol(Terminal directive, ConstantExpressionSymbol externalExpression, ImmutableArray<ExternalSpecifierSymbol> items, Terminal semicolon) : base(items) {
            Directive = directive;
            ExternalExpression = externalExpression;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     external expression
        /// </summary>
        public ConstantExpressionSymbol ExternalExpression { get; }

        /// <summary>
        ///     kind
        /// </summary>
        public int Kind
            => Directive.GetSymbolKind();

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     directive
        /// </summary>
        public Terminal Directive { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Directive, visitor);
            AcceptPart(this, ExternalExpression, visitor);
            AcceptPart(this, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     length
        /// </summary>
        public override int Length
            => Directive.GetSymbolLength() +
                ExternalExpression.GetSymbolLength() +
                ItemLength +
                Semicolon.GetSymbolLength();

    }
}
