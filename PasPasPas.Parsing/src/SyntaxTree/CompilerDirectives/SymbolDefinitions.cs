using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     symbol definitions switch
    /// </summary>
    public class SymbolDefinitions : CompilerDirectiveBase {

        /// <summary>
        ///     definition mode
        /// </summary>
        public SymbolDefinitionInfo Mode { get; set; }

        /// <summary>
        ///     references mode
        /// </summary>
        public SymbolReferenceInfo ReferencesMode { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}
