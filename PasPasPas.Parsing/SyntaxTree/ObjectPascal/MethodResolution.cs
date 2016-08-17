using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     method resolution
    /// </summary>
    public class MethodResolution : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public MethodResolution(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     kind (procedure/function)
        /// </summary>
        public int Kind { get; internal set; }

        /// <summary>
        ///     resolve identifier
        /// </summary>
        public PascalIdentifier ResolveIdentifier { get; internal set; }

        /// <summary>
        ///     identifier to be resolved
        /// </summary>
        public NamespaceName TypeName { get; internal set; }

        /// <summary>
        ///     format resolution
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            switch (Kind) {
                case TokenKind.Procedure:
                    result.Keyword("procedure");
                    break;

                case TokenKind.Function:
                    result.Keyword("function");
                    break;
            }

            result.Space();
            TypeName.ToFormatter(result);
            result.Space();
            result.Operator("=");
            result.Space();
            ResolveIdentifier.ToFormatter(result);
            result.Punct(";");
        }
    }
}