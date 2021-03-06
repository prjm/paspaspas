﻿#nullable disable
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method resolutions
    /// </summary>
    public class StructureMethodResolutionDefinition : AbstractSyntaxPartBase {

        /// <summary>
        ///     method resolutions
        /// </summary>
        public ISyntaxPartCollection<StructureMethodResolution> Resolutions { get; }

        /// <summary>
        ///     create a new method resolution definition of a structured type
        /// </summary>
        public StructureMethodResolutionDefinition()
            => Resolutions = new SyntaxPartCollection<StructureMethodResolution>();

        /// <summary>
        ///     add a method resolution
        /// </summary>
        /// <param name="result"></param>
        public void Add(StructureMethodResolution result)
            => Resolutions.Add(result);

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Resolutions, visitor);
            visitor.EndVisit(this);
        }
    }
}
