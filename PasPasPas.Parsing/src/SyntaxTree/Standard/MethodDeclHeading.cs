using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     method declaration heading
    /// </summary>
    public class MethodDeclarationHeading : VariableLengthSyntaxTreeBase<MethodDeclarationName> {

        /// <summary>
        ///     create a new method declaration heading
        /// </summary>
        /// <param name="kindSymbol"></param>
        /// <param name="items"></param>
        /// <param name="parameters"></param>
        /// <param name="colonSymbol"></param>
        /// <param name="resultTypeAttributes"></param>
        /// <param name="resultType"></param>
        public MethodDeclarationHeading(Terminal kindSymbol, ImmutableArray<MethodDeclarationName> items, FormalParameterSection parameters, Terminal colonSymbol, UserAttributes resultTypeAttributes, TypeSpecification resultType) : base(items) {
            KindSymbol = kindSymbol;
            Parameters = parameters;
            ColonSymbol = colonSymbol;
            ResultTypeAttributes = resultTypeAttributes;
            ResultType = resultType;
        }

        /// <summary>
        ///     method kind
        /// </summary>
        public int Kind
            => KindSymbol.GetSymbolKind();

        /// <summary>
        ///     parameters
        /// </summary>
        public FormalParameterSection Parameters { get; }

        /// <summary>
        ///     result type
        /// </summary>
        public TypeSpecification ResultType { get; }

        /// <summary>
        ///     result type attributes
        /// </summary>
        public SyntaxPartBase ResultTypeAttributes { get; }

        /// <summary>
        ///     kind symbol
        /// </summary>
        public Terminal KindSymbol { get; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, KindSymbol, visitor);
            AcceptPart(this, visitor);
            AcceptPart(this, Parameters, visitor);
            AcceptPart(this, ColonSymbol, visitor);
            AcceptPart(this, ResultTypeAttributes, visitor);
            AcceptPart(this, ResultType, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => KindSymbol.GetSymbolLength() +
                ItemLength +
                Parameters.GetSymbolLength() +
                ColonSymbol.GetSymbolLength() +
                ResultTypeAttributes.GetSymbolLength() +
                ResultType.GetSymbolLength();

    }
}