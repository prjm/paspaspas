﻿using PasPasPas.Infrastructure.Log;
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
            = null;

        /// <summary>
        ///     current unit mode
        /// </summary>
        public UnitMode CurrentUnitMode { get; set; }
            = UnitMode.Unknown;

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
        ///     current type specification scope
        /// </summary>
        public Stack<ITypeTarget> CurrentTypeSpecificationScope { get; }
            = new Stack<ITypeTarget>();

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
        ///     struct type mode
        /// </summary>
        public StructTypeMode CurrentStructTypeMode { get; set; }
            = StructTypeMode.Undefined;

        /// <summary>
        ///     last type declaration
        /// </summary>
        public ITypeTarget LastTypeDeclaration
        {
            get
            {
                if (CurrentTypeSpecificationScope.Count < 1) return null;
                return CurrentTypeSpecificationScope.Peek();
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
        ///     define an expression value
        /// </summary>
        /// <param name="value"></param>
        public void DefineTypeValue(ITypeSpecification value) {
            if (CurrentTypeSpecificationScope.Count > 0) {
                CurrentTypeSpecificationScope.Peek().TypeValue = value;
                var target = value as ITypeTarget;
                if (target != null) {
                    CurrentTypeSpecificationScope.Push(target);
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
        public T Declare<T>(ISyntaxPart constDeclaration) where T : DeclaredSymbol, new() {
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
        ///     complate a declaration
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
        ///     end a type specification
        /// </summary>
        public void EndTypeSpecification() {
            if (CurrentTypeSpecificationScope.Count > 0)
                CurrentTypeSpecificationScope.Pop();
        }

        /// <summary>
        ///     begin a type specification
        /// </summary>
        /// <param name="declaration">type declaraction</param>
        public void BeginTypeSpecification(ITypeTarget declaration) {
            CurrentTypeSpecificationScope.Push(declaration);
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
            if (CurrentExpressionScope.Count > 0)
                CurrentExpressionScope.Pop();
        }

        /// <summary>
        ///     begin expression
        /// </summary>
        /// <param name="declaration"></param>
        public void BeginExpression(IExpressionTarget declaration) {
            if (CurrentExpressionScope.Count > 0) {
                CurrentExpressionScope.Push(declaration);
            }
            else {
                CurrentExpressionScope.Push(declaration);
            }
        }

        /// <summary>
        ///     define a type value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public T DefineTypeValue<T>(ISyntaxPart parent) where T : TypeSpecificationBase, new() {
            var node = CreateNode<T>(LastTypeDeclaration, parent);
            DefineTypeValue(node);
            return node;
        }

    }
}