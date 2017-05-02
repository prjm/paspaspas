using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Standard;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.Tokenizer;
using System;
using System.Linq;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Infrastructure.Utils;

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
    ///     convert a concrete syntax tree to an abstract one
    /// </summary>
    public class TreeTransformer : IStartVisitor<Unit> {

        private Visitor visitor;

        public IStartEndVisitor AsVisitor() {
            return visitor;
        }

        #region Unit

        public void StartVisit(Unit unit) {
            CompilationUnit result = CreatePartNode<CompilationUnit>(Project, unit);
            result.FileType = CompilationUnitType.Unit;
            result.UnitName = ExtractSymbolName(unit.UnitName);
            result.Hints = ExtractHints(unit.Hints);
            result.FilePath = unit.FilePath;
            result.InterfaceSymbols = new DeclaredSymbols() { Parent = result };
            result.ImplementationSymbols = new DeclaredSymbols() { Parent = result };
            Project.Add(result, LogSource);
            CurrentUnit = result;
        }

        private void EndVisitItem(Unit unit)
            => CurrentUnit = null;

        #endregion
        #region Library

        private AbstractSyntaxPart BeginVisitItem(Library library) {
            CompilationUnit result = CreatePartNode<CompilationUnit>(Project, library);
            result.FileType = CompilationUnitType.Library;
            result.UnitName = ExtractSymbolName(library.LibraryName);
            result.Hints = ExtractHints(library.Hints);
            result.FilePath = library.FilePath;
            if (library.MainBlock.Body.AssemblerBlock != null)
                result.InitializationBlock = new BlockOfAssemblerStatements();
            else
                result.InitializationBlock = new BlockOfStatements();
            result.Symbols = new DeclaredSymbols() { Parent = result };
            Project.Add(result, LogSource);
            CurrentUnitMode[result] = UnitMode.Library;
            CurrentUnit = result;
            return result;
        }

        private void EndVisitItem(Library library) {
            CurrentUnitMode.Reset(CurrentUnit);
            CurrentUnit = null;
        }

        #endregion
        #region Program

        /// <summary>
        ///     visit a program
        /// </summary>
        /// <param name="program"></param>
        /// <param name="parameter"></param>
        private AbstractSyntaxPart BeginVisitItem(Program program) {
            CompilationUnit result = CreatePartNode<CompilationUnit>(Project, program);
            result.FileType = CompilationUnitType.Program;
            result.UnitName = ExtractSymbolName(program.ProgramName);
            result.FilePath = program.FilePath;
            result.InitializationBlock = new BlockOfStatements();
            result.Symbols = new DeclaredSymbols() { Parent = result };
            CurrentUnitMode[result] = UnitMode.Program;
            Project.Add(result, LogSource);
            CurrentUnit = result;
            return result;
        }

        private void EndVisitItem(Program program) {
            CurrentUnitMode.Reset(CurrentUnit);
            CurrentUnit = null;
        }

        #endregion
        #region Package

        private AbstractSyntaxPart BeginVisitItem(Package package) {
            CompilationUnit result = CreatePartNode<CompilationUnit>(Project, package);
            result.FileType = CompilationUnitType.Package;
            result.UnitName = ExtractSymbolName(package.PackageName);
            result.FilePath = package.FilePath;
            Project.Add(result, LogSource);
            CurrentUnit = result;
            return result;
        }

        private void EndVisitItem(Package package)
            => CurrentUnit = null;

        #endregion
        #region UnitInterface

        private AbstractSyntaxPart BeginVisitItem(UnitInterface unitInterface) {
            CurrentUnitMode[CurrentUnit] = UnitMode.Interface;
            CurrentUnit.Symbols = CurrentUnit.InterfaceSymbols;
            return CurrentUnit.InterfaceSymbols;
        }


        private void EndVisitItem(UnitInterface unitInterface) {
            CurrentUnitMode.Reset(CurrentUnit);
            CurrentUnit.Symbols = null; ;
        }

        #endregion
        #region UnitImplementation

        private AbstractSyntaxPart BeginVisitItem(UnitImplementation unitImplementation) {
            CurrentUnitMode[CurrentUnit] = UnitMode.Implementation;
            CurrentUnit.Symbols = CurrentUnit.ImplementationSymbols;
            return CurrentUnit.ImplementationSymbols;
        }

        private void EndVisitItem(UnitImplementation unit) {
            CurrentUnit.Symbols = null;
            CurrentUnitMode.Reset(CurrentUnit);
        }


        #endregion
        #region ConstSection

        private AbstractSyntaxPart BeginVisitItem(ConstSection constSection) {
            if (constSection.Kind == TokenKind.Const) {
                CurrentDeclarationMode = DeclarationMode.Const;
            }
            else if (constSection.Kind == TokenKind.Resourcestring) {
                CurrentDeclarationMode = DeclarationMode.ResourceString;
            }
            return null;
        }

        private void EndVisitItem(ConstSection constSection)
            => CurrentDeclarationMode = DeclarationMode.Unknown;

        #endregion
        #region TypeSection

        private AbstractSyntaxPart BeginVisitItem(TypeSection typeSection) {
            CurrentDeclarationMode = DeclarationMode.Types;
            return null;
        }

        private void EndVisitItem(TypeSection typeSection)
            => CurrentDeclarationMode = DeclarationMode.Unknown;

        #endregion
        #region TypeDeclaration

        private AbstractSyntaxPart BeginVisitItem(Standard.TypeDeclaration typeDeclaration) {
            var symbols = LastValue as IDeclaredSymbolTarget;
            Abstract.TypeDeclaration declaration = CreateNode<Abstract.TypeDeclaration>(typeDeclaration);
            declaration.Name = ExtractSymbolName(typeDeclaration.TypeId?.Identifier);
            declaration.Generics = ExtractGenericDefinition(declaration, typeDeclaration.TypeId?.GenericDefinition);
            declaration.Attributes = ExtractAttributes(typeDeclaration.Attributes, CurrentUnit);
            declaration.Hints = ExtractHints(typeDeclaration.Hint);
            symbols.Symbols.AddDirect(declaration, LogSource);
            return declaration;
        }

        #endregion
        #region ConstDeclaration

        private AbstractSyntaxPart BeginVisitItem(ConstDeclaration constDeclaration) {
            var symbols = LastValue as IDeclaredSymbolTarget;
            ConstantDeclaration declaration = CreateNode<ConstantDeclaration>(constDeclaration);
            declaration.Name = ExtractSymbolName(constDeclaration.Identifier);
            declaration.Mode = CurrentDeclarationMode;
            declaration.Attributes = ExtractAttributes(constDeclaration.Attributes, CurrentUnit);
            declaration.Hints = ExtractHints(constDeclaration.Hint);
            symbols.Symbols.AddDirect(declaration, LogSource);
            return declaration;
        }

        #endregion

        #region VarSection

        private AbstractSyntaxPart BeginVisitItem(LabelDeclarationSection lblSection) {
            CurrentDeclarationMode = DeclarationMode.Label;
            return null;
        }

        private void EndVisitItem(LabelDeclarationSection lblSection)
            => CurrentDeclarationMode = DeclarationMode.Unknown;

        #endregion
        #region VarSection

        private AbstractSyntaxPart BeginVisitItem(VarSection varSection) {
            if (varSection.Kind == TokenKind.Var)
                CurrentDeclarationMode = DeclarationMode.Var;
            else if (varSection.Kind == TokenKind.ThreadVar)
                CurrentDeclarationMode = DeclarationMode.ThreadVar;
            else
                CurrentDeclarationMode = DeclarationMode.Unknown;

            return null;
        }

        private void EndVisitItem(VarSection varSection)
            => CurrentDeclarationMode = DeclarationMode.Unknown;


        #endregion
        #region VarDeclaration

        private AbstractSyntaxPart BeginVisitItem(VarDeclaration varDeclaration) {
            var symbols = LastValue as IDeclaredSymbolTarget;
            VariableDeclaration declaration = CreateNode<VariableDeclaration>(varDeclaration);
            declaration.Mode = CurrentDeclarationMode;
            declaration.Hints = ExtractHints(varDeclaration.Hints);

            foreach (ISyntaxPart child in varDeclaration.Identifiers.Parts) {
                var ident = child as Identifier;
                if (ident != null) {
                    VariableName name = CreatePartNode<VariableName>(declaration, child);
                    name.Name = ExtractSymbolName(ident);
                    declaration.Names.Add(name);
                    symbols.Symbols.Add(name, LogSource);
                }
            }

            declaration.Attributes = ExtractAttributes(varDeclaration.Attributes, CurrentUnit);
            symbols.Symbols.Items.Add(declaration);
            return declaration;
        }

        #endregion
        #region VarValueSpecification

        private AbstractSyntaxPart BeginVisitItem(VarValueSpecification varValue) {
            var varDeclaration = LastValue as VariableDeclaration;

            if (varValue.Absolute != null)
                varDeclaration.ValueKind = VariableValueKind.Absolute;
            else if (varValue.InitialValue != null)
                varDeclaration.ValueKind = VariableValueKind.InitialValue;

            return null;
        }

        #endregion
        #region ConstantExpression

        private AbstractSyntaxPart BeginVisitItem(ConstantExpression constExpression) {

            if (constExpression.IsSetConstant) {
                SetConstant result = CreateNode<SetConstant>(constExpression);
                DefineExpressionValue(result);
                return result;
            }

            if (constExpression.IsRecordConstant) {
                RecordConstant result = CreateNode<RecordConstant>(constExpression);
                DefineExpressionValue(result);
                return result;
            }

            return null;
        }

        #endregion
        #region RecordConstantExpression

        private AbstractSyntaxPart BeginVisitItem(RecordConstantExpression constExpression) {
            RecordConstantItem expression = CreateNode<RecordConstantItem>(constExpression);
            DefineExpressionValue(expression);
            expression.Name = ExtractSymbolName(constExpression.Name);
            return expression;
        }

        #endregion
        #region Expression

        private AbstractSyntaxPart BeginVisitItem(Expression expression) {
            if (expression.LeftOperand != null && expression.RightOperand != null) {
                BinaryOperator currentExpression = CreateNode<BinaryOperator>(expression);
                DefineExpressionValue(currentExpression);
                currentExpression.Kind = BinaryOperator.ConvertKind(expression.Kind);
                return currentExpression;
            }
            return null;
        }

        #endregion
        #region SimpleExpression

        private AbstractSyntaxPart BeginVisitItem(SimpleExpression simpleExpression) {
            if (simpleExpression.LeftOperand != null && simpleExpression.RightOperand != null) {
                BinaryOperator currentExpression = CreateNode<BinaryOperator>(simpleExpression);
                DefineExpressionValue(currentExpression);
                currentExpression.Kind = BinaryOperator.ConvertKind(simpleExpression.Kind);
                return currentExpression;
            }
            return null;
        }

        #endregion       
        #region Term

        private AbstractSyntaxPart BeginVisitItem(Term term) {
            if (term.LeftOperand != null && term.RightOperand != null) {
                BinaryOperator currentExpression = CreateNode<BinaryOperator>(term);
                DefineExpressionValue(currentExpression);
                currentExpression.Kind = BinaryOperator.ConvertKind(term.Kind);
                return currentExpression;
            }
            return null;
        }

        #endregion
        #region Factor

        private AbstractSyntaxPart BeginVisitItem(Factor factor) {

            // unary operators
            if (factor.AddressOf != null || factor.Not != null || factor.Plus != null || factor.Minus != null) {
                UnaryOperator value = CreateNode<UnaryOperator>(factor);
                DefineExpressionValue(value);

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
                ConstantValue value = CreateNode<ConstantValue>(factor);
                value.Kind = ConstantValueKind.Nil;
                DefineExpressionValue(value);
                return value;
            }

            if (factor.PointerTo != null) {
                SymbolReference value = CreateNode<SymbolReference>(factor);
                value.Name = ExtractSymbolName(factor.PointerTo);
                value.PointerTo = true;
                DefineExpressionValue(value);
                return value;
            }

            if (factor.IsFalse) {
                ConstantValue value = CreateNode<ConstantValue>(factor);
                value.Kind = ConstantValueKind.False;
                DefineExpressionValue(value);
                return value;
            }
            else if (factor.IsTrue) {
                ConstantValue value = CreateNode<ConstantValue>(factor);
                value.Kind = ConstantValueKind.True;
                DefineExpressionValue(value);
                return value;
            }
            else if (factor.IntValue != null) {
                ConstantValue value = CreateNode<ConstantValue>(factor);
                value.Kind = ConstantValueKind.Integer;
                DefineExpressionValue(value);
                return value;
            }
            else if (factor.RealValue != null) {
                ConstantValue value = CreateNode<ConstantValue>(factor);
                value.Kind = ConstantValueKind.RealNumber;
                DefineExpressionValue(value);
                return value;
            }
            if (factor.StringValue != null) {
                ConstantValue value = CreateNode<ConstantValue>(factor);
                value.Kind = ConstantValueKind.QuotedString;
                DefineExpressionValue(value);
                return value;
            }
            if (factor.HexValue != null) {
                ConstantValue value = CreateNode<ConstantValue>(factor);
                value.Kind = ConstantValueKind.HexNumber;
                DefineExpressionValue(value);
                return value;
            }
            return null;
        }

        #endregion
        #region UsesClause

        private AbstractSyntaxPart BeginVisitItem(UsesClause unit) {
            if (unit.UsesList == null)
                return null;

            foreach (ISyntaxPart part in unit.UsesList.Parts) {
                var name = part as NamespaceName;
                if (name == null)
                    continue;

                RequiredUnitName unitName = CreatePartNode<RequiredUnitName>(CurrentUnit.RequiredUnits, part);
                unitName.Name = ExtractSymbolName(name);
                unitName.Mode = CurrentUnitMode[CurrentUnit];
                CurrentUnit.RequiredUnits.Add(unitName, LogSource);
            }

            return null;
        }

        #endregion
        #region UsesFileClause

        private AbstractSyntaxPart BeginVisitItem(UsesFileClause unit) {
            if (unit.Files == null)
                return null;

            foreach (ISyntaxPart part in unit.Files.Parts) {
                var name = part as NamespaceFileName;
                if (name == null)
                    continue;

                RequiredUnitName unitName = CreatePartNode<RequiredUnitName>(CurrentUnit.RequiredUnits, part);
                unitName.Name = ExtractSymbolName(name);
                unitName.Mode = CurrentUnitMode[CurrentUnit];
                unitName.FileName = name.QuotedFileName?.UnquotedValue;
                CurrentUnit.RequiredUnits.Add(unitName, LogSource);
            }

            return null;
        }

        #endregion
        #region PackageRequires

        private AbstractSyntaxPart BeginVisitItem(PackageRequires requires) {
            CurrentUnitMode[CurrentUnit] = UnitMode.Requires;

            if (requires.RequiresList == null)
                return null;

            foreach (ISyntaxPart part in requires.RequiresList.Parts) {
                var name = part as NamespaceName;
                if (name == null)
                    continue;

                RequiredUnitName unitName = CreatePartNode<RequiredUnitName>(CurrentUnit.RequiredUnits, part);
                unitName.Name = ExtractSymbolName(name);
                unitName.Mode = CurrentUnitMode[CurrentUnit];
                CurrentUnit.RequiredUnits.Add(unitName, LogSource);
            }

            return null;
        }

        private void EndVisitItem(PackageRequires requires)
            => CurrentUnitMode[CurrentUnit] = UnitMode.Interface;

        #endregion
        #region PackageContains

        private AbstractSyntaxPart BeginVisitItem(PackageContains contains) {
            CurrentUnitMode[CurrentUnit] = UnitMode.Contains;

            if (contains.ContainsList == null)
                return null;

            foreach (ISyntaxPart part in contains.ContainsList.Parts) {
                var name = part as NamespaceFileName;
                if (name == null)
                    continue;

                RequiredUnitName unitName = CreatePartNode<RequiredUnitName>(CurrentUnit.RequiredUnits, part);
                unitName.Name = ExtractSymbolName(name);
                unitName.Mode = CurrentUnitMode[CurrentUnit];
                unitName.FileName = name.QuotedFileName?.UnquotedValue;
                CurrentUnit.RequiredUnits.Add(unitName, LogSource);
            }

            return null;
        }

        private void EndVisitItem(PackageContains contains)
            => CurrentUnitMode.Reset(CurrentUnit);

        #endregion
        #region StructType

        private AbstractSyntaxPart BeginVisitItem(StructType structType) {
            if (structType.Packed)
                CurrentStructTypeMode = StructTypeMode.Packed;
            else
                CurrentStructTypeMode = StructTypeMode.Unpacked;
            return null;
        }

        private void EndVisitItem(StructType factor)
            => CurrentStructTypeMode = StructTypeMode.Undefined;

        #endregion
        #region ArrayType

        private AbstractSyntaxPart BeginVisitItem(ArrayType array) {
            ArrayTypeDeclaration value = CreateNode<ArrayTypeDeclaration>(array);
            value.PackedType = CurrentStructTypeMode == StructTypeMode.Packed;
            DefineTypeValue(value);

            if (array.ArrayOfConst) {
                MetaType metaType = CreatePartNode<MetaType>(value, array);
                metaType.Kind = MetaTypeKind.Const;
                value.TypeValue = metaType;
            }

            return value;
        }

        #endregion
        #region SetDefinition

        private AbstractSyntaxPart BeginVisitItem(SetDefinition set) {
            SetTypeDeclaration value = CreateNode<SetTypeDeclaration>(set);
            value.PackedType = CurrentStructTypeMode == StructTypeMode.Packed;
            DefineTypeValue(value);
            return value;
        }

        #endregion
        #region FileTypeDefinition

        private AbstractSyntaxPart BeginVisitItem(FileType set) {
            FileTypeDeclaration value = CreateNode<FileTypeDeclaration>(set);
            value.PackedType = CurrentStructTypeMode == StructTypeMode.Packed;
            DefineTypeValue(value);
            return value;
        }

        #endregion
        #region ClassOf

        private AbstractSyntaxPart BeginVisitItem(ClassOfDeclaration classOf) {
            ClassOfTypeDeclaration value = CreateNode<ClassOfTypeDeclaration>(classOf);
            value.PackedType = CurrentStructTypeMode == StructTypeMode.Packed;
            DefineTypeValue(value);
            return value;
        }

        #endregion
        #region TypeName                                       

        private AbstractSyntaxPart BeginVisitItem(TypeName typeName) {
            MetaType value = CreateNode<MetaType>(typeName);
            value.Kind = typeName.MapTypeKind();
            DefineTypeValue(value);
            return value;
        }

        private AbstractSyntaxPart BeginVisitChildItem(TypeName typeName, ISyntaxPart part) {
            var name = part as GenericNamespaceName;
            var value = LastValue as MetaType;

            if (name == null || value == null)
                return null;

            GenericNameFragment fragment = CreatePartNode<GenericNameFragment>(value, name);
            fragment.Name = ExtractSymbolName(name.Name);
            value.AddFragment(fragment);
            return fragment;
        }


        #endregion
        #region SimpleType

        private AbstractSyntaxPart BeginVisitItem(SimpleType simpleType) {
            if (simpleType.SubrangeStart != null) {
                SubrangeType subrange = CreateNode<SubrangeType>(simpleType);
                DefineTypeValue(subrange);
                return subrange;
            }

            if (simpleType.EnumType != null)
                return null;

            TypeAlias value = CreateNode<TypeAlias>(simpleType);
            value.IsNewType = simpleType.NewType;

            if (simpleType.TypeOf)
                LogSource.Warning(StructuralErrors.UnsupportedTypeOfConstruct, simpleType);

            DefineTypeValue(value);
            return value;
        }

        private AbstractSyntaxPart BeginVisitChildItem(SimpleType simpleType, ISyntaxPart part) {
            var name = part as GenericNamespaceName;
            var value = LastValue as TypeAlias;

            if (name == null || value == null)
                return null;

            GenericNameFragment fragment = CreatePartNode<GenericNameFragment>(value, name);
            fragment.Name = ExtractSymbolName(name.Name);
            value.AddFragment(fragment);
            return fragment;
        }

        #endregion
        #region EnumTypeDefinition

        private AbstractSyntaxPart BeginVisitItem(EnumTypeDefinition type) {
            EnumType value = CreateNode<EnumType>(type);
            DefineTypeValue(value);
            return value;
        }


        #endregion
        #region EnumValue

        private AbstractSyntaxPart BeginVisitItem(EnumValue enumValue) {
            var enumDeclaration = LastValue as EnumType;
            if (enumDeclaration != null) {
                EnumTypeValue value = CreateNode<EnumTypeValue>(enumValue);
                value.Name = ExtractSymbolName(enumValue.EnumName);
                enumDeclaration.Add(value, LogSource);
                return value;
            }
            return null;
        }

        #endregion
        #region ArrayIndex

        private AbstractSyntaxPart BeginVisitItem(ArrayIndex arrayIndex) {
            if (arrayIndex.EndIndex != null) {
                BinaryOperator binOp = CreateNode<BinaryOperator>(arrayIndex);
                DefineExpressionValue(binOp);
                binOp.Kind = ExpressionKind.RangeOperator;
                return binOp;
            }

            return null;
        }

        #endregion
        #region PointerType

        private AbstractSyntaxPart BeginVisitItem(PointerType pointer) {
            if (pointer.GenericPointer) {
                MetaType result = CreateNode<MetaType>(pointer);
                result.Kind = MetaTypeKind.Pointer;
                DefineTypeValue(result);
                return result;
            }
            else {
                PointerToType result = CreateNode<PointerToType>(pointer);
                DefineTypeValue(result);
                return result;
            }
        }

        #endregion
        #region StringType

        private AbstractSyntaxPart BeginVisitItem(StringType stringType) {
            MetaType result = CreateNode<MetaType>(stringType);
            result.Kind = MetaType.ConvertKind(stringType.Kind);
            DefineTypeValue(result);
            return result;
        }

        #endregion
        #region ProcedureTypeDefinition

        private AbstractSyntaxPart BeginVisitItem(ProcedureTypeDefinition proceduralType) {
            ProceduralType result = CreateNode<ProceduralType>(proceduralType);
            DefineTypeValue(result);
            result.Kind = Abstract.MethodDeclaration.MapKind(proceduralType.Kind);
            result.MethodDeclaration = proceduralType.MethodDeclaration;
            result.AllowAnonymousMethods = proceduralType.AllowAnonymousMethods;

            if (proceduralType.ReturnTypeAttributes != null)
                result.ReturnAttributes = ExtractAttributes(proceduralType.ReturnTypeAttributes, CurrentUnit);

            return result;
        }

        #endregion
        #region FormalParameterDefinition

        private AbstractSyntaxPart BeginVisitItem(FormalParameterDefinition formalParameter) {
            var paramterTarget = LastValue as IParameterTarget;
            ParameterTypeDefinition result = CreatePartNode<ParameterTypeDefinition>(paramterTarget.Parameters, formalParameter);
            paramterTarget.Parameters.Items.Add(result);
            return result;
        }

        #endregion
        #region FormalParameter

        private AbstractSyntaxPart BeginVisitItem(FormalParameter formalParameter) {
            ParameterDefinition result = CreateNode<ParameterDefinition>(formalParameter);
            var typeDefinition = LastValue as ParameterTypeDefinition;
            var allParams = typeDefinition.Parent as ParameterDefinitions;
            result.Name = ExtractSymbolName(formalParameter.ParameterName);
            result.Attributes = ExtractAttributes(formalParameter.Attributes, CurrentUnit);
            result.ParameterKind = ParameterDefinition.MapKind(formalParameter.ParameterType);
            typeDefinition.Parameters.Add(result);
            allParams.Add(result, LogSource);
            return result;
        }

        #endregion   
        #region UnitInitialization

        private AbstractSyntaxPart BeginVisitItem(UnitInitialization unitBlock) {
            BlockOfStatements result = CreatePartNode<BlockOfStatements>(CurrentUnit, unitBlock);
            CurrentUnit.InitializationBlock = result;
            return result;
        }

        #endregion
        #region UnitFinalization

        private AbstractSyntaxPart BeginVisitItem(UnitFinalization unitBlock) {
            BlockOfStatements result = CreatePartNode<BlockOfStatements>(CurrentUnit, unitBlock);
            CurrentUnit.FinalizationBlock = result;
            return result;
        }

        #endregion
        #region CompoundStatement

        private AbstractSyntaxPart BeginVisitItem(CompoundStatement block) {

            if (block.AssemblerBlock != null) {
                var statementTarget = LastValue as IStatementTarget;
                var blockTarget = LastValue as IBlockTarget;
                BlockOfAssemblerStatements result = CreatePartNode<BlockOfAssemblerStatements>(LastValue, block);
                if (statementTarget != null)
                    statementTarget.Statements.Add(result);
                else if (blockTarget != null)
                    blockTarget.Block = result;
                return result;
            }

            else {
                var statementTarget = LastValue as IStatementTarget;
                var blockTarget = LastValue as IBlockTarget;
                BlockOfStatements result = CreatePartNode<BlockOfStatements>(LastValue, block);
                if (statementTarget != null)
                    statementTarget.Statements.Add(result);
                else if (blockTarget != null)
                    blockTarget.Block = result;
                return result;
            }

        }

        #endregion
        #region Label

        private AbstractSyntaxPart BeginVisitItem(Label label) {
            SymbolName name = null;

            var standardLabel = label.LabelName as Identifier;
            if (standardLabel != null) {
                name = ExtractSymbolName(standardLabel);
            }

            Token intLabel = (label.LabelName as StandardInteger)?.LastTerminalToken;
            if (intLabel != null) {
                name = new SimpleSymbolName(intLabel.Value);
            }

            Token hexLabel = (label.LabelName as HexNumber)?.LastTerminalToken;
            if (hexLabel != null) {
                name = new SimpleSymbolName(hexLabel.Value);
            }

            if (name == null)
                return null;

            if (CurrentDeclarationMode == DeclarationMode.Label) {
                var symbols = LastValue as IDeclaredSymbolTarget;
                ConstantDeclaration declaration = CreateNode<ConstantDeclaration>(label);
                declaration.Name = name;
                declaration.Mode = CurrentDeclarationMode;
                symbols.Symbols.AddDirect(declaration, LogSource);
                return declaration;
            }

            var parent = LastValue as ILabelTarget;
            if (parent == null)
                return null;
            parent.LabelName = name;
            return null;
        }

        #endregion
        #region ClassDeclaration

        private AbstractSyntaxPart BeginVisitItem(ClassDeclaration classDeclaration) {
            StructuredType result = CreateNode<StructuredType>(classDeclaration);
            result.Kind = StructuredTypeKind.Class;
            result.SealedClass = classDeclaration.Sealed;
            result.AbstractClass = classDeclaration.Abstract;
            result.ForwardDeclaration = classDeclaration.ForwardDeclaration;
            DefineTypeValue(result);
            CurrentMemberVisibility[result] = MemberVisibility.Public;
            return result;
        }

        private void EndVisitItem(ClassDeclaration classDeclaration) {
            var parentType = LastValue as StructuredType;
            CurrentMemberVisibility.Reset(parentType);
        }

        #endregion
        #region ClassDeclarationItem

        private AbstractSyntaxPart BeginVisitItem(ClassDeclarationItem classDeclarationItem) {
            var parentType = LastValue as StructuredType;

            if (parentType == null)
                return null;

            if (classDeclarationItem.Visibility != TokenKind.Undefined) {
                CurrentMemberVisibility[parentType] = StructuredType.MapVisibility(classDeclarationItem.Visibility, classDeclarationItem.Strict);
            };

            return null;
        }

        #endregion
        #region ClassField

        private AbstractSyntaxPart BeginVisitItem(ClassField field) {
            var structType = LastValue as StructuredType;
            var declItem = field.Parent as IStructuredTypeMember;
            StructureFields result = CreateNode<StructureFields>(field);
            result.Visibility = CurrentMemberVisibility[structType];
            structType.Fields.Items.Add(result);
            IList<SymbolAttribute> extractedAttributes = ExtractAttributes(declItem.Attributes, CurrentUnit);
            result.ClassItem = declItem.ClassItem;

            foreach (ISyntaxPart part in field.Names.Parts) {
                var attrs = part as UserAttributes;

                if (attrs != null) {
                    extractedAttributes = ExtractAttributes(attrs, CurrentUnit, extractedAttributes);
                    continue;
                }

                var partName = part as Identifier;
                if (partName == null)
                    continue;

                StructureField fieldName = CreatePartNode<StructureField>(result, partName);
                fieldName.Name = ExtractSymbolName(partName);
                fieldName.Attributes = extractedAttributes;
                structType.Fields.Add(fieldName, LogSource);
                result.Fields.Add(fieldName);
                extractedAttributes = null;
            }

            result.Hints = ExtractHints(field.Hint);

            return result;
        }

        #endregion
        #region ClassProperty

        private AbstractSyntaxPart BeginVisitItem(ClassProperty property) {
            StructureProperty result = CreateNode<StructureProperty>(property);
            var parent = LastValue as StructuredType;
            var declItem = property.Parent as IStructuredTypeMember;
            result.Name = ExtractSymbolName(property.PropertyName);
            parent.Properties.Add(result, LogSource);
            result.Visibility = CurrentMemberVisibility[parent];

            if (declItem != null) {
                result.Attributes = ExtractAttributes(declItem.Attributes, CurrentUnit);
            }

            return result;
        }

        #endregion    
        #region ClassPropertyReadWrite

        private AbstractSyntaxPart BeginVisitItem(ClassPropertyReadWrite property) {
            var parent = LastValue as StructureProperty;
            StructurePropertyAccessor result = CreateNode<StructurePropertyAccessor>(property);
            result.Kind = StructurePropertyAccessor.MapKind(property.Kind);
            result.Name = ExtractSymbolName(property.Member);
            parent.Accessors.Add(result);
            return result;
        }

        #endregion
        #region  ClassPropertyDispInterface

        private AbstractSyntaxPart BeginVisitItem(ClassPropertyDispInterface property) {
            var parent = LastValue as StructureProperty;
            StructurePropertyAccessor result = CreateNode<StructurePropertyAccessor>(property);

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

        private AbstractSyntaxPart BeginVisitItem(ClassPropertySpecifier property) {
            if (property.PropertyReadWrite != null)
                return null;

            if (property.PropertyDispInterface != null)
                return null;

            var parent = LastValue as StructureProperty;
            StructurePropertyAccessor result = CreateNode<StructurePropertyAccessor>(property);

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

        private AbstractSyntaxPart BeginVisitItem(ClassMethod method) {
            StructureMethod result = CreateNode<StructureMethod>(method);
            var parent = LastValue as StructuredType;
            result.Visibility = CurrentMemberVisibility[parent];

            var declItem = method.Parent as IStructuredTypeMember;
            if (declItem != null) {
                result.ClassItem = declItem.ClassItem;
                result.Attributes = ExtractAttributes(declItem.Attributes, CurrentUnit);
            }

            result.Name = ExtractSymbolName(method.Identifier);
            result.Kind = Abstract.MethodDeclaration.MapKind(method.MethodKind);
            result.Generics = ExtractGenericDefinition(result, method.GenericDefinition);
            parent.Methods.Add(result, LogSource);
            return result;
        }

        #endregion
        #region MethodResolution

        private AbstractSyntaxPart BeginVisitItem(MethodResolution methodResolution) {
            StructureMethodResolution result = CreateNode<StructureMethodResolution>(methodResolution);
            var parent = LastValue as StructuredType;
            result.Attributes = ExtractAttributes(((ClassDeclarationItem)methodResolution.Parent).Attributes, CurrentUnit);
            result.Kind = StructureMethodResolution.MapKind(methodResolution.Kind);
            result.Target = ExtractSymbolName(methodResolution.ResolveIdentifier);
            parent.MethodResolutions.Add(result);
            return result;
        }

        #endregion
        #region ReintroduceDirective

        private AbstractSyntaxPart BeginVisitItem(ReintroduceDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(directive);
            result.Kind = MethodDirectiveKind.Reintroduce;
            parent.Directives.Add(result);
            return result;
        }

        #endregion
        #region OverloadDirective

        private AbstractSyntaxPart BeginVisitItem(OverloadDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(directive);
            result.Kind = MethodDirectiveKind.Overload;
            parent.Directives.Add(result);
            return result;
        }

        #endregion
        #region DispIdDirective

        private AbstractSyntaxPart BeginVisitItem(DispIdDirective directive) {
            var parent = LastValue as IDirectiveTarget;

            if (parent == null)
                return null;

            MethodDirective result = CreateNode<MethodDirective>(directive);
            result.Kind = MethodDirectiveKind.DispId;
            parent.Directives.Add(result);
            return result;
        }

        #endregion
        #region InlineDirective

        private AbstractSyntaxPart BeginVisitItem(InlineDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(directive);

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

        private AbstractSyntaxPart BeginVisitItem(AbstractDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(directive);

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

        private AbstractSyntaxPart BeginVisitItem(OldCallConvention directive) {
            var parent = LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(directive);

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

        private AbstractSyntaxPart BeginVisitItem(ExternalDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(directive);

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

        private AbstractSyntaxPart BeginVisitItem(ExternalSpecifier directive) {
            var parent = LastValue as MethodDirective;
            MethodDirectiveSpecifier result = CreateNode<MethodDirectiveSpecifier>(directive);
            result.Kind = MethodDirectiveSpecifier.MapKind(directive.Kind);
            parent.Specifiers.Add(result);
            return result;
        }

        #endregion
        #region CallConvention

        private AbstractSyntaxPart BeginVisitItem(CallConvention directive) {
            var parent = LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(directive);

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

        private AbstractSyntaxPart BeginVisitItem(BindingDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(directive);

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

        private AbstractSyntaxPart BeginVisitChildItem(MethodDirectives parent, ISyntaxPart child) {
            var hints = child as HintingInformation;
            var lastValue = LastValue as IDirectiveTarget;

            if (hints != null && lastValue != null) {
                lastValue.Hints = ExtractHints(hints, lastValue.Hints);
            }

            return null;
        }

        #endregion
        #region FunctionDirectives

        private AbstractSyntaxPart BeginVisitChildItem(FunctionDirectives parent, ISyntaxPart child) {
            var hints = child as HintingInformation;
            var lastValue = LastValue as IDirectiveTarget;

            if (hints != null && lastValue != null) {
                lastValue.Hints = ExtractHints(hints, lastValue.Hints);
            }

            return null;
        }

        #endregion
        #region ExportedProcedureHeading

        private AbstractSyntaxPart BeginVisitItem(ExportedProcedureHeading procHeading) {
            var symbols = LastValue as IDeclaredSymbolTarget;
            GlobalMethod result = CreateNode<GlobalMethod>(procHeading);
            result.Name = ExtractSymbolName(procHeading.Name);
            result.Kind = Abstract.MethodDeclaration.MapKind(procHeading.Kind);
            result.Attributes = ExtractAttributes(procHeading.Attributes, CurrentUnit);
            result.ReturnAttributes = ExtractAttributes(procHeading.ResultAttributes, CurrentUnit);
            symbols.Symbols.AddDirect(result, LogSource);
            return result;
        }

        #endregion
        #region UnsafeDirective

        private AbstractSyntaxPart BeginVisitItem(UnsafeDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(directive);
            result.Kind = MethodDirectiveKind.Unsafe;
            parent.Directives.Add(result);
            return result;
        }

        #endregion
        #region ForwardDirective

        private AbstractSyntaxPart BeginVisitItem(ForwardDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(directive);
            result.Kind = MethodDirectiveKind.Forward;
            parent.Directives.Add(result);
            return result;
        }

        #endregion
        #region ExportsSection

        private AbstractSyntaxPart BeginVisitItem(ExportsSection exportsSection) {
            CurrentDeclarationMode = DeclarationMode.Exports;
            return null;
        }

        private AbstractSyntaxPart EndVisitItem(ExportsSection exportsSection) {
            CurrentDeclarationMode = DeclarationMode.Unknown;
            return null;
        }


        #endregion
        #region ExportItem

        private AbstractSyntaxPart BeginVisitItem(ExportItem exportsSection) {
            var declarations = LastValue as IDeclaredSymbolTarget;
            ExportedMethodDeclaration result = CreateNode<ExportedMethodDeclaration>(exportsSection);
            result.Name = ExtractSymbolName(exportsSection.ExportName);
            result.IsResident = exportsSection.Resident;
            result.HasIndex = exportsSection.IndexParameter != null;
            result.HasName = exportsSection.NameParameter != null;
            declarations.Symbols.AddDirect(result, LogSource);
            return result;
        }

        #endregion
        #region RecordItem

        private AbstractSyntaxPart BeginVisitItem(RecordItem recordDeclarationItem) {
            var parentType = LastValue as StructuredType;

            if (parentType == null)
                return null;

            if (recordDeclarationItem.Visibility != TokenKind.Undefined) {
                CurrentMemberVisibility[parentType] = StructuredType.MapVisibility(recordDeclarationItem.Visibility, recordDeclarationItem.Strict);
            };

            return null;
        }

        #endregion
        #region RecordDeclaration

        private AbstractSyntaxPart BeginVisitItem(RecordDeclaration recordDeclaration) {
            StructuredType result = CreateNode<StructuredType>(recordDeclaration);
            result.Kind = StructuredTypeKind.Record;
            DefineTypeValue(result);
            CurrentMemberVisibility[result] = MemberVisibility.Public;
            return result;
        }

        private void EndVisitItem(RecordDeclaration recordDeclaration) {
            var parentType = LastValue as StructuredType;
            CurrentMemberVisibility.Reset(parentType);
        }

        #endregion
        #region RecordField

        private AbstractSyntaxPart BeginVisitItem(RecordField fieldDeclaration) {
            StructuredType structType = null;
            StructureVariant varFields = null;
            IList<StructureFields> fields = null;

            if (LastValue is StructureVariantFields) {
                structType = LastValue.Parent?.Parent as StructuredType;
                varFields = structType.Variants;
                fields = (LastValue as StructureVariantFields)?.Fields;
            }
            else {
                structType = LastValue as StructuredType;
                fields = structType.Fields.Items;
            }

            var declItem = fieldDeclaration.Parent as RecordItem;
            StructureFields result = CreateNode<StructureFields>(fieldDeclaration);
            result.Visibility = CurrentMemberVisibility[structType];

            if (fields != null)
                fields.Add(result);

            IList<SymbolAttribute> extractedAttributes = null;
            if (declItem != null)
                extractedAttributes = ExtractAttributes(declItem.Attributes, CurrentUnit);

            foreach (ISyntaxPart part in fieldDeclaration.Names.Parts) {
                var attrs = part as UserAttributes;

                if (attrs != null) {
                    extractedAttributes = ExtractAttributes(attrs, CurrentUnit, extractedAttributes);
                    continue;
                }

                var partName = part as Identifier;
                if (partName == null)
                    continue;

                StructureField fieldName = CreatePartNode<StructureField>(result, partName);
                fieldName.Name = ExtractSymbolName(partName);
                fieldName.Attributes = extractedAttributes;

                if (varFields == null)
                    structType.Fields.Add(fieldName, LogSource);
                else
                    varFields.Add(fieldName, LogSource);

                result.Fields.Add(fieldName);
                extractedAttributes = null;
            }

            result.Hints = ExtractHints(fieldDeclaration.Hint);
            return result;
        }


        #endregion
        #region ParseRecordVariantSection

        private AbstractSyntaxPart BeginVisitItem(RecordVariantSection variantSection) {
            var structType = LastValue as StructuredType;
            StructureVariantItem result = CreateNode<StructureVariantItem>(variantSection);
            result.Name = ExtractSymbolName(variantSection.Name);
            structType.Variants.Items.Add(result);
            return result;
        }

        #endregion
        #region RecordVariant

        private AbstractSyntaxPart BeginVisitItem(RecordVariant variantItem) {
            var structType = LastValue as StructureVariantItem;
            StructureVariantFields result = CreateNode<StructureVariantFields>(variantItem);
            structType.Items.Add(result);
            return result;
        }

        #endregion
        #region RecordHelperDefinition       

        private AbstractSyntaxPart BeginVisitItem(RecordHelperDefinition recordHelper) {
            StructuredType result = CreateNode<StructuredType>(recordHelper);
            result.Kind = StructuredTypeKind.RecordHelper;
            DefineTypeValue(result);
            CurrentMemberVisibility[result] = MemberVisibility.Public;
            return result;
        }

        private void EndVisitItem(RecordHelperDefinition classDeclaration) {
            var parentType = LastValue as StructuredType;
            CurrentMemberVisibility.Reset(parentType);
        }

        #endregion
        #region RecordHelperItem

        private AbstractSyntaxPart BeginVisitItem(RecordHelperItem recordDeclarationItem) {
            var parentType = LastValue as StructuredType;

            if (parentType == null)
                return null;

            if (recordDeclarationItem.Visibility != TokenKind.Undefined) {
                CurrentMemberVisibility[parentType] = StructuredType.MapVisibility(recordDeclarationItem.Visibility, recordDeclarationItem.Strict);
            };

            return null;
        }

        #endregion
        #region ObjectDeclaration       

        private AbstractSyntaxPart BeginVisitItem(ObjectDeclaration objectDeclaration) {
            StructuredType result = CreateNode<StructuredType>(objectDeclaration);
            result.Kind = StructuredTypeKind.Object;
            DefineTypeValue(result);
            CurrentMemberVisibility[result] = MemberVisibility.Public;
            return result;
        }

        private void EndVisitItem(ObjectDeclaration objectDeclaration) {
            var parentType = LastValue as StructuredType;
            CurrentMemberVisibility.Reset(parentType);
        }


        #endregion
        #region ObjectItem

        private AbstractSyntaxPart BeginVisitItem(ObjectItem objectItem) {
            var parentType = LastValue as StructuredType;

            if (parentType == null)
                return null;

            if (objectItem.Visibility != TokenKind.Undefined) {
                CurrentMemberVisibility[parentType] = StructuredType.MapVisibility(objectItem.Visibility, objectItem.Strict);
            };

            return null;
        }

        #endregion
        #region InterfaceDefinition

        private AbstractSyntaxPart BeginVisitItem(InterfaceDefinition interfaceDeclaration) {
            StructuredType result = CreateNode<StructuredType>(interfaceDeclaration);
            if (interfaceDeclaration.DisplayInterface)
                result.Kind = StructuredTypeKind.DispInterface;
            else
                result.Kind = StructuredTypeKind.Interface;

            result.ForwardDeclaration = interfaceDeclaration.ForwardDeclaration;
            DefineTypeValue(result);
            CurrentMemberVisibility[result] = MemberVisibility.Public;
            return result;
        }

        private void EndVisitItem(InterfaceDefinition interfaceDeclaration) {
            var parentType = LastValue as StructuredType;
            CurrentMemberVisibility.Reset(parentType);
        }

        #endregion
        #region InterfaceGuid

        private AbstractSyntaxPart BeginVisitItem(InterfaceGuid interfaceGuid) {
            var structType = LastValue as StructuredType;

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

        private AbstractSyntaxPart BeginVisitItem(ClassHelperDef classHelper) {
            StructuredType result = CreateNode<StructuredType>(classHelper);
            result.Kind = StructuredTypeKind.ClassHelper;
            DefineTypeValue(result);
            CurrentMemberVisibility[result] = MemberVisibility.Public;
            return result;
        }

        private void EndVisitItem(ClassHelperDef classHelper) {
            var parentType = LastValue as StructuredType;
            CurrentMemberVisibility.Reset(parentType);
        }


        #endregion
        #region ClassHelperItem

        private AbstractSyntaxPart BeginVisitItem(ClassHelperItem classHelperItem) {
            var parentType = LastValue as StructuredType;

            if (parentType == null)
                return null;

            if (classHelperItem.Visibility != TokenKind.Undefined) {
                CurrentMemberVisibility[parentType] = StructuredType.MapVisibility(classHelperItem.Visibility, classHelperItem.Strict);
            };

            return null;
        }

        #endregion
        #region ProcedureDeclaration

        private AbstractSyntaxPart BeginVisitItem(ProcedureDeclaration procedure) {
            var symbolTarget = LastValue as IDeclaredSymbolTarget;
            MethodImplementation result = CreateNode<MethodImplementation>(procedure);
            result.Name = ExtractSymbolName(procedure.Heading.Name);
            result.Kind = Abstract.MethodDeclaration.MapKind(procedure.Heading.Kind);
            symbolTarget.Symbols.AddDirect(result, LogSource);
            return result;
        }

        #endregion
        #region MethodDeclaration

        private AbstractSyntaxPart BeginVisitItem(Standard.MethodDeclaration method) {
            CompilationUnit unit = CurrentUnit;
            GenericSymbolName name = ExtractSymbolName(method.Heading.Qualifiers);
            MethodImplementation result = CreateNode<MethodImplementation>(method);
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

        private AbstractSyntaxPart BeginVisitItem(StatementPart part) {

            if (part.DesignatorPart == null && part.Assignment == null)
                return null;

            StructuredStatement result = CreateNode<StructuredStatement>(part);
            var target = LastValue as IStatementTarget;
            if (part.Assignment != null) {
                result.Kind = StructuredStatementKind.Assignment;
            }
            else {
                result.Kind = StructuredStatementKind.ExpressionStatement;
            }

            target.Statements.Add(result);
            return result;
        }

        #endregion
        #region ClosureExpression

        private AbstractSyntaxPart BeginVisitItem(ClosureExpression closure) {
            MethodImplementation result = CreateNode<MethodImplementation>(closure);

            result.Name = new SimpleSymbolName(CurrentUnit.GenerateSymbolName());
            IExpressionTarget expression = LastExpression;
            expression.Value = result;
            return result;
        }

        #endregion
        #region RaiseStatement

        private AbstractSyntaxPart BeginVisitItem(RaiseStatement raise) {
            StructuredStatement result = CreateNode<StructuredStatement>(raise);
            var target = LastValue as IStatementTarget;
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

        private AbstractSyntaxPart BeginVisitItem(TryStatement tryStatement) {
            StructuredStatement result = CreateNode<StructuredStatement>(tryStatement);
            var target = LastValue as IStatementTarget;
            target.Statements.Add(result);

            if (tryStatement.Finally != null) {
                result.Kind = StructuredStatementKind.TryFinally;
            }
            else if (tryStatement.Handlers != null) {
                result.Kind = StructuredStatementKind.TryExcept;
            }

            return result;
        }

        private AbstractSyntaxPart BeginVisitChildItem(TryStatement tryStatement, ISyntaxPart child) {
            var statements = child as StatementList;
            if (statements == null)
                return null;

            BlockOfStatements result = CreateNode<BlockOfStatements>(child);
            var target = LastValue as IStatementTarget;
            target.Statements.Add(result);
            return result;
        }

        #endregion
        #region ExceptHandlers

        private AbstractSyntaxPart BeginVisitItem(ExceptHandlers exceptHandlers) {
            StructuredStatement result = CreateNode<StructuredStatement>(exceptHandlers);
            var target = LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.ExceptElse;
            target.Statements.Add(result);
            return result;
        }

        #endregion
        #region ExceptHandler

        private AbstractSyntaxPart BeginVisitItem(ExceptHandler exceptHandler) {
            StructuredStatement result = CreateNode<StructuredStatement>(exceptHandler);
            var target = LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.ExceptOn;
            result.Name = ExtractSymbolName(exceptHandler.Name);
            target.Statements.Add(result);
            return result;
        }

        #endregion
        #region WithStatement

        private AbstractSyntaxPart BeginVisitItem(WithStatement withStatement) {
            StructuredStatement result = CreateNode<StructuredStatement>(withStatement);
            var target = LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.With;
            target.Statements.Add(result);
            return result;
        }

        #endregion
        #region ForStatement

        private AbstractSyntaxPart BeginVisitItem(ForStatement forStatement) {
            StructuredStatement result = CreateNode<StructuredStatement>(forStatement);
            var target = LastValue as IStatementTarget;

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

        private AbstractSyntaxPart BeginVisitItem(WhileStatement withStatement) {
            StructuredStatement result = CreateNode<StructuredStatement>(withStatement);
            var target = LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.While;
            target.Statements.Add(result);
            return result;
        }

        #endregion
        #region RepeatStatement

        private AbstractSyntaxPart BeginVisitItem(RepeatStatement repeateStatement) {
            StructuredStatement result = CreateNode<StructuredStatement>(repeateStatement);
            var target = LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.Repeat;
            target.Statements.Add(result);
            return result;
        }

        #endregion
        #region CaseStatement

        private AbstractSyntaxPart BeginVisitItem(CaseStatement caseStatement) {
            StructuredStatement result = CreateNode<StructuredStatement>(caseStatement);
            var target = LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.Case;
            target.Statements.Add(result);
            return result;
        }

        private AbstractSyntaxPart BeginVisitChildItem(CaseStatement caseStatement, ISyntaxPart child) {

            if (caseStatement.Else != child)
                return null;

            StructuredStatement result = CreateNode<StructuredStatement>(caseStatement);
            var target = LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.CaseElse;
            target.Statements.Add(result);
            return result;
        }

        #endregion
        #region CaseItem

        private AbstractSyntaxPart BeginVisitItem(CaseItem caseItem) {
            StructuredStatement result = CreateNode<StructuredStatement>(caseItem);
            var target = LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.CaseItem;
            target.Statements.Add(result);
            return result;
        }

        #endregion
        #region CaseLabel

        private AbstractSyntaxPart BeginVisitItem(CaseLabel caseLabel) {
            if (caseLabel.EndExpression != null) {
                BinaryOperator binOp = CreateNode<BinaryOperator>(caseLabel);
                DefineExpressionValue(binOp);
                binOp.Kind = ExpressionKind.RangeOperator;
                return binOp;
            }

            return null;
        }

        #endregion
        #region IfStatement

        private AbstractSyntaxPart BeginVisitItem(IfStatement ifStatement) {
            StructuredStatement result = CreateNode<StructuredStatement>(ifStatement);
            var target = LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.IfThen;
            target.Statements.Add(result);
            return result;
        }

        private AbstractSyntaxPart BeginVisitChildItem(IfStatement ifStatement, ISyntaxPart child) {

            if (ifStatement.ElsePart != child)
                return null;

            StructuredStatement result = CreateNode<StructuredStatement>(ifStatement);
            var target = LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.IfElse;
            target.Statements.Add(result);
            return result;
        }

        #endregion
        #region GoToStatement

        private AbstractSyntaxPart BeginVisitItem(GoToStatement gotoStatement) {
            StructuredStatement result = CreateNode<StructuredStatement>(gotoStatement);
            var target = LastValue as IStatementTarget;

            if (gotoStatement.Break)
                result.Kind = StructuredStatementKind.Break;
            else if (gotoStatement.Continue)
                result.Kind = StructuredStatementKind.Continue;
            else if (gotoStatement.GoToLabel != null)
                result.Kind = StructuredStatementKind.GoToLabel;
            else if (gotoStatement.Exit)
                result.Kind = StructuredStatementKind.Exit;

            target.Statements.Add(result);
            return result;
        }

        #endregion
        #region AsmBlock

        private AbstractSyntaxPart BeginVisitItem(AsmBlock block) {
            var statementTarget = LastValue as IStatementTarget;
            var blockTarget = LastValue as IBlockTarget;
            BlockOfAssemblerStatements result = CreatePartNode<BlockOfAssemblerStatements>(LastValue, block);
            if (statementTarget != null)
                statementTarget.Statements.Add(result);
            else if (blockTarget != null)
                blockTarget.Block = result;
            return result;
        }

        #endregion
        #region AsmPseudoOp

        private AbstractSyntaxPart BeginVisitItem(AsmPseudoOp op) {
            var statementTarget = LastValue as BlockOfAssemblerStatements;
            AssemblerStatement result = CreatePartNode<AssemblerStatement>(LastValue, op);

            if (op.ParamsOperation) {
                result.Kind = AssemblerStatementKind.ParamsOperation;
                ConstantValue operand = CreateNode<ConstantValue>(op.NumberOfParams);
                operand.Kind = ConstantValueKind.Integer;
                operand.IntValue = DigitTokenGroupValue.Unwrap(op.NumberOfParams.FirstTerminalToken);
                result.Operands.Add(operand);
            }
            else if (op.PushEnvOperation) {
                result.Kind = AssemblerStatementKind.PushEnvOperation;
                SymbolReference operand = CreateNode<SymbolReference>(op.Register);
                operand.Name = ExtractSymbolName(op.Register);
                result.Operands.Add(operand);
            }
            else if (op.SaveEnvOperation) {
                result.Kind = AssemblerStatementKind.SaveEnvOperation;
                SymbolReference operand = CreateNode<SymbolReference>(op.Register);
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

        private AbstractSyntaxPart BeginVisitItem(LocalAsmLabel label) {
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

            var parent = LastValue as ILabelTarget;
            parent.LabelName = new SimpleSymbolName(value);
            return null;
        }

        #endregion
        #region AsmStatement

        private AbstractSyntaxPart BeginVisitItem(AsmStatement statement) {
            AssemblerStatement result = CreateNode<AssemblerStatement>(statement);
            var parent = LastValue as BlockOfAssemblerStatements;
            parent.Statements.Add(result);
            result.OpCode = ExtractSymbolName(statement.OpCode?.OpCode);
            result.SegmentPrefix = ExtractSymbolName(statement.Prefix?.SegmentPrefix);
            result.LockPrefix = ExtractSymbolName(statement.Prefix?.LockPrefix);

            return result;
        }

        #endregion
        #region ParseAssemblyOperand

        private AbstractSyntaxPart BeginVisitItem(AsmOperand statement) {

            if (statement.LeftTerm != null && statement.RightTerm != null) {
                BinaryOperator currentExpression = CreateNode<BinaryOperator>(statement);
                DefineExpressionValue(currentExpression);
                currentExpression.Kind = BinaryOperator.ConvertKind(statement.Kind);
                return currentExpression;
            }

            if (statement.NotExpression != null) {
                UnaryOperator currentExpression = CreateNode<UnaryOperator>(statement);
                DefineExpressionValue(currentExpression);
                currentExpression.Kind = ExpressionKind.Not;
                return currentExpression;
            }

            return null;
        }

        #endregion
        #region AsmExpression

        private AbstractSyntaxPart BeginVisitItem(AsmExpression statement) {

            if (statement.Offset != null) {
                UnaryOperator currentExpression = CreateNode<UnaryOperator>(statement);
                DefineExpressionValue(currentExpression);
                currentExpression.Kind = ExpressionKind.AsmOffset;
                return currentExpression;
            }

            if (statement.BytePtrKind != null) {
                UnaryOperator currentExpression = CreateNode<UnaryOperator>(statement);
                DefineExpressionValue(currentExpression);
                currentExpression.Kind = UnaryOperator.MapKind(ExtractSymbolName(statement.BytePtrKind)?.CompleteName);
                return currentExpression;
            }

            if (statement.TypeExpression != null) {
                UnaryOperator currentExpression = CreateNode<UnaryOperator>(statement);
                DefineExpressionValue(currentExpression);
                currentExpression.Kind = ExpressionKind.AsmType;
                return currentExpression;
            }

            if (statement.RightOperand != null) {
                BinaryOperator currentExpression = CreateNode<BinaryOperator>(statement);
                DefineExpressionValue(currentExpression);
                currentExpression.Kind = BinaryOperator.ConvertKind(statement.BinaryOperatorKind);
                return currentExpression;
            }

            return null;
        }

        #endregion
        #region AsmTerm

        private AbstractSyntaxPart BeginVisitItem(AsmTerm statement) {

            if (statement.Kind != TokenKind.Undefined) {
                BinaryOperator currentExpression = CreateNode<BinaryOperator>(statement);
                DefineExpressionValue(currentExpression);
                currentExpression.Kind = BinaryOperator.ConvertKind(statement.Kind);
                return currentExpression;
            }

            if (statement.Subtype != null) {
                BinaryOperator currentExpression = CreateNode<BinaryOperator>(statement);
                DefineExpressionValue(currentExpression);
                currentExpression.Kind = ExpressionKind.Dot;
                return currentExpression;
            }

            return null;
        }

        #endregion
        #region DesignatorStatement

        private AbstractSyntaxPart BeginVisitItem(DesignatorStatement designator) {
            if (!designator.Inherited && designator.Name == null)
                return null;

            SymbolReference result = CreateNode<SymbolReference>(designator);
            if (designator.Inherited)
                result.Inherited = true;

            DefineExpressionValue(result);
            return result;
        }

        #endregion
        #region DesignatorItem

        private AbstractSyntaxPart BeginVisitItem(DesignatorItem designator) {
            var parent = LastValue as SymbolReference;

            if (designator.Dereference) {
                SymbolReferencePart part = CreateNode<SymbolReferencePart>(designator);
                parent.AddPart(part);
                part.Kind = SymbolReferencePartKind.Dereference;
                return part;
            }

            if (designator.Subitem != null) {
                SymbolReferencePart part = CreateNode<SymbolReferencePart>(designator);
                parent.AddPart(part);
                part.Kind = SymbolReferencePartKind.SubItem;
                part.Name = ExtractSymbolName(designator.Subitem);
                part.GenericType = ExtractGenericDefinition(part, designator.SubitemGenericType);
                return (AbstractSyntaxPart)part.GenericType ?? part;
            }

            if (designator.IndexExpression != null) {
                SymbolReferencePart part = CreateNode<SymbolReferencePart>(designator);
                parent.AddPart(part);
                part.Kind = SymbolReferencePartKind.ArrayIndex;
                return part;
            }

            if (designator.ParameterList) {
                SymbolReferencePart part = CreateNode<SymbolReferencePart>(designator);
                parent.AddPart(part);
                part.Kind = SymbolReferencePartKind.CallParameters;
                return part;
            }

            return null;
        }

        #endregion
        #region Parameter

        private AbstractSyntaxPart BeginVisitItem(Parameter param) {
            if (param.ParameterName == null)
                return null;

            SymbolReference result = CreateNode<SymbolReference>(param);
            result.NamedParameter = true;
            result.Name = ExtractSymbolName(param.ParameterName);
            DefineExpressionValue(result);
            return result;
        }

        #endregion
        #region FormattedExpression

        private AbstractSyntaxPart BeginVisitItem(Standard.FormattedExpression expr) {
            if (expr.Width == null && expr.Decimals == null)
                return null;

            Abstract.FormattedExpression result = CreateNode<Abstract.FormattedExpression>(expr);
            DefineExpressionValue(result);
            return result;
        }

        #endregion
        #region SetSection

        private AbstractSyntaxPart BeginVisitItem(SetSection expr) {
            SetExpression result = CreateNode<SetExpression>(expr);
            DefineExpressionValue(result);
            return result;
        }

        #endregion

        private AbstractSyntaxPart BeginVisitItem(SetSectnPart part) {
            if (part.Continuation != TokenKind.DotDot) {
                var arrayExpression = LastExpression as SetExpression;

                if (arrayExpression == null)
                    return null;

                var binOp = arrayExpression.Expressions.LastOrDefault() as BinaryOperator;

                if (binOp != null && binOp.RightOperand == null)
                    return binOp;

                return null;
            }

            BinaryOperator result = CreateNode<BinaryOperator>(part);
            result.Kind = ExpressionKind.RangeOperator;
            DefineExpressionValue(result);
            return result;
        }

        #region AsmFactor

        private AbstractSyntaxPart BeginVisitItem(AsmFactor factor) {

            if (factor.Number != null) {
                ConstantValue value = CreateNode<ConstantValue>(factor);
                value.Kind = ConstantValueKind.Integer;
                DefineExpressionValue(value);
                return value;
            }

            if (factor.RealNumber != null) {
                ConstantValue value = CreateNode<ConstantValue>(factor);
                value.Kind = ConstantValueKind.RealNumber;
                DefineExpressionValue(value);
                return value;
            }

            if (factor.HexNumber != null) {
                ConstantValue value = CreateNode<ConstantValue>(factor);
                value.Kind = ConstantValueKind.HexNumber;
                DefineExpressionValue(value);
                return value;
            }

            if (factor.QuotedString != null) {
                ConstantValue value = CreateNode<ConstantValue>(factor);
                value.Kind = ConstantValueKind.QuotedString;
                DefineExpressionValue(value);
                return value;
            }

            if (factor.Identifier != null) {
                SymbolReference value = CreateNode<SymbolReference>(factor);
                value.Name = ExtractSymbolName(factor.Identifier);
                DefineExpressionValue(value);
                return value;
            }

            if (factor.SegmentPrefix != null) {
                BinaryOperator currentExpression = CreateNode<BinaryOperator>(factor);
                DefineExpressionValue(currentExpression);
                currentExpression.Kind = ExpressionKind.AsmSegmentPrefix;
                SymbolReference reference = CreatePartNode<SymbolReference>(currentExpression, factor);
                reference.Name = ExtractSymbolName(factor.SegmentPrefix);
                currentExpression.LeftOperand = reference;
                return currentExpression;
            }

            if (factor.MemorySubexpression != null) {
                UnaryOperator currentExpression = CreateNode<UnaryOperator>(factor);
                DefineExpressionValue(currentExpression);
                currentExpression.Kind = ExpressionKind.AsmMemorySubexpression;
                return currentExpression;
            }

            if (factor.Label != null) {
                SymbolReference reference = CreateNode<SymbolReference>(factor);
                DefineExpressionValue(reference);
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

        private static GenericTypes ExtractGenericDefinition(AbstractSyntaxPart parent, GenericSuffix genericDefinition) {
            if (genericDefinition == null)
                return null;

            GenericTypes result = CreatePartNode<GenericTypes>(parent, genericDefinition);
            result.TypeReference = true;

            return result;
        }

        private GenericTypes ExtractGenericDefinition(AbstractSyntaxPart parent, GenericDefinition genericDefinition) {
            if (genericDefinition == null)
                return null;

            GenericTypes result = CreatePartNode<GenericTypes>(parent, genericDefinition);

            foreach (ISyntaxPart part in genericDefinition.Parts) {
                var idPart = part as Identifier;

                if (idPart != null) {
                    GenericType generic = CreatePartNode<GenericType>(result, part);
                    generic.Name = ExtractSymbolName(idPart);
                    result.Add(generic, LogSource);
                    continue;
                }

                var genericPart = part as GenericDefinitionPart;

                if (genericPart != null) {
                    GenericType generic = CreatePartNode<GenericType>(result, part);
                    generic.Name = ExtractSymbolName(genericPart.Identifier);
                    result.Add(generic, LogSource);

                    foreach (ISyntaxPart constraintPart in genericPart.Parts) {
                        var constraint = constraintPart as Standard.ConstrainedGeneric;
                        if (constraint != null) {
                            GenericConstraint cr = CreatePartNode<GenericConstraint>(generic, constraint);
                            cr.Kind = GenericConstraint.MapKind(constraint);
                            cr.Name = ExtractSymbolName(constraint.ConstraintIdentifier);
                            generic.Add(cr, LogSource);
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

        private T CreateNode<T>(ISyntaxPart element) where T : AbstractSyntaxPart, new()
            => CreatePartNode<T>(LastValue, element);

        private static T CreatePartNode<T>(AbstractSyntaxPart parent, ISyntaxPart element) where T : AbstractSyntaxPart, new() {
            var result = new T() {
                Parent = parent
            };
            return result;
        }

        #endregion

        private LogSource logSource;

        /// <summary>
        ///     message group id
        /// </summary>
        private static readonly Guid MessageGroupId
            = new Guid(new byte[] { 0x9, 0x1b, 0x66, 0xae, 0xc7, 0x4c, 0xd3, 0x45, 0xb5, 0x75, 0x1d, 0x4f, 0x9, 0x7e, 0xcb, 0x68 });
        /* {ae661b09-4cc7-45d3-b575-1d4f097ecb68} */

        /// <summary>
        ///     tree states
        /// </summary>
        private IDictionary<AbstractSyntaxPart, object> currentValues
            = new Dictionary<AbstractSyntaxPart, object>();

        /// <summary>
        ///     log source
        /// </summary>
        public LogSource LogSource {
            get {
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
        public DictionaryIndexHelper<AbstractSyntaxPart, UnitMode> CurrentUnitMode { get; }

        /// <summary>
        ///     currennt member visibility
        /// </summary>
        public DictionaryIndexHelper<AbstractSyntaxPart, MemberVisibility> CurrentMemberVisibility { get; }

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
        public IExpressionTarget LastExpression {
            get {
                if (WorkingStack.Count > 0)
                    return WorkingStack.Peek().Data as IExpressionTarget;
                else
                    return null;

            }
        }

        /// <summary>
        ///     create a new options set
        /// </summary>
        public TreeTransformer() {
            CurrentUnitMode = new DictionaryIndexHelper<AbstractSyntaxPart, UnitMode>(currentValues);
            CurrentMemberVisibility = new DictionaryIndexHelper<AbstractSyntaxPart, MemberVisibility>(currentValues);
        }

        /// <summary>
        ///     struct type mode
        /// </summary>
        public StructTypeMode CurrentStructTypeMode { get; set; }
            = StructTypeMode.Undefined;

        /// <summary>
        ///     last type declaration
        /// </summary>
        public ITypeTarget LastTypeDeclaration {
            get {
                if (WorkingStack.Count > 0)
                    return WorkingStack.Peek().Data as ITypeTarget;
                else
                    return null;
            }
        }

        /// <summary>
        ///     last value from the working stack
        /// </summary>
        public AbstractSyntaxPart LastValue {
            get {
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
        public void DefineExpressionValue(IExpression value) {
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

    }
}
