using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     single statement
    /// </summary>
    public class Statement : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new statement
        /// </summary>
        /// <param name="label"></param>
        /// <param name="colonSymbol"></param>
        /// <param name="part"></param>
        /// <param name="semicolon"></param>
        public Statement(Label label, Terminal colonSymbol, StatementPart part, Terminal semicolon) {
            Label = label;
            ColonSymbol = colonSymbol;
            Part = part;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     label
        /// </summary>
        public SyntaxPartBase Label { get; }

        /// <summary>
        ///     statement part
        /// </summary>
        public SyntaxPartBase Part { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Label, visitor);
            AcceptPart(this, Part, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Label.GetSymbolLength() +
               ColonSymbol.GetSymbolLength() +
               Part.GetSymbolLength() +
               Semicolon.GetSymbolLength();

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; }


        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }
    }
}