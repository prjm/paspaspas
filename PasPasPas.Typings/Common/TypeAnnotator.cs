using System;
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
        IEndVisitor<CompilationUnit>,
        IStartVisitor<EnumType>, IEndVisitor<EnumType>,
        IEndVisitor<EnumTypeValue>,
        IEndVisitor<Parsing.SyntaxTree.Abstract.SubrangeType>,
        IEndVisitor<TypeDeclaration>,
        IEndVisitor<SetTypeDeclaration>,
        IEndVisitor<ArrayTypeDeclaration>,
        IStartVisitor<StructuredType>,
        IEndVisitor<StructuredType>,
        IStartVisitor<MethodDeclaration>, IEndVisitor<MethodDeclaration>,
        IEndVisitor<ParameterTypeDefinition> {

        private readonly IStartEndVisitor visitor;
        private readonly ITypedEnvironment environment;
        private readonly Stack<ITypeDefinition> currentTypeDefintion;
        private Scope scope;

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
            scope = new Scope(null);
            currentTypeDefintion = new Stack<ITypeDefinition>();
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

                var left = element.LeftOperand?.TypeInfo?.TypeKind;
                var right = element.RightOperand?.TypeInfo?.TypeKind;

                if (left.HasValue && right.HasValue) {
                    if (left.Value.Textual() && right.Value.Textual())
                        element.TypeInfo = GetTypeOfOperator(DefinedOperators.ConcatOperation, element.LeftOperand?.TypeInfo, element.RightOperand?.TypeInfo);
                    else if (left.Value.Numerical() && right.Value.Numerical())
                        element.TypeInfo = GetTypeOfOperator(DefinedOperators.PlusOperation, element.LeftOperand?.TypeInfo, element.RightOperand?.TypeInfo);
                }
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
            else if (element.Kind == ExpressionKind.RangeOperator) {
                var left = element.LeftOperand?.TypeInfo?.TypeKind;
                var right = element.RightOperand?.TypeInfo?.TypeKind;
                var leftId = element.LeftOperand?.TypeInfo?.TypeId;
                var rightId = element.RightOperand?.TypeInfo?.TypeId;

                if (left.Ordinal() && right.Ordinal()) {
                    if (left.Integral() && right.Integral())
                        element.TypeInfo = environment.TypeRegistry.GetTypeByIdOrUndefinedType(environment.TypeRegistry.GetSmallestIntegralTypeOrNext(leftId.Value, rightId.Value));
                    else if (leftId.HasValue && rightId.HasValue && leftId.Value == rightId.Value)
                        element.TypeInfo = environment.TypeRegistry.GetTypeByIdOrUndefinedType(leftId.Value);
                }
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
            if (element.TypeValue is ITypedSyntaxNode typeRef && typeRef.TypeInfo != null)
                element.TypeInfo = typeRef.TypeInfo;
            else
                element.TypeInfo = environment.TypeRegistry.GetTypeByIdOrUndefinedType(TypeIds.ErrorType);

            foreach (var vardef in element.Names) {
                element.TypeInfo.ProvideScope(vardef.Name.CompleteName, scope);
            }
        }

        /// <summary>
        ///     visit a variable declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(Parsing.SyntaxTree.Abstract.TypeAlias element) {
            var typeName = element.AsScopedName;

            if (typeName == default(ScopedName)) {
                element.TypeInfo = environment.TypeRegistry.GetTypeByIdOrUndefinedType(TypeIds.ErrorType);
                return;
            }

            var entry = scope.ResolveName(typeName);

            if (entry != null && entry.Kind == ScopeEntryKind.TypeName) {
                element.TypeInfo = environment.TypeRegistry.GetTypeByIdOrUndefinedType(entry.TypeId);
            }
        }

        /// <summary>
        ///     visit a meta type reference
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(MetaType element) {
            if (element.Kind == MetaTypeKind.NamedType) {
                var name = element.AsScopedName;
                var entry = scope.ResolveName(name);
                if (entry != null && (entry.Kind == ScopeEntryKind.TypeName || entry.Kind == ScopeEntryKind.VariableReference)) {
                    element.TypeInfo = environment.TypeRegistry.GetTypeByIdOrUndefinedType(entry.TypeId);
                }
            }

            else if (element.Kind == MetaTypeKind.String) {
                element.TypeInfo = environment.TypeRegistry.GetTypeByIdOrUndefinedType(TypeIds.StringType);
            }

            else if (element.Kind == MetaTypeKind.AnsiString) {
                element.TypeInfo = environment.TypeRegistry.GetTypeByIdOrUndefinedType(TypeIds.AnsiStringType);
            }

            else if (element.Kind == MetaTypeKind.ShortString) {
                element.TypeInfo = environment.TypeRegistry.GetTypeByIdOrUndefinedType(TypeIds.ShortStringType);
            }

            else if (element.Kind == MetaTypeKind.UnicodeString) {
                element.TypeInfo = environment.TypeRegistry.GetTypeByIdOrUndefinedType(TypeIds.UnicodeStringType);
            }

            else if (element.Kind == MetaTypeKind.WideString) {
                element.TypeInfo = environment.TypeRegistry.GetTypeByIdOrUndefinedType(TypeIds.WideStringType);
            }

            else if (element.Kind == MetaTypeKind.Pointer) {
                element.TypeInfo = environment.TypeRegistry.GetTypeByIdOrUndefinedType(TypeIds.GenericPointer);
            }

        }

        /// <summary>
        ///     begin visit a unit
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(CompilationUnit element) {
            CurrentUnit = element;
            scope = scope.Open(element.UnitName.CompleteName);
            foreach (var entry in environment.TypeRegistry.RegisteredTypes)
                if (entry.TypeName != null) {
                    scope.AddEntry(entry.TypeName, new ScopeEntry(ScopeEntryKind.TypeName) { TypeId = entry.TypeId });
                    scope.AddEntry(new ScopedName(entry.TypeName[entry.TypeName.Length - 1]), new ScopeEntry(ScopeEntryKind.TypeName) { TypeId = entry.TypeId });
                }
        }

        /// <summary>
        ///     end visiting a unit
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(CompilationUnit element) {
            scope = scope.Close(element.UnitName.CompleteName);
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

        /// <summary>
        ///     start visting an enumeration type
        /// </summary>
        /// <param name="element">enumeration type definition</param>
        public void StartVisit(EnumType element) {
            var typeId = RequireUserTypeId();
            var typeDef = new EnumeratedType(typeId);
            RegisterUserDefinedType(typeDef);
            currentTypeDefintion.Push(typeDef);
        }

        /// <summary>
        ///     register a new type definition
        /// </summary>
        /// <param name="typeDef"></param>
        private void RegisterUserDefinedType(ITypeDefinition typeDef)
            => environment.TypeRegistry.RegisterType(typeDef);

        /// <summary>
        ///     require a new type id for a user defined type
        /// </summary>
        /// <returns></returns>
        private int RequireUserTypeId()
            => environment.TypeRegistry.RequireUserTypeId();

        /// <summary>
        ///     end visiting an enum type definition
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(EnumType element) {
            var typeDef = currentTypeDefintion.Pop();
            element.TypeInfo = typeDef;
        }

        /// <summary>
        ///     enum type value definition
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(EnumTypeValue element) {
            var typeDef = currentTypeDefintion.Peek() as EnumeratedType;
            if (typeDef == null)
                return;

            typeDef.DefineEnumValue(element.SymbolName, false, -1);
        }

        /// <summary>
        ///     visit a subrange type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(Parsing.SyntaxTree.Abstract.SubrangeType element) {

            var left = element.RangeStart?.TypeInfo?.TypeKind;
            var right = element.RangeEnd?.TypeInfo?.TypeKind;
            var typeId = RequireUserTypeId();

            if (left.HasValue && element.RangeEnd == null) {
                element.TypeInfo = new Simple.SubrangeType(typeId, element.RangeStart.TypeInfo.TypeId);
                return;
            }

            if (left.HasValue && right.HasValue) {

                if (left.Value.Integral() && right.Value.Integral()) {
                    var baseTypeId = environment.TypeRegistry.GetSmallestIntegralTypeOrNext(element.RangeStart.TypeInfo.TypeId, element.RangeEnd.TypeInfo.TypeId);
                    element.TypeInfo = new Simple.SubrangeType(typeId, baseTypeId);
                    environment.TypeRegistry.RegisterType(element.TypeInfo);
                    return;
                }

                if (left.Value == CommonTypeKind.WideCharType && right.Value == CommonTypeKind.WideCharType &&
                    element.RangeStart.TypeInfo.TypeId == element.RangeEnd.TypeInfo.TypeId) {
                    var baseTypeId = element.RangeStart.TypeInfo.TypeId;
                    element.TypeInfo = new Simple.SubrangeType(typeId, baseTypeId);
                    environment.TypeRegistry.RegisterType(element.TypeInfo);
                    return;
                }

                if (left.Value == CommonTypeKind.EnumerationType && right.Value == CommonTypeKind.EnumerationType &&
                    element.RangeStart.TypeInfo.TypeId == element.RangeEnd.TypeInfo.TypeId) {
                    var baseTypeId = element.RangeStart.TypeInfo.TypeId;
                    element.TypeInfo = new Simple.SubrangeType(typeId, baseTypeId);
                    environment.TypeRegistry.RegisterType(element.TypeInfo);
                    return;
                }

            }

            element.TypeInfo = environment.TypeRegistry.GetTypeByIdOrUndefinedType(TypeIds.ErrorType);
        }

        /// <summary>
        ///     declare a type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(TypeDeclaration element) {

            if (element.TypeValue is ITypedSyntaxNode declaredType && declaredType.TypeInfo != null) {
                scope.AddEntry(new ScopedName(element.Name.CompleteName), new ScopeEntry(ScopeEntryKind.TypeName) { TypeId = declaredType.TypeInfo.TypeId });
            }
        }

        /// <summary>
        ///     declare a set type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(SetTypeDeclaration element) {

            if (element.TypeValue is ITypedSyntaxNode declaredEnum && declaredEnum.TypeInfo != null && declaredEnum.TypeInfo.TypeKind.Ordinal()) {
                var typeId = RequireUserTypeId();
                var setType = new SetType(typeId, declaredEnum.TypeInfo.TypeId);
                RegisterUserDefinedType(setType);
                element.TypeInfo = setType;
                return;
            }

            element.TypeInfo = environment.TypeRegistry.GetTypeByIdOrUndefinedType(TypeIds.ErrorType);
        }

        /// <summary>
        ///     visit an array declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ArrayTypeDeclaration element) {
            var typeId = RequireUserTypeId();
            var typeDef = new ArrayType(typeId);

            if (element.TypeValue != null && element.TypeValue.TypeInfo != null)
                typeDef.BaseType = element.TypeValue.TypeInfo;
            else
                typeDef.BaseType = environment.TypeRegistry.GetTypeByIdOrUndefinedType(TypeIds.ErrorType);

            foreach (var indexDef in element.IndexItems) {
                if (indexDef.TypeInfo != null)
                    typeDef.IndexTypes.Add(indexDef.TypeInfo);
                else
                    typeDef.IndexTypes.Add(environment.TypeRegistry.GetTypeByIdOrUndefinedType(TypeIds.ErrorType));
            }

            RegisterUserDefinedType(typeDef);
            element.TypeInfo = typeDef;
        }

        /// <summary>
        ///     start visting a structured type
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(StructuredType element) {
            var typeId = RequireUserTypeId();
            var typeDef = new StructuredTypeDeclaration(typeId, element.Kind);
            RegisterUserDefinedType(typeDef);
            typeDef.BaseClass = environment.TypeRegistry.GetTypeByIdOrUndefinedType(TypeIds.TObject);

            currentTypeDefintion.Push(typeDef);
        }

        /// <summary>
        ///     end visiting a structured type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(StructuredType element) {
            var typeDef = currentTypeDefintion.Pop();
            element.TypeInfo = typeDef;
        }

        /// <summary>
        ///     start visting a method declaration
        /// </summary>ele
        /// <param name="element"></param>
        public void StartVisit(MethodDeclaration element) {
            var typeDef = currentTypeDefintion.Peek() as StructuredTypeDeclaration;
            var method = typeDef.AddOrExtendMethod(element.Name.CompleteName);
            method.AddParameterGroup();
        }

        /// <summary>
        ///     end visting a method declaration
        /// </summary>ele
        /// <param name="element"></param>
        public void EndVisit(MethodDeclaration element) {
            if (element.Kind == ProcedureKind.Function) {
                var typeDef = currentTypeDefintion.Peek() as StructuredTypeDeclaration;
                var method = typeDef.Methods[typeDef.Methods.Count - 1];
                var methodParams = method.Parameters[method.Parameters.Count - 1];

                if (element.TypeValue != null && element.TypeValue.TypeInfo != null)
                    methodParams.ResultType = element.TypeValue.TypeInfo;
                else
                    methodParams.ResultType = environment.TypeRegistry.GetTypeByIdOrUndefinedType(TypeIds.ErrorType);
            }
        }

        /// <summary>
        ///     visit a paramer type definition
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ParameterTypeDefinition element) {
            if (element.TypeValue != null && element.TypeValue.TypeInfo != null) {
                var typeDef = currentTypeDefintion.Peek() as StructuredTypeDeclaration;
                var method = typeDef.Methods[typeDef.Methods.Count - 1];
                var parms = method.Parameters[method.Parameters.Count - 1];

                foreach (var name in element.Parameters) {
                    var param = parms.AddParameter(name.Name.CompleteName);
                    param.ParamType = element.TypeValue.TypeInfo;
                }
            }
        }
    }
}
