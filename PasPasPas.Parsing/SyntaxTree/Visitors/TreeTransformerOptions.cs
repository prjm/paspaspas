using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using System;
using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     stack entry
    /// </summary>
    public class WorkingStackEntry {
        private ISyntaxPart child;
        private ISyntaxPart parent;
        private AbstractSyntaxPart visitResult;

        /// <summary>
        ///     create a new entry
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <param name="visitResult"></param>
        public WorkingStackEntry(ISyntaxPart parent, ISyntaxPart child, AbstractSyntaxPart visitResult) {
            this.parent = parent;
            this.child = child;
            this.visitResult = visitResult;
        }

        /// <summary>
        ///     data
        /// </summary>
        public AbstractSyntaxPart Data
            => visitResult;

        /// <summary>
        ///     defining node
        /// </summary>
        public ISyntaxPart DefiningNode
            => parent;

        /// <summary>
        ///     defining child node
        /// </summary>
        public ISyntaxPart ChildNode
             => child;
    }

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
        ///     working stack for tree transformations
        /// </summary>
        public Stack<WorkingStackEntry> WorkingStack { get; }
            = new Stack<WorkingStackEntry>();

        /// <summary>
        ///     const declaration mode
        /// </summary>
        public DeclarationMode CurrentDeclarationMode { get; internal set; }

        /// <summary>
        ///     last expression
        /// </summary>
        public IExpressionTarget LastExpression
        {
            get
            {
                if (WorkingStack.Count > 0)
                    return WorkingStack.Peek().Data as IExpressionTarget;
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
                if (WorkingStack.Count > 0)
                    return WorkingStack.Peek().Data as ITypeTarget;
                else
                    return null;
            }
        }

        /// <summary>
        ///     last value from the working stack
        /// </summary>
        public ISyntaxPart LastValue
        {
            get
            {
                if (WorkingStack.Count > 0)
                    return WorkingStack.Peek().Data;
                else
                    return null;
            }
        }

        /// <summary>
        ///     define an expression value
        /// </summary>
        /// <param name="value"></param>
        public void DefineExpressionValue(ExpressionBase value) {
            if (WorkingStack.Count > 0) {
                var lastExpression = WorkingStack.Peek().Data as IExpressionTarget;
                if (lastExpression != null) {
                    lastExpression.Value = value;
                    return;
                }
            }
            // error ??
        }

        /// <summary>
        ///     define an expression value
        /// </summary>
        /// <param name="value"></param>
        public void DefineTypeValue(ITypeSpecification value) {
            if (WorkingStack.Count > 0) {
                var typeTarget = WorkingStack.Peek().Data as ITypeTarget;
                if (typeTarget != null) {
                    typeTarget.TypeValue = value;
                    return;
                }
            }
            // error ??
        }

        /// <summary>
        ///     define a tree node
        /// </summary>
        /// <typeparam name="T">object type to declare</typeparam>
        /// <param name="parentItem"></param>
        /// <returns></returns>
        public T Define<T>(ISyntaxPart parentItem) where T : AbstractSyntaxPart, new() {
            if (WorkingStack.Count > 0) {
                var scope = WorkingStack.Peek();
                T declaration = CreateNode<T>(scope, parentItem);
                return declaration;
            }
            else {
                T declaration = CreateNode<T>(null, parentItem);
                return declaration;
            }
        }

        private static T CreateNode<T>(object parent, ISyntaxPart element) where T : new() {
            var result = new T();
            return result;
        }

        /// <summary>
        ///     add entry to symbol table
        /// </summary>
        /// <param name="declaration"></param>
        public void AddSymbolTableEntry<T>(T declaration) where T : SymbolTableEntryBase {
            if (WorkingStack.Count > 0) {
                var scope = WorkingStack.Peek().Data as SymbolTableBase<T>;
                if (scope != null) {
                    scope.Add(declaration, LogSource);
                    return;
                }
            }
            // error ??            
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