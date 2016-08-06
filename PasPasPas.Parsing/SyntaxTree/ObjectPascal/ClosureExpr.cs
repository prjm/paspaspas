using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     closure expression
    /// </summary>
    public class ClosureExpr : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ClosureExpr(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        /// block
        /// </summary>
        public Block Block { get; internal set; }

        /// <summary>
        ///     closue kind
        /// </summary>
        public int Kind { get; internal set; }

        /// <summary>
        ///     closure parameters
        /// </summary>
        public FormalParameterSection Parameters { get; internal set; }

        /// <summary>
        ///     closure return type
        /// </summary>
        public TypeSpecification ReturnType { get; internal set; }

        /// <summary>
        ///     format closure
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            switch (Kind) {
                case PascalToken.Function:
                    result.Keyword("function");
                    break;
                case PascalToken.Procedure:
                    result.Keyword("procedure");
                    break;
            }
            result.Space();
            result.Part(Parameters);
            if (ReturnType != null) {
                result.Punct(":");
                result.Space();
                result.Part(ReturnType);
            }
            result.StartIndent();

            result.NewLine();
            result.StartIndent();
            result.Part(Block);
            result.EndIndent();
        }
    }
}