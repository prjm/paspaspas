namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     base class for a visitor
    /// </summary>
    public sealed class Visitor : IStartVisitor, IEndVisitor {

        private readonly object specificVisitor;

        /// <summary>
        ///     create a new visitor
        /// </summary>
        /// <param name="specificVisitor">specific visitor</param>
        public Visitor(object specificVisitor) {
            this.specificVisitor = specificVisitor;
        }

        /// <summary>
        ///     visit an element
        /// </summary>
        /// <typeparam name="VisitorType"></typeparam>
        /// <param name="element to visit"></param>
        public void StartVisit<VisitorType>(VisitorType element) {
            var s = specificVisitor as IStartVisitor<VisitorType>;
            s?.StartVisit(element);
        }

        /// <summary>
        ///     visit an element
        /// </summary>
        /// <typeparam name="VisitorType"></typeparam>
        /// <param name="element to visit"></param>
        public void EndVisit<VisitorType>(VisitorType element) {
            var e = specificVisitor as IEndVisitor<VisitorType>;
            e?.EndVisit(element);
        }
    }
}
