using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Standard;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Infrastructure.Utils;
using static PasPasPas.Parsing.Tokenizer.TokenizerWithLookahead;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     syntax tree terminal
    /// </summary>
    public class Terminal : StandardSyntaxTreeBase {

        private readonly Token token;
        private readonly string prefix;
        private readonly string suffix;

        /// <summary>
        ///     create a new terminal token
        /// </summary>
        /// <param name="baseToken"></param>
        public Terminal(TokenSequence baseToken) {
            token = baseToken.Value;
            prefix = baseToken.Prefix;
            suffix = baseToken.Suffix;
        }

        /// <summary>
        ///     token
        /// </summary>
        public Token Token
            => token;

        /// <summary>
        ///     empty part list
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts { get; }
            = new EmptyEnumerable<ISyntaxPart>();

        /// <summary>
        ///     token value
        /// </summary>
        public string Value
            => Token.Value;

        /// <summary>
        ///     token kind
        /// </summary>
        public int Kind
            => Token.Kind;

        public string Suffix
            => suffix;

        public string Prefix
            => prefix;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }
    }
}