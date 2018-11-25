using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     basic interface for a visitor
    /// </summary>
    public interface IStartVisitor {

        /// <summary>
        ///     visit an element
        /// </summary>
        /// <typeparam name="TNodeToVisit"></typeparam>
        /// <param name="element">element to visit</param>
        void StartVisit<TNodeToVisit>(TNodeToVisit element);

    }

    /// <summary>
    ///     basic interface for a visitor
    /// </summary>
    public interface IEndVisitor {

        /// <summary>
        ///     visit an element
        /// </summary>
        /// <typeparam name="TNodeToVisit"></typeparam>
        /// <param name="element">element to visit</param>
        void EndVisit<TNodeToVisit>(TNodeToVisit element);

    }

    /// <summary>
    ///     start / end visitor
    /// </summary>
    public interface IStartEndVisitor : IStartVisitor, IEndVisitor {

    }


    /// <summary>
    ///     visitor for a concrete type
    /// </summary>
    /// <typeparam name="TNodeToVisit">Visitor type</typeparam>
    public interface IStartVisitor<TNodeToVisit> {

        /// <summary>
        ///     get the concrete visitor
        /// </summary>
        /// <returns>concrete visitor</returns>
        IStartEndVisitor AsVisitor();

        /// <summary>
        ///     visit an element
        /// </summary>
        /// <param name="element">element to visit</param>
        void StartVisit(TNodeToVisit element);

    }

    /// <summary>
    ///     visitor for a concrete type
    /// </summary>
    /// <typeparam name="TNodeToVisit">Visitor type</typeparam>
    public interface IEndVisitor<TNodeToVisit> {

        /// <summary>
        ///     get the concrete visitor
        /// </summary>
        /// <returns>concrete visitor</returns>
        IStartEndVisitor AsVisitor();

        /// <summary>
        ///     visit an element
        /// </summary>
        /// <param name="element">element to visit</param>
        void EndVisit(TNodeToVisit element);

    }

    /// <summary>
    ///     visit tree node children
    /// </summary>
    /// <typeparam name="TNodeToVisit"></typeparam>
    public interface IChildVisitor<TNodeToVisit> {

        /// <summary>
        ///     start visiting
        /// </summary>
        /// <param name="element"></param>
        /// <param name="child"></param>
        void StartVisitChild(TNodeToVisit element, ISyntaxPart child);

        /// <summary>
        ///     stop visiting
        /// </summary>
        /// <param name="element"></param>
        /// <param name="child"></param>
        void EndVisitChild(TNodeToVisit element, ISyntaxPart child);

    }

    /// <summary>
    ///     generic child visitor
    /// </summary>
    public interface IChildVisitor {

        /// <summary>
        ///     start visiting a child
        /// </summary>
        /// <typeparam name="TNodeToVisit"></typeparam>
        /// <param name="element"></param>
        /// <param name="child"></param>
        void StartVisitChild<TNodeToVisit>(TNodeToVisit element, ISyntaxPart child);

        /// <summary>
        ///     stop visiting a child
        /// </summary>
        /// <typeparam name="TNodeToVisit"></typeparam>
        /// <param name="element"></param>
        /// <param name="child"></param>
        void EndVisitChild<TNodeToVisit>(TNodeToVisit element, ISyntaxPart child);


    }
}
