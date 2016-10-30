using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using System;
using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     options to transform a concrete syntax tree to an abstract one
    /// </summary>
    public class TreeTransformerOptions {

        private LogSource logSource;

        /// <summary>
        ///     message group id
        /// </summary>
        private static readonly Guid MessageGroupId
            = new Guid(new byte[] { 0x9, 0x1b, 0x66, 0xae, 0xc7, 0x4c, 0xd3, 0x45, 0xb5, 0x75, 0x1d, 0x4f, 0x9, 0x7e, 0xcb, 0x68 });
        /* {ae661b09-4cc7-45d3-b575-1d4f097ecb68} */

        /// <summary>
        ///     log source
        /// </summary>
        public LogSource LogSource
        {
            get
            {
                if (logSource != null)
                    return logSource;

                if (LogManager != null) {
                    logSource = new LogSource(LogManager, MessageGroupId);
                    return logSource;
                }

                return null;
            }
        }

        /// <summary>
        ///     log manager
        /// </summary>
        public LogManager LogManager { get; set; }

        /// <summary>
        ///     project root
        /// </summary>
        public ProjectRoot Project { get; }
            = new ProjectRoot();

        /// <summary>
        ///     current compilation unit
        /// </summary>
        public CompilationUnit CurrentUnit { get; set; }

        /// <summary>
        ///     current unit mode
        /// </summary>
        public UnitMode CurrentUnitMode { get; set; }

        /// <summary>
        ///     current definition scope
        /// </summary>
        public Stack<DeclaredSymbols> CurrentDefinitionScope { get; }
            = new Stack<DeclaredSymbols>();

        /// <summary>
        ///     const declaration mode
        /// </summary>
        public ConstMode CurrentConstDeclarationMode { get; internal set; }

        /// <summary>
        ///     current expression scope
        /// </summary>
        public Stack<ExpressionBase> CurrentExpressionScope { get; }
            = new Stack<ExpressionBase>();

        /// <summary>
        ///     remove an expected parameter from the stack
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stack"></param>
        /// <param name="expected"></param>
        public void PopOrFail<T>(Stack<T> stack, T expected) {
            if (EqualityComparer<T>.Default.Equals(stack.Peek(), expected))
                stack.Pop();
            else
                throw new InvalidOperationException();

        }

        /// <summary>
        ///     pop the last element from the stack
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stack"></param>
        public void PopLastOrFail<T>(Stack<T> stack) {
            if (stack.Count > 1) {
                stack.Clear();
                throw new InvalidOperationException();
            }
            else {
                stack.Pop();
            }
        }
    }
}