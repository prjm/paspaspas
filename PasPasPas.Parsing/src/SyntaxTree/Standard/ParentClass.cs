using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     parent class definition
    /// </summary>
    public class ParentClass : VariableLengthSyntaxTreeBase<TypeName> {

        /// <summary>
        ///     create a new parent class definition
        /// </summary>
        /// <param name="openParen"></param>
        /// <param name="typeNames"></param>
        /// <param name="closeParen"></param>
        public ParentClass(Terminal openParen, ImmutableArray<TypeName> typeNames, Terminal closeParen) : base(typeNames) {
            OpenParen = openParen;
            CloseParen = closeParen;
        }

        /// <summary>
        ///     open paren
        /// </summary>
        public Terminal OpenParen { get; set; }

        /// <summary>
        ///     close paren
        /// </summary>
        public Terminal CloseParen { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, OpenParen, visitor);
            AcceptPart(this, visitor);
            AcceptPart(this, CloseParen, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => OpenParen.GetSymbolLength() +
               ItemLength +
               CloseParen.GetSymbolLength();

    }
}