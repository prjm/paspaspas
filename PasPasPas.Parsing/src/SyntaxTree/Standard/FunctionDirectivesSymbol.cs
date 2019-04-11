using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     function directive
    /// </summary>
    public class FunctionDirectivesSymbol : VariableLengthSyntaxTreeBase<SyntaxPartBase> {

        /// <summary>
        ///     create a new set of function directives
        /// </summary>
        /// <param name="items"></param>
        public FunctionDirectivesSymbol(ImmutableArray<SyntaxPartBase> items) : base(items) {
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
        public override int Length => ItemLength;

        /// <summary>
        ///     test if this is a forward declaration
        /// </summary>
        public bool IsForwardDeclaration {
            get {
                for (var i = 0; i < Items.Length; i++) {
                    var directive = Items[i] as ForwardDirectiveSymbol;
                    if (directive != default)
                        return true;
                }
                return false;
            }
        }
    }
}