using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method
    /// </summary>
    public class StructureMethod : SymbolTableEntryBase, IParameterTarget, ITypeTarget {

        /// <summary>
        ///     directives
        /// </summary>
        public IList<MethodDirective> Directives { get; }
            = new List<MethodDirective>();

        /// <summary>
        ///     generic method parameter
        /// </summary>
        public GenericTypes Generics { get; set; }

        /// <summary>
        ///     method kind
        /// </summary>
        public StructureMethodKind Kind { get; set; }

        /// <summary>
        ///     name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     parameters
        /// </summary>
        public ParameterDefinitions Parameters { get; }
            = new ParameterDefinitions();

        /// <summary>
        ///     return type
        /// </summary>
        public ITypeSpecification TypeValue {
            get; set;
        }

        /// <summary>
        ///     symbol name
        /// </summary>
        protected override string InternalSymbolName
            => Name?.CompleteName;

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (MethodDirective directive in Directives)
                    yield return directive;
            }
        }

        /// <summary>
        ///     symbol hints
        /// </summary>
        public SymbolHints Hints { get; set; }

        /// <summary>
        ///     user attributes
        /// </summary>
        public IList<SymbolAttribute> Attributes { get; set; }

        /// <summary>
        ///     method kind
        /// </summary>
        /// <param name="methodKind"></param>
        /// <returns></returns>
        public static StructureMethodKind MapKind(int methodKind) {
            switch (methodKind) {
                case TokenKind.Function:
                    return StructureMethodKind.Function;
                case TokenKind.Procedure:
                    return StructureMethodKind.Procedure;
                case TokenKind.Constructor:
                    return StructureMethodKind.Constructor;
                case TokenKind.Destructor:
                    return StructureMethodKind.Destructor;
                case TokenKind.Operator:
                    return StructureMethodKind.Operator;
                default:
                    return StructureMethodKind.Undefined;
            }
        }
    }
}
