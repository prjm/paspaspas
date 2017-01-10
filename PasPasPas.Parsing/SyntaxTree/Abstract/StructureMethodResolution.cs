using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method resolution
    /// </summary>
    public class StructureMethodResolution : AbstractSyntaxPart, ITypeTarget {

        /// <summary>
        ///     type value
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }


        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (TypeValue != null)
                    yield return TypeValue;
            }
        }

        /// <summary>
        ///     attributes
        /// </summary>                                             
        public IList<SymbolAttribute> Attributes { get; set; }

        /// <summary>
        ///     methodkind
        /// </summary>
        public StructureMethodResolutionKind Kind { get; set; }

        /// <summary>
        ///     target identifier
        /// </summary>
        public SymbolName Target { get; set; }

        /// <summary>
        ///     map token kind to method kind
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static StructureMethodResolutionKind MapKind(int kind) {
            switch (kind) {
                case TokenKind.Function:
                    return StructureMethodResolutionKind.Function;

                case TokenKind.Procedure:
                    return StructureMethodResolutionKind.Procedure;

                default:
                    return StructureMethodResolutionKind.Undefined;

            }
        }
    }
}
