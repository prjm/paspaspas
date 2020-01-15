using PasPasPas.Parsing.SyntaxTree.Abstract;

namespace PasPasPas.Typings.Common {

    public partial class TypeAnnotator {

        /// <summary>
        ///     visit a statement
        /// </summary>
        /// <param name="element">statement to check</param>
        public void EndVisit(StructuredStatement element) {
            if (element.Kind == StructuredStatementKind.Assignment) {
                EndVisitAsignment(element);
            }
        }

        private void EndVisitAsignment(StructuredStatement element) {
            var left = element.Expressions.Count > 0 ? element.Expressions[0]?.TypeInfo : null;
            var right = element.Expressions.Count > 1 ? element.Expressions[1]?.TypeInfo : null;
            if (left != null && right != null) {
                environment.TypeRegistry.GetTypeByIdOrUndefinedType(left.TypeId).CanBeAssignedFrom(environment.TypeRegistry.GetTypeByIdOrUndefinedType(right.TypeId));
            }

        }
    }
}
