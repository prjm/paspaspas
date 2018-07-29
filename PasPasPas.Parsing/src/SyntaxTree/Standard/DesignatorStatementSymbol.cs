using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     designator
    /// </summary>
    public class DesignatorStatementSymbol : VariableLengthSyntaxTreeBase<SyntaxPartBase> {

        /// <summary>
        ///     create a new designator statement
        /// </summary>
        /// <param name="inherited"></param>
        /// <param name="name"></param>
        /// <param name="immutableArray"></param>
        public DesignatorStatementSymbol(Terminal inherited, TypeName name, ImmutableArray<SyntaxPartBase> immutableArray) : base(immutableArray) {
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
        public TypeName Name { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
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