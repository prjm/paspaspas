using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method definitions
    /// </summary>
    public class StructureMethodDefinitionCollection : SymbolTableBaseCollection<StructureMethod> {

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart<StructureMethodDefinitionCollection, StructureMethod>(this, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     merge overloaded methods
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

            var overload = existingEntry as OverloadedStructureMethod;
            if (overload == default) {
                overload = new OverloadedStructureMethod();
                overload.AddOverload(existingEntry);
            }
            overload.AddOverload(entry);
            return overload;
        }

    }
}
