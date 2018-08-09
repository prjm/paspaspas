using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     parameter list
    /// </summary>
    public class FormalParametersSymbol : VariableLengthSyntaxTreeBase<FormalParameterDefinitionSymbol> {

        /// <summary>
        ///     create a new set of formal parameters
        /// </summary>
        /// <param name="items"></param>
        public FormalParametersSymbol(ImmutableArray<FormalParameterDefinitionSymbol> items) : base(items) {
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
        ///     item length
        /// </summary>
        public override int Length => ItemLength;

    }
}