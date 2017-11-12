using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     single declared symbol
    /// </summary>
    public class SingleDeclaredSymbol : DeclaredSymbolGroup {

        private readonly AbstractSyntaxPartBase baseSymbolDefinition;

        /// <summary>
        ///     create a new symbol group
        /// </summary>
        /// <param name="baseSymbol">symbol to be wrapped</param>
        public SingleDeclaredSymbol(AbstractSyntaxPartBase baseSymbol)
            => baseSymbolDefinition = baseSymbol;

        /// <summary>
        ///     visit this symbol group
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                yield return baseSymbolDefinition;
            }
        }

    }
}
