using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     additional hinting information
    /// </summary>
    public class HintSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     hint for deprecation
        /// </summary>
        public bool Deprecated { get; set; }

        /// <summary>
        ///     comment for deprecation
        /// </summary>
        public SyntaxPartBase DeprecatedComment { get; set; }

        /// <summary>
        ///     hint for experimental
        /// </summary>
        public bool Experimental { get; set; }

        /// <summary>
        ///     hint for library
        /// </summary>
        public bool Library { get; set; }

        /// <summary>
        ///     hint for platform
        /// </summary>
        public bool Platform { get; set; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; set; }

        /// <summary>
        ///    hint symbol
        /// </summary>
        public Terminal Symbol { get; set; }


        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            AcceptPart(this, DeprecatedComment, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Symbol.GetSymbolLength() +
               DeprecatedComment.GetSymbolLength() +
               Semicolon.GetSymbolLength();

    }
}