using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     program header
    /// </summary>
    public class ProgramHeadSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     name of the program
        /// </summary>
        public NamespaceName Name { get; set; }

        /// <summary>
        ///     program parameters
        /// </summary>
        public ProgramParameterList Parameters { get; set; }

        /// <summary>
        ///     program symbol
        /// </summary>
        public Terminal ProgramSymbol { get; set; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; set; }

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
        public int Length
            => ProgramSymbol.Length + Name.Length + Parameters.Length + Semicolon.Length;


    }
}