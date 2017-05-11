using System.Linq;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     helper class for tree visitors
    /// </summary>
    public static class VisitorHelper {

        private static ISyntaxPart GetNextSibling(ISyntaxPart part) {
            ISyntaxPart parent = part.ParentItem;
            var found = false;

            foreach (ISyntaxPart child in parent.Parts) {
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

                ISyntaxPart firstChild = GetFirstChild(current);

                if (firstChild != null) {
                    visitor.BeginVisitChild(current, visitorParameter, firstChild);
                    current = firstChild;
                }
                else {

                    if (!visitor.EndVisit(current, visitorParameter))
                        return;

                    if (current.ParentItem == null)
                        return;

                    visitor.EndVisitChild(current.ParentItem, visitorParameter, current);

                    ISyntaxPart sibling = GetNextSibling(current);
                    while (sibling == null) {
                        current = current.ParentItem;
                        if (current == part)
                            return;

                        if (!visitor.EndVisit(current, visitorParameter))
                            return;

                        visitor.EndVisitChild(current.ParentItem, visitorParameter, current);
                        sibling = GetNextSibling(current);
                    }
                    current = sibling;
                    visitor.BeginVisitChild(current.ParentItem, visitorParameter, current);
                }

            }
        }
    }
}
