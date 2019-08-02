using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     a list of user attributes
    /// </summary>
    public class UserAttributesSymbol : VariableLengthSyntaxTreeBase<UserAttributeSetSymbol> {

        /// <summary>
        ///     create a new attribute list
        /// </summary>
        /// <param name="items"></param>
        public UserAttributesSymbol(ImmutableArray<UserAttributeSetSymbol> items) : base(items) {
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
        public override int Length
            => ItemLength;

    }
}