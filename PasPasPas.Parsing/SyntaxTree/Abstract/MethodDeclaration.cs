using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     a basic method declaration
    /// </summary>
    public class MethodDeclaration : DeclaredSymbol, IParameterTarget, ITypeTarget {

        /// <summary>
        ///     create a new procedurail type
        /// </summary>
        public MethodDeclaration() {
            Parameters = new ParameterDefinitions() { Parent = this };
        }

        /// <summary>
        ///     procedure kind
        /// </summary>
        public ProcedureKind Kind { get; set; }

        /// <summary>
        ///     parameters
        /// </summary>
        public ParameterDefinitions Parameters { get; }

        /// <summary>
        ///     return type
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     method kind
        /// </summary>
        /// <param name="methodKind"></param>
        /// <returns></returns>
        public static ProcedureKind MapKind(int methodKind) {
            switch (methodKind) {
                case TokenKind.Function:
                    return ProcedureKind.Function;
                case TokenKind.Procedure:
                    return ProcedureKind.Procedure;
                case TokenKind.Constructor:
                    return ProcedureKind.Constructor;
                case TokenKind.Destructor:
                    return ProcedureKind.Destructor;
                case TokenKind.Operator:
                    return ProcedureKind.Operator;
                default:
                    return ProcedureKind.Unknown;
            }
        }

        /// <summary>
        ///     return type attributes
        /// </summary>
        public IList<SymbolAttribute> ReturnAttributes { get; set; }

        /// <summary>
        ///     user attributes
        /// </summary>
        public IList<SymbolAttribute> Attributes { get; set; }

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (ParameterTypeDefinition parameter in Parameters.Items)
                    yield return parameter;
                if (TypeValue != null)
                    yield return TypeValue;
                foreach (MethodDirective directive in Directives)
                    yield return directive;
            }
        }


        /// <summary>
        ///     directives
        /// </summary>
        public IList<MethodDirective> Directives { get; }
            = new List<MethodDirective>();



        /// <summary>
        ///     symbol hints
        /// </summary>
        public SymbolHints Hints { get; set; }

    }
}
