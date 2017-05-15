using System;
using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     base class for a visitor
    /// </summary>
    public class Visitor : IStartEndVisitor {

        private readonly object specificVisitor;
        private readonly Stack<WorkingStackEntry> stack
            = new Stack<WorkingStackEntry>();

        public object SpecificVisitor
            => specificVisitor;

        public Stack<WorkingStackEntry> WorkingStack
            => stack;

        /// <summary>
        ///     create a new visitor
        /// </summary>
        /// <param name="specificVisitor">specific visitor</param>
        public Visitor(object specificVisitor)
            => this.specificVisitor = specificVisitor;

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
            while (stack != null && stack.Count > 0 && object.Equals(stack.Peek().DefiningNode, element))
                stack.Pop();
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
            while (WorkingStack != null && WorkingStack.Count > 0 && object.Equals(WorkingStack.Peek().DefiningNode, element) && WorkingStack.Peek().ChildNode == child)
                WorkingStack.Pop();
        }


    }

}