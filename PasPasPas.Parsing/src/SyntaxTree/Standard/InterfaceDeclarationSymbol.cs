using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     interface declaration
    /// </summary>
    public class InterfaceDeclarationSymbol : VariableLengthSyntaxTreeBase<SyntaxPartBase> {

        /// <summary>
        ///     create a new interface declaration symbol
        /// </summary>
        /// <param name="items"></param>
        public InterfaceDeclarationSymbol(ImmutableArray<SyntaxPartBase> items) : base(items) { }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ItemLength;

    }
}