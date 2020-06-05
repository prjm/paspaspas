#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     program header
    /// </summary>
    public class ProgramHeadSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new program head symbol
        /// </summary>
        /// <param name="programSymbol"></param>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="semicolon"></param>
        public ProgramHeadSymbol(Terminal programSymbol, NamespaceNameSymbol name, ProgramParameterListSymbol parameters, Terminal semicolon) {
            ProgramSymbol = programSymbol;
            Name = name;
            Parameters = parameters;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     name of the program
        /// </summary>
        public NamespaceNameSymbol Name { get; }

        /// <summary>
        ///     program parameters
        /// </summary>
        public ProgramParameterListSymbol Parameters { get; }

        /// <summary>
        ///     program symbol
        /// </summary>
        public Terminal ProgramSymbol { get; }

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
            AcceptPart(this, ProgramSymbol, visitor);
            AcceptPart(this, Name, visitor);
            AcceptPart(this, Parameters, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     item length
        /// </summary>
        public override int Length
            => ProgramSymbol.GetSymbolLength() +
                Name.GetSymbolLength() +
                Parameters.GetSymbolLength() +
                Semicolon.GetSymbolLength();

    }
}