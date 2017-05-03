using System;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     base class for a visitor
    /// </summary>
    public class Visitor : IStartEndVisitor {

        private readonly object specificVisitor;

        public object SpecificVisitor
            => specificVisitor;

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


    /// <summary>
    ///     base class for a visitor
    /// </summary>
    public sealed class ChildVisitor : Visitor, IChildVisitor {

        /// <summary>
        ///     create a new visitor
        /// </summary>
        /// <param name="specificVisitor">specific visitor</param>
        public ChildVisitor(object specificVisitor) : base(specificVisitor) { }

        public void StartVisitChild<VisitorType>(VisitorType element, ISyntaxPart child) {
            var e = SpecificVisitor as IChildVisitor<VisitorType>;
            e?.StartVisitChild(element, child);
        }

        public void EndVisitChild<VisitorType>(VisitorType element, ISyntaxPart child) {
            var e = SpecificVisitor as IChildVisitor<VisitorType>;
            e?.EndVisitChild(element, child);
        }


    }

}