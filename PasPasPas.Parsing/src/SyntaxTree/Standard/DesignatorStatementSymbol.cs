using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     designator
    /// </summary>
    public class DesignatorStatementSymbol : VariableLengthSyntaxTreeBase<DesignatorItemSymbol> {

        /// <summary>
        ///     create a new designator statement
        /// </summary>
        /// <param name="inherited"></param>
        /// <param name="name"></param>
        /// <param name="immutableArray"></param>
        public DesignatorStatementSymbol(Terminal inherited, TypeNameSymbol name, ImmutableArray<DesignatorItemSymbol> immutableArray) : base(immutableArray) {
            Name = name;
            Inherited = inherited;
        }

        /// <summary>
        ///     inherited
        /// </summary>
        public Terminal Inherited { get; }

        /// <summary>
        ///     name
        /// </summary>
        public TypeNameSymbol Name { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Inherited, visitor);
            AcceptPart(this, Name, visitor);
            AcceptPart(this, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Inherited.GetSymbolLength() +
                Name.GetSymbolLength() +
                ItemLength;


    }
}