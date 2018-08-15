using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     reintroduce directive
    /// </summary>
    public class ReintroduceSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new reintroduce symbol
        /// </summary>
        /// <param name="directive"></param>
        /// <param name="semicolon"></param>
        public ReintroduceSymbol(Terminal directive, Terminal semicolon) {
            Directive = directive;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     directive
        /// </summary>
        public Terminal Directive { get;  }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get;  }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Directive.GetSymbolLength() + Semicolon.GetSymbolLength();

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Directive, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }


    }
}
