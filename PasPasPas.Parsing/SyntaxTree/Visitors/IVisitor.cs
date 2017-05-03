using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public interface IChildVisitor<VisitorType> {

        void StartVisitChild(VisitorType element, ISyntaxPart child);

        void EndVisitChild(VisitorType element, ISyntaxPart child);

    }

    public interface IChildVisitor {

        void StartVisitChild<VisitorType>(VisitorType element, ISyntaxPart child);

        void EndVisitChild<VisitorType>(VisitorType element, ISyntaxPart child);


    }
}
