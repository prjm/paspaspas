using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     properties
    /// </summary>
    public class StructurePropertyDefinitionCollection : SymbolTableBaseCollection<StructureProperty> {

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
        }

    }
}
