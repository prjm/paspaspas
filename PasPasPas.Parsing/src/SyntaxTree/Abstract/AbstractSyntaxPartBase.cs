﻿#nullable disable
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     base class for parts of the abstract syntax tree
    /// </summary>
    public abstract class AbstractSyntaxPartBase : ISyntaxPart {

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => 0;

        /// <summary>
        ///     generic type name separator
        /// </summary>
        public const string GenericSeparator = "`";

        /// <summary>
        ///     accept visitors
        /// </summary>
        public abstract void Accept(IStartEndVisitor visitor);

        /// <summary>
        ///     accept parts
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="element"></param>
        /// <param name="parts"></param>
        /// <param name="visitor"></param>
        protected static void AcceptPart<TParent, TChild>(TParent element, ISyntaxPartCollection<TChild> parts, IStartEndVisitor visitor) where TChild : class, ISyntaxPart {
            if (parts.Count < 1)
                return;

            for (var i = 0; i < parts.Count; i++)
                AcceptPart(element, parts[i], visitor);
        }

        /// <summary>
        ///     accept parts
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="element"></param>
        /// <param name="visitor"></param>
        protected static void AcceptPart<TParent, TChild>(TParent element, IStartEndVisitor visitor) where TParent : SymbolTableBaseCollection<TChild> where TChild : class, ISyntaxPart, ISymbolTableEntry {
            if (element.Count < 1)
                return;

            for (var i = 0; i < element.Count; i++)
                AcceptPart(element, element[i], visitor);
        }

        /// <summary>
        ///     visit a child node
        /// </summary>
        /// <typeparam name="T">visitor type</typeparam>
        /// <param name="element"></param>
        /// <param name="part"></param>
        /// <param name="visitor"></param>
        protected static void AcceptPart<T>(T element, ISyntaxPart part, IStartEndVisitor visitor) {

            if (part == null)
                return;

            var childVisitor = visitor as IChildVisitor;
            childVisitor?.StartVisitChild<T>(element, part);
            part.Accept(visitor);
            childVisitor?.EndVisitChild<T>(element, part);
        }

    }
}