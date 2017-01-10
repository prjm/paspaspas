using System.Collections.Generic;

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
    }
}
