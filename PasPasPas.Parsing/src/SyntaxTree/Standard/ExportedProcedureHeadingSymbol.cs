using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     exported procedure heading for an interface section of an unit
    /// </summary>
    public class ExportedProcedureHeadingSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new exported procedure heading symbol
        /// </summary>
        /// <param name="procSymbol"></param>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="colonSymbol"></param>
        /// <param name="resultAttributes"></param>
        /// <param name="resultType"></param>
        /// <param name="semicolon"></param>
        /// <param name="directives"></param>
        public ExportedProcedureHeadingSymbol(Terminal procSymbol, Identifier name, FormalParameterSection parameters, Terminal colonSymbol, UserAttributes resultAttributes, TypeSpecification resultType, Terminal semicolon, FunctionDirectivesSymbol directives) {
            ProcSymbol = procSymbol;
            Name = name;
            Parameters = parameters;
            ColonSymbol = colonSymbol;
            ResultAttributes = resultAttributes;
            ResultType = resultType;
            Semicolon = semicolon;
            Directives = directives;
        }

        /// <summary>
        ///     function directives
        /// </summary>
        public FunctionDirectivesSymbol Directives { get; }

        /// <summary>
        ///     heading kind
        /// </summary>
        public int Kind
            => ProcSymbol.GetSymbolKind();

        /// <summary>
        ///     exported procedure name
        /// </summary>
        public Identifier Name { get; }

        /// <summary>
        ///     parameters
        /// </summary>
        public FormalParameterSection Parameters { get; }

        /// <summary>
        ///     result attributes
        /// </summary>
        public SyntaxPartBase ResultAttributes { get; }

        /// <summary>
        ///     result attributes
        /// </summary>
        public UserAttributes Attributes { get; }

        /// <summary>
        ///     result types
        /// </summary>
        public TypeSpecification ResultType { get; }

        /// <summary>
        ///     procedure symbol
        /// </summary>
        public Terminal ProcSymbol { get; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, ProcSymbol, visitor);
            AcceptPart(this, Name, visitor);
            AcceptPart(this, Parameters, visitor);
            AcceptPart(this, ColonSymbol, visitor);
            AcceptPart(this, ResultAttributes, visitor);
            AcceptPart(this, ResultType, visitor);
            AcceptPart(this, Semicolon, visitor);
            AcceptPart(this, Directives, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ProcSymbol.GetSymbolLength() +
                Name.GetSymbolLength() +
                Parameters.GetSymbolLength() +
                ColonSymbol.GetSymbolLength() +
                ResultAttributes.GetSymbolLength() +
                ResultType.GetSymbolLength() +
                Semicolon.GetSymbolLength() +
                Directives.GetSymbolLength();

    }

}
