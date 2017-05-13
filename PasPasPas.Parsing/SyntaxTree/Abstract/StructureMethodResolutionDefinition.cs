using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method resolutions
    /// </summary>
    public class StructureMethodResolutionDefinition : AbstractSyntaxPartBase {

        public ISyntaxPartList<StructureMethodResolution> Resolutions { get; }

        /// <summary>
        ///     create a new method resolution definition of a structured type
        /// </summary>
        public StructureMethodResolutionDefinition()
            => Resolutions = new SyntaxPartCollection<StructureMethodResolution>(this);

        /// <summary>
        ///     add a method resolution
        /// </summary>
        /// <param name="result"></param>
        public void Add(StructureMethodResolution result)
            => Resolutions.Add(result);

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
