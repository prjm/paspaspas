using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     generic definition
    /// </summary>
    public class GenericDefinition : VariableLengthSyntaxTreeBase<GenericDefinitionPart> {

        /// <summary>
        ///     create a new generic definition
        /// </summary>
        /// <param name="openBrackets"></param>
        /// <param name="items"></param>
        /// <param name="closeBrackets"></param>
        public GenericDefinition(Terminal openBrackets, ImmutableArray<GenericDefinitionPart> items, Terminal closeBrackets) : base(items) {
            OpenBrackets = openBrackets;
            CloseBrackets = closeBrackets;
        }

        /// <summary>
        ///     open brackets
        /// </summary>
        public Terminal OpenBrackets { get; }

        /// <summary>
        ///     close brackets
        /// </summary>
        public Terminal CloseBrackets { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, OpenBrackets, visitor);
            AcceptPart(this, visitor);
            AcceptPart(this, CloseBrackets, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => OpenBrackets.GetSymbolLength() +
               ItemLength +
               CloseBrackets.GetSymbolLength();
    }
}