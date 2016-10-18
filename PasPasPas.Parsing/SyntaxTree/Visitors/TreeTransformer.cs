using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Standard;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     convert a concrete syntax tree to an abstract one
    /// </summary>
    public class TreeTransformer : SyntaxPartVisitorBase<TreeTransformerOptions> {

        /// <summary>
        ///     visit a syntax node
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool BeginVisit(ISyntaxPart syntaxPart, TreeTransformerOptions parameter) {
            dynamic part = syntaxPart;
            BeginVisitItem(part, parameter);
            return true;
        }

        /// <summary>
        ///     visit a unit
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="parameter"></param>
        private void BeginVisitItem(Unit unit, TreeTransformerOptions parameter) {
            var result = CreateTreeNode<CompilationUnit>(unit);
            parameter.Project.Add(result);
        }

        private T CreateTreeNode<T>(ISyntaxPart parent) where T : new() {
            var result = new T();
            return result;
        }

    }
}
