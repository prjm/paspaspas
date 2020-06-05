#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     generic type identifier
    /// </summary>
    public class GenericTypeIdentifierSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new generic type identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="genericDefinition"></param>
        public GenericTypeIdentifierSymbol(IdentifierSymbol identifier, GenericDefinitionSymbol genericDefinition) {
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
        public IdentifierSymbol Identifier { get; }

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
            => Identifier.GetSymbolLength() +
                GenericDefinition.GetSymbolLength();

    }
}