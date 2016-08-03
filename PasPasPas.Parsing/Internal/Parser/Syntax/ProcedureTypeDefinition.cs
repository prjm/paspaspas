using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     procedure type definition
    /// </summary>
    public class ProcedureTypeDefinition : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ProcedureTypeDefinition(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     kind (function or procedure)
        /// </summary>
        public int Kind { get; internal set; }

        /// <summary>
        ///     <c>true</c> if this is a method declaration
        /// </summary>
        public bool MethodDeclaration { get; internal set; } = false;

        /// <summary>
        ///     function / procedure parameters
        /// </summary>
        public FormalParameterSection Parameters { get; internal set; }

        /// <summary>
        ///     return types
        /// </summary>
        public TypeSpecification ReturnType { get; internal set; }

        /// <summary>
        ///     attributes of return types
        /// </summary>
        public UserAttributes ReturnTypeAttributes { get; internal set; }

        /// <summary>
        ///     format procedure reference
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (Kind == PascalToken.Function) {
                result.Keyword("function").Space();
            }
            else if (Kind == PascalToken.Procedure) {
                result.Keyword("procedure").Space();
            }

            result.Part(Parameters);

            if (ReturnType != null) {
                result.Space().Punct(":").Space();
                result.Part(ReturnTypeAttributes).Space();
                result.Part(ReturnType);
            }

            if (MethodDeclaration) {
                result.Keyword("of").Space().Keyword("object");
            }
        }
    }
}