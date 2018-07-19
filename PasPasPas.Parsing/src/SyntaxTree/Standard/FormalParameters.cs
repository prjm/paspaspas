using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     parameter list
    /// </summary>
    public class FormalParameters : VariableLengthSyntaxTreeBase<FormalParameterDefinition> {

        /// <summary>
        ///     create a new set of formal parameters
        /// </summary>
        /// <param name="items"></param>
        public FormalParameters(ImmutableArray<FormalParameterDefinition> items) : base(items) {
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