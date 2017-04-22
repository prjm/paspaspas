using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Standard;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.Tokenizer;
using System;
using System.Linq;

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
            result.InterfaceSymbols = new DeclaredSymbols() { Parent = result };
            result.ImplementationSymbols = new DeclaredSymbols() { Parent = result };
            parameter.Project.Add(result, parameter.LogSource);
            parameter.CurrentUnit = result;
            return result;
        }

        private void EndVisitItem(Unit unit, TreeTransformerOptions parameter)
            => parameter.CurrentUnit = null;

        #endregion
        #region Library

        private AbstractSyntaxPart BeginVisitItem(Library library, TreeTransformerOptions parameter) {
            CompilationUnit result = CreatePartNode<CompilationUnit>(parameter.Project, library);
            result.FileType = CompilationUnitType.Library;
            result.UnitName = ExtractSymbolName(library.LibraryName);
            result.Hints = ExtractHints(library.Hints);
            result.FilePath = library.FilePath;
            if (library.MainBlock.Body.AssemblerBlock != null)
                result.InitializationBlock = new BlockOfAssemblerStatements();
            else
                result.InitializationBlock = new BlockOfStatements();
            result.Symbols = new DeclaredSymbols() { Parent = result };
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
            result.Symbols = new DeclaredSymbols() { Parent = result };
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
            parameter.CurrentUnit.Symbols = parameter.CurrentUnit.InterfaceSymbols;
            return parameter.CurrentUnit.InterfaceSymbols;
        }


        private void EndVisitItem(UnitInterface unitInterface, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode.Reset(parameter.CurrentUnit);
            parameter.CurrentUnit.Symbols = null; ;
        }

        #endregion
        #region UnitImplementation

        private AbstractSyntaxPart BeginVisitItem(UnitImplementation unitImplementation, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode[parameter.CurrentUnit] = UnitMode.Implementation;
            parameter.CurrentUnit.Symbols = parameter.CurrentUnit.ImplementationSymbols;
            return parameter.CurrentUnit.ImplementationSymbols;
        }

        private void EndVisitItem(UnitImplementation unit, TreeTransformerOptions parameter) {
            parameter.CurrentUnit.Symbols = null;
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

        private AbstractSyntaxPart BeginVisitItem(TypeSection typeSection, TreeTransformerOptions parameter) {
            parameter.CurrentDeclarationMode = DeclarationMode.Types;
            return null;
        }

        private void EndVisitItem(TypeSection typeSection, TreeTransformerOptions parameter)
            => parameter.CurrentDeclarationMode = DeclarationMode.Unknown;

        #endregion
        #region TypeDeclaration

        private AbstractSyntaxPart BeginVisitItem(Standard.TypeDeclaration typeDeclaration, TreeTransformerOptions parameter) {
            var symbols = parameter.LastValue as IDeclaredSymbolTarget;
            Abstract.TypeDeclaration declaration = CreateNode<Abstract.TypeDeclaration>(parameter, typeDeclaration);
            declaration.Name = ExtractSymbolName(typeDeclaration.TypeId?.Identifier);
            declaration.Generics = ExtractGenericDefinition(declaration, typeDeclaration.TypeId?.GenericDefinition, parameter);
            declaration.Attributes = ExtractAttributes(typeDeclaration.Attributes, parameter.CurrentUnit);
            declaration.Hints = ExtractHints(typeDeclaration.Hint);
            symbols.Symbols.AddDirect(declaration, parameter.LogSource);
            return declaration;
        }

        #endregion
        #region ConstDeclaration

        private AbstractSyntaxPart BeginVisitItem(ConstDeclaration constDeclaration, TreeTransformerOptions parameter) {
            var symbols = parameter.LastValue as IDeclaredSymbolTarget;
            ConstantDeclaration declaration = CreateNode<ConstantDeclaration>(parameter, constDeclaration);
            declaration.Name = ExtractSymbolName(constDeclaration.Identifier);
            declaration.Mode = parameter.CurrentDeclarationMode;
            declaration.Attributes = ExtractAttributes(constDeclaration.Attributes, parameter.CurrentUnit);
            declaration.Hints = ExtractHints(constDeclaration.Hint);
            symbols.Symbols.AddDirect(declaration, parameter.LogSource);
            return declaration;
        }

        #endregion
        #region VarSection

        private AbstractSyntaxPart BeginVisitItem(VarSection varSection, TreeTransformerOptions parameter) {
            if (varSection.Kind == TokenKind.Var)
                parameter.CurrentDeclarationMode = DeclarationMode.Var;
            else if (varSection.Kind == TokenKind.ThreadVar)
                parameter.CurrentDeclarationMode = DeclarationMode.ThreadVar;
            else
                parameter.CurrentDeclarationMode = DeclarationMode.Unknown;

            return null;
        }

        private void EndVisitItem(VarSection varSection, TreeTransformerOptions parameter)
            => parameter.CurrentDeclarationMode = DeclarationMode.Unknown;


        #endregion
        #region VarDeclaration

        private AbstractSyntaxPart BeginVisitItem(VarDeclaration varDeclaration, TreeTransformerOptions parameter) {
            var symbols = parameter.LastValue as IDeclaredSymbolTarget;
            VariableDeclaration declaration = CreateNode<VariableDeclaration>(parameter, varDeclaration);
            declaration.Mode = parameter.CurrentDeclarationMode;
            declaration.Hints = ExtractHints(varDeclaration.Hints);

            foreach (ISyntaxPart child in varDeclaration.Identifiers.Parts) {
                var ident = child as Identifier;
                if (ident != null) {
                    VariableName name = CreatePartNode<VariableName>(declaration, child);
                    name.Name = ExtractSymbolName(ident);
                    declaration.Names.Add(name);
                    symbols.Symbols.Add(name, parameter.LogSource);
                }
            }

            declaration.Attributes = ExtractAttributes(varDeclaration.Attributes, parameter.CurrentUnit);
            symbols.Symbols.Items.Add(declaration);
            return declaration;
        }

        #endregion
        #region VarValueSpecification

        private AbstractSyntaxPart BeginVisitItem(VarValueSpecification varValue, TreeTransformerOptions parameter) {
            var varDeclaration = parameter.LastValue as VariableDeclaration;

            if (varValue.Absolute != null)
                varDeclaration.ValueKind = VariableValueKind.Absolute;
            else if (varValue.InitialValue != null)
                varDeclaration.ValueKind = VariableValueKind.InitialValue;

            return null;
        }

        #endregion
        #region ConstantExpression

        private AbstractSyntaxPart BeginVisitItem(ConstantExpression constExpression, TreeTransformerOptions parameter) {

            if (constExpression.IsSetConstant) {
                SetConstant result = CreateNode<SetConstant>(parameter, constExpression);
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
                SymbolReference value = CreateNode<SymbolReference>(parameter, factor);
                value.Name = ExtractSymbolName(factor.PointerTo);
                value.PointerTo = true;
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
                ConstantValue value = CreateNode<ConstantValue>(parameter, factor);
                value.Kind = ConstantValueKind.RealNumber;
                parameter.DefineExpressionValue(value);
                return value;
            }
            if (factor.StringValue != null) {
                ConstantValue value = CreateNode<ConstantValue>(parameter, factor);
                value.Kind = ConstantValueKind.QuotedString;
                parameter.DefineExpressionValue(value);
                return value;
            }
            if (factor.HexValue != null) {
                ConstantValue value = CreateNode<ConstantValue>(parameter, factor);
                value.Kind = ConstantValueKind.HexNumber;
                parameter.DefineExpressionValue(value);
                return value;
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

        private void EndVisitItem(PackageRequires requires, TreeTransformerOptions parameter)
            => parameter.CurrentUnitMode[parameter.CurrentUnit] = UnitMode.Interface;

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

        private void EndVisitItem(PackageContains contains, TreeTransformerOptions parameter)
            => parameter.CurrentUnitMode.Reset(parameter.CurrentUnit);

        #endregion
        #region StructType

        private AbstractSyntaxPart BeginVisitItem(StructType structType, TreeTransformerOptions parameter) {
            if (structType.Packed)
                parameter.CurrentStructTypeMode = StructTypeMode.Packed;
            else
                parameter.CurrentStructTypeMode = StructTypeMode.Unpacked;
            return null;
        }

        private void EndVisitItem(StructType factor, TreeTransformerOptions parameter)
            => parameter.CurrentStructTypeMode = StructTypeMode.Undefined;

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
            result.Kind = Abstract.MethodDeclaration.MapKind(proceduralType.Kind);
            result.MethodDeclaration = proceduralType.MethodDeclaration;
            result.AllowAnonymousMethods = proceduralType.AllowAnonymousMethods;

            if (proceduralType.ReturnTypeAttributes != null)
                result.ReturnAttributes = ExtractAttributes(proceduralType.ReturnTypeAttributes, parameter.CurrentUnit);

            return result;
        }

        #endregion
        #region FormalParameterDefinition

        private AbstractSyntaxPart BeginVisitItem(FormalParameterDefinition formalParameter, TreeTransformerOptions parameter) {
            var paramterTarget = parameter.LastValue as IParameterTarget;
            ParameterTypeDefinition result = CreatePartNode<ParameterTypeDefinition>(paramterTarget.Parameters, formalParameter);
            paramterTarget.Parameters.Items.Add(result);
            return result;
        }

        #endregion
        #region FormalParameter

        private AbstractSyntaxPart BeginVisitItem(FormalParameter formalParameter, TreeTransformerOptions parameter) {
            ParameterDefinition result = CreateNode<ParameterDefinition>(parameter, formalParameter);
            var typeDefinition = parameter.LastValue as ParameterTypeDefinition;
            var allParams = typeDefinition.Parent as ParameterDefinitions;
            result.Name = ExtractSymbolName(formalParameter.ParameterName);
            result.Attributes = ExtractAttributes(formalParameter.Attributes, parameter.CurrentUnit);
            result.ParameterKind = ParameterDefinition.MapKind(formalParameter.ParameterType);
            typeDefinition.Parameters.Add(result);
            allParams.Add(result, parameter.LogSource);
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
                var blockTarget = parameter.LastValue as IBlockTarget;
                BlockOfAssemblerStatements result = CreatePartNode<BlockOfAssemblerStatements>(parameter.LastValue, block);
                if (statementTarget != null)
                    statementTarget.Statements.Add(result);
                else if (blockTarget != null)
                    blockTarget.Block = result;
                return result;
            }

            else {
                var statementTarget = parameter.LastValue as IStatementTarget;
                var blockTarget = parameter.LastValue as IBlockTarget;
                BlockOfStatements result = CreatePartNode<BlockOfStatements>(parameter.LastValue, block);
                if (statementTarget != null)
                    statementTarget.Statements.Add(result);
                else if (blockTarget != null)
                    blockTarget.Block = result;
                return result;
            }

        }

        #endregion
        #region Label

        private AbstractSyntaxPart BeginVisitItem(Label label, TreeTransformerOptions parameter) {
            var parent = parameter.LastValue as ILabelTarget;

            if (parent == null)
                return null;

            var standardLabel = label.LabelName as Identifier;
            if (standardLabel != null) {
                parent.LabelName = ExtractSymbolName(standardLabel);
                return null;
            }

            Token intLabel = (label.LabelName as StandardInteger)?.LastTerminalToken;
            if (intLabel != null) {
                parent.LabelName = new SimpleSymbolName(intLabel.Value);
                return null;
            }

            Token hexLabel = (label.LabelName as HexNumber)?.LastTerminalToken;
            if (hexLabel != null) {
                parent.LabelName = new SimpleSymbolName(hexLabel.Value);
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
            var declItem = field.Parent as IStructuredTypeMember;
            StructureFields result = CreateNode<StructureFields>(parameter, field);
            result.Visibility = parameter.CurrentMemberVisibility[structType];
            structType.Fields.Items.Add(result);
            IList<SymbolAttribute> extractedAttributes = ExtractAttributes(declItem.Attributes, parameter.CurrentUnit);
            result.ClassItem = declItem.ClassItem;

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
                structType.Fields.Add(fieldName, parameter.LogSource);
                result.Fields.Add(fieldName);
                extractedAttributes = null;
            }

            result.Hints = ExtractHints(field.Hint);

            return result;
        }

        #endregion
        #region ClassProperty

        private AbstractSyntaxPart BeginVisitItem(ClassProperty property, TreeTransformerOptions parameter) {
            StructureProperty result = CreateNode<StructureProperty>(parameter, property);
            var parent = parameter.LastValue as StructuredType;
            var declItem = property.Parent as IStructuredTypeMember;
            result.Name = ExtractSymbolName(property.PropertyName);
            parent.Properties.Add(result, parameter.LogSource);
            result.Visibility = parameter.CurrentMemberVisibility[parent];

            if (declItem != null) {
                result.Attributes = ExtractAttributes(declItem.Attributes, parameter.CurrentUnit);
            }

            return result;
        }

        #endregion    
        #region ClassPropertyReadWrite

        private AbstractSyntaxPart BeginVisitItem(ClassPropertyReadWrite property, TreeTransformerOptions parameter) {
            var parent = parameter.LastValue as StructureProperty;
            StructurePropertyAccessor result = CreateNode<StructurePropertyAccessor>(parameter, property);
            result.Kind = StructurePropertyAccessor.MapKind(property.Kind);
            result.Name = ExtractSymbolName(property.Member);
            parent.Accessors.Add(result);
            return result;
        }

        #endregion
        #region  ClassPropertyDispInterface

        private AbstractSyntaxPart BeginVisitItem(ClassPropertyDispInterface property, TreeTransformerOptions parameter) {
            var parent = parameter.LastValue as StructureProperty;
            StructurePropertyAccessor result = CreateNode<StructurePropertyAccessor>(parameter, property);

            if (property.ReadOnly) {
                result.Kind = StructurePropertyAccessorKind.ReadOnly;
            }
            else if (property.WriteOnly) {
                result.Kind = StructurePropertyAccessorKind.WriteOnly;
            }
            else if (property.DispId != null) {
                result.Kind = StructurePropertyAccessorKind.DispId;
            }
            else {
                result.Kind = StructurePropertyAccessorKind.Undefined;
            }

            parent.Accessors.Add(result);
            return result;
        }

        #endregion
        #region ParseClassPropertyAccessSpecifier

        private AbstractSyntaxPart BeginVisitItem(ClassPropertySpecifier property, TreeTransformerOptions parameter) {
            if (property.PropertyReadWrite != null)
                return null;

            if (property.PropertyDispInterface != null)
                return null;

            var parent = parameter.LastValue as StructureProperty;
            StructurePropertyAccessor result = CreateNode<StructurePropertyAccessor>(parameter, property);

            if (property.IsStored) {
                result.Kind = StructurePropertyAccessorKind.Stored;
            }
            else if (property.IsDefault) {
                result.Kind = StructurePropertyAccessorKind.Default;
            }
            else if (property.NoDefault) {
                result.Kind = StructurePropertyAccessorKind.NoDefault;
            }
            else if (property.ImplementsTypeId != null) {
                result.Kind = StructurePropertyAccessorKind.Implements;
                result.Name = ExtractSymbolName(property.ImplementsTypeId);
            }

            parent.Accessors.Add(result);
            return result;
        }

        #endregion
        #region ClassMethod

        private AbstractSyntaxPart BeginVisitItem(ClassMethod method, TreeTransformerOptions parameter) {
            StructureMethod result = CreateNode<StructureMethod>(parameter, method);
            var parent = parameter.LastValue as StructuredType;
            result.Visibility = parameter.CurrentMemberVisibility[parent];

            var declItem = method.Parent as IStructuredTypeMember;
            if (declItem != null) {
                result.ClassItem = declItem.ClassItem;
                result.Attributes = ExtractAttributes(declItem.Attributes, parameter.CurrentUnit);
            }

            result.Name = ExtractSymbolName(method.Identifier);
            result.Kind = Abstract.MethodDeclaration.MapKind(method.MethodKind);
            result.Generics = ExtractGenericDefinition(result, method.GenericDefinition, parameter);
            parent.Methods.Add(result, parameter.LogSource);
            return result;
        }

        #endregion
        #region MethodResolution

        private AbstractSyntaxPart BeginVisitItem(MethodResolution methodResolution, TreeTransformerOptions parameter) {
            StructureMethodResolution result = CreateNode<StructureMethodResolution>(parameter, methodResolution);
            var parent = parameter.LastValue as StructuredType;
            result.Attributes = ExtractAttributes(((ClassDeclarationItem)methodResolution.Parent).Attributes, parameter.CurrentUnit);
            result.Kind = StructureMethodResolution.MapKind(methodResolution.Kind);
            result.Target = ExtractSymbolName(methodResolution.ResolveIdentifier);
            parent.MethodResolutions.Add(result);
            return result;
        }

        #endregion
        #region ReintroduceDirective

        private AbstractSyntaxPart BeginVisitItem(ReintroduceDirective directive, TreeTransformerOptions parameter) {
            var parent = parameter.LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(parameter, directive);
            result.Kind = MethodDirectiveKind.Reintroduce;
            parent.Directives.Add(result);
            return result;
        }

        #endregion
        #region OverloadDirective

        private AbstractSyntaxPart BeginVisitItem(OverloadDirective directive, TreeTransformerOptions parameter) {
            var parent = parameter.LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(parameter, directive);
            result.Kind = MethodDirectiveKind.Overload;
            parent.Directives.Add(result);
            return result;
        }

        #endregion
        #region DispIdDirective

        private AbstractSyntaxPart BeginVisitItem(DispIdDirective directive, TreeTransformerOptions parameter) {
            var parent = parameter.LastValue as IDirectiveTarget;

            if (parent == null)
                return null;

            MethodDirective result = CreateNode<MethodDirective>(parameter, directive);
            result.Kind = MethodDirectiveKind.DispId;
            parent.Directives.Add(result);
            return result;
        }

        #endregion
        #region InlineDirective

        private AbstractSyntaxPart BeginVisitItem(InlineDirective directive, TreeTransformerOptions parameter) {
            var parent = parameter.LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(parameter, directive);

            if (directive.Kind == TokenKind.Inline) {
                result.Kind = MethodDirectiveKind.Inline;
            }
            else if (directive.Kind == TokenKind.Assembler) {
                result.Kind = MethodDirectiveKind.Assembler;
            }

            parent.Directives.Add(result);
            return result;
        }

        #endregion
        #region AbstractDirective

        private AbstractSyntaxPart BeginVisitItem(AbstractDirective directive, TreeTransformerOptions parameter) {
            var parent = parameter.LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(parameter, directive);

            if (directive.Kind == TokenKind.Abstract) {
                result.Kind = MethodDirectiveKind.Abstract;
            }
            else if (directive.Kind == TokenKind.Final) {
                result.Kind = MethodDirectiveKind.Final;
            }

            parent.Directives.Add(result);
            return result;
        }

        #endregion
        #region OldCallConvention

        private AbstractSyntaxPart BeginVisitItem(OldCallConvention directive, TreeTransformerOptions parameter) {
            var parent = parameter.LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(parameter, directive);

            if (directive.Kind == TokenKind.Far) {
                result.Kind = MethodDirectiveKind.Far;
            }

            else if (directive.Kind == TokenKind.Local) {
                result.Kind = MethodDirectiveKind.Local;
            }

            else if (directive.Kind == TokenKind.Near) {
                result.Kind = MethodDirectiveKind.Near;
            }

            parent.Directives.Add(result);
            return result;
        }

        #endregion
        #region ExternalDirective

        private AbstractSyntaxPart BeginVisitItem(ExternalDirective directive, TreeTransformerOptions parameter) {
            var parent = parameter.LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(parameter, directive);

            if (directive.Kind == TokenKind.VarArgs) {
                result.Kind = MethodDirectiveKind.VarArgs;
            }

            else if (directive.Kind == TokenKind.External) {
                result.Kind = MethodDirectiveKind.External;
            }

            parent.Directives.Add(result);
            return result;
        }

        #endregion
        #region ExternalSpecifier

        private AbstractSyntaxPart BeginVisitItem(ExternalSpecifier directive, TreeTransformerOptions parameter) {
            var parent = parameter.LastValue as MethodDirective;
            MethodDirectiveSpecifier result = CreateNode<MethodDirectiveSpecifier>(parameter, directive);
            result.Kind = MethodDirectiveSpecifier.MapKind(directive.Kind);
            parent.Specifiers.Add(result);
            return result;
        }

        #endregion
        #region CallConvention

        private AbstractSyntaxPart BeginVisitItem(CallConvention directive, TreeTransformerOptions parameter) {
            var parent = parameter.LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(parameter, directive);

            if (directive.Kind == TokenKind.Cdecl) {
                result.Kind = MethodDirectiveKind.Cdecl;
            }
            else if (directive.Kind == TokenKind.Pascal) {
                result.Kind = MethodDirectiveKind.Pascal;
            }
            else if (directive.Kind == TokenKind.Register) {
                result.Kind = MethodDirectiveKind.Register;
            }
            else if (directive.Kind == TokenKind.Safecall) {
                result.Kind = MethodDirectiveKind.Safecall;
            }
            else if (directive.Kind == TokenKind.Stdcall) {
                result.Kind = MethodDirectiveKind.StdCall;
            }
            else if (directive.Kind == TokenKind.Export) {
                result.Kind = MethodDirectiveKind.Export;
            }

            parent.Directives.Add(result);
            return result;
        }

        #endregion
        #region BindingDirective

        private AbstractSyntaxPart BeginVisitItem(BindingDirective directive, TreeTransformerOptions parameter) {
            var parent = parameter.LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(parameter, directive);

            if (directive.Kind == TokenKind.Static) {
                result.Kind = MethodDirectiveKind.Static;
            }
            else if (directive.Kind == TokenKind.Dynamic) {
                result.Kind = MethodDirectiveKind.Dynamic;
            }
            else if (directive.Kind == TokenKind.Virtual) {
                result.Kind = MethodDirectiveKind.Virtual;
            }
            else if (directive.Kind == TokenKind.Override) {
                result.Kind = MethodDirectiveKind.Override;
            }
            else if (directive.Kind == TokenKind.Message) {
                result.Kind = MethodDirectiveKind.Message;
            }

            parent.Directives.Add(result);
            return result;
        }

        #endregion
        #region MethodDirectives

        private AbstractSyntaxPart BeginVisitChildItem(MethodDirectives parent, TreeTransformerOptions parameter, ISyntaxPart child) {
            var hints = child as HintingInformation;
            var lastValue = parameter.LastValue as IDirectiveTarget;

            if (hints != null && lastValue != null) {
                lastValue.Hints = ExtractHints(hints, lastValue.Hints);
            }

            return null;
        }

        #endregion
        #region FunctionDirectives

        private AbstractSyntaxPart BeginVisitChildItem(FunctionDirectives parent, TreeTransformerOptions parameter, ISyntaxPart child) {
            var hints = child as HintingInformation;
            var lastValue = parameter.LastValue as IDirectiveTarget;

            if (hints != null && lastValue != null) {
                lastValue.Hints = ExtractHints(hints, lastValue.Hints);
            }

            return null;
        }

        #endregion
        #region ExportedProcedureHeading

        private AbstractSyntaxPart BeginVisitItem(ExportedProcedureHeading procHeading, TreeTransformerOptions parameter) {
            var symbols = parameter.LastValue as IDeclaredSymbolTarget;
            GlobalMethod result = CreateNode<GlobalMethod>(parameter, procHeading);
            result.Name = ExtractSymbolName(procHeading.Name);
            result.Kind = Abstract.MethodDeclaration.MapKind(procHeading.Kind);
            result.Attributes = ExtractAttributes(procHeading.Attributes, parameter.CurrentUnit);
            result.ReturnAttributes = ExtractAttributes(procHeading.ResultAttributes, parameter.CurrentUnit);
            symbols.Symbols.AddDirect(result, parameter.LogSource);
            return result;
        }

        #endregion
        #region UnsafeDirective

        private AbstractSyntaxPart BeginVisitItem(UnsafeDirective directive, TreeTransformerOptions parameter) {
            var parent = parameter.LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(parameter, directive);
            result.Kind = MethodDirectiveKind.Unsafe;
            parent.Directives.Add(result);
            return result;
        }

        #endregion
        #region ForwardDirective

        private AbstractSyntaxPart BeginVisitItem(ForwardDirective directive, TreeTransformerOptions parameter) {
            var parent = parameter.LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(parameter, directive);
            result.Kind = MethodDirectiveKind.Forward;
            parent.Directives.Add(result);
            return result;
        }

        #endregion
        #region ExportsSection

        private AbstractSyntaxPart BeginVisitItem(ExportsSection exportsSection, TreeTransformerOptions parameter) {
            parameter.CurrentDeclarationMode = DeclarationMode.Exports;
            return null;
        }

        private AbstractSyntaxPart EndVisitItem(ExportsSection exportsSection, TreeTransformerOptions parameter) {
            parameter.CurrentDeclarationMode = DeclarationMode.Unknown;
            return null;
        }


        #endregion
        #region ExportItem

        private AbstractSyntaxPart BeginVisitItem(ExportItem exportsSection, TreeTransformerOptions parameter) {
            var declarations = parameter.LastValue as IDeclaredSymbolTarget;
            ExportedMethodDeclaration result = CreateNode<ExportedMethodDeclaration>(parameter, exportsSection);
            result.Name = ExtractSymbolName(exportsSection.ExportName);
            result.IsResident = exportsSection.Resident;
            result.HasIndex = exportsSection.IndexParameter != null;
            result.HasName = exportsSection.NameParameter != null;
            declarations.Symbols.AddDirect(result, parameter.LogSource);
            return result;
        }

        #endregion
        #region RecordItem

        private AbstractSyntaxPart BeginVisitItem(RecordItem recordDeclarationItem, TreeTransformerOptions parameter) {
            var parentType = parameter.LastValue as StructuredType;

            if (parentType == null)
                return null;

            if (recordDeclarationItem.Visibility != TokenKind.Undefined) {
                parameter.CurrentMemberVisibility[parentType] = StructuredType.MapVisibility(recordDeclarationItem.Visibility, recordDeclarationItem.Strict);
            };

            return null;
        }

        #endregion
        #region RecordDeclaration

        private AbstractSyntaxPart BeginVisitItem(RecordDeclaration recordDeclaration, TreeTransformerOptions parameter) {
            StructuredType result = CreateNode<StructuredType>(parameter, recordDeclaration);
            result.Kind = StructuredTypeKind.Record;
            parameter.DefineTypeValue(result);
            parameter.CurrentMemberVisibility[result] = MemberVisibility.Public;
            return result;
        }

        private void EndVisitItem(RecordDeclaration recordDeclaration, TreeTransformerOptions parameter) {
            var parentType = parameter.LastValue as StructuredType;
            parameter.CurrentMemberVisibility.Reset(parentType);
        }

        #endregion
        #region RecordField

        private AbstractSyntaxPart BeginVisitItem(RecordField fieldDeclaration, TreeTransformerOptions parameter) {
            StructuredType structType = null;
            StructureVariant varFields = null;
            IList<StructureFields> fields = null;

            if (parameter.LastValue is StructureVariantFields) {
                structType = parameter.LastValue.Parent?.Parent as StructuredType;
                varFields = structType.Variants;
                fields = (parameter.LastValue as StructureVariantFields)?.Fields;
            }
            else {
                structType = parameter.LastValue as StructuredType;
                fields = structType.Fields.Items;
            }

            var declItem = fieldDeclaration.Parent as RecordItem;
            StructureFields result = CreateNode<StructureFields>(parameter, fieldDeclaration);
            result.Visibility = parameter.CurrentMemberVisibility[structType];

            if (fields != null)
                fields.Add(result);

            IList<SymbolAttribute> extractedAttributes = null;
            if (declItem != null)
                extractedAttributes = ExtractAttributes(declItem.Attributes, parameter.CurrentUnit);

            foreach (ISyntaxPart part in fieldDeclaration.Names.Parts) {
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

                if (varFields == null)
                    structType.Fields.Add(fieldName, parameter.LogSource);
                else
                    varFields.Add(fieldName, parameter.LogSource);

                result.Fields.Add(fieldName);
                extractedAttributes = null;
            }

            result.Hints = ExtractHints(fieldDeclaration.Hint);
            return result;
        }


        #endregion
        #region ParseRecordVariantSection

        private AbstractSyntaxPart BeginVisitItem(RecordVariantSection variantSection, TreeTransformerOptions parameter) {
            var structType = parameter.LastValue as StructuredType;
            StructureVariantItem result = CreateNode<StructureVariantItem>(parameter, variantSection);
            result.Name = ExtractSymbolName(variantSection.Name);
            structType.Variants.Items.Add(result);
            return result;
        }

        #endregion
        #region RecordVariant

        private AbstractSyntaxPart BeginVisitItem(RecordVariant variantItem, TreeTransformerOptions parameter) {
            var structType = parameter.LastValue as StructureVariantItem;
            StructureVariantFields result = CreateNode<StructureVariantFields>(parameter, variantItem);
            structType.Items.Add(result);
            return result;
        }

        #endregion
        #region RecordHelperDefinition       

        private AbstractSyntaxPart BeginVisitItem(RecordHelperDefinition recordHelper, TreeTransformerOptions parameter) {
            StructuredType result = CreateNode<StructuredType>(parameter, recordHelper);
            result.Kind = StructuredTypeKind.RecordHelper;
            parameter.DefineTypeValue(result);
            parameter.CurrentMemberVisibility[result] = MemberVisibility.Public;
            return result;
        }

        private void EndVisitItem(RecordHelperDefinition classDeclaration, TreeTransformerOptions parameter) {
            var parentType = parameter.LastValue as StructuredType;
            parameter.CurrentMemberVisibility.Reset(parentType);
        }

        #endregion
        #region RecordHelperItem

        private AbstractSyntaxPart BeginVisitItem(RecordHelperItem recordDeclarationItem, TreeTransformerOptions parameter) {
            var parentType = parameter.LastValue as StructuredType;

            if (parentType == null)
                return null;

            if (recordDeclarationItem.Visibility != TokenKind.Undefined) {
                parameter.CurrentMemberVisibility[parentType] = StructuredType.MapVisibility(recordDeclarationItem.Visibility, recordDeclarationItem.Strict);
            };

            return null;
        }

        #endregion
        #region ObjectDeclaration       

        private AbstractSyntaxPart BeginVisitItem(ObjectDeclaration objectDeclaration, TreeTransformerOptions parameter) {
            StructuredType result = CreateNode<StructuredType>(parameter, objectDeclaration);
            result.Kind = StructuredTypeKind.Object;
            parameter.DefineTypeValue(result);
            parameter.CurrentMemberVisibility[result] = MemberVisibility.Public;
            return result;
        }

        private void EndVisitItem(ObjectDeclaration objectDeclaration, TreeTransformerOptions parameter) {
            var parentType = parameter.LastValue as StructuredType;
            parameter.CurrentMemberVisibility.Reset(parentType);
        }


        #endregion
        #region ObjectItem

        private AbstractSyntaxPart BeginVisitItem(ObjectItem objectItem, TreeTransformerOptions parameter) {
            var parentType = parameter.LastValue as StructuredType;

            if (parentType == null)
                return null;

            if (objectItem.Visibility != TokenKind.Undefined) {
                parameter.CurrentMemberVisibility[parentType] = StructuredType.MapVisibility(objectItem.Visibility, objectItem.Strict);
            };

            return null;
        }

        #endregion
        #region InterfaceDefinition

        private AbstractSyntaxPart BeginVisitItem(InterfaceDefinition interfaceDeclaration, TreeTransformerOptions parameter) {
            StructuredType result = CreateNode<StructuredType>(parameter, interfaceDeclaration);
            if (interfaceDeclaration.DisplayInterface)
                result.Kind = StructuredTypeKind.DispInterface;
            else
                result.Kind = StructuredTypeKind.Interface;

            result.ForwardDeclaration = interfaceDeclaration.ForwardDeclaration;
            parameter.DefineTypeValue(result);
            parameter.CurrentMemberVisibility[result] = MemberVisibility.Public;
            return result;
        }

        private void EndVisitItem(InterfaceDefinition interfaceDeclaration, TreeTransformerOptions parameter) {
            var parentType = parameter.LastValue as StructuredType;
            parameter.CurrentMemberVisibility.Reset(parentType);
        }

        #endregion
        #region InterfaceGuid

        private AbstractSyntaxPart BeginVisitItem(InterfaceGuid interfaceGuid, TreeTransformerOptions parameter) {
            var structType = parameter.LastValue as StructuredType;

            if (interfaceGuid.IdIdentifier != null) {
                structType.GuidName = ExtractSymbolName(interfaceGuid.IdIdentifier);
            }
            else if (interfaceGuid.Id != null) {
                structType.GuidId = QuotedStringTokenValue.Unwrap(interfaceGuid.Id.FirstTerminalToken);
            }


            return null;
        }

        #endregion
        #region ClassHelper

        private AbstractSyntaxPart BeginVisitItem(ClassHelperDef classHelper, TreeTransformerOptions parameter) {
            StructuredType result = CreateNode<StructuredType>(parameter, classHelper);
            result.Kind = StructuredTypeKind.ClassHelper;
            parameter.DefineTypeValue(result);
            parameter.CurrentMemberVisibility[result] = MemberVisibility.Public;
            return result;
        }

        private void EndVisitItem(ClassHelperDef classHelper, TreeTransformerOptions parameter) {
            var parentType = parameter.LastValue as StructuredType;
            parameter.CurrentMemberVisibility.Reset(parentType);
        }


        #endregion
        #region ClassHelperItem

        private AbstractSyntaxPart BeginVisitItem(ClassHelperItem classHelperItem, TreeTransformerOptions parameter) {
            var parentType = parameter.LastValue as StructuredType;

            if (parentType == null)
                return null;

            if (classHelperItem.Visibility != TokenKind.Undefined) {
                parameter.CurrentMemberVisibility[parentType] = StructuredType.MapVisibility(classHelperItem.Visibility, classHelperItem.Strict);
            };

            return null;
        }

        #endregion
        #region ProcedureDeclaration

        private AbstractSyntaxPart BeginVisitItem(ProcedureDeclaration procedure, TreeTransformerOptions parameter) {
            var symbolTarget = parameter.LastValue as IDeclaredSymbolTarget;
            MethodImplementation result = CreateNode<MethodImplementation>(parameter, procedure);
            result.Name = ExtractSymbolName(procedure.Heading.Name);
            result.Kind = Abstract.MethodDeclaration.MapKind(procedure.Heading.Kind);
            symbolTarget.Symbols.AddDirect(result, parameter.LogSource);
            return result;
        }

        #endregion
        #region MethodDeclaration

        private AbstractSyntaxPart BeginVisitItem(Standard.MethodDeclaration method, TreeTransformerOptions parameter) {
            CompilationUnit unit = parameter.CurrentUnit;
            GenericSymbolName name = ExtractSymbolName(method.Heading.Qualifiers);
            MethodImplementation result = CreateNode<MethodImplementation>(parameter, method);
            result.Kind = Abstract.MethodDeclaration.MapKind(method.Heading.Kind);
            result.Name = name;

            DeclaredSymbol type = unit.InterfaceSymbols.Find(name.NamespaceParts);

            if (type == null)
                type = unit.ImplementationSymbols.Find(name.NamespaceParts);

            var typeDecl = type as Abstract.TypeDeclaration;
            var typeStruct = typeDecl.TypeValue as StructuredType;
            StructureMethod declaration = typeStruct.Methods[name.Name];
            declaration.Implementation = result;

            return result;
        }

        #endregion
        #region StatementPart

        private AbstractSyntaxPart BeginVisitItem(StatementPart part, TreeTransformerOptions parameter) {
            if (part.Assignment != null) {
                Assignment result = CreateNode<Assignment>(parameter, part);
                var parent = parameter.LastValue as IStatementTarget;
                parent.Statements.Add(result);
                return result;
            };
            return null;
        }

        #endregion
        #region ClosureExpression

        private AbstractSyntaxPart BeginVisitItem(ClosureExpression closure, TreeTransformerOptions parameter) {
            MethodImplementation result = CreateNode<MethodImplementation>(parameter, closure);

            result.Name = new SimpleSymbolName(parameter.CurrentUnit.GenerateSymbolName());
            IExpressionTarget expression = parameter.LastExpression;
            expression.Value = result;
            return result;
        }

        #endregion
        #region RaiseStatement

        private AbstractSyntaxPart BeginVisitItem(RaiseStatement raise, TreeTransformerOptions parameter) {
            StructuredStatement result = CreateNode<StructuredStatement>(parameter, raise);
            var target = parameter.LastValue as IStatementTarget;
            target.Statements.Add(result);

            if (raise.Raise != null && raise.At == null) {
                result.Kind = StructuredStatementKind.Raise;
            }
            else if (raise.Raise != null && raise.At != null) {
                result.Kind = StructuredStatementKind.RaiseAt;
            }
            else if (raise.Raise == null && raise.At != null) {
                result.Kind = StructuredStatementKind.RaiseAtOnly;
            }
            return result;
        }

        #endregion
        #region TryStatement

        private AbstractSyntaxPart BeginVisitItem(TryStatement tryStatement, TreeTransformerOptions parameter) {
            StructuredStatement result = CreateNode<StructuredStatement>(parameter, tryStatement);
            var target = parameter.LastValue as IStatementTarget;
            target.Statements.Add(result);

            if (tryStatement.Finally != null) {
                result.Kind = StructuredStatementKind.TryFinally;
            }
            else if (tryStatement.Handlers != null) {
                result.Kind = StructuredStatementKind.TryExcept;
            }

            return result;
        }

        private AbstractSyntaxPart BeginVisitChildItem(TryStatement tryStatement, TreeTransformerOptions parameter, ISyntaxPart child) {
            var statements = child as StatementList;
            if (statements == null)
                return null;

            BlockOfStatements result = CreateNode<BlockOfStatements>(parameter, child);
            var target = parameter.LastValue as IStatementTarget;
            target.Statements.Add(result);
            return result;
        }

        #endregion
        #region ExceptHandlers

        private AbstractSyntaxPart BeginVisitItem(ExceptHandlers exceptHandlers, TreeTransformerOptions parameter) {
            StructuredStatement result = CreateNode<StructuredStatement>(parameter, exceptHandlers);
            var target = parameter.LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.ExceptElse;
            target.Statements.Add(result);
            return result;
        }

        #endregion
        #region ExceptHandler

        private AbstractSyntaxPart BeginVisitItem(ExceptHandler exceptHandler, TreeTransformerOptions parameter) {
            StructuredStatement result = CreateNode<StructuredStatement>(parameter, exceptHandler);
            var target = parameter.LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.ExceptOn;
            result.Name = ExtractSymbolName(exceptHandler.Name);
            target.Statements.Add(result);
            return result;
        }

        #endregion
        #region WithStatement

        private AbstractSyntaxPart BeginVisitItem(WithStatement withStatement, TreeTransformerOptions parameter) {
            StructuredStatement result = CreateNode<StructuredStatement>(parameter, withStatement);
            var target = parameter.LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.With;
            target.Statements.Add(result);
            return result;
        }

        #endregion
        #region ForStatement

        private AbstractSyntaxPart BeginVisitItem(ForStatement forStatement, TreeTransformerOptions parameter) {
            StructuredStatement result = CreateNode<StructuredStatement>(parameter, forStatement);
            var target = parameter.LastValue as IStatementTarget;

            switch (forStatement.Kind) {
                case TokenKind.To:
                    result.Kind = StructuredStatementKind.ForTo;
                    break;
                case TokenKind.DownTo:
                    result.Kind = StructuredStatementKind.ForDownTo;
                    break;
                case TokenKind.In:
                    result.Kind = StructuredStatementKind.ForIn;
                    break;
            }

            result.Name = ExtractSymbolName(forStatement.Variable);
            target.Statements.Add(result);
            return result;
        }

        #endregion
        #region WhileStatement

        private AbstractSyntaxPart BeginVisitItem(WhileStatement withStatement, TreeTransformerOptions parameter) {
            StructuredStatement result = CreateNode<StructuredStatement>(parameter, withStatement);
            var target = parameter.LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.While;
            target.Statements.Add(result);
            return result;
        }

        #endregion
        #region RepeatStatement

        private AbstractSyntaxPart BeginVisitItem(RepeatStatement repeateStatement, TreeTransformerOptions parameter) {
            StructuredStatement result = CreateNode<StructuredStatement>(parameter, repeateStatement);
            var target = parameter.LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.Repeat;
            target.Statements.Add(result);
            return result;
        }

        #endregion


        #region AsmBlock

        private AbstractSyntaxPart BeginVisitItem(AsmBlock block, TreeTransformerOptions parameter) {
            var statementTarget = parameter.LastValue as IStatementTarget;
            var blockTarget = parameter.LastValue as IBlockTarget;
            BlockOfAssemblerStatements result = CreatePartNode<BlockOfAssemblerStatements>(parameter.LastValue, block);
            if (statementTarget != null)
                statementTarget.Statements.Add(result);
            else if (blockTarget != null)
                blockTarget.Block = result;
            return result;
        }

        #endregion
        #region AsmPseudoOp

        private AbstractSyntaxPart BeginVisitItem(AsmPseudoOp op, TreeTransformerOptions parameter) {
            var statementTarget = parameter.LastValue as BlockOfAssemblerStatements;
            AssemblerStatement result = CreatePartNode<AssemblerStatement>(parameter.LastValue, op);

            if (op.ParamsOperation) {
                result.Kind = AssemblerStatementKind.ParamsOperation;
                ConstantValue operand = CreateNode<ConstantValue>(parameter, op.NumberOfParams);
                operand.Kind = ConstantValueKind.Integer;
                operand.IntValue = DigitTokenGroupValue.Unwrap(op.NumberOfParams.FirstTerminalToken);
                result.Operands.Add(operand);
            }
            else if (op.PushEnvOperation) {
                result.Kind = AssemblerStatementKind.PushEnvOperation;
                SymbolReference operand = CreateNode<SymbolReference>(parameter, op.Register);
                operand.Name = ExtractSymbolName(op.Register);
                result.Operands.Add(operand);
            }
            else if (op.SaveEnvOperation) {
                result.Kind = AssemblerStatementKind.SaveEnvOperation;
                SymbolReference operand = CreateNode<SymbolReference>(parameter, op.Register);
                operand.Name = ExtractSymbolName(op.Register);
                result.Operands.Add(operand);
            }
            else if (op.NoFrame) {
                result.Kind = AssemblerStatementKind.NoFrameOperation;
            }

            statementTarget.Statements.Add(result);
            return result;
        }

        #endregion
        #region LocalAsmLabel

        private AbstractSyntaxPart BeginVisitItem(LocalAsmLabel label, TreeTransformerOptions parameter) {
            var value = string.Empty;
            foreach (ISyntaxPart token in label.Parts) {
                var terminal = token as Terminal;
                var integer = token as StandardInteger;
                var ident = token as Identifier;

                if (terminal != null)
                    value = string.Concat(value, terminal.Value);

                if (integer != null)
                    value = string.Concat(value, integer.FirstTerminalToken.Value);

                if (ident != null)
                    value = string.Concat(value, ident.FirstTerminalToken.Value);

            }

            var parent = parameter.LastValue as ILabelTarget;
            parent.LabelName = new SimpleSymbolName(value);
            return null;
        }

        #endregion
        #region AsmStatement

        private AbstractSyntaxPart BeginVisitItem(AsmStatement statement, TreeTransformerOptions parameter) {
            AssemblerStatement result = CreateNode<AssemblerStatement>(parameter, statement);
            var parent = parameter.LastValue as BlockOfAssemblerStatements;
            parent.Statements.Add(result);
            result.OpCode = ExtractSymbolName(statement.OpCode?.OpCode);
            result.SegmentPrefix = ExtractSymbolName(statement.Prefix?.SegmentPrefix);
            result.LockPrefix = ExtractSymbolName(statement.Prefix?.LockPrefix);

            return result;
        }

        #endregion
        #region ParseAssemblyOperand

        private AbstractSyntaxPart BeginVisitItem(AsmOperand statement, TreeTransformerOptions parameter) {

            if (statement.LeftTerm != null && statement.RightTerm != null) {
                BinaryOperator currentExpression = CreateNode<BinaryOperator>(parameter, statement);
                parameter.DefineExpressionValue(currentExpression);
                currentExpression.Kind = BinaryOperator.ConvertKind(statement.Kind);
                return currentExpression;
            }

            if (statement.NotExpression != null) {
                UnaryOperator currentExpression = CreateNode<UnaryOperator>(parameter, statement);
                parameter.DefineExpressionValue(currentExpression);
                currentExpression.Kind = ExpressionKind.Not;
                return currentExpression;
            }

            return null;
        }

        #endregion
        #region AsmExpression

        private AbstractSyntaxPart BeginVisitItem(AsmExpression statement, TreeTransformerOptions parameter) {

            if (statement.Offset != null) {
                UnaryOperator currentExpression = CreateNode<UnaryOperator>(parameter, statement);
                parameter.DefineExpressionValue(currentExpression);
                currentExpression.Kind = ExpressionKind.AsmOffset;
                return currentExpression;
            }

            if (statement.BytePtrKind != null) {
                UnaryOperator currentExpression = CreateNode<UnaryOperator>(parameter, statement);
                parameter.DefineExpressionValue(currentExpression);
                currentExpression.Kind = UnaryOperator.MapKind(ExtractSymbolName(statement.BytePtrKind)?.CompleteName);
                return currentExpression;
            }

            if (statement.TypeExpression != null) {
                UnaryOperator currentExpression = CreateNode<UnaryOperator>(parameter, statement);
                parameter.DefineExpressionValue(currentExpression);
                currentExpression.Kind = ExpressionKind.AsmType;
                return currentExpression;
            }

            if (statement.RightOperand != null) {
                BinaryOperator currentExpression = CreateNode<BinaryOperator>(parameter, statement);
                parameter.DefineExpressionValue(currentExpression);
                currentExpression.Kind = BinaryOperator.ConvertKind(statement.BinaryOperatorKind);
                return currentExpression;
            }

            return null;
        }

        #endregion
        #region AsmTerm

        private AbstractSyntaxPart BeginVisitItem(AsmTerm statement, TreeTransformerOptions parameter) {

            if (statement.Kind != TokenKind.Undefined) {
                BinaryOperator currentExpression = CreateNode<BinaryOperator>(parameter, statement);
                parameter.DefineExpressionValue(currentExpression);
                currentExpression.Kind = BinaryOperator.ConvertKind(statement.Kind);
                return currentExpression;
            }

            if (statement.Subtype != null) {
                BinaryOperator currentExpression = CreateNode<BinaryOperator>(parameter, statement);
                parameter.DefineExpressionValue(currentExpression);
                currentExpression.Kind = ExpressionKind.Dot;
                return currentExpression;
            }

            return null;
        }

        #endregion
        #region DesignatorStatement

        private AbstractSyntaxPart BeginVisitItem(DesignatorStatement designator, TreeTransformerOptions parameter) {
            if (!designator.Inherited && designator.Name == null)
                return null;

            SymbolReference result = CreateNode<SymbolReference>(parameter, designator);
            if (designator.Inherited)
                result.Inherited = true;

            parameter.DefineExpressionValue(result);
            return result;
        }

        #endregion
        #region DesignatorItem

        private AbstractSyntaxPart BeginVisitItem(DesignatorItem designator, TreeTransformerOptions parameter) {
            var parent = parameter.LastValue as SymbolReference;

            if (designator.Dereference) {
                SymbolReferencePart part = CreateNode<SymbolReferencePart>(parameter, designator);
                parent.AddPart(part);
                part.Kind = SymbolReferencePartKind.Dereference;
                return part;
            }

            if (designator.Subitem != null) {
                SymbolReferencePart part = CreateNode<SymbolReferencePart>(parameter, designator);
                parent.AddPart(part);
                part.Kind = SymbolReferencePartKind.SubItem;
                part.Name = ExtractSymbolName(designator.Subitem);
                part.GenericType = ExtractGenericDefinition(part, designator.SubitemGenericType, parameter);
                return (AbstractSyntaxPart)part.GenericType ?? part;
            }

            if (designator.IndexExpression != null) {
                SymbolReferencePart part = CreateNode<SymbolReferencePart>(parameter, designator);
                parent.AddPart(part);
                part.Kind = SymbolReferencePartKind.ArrayIndex;
                return part;
            }

            if (designator.ParameterList) {
                SymbolReferencePart part = CreateNode<SymbolReferencePart>(parameter, designator);
                parent.AddPart(part);
                part.Kind = SymbolReferencePartKind.CallParameters;
                return part;
            }

            return null;
        }

        #endregion
        #region Parameter

        private AbstractSyntaxPart BeginVisitItem(Parameter param, TreeTransformerOptions parameter) {
            if (param.ParameterName == null)
                return null;

            SymbolReference result = CreateNode<SymbolReference>(parameter, param);
            result.NamedParameter = true;
            result.Name = ExtractSymbolName(param.ParameterName);
            parameter.DefineExpressionValue(result);
            return result;
        }

        #endregion
        #region FormattedExpression

        private AbstractSyntaxPart BeginVisitItem(Standard.FormattedExpression expr, TreeTransformerOptions parameter) {
            if (expr.Width == null && expr.Decimals == null)
                return null;

            Abstract.FormattedExpression result = CreateNode<Abstract.FormattedExpression>(parameter, expr);
            parameter.DefineExpressionValue(result);
            return result;
        }

        #endregion
        #region SetSection

        private AbstractSyntaxPart BeginVisitItem(SetSection expr, TreeTransformerOptions parameter) {
            SetExpression result = CreateNode<SetExpression>(parameter, expr);
            parameter.DefineExpressionValue(result);
            return result;
        }

        #endregion

        private AbstractSyntaxPart BeginVisitItem(SetSectnPart part, TreeTransformerOptions parameter) {
            if (part.Continuation != TokenKind.DotDot) {
                var arrayExpression = parameter.LastExpression as SetExpression;

                if (arrayExpression == null)
                    return null;

                var binOp = arrayExpression.Expressions.LastOrDefault() as BinaryOperator;

                if (binOp != null && binOp.RightOperand == null)
                    return binOp;

                return null;
            }

            BinaryOperator result = CreateNode<BinaryOperator>(parameter, part);
            result.Kind = ExpressionKind.RangeOperator;
            parameter.DefineExpressionValue(result);
            return result;
        }

        #region AsmFactor

        private AbstractSyntaxPart BeginVisitItem(AsmFactor factor, TreeTransformerOptions parameter) {

            if (factor.Number != null) {
                ConstantValue value = CreateNode<ConstantValue>(parameter, factor);
                value.Kind = ConstantValueKind.Integer;
                parameter.DefineExpressionValue(value);
                return value;
            }

            if (factor.RealNumber != null) {
                ConstantValue value = CreateNode<ConstantValue>(parameter, factor);
                value.Kind = ConstantValueKind.RealNumber;
                parameter.DefineExpressionValue(value);
                return value;
            }

            if (factor.HexNumber != null) {
                ConstantValue value = CreateNode<ConstantValue>(parameter, factor);
                value.Kind = ConstantValueKind.HexNumber;
                parameter.DefineExpressionValue(value);
                return value;
            }

            if (factor.QuotedString != null) {
                ConstantValue value = CreateNode<ConstantValue>(parameter, factor);
                value.Kind = ConstantValueKind.QuotedString;
                parameter.DefineExpressionValue(value);
                return value;
            }

            if (factor.Identifier != null) {
                SymbolReference value = CreateNode<SymbolReference>(parameter, factor);
                value.Name = ExtractSymbolName(factor.Identifier);
                parameter.DefineExpressionValue(value);
                return value;
            }

            if (factor.SegmentPrefix != null) {
                BinaryOperator currentExpression = CreateNode<BinaryOperator>(parameter, factor);
                parameter.DefineExpressionValue(currentExpression);
                currentExpression.Kind = ExpressionKind.AsmSegmentPrefix;
                SymbolReference reference = CreatePartNode<SymbolReference>(currentExpression, factor);
                reference.Name = ExtractSymbolName(factor.SegmentPrefix);
                currentExpression.LeftOperand = reference;
                return currentExpression;
            }

            if (factor.MemorySubexpression != null) {
                UnaryOperator currentExpression = CreateNode<UnaryOperator>(parameter, factor);
                parameter.DefineExpressionValue(currentExpression);
                currentExpression.Kind = ExpressionKind.AsmMemorySubexpression;
                return currentExpression;
            }

            if (factor.Label != null) {
                SymbolReference reference = CreateNode<SymbolReference>(parameter, factor);
                parameter.DefineExpressionValue(reference);
                return reference;
            }

            return null;
        }

        #endregion

        #region Extractors

        private static SymbolName ExtractSymbolName(NamespaceName name) {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var result = new NamespacedSymbolName();

            foreach (var nspace in name.Namespace)
                if (!string.IsNullOrWhiteSpace(nspace))
                    result.Append(nspace);

            if (!string.IsNullOrWhiteSpace(name.Name))
                result.Append(name.Name);

            return result;
        }

        private GenericSymbolName ExtractSymbolName(IList<MethodDeclarationName> qualifiers) {
            var result = new GenericSymbolName();

            foreach (MethodDeclarationName name in qualifiers) {
                if (name.Name != null) {
                    foreach (var namePart in name.Name.Namespace)
                        if (!string.IsNullOrWhiteSpace(namePart))
                            result.AddName(namePart);
                    if (!string.IsNullOrWhiteSpace(name.Name.Name))
                        result.AddName(name.Name.Name);
                    if (name.GenericDefinition != null) {
                        foreach (ISyntaxPart part in name.GenericDefinition.Parts) {
                            var idPart = part as Identifier;

                            if (idPart != null) {
                                result.AddGenericPart(SyntaxPartBase.IdentifierValue(idPart));
                                continue;
                            }

                            var genericPart = part as GenericDefinitionPart;

                            if (genericPart != null) {
                                result.AddGenericPart(SyntaxPartBase.IdentifierValue(genericPart.Identifier));
                            }
                        }
                    }
                }
            }

            return result;
        }

        private static SymbolName ExtractSymbolName(Identifier name) {
            var result = new SimpleSymbolName(name?.FirstTerminalToken?.Value);
            return result;
        }

        private static SymbolName ExtractSymbolName(NamespaceFileName name) {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var result = new NamespacedSymbolFileName();

            foreach (var nspace in name.NamespaceName?.Namespace)
                if (!string.IsNullOrWhiteSpace(nspace))
                    result.Append(nspace);

            if (!string.IsNullOrWhiteSpace(name.NamespaceName?.Name))
                result.Append(name.NamespaceName?.Name);

            return result;
        }

        private static GenericTypes ExtractGenericDefinition(AbstractSyntaxPart parent, GenericSuffix genericDefinition, TreeTransformerOptions parameter) {
            if (genericDefinition == null)
                return null;

            GenericTypes result = CreatePartNode<GenericTypes>(parent, genericDefinition);
            result.TypeReference = true;

            return result;
        }

        private static GenericTypes ExtractGenericDefinition(AbstractSyntaxPart parent, GenericDefinition genericDefinition, TreeTransformerOptions parameter) {
            if (genericDefinition == null)
                return null;

            GenericTypes result = CreatePartNode<GenericTypes>(parent, genericDefinition);

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
                ExtractHints(hint, result);
            }

            return result;
        }

        private static SymbolHints ExtractHints(HintingInformation hint, SymbolHints result = null) {
            if (result == null)
                result = new SymbolHints();

            result.SymbolIsDeprecated = result.SymbolIsDeprecated || hint.Deprecated;
            result.DeprecatedInformation = (result.DeprecatedInformation ?? string.Empty) + hint.DeprecatedComment?.UnquotedValue;
            result.SymbolInLibrary = result.SymbolInLibrary || hint.Library;
            result.SymbolIsPlatformSpecific = result.SymbolIsPlatformSpecific || hint.Platform;
            result.SymbolIsExperimental = result.SymbolIsExperimental || hint.Experimental;
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
