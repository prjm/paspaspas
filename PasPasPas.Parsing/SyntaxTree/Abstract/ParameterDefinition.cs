using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     formal parameter definition
    /// </summary>
    public class ParameterDefinition : SymbolTableEntryBase, ITypeTarget {

        /// <summary>
        ///     attributes
        /// </summary>
        public IEnumerable<SymbolAttribute> Attributes { get; set; }

        /// <summary>
        ///     parameter name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     parameter kind
        /// </summary>
        public ParameterReferenceKind ParameterKind { get; set; }

        /// <summary>
        ///     parameter type
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     symbol name
        /// </summary>
        protected override string InternalSymbolName
            => Name?.CompleteName;

        /// <summary>
        ///     map parameter reference kind
        /// </summary>
        /// <param name="parameterType"></param>
        /// <returns></returns>
        public static ParameterReferenceKind MapKind(int parameterType) {
            switch (parameterType) {
                case TokenKind.Const:
                    return ParameterReferenceKind.Const;
                case TokenKind.Var:
                    return ParameterReferenceKind.Var;
                case TokenKind.Out:
                    return ParameterReferenceKind.Out;
                default:
                    return ParameterReferenceKind.Undefined;
            }
        }
    }
}
