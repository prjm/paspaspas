#nullable disable
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     generic constraint
    /// </summary>
    public class ConstrainedGenericSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new constrained generic symbol
        /// </summary>
        /// <param name="constraintSymbol"></param>
        /// <param name="identifier"></param>
        /// <param name="comma"></param>
        public ConstrainedGenericSymbol(Terminal constraintSymbol, IdentifierSymbol identifier, Terminal comma) {
            ConstraintSymbol = constraintSymbol;
            ConstraintIdentifier = identifier;
            Comma = comma;
        }

        /// <summary>
        ///     class constraints
        /// </summary>
        public bool ClassConstraint
            => ConstraintSymbol.GetSymbolKind() == TokenKind.Class;

        /// <summary>
        ///     constraint identifier
        /// </summary>
        public IdentifierSymbol ConstraintIdentifier { get; }

        /// <summary>
        ///     constructor constraint
        /// </summary>
        public bool ConstructorConstraint
            => ConstraintSymbol.GetSymbolKind() == TokenKind.Constructor;

        /// <summary>
        ///     record constraint
        /// </summary>
        public bool RecordConstraint
            => ConstraintSymbol.GetSymbolKind() == TokenKind.Record;

        /// <summary>
        ///     comma
        /// </summary>
        public Terminal Comma { get; }

        /// <summary>
        ///     constraint symbol
        /// </summary>
        public Terminal ConstraintSymbol { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, ConstraintSymbol, visitor);
            AcceptPart(this, ConstraintIdentifier, visitor);
            AcceptPart(this, Comma, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ConstraintSymbol.GetSymbolLength() +
                ConstraintIdentifier.GetSymbolLength() +
                Comma.GetSymbolLength();


    }
}