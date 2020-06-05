#nullable disable
using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     method directives
    /// </summary>
    public class MethodDirectivesSymbol : VariableLengthSyntaxTreeBase<SyntaxPartBase> {

        /// <summary>
        ///     create a new set of method directives
        /// </summary>
        /// <param name="items"></param>
        public MethodDirectivesSymbol(ImmutableArray<SyntaxPartBase> items) : base(items) {
        }

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