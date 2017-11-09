﻿using System;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Typings.Operators;
using PasPasPas.Typings.Simple;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     visitor to annotate typs
    /// </summary>
    public class TypeAnnotator :

        IEndVisitor<ConstantValue>,
        IEndVisitor<UnaryOperator>,
        IEndVisitor<BinaryOperator>,
        IEndVisitor<VariableDeclaration>,
        IEndVisitor<Parsing.SyntaxTree.Abstract.TypeAlias>,
        IEndVisitor<MetaType>,
        IEndVisitor<SymbolReference>,
        IStartVisitor<CompilationUnit>,
        IEndVisitor<CompilationUnit> {

        private readonly IStartEndVisitor visitor;
        private readonly ITypedEnvironment environment;
        private readonly IDictionary<string, int> localTypes;

        /// <summary>
        ///     current unit
        /// </summary>
        public CompilationUnit CurrentUnit { get; private set; }

        /// <summary>
        ///     as common visitor
        /// </summary>
        /// <returns></returns>
        public IStartEndVisitor AsVisitor() =>
            visitor;

        /// <summary>
        ///     create a new type annotator
        /// </summary>
        /// <param name="env">typed environment</param>
        public TypeAnnotator(ITypedEnvironment env) {
            visitor = new Visitor(this);
            environment = env;
            localTypes = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     determine the type of a constant value
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ConstantValue element) {

            if (element.Kind == ConstantValueKind.HexNumber ||
                element.Kind == ConstantValueKind.Integer ||
                element.Kind == ConstantValueKind.QuotedString ||
                element.Kind == ConstantValueKind.RealNumber ||
                element.Kind == ConstantValueKind.True ||
                element.Kind == ConstantValueKind.False) {
                var typeId = LiteralValues.GetTypeFor(element.LiteralValue);
                element.TypeInfo = environment.TypeRegistry.GetTypeByIdOrUndefinedType(typeId);
            }
        }

        /// <summary>
        ///     annotate binary operators
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(BinaryOperator element) {
            if (element.Kind == ExpressionKind.And) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.AndOperation, element.LeftOperand?.TypeInfo, element.RightOperand?.TypeInfo);
            }
            else if (element.Kind == ExpressionKind.Or) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.OrOperation, element.LeftOperand?.TypeInfo, element.RightOperand?.TypeInfo);
            }
            else if (element.Kind == ExpressionKind.Xor) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.XorOperation, element.LeftOperand?.TypeInfo, element.RightOperand?.TypeInfo);
            }
            else if (element.Kind == ExpressionKind.Plus) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.PlusOperation, element.LeftOperand?.TypeInfo, element.RightOperand?.TypeInfo);
            }
            else if (element.Kind == ExpressionKind.Minus) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.MinusOperation, element.LeftOperand?.TypeInfo, element.RightOperand?.TypeInfo);
            }
            else if (element.Kind == ExpressionKind.Times) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.TimesOperation, element.LeftOperand?.TypeInfo, element.RightOperand?.TypeInfo);
            }
            else if (element.Kind == ExpressionKind.Div) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.DivOperation, element.LeftOperand?.TypeInfo, element.RightOperand?.TypeInfo);
            }
            else if (element.Kind == ExpressionKind.Mod) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.ModOperation, element.LeftOperand?.TypeInfo, element.RightOperand?.TypeInfo);
            }
            else if (element.Kind == ExpressionKind.Slash) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.SlashOperation, element.LeftOperand?.TypeInfo, element.RightOperand?.TypeInfo);
            }
            else if (element.Kind == ExpressionKind.Shl) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.ShlOperation, element.LeftOperand?.TypeInfo, element.RightOperand?.TypeInfo);
            }
            else if (element.Kind == ExpressionKind.Shr) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.ShrOperation, element.LeftOperand?.TypeInfo, element.RightOperand?.TypeInfo);
            }
            else if (element.Kind == ExpressionKind.EqualsSign) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.EqualsOperator, element.LeftOperand?.TypeInfo, element.RightOperand?.TypeInfo);
            }
            else if (element.Kind == ExpressionKind.NotEquals) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.NotEqualsOperator, element.LeftOperand?.TypeInfo, element.RightOperand?.TypeInfo);
            }
            else if (element.Kind == ExpressionKind.GreaterThen) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.GreaterThen, element.LeftOperand?.TypeInfo, element.RightOperand?.TypeInfo);
            }
            else if (element.Kind == ExpressionKind.GreaterThenEquals) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.GreaterThenEqual, element.LeftOperand?.TypeInfo, element.RightOperand?.TypeInfo);
            }
            else if (element.Kind == ExpressionKind.LessThen) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.LessThen, element.LeftOperand?.TypeInfo, element.RightOperand?.TypeInfo);
            }
            else if (element.Kind == ExpressionKind.LessThenEquals) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.LessThenOrEqual, element.LeftOperand?.TypeInfo, element.RightOperand?.TypeInfo);
            }
        }

        /// <summary>
        ///     determine the type of an unary operator
        /// </summary>
        /// <param name="element">operator to determine the type of</param>
        public void EndVisit(UnaryOperator element) {
            if (element.Kind == ExpressionKind.Not) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.NotOperation, element.Value?.TypeInfo);
            }
            else if (element.Kind == ExpressionKind.UnaryMinus) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.UnaryMinus, element.Value?.TypeInfo);
            }
            else if (element.Kind == ExpressionKind.UnaryPlus) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.UnaryPlus, element.Value?.TypeInfo);
            }
        }

        /// <summary>
        ///     gets the type of a given operator
        /// </summary>
        /// <param name="operatorKind"></param>
        /// <param name="typeInfo"></param>
        /// <returns></returns>
        private ITypeDefinition GetTypeOfOperator(int operatorKind, ITypeDefinition typeInfo) {
            if (typeInfo == null)
                return null;

            var operation = environment.TypeRegistry.GetOperator(operatorKind);

            if (operation == null)
                return null;

            var signature = new Signature(typeInfo.TypeId);
            var typeId = operation.GetOutputTypeForOperation(signature);
            return environment.TypeRegistry.GetTypeByIdOrUndefinedType(typeId);
        }

        /// <summary>
        ///     gets the type of a given binary operator
        /// </summary>
        /// <param name="operatorKind"></param>
        /// <param name="typeInfo1"></param>
        /// <param name="typeInfo2"></param>
        /// <returns></returns>
        private ITypeDefinition GetTypeOfOperator(int operatorKind, ITypeDefinition typeInfo1, ITypeDefinition typeInfo2) {
            if (typeInfo1 == null)
                return null;

            if (typeInfo2 == null)
                return null;

            var operation = environment.TypeRegistry.GetOperator(operatorKind);

            if (operation == null)
                return null;

            var signature = new Signature(typeInfo1.TypeId, typeInfo2.TypeId);
            var typeId = operation.GetOutputTypeForOperation(signature);
            return environment.TypeRegistry.GetTypeByIdOrUndefinedType(typeId);
        }

        /// <summary>
        ///     visit a variable declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(VariableDeclaration element) {
            if (element.TypeValue is ITypedSyntaxNode typeRef)
                element.TypeInfo = typeRef.TypeInfo;
        }

        /// <summary>
        ///     visit a variable declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(Parsing.SyntaxTree.Abstract.TypeAlias element) {
            var typeName = element.CompleteTypeName;

            if (typeName == default(ScopedName)) {
                element.TypeInfo = environment.TypeRegistry.GetTypeByIdOrUndefinedType(TypeIds.ErrorType);
                return;
            }

            if (typeName.Length == 1 && localTypes.ContainsKey(typeName[0])) {
                element.TypeInfo = environment.TypeRegistry.GetTypeByIdOrUndefinedType(localTypes[typeName[0]]);
                return;
            }

            element.TypeInfo = environment.TypeRegistry.GetTypeByNameOrUndefinedType(typeName);
        }

        /// <summary>
        ///     visit a meta type reference
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(MetaType element) {
            if (element.Kind == MetaTypeKind.NamedType && element.Fragments.Count == 1) {
                var name = element.Fragments[0].Name;
                if (CurrentUnit.Symbols.Contains(name.CompleteName)) {
                    var item = CurrentUnit.Symbols[name.CompleteName];
                    if (item.ParentItem is ITypedSyntaxNode typeInfo) {
                        element.TypeInfo = typeInfo.TypeInfo;
                    }
                }
            }
        }

        /// <summary>
        ///     begin visit a unit
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(CompilationUnit element) {
            CurrentUnit = element;
            localTypes.Clear();
            foreach (var entry in environment.TypeRegistry.RegisteredTypes)
                if (entry.TypeName != null)
                    localTypes.Add(entry.TypeName[entry.TypeName.Length - 1], entry.TypeId);
        }

        /// <summary>
        ///     end visiting a unit
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(CompilationUnit element) {
            localTypes.Clear();
            CurrentUnit = null;
        }

        /// <summary>
        ///     end visiting a symbol reference
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(SymbolReference element) {
            if (element.TypeValue is ITypedSyntaxNode typeRef)
                element.TypeInfo = typeRef.TypeInfo;
        }


    }
}
