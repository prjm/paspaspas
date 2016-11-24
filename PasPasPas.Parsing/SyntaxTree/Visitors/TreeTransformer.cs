using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Standard;
using PasPasPas.Parsing.Parser;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     convert a concrete syntax tree to an abstract one
    /// </summary>
    public class TreeTransformer : SyntaxPartVisitorBase<TreeTransformerOptions> {

        #region Unit

        private void BeginVisitItem(Unit unit, TreeTransformerOptions parameter) {
            var result = CreateTreeNode<CompilationUnit>(null, unit);
            result.FileType = CompilationUnitType.Unit;
            result.UnitName = ExtractSymbolName(result, unit.UnitName);
            result.Hints = ExtractHints(result, unit.Hints);
            result.FilePath = unit.FilePath;
            parameter.Project.Add(result, parameter.LogSource);
            parameter.CurrentUnit = result;
        }

        private void EndVisitItem(Unit unit, TreeTransformerOptions parameter) {
            parameter.CurrentUnit = null;
        }

        #endregion
        #region Library

        private void BeginVisitItem(Library library, TreeTransformerOptions parameter) {
            var result = CreateTreeNode<CompilationUnit>(null, library);
            result.FileType = CompilationUnitType.Library;
            result.UnitName = ExtractSymbolName(result, library.LibraryName);
            result.Hints = ExtractHints(result, library.Hints);
            result.FilePath = library.FilePath;
            parameter.Project.Add(result, parameter.LogSource);
            parameter.CurrentUnitMode = UnitMode.Library;
            parameter.CurrentUnit = result;
        }

        private void EndVisitItem(Library library, TreeTransformerOptions parameter) {
            parameter.CurrentUnit = null;
            parameter.CurrentUnitMode = UnitMode.Unknown;
        }

        #endregion
        #region Program

        /// <summary>
        ///     visit a program
        /// </summary>
        /// <param name="program"></param>
        /// <param name="parameter"></param>
        private void BeginVisitItem(Program program, TreeTransformerOptions parameter) {
            var result = CreateTreeNode<CompilationUnit>(null, program);
            result.FileType = CompilationUnitType.Program;
            result.UnitName = ExtractSymbolName(result, program.ProgramName);
            result.FilePath = program.FilePath;
            parameter.CurrentUnitMode = UnitMode.Program;
            parameter.Project.Add(result, parameter.LogSource);
            parameter.CurrentUnit = result;
        }

        private void EndVisitItem(Program program, TreeTransformerOptions parameter) {
            parameter.CurrentUnit = null;
            parameter.CurrentUnitMode = UnitMode.Unknown;
        }

        #endregion
        #region Package

        private void BeginVisitItem(Package package, TreeTransformerOptions parameter) {
            var result = CreateTreeNode<CompilationUnit>(null, package);
            result.FileType = CompilationUnitType.Package;
            result.UnitName = ExtractSymbolName(result, package.PackageName);
            result.FilePath = package.FilePath;
            parameter.Project.Add(result, parameter.LogSource);
            parameter.CurrentUnit = result;
        }

        private void EndVisitItem(Package package, TreeTransformerOptions parameter) {
            parameter.CurrentUnit = null;
        }

        #endregion
        #region UnitInterface

        private void BeginVisitItem(UnitInterface unitInterface, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode = UnitMode.Interface;
            parameter.BeginDeclare(parameter.CurrentUnit.InterfaceSymbols);
        }


        private void EndVisitItem(UnitInterface unitInterface, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode = UnitMode.Unknown;
            parameter.EndDeclare(parameter.CurrentUnit.InterfaceSymbols);
        }

        #endregion
        #region UnitImplementation

        private void BeginVisitItem(UnitImplementation unitImplementation, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode = UnitMode.Implementation;
            parameter.BeginDeclare(parameter.CurrentUnit.ImplementationSymbols);
        }

        private void EndVisitItem(UnitImplementation unit, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode = UnitMode.Unknown;
            parameter.EndDeclare(parameter.CurrentUnit.ImplementationSymbols);
        }

        #endregion
        #region ConstSection

        private void BeginVisitItem(ConstSection constSection, TreeTransformerOptions parameter) {
            if (constSection.Kind == TokenKind.Const) {
                parameter.CurrentDeclarationMode = DeclarationMode.Const;
            }
            else if (constSection.Kind == TokenKind.Resourcestring) {
                parameter.CurrentDeclarationMode = DeclarationMode.ResourceString;
            }
        }

        private void EndVisitItem(ConstSection constSection, TreeTransformerOptions parameter) {
            parameter.CurrentDeclarationMode = DeclarationMode.Unknown;
        }

        #endregion
        #region TypeSection

        private void BeginVisitItem(TypeSection constSection, TreeTransformerOptions parameter) {
            parameter.CurrentDeclarationMode = DeclarationMode.Types;
        }

        private void EndVisitItem(TypeSection constSection, TreeTransformerOptions parameter) {
            parameter.CurrentDeclarationMode = DeclarationMode.Unknown;
        }

        #endregion
        #region TypeDeclaration

        private void BeginVisitItem(Standard.TypeDeclaration typeDeclaration, TreeTransformerOptions parameter) {
            var declaration = parameter.Declare<Abstract.TypeDeclaration>(typeDeclaration);
            declaration.Name = ExtractSymbolName(declaration, typeDeclaration.TypeId?.Identifier);
            declaration.Generics = ExtractGenericDefinition(declaration, typeDeclaration.TypeId?.GenericDefinition, parameter);
            declaration.Attributes = ExtractAttributes(declaration, typeDeclaration.Attributes, parameter.CurrentUnit);
            declaration.Hints = ExtractHints(typeDeclaration, typeDeclaration.Hint);
            parameter.CompleteDeclaration(declaration);
            parameter.BeginTypeSpecification(declaration);
        }

        private void EndVisitItem(Standard.TypeDeclaration typeDeclaration, TreeTransformerOptions parameter) {
            parameter.EndTypeSpecification();
        }

        #endregion
        #region ConstDeclaration

        private void BeginVisitItem(ConstDeclaration constDeclaration, TreeTransformerOptions parameter) {
            var declaration = parameter.Declare<ConstantDeclaration>(constDeclaration);
            declaration.Name = ExtractSymbolName(declaration, constDeclaration.Identifier);
            declaration.Mode = parameter.CurrentDeclarationMode;
            declaration.Attributes = ExtractAttributes(declaration, constDeclaration.Attributes, parameter.CurrentUnit);
            declaration.Hints = ExtractHints(constDeclaration, constDeclaration.Hint);
            parameter.CompleteDeclaration(declaration);
            parameter.BeginExpression(declaration);
            parameter.BeginTypeSpecification(declaration);
        }

        private void EndVisitItem(ConstDeclaration constDeclaration, TreeTransformerOptions parameter) {
            parameter.EndExpression();
            parameter.EndTypeSpecification();
        }

        #endregion
        #region ConstantExpression

        private void BeginVisitItem(ConstantExpression constExpression, TreeTransformerOptions parameter) {

            if (constExpression.IsArrayConstant) {
                parameter.DefineExpressionValue<ArrayConstant>(constExpression);
            }

            if (constExpression.IsRecordConstant) {
                parameter.DefineExpressionValue<RecordConstant>(constExpression);
            }
        }

        private void EndVisitItem(ConstantExpression constExpression, TreeTransformerOptions parameter) {

            if (constExpression.IsArrayConstant || constExpression.IsRecordConstant) {
                parameter.CompleteExpression();
            }
        }

        #endregion
        #region RecordConstantExpression

        private void BeginVisitItem(RecordConstantExpression constExpression, TreeTransformerOptions parameter) {
            var expression = parameter.DefineExpressionValue<RecordConstantItem>(constExpression);
            expression.Name = ExtractSymbolName(constExpression, constExpression.Name);
        }

        private void EndVisitItem(RecordConstantExpression constExpression, TreeTransformerOptions parameter) {
            parameter.CompleteExpression();
        }

        #endregion
        #region Expression

        private void BeginVisitItem(Expression expression, TreeTransformerOptions parameter) {
            if (expression.LeftOperand != null && expression.RightOperand != null) {
                var currentExpression = parameter.DefineExpressionValue<BinaryOperator>(expression);
                currentExpression.Kind = BinaryOperator.ConvertKind(expression.Kind);
            }
        }

        private void EndVisitItem(Expression expression, TreeTransformerOptions parameter) {
            if (expression.LeftOperand != null && expression.RightOperand != null) {
                parameter.CompleteExpression();
            }
        }

        #endregion
        #region SimpleExpression

        private void BeginVisitItem(SimpleExpression simpleExpression, TreeTransformerOptions parameter) {
            if (simpleExpression.LeftOperand != null && simpleExpression.RightOperand != null) {
                var currentExpression = parameter.DefineExpressionValue<BinaryOperator>(simpleExpression);
                currentExpression.Kind = BinaryOperator.ConvertKind(simpleExpression.Kind);
            }
        }

        private void EndVisitItem(SimpleExpression simpleExpression, TreeTransformerOptions parameter) {
            if (simpleExpression.LeftOperand != null && simpleExpression.RightOperand != null) {
                parameter.CompleteExpression();
            }
        }

        #endregion       
        #region Term

        private void BeginVisitItem(Term term, TreeTransformerOptions parameter) {
            if (term.LeftOperand != null && term.RightOperand != null) {
                var currentExpression = parameter.DefineExpressionValue<BinaryOperator>(term);
                currentExpression.Kind = BinaryOperator.ConvertKind(term.Kind);
            }
        }

        private void EndVisitItem(Term term, TreeTransformerOptions parameter) {
            if (term.LeftOperand != null && term.RightOperand != null) {
                parameter.CompleteExpression();
            }
        }



        #endregion
        #region Factor

        private void BeginVisitItem(Factor factor, TreeTransformerOptions parameter) {

            // unary operators
            if (factor.AddressOf != null || factor.Not != null || factor.Plus != null || factor.Minus != null) {
                var value = parameter.DefineExpressionValue<UnaryOperator>(factor);
                if (factor.AddressOf != null)
                    value.Kind = ExpressionKind.AddressOf;
                else if (factor.Not != null)
                    value.Kind = ExpressionKind.Not;
                else if (factor.Plus != null)
                    value.Kind = ExpressionKind.UnaryPlus;
                else if (factor.Minus != null)
                    value.Kind = ExpressionKind.UnaryMinus;
                return;
            }

            // constant values
            if (factor.IsNil) {
                var value = CreateLeafNode<ConstantValue>(parameter.LastExpression, factor);
                value.Kind = ConstantValueKind.Nil;
                parameter.DefineExpressionValue(value);
                return;
            }

            if (factor.PointerTo != null) {
                var value = CreateLeafNode<VariableValue>(parameter.LastExpression, factor);
                value.Name = ExtractSymbolName(value, factor.PointerTo);
                parameter.DefineExpressionValue(value);
                return;
            }

            if (factor.IsFalse) {
                var value = CreateLeafNode<ConstantValue>(parameter.LastExpression, factor);
                value.Kind = ConstantValueKind.False;
                parameter.DefineExpressionValue(value);
            }
            else if (factor.IsTrue) {
                var value = CreateLeafNode<ConstantValue>(parameter.LastExpression, factor);
                value.Kind = ConstantValueKind.True;
                parameter.DefineExpressionValue(value);
            }
            else if (factor.IntValue != null) {

            }
            else if (factor.RealValue != null) {

            }
            if (factor.StringValue != null) {

            }
        }

        private void EndVisitItem(Factor factor, TreeTransformerOptions parameter) {
            if (factor.AddressOf != null || factor.Not != null || factor.Plus != null || factor.Minus != null) {
                parameter.CompleteExpression();
            }
        }

        #endregion
        #region UsesClause

        private void BeginVisitItem(UsesClause unit, TreeTransformerOptions parameter) {
            if (unit.UsesList == null)
                return;

            foreach (var part in unit.UsesList.Parts) {
                var name = part as NamespaceName;
                if (name == null)
                    continue;

                var unitName = CreateLeafNode<RequiredUnitName>(unit, part);
                unitName.Name = ExtractSymbolName(unitName, name);
                unitName.Mode = parameter.CurrentUnitMode;
                parameter.CurrentUnit.RequiredUnits.Add(unitName, parameter.LogSource);
            }
        }

        #endregion
        #region UsesFileClause

        private void BeginVisitItem(UsesFileClause unit, TreeTransformerOptions parameter) {
            if (unit.Files == null)
                return;

            foreach (var part in unit.Files.Parts) {
                var name = part as NamespaceFileName;
                if (name == null)
                    continue;

                var unitName = CreateLeafNode<RequiredUnitName>(parameter.CurrentUnit, part);
                unitName.Name = ExtractSymbolName(unitName, name);
                unitName.Mode = parameter.CurrentUnitMode;
                unitName.FileName = name.QuotedFileName?.UnquotedValue;
                parameter.CurrentUnit.RequiredUnits.Add(unitName, parameter.LogSource);
            }
        }

        #endregion
        #region PackageRequires

        private void BeginVisitItem(PackageRequires requires, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode = UnitMode.Requires;

            if (requires.RequiresList == null)
                return;

            foreach (var part in requires.RequiresList.Parts) {
                var name = part as NamespaceName;
                if (name == null)
                    continue;

                var unitName = CreateLeafNode<RequiredUnitName>(parameter.CurrentUnit, part);
                unitName.Name = ExtractSymbolName(unitName, name);
                unitName.Mode = parameter.CurrentUnitMode;
                parameter.CurrentUnit.RequiredUnits.Add(unitName, parameter.LogSource);
            }
        }

        private void EndVisitItem(PackageRequires requires, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode = UnitMode.Unknown;
        }

        #endregion
        #region PackageContains

        private void BeginVisitItem(PackageContains contains, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode = UnitMode.Contains;

            if (contains.ContainsList == null)
                return;

            foreach (var part in contains.ContainsList.Parts) {
                var name = part as NamespaceFileName;
                if (name == null)
                    continue;

                var unitName = CreateLeafNode<RequiredUnitName>(parameter.CurrentUnit, part);
                unitName.Name = ExtractSymbolName(unitName, name);
                unitName.Mode = parameter.CurrentUnitMode;
                unitName.FileName = name.QuotedFileName?.UnquotedValue;
                parameter.CurrentUnit.RequiredUnits.Add(unitName, parameter.LogSource);
            }
        }

        private void EndVisitItem(PackageContains contains, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode = UnitMode.Unknown;
        }

        #endregion
        #region StructType

        private void BeginVisitItem(StructType structType, TreeTransformerOptions parameter) {
            if (structType.Packed)
                parameter.CurrentStructTypeMode = StructTypeMode.Packed;
            else
                parameter.CurrentStructTypeMode = StructTypeMode.Unpacked;
        }

        private void EndVisitItem(StructType factor, TreeTransformerOptions parameter) {
            parameter.CurrentStructTypeMode = StructTypeMode.Undefined;
        }

        #endregion
        #region ArrayType

        private void BeginVisitItem(ArrayType array, TreeTransformerOptions parameter) {
            var value = CreateLeafNode<ArrayTypeDeclaration>(parameter.LastTypeDeclaration, array);
            value.PackedType = parameter.CurrentStructTypeMode == StructTypeMode.Packed;
            parameter.BeginExpression(value);
            parameter.DefineTypeValue(value);

            if (array.ArrayOfConst) {
                var metaType = parameter.DefineTypeValue<MetaType>(array);
                metaType.Kind = MetaTypeKind.Const;
            }
        }

        private void EndVisitItem(ArrayType array, TreeTransformerOptions parameter) {
            parameter.EndExpression();
        }

        #endregion
        #region SetDefinition

        private void BeginVisitItem(SetDefinition set, TreeTransformerOptions parameter) {
            var value = CreateLeafNode<SetTypeDeclaration>(parameter.LastTypeDeclaration, set);
            value.PackedType = parameter.CurrentStructTypeMode == StructTypeMode.Packed;
            parameter.DefineTypeValue(value);
        }

        #endregion
        #region FileTypeDefinition

        private void BeginVisitItem(FileType set, TreeTransformerOptions parameter) {
            var value = CreateLeafNode<FileTypeDeclaration>(parameter.LastTypeDeclaration, set);
            value.PackedType = parameter.CurrentStructTypeMode == StructTypeMode.Packed;
            parameter.DefineTypeValue(value);
        }

        #endregion
        #region ClassOf

        private void BeginVisitItem(ClassOfDeclaration classOf, TreeTransformerOptions parameter) {
            var value = CreateLeafNode<ClassOfTypeDeclaration>(parameter.LastTypeDeclaration, classOf);
            value.PackedType = parameter.CurrentStructTypeMode == StructTypeMode.Packed;
            parameter.DefineTypeValue(value);
        }

        #endregion
        #region TypeName

        private void BeginVisitItem(TypeName typeName, TreeTransformerOptions parameter) {
            var value = CreateLeafNode<MetaType>(parameter.LastTypeDeclaration, typeName);
            value.Kind = typeName.MapTypeKind();
            value.Name = ExtractSymbolName(value, typeName.NamedType);
            parameter.DefineTypeValue(value);
        }

        #endregion
        #region SimpleType

        private void BeginVisitItem(SimpleType simpleType, TreeTransformerOptions parameter) {
            if (simpleType.SubrangeStart != null) {
                var value = CreateLeafNode<SubrangeType>(parameter.LastTypeDeclaration, simpleType);
                parameter.BeginExpression(value);
                parameter.DefineTypeValue(value);
            }

            else {
                var value = CreateLeafNode<TypeAlias>(parameter.LastTypeDeclaration, simpleType);
                value.IsNewType = simpleType.NewType;

                if (simpleType.TypeOf)
                    parameter.LogSource.Warning(StructuralErrors.UnsupportedTypeOfConstruct, simpleType);

                parameter.DefineTypeValue(value);
            }
        }

        private void BeginVisitChildItem(SimpleType simpleType, TreeTransformerOptions parameter, ISyntaxPart part) {
            var name = part as GenericNamespaceName;
            var value = parameter.LastTypeDeclaration?.TypeValue as TypeAlias;

            if (name == null || value == null)
                return;

            var fragment = CreateLeafNode<GenericNameFragment>(value, name);
            fragment.Name = ExtractSymbolName(fragment, name.Name);
            value.AddFragment(fragment);
            if (name.GenericPart != null)
                parameter.BeginTypeSpecification(fragment);
        }

        private void EndVisitChildItem(SimpleType simpleType, TreeTransformerOptions parameter, ISyntaxPart part) {
            var name = part as GenericNamespaceName;
            if (name == null || name.GenericPart == null)
                return;
            parameter.EndTypeSpecification();
        }

        private void EndVisitItem(SimpleType simpleType, TreeTransformerOptions parameter) {
            if (simpleType.SubrangeStart != null) {
                parameter.CompleteExpression();
            }
        }

        #endregion
        #region EnumTypeDefinition

        private void BeginVisitItem(EnumTypeDefinition type, TreeTransformerOptions parameter) {
            var value = CreateLeafNode<EnumType>(parameter.LastTypeDeclaration, type);
            parameter.DefineTypeValue(value);
        }


        #endregion
        #region EnumValue

        private void BeginVisitItem(EnumValue enumValue, TreeTransformerOptions parameter) {
            var enumDeclaration = parameter.LastTypeDeclaration.TypeValue as EnumType;
            if (enumDeclaration != null) {
                var value = CreateLeafNode<EnumTypeValue>(enumDeclaration, enumValue);
                value.Name = ExtractSymbolName(value, enumValue.EnumName);
                enumDeclaration.Add(value, parameter.LogSource);
                if (enumValue.Value != null) {
                    parameter.BeginExpression(value);
                }
            }
        }

        private void EndVisitItem(EnumValue enumValue, TreeTransformerOptions parameter) {
            if (enumValue.Value != null) {
                parameter.CompleteExpression();
            }
        }

        #endregion
        #region ArrayIndex

        private void BeginVisitItem(ArrayIndex arrayIndex, TreeTransformerOptions parameter) {
            if (arrayIndex.EndIndex != null) {
                var binOp = parameter.DefineExpressionValue<BinaryOperator>(arrayIndex);
                binOp.Kind = ExpressionKind.RangeOperator;
            }
        }

        private void EndVisitItem(ArrayIndex arrayIndex, TreeTransformerOptions parameter) {
            if (arrayIndex.EndIndex != null) {
                parameter.EndExpression();
            }
        }

        #endregion
        #region PointerType

        private void BeginVisitItem(PointerType pointer, TreeTransformerOptions parameter) {
            if (pointer.GenericPointer) {
                var result = CreateLeafNode<MetaType>(parameter.CurrentTypeSpecificationScope, pointer);
                result.Kind = MetaTypeKind.Pointer;
                parameter.DefineTypeValue(result);
            }
            else {
                var result = CreateLeafNode<PointerToType>(parameter.CurrentTypeSpecificationScope, pointer);
                parameter.DefineTypeValue(result);
            }
        }

        private void EndVisitItem(PointerType pointer, TreeTransformerOptions parameter) {
            if (!pointer.GenericPointer)
                parameter.EndTypeSpecification();
        }

        #endregion

        #region Extractors

        private static SymbolName ExtractSymbolName(object parent, NamespaceName name) {
            var result = CreateLeafNode<SymbolName>(parent, name);
            result.Name = name?.Name;
            result.Namespace = name?.Namespace;
            return result;
        }

        private static SymbolName ExtractSymbolName(object parent, Identifier name) {
            var result = CreateLeafNode<SymbolName>(parent, name);
            result.Name = name.FirstTerminalToken?.Value;
            return result;
        }

        private static SymbolName ExtractSymbolName(object parent, NamespaceFileName name) {
            var result = CreateLeafNode<SymbolName>(parent, name);
            result.Name = name?.NamespaceName?.Name;
            result.Namespace = name?.NamespaceName?.Namespace;
            return result;
        }

        private GenericTypes ExtractGenericDefinition(object parent, GenericDefinition genericDefinition, TreeTransformerOptions parameter) {
            var result = CreateLeafNode<GenericTypes>(parent, genericDefinition);

            if (genericDefinition == null)
                return result;

            foreach (var part in genericDefinition.Parts) {
                var idPart = part as Identifier;

                if (idPart != null) {
                    var generic = CreateLeafNode<GenericType>(result, part);
                    generic.Name = ExtractSymbolName(genericDefinition, idPart);
                    result.Add(generic, parameter.LogSource);
                    continue;
                }

                var genericPart = part as GenericDefinitionPart;

                if (genericPart != null) {
                    var generic = CreateLeafNode<GenericType>(result, part);
                    generic.Name = ExtractSymbolName(genericDefinition, genericPart.Identifier);
                    result.Add(generic, parameter.LogSource);

                    foreach (var constraintPart in genericPart.Parts) {
                        var constraint = constraintPart as Standard.ConstrainedGeneric;
                        if (constraint != null) {
                            var cr = CreateLeafNode<GenericConstraint>(generic, constraint);
                            cr.Kind = GenericConstraint.MapKind(constraint);
                            cr.Name = ExtractSymbolName(cr, constraint.ConstraintIdentifier);
                            generic.Add(cr, parameter.LogSource);
                        }
                    }

                    continue;
                }
            }


            return result;
        }

        private static SymbolHints ExtractHints(object parent, HintingInformationList hints) {
            var result = CreateLeafNode<SymbolHints>(parent, hints);

            if (hints == null || hints.PartList.Count < 1)
                return result;

            foreach (var part in hints.Parts) {
                var hint = part as HintingInformation;
                if (hint == null) continue;
                result.SymbolIsDeprecated = result.SymbolIsDeprecated || hint.Deprecated;
                result.DeprecatedInformation = (result.DeprecatedInformation ?? string.Empty) + hint.DeprecatedComment?.UnquotedValue;
                result.SymbolInLibrary = result.SymbolInLibrary || hint.Library;
                result.SymbolIsPlatformSpecific = result.SymbolIsPlatformSpecific || hint.Platform;
                result.SymbolIsExperimental = result.SymbolIsExperimental || hint.Experimental;
            }

            return result;
        }

        private IEnumerable<SymbolAttribute> ExtractAttributes(object parent, UserAttributes attributes, CompilationUnit parentUnit) {
            if (attributes == null || attributes.PartList.Count < 1)
                return EmptyCollection<SymbolAttribute>.Instance;

            var result = new List<SymbolAttribute>();

            foreach (var part in attributes.Parts) {
                var attribute = part as UserAttributeDefinition;
                var isAssemblyAttribute = false;

                if (attribute == null) {
                    var assemblyAttribute = part as AssemblyAttributeDeclaration;
                    if (assemblyAttribute == null)
                        continue;

                    attribute = assemblyAttribute.Attribute;
                    isAssemblyAttribute = true;
                }

                var userAttribute = CreateLeafNode<SymbolAttribute>(parent, attribute);
                userAttribute.Name = ExtractSymbolName(userAttribute, attribute.Name);

                if (!isAssemblyAttribute)
                    result.Add(userAttribute);
                else
                    parentUnit.AddAssemblyAttribute(userAttribute);
            }

            return result;
        }



        #endregion
        #region Helper functions

        private static T CreateTreeNode<T>(ISyntaxPart parent, ISyntaxPart element) where T : ISyntaxPart, new() {
            var result = new T();
            result.Parent = parent;
            return result;
        }


        private static T CreateLeafNode<T>(object parent, ISyntaxPart element) where T : new() {
            var result = new T();
            return result;
        }

        /// <summary>
        ///     visit a syntax node
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool BeginVisit(ISyntaxPart syntaxPart, TreeTransformerOptions parameter) {
            dynamic part = syntaxPart;
            BeginVisitItem(part, parameter);
            return true;
        }

        /// <summary>
        ///     start visiting a child item
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="visitorParameter"></param>
        /// <param name="child"></param>
        public override void BeginVisitChild(ISyntaxPart parent, TreeTransformerOptions visitorParameter, ISyntaxPart child) {
            dynamic part = parent;
            BeginVisitChildItem(part, visitorParameter, child);
        }

        /// <summary>
        ///     end visiting a syntax node
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool EndVisit(ISyntaxPart syntaxPart, TreeTransformerOptions parameter) {
            dynamic part = syntaxPart;
            EndVisitItem(part, parameter);
            return true;
        }

        /// <summary>
        ///     end visiting a child
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="visitorParameter"></param>
        /// <param name="child"></param>
        public override void EndVisitChild(ISyntaxPart parent, TreeTransformerOptions visitorParameter, ISyntaxPart child) {
            dynamic part = parent;
            EndVisitChildItem(part, visitorParameter, child);
        }



        private void BeginVisitItem(ISyntaxPart part, TreeTransformerOptions parameter) {
            //..
        }

        private void BeginVisitChildItem(ISyntaxPart part, TreeTransformerOptions parameter, ISyntaxPart child) {
            //..
        }


        private void EndVisitItem(ISyntaxPart part, TreeTransformerOptions parameter) {
            //..
        }

        private void EndVisitChildItem(ISyntaxPart part, TreeTransformerOptions parameter, ISyntaxPart child) {
            //..
        }

        #endregion


    }
}
