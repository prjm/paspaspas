using System.Linq;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     helper class for tree visitors
    /// </summary>
    public static class VisitorHelper {

        private static ISyntaxPart GetNextSibling(ISyntaxPart part) {
            var parent = part.Parent;
            var found = false;

            foreach (var child in parent.Parts) {
                if (found)
                    return child;
                found = child == part;
            }

            return null;
        }

        private static ISyntaxPart GetFirstChild(ISyntaxPart current)
            => current.Parts.FirstOrDefault();

        /// <summary>
        ///     accept a visitor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="part"></param>
        /// <param name="visitor"></param>
        /// <param name="visitorParameter"></param>
        public static void AcceptVisitor<T>(ISyntaxPart part, ISyntaxPartVisitor<T> visitor, T visitorParameter) {
            ISyntaxPart current = part;

            while (true) {

                if (!visitor.BeginVisit(current, visitorParameter))
                    return;

                var firstChild = GetFirstChild(current);

                if (firstChild != null) {
                    visitor.BeginVisitChild(current, visitorParameter, firstChild);
                    current = firstChild;
                }
                else {

                    if (!visitor.EndVisit(current, visitorParameter))
                        return;

                    if (current.Parent == null)
                        return;

                    visitor.EndVisitChild(current.Parent, visitorParameter, current);

                    var sibling = GetNextSibling(current);
                    while (sibling == null) {
                        current = current.Parent;
                        if (current == part)
                            return;

                        if (!visitor.EndVisit(current, visitorParameter))
                            return;

                        visitor.EndVisitChild(current.Parent, visitorParameter, current);
                        sibling = GetNextSibling(current);
                    }
                    current = sibling;
                    visitor.BeginVisitChild(current.Parent, visitorParameter, current);
                }

            }
        }
    }
}
