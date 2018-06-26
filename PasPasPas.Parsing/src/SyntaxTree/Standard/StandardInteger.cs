using System;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     constant integer
    /// </summary>
    public class StandardInteger : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new integer value
        /// </summary>
        /// <param name="value"></param>
        public StandardInteger(Terminal value)
            => Value = value;

        /// <summary>
        ///     integer value
        /// </summary>
        public Terminal Value { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Value.Length;

    }
}
