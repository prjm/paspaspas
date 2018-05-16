using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     basic interface for a visitor
    /// </summary>
    public interface IStartVisitor {

        /// <summary>
        ///     visit an element
        /// </summary>
        /// <typeparam name="VisitorType"></typeparam>
        /// <param name="element">element to visit</param>
        void StartVisit<VisitorType>(VisitorType element);

    }

    /// <summary>
    ///     basic interface for a visitor
    /// </summary>
    public interface IEndVisitor {

        /// <summary>
        ///     visit an element
        /// </summary>
        /// <typeparam name="VisitorType"></typeparam>
        /// <param name="element">element to visit</param>
        void EndVisit<VisitorType>(VisitorType element);

    }

    /// <summary>
    ///     start / end visitor
    /// </summary>
    public interface IStartEndVisitor : IStartVisitor, IEndVisitor {

    }


    /// <summary>
    ///     visitor for a concrete type
    /// </summary>
    /// <typeparam name="VisitorType">Visitor type</typeparam>
    public interface IStartVisitor<VisitorType> {

        /// <summary>
        ///     get the concrete visitor
        /// </summary>
        /// <returns>concrete visitor</returns>
        IStartEndVisitor AsVisitor();

        /// <summary>
        ///     visit an element
        /// </summary>
        /// <param name="element">element to visit</param>
        void StartVisit(VisitorType element);

    }

    /// <summary>
    ///     visitor for a concrete type
    /// </summary>
    /// <typeparam name="VisitorType">Visitor type</typeparam>
    public interface IEndVisitor<VisitorType> {

        /// <summary>
        ///     get the concrete visitor
        /// </summary>
        /// <returns>concrete visitor</returns>
        IStartEndVisitor AsVisitor();

        /// <summary>
        ///     visit an element
        /// </summary>
        /// <param name="element">element to visit</param>
        void EndVisit(VisitorType element);

    }

    /// <summary>
    ///     visit tree node children
    /// </summary>
    /// <typeparam name="VisitorType"></typeparam>
    public interface IChildVisitor<VisitorType> {

        /// <summary>
        ///     start visiting
        /// </summary>
        /// <param name="element"></param>
        /// <param name="child"></param>
        void StartVisitChild(VisitorType element, ISyntaxPart child);

        /// <summary>
        ///     stop visiting
        /// </summary>
        /// <param name="element"></param>
        /// <param name="child"></param>
        void EndVisitChild(VisitorType element, ISyntaxPart child);

    }

    /// <summary>
    ///     generic child visitor
    /// </summary>
    public interface IChildVisitor {

        /// <summary>
        ///     start visiting a child
        /// </summary>
        /// <typeparam name="VisitorType"></typeparam>
        /// <param name="element"></param>
        /// <param name="child"></param>
        void StartVisitChild<VisitorType>(VisitorType element, ISyntaxPart child);

        /// <summary>
        ///     stop visiting a child
        /// </summary>
        /// <typeparam name="VisitorType"></typeparam>
        /// <param name="element"></param>
        /// <param name="child"></param>
        void EndVisitChild<VisitorType>(VisitorType element, ISyntaxPart child);


    }
}
