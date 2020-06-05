#nullable disable
using System.Collections.Generic;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     base class for a visitor
    /// </summary>
    public class Visitor : IStartEndVisitor {

        private readonly object specificVisitor;
        private readonly Stack<WorkingStackEntry> stack
            = new Stack<WorkingStackEntry>();

        /// <summary>
        ///     visitor to use
        /// </summary>
        public object SpecificVisitor
            => specificVisitor;

        /// <summary>
        ///     working stack
        /// </summary>
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
        /// <typeparam name="TElement"></typeparam>
        /// <param name="element to visit"></param>
        public void StartVisit<TElement>(TElement element) {
            var s = specificVisitor as IStartVisitor<TElement>;
            s?.StartVisit(element);
        }

        /// <summary>
        ///     visit an element
        /// </summary>
        /// <typeparam name="TElement">element type to visit</typeparam>
        /// <param name="element to visit"></param>
        public void EndVisit<TElement>(TElement element) {
            var e = specificVisitor as IEndVisitor<TElement>;
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

        /// <summary>
        ///     start visiting a child
        /// </summary>
        /// <typeparam name="TElement">t</typeparam>
        /// <param name="element"></param>
        /// <param name="child"></param>
        public void StartVisitChild<TElement>(TElement element, ISyntaxPart child) {
            var e = SpecificVisitor as IChildVisitor<TElement>;
            e?.StartVisitChild(element, child);
        }

        /// <summary>
        ///     stop visiting a child
        /// </summary>
        /// <typeparam name="TElement">visitor type</typeparam>
        /// <param name="element">element to visit</param>
        /// <param name="child">child to visit</param>
        public void EndVisitChild<TElement>(TElement element, ISyntaxPart child) {
            var e = SpecificVisitor as IChildVisitor<TElement>;
            e?.EndVisitChild(element, child);
            while (WorkingStack != null && WorkingStack.Count > 0 && object.Equals(WorkingStack.Peek().DefiningNode, element) && WorkingStack.Peek().ChildNode == child)
                WorkingStack.Pop();
        }


    }

}