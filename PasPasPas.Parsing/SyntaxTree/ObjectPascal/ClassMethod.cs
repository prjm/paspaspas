using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     class method definition
    /// </summary>
    public class ClassMethod : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ClassMethod(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     directviea
        /// </summary>
        public MethodDirectives Directives { get; internal set; }

        /// <summary>
        ///     generic definition
        /// </summary>
        public GenericDefinition GenericDefinition { get; internal set; }

        /// <summary>
        ///     method identifier
        /// </summary>
        public PascalIdentifier Identifier { get; internal set; }

        /// <summary>
        ///     expression for a message method
        /// </summary>
        public SyntaxPartBase MessageExpression { get; internal set; }

        /// <summary>
        ///     method kind
        /// </summary>
        public int MethodKind { get; internal set; }

        /// <summary>
        ///     formal parameters
        /// </summary>
        public FormalParameters Parameters { get; internal set; }

        /// <summary>
        ///     Result type attributes
        /// </summary>
        public UserAttributes ResultAttributes { get; internal set; }

        /// <summary>
        ///     parse a type specification
        /// </summary>
        public TypeSpecification ResultType { get; internal set; }

        /// <summary>
        ///     format method declaration
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            switch (MethodKind) {

                case PascalToken.Function:
                    result.Keyword("function");
                    break;

                case PascalToken.Procedure:
                    result.Keyword("procedure");
                    break;

                case PascalToken.Constructor:
                    result.Keyword("constructor");
                    break;

                case PascalToken.Destructor:
                    result.Keyword("destructor");
                    break;

            }

            result.Space();
            result.Identifier(Identifier.Value);

            if (GenericDefinition != null)
                GenericDefinition.ToFormatter(result);

            if (Parameters != null) {
                result.Punct("(");
                Parameters.ToFormatter(result);
                result.Punct(")");
            }

            if (ResultType != null) {
                result.Punct(":");
                result.Space();

                if (ResultAttributes.Count > 0) {
                    ResultAttributes.ToFormatter(result);
                    result.Space();

                }


                ResultType.ToFormatter(result);
            }

            result.Punct(";").Space();
            result.Part(Directives);
        }
    }
}