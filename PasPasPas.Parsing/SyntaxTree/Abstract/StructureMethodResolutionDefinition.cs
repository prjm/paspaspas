using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method resolutions
    /// </summary>
    public class StructureMethodResolutionDefinition : AbstractSyntaxPart {

        private IList<StructureMethodResolution> resolutions
               = new List<StructureMethodResolution>();

        /// <summary>
        ///     resolutions
        /// </summary>
        public IList<StructureMethodResolution> Resolutions
            => resolutions;

        /// <summary>
        ///     add a method resolution
        /// </summary>
        /// <param name="result"></param>
        public void Add(StructureMethodResolution result) {
            resolutions.Add(result);
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(this, startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }
    }
}
