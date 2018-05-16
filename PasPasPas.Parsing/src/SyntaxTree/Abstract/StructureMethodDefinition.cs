using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method definitions
    /// </summary>
    public class StructureMethodDefinition : SymbolTableBase<StructureMethod> {

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     merge overoaded methos
        /// </summary>
        protected override bool HasDuplicateReplacement
            => true;

        /// <summary>
        ///     merge overloaded methods
        /// </summary>
        /// <param name="existingEntry">existing method entry</param>
        /// <param name="entry">overloaded entry</param>
        /// <returns></returns>
        protected override StructureMethod MergeDuplicates(StructureMethod existingEntry, StructureMethod entry) {
            var result = existingEntry;

            if (!result.IsOverloaded && existingEntry.IsOverloaded) {
                // todo: warning here
            }

            result.AddOverload(entry);
            return result;
        }

    }
}
