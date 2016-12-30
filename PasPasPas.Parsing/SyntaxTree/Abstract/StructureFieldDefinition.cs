using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     field definitions
    /// </summary>
    public class StructureFieldDefinition : AbstractSyntaxPart {

        /// <summary>
        ///     parameter list
        /// </summary>
        public IList<StructureFields> Fields { get; }
            = new List<StructureFields>();


    }
}
