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
            var result = CreateTreeNode<CompilationUnit>(null, unit);
            result.FileType = CompilationUnitType.Unit;
            result.UnitName = CreateLeafNode<SymbolName>(result, unit.UnitName);
            result.UnitName.Name = unit.UnitName?.Name;
            result.UnitName.Namespace = unit.UnitName?.Namespace;
            result.FilePath = unit.UnitHead.FirstTerminalToken.FilePath;
            parameter.Project.Add(result, parameter.LogSource);
        }

        private void BeginVisitItem(ISyntaxPart part, TreeTransformerOptions parameter) {
            //..
        }

        private static T CreateTreeNode<T>(ISyntaxPart parent, ISyntaxPart element) where T : ISyntaxPart, new() {
            var result = new T();
            result.Parent = parent;
            return result;
        }


        private static T CreateLeafNode<T>(ISyntaxPart parent, ISyntaxPart element) where T : new() {
            var result = new T();
            return result;
        }

    }
}
