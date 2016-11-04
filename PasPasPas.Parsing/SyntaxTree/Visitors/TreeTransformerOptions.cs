using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using System;
using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Standard;

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
        public DeclarationMode CurrentDeclarationMode { get; internal set; }

        /// <summary>
        ///     current expression scope
        /// </summary>
        public Stack<IExpressionTarget> CurrentExpressionScope { get; }
            = new Stack<IExpressionTarget>();


        /// <summary>
        ///     last expression
        /// </summary>
        public IExpressionTarget LastExpression
        {
            get
            {
                if (CurrentExpressionScope.Count > 0)
                    return CurrentExpressionScope.Peek();
                else
                    return null;

            }
        }

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
                // error ?
            }
            else {
                stack.Pop();
            }
        }

        /// <summary>
        ///     stop declaring symbols
        /// </summary>
        /// <param name="symbolsToDeclare">symbol list</param>
        public void EndDeclare(DeclaredSymbols symbolsToDeclare) {
            PopOrFail(CurrentDefinitionScope, symbolsToDeclare);
        }

        /// <summary>
        ///     start decalring symbols
        /// </summary>
        /// <param name="symbolsToDeclare">symbols to declare</param>
        public void BeginDeclare(DeclaredSymbols symbolsToDeclare) {
            CurrentDefinitionScope.Push(symbolsToDeclare);
        }

        /// <summary>
        ///     define an expression value
        /// </summary>
        /// <param name="value"></param>
        public void DefineExpressionValue(ExpressionBase value) {
            if (CurrentExpressionScope.Count > 0) {
                CurrentExpressionScope.Peek().Value = value;
                var target = value as IExpressionTarget;
                if (target != null) {
                    CurrentExpressionScope.Push(target);
                }
            }
            else {
                // error ??
            }
        }

        /// <summary>
        ///     declare an object
        /// </summary>
        /// <typeparam name="T">object type to declare</typeparam>
        /// <param name="constDeclaration"></param>
        /// <returns></returns>
        public T Declare<T>(ConstDeclaration constDeclaration) where T : DeclaredSymbol, new() {
            if (CurrentDefinitionScope.Count > 0) {
                var scope = CurrentDefinitionScope.Peek();
                T declaration = CreateNode<T>(scope, constDeclaration);
                return declaration;
            }
            else {
                // error ?
                T declaration = CreateNode<T>(null, constDeclaration);
                return declaration;
            }
        }

        private static T CreateNode<T>(object parent, ISyntaxPart element) where T : new() {
            var result = new T();
            return result;
        }

        /// <summary>
        ///     complaete a declaration
        /// </summary>
        /// <param name="declaration"></param>
        public void CompleteDeclaration(DeclaredSymbol declaration) {
            if (CurrentDefinitionScope.Count > 0) {
                var scope = CurrentDefinitionScope.Peek();
                scope.Add(declaration, LogSource);
            }
            else {
                // error ??
            }
        }

        /// <summary>
        ///     define an expression value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        public T DefineExpressionValue<T>(ISyntaxPart parent) where T : ExpressionBase, new() {
            var node = CreateNode<T>(LastExpression, parent);
            DefineExpressionValue(node);
            return node;
        }


        /// <summary>
        ///     complete expression
        /// </summary>
        public void CompleteExpression() {
            if (CurrentExpressionScope.Count > 0) {
                CurrentExpressionScope.Pop();
            }
            else {
                // error ?               
            }
        }

        /// <summary>
        ///     end expression definition
        /// </summary>
        public void EndExpression() {
            PopLastOrFail(CurrentExpressionScope);
        }

        /// <summary>
        ///     begin expression
        /// </summary>
        /// <param name="declaration"></param>
        public void BeginExpression(IExpressionTarget declaration) {
            if (CurrentExpressionScope.Count > 0) {
                // error ?
            }
            else {
                CurrentExpressionScope.Push(declaration);
            }
        }
    }
}