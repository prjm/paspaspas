using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     procedural type specification
    /// </summary>
    public class ProceduralType : TypeSpecificationBase, IParameterTarget {

        /// <summary>
        ///     procedure kind
        /// </summary>
        public ProcedureKind Kind { get; set; }

        /// <summary>
        ///     return type attributes
        /// </summary>
        public IEnumerable<SymbolAttribute> ReturnAttributes { get; set; }

        /// <summary>
        ///     parameters
        /// </summary>
        public ParameterDefinitions Parameters { get; }
            = new ParameterDefinitions();

        /// <summary>
        ///     maps a token kind to a procedure kind
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static ProcedureKind MapKind(int kind) {
            switch (kind) {
                case TokenKind.Function:
                    return ProcedureKind.Function;
                case TokenKind.Procedure:
                    return ProcedureKind.Procedure;
                default:
                    return ProcedureKind.Unknown;
            }
        }

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
        {
            get
            {
                foreach (var parameter in Parameters)
                    yield return parameter;

                if (ReturnAttributes != null) {
                    foreach (var attribute in ReturnAttributes)
                        yield return attribute;
                }
            }
        }
    }
}
