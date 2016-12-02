using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     procedural type specification
    /// </summary>
    public class ProceduralType : TypeSpecificationBase {

        /// <summary>
        ///     procedure kind
        /// </summary>
        public ProcedureKind Kind { get; set; }

        /// <summary>
        ///     return type attributes
        /// </summary>
        public IEnumerable<SymbolAttribute> ReturnAttributes { get; internal set; }

        /// <summary>
        ///     parameters
        /// </summary>
        public IList<ParameterDefinition> Parameters { get; }
            = new List<ParameterDefinition>();

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
