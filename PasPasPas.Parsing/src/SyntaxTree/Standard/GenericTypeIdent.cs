using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     generic type identifier
    /// </summary>
    public class GenericTypeIdentifier : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new generic type identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="genericDefinition"></param>
        public GenericTypeIdentifier(Identifier identifier, GenericDefinitionSymbol genericDefinition) {
            Identifier = identifier;
            GenericDefinition = genericDefinition;
        }

        /// <summary>
        ///     generic definition
        /// </summary>
        public GenericDefinitionSymbol GenericDefinition { get; }

        /// <summary>
        ///     type name
        /// </summary>
        public Identifier Identifier { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Identifier, visitor);
            AcceptPart(this, GenericDefinition, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Identifier.GetSymbolLength() + GenericDefinition.GetSymbolLength();

    }
}