using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     simple type definition
    /// </summary>
    public class SimpleTypeSymbol : VariableLengthSyntaxTreeBase<GenericNamespaceNameSymbol> {

        /// <summary>
        ///     create a new simple type
        /// </summary>
        /// <param name="enumTypeDefinition"></param>
        public SimpleTypeSymbol(EnumTypeDefinitionSymbol enumTypeDefinition) : base(ImmutableArray<GenericNamespaceNameSymbol>.Empty)
            => EnumType = enumTypeDefinition;

        /// <summary>
        ///     create a new simple type
        /// </summary>
        /// <param name="newType"></param>
        /// <param name="typeOf"></param>
        /// <param name="items"></param>
        public SimpleTypeSymbol(Terminal newType, Terminal typeOf, ImmutableArray<GenericNamespaceNameSymbol> items) : base(items) {
            NewType = newType;
            TypeOf = typeOf;
        }

        /// <summary>
        ///     create a new simple type*
        /// </summary>
        /// <param name="newType"></param>
        /// <param name="typeOf"></param>
        /// <param name="subrangeStart"></param>
        /// <param name="dotDot"></param>
        /// <param name="subrangeEnd"></param>
        public SimpleTypeSymbol(Terminal newType, Terminal typeOf, ConstantExpressionSymbol subrangeStart, Terminal dotDot, ConstantExpressionSymbol subrangeEnd) : base(ImmutableArray<GenericNamespaceNameSymbol>.Empty) {
            NewType = newType;
            TypeOf = typeOf;
            SubrangeStart = subrangeStart;
            DotDot = dotDot;
            SubrangeEnd = subrangeEnd;
        }

        /// <summary>
        ///     enumeration
        /// </summary>
        public EnumTypeDefinitionSymbol EnumType { get; }

        /// <summary>
        ///     <c>true</c> for a new type definition
        /// </summary>
        public Terminal NewType { get; }

        /// <summary>
        ///     subrange start
        /// </summary>
        public ConstantExpressionSymbol SubrangeEnd { get; }

        /// <summary>
        ///     subrange end
        /// </summary>
        public ConstantExpressionSymbol SubrangeStart { get; }

        /// <summary>
        ///     <c>type of</c> declaration
        /// </summary>
        public Terminal TypeOf { get; }

        /// <summary>
        ///     dots
        /// </summary>
        public Terminal DotDot { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, EnumType, visitor);
            AcceptPart(this, NewType, visitor);
            AcceptPart(this, TypeOf, visitor);
            AcceptPart(this, visitor);
            AcceptPart(this, SubrangeStart, visitor);
            AcceptPart(this, DotDot, visitor);
            AcceptPart(this, SubrangeEnd, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length =>
            EnumType.GetSymbolLength() +
            NewType.GetSymbolLength() +
            TypeOf.GetSymbolLength() +
            ItemLength +
            SubrangeStart.GetSymbolLength() +
            DotDot.GetSymbolLength() +
            SubrangeEnd.GetSymbolLength();

    }
}