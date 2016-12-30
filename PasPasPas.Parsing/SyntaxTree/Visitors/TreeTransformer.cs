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

        private AbstractSyntaxPart BeginVisitItem(Unit unit, TreeTransformerOptions parameter) {
            CompilationUnit result = CreatePartNode<CompilationUnit>(parameter.Project, unit);
            result.FileType = CompilationUnitType.Unit;
            result.UnitName = ExtractSymbolName(unit.UnitName);
            result.Hints = ExtractHints(unit.Hints);
            result.FilePath = unit.FilePath;
            parameter.Project.Add(result, parameter.LogSource);
            parameter.CurrentUnit = result;
            return result;
        }

        private void EndVisitItem(Unit unit, TreeTransformerOptions parameter) {
            parameter.CurrentUnit = null;
        }

        #endregion
        #region Library

        private AbstractSyntaxPart BeginVisitItem(Library library, TreeTransformerOptions parameter) {
            CompilationUnit result = CreatePartNode<CompilationUnit>(parameter.Project, library);
            result.FileType = CompilationUnitType.Library;
            result.UnitName = ExtractSymbolName(library.LibraryName);
            result.Hints = ExtractHints(library.Hints);
            result.FilePath = library.FilePath;
            result.InitializationBlock = new BlockOfStatements();
            parameter.Project.Add(result, parameter.LogSource);
            parameter.CurrentUnitMode[result] = UnitMode.Library;
            parameter.CurrentUnit = result;
            return result;
        }

        private void EndVisitItem(Library library, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode.Reset(parameter.CurrentUnit);
            parameter.CurrentUnit = null;
        }

        #endregion
        #region Program

        /// <summary>
        ///     visit a program
        /// </summary>
        /// <param name="program"></param>
        /// <param name="parameter"></param>
        private AbstractSyntaxPart BeginVisitItem(Program program, TreeTransformerOptions parameter) {
            CompilationUnit result = CreatePartNode<CompilationUnit>(parameter.Project, program);
            result.FileType = CompilationUnitType.Program;
            result.UnitName = ExtractSymbolName(program.ProgramName);
            result.FilePath = program.FilePath;
            result.InitializationBlock = new BlockOfStatements();
            parameter.CurrentUnitMode[result] = UnitMode.Program;
            parameter.Project.Add(result, parameter.LogSource);
            parameter.CurrentUnit = result;
            return result;
        }

        private void EndVisitItem(Program program, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode.Reset(parameter.CurrentUnit);
            parameter.CurrentUnit = null;
        }

        #endregion
        #region Package

        private AbstractSyntaxPart BeginVisitItem(Package package, TreeTransformerOptions parameter) {
            CompilationUnit result = CreatePartNode<CompilationUnit>(parameter.Project, package);
            result.FileType = CompilationUnitType.Package;
            result.UnitName = ExtractSymbolName(package.PackageName);
            result.FilePath = package.FilePath;
            parameter.Project.Add(result, parameter.LogSource);
            parameter.CurrentUnit = result;
            return result;
        }

        private void EndVisitItem(Package package, TreeTransformerOptions parameter)
            => parameter.CurrentUnit = null;

        #endregion
        #region UnitInterface

        private AbstractSyntaxPart BeginVisitItem(UnitInterface unitInterface, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode[parameter.CurrentUnit] = UnitMode.Interface;
            return parameter.CurrentUnit.InterfaceSymbols;
        }


        private void EndVisitItem(UnitInterface unitInterface, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode.Reset(parameter.CurrentUnit);
        }

        #endregion
        #region UnitImplementation

        private AbstractSyntaxPart BeginVisitItem(UnitImplementation unitImplementation, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode[parameter.CurrentUnit] = UnitMode.Implementation;
            return parameter.CurrentUnit.ImplementationSymbols;
        }

        private void EndVisitItem(UnitImplementation unit, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode.Reset(parameter.CurrentUnit);
        }


        #endregion
        #region ConstSection

        private AbstractSyntaxPart BeginVisitItem(ConstSection constSection, TreeTransformerOptions parameter) {
            if (constSection.Kind == TokenKind.Const) {
                parameter.CurrentDeclarationMode = DeclarationMode.Const;
            }
            else if (constSection.Kind == TokenKind.Resourcestring) {
                parameter.CurrentDeclarationMode = DeclarationMode.ResourceString;
            }
            return null;
        }

        private void EndVisitItem(ConstSection constSection, TreeTransformerOptions parameter)
            => parameter.CurrentDeclarationMode = DeclarationMode.Unknown;

        #endregion
        #region TypeSection

        private AbstractSyntaxPart BeginVisitItem(TypeSection constSection, TreeTransformerOptions parameter) {
            parameter.CurrentDeclarationMode = DeclarationMode.Types;
            return null;
        }

        private void EndVisitItem(TypeSection constSection, TreeTransformerOptions parameter)
            => parameter.CurrentDeclarationMode = DeclarationMode.Unknown;

        #endregion
        #region TypeDeclaration

        private AbstractSyntaxPart BeginVisitItem(Standard.TypeDeclaration typeDeclaration, TreeTransformerOptions parameter) {
            Abstract.TypeDeclaration declaration = CreateNode<Abstract.TypeDeclaration>(parameter, typeDeclaration);
            declaration.Name = ExtractSymbolName(typeDeclaration.TypeId?.Identifier);
            declaration.Generics = ExtractGenericDefinition(typeDeclaration.TypeId?.GenericDefinition, parameter);
            declaration.Attributes = ExtractAttributes(typeDeclaration.Attributes, parameter.CurrentUnit);
            declaration.Hints = ExtractHints(typeDeclaration.Hint);
            parameter.AddSymbolTableEntry<DeclaredSymbol>(declaration);
            return declaration;
        }

        #endregion
        #region ConstDeclaration

        private AbstractSyntaxPart BeginVisitItem(ConstDeclaration constDeclaration, TreeTransformerOptions parameter) {
            ConstantDeclaration declaration = CreateNode<ConstantDeclaration>(parameter, constDeclaration);
            declaration.Name = ExtractSymbolName(constDeclaration.Identifier);
            declaration.Mode = parameter.CurrentDeclarationMode;
            declaration.Attributes = ExtractAttributes(constDeclaration.Attributes, parameter.CurrentUnit);
            declaration.Hints = ExtractHints(constDeclaration.Hint);
            parameter.AddSymbolTableEntry<DeclaredSymbol>(declaration);
            return declaration;
        }

        #endregion
        #region ConstantExpression

        private AbstractSyntaxPart BeginVisitItem(ConstantExpression constExpression, TreeTransformerOptions parameter) {

            if (constExpression.IsArrayConstant) {
                ArrayConstant result = CreateNode<ArrayConstant>(parameter, constExpression);
                parameter.DefineExpressionValue(result);
                return result;
            }

            if (constExpression.IsRecordConstant) {
                RecordConstant result = CreateNode<RecordConstant>(parameter, constExpression);
                parameter.DefineExpressionValue(result);
                return result;
            }

            return null;
        }

        #endregion
        #region RecordConstantExpression

        private AbstractSyntaxPart BeginVisitItem(RecordConstantExpression constExpression, TreeTransformerOptions parameter) {
            RecordConstantItem expression = CreateNode<RecordConstantItem>(parameter, constExpression);
            parameter.DefineExpressionValue(expression);
            expression.Name = ExtractSymbolName(constExpression.Name);
            return expression;
        }

        #endregion
        #region Expression

        private AbstractSyntaxPart BeginVisitItem(Expression expression, TreeTransformerOptions parameter) {
            if (expression.LeftOperand != null && expression.RightOperand != null) {
                BinaryOperator currentExpression = CreateNode<BinaryOperator>(parameter, expression);
                parameter.DefineExpressionValue(currentExpression);
                currentExpression.Kind = BinaryOperator.ConvertKind(expression.Kind);
                return currentExpression;
            }
            return null;
        }

        #endregion
        #region SimpleExpression

        private AbstractSyntaxPart BeginVisitItem(SimpleExpression simpleExpression, TreeTransformerOptions parameter) {
            if (simpleExpression.LeftOperand != null && simpleExpression.RightOperand != null) {
                BinaryOperator currentExpression = CreateNode<BinaryOperator>(parameter, simpleExpression);
                parameter.DefineExpressionValue(currentExpression);
                currentExpression.Kind = BinaryOperator.ConvertKind(simpleExpression.Kind);
                return currentExpression;
            }
            return null;
        }

        #endregion       
        #region Term

        private AbstractSyntaxPart BeginVisitItem(Term term, TreeTransformerOptions parameter) {
            if (term.LeftOperand != null && term.RightOperand != null) {
                BinaryOperator currentExpression = CreateNode<BinaryOperator>(parameter, term);
                parameter.DefineExpressionValue(currentExpression);
                currentExpression.Kind = BinaryOperator.ConvertKind(term.Kind);
                return currentExpression;
            }
            return null;
        }

        #endregion
        #region Factor

        private AbstractSyntaxPart BeginVisitItem(Factor factor, TreeTransformerOptions parameter) {

            // unary operators
            if (factor.AddressOf != null || factor.Not != null || factor.Plus != null || factor.Minus != null) {
                UnaryOperator value = CreateNode<UnaryOperator>(parameter, factor);
                parameter.DefineExpressionValue(value);

                if (factor.AddressOf != null)
                    value.Kind = ExpressionKind.AddressOf;
                else if (factor.Not != null)
                    value.Kind = ExpressionKind.Not;
                else if (factor.Plus != null)
                    value.Kind = ExpressionKind.UnaryPlus;
                else if (factor.Minus != null)
                    value.Kind = ExpressionKind.UnaryMinus;
                return value;
            }

            // constant values
            if (factor.IsNil) {
                ConstantValue value = CreateNode<ConstantValue>(parameter, factor);
                value.Kind = ConstantValueKind.Nil;
                parameter.DefineExpressionValue(value);
                return value;
            }

            if (factor.PointerTo != null) {
                VariableValue value = CreateNode<VariableValue>(parameter, factor);
                value.Name = ExtractSymbolName(factor.PointerTo);
                parameter.DefineExpressionValue(value);
                return value;
            }

            if (factor.IsFalse) {
                ConstantValue value = CreateNode<ConstantValue>(parameter, factor);
                value.Kind = ConstantValueKind.False;
                parameter.DefineExpressionValue(value);
                return value;
            }
            else if (factor.IsTrue) {
                ConstantValue value = CreateNode<ConstantValue>(parameter, factor);
                value.Kind = ConstantValueKind.True;
                parameter.DefineExpressionValue(value);
                return value;
            }
            else if (factor.IntValue != null) {
                ConstantValue value = CreateNode<ConstantValue>(parameter, factor);
                value.Kind = ConstantValueKind.Integer;
                parameter.DefineExpressionValue(value);
                return value;
            }
            else if (factor.RealValue != null) {

            }
            if (factor.StringValue != null) {

            }
            return null;
        }

        #endregion
        #region UsesClause

        private AbstractSyntaxPart BeginVisitItem(UsesClause unit, TreeTransformerOptions parameter) {
            if (unit.UsesList == null)
                return null;

            foreach (ISyntaxPart part in unit.UsesList.Parts) {
                var name = part as NamespaceName;
                if (name == null)
                    continue;

                RequiredUnitName unitName = CreatePartNode<RequiredUnitName>(parameter.CurrentUnit.RequiredUnits, part);
                unitName.Name = ExtractSymbolName(name);
                unitName.Mode = parameter.CurrentUnitMode[parameter.CurrentUnit];
                parameter.CurrentUnit.RequiredUnits.Add(unitName, parameter.LogSource);
            }

            return null;
        }

        #endregion
        #region UsesFileClause

        private AbstractSyntaxPart BeginVisitItem(UsesFileClause unit, TreeTransformerOptions parameter) {
            if (unit.Files == null)
                return null;

            foreach (ISyntaxPart part in unit.Files.Parts) {
                var name = part as NamespaceFileName;
                if (name == null)
                    continue;

                RequiredUnitName unitName = CreatePartNode<RequiredUnitName>(parameter.CurrentUnit.RequiredUnits, part);
                unitName.Name = ExtractSymbolName(name);
                unitName.Mode = parameter.CurrentUnitMode[parameter.CurrentUnit];
                unitName.FileName = name.QuotedFileName?.UnquotedValue;
                parameter.CurrentUnit.RequiredUnits.Add(unitName, parameter.LogSource);
            }

            return null;
        }

        #endregion
        #region PackageRequires

        private AbstractSyntaxPart BeginVisitItem(PackageRequires requires, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode[parameter.CurrentUnit] = UnitMode.Requires;

            if (requires.RequiresList == null)
                return null;

            foreach (ISyntaxPart part in requires.RequiresList.Parts) {
                var name = part as NamespaceName;
                if (name == null)
                    continue;

                RequiredUnitName unitName = CreatePartNode<RequiredUnitName>(parameter.CurrentUnit.RequiredUnits, part);
                unitName.Name = ExtractSymbolName(name);
                unitName.Mode = parameter.CurrentUnitMode[parameter.CurrentUnit];
                parameter.CurrentUnit.RequiredUnits.Add(unitName, parameter.LogSource);
            }

            return null;
        }

        private void EndVisitItem(PackageRequires requires, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode[parameter.CurrentUnit] = UnitMode.Interface;
        }

        #endregion
        #region PackageContains

        private AbstractSyntaxPart BeginVisitItem(PackageContains contains, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode[parameter.CurrentUnit] = UnitMode.Contains;

            if (contains.ContainsList == null)
                return null;

            foreach (ISyntaxPart part in contains.ContainsList.Parts) {
                var name = part as NamespaceFileName;
                if (name == null)
                    continue;

                RequiredUnitName unitName = CreatePartNode<RequiredUnitName>(parameter.CurrentUnit.RequiredUnits, part);
                unitName.Name = ExtractSymbolName(name);
                unitName.Mode = parameter.CurrentUnitMode[parameter.CurrentUnit];
                unitName.FileName = name.QuotedFileName?.UnquotedValue;
                parameter.CurrentUnit.RequiredUnits.Add(unitName, parameter.LogSource);
            }

            return null;
        }

        private void EndVisitItem(PackageContains contains, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode.Reset(parameter.CurrentUnit);
        }

        #endregion
        #region StructType

        private AbstractSyntaxPart BeginVisitItem(StructType structType, TreeTransformerOptions parameter) {
            if (structType.Packed)
                parameter.CurrentStructTypeMode = StructTypeMode.Packed;
            else
                parameter.CurrentStructTypeMode = StructTypeMode.Unpacked;
            return null;
        }

        private void EndVisitItem(StructType factor, TreeTransformerOptions parameter) {
            parameter.CurrentStructTypeMode = StructTypeMode.Undefined;
        }

        #endregion
        #region ArrayType

        private AbstractSyntaxPart BeginVisitItem(ArrayType array, TreeTransformerOptions parameter) {
            ArrayTypeDeclaration value = CreateNode<ArrayTypeDeclaration>(parameter, array);
            value.PackedType = parameter.CurrentStructTypeMode == StructTypeMode.Packed;
            parameter.DefineTypeValue(value);

            if (array.ArrayOfConst) {
                MetaType metaType = CreatePartNode<MetaType>(value, array);
                metaType.Kind = MetaTypeKind.Const;
                value.TypeValue = metaType;
            }

            return value;
        }

        #endregion
        #region SetDefinition

        private AbstractSyntaxPart BeginVisitItem(SetDefinition set, TreeTransformerOptions parameter) {
            SetTypeDeclaration value = CreateNode<SetTypeDeclaration>(parameter, set);
            value.PackedType = parameter.CurrentStructTypeMode == StructTypeMode.Packed;
            parameter.DefineTypeValue(value);
            return value;
        }

        #endregion
        #region FileTypeDefinition

        private AbstractSyntaxPart BeginVisitItem(FileType set, TreeTransformerOptions parameter) {
            FileTypeDeclaration value = CreateNode<FileTypeDeclaration>(parameter, set);
            value.PackedType = parameter.CurrentStructTypeMode == StructTypeMode.Packed;
            parameter.DefineTypeValue(value);
            return value;
        }

        #endregion
        #region ClassOf

        private AbstractSyntaxPart BeginVisitItem(ClassOfDeclaration classOf, TreeTransformerOptions parameter) {
            ClassOfTypeDeclaration value = CreateNode<ClassOfTypeDeclaration>(parameter, classOf);
            value.PackedType = parameter.CurrentStructTypeMode == StructTypeMode.Packed;
            parameter.DefineTypeValue(value);
            return value;
        }

        #endregion
        #region TypeName                                       

        private AbstractSyntaxPart BeginVisitItem(TypeName typeName, TreeTransformerOptions parameter) {
            MetaType value = CreateNode<MetaType>(parameter, typeName);
            value.Kind = typeName.MapTypeKind();
            parameter.DefineTypeValue(value);
            return value;
        }

        private AbstractSyntaxPart BeginVisitChildItem(TypeName typeName, TreeTransformerOptions parameter, ISyntaxPart part) {
            var name = part as GenericNamespaceName;
            var value = parameter.LastValue as MetaType;

            if (name == null || value == null)
                return null;

            GenericNameFragment fragment = CreatePartNode<GenericNameFragment>(value, name);
            fragment.Name = ExtractSymbolName(name.Name);
            value.AddFragment(fragment);
            return fragment;
        }


        #endregion
        #region SimpleType

        private AbstractSyntaxPart BeginVisitItem(SimpleType simpleType, TreeTransformerOptions parameter) {
            if (simpleType.SubrangeStart != null) {
                SubrangeType subrange = CreateNode<SubrangeType>(parameter, simpleType);
                parameter.DefineTypeValue(subrange);
                return subrange;
            }

            if (simpleType.EnumType != null)
                return null;

            TypeAlias value = CreateNode<TypeAlias>(parameter, simpleType);
            value.IsNewType = simpleType.NewType;

            if (simpleType.TypeOf)
                parameter.LogSource.Warning(StructuralErrors.UnsupportedTypeOfConstruct, simpleType);

            parameter.DefineTypeValue(value);
            return value;
        }

        private AbstractSyntaxPart BeginVisitChildItem(SimpleType simpleType, TreeTransformerOptions parameter, ISyntaxPart part) {
            var name = part as GenericNamespaceName;
            var value = parameter.LastValue as TypeAlias;

            if (name == null || value == null)
                return null;

            GenericNameFragment fragment = CreatePartNode<GenericNameFragment>(value, name);
            fragment.Name = ExtractSymbolName(name.Name);
            value.AddFragment(fragment);
            return fragment;
        }

        #endregion
        #region EnumTypeDefinition

        private AbstractSyntaxPart BeginVisitItem(EnumTypeDefinition type, TreeTransformerOptions parameter) {
            EnumType value = CreateNode<EnumType>(parameter, type);
            parameter.DefineTypeValue(value);
            return value;
        }


        #endregion
        #region EnumValue

        private AbstractSyntaxPart BeginVisitItem(EnumValue enumValue, TreeTransformerOptions parameter) {
            var enumDeclaration = parameter.LastValue as EnumType;
            if (enumDeclaration != null) {
                EnumTypeValue value = CreateNode<EnumTypeValue>(parameter, enumValue);
                value.Name = ExtractSymbolName(enumValue.EnumName);
                enumDeclaration.Add(value, parameter.LogSource);
                return value;
            }
            return null;
        }

        #endregion
        #region ArrayIndex

        private AbstractSyntaxPart BeginVisitItem(ArrayIndex arrayIndex, TreeTransformerOptions parameter) {
            if (arrayIndex.EndIndex != null) {
                BinaryOperator binOp = CreateNode<BinaryOperator>(parameter, arrayIndex);
                parameter.DefineExpressionValue(binOp);
                binOp.Kind = ExpressionKind.RangeOperator;
                return binOp;
            }

            return null;
        }

        #endregion
        #region PointerType

        private AbstractSyntaxPart BeginVisitItem(PointerType pointer, TreeTransformerOptions parameter) {
            if (pointer.GenericPointer) {
                MetaType result = CreateNode<MetaType>(parameter, pointer);
                result.Kind = MetaTypeKind.Pointer;
                parameter.DefineTypeValue(result);
                return result;
            }
            else {
                PointerToType result = CreateNode<PointerToType>(parameter, pointer);
                parameter.DefineTypeValue(result);
                return result;
            }
        }

        #endregion
        #region StringType

        private AbstractSyntaxPart BeginVisitItem(StringType stringType, TreeTransformerOptions parameter) {
            MetaType result = CreateNode<MetaType>(parameter, stringType);
            result.Kind = MetaType.ConvertKind(stringType.Kind);
            parameter.DefineTypeValue(result);
            return result;
        }

        #endregion
        #region ProcedureTypeDefinition

        private AbstractSyntaxPart BeginVisitItem(ProcedureTypeDefinition proceduralType, TreeTransformerOptions parameter) {
            ProceduralType result = CreateNode<ProceduralType>(parameter, proceduralType);
            parameter.DefineTypeValue(result);
            result.Kind = ProceduralType.MapKind(proceduralType.Kind);
            result.MethodDeclaration = proceduralType.MethodDeclaration;
            result.AllowAnonymousMethods = proceduralType.AllowAnonymousMethods;

            if (proceduralType.ReturnTypeAttributes != null)
                result.ReturnAttributes = ExtractAttributes(proceduralType.ReturnTypeAttributes, parameter.CurrentUnit);

            return result;
        }

        #endregion
        #region FormalParameterDefinition

        private AbstractSyntaxPart BeginVisitItem(FormalParameterDefinition formalParameter, TreeTransformerOptions parameter) {
            ParameterTypeDefinition result = CreateNode<ParameterTypeDefinition>(parameter, formalParameter);
            var paramterTarget = parameter.LastValue as IParameterTarget;
            paramterTarget.Parameters.Add(result);
            return result;
        }

        #endregion
        #region FormalParameter

        private AbstractSyntaxPart BeginVisitItem(FormalParameter formalParameter, TreeTransformerOptions parameter) {
            ParameterDefinition result = CreateNode<ParameterDefinition>(parameter, formalParameter);
            var typeDefinition = parameter.LastValue as ParameterTypeDefinition;
            result.Name = ExtractSymbolName(formalParameter.ParameterName);
            result.Attributes = ExtractAttributes(formalParameter.Attributes, parameter.CurrentUnit);
            result.ParameterKind = ParameterDefinition.MapKind(formalParameter.ParameterType);
            typeDefinition.Add(result, parameter.LogSource);
            return result;
        }



        #endregion
        #region UnitInitialization

        private AbstractSyntaxPart BeginVisitItem(UnitInitialization unitBlock, TreeTransformerOptions parameter) {
            BlockOfStatements result = CreatePartNode<BlockOfStatements>(parameter.CurrentUnit, unitBlock);
            parameter.CurrentUnit.InitializationBlock = result;
            return result;
        }

        #endregion
        #region UnitFinalization

        private AbstractSyntaxPart BeginVisitItem(UnitFinalization unitBlock, TreeTransformerOptions parameter) {
            BlockOfStatements result = CreatePartNode<BlockOfStatements>(parameter.CurrentUnit, unitBlock);
            parameter.CurrentUnit.FinalizationBlock = result;
            return result;
        }

        #endregion
        #region CompoundStatement

        private AbstractSyntaxPart BeginVisitItem(CompoundStatement block, TreeTransformerOptions parameter) {

            if (block.AssemblerBlock != null) {
                var statementTarget = parameter.LastValue as IStatementTarget;
                BlockOfAssemblerStatements result = CreatePartNode<BlockOfAssemblerStatements>(parameter.LastValue, block);
                statementTarget.Statements.Add(result);
                return result;
            }

            else {
                var statementTarget = parameter.LastValue as IStatementTarget;
                BlockOfStatements result = CreatePartNode<BlockOfStatements>(parameter.LastValue, block);
                statementTarget.Statements.Add(result);
                return result;
            }

        }

        #endregion
        #region Label

        private AbstractSyntaxPart BeginVisitItem(Label label, TreeTransformerOptions parameter) {
            var parent = parameter.LastValue as StatementBase;

            if (parent == null)
                return null;

            var standardLabel = label.LabelName as Identifier;
            if (standardLabel != null) {
                parent.LabelName = ExtractSymbolName(standardLabel);
                return null;
            }

            Token intLabel = (label.LabelName as StandardInteger)?.LastTerminalToken;
            if (intLabel != null) {
                parent.LabelName = new SymbolName() { Name = intLabel.Value };
                return null;
            }

            Token hexLabel = (label.LabelName as HexNumber)?.LastTerminalToken;
            if (hexLabel != null) {
                parent.LabelName = new SymbolName() { Name = hexLabel.Value };
                return null;
            }

            return null;
        }

        #endregion
        #region ClassDeclaration

        private AbstractSyntaxPart BeginVisitItem(ClassDeclaration classDeclaration, TreeTransformerOptions parameter) {
            StructuredType result = CreateNode<StructuredType>(parameter, classDeclaration);
            result.Kind = StructuredTypeKind.Class;
            result.SealedClass = classDeclaration.Sealed;
            result.AbstractClass = classDeclaration.Abstract;
            result.ForwardDeclaration = classDeclaration.ForwardDeclaration;
            parameter.DefineTypeValue(result);
            parameter.CurrentMemberVisibility[result] = MemberVisibility.Public;
            return result;
        }

        private void EndVisitItem(ClassDeclaration classDeclaration, TreeTransformerOptions parameter) {
            var parentType = parameter.LastValue as StructuredType;
            parameter.CurrentMemberVisibility.Reset(parentType);
        }

        #endregion
        #region ClassDeclarationItem

        private AbstractSyntaxPart BeginVisitItem(ClassDeclarationItem classDeclarationItem, TreeTransformerOptions parameter) {
            var parentType = parameter.LastValue as StructuredType;

            if (parentType == null)
                return null;

            if (classDeclarationItem.Visibility != TokenKind.Undefined) {
                parameter.CurrentMemberVisibility[parentType] = StructuredType.MapVisibility(classDeclarationItem.Visibility, classDeclarationItem.Strict);
            };

            return null;
        }

        #endregion
        #region ClassField

        private AbstractSyntaxPart BeginVisitItem(ClassField field, TreeTransformerOptions parameter) {
            var structType = parameter.LastValue as StructuredType;
            StructureFields result = CreateNode<StructureFields>(parameter, field);
            result.Visibility = parameter.CurrentMemberVisibility[structType];
            structType.Fields.Fields.Add(result);
            IList<SymbolAttribute> extractedAttributes = ExtractAttributes(((ClassDeclarationItem)field.Parent).Attributes, parameter.CurrentUnit);


            foreach (ISyntaxPart part in field.Names.Parts) {
                var attrs = part as UserAttributes;

                if (attrs != null) {
                    extractedAttributes = ExtractAttributes(attrs, parameter.CurrentUnit, extractedAttributes);
                    continue;
                }

                var partName = part as Identifier;
                if (partName == null)
                    continue;

                StructureField fieldName = CreatePartNode<StructureField>(result, partName);
                fieldName.Name = ExtractSymbolName(partName);
                fieldName.Attributes = extractedAttributes;
                result.Add(fieldName, parameter.LogSource);
                extractedAttributes = null;
            }


            return result;
        }

        #endregion

        #region Extractors

        private static SymbolName ExtractSymbolName(NamespaceName name) {
            var result = new SymbolName() {
                Name = name?.Name,
                Namespace = name?.Namespace
            };
            return result;
        }

        private static SymbolName ExtractSymbolName(Identifier name) {
            var result = new SymbolName() {
                Name = name.FirstTerminalToken?.Value
            };
            return result;
        }

        private static SymbolName ExtractSymbolName(NamespaceFileName name) {
            var result = new SymbolName() {
                Name = name?.NamespaceName?.Name,
                Namespace = name?.NamespaceName?.Namespace
            };
            return result;
        }

        private static GenericTypes ExtractGenericDefinition(GenericDefinition genericDefinition, TreeTransformerOptions parameter) {
            var result = new GenericTypes();

            if (genericDefinition == null)
                return result;

            foreach (ISyntaxPart part in genericDefinition.Parts) {
                var idPart = part as Identifier;

                if (idPart != null) {
                    GenericType generic = CreatePartNode<GenericType>(result, part);
                    generic.Name = ExtractSymbolName(idPart);
                    result.Add(generic, parameter.LogSource);
                    continue;
                }

                var genericPart = part as GenericDefinitionPart;

                if (genericPart != null) {
                    GenericType generic = CreatePartNode<GenericType>(result, part);
                    generic.Name = ExtractSymbolName(genericPart.Identifier);
                    result.Add(generic, parameter.LogSource);

                    foreach (ISyntaxPart constraintPart in genericPart.Parts) {
                        var constraint = constraintPart as Standard.ConstrainedGeneric;
                        if (constraint != null) {
                            GenericConstraint cr = CreatePartNode<GenericConstraint>(generic, constraint);
                            cr.Kind = GenericConstraint.MapKind(constraint);
                            cr.Name = ExtractSymbolName(constraint.ConstraintIdentifier);
                            generic.Add(cr, parameter.LogSource);
                        }
                    }

                    continue;
                }
            }


            return result;
        }

        private static SymbolHints ExtractHints(HintingInformationList hints) {
            var result = new SymbolHints();

            if (hints == null || hints.PartList.Count < 1)
                return result;

            foreach (ISyntaxPart part in hints.Parts) {
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

        private IList<SymbolAttribute> ExtractAttributes(UserAttributes attributes, CompilationUnit parentUnit, IList<SymbolAttribute> result = null) {
            if (attributes == null || attributes.PartList.Count < 1)
                return EmptyCollection<SymbolAttribute>.Instance;

            if (result == null)
                result = new List<SymbolAttribute>();

            foreach (ISyntaxPart part in attributes.Parts) {
                var attribute = part as UserAttributeDefinition;
                var isAssemblyAttribute = false;

                if (attribute == null) {
                    var assemblyAttribute = part as AssemblyAttributeDeclaration;
                    if (assemblyAttribute == null)
                        continue;

                    attribute = assemblyAttribute.Attribute;
                    isAssemblyAttribute = true;
                }

                var userAttribute = new SymbolAttribute() {
                    Name = ExtractSymbolName(attribute.Name)
                };
                if (!isAssemblyAttribute) {
                    result.Add(userAttribute);
                }
                else
                    parentUnit.AddAssemblyAttribute(userAttribute);
            }

            return result;
        }



        #endregion
        #region Helper functions

        private static T CreateNode<T>(TreeTransformerOptions parameter, ISyntaxPart element) where T : AbstractSyntaxPart, new()
            => CreatePartNode<T>(parameter.LastValue, element);

        private static T CreatePartNode<T>(AbstractSyntaxPart parent, ISyntaxPart element) where T : AbstractSyntaxPart, new() {
            var result = new T() {
                Parent = parent
            };
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
            AbstractSyntaxPart visitResult = BeginVisitItem(part, parameter);
            if (visitResult != null)
                parameter.WorkingStack.Push(new WorkingStackEntry(syntaxPart, null, visitResult));
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
            AbstractSyntaxPart visitResult = BeginVisitChildItem(part, visitorParameter, child);
            if (visitResult != null)
                visitorParameter.WorkingStack.Push(new WorkingStackEntry(parent, child, visitResult));
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
            if (parameter.WorkingStack.Count < 1) return true;

            WorkingStackEntry lastEntry = parameter.WorkingStack.Peek();

            while (lastEntry.DefiningNode == syntaxPart && //
                lastEntry.ChildNode == null) {
                parameter.WorkingStack.Pop();
                if (parameter.WorkingStack.Count < 1)
                    return true;
                lastEntry = parameter.WorkingStack.Peek();
            }

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
            if (visitorParameter.WorkingStack.Count < 1)
                return;

            WorkingStackEntry lastEntry = visitorParameter.WorkingStack.Peek();
            if (lastEntry.DefiningNode == parent && //
                lastEntry.ChildNode == child)
                visitorParameter.WorkingStack.Pop();
        }



        private AbstractSyntaxPart BeginVisitItem(ISyntaxPart part, TreeTransformerOptions parameter)
            => null;

        private AbstractSyntaxPart BeginVisitChildItem(ISyntaxPart part, TreeTransformerOptions parameter, ISyntaxPart child)
            => null;

        private void EndVisitItem(ISyntaxPart part, TreeTransformerOptions parameter) {
            //..
        }

        private void EndVisitChildItem(ISyntaxPart part, TreeTransformerOptions parameter, ISyntaxPart child) {
            //..
        }

        #endregion


    }
}
