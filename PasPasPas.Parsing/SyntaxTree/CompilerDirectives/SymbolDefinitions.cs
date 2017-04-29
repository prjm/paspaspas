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
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }


    }
}
