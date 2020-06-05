#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     pseudo x64 operation
    /// </summary>
    public class AsmPseudoOpSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new asm pseudo op symbol
        /// </summary>
        /// <param name="dot"></param>
        /// <param name="kindSymbol"></param>
        /// <param name="mode"></param>
        /// <param name="numberOfParams"></param>
        /// <param name="register"></param>
        public AsmPseudoOpSymbol(Terminal dot, IdentifierSymbol kindSymbol, AsmPrefixSymbolKind mode, StandardInteger numberOfParams, IdentifierSymbol register) {
            NumberOfParams = numberOfParams;
            Register = register;
            Mode = mode;
            DotSymbol = dot;
            Kind = kindSymbol;
            NumberOfParams = numberOfParams;
        }

        /// <summary>
        ///     operation kind
        /// </summary>
        public IdentifierSymbol Kind { get; }

        /// <summary>
        ///     number of parameters
        /// </summary>
        public StandardInteger NumberOfParams { get; }

        /// <summary>
        ///     register name
        /// </summary>
        public IdentifierSymbol Register { get; }

        /// <summary>
        ///     mode
        /// </summary>
        public AsmPrefixSymbolKind Mode { get; }

        /// <summary>
        ///     dot symbol
        /// </summary>
        public Terminal DotSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, DotSymbol, visitor);
            AcceptPart(this, Kind, visitor);
            AcceptPart(this, Register, visitor);
            AcceptPart(this, NumberOfParams, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol name length
        /// </summary>
        public override int Length
            => DotSymbol.GetSymbolLength() +
                Kind.GetSymbolLength() +
                Register.GetSymbolLength() +
                NumberOfParams.GetSymbolLength();
    }
}
