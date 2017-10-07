using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Standard;
using PasPasPas.Parsing.Tokenizer;
using System;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     convert a concrete syntax tree to an abstract one
    ///     using a working stack and a generic visitor pattern
    /// </summary>
    public class TreeTransformer :

        IStartVisitor<Unit>, IEndVisitor<Unit>,
        IStartVisitor<Library>, IEndVisitor<Library>,
        IStartVisitor<Program>, IEndVisitor<Program>,
        IStartVisitor<Package>, IEndVisitor<Package>,
        IStartVisitor<UnitInterface>, IEndVisitor<UnitInterface>,
        IStartVisitor<UnitImplementation>, IEndVisitor<UnitImplementation>,
        IStartVisitor<ConstSection>, IEndVisitor<ConstSection>,
        IStartVisitor<TypeSection>, IEndVisitor<TypeSection>,
        IStartVisitor<Standard.TypeDeclaration>, IEndVisitor<Standard.TypeDeclaration>,
        IStartVisitor<ConstDeclaration>,
        IStartVisitor<LabelDeclarationSection>, IEndVisitor<LabelDeclarationSection>,
        IStartVisitor<VarSection>, IEndVisitor<VarSection>,
        IStartVisitor<VarDeclaration>,
        IStartVisitor<VarValueSpecification>, IEndVisitor<VarValueSpecification>,
        IStartVisitor<ConstantExpression>,
        IStartVisitor<RecordConstantExpression>,
        IStartVisitor<Expression>,
        IStartVisitor<SimpleExpression>,
        IStartVisitor<Term>,
        IStartVisitor<Factor>,
        IStartVisitor<UsesClause>,
        IStartVisitor<UsesFileClause>,
        IStartVisitor<PackageRequires>, IEndVisitor<PackageRequires>,
        IStartVisitor<PackageContains>, IEndVisitor<PackageContains>,
        IStartVisitor<StructType>, IEndVisitor<StructType>,
        IStartVisitor<ArrayType>,
        IStartVisitor<SetDefinition>,
        IStartVisitor<FileType>,
        IStartVisitor<ClassOfDeclaration>,
        IStartVisitor<TypeName>,
        IStartVisitor<SimpleType>,
        IStartVisitor<EnumTypeDefinition>,
        IStartVisitor<EnumValue>,
        IStartVisitor<ArrayIndex>,
        IStartVisitor<PointerType>,
        IStartVisitor<StringType>,
        IStartVisitor<ProcedureTypeDefinition>,
        IStartVisitor<FormalParameterDefinition>,
        IStartVisitor<FormalParameter>,
        IStartVisitor<UnitInitialization>,
        IStartVisitor<UnitFinalization>,
        IStartVisitor<CompoundStatement>, IEndVisitor<CompoundStatement>,
        IStartVisitor<Label>, IEndVisitor<Label>,
        IStartVisitor<ClassDeclaration>, IEndVisitor<ClassDeclaration>,
        IStartVisitor<ClassDeclarationItem>,
        IStartVisitor<ClassField>,
        IStartVisitor<ClassProperty>,
        IStartVisitor<ClassPropertyReadWrite>,
        IStartVisitor<ClassPropertyDispInterface>,
        IStartVisitor<ClassPropertySpecifier>,
        IStartVisitor<ClassMethod>,
        IStartVisitor<MethodResolution>,
        IStartVisitor<ReintroduceDirective>,
        IStartVisitor<OverloadDirective>,
        IStartVisitor<DispIdDirective>,
        IStartVisitor<InlineDirective>,
        IStartVisitor<AbstractDirective>,
        IStartVisitor<OldCallConvention>,
        IStartVisitor<ExternalDirective>,
        IStartVisitor<ExternalSpecifier>,
        IStartVisitor<CallConvention>,
        IStartVisitor<BindingDirective>,
        IStartVisitor<ExportedProcedureHeading>,
        IStartVisitor<UnsafeDirective>,
        IStartVisitor<ForwardDirective>,
        IStartVisitor<ExportsSection>,
        IStartVisitor<ExportItem>,
        IStartVisitor<RecordItem>,
        IStartVisitor<RecordDeclaration>,
        IStartVisitor<RecordField>,
        IStartVisitor<RecordVariantSection>,
        IStartVisitor<RecordVariant>,
        IStartVisitor<RecordHelperDefinition>,
        IStartVisitor<RecordHelperItem>,
        IStartVisitor<ObjectDeclaration>,
        IStartVisitor<ObjectItem>,
        IStartVisitor<InterfaceDefinition>,
        IStartVisitor<InterfaceGuid>,
        IStartVisitor<ClassHelperDef>,
        IStartVisitor<ClassHelperItem>,
        IStartVisitor<ProcedureDeclaration>,
        IStartVisitor<Standard.MethodDeclaration>,
        IStartVisitor<StatementPart>,
        IStartVisitor<ClosureExpression>,
        IStartVisitor<RaiseStatement>,
        IStartVisitor<TryStatement>,
        IStartVisitor<ExceptHandlers>,
        IStartVisitor<ExceptHandler>,
        IStartVisitor<WithStatement>,
        IStartVisitor<ForStatement>,
        IStartVisitor<WhileStatement>,
        IStartVisitor<RepeatStatement>,
        IStartVisitor<CaseStatement>,
        IStartVisitor<CaseItem>,
        IStartVisitor<CaseLabel>,
        IStartVisitor<IfStatement>,
        IStartVisitor<GoToStatement>,
        IStartVisitor<AsmBlock>,
        IStartVisitor<AsmPseudoOp>,
        IStartVisitor<LocalAsmLabel>,
        IStartVisitor<AsmStatement>,
        IStartVisitor<AsmOperand>,
        IStartVisitor<AsmExpression>,
        IStartVisitor<AsmTerm>,
        IStartVisitor<DesignatorStatement>,
        IStartVisitor<DesignatorItem>,
        IStartVisitor<Parameter>,
        IStartVisitor<Standard.FormattedExpression>,
        IStartVisitor<SetSection>,
        IStartVisitor<SetSectnPart>,
        IStartVisitor<AsmFactor>,
        IChildVisitor<CaseStatement>,
        IChildVisitor<TypeName>,
        IChildVisitor<SimpleType>,
        IChildVisitor<MethodDirectives>,
        IChildVisitor<FunctionDirectives>,
        IChildVisitor<TryStatement>,
        IChildVisitor<IfStatement> {

        private Visitor visitor;

        public IStartEndVisitor AsVisitor()
            => visitor;

        #region Unit

        public void StartVisit(Unit unit) {
            var result = new CompilationUnit();
            InitNode(result, unit, Project);
            result.FileType = CompilationUnitType.Unit;
            result.UnitName = ExtractSymbolName(unit.UnitName);
            result.Hints = ExtractHints(unit.Hints);
            result.InterfaceSymbols = new DeclaredSymbols() { ParentItem = result };
            result.ImplementationSymbols = new DeclaredSymbols() { ParentItem = result };
            result.FilePath = unit.FilePath;
            Project.Add(result, LogSource);
            CurrentUnit = result;
        }

        public void EndVisit(Unit unit)
            => CurrentUnit = null;

        #endregion
        #region Library

        public void StartVisit(Library library) {
            var result = new CompilationUnit();
            InitNode(result, library, Project);
            result.FileType = CompilationUnitType.Library;
            result.UnitName = ExtractSymbolName(library.LibraryName);
            result.Hints = ExtractHints(library.Hints);
            result.FilePath = library.FilePath;
            if (library.MainBlock.Body.AssemblerBlock != null)
                result.InitializationBlock = new BlockOfAssemblerStatements();
            else
                result.InitializationBlock = new BlockOfStatements();
            result.Symbols = new DeclaredSymbols();
            Project.Add(result, LogSource);
            CurrentUnitMode[result] = UnitMode.Library;
            CurrentUnit = result;
        }

        public void EndVisit(Library library) {
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
        public void StartVisit(Program program) {
            var result = new CompilationUnit();
            InitNode(result, program, Project);
            result.FileType = CompilationUnitType.Program;
            result.UnitName = ExtractSymbolName(program.ProgramName);
            result.InitializationBlock = new BlockOfStatements();
            result.Symbols = new DeclaredSymbols() { ParentItem = result };
            result.FilePath = program.FilePath;
            CurrentUnitMode[result] = UnitMode.Program;
            Project.Add(result, LogSource);
            CurrentUnit = result;
        }

        public void EndVisit(Program program) {
            CurrentUnitMode.Reset(CurrentUnit);
            CurrentUnit = null;
        }

        #endregion
        #region Package

        public void StartVisit(Package package) {
            var result = new CompilationUnit();
            InitNode(result, package, Project);
            result.FilePath = package.FilePath;
            result.FileType = CompilationUnitType.Package;
            result.UnitName = ExtractSymbolName(package.PackageName);
            Project.Add(result, LogSource);
            CurrentUnit = result;
        }

        public void EndVisit(Package package)
            => CurrentUnit = null;

        #endregion
        #region UnitInterface

        public void StartVisit(UnitInterface unitInterface) {
            CurrentUnitMode[CurrentUnit] = UnitMode.Interface;
            CurrentUnit.Symbols = CurrentUnit.InterfaceSymbols;
            AddToStack(unitInterface, CurrentUnit.InterfaceSymbols);
        }


        public void EndVisit(UnitInterface unitInterface) {
            CurrentUnitMode.Reset(CurrentUnit);
            CurrentUnit.Symbols = null;
        }

        #endregion
        #region UnitImplementation

        public void StartVisit(UnitImplementation unitImplementation) {
            CurrentUnitMode[CurrentUnit] = UnitMode.Implementation;
            CurrentUnit.Symbols = CurrentUnit.ImplementationSymbols;
            AddToStack(unitImplementation, CurrentUnit.ImplementationSymbols);
        }

        public void EndVisit(UnitImplementation unit) {
            CurrentUnit.Symbols = null;
            CurrentUnitMode.Reset(CurrentUnit);
        }


        #endregion
        #region ConstSection

        public void StartVisit(ConstSection constSection) {
            if (constSection.Kind == TokenKind.Const) {
                CurrentDeclarationMode = DeclarationMode.Const;
            }
            else if (constSection.Kind == TokenKind.Resourcestring) {
                CurrentDeclarationMode = DeclarationMode.ResourceString;
            }
        }

        public void EndVisit(ConstSection constSection)
            => CurrentDeclarationMode = DeclarationMode.Unknown;

        #endregion
        #region TypeSection

        public void StartVisit(TypeSection typeSection)
            => CurrentDeclarationMode = DeclarationMode.Types;

        public void EndVisit(TypeSection typeSection)
            => CurrentDeclarationMode = DeclarationMode.Unknown;

        #endregion
        #region TypeDeclaration

        public void StartVisit(Standard.TypeDeclaration typeDeclaration) {
            var symbols = LastValue as IDeclaredSymbolTarget;
            var declaration = new Abstract.TypeDeclaration();
            InitNode(declaration, typeDeclaration);
            declaration.Name = ExtractSymbolName(typeDeclaration.TypeId?.Identifier);
            declaration.Generics = ExtractGenericDefinition(declaration, typeDeclaration, typeDeclaration.TypeId?.GenericDefinition);
            declaration.Attributes = ExtractAttributes(typeDeclaration.Attributes, CurrentUnit);
            declaration.Hints = ExtractHints(typeDeclaration.Hint);
            symbols.Symbols.AddDirect(declaration, LogSource);
        }

        public void EndVisit(Standard.TypeDeclaration typeDeclaration) {
        }

        #endregion
        #region ConstDeclaration

        public void StartVisit(ConstDeclaration constDeclaration) {
            var symbols = LastValue as IDeclaredSymbolTarget;
            var declaration = new ConstantDeclaration();
            InitNode(declaration, constDeclaration);
            declaration.Name = ExtractSymbolName(constDeclaration.Identifier);
            declaration.Mode = CurrentDeclarationMode;
            declaration.Attributes = ExtractAttributes(constDeclaration.Attributes, CurrentUnit);
            declaration.Hints = ExtractHints(constDeclaration.Hint);
            symbols.Symbols.AddDirect(declaration, LogSource);
        }

        #endregion

        #region VarSection

        public void StartVisit(LabelDeclarationSection lblSection)
            => CurrentDeclarationMode = DeclarationMode.Label;

        public void EndVisit(LabelDeclarationSection lblSection)
            => CurrentDeclarationMode = DeclarationMode.Unknown;

        #endregion
        #region VarSection

        public void StartVisit(VarSection varSection) {
            if (varSection.Kind == TokenKind.Var)
                CurrentDeclarationMode = DeclarationMode.Var;
            else if (varSection.Kind == TokenKind.ThreadVar)
                CurrentDeclarationMode = DeclarationMode.ThreadVar;
            else
                CurrentDeclarationMode = DeclarationMode.Unknown;
        }

        public void EndVisit(VarSection varSection)
            => CurrentDeclarationMode = DeclarationMode.Unknown;


        #endregion
        #region VarDeclaration

        public void StartVisit(VarDeclaration varDeclaration) {
            var symbols = LastValue as IDeclaredSymbolTarget;
            var declaration = new VariableDeclaration();
            InitNode(declaration, varDeclaration);
            declaration.Mode = CurrentDeclarationMode;
            declaration.Hints = ExtractHints(varDeclaration.Hints);

            foreach (var child in varDeclaration.Identifiers.Parts) {
                if (child is Identifier ident) {
                    var name = new VariableName();
                    InitNode(name, child, declaration);
                    name.Name = ExtractSymbolName(ident);
                    declaration.Names.Add(name);
                    symbols.Symbols.Add(name, LogSource);
                    visitor.WorkingStack.Pop();
                }
            }

            declaration.Attributes = ExtractAttributes(varDeclaration.Attributes, CurrentUnit);
            symbols.Symbols.Items.Add(declaration);
        }

        #endregion
        #region VarValueSpecification

        public void StartVisit(VarValueSpecification varValue) {
            var varDeclaration = LastValue as VariableDeclaration;

            if (varValue.Absolute != null)
                varDeclaration.ValueKind = VariableValueKind.Absolute;
            else if (varValue.InitialValue != null)
                varDeclaration.ValueKind = VariableValueKind.InitialValue;
        }

        public void EndVisit(VarValueSpecification varValue) {
        }

        #endregion
        #region ConstantExpression

        public void StartVisit(ConstantExpression constExpression) {

            if (constExpression.IsArrayConstant) {
                var lastExpression = LastExpression;
                var result = new ArrayConstant();
                InitNode(result, constExpression);
                lastExpression.Value = result;
            }

            if (constExpression.IsRecordConstant) {
                var lastExpression = LastExpression;
                var result = new RecordConstant();
                InitNode(result, constExpression);
                lastExpression.Value = result;
            }

        }

        #endregion
        #region RecordConstantExpression

        public void StartVisit(RecordConstantExpression constExpression) {
            var lastExpression = LastExpression;
            var expression = new RecordConstantItem();
            InitNode(expression, constExpression);
            lastExpression.Value = expression;
            expression.Name = ExtractSymbolName(constExpression.Name);
        }

        #endregion
        #region Expression

        public void StartVisit(Expression expression) {
            if (expression.LeftOperand != null && expression.RightOperand != null) {
                var lastExpression = LastExpression;
                var currentExpression = new BinaryOperator();
                InitNode(currentExpression, expression);
                lastExpression.Value = currentExpression;
                currentExpression.Kind = TokenKindMapper.ForExpression(expression.Kind);
            }
        }

        #endregion
        #region SimpleExpression

        public void StartVisit(SimpleExpression simpleExpression) {
            if (simpleExpression.LeftOperand != null && simpleExpression.RightOperand != null) {
                var lastExpression = LastExpression;
                var currentExpression = new BinaryOperator();
                InitNode(currentExpression, simpleExpression);
                lastExpression.Value = currentExpression;
                currentExpression.Kind = TokenKindMapper.ForExpression(simpleExpression.Kind);
            }
        }

        #endregion       
        #region Term

        public void StartVisit(Term term) {
            if (term.LeftOperand != null && term.RightOperand != null) {
                var lastExpression = LastExpression;
                var currentExpression = new BinaryOperator();
                InitNode(currentExpression, term);
                lastExpression.Value = currentExpression;
                currentExpression.Kind = TokenKindMapper.ForExpression(term.Kind);
            }
        }

        #endregion
        #region Factor

        public void StartVisit(Factor factor) {

            // unary operators
            if (factor.AddressOf != null || factor.Not != null || factor.Plus != null || factor.Minus != null) {
                var lastExpression = LastExpression;
                var value = new UnaryOperator();
                InitNode(value, factor);
                lastExpression.Value = value;

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
                var lastExpression = LastExpression;
                var value = new ConstantValue();
                InitNode(value, factor);
                value.Kind = ConstantValueKind.Nil;
                lastExpression.Value = value;
                return;
            }

            if (factor.PointerTo != null) {
                var lastExpression = LastExpression;
                var value = new SymbolReference();
                InitNode(value, factor);
                value.Name = ExtractSymbolName(factor.PointerTo);
                value.PointerTo = true;
                lastExpression.Value = value;
                return;
            }

            if (factor.IsFalse) {
                var lastExpression = LastExpression;
                var value = new ConstantValue();
                InitNode(value, factor);
                value.Kind = ConstantValueKind.False;
                lastExpression.Value = value;
                return;
            }

            if (factor.IsTrue) {
                var lastExpression = LastExpression;
                var value = new ConstantValue();
                InitNode(value, factor);
                value.Kind = ConstantValueKind.True;
                lastExpression.Value = value;
                return;
            }

            if (factor.IntValue != null) {
                var lastExpression = LastExpression;
                var value = new ConstantValue();
                InitNode(value, factor);
                value.Kind = ConstantValueKind.Integer;
                value.LiteralValue = factor.IntValue.FirstTerminalToken.ParsedValue;
                lastExpression.Value = value;
                return;
            }

            if (factor.RealValue != null) {
                var lastExpression = LastExpression;
                var value = new ConstantValue();
                InitNode(value, factor);
                value.Kind = ConstantValueKind.RealNumber;
                value.LiteralValue = factor.RealValue.FirstTerminalToken.ParsedValue;
                lastExpression.Value = value;
                return;
            }

            if (factor.StringValue != null) {
                var lastExpression = LastExpression;
                var value = new ConstantValue();
                InitNode(value, factor);
                value.Kind = ConstantValueKind.QuotedString;
                value.LiteralValue = factor.StringValue.FirstTerminalToken.ParsedValue;
                lastExpression.Value = value;
                return;
            }

            if (factor.HexValue != null) {
                var lastExpression = LastExpression;
                var value = new ConstantValue();
                InitNode(value, factor);
                value.Kind = ConstantValueKind.HexNumber;
                value.LiteralValue = factor.HexValue.FirstTerminalToken.ParsedValue;
                lastExpression.Value = value;
                return;
            }
        }

        #endregion
        #region UsesClause

        public void StartVisit(UsesClause unit) {
            if (unit.UsesList == null)
                return;

            foreach (var part in unit.UsesList.Parts) {
                var name = part as NamespaceName;
                if (name == null)
                    continue;

                var unitName = new RequiredUnitName();
                InitNode(unitName, part, CurrentUnit.RequiredUnits);
                unitName.Name = ExtractSymbolName(name);
                unitName.Mode = CurrentUnitMode[CurrentUnit];
                CurrentUnit.RequiredUnits.Add(unitName, LogSource);
                visitor.WorkingStack.Pop();
            }
        }


        #endregion
        #region UsesFileClause

        public void StartVisit(UsesFileClause unit) {
            if (unit.Files == null)
                return;

            foreach (var part in unit.Files.Parts) {
                var name = part as NamespaceFileName;
                if (name == null)
                    continue;

                var unitName = new RequiredUnitName();
                InitNode(unitName, part, CurrentUnit.RequiredUnits);
                unitName.Name = ExtractSymbolName(name);
                unitName.Mode = CurrentUnitMode[CurrentUnit];
                unitName.FileName = name.QuotedFileName?.UnquotedValue;
                CurrentUnit.RequiredUnits.Add(unitName, LogSource);
            }
        }

        #endregion
        #region PackageRequires

        public void StartVisit(PackageRequires requires) {
            CurrentUnitMode[CurrentUnit] = UnitMode.Requires;

            if (requires.RequiresList == null)
                return;

            foreach (var part in requires.RequiresList.Parts) {
                var name = part as NamespaceName;
                if (name == null)
                    continue;

                var unitName = new RequiredUnitName();
                InitNode(unitName, name, CurrentUnit.RequiredUnits);
                unitName.Name = ExtractSymbolName(name);
                unitName.Mode = CurrentUnitMode[CurrentUnit];
                CurrentUnit.RequiredUnits.Add(unitName, LogSource);
            }
        }

        public void EndVisit(PackageRequires requires)
            => CurrentUnitMode[CurrentUnit] = UnitMode.Interface;

        #endregion
        #region PackageContains

        public void StartVisit(PackageContains contains) {
            CurrentUnitMode[CurrentUnit] = UnitMode.Contains;

            if (contains.ContainsList == null)
                return;

            foreach (var part in contains.ContainsList.Parts) {
                var name = part as NamespaceFileName;
                if (name == null)
                    continue;

                var unitName = new RequiredUnitName();
                InitNode(unitName, name, CurrentUnit.RequiredUnits);
                unitName.Name = ExtractSymbolName(name);
                unitName.Mode = CurrentUnitMode[CurrentUnit];
                unitName.FileName = name.QuotedFileName?.UnquotedValue;
                CurrentUnit.RequiredUnits.Add(unitName, LogSource);
            }
        }

        public void EndVisit(PackageContains contains)
            => CurrentUnitMode.Reset(CurrentUnit);

        #endregion
        #region StructType

        public void StartVisit(StructType structType) {
            if (structType.Packed)
                CurrentStructTypeMode = StructTypeMode.Packed;
            else
                CurrentStructTypeMode = StructTypeMode.Unpacked;
            ;
        }

        public void EndVisit(StructType factor)
            => CurrentStructTypeMode = StructTypeMode.Undefined;

        #endregion
        #region ArrayType

        public void StartVisit(ArrayType array) {
            var target = LastTypeDeclaration;
            var value = new ArrayTypeDeclaration();
            InitNode(value, array);
            value.PackedType = CurrentStructTypeMode == StructTypeMode.Packed;
            target.TypeValue = value;

            if (array.ArrayOfConst) {
                var metaType = new MetaType();
                InitNode(metaType, array, value);
                metaType.Kind = MetaTypeKind.Const;
                value.TypeValue = metaType;
                visitor.WorkingStack.Pop();
            }
        }

        #endregion
        #region SetDefinition

        public void StartVisit(SetDefinition set) {
            var typeTarget = LastTypeDeclaration;
            var value = new SetTypeDeclaration();
            InitNode(value, set);
            value.PackedType = CurrentStructTypeMode == StructTypeMode.Packed;
            typeTarget.TypeValue = value;
        }

        #endregion
        #region FileTypeDefinition

        public void StartVisit(FileType fileType) {
            var typeTarget = LastTypeDeclaration;
            var value = new FileTypeDeclaration();
            InitNode(value, fileType);
            value.PackedType = CurrentStructTypeMode == StructTypeMode.Packed;
            typeTarget.TypeValue = value;
        }

        #endregion
        #region ClassOf

        public void StartVisit(ClassOfDeclaration classOf) {
            var typeTarget = LastTypeDeclaration;
            var value = new ClassOfTypeDeclaration();
            InitNode(value, classOf);
            value.PackedType = CurrentStructTypeMode == StructTypeMode.Packed;
            typeTarget.TypeValue = value;
        }

        #endregion
        #region TypeName                                       

        public void StartVisit(TypeName typeName) {
            var typeTarget = LastTypeDeclaration;
            var value = new MetaType();
            InitNode(value, typeName);
            value.Kind = typeName.MapTypeKind();
            typeTarget.TypeValue = value;
        }

        public void StartVisitChild(TypeName typeName, ISyntaxPart part) {
            var name = part as GenericNamespaceName;
            var value = LastValue as MetaType;

            if (name == null || value == null)
                return;

            var fragment = new GenericNameFragment();
            InitNode(fragment, name);
            fragment.Name = ExtractSymbolName(name.Name);
            value.AddFragment(fragment);
        }


        #endregion
        #region SimpleType

        public void StartVisit(SimpleType simpleType) {
            var typeTarget = LastTypeDeclaration;

            if (simpleType.SubrangeStart != null) {
                var subrange = new SubrangeType();
                InitNode(subrange, simpleType);
                typeTarget.TypeValue = subrange;
                return;
            }

            if (simpleType.EnumType != null)
                return;

            var value = new TypeAlias();
            InitNode(value, simpleType);
            value.IsNewType = simpleType.NewType;

            if (simpleType.TypeOf)
                LogSource.Warning(StructuralErrors.UnsupportedTypeOfConstruct, simpleType);

            typeTarget.TypeValue = value;
        }

        public void StartVisitChild(SimpleType simpleType, ISyntaxPart part) {
            var name = part as GenericNamespaceName;
            var value = LastValue as TypeAlias;

            if (name == null || value == null)
                return;

            var fragment = new GenericNameFragment();
            InitNode(fragment, part);
            fragment.Name = ExtractSymbolName(name.Name);
            value.AddFragment(fragment);
        }

        #endregion
        #region EnumTypeDefinition

        public void StartVisit(EnumTypeDefinition type) {
            var typeTarget = LastTypeDeclaration;
            var value = new EnumType();
            InitNode(value, type);
            typeTarget.TypeValue = value;
        }

        #endregion
        #region EnumValue

        public void StartVisit(EnumValue enumValue) {
            if (LastValue is EnumType enumDeclaration) {
                var value = new EnumTypeValue();
                InitNode(value, enumValue);
                value.Name = ExtractSymbolName(enumValue.EnumName);
                enumDeclaration.Add(value, LogSource);
            }
        }

        #endregion
        #region ArrayIndex

        public void StartVisit(ArrayIndex arrayIndex) {
            if (arrayIndex.EndIndex != null) {
                var lastExpression = LastExpression;
                var binOp = new BinaryOperator();
                InitNode(binOp, arrayIndex);
                lastExpression.Value = binOp;
                binOp.Kind = ExpressionKind.RangeOperator;
            }
        }

        #endregion
        #region PointerType

        public void StartVisit(PointerType pointer) {
            var typeTarget = LastTypeDeclaration;

            if (pointer.GenericPointer) {
                var result = new MetaType();
                InitNode(result, pointer);
                result.Kind = MetaTypeKind.Pointer;
                typeTarget.TypeValue = result;
            }
            else {
                var result = new PointerToType();
                InitNode(result, pointer);
                typeTarget.TypeValue = result;
            }
        }

        #endregion
        #region StringType

        public void StartVisit(StringType stringType) {
            var typeTarget = LastTypeDeclaration;
            var result = new MetaType();
            InitNode(result, stringType);
            result.Kind = TokenKindMapper.ForMetaType(stringType.Kind);
            typeTarget.TypeValue = result;
        }

        #endregion
        #region ProcedureTypeDefinition

        public void StartVisit(ProcedureTypeDefinition proceduralType) {
            var typeTarget = LastTypeDeclaration;
            var result = new ProceduralType();
            InitNode(result, proceduralType);
            typeTarget.TypeValue = result;
            result.Kind = TokenKindMapper.MapMethodKind(proceduralType.Kind);
            result.MethodDeclaration = proceduralType.MethodDeclaration;
            result.AllowAnonymousMethods = proceduralType.AllowAnonymousMethods;

            if (proceduralType.ReturnTypeAttributes != null)
                result.ReturnAttributes = ExtractAttributes(proceduralType.ReturnTypeAttributes, CurrentUnit);
        }

        #endregion
        #region FormalParameterDefinition

        public void StartVisit(FormalParameterDefinition formalParameter) {
            var paramterTarget = LastValue as IParameterTarget;
            var result = new ParameterTypeDefinition();
            InitNode(result, formalParameter, paramterTarget.Parameters);
            paramterTarget.Parameters.Items.Add(result);
        }

        #endregion
        #region FormalParameter

        public void StartVisit(FormalParameter formalParameter) {
            var typeDefinition = LastValue as ParameterTypeDefinition;
            var result = new ParameterDefinition();
            InitNode(result, formalParameter);
            var allParams = typeDefinition.ParentItem as ParameterDefinitions;
            result.Name = ExtractSymbolName(formalParameter.ParameterName);
            result.Attributes = ExtractAttributes(formalParameter.Attributes, CurrentUnit);
            result.ParameterKind = TokenKindMapper.ForParameterReferenceKind(formalParameter.ParameterType);
            typeDefinition.Parameters.Add(result);
            allParams.Add(result, LogSource);
        }

        #endregion   
        #region UnitInitialization

        public void StartVisit(UnitInitialization unitBlock) {
            var result = new BlockOfStatements();
            InitNode(result, unitBlock, CurrentUnit);
            CurrentUnit.InitializationBlock = result;
        }

        #endregion
        #region UnitFinalization

        public void StartVisit(UnitFinalization unitBlock) {
            var result = new BlockOfStatements();
            InitNode(result, unitBlock, CurrentUnit);
            CurrentUnit.FinalizationBlock = result;
        }


        #endregion
        #region CompoundStatement

        public void StartVisit(CompoundStatement block) {

            if (block.AssemblerBlock != null) {
                var statementTarget = LastValue as IStatementTarget;
                var blockTarget = LastValue as IBlockTarget;
                var result = new BlockOfAssemblerStatements();
                InitNode(result, block, LastValue);
                if (statementTarget != null)
                    statementTarget.Statements.Add(result);
                else if (blockTarget != null)
                    blockTarget.Block = result;

            }

            else {
                var statementTarget = LastValue as IStatementTarget;
                var blockTarget = LastValue as IBlockTarget;
                var result = new BlockOfStatements();
                InitNode(result, block, LastValue);
                if (statementTarget != null)
                    statementTarget.Statements.Add(result);
                else if (blockTarget != null)
                    blockTarget.Block = result;
            }

        }

        public void EndVisit(CompoundStatement block) {
        }


        #endregion
        #region Label

        public void StartVisit(Label label) {
            SymbolName name = null;

            if (label.LabelName is Identifier standardLabel) {
                name = ExtractSymbolName(standardLabel);
            }

            var intLabel = (label.LabelName as StandardInteger)?.LastTerminalToken;
            if (intLabel != null) {
                name = new SimpleSymbolName(intLabel.Value.ParsedValue.ToString());
            }

            var hexLabel = (label.LabelName as HexNumber)?.LastTerminalToken;
            if (hexLabel != null) {
                name = new SimpleSymbolName(hexLabel.Value.ParsedValue.ToString());
            }

            if (name == null)
                return;

            if (CurrentDeclarationMode == DeclarationMode.Label) {
                var symbols = LastValue as IDeclaredSymbolTarget;
                var declaration = new ConstantDeclaration();
                InitNode(declaration, label);
                declaration.Name = name;
                declaration.Mode = CurrentDeclarationMode;
                symbols.Symbols.AddDirect(declaration, LogSource);
            }

            var parent = LastValue as ILabelTarget;
            if (parent == null)
                return;

            parent.LabelName = name;
        }

        public void EndVisit(Label label) {
        }


        #endregion
        #region ClassDeclaration

        public void StartVisit(ClassDeclaration classDeclaration) {
            var typeTarget = LastTypeDeclaration;
            var result = new StructuredType();
            InitNode(result, classDeclaration);
            result.Kind = StructuredTypeKind.Class;
            result.SealedClass = classDeclaration.Sealed;
            result.AbstractClass = classDeclaration.Abstract;
            result.ForwardDeclaration = classDeclaration.ForwardDeclaration;
            typeTarget.TypeValue = result;
            CurrentMemberVisibility[result] = MemberVisibility.Public;

        }

        public void EndVisit(ClassDeclaration classDeclaration) {
            var parentType = LastValue as StructuredType;
            CurrentMemberVisibility.Reset(parentType);
        }

        #endregion
        #region ClassDeclarationItem

        public void StartVisit(ClassDeclarationItem classDeclarationItem) {
            var parentType = LastValue as StructuredType;

            if (parentType == null)
                return;

            if (classDeclarationItem.Visibility != TokenKind.Undefined) {
                CurrentMemberVisibility[parentType] = TokenKindMapper.ForVisibility(classDeclarationItem.Visibility, classDeclarationItem.Strict);
            }
        }

        #endregion
        #region ClassField

        public void StartVisit(ClassField field) {
            var structType = LastValue as StructuredType;
            var declItem = field.ParentItem as IStructuredTypeMember;
            var result = new StructureFields();
            InitNode(result, field);
            result.Visibility = CurrentMemberVisibility[structType];
            structType.Fields.Items.Add(result);
            var extractedAttributes = ExtractAttributes(declItem.Attributes, CurrentUnit);
            result.ClassItem = declItem.ClassItem;

            foreach (var part in field.Names.Parts) {

                if (part is UserAttributes attrs) {
                    extractedAttributes = ExtractAttributes(attrs, CurrentUnit, extractedAttributes);
                    continue;
                }

                var partName = part as Identifier;
                if (partName == null)
                    continue;

                var fieldName = new StructureField();
                InitNode(fieldName, part, result);
                fieldName.Name = ExtractSymbolName(partName);
                fieldName.Attributes = extractedAttributes;
                structType.Fields.Add(fieldName, LogSource);
                result.Fields.Add(fieldName);
                extractedAttributes = null;
                visitor.WorkingStack.Pop();
            }

            result.Hints = ExtractHints(field.Hint);
        }

        #endregion
        #region ClassProperty

        public void StartVisit(ClassProperty property) {
            var parent = LastValue as StructuredType;
            var result = new StructureProperty();
            InitNode(result, property);
            var declItem = property.ParentItem as IStructuredTypeMember;
            result.Name = ExtractSymbolName(property.PropertyName);
            parent.Properties.Add(result, LogSource);
            result.Visibility = CurrentMemberVisibility[parent];

            if (declItem != null) {
                result.Attributes = ExtractAttributes(declItem.Attributes, CurrentUnit);
            }


        }

        #endregion    
        #region ClassPropertyReadWrite

        public void StartVisit(ClassPropertyReadWrite property) {
            var parent = LastValue as StructureProperty;
            var result = new StructurePropertyAccessor();
            InitNode(result, property);
            result.Kind = TokenKindMapper.ForPropertyAccessor(property.Kind);
            result.Name = ExtractSymbolName(property.Member);
            parent.Accessors.Add(result);

        }

        #endregion
        #region  ClassPropertyDispInterface

        public void StartVisit(ClassPropertyDispInterface property) {
            var parent = LastValue as StructureProperty;
            var result = new StructurePropertyAccessor();
            InitNode(result, property);

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

        }

        #endregion
        #region ParseClassPropertyAccessSpecifier

        public void StartVisit(ClassPropertySpecifier property) {
            if (property.PropertyReadWrite != null)
                return;

            if (property.PropertyDispInterface != null)
                return;

            var parent = LastValue as StructureProperty;
            var result = new StructurePropertyAccessor();
            InitNode(result, property);

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

        }

        #endregion
        #region ClassMethod

        public void StartVisit(ClassMethod method) {
            var parent = LastValue as StructuredType;
            var result = new StructureMethod();
            InitNode(result, method);
            result.Visibility = CurrentMemberVisibility[parent];

            if (method.ParentItem is IStructuredTypeMember declItem) {
                result.ClassItem = declItem.ClassItem;
                result.Attributes = ExtractAttributes(declItem.Attributes, CurrentUnit);
            }

            result.Name = ExtractSymbolName(method.Identifier);
            result.Kind = TokenKindMapper.MapMethodKind(method.MethodKind);
            result.Generics = ExtractGenericDefinition(result, method, method.GenericDefinition);
            parent.Methods.Add(result, LogSource);

        }

        #endregion
        #region MethodResolution

        public void StartVisit(MethodResolution methodResolution) {
            var parent = LastValue as StructuredType;
            var result = new StructureMethodResolution();
            InitNode(result, methodResolution);
            result.Attributes = ExtractAttributes(((ClassDeclarationItem)methodResolution.ParentItem).Attributes, CurrentUnit);
            result.Kind = TokenKindMapper.ForMethodResolutionKind(methodResolution.Kind);
            result.Target = ExtractSymbolName(methodResolution.ResolveIdentifier);
            parent.MethodResolutions.Add(result);

        }

        #endregion
        #region ReintroduceDirective

        public void StartVisit(ReintroduceDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            var result = new MethodDirective();
            InitNode(result, directive);
            result.Kind = MethodDirectiveKind.Reintroduce;
            parent.Directives.Add(result);

        }

        #endregion
        #region OverloadDirective

        public void StartVisit(OverloadDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            var result = new MethodDirective();
            InitNode(result, directive);
            result.Kind = MethodDirectiveKind.Overload;
            parent.Directives.Add(result);

        }

        #endregion
        #region DispIdDirective

        public void StartVisit(DispIdDirective directive) {
            var parent = LastValue as IDirectiveTarget;

            if (parent == null)
                return;

            var result = new MethodDirective();
            InitNode(result, directive);
            result.Kind = MethodDirectiveKind.DispId;
            parent.Directives.Add(result);

        }

        #endregion
        #region InlineDirective

        public void StartVisit(InlineDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            var result = new MethodDirective();
            InitNode(result, directive);

            if (directive.Kind == TokenKind.Inline) {
                result.Kind = MethodDirectiveKind.Inline;
            }
            else if (directive.Kind == TokenKind.Assembler) {
                result.Kind = MethodDirectiveKind.Assembler;
            }

            parent.Directives.Add(result);

        }

        #endregion
        #region AbstractDirective

        public void StartVisit(AbstractDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            var result = new MethodDirective();
            InitNode(result, directive);

            if (directive.Kind == TokenKind.Abstract) {
                result.Kind = MethodDirectiveKind.Abstract;
            }
            else if (directive.Kind == TokenKind.Final) {
                result.Kind = MethodDirectiveKind.Final;
            }

            parent.Directives.Add(result);

        }

        #endregion
        #region OldCallConvention

        public void StartVisit(OldCallConvention directive) {
            var parent = LastValue as IDirectiveTarget;
            var result = new MethodDirective();
            InitNode(result, directive);

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

        }

        #endregion
        #region ExternalDirective

        public void StartVisit(ExternalDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            var result = new MethodDirective();
            InitNode(result, directive);

            if (directive.Kind == TokenKind.VarArgs) {
                result.Kind = MethodDirectiveKind.VarArgs;
            }

            else if (directive.Kind == TokenKind.External) {
                result.Kind = MethodDirectiveKind.External;
            }

            parent.Directives.Add(result);

        }

        #endregion
        #region ExternalSpecifier

        public void StartVisit(ExternalSpecifier directive) {
            var parent = LastValue as MethodDirective;
            var result = new MethodDirectiveSpecifier();
            InitNode(result, directive);
            result.Kind = TokenKindMapper.ForMethodDirective(directive.Kind);
            parent.Specifiers.Add(result);

        }

        #endregion
        #region CallConvention

        public void StartVisit(CallConvention directive) {
            var parent = LastValue as IDirectiveTarget;
            var result = new MethodDirective();
            InitNode(result, directive);

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

        }

        #endregion
        #region BindingDirective

        public void StartVisit(BindingDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            var result = new MethodDirective();
            InitNode(result, directive);

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

        }

        #endregion
        #region MethodDirectives

        public void StartVisitChild(MethodDirectives parent, ISyntaxPart child) {
            var hints = child as HintingInformation;
            var lastValue = LastValue as IDirectiveTarget;

            if (hints != null && lastValue != null) {
                lastValue.Hints = ExtractHints(hints, lastValue.Hints);
            }
        }

        #endregion
        #region FunctionDirectives

        public void StartVisitChild(FunctionDirectives parent, ISyntaxPart child) {
            var hints = child as HintingInformation;
            var lastValue = LastValue as IDirectiveTarget;

            if (hints != null && lastValue != null) {
                lastValue.Hints = ExtractHints(hints, lastValue.Hints);
            }
        }

        #endregion
        #region ExportedProcedureHeading

        public void StartVisit(ExportedProcedureHeading procHeading) {
            var symbols = LastValue as IDeclaredSymbolTarget;
            var result = new GlobalMethod();
            InitNode(result, procHeading);
            result.Name = ExtractSymbolName(procHeading.Name);
            result.Kind = TokenKindMapper.MapMethodKind(procHeading.Kind);
            result.Attributes = ExtractAttributes(procHeading.Attributes, CurrentUnit);
            result.ReturnAttributes = ExtractAttributes(procHeading.ResultAttributes, CurrentUnit);
            symbols.Symbols.AddDirect(result, LogSource);

        }

        #endregion
        #region UnsafeDirective

        public void StartVisit(UnsafeDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            var result = new MethodDirective();
            InitNode(result, directive);
            result.Kind = MethodDirectiveKind.Unsafe;
            parent.Directives.Add(result);

        }

        #endregion
        #region ForwardDirective

        public void StartVisit(ForwardDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            var result = new MethodDirective();
            InitNode(result, directive);
            result.Kind = MethodDirectiveKind.Forward;
            parent.Directives.Add(result);

        }

        #endregion
        #region ExportsSection

        public void StartVisit(ExportsSection exportsSection) {
            CurrentDeclarationMode = DeclarationMode.Exports;
            ;
        }

        private AbstractSyntaxPartBase EndVisitItem(ExportsSection exportsSection) {
            CurrentDeclarationMode = DeclarationMode.Unknown;
            return null;
            ;
        }


        #endregion
        #region ExportItem

        public void StartVisit(ExportItem exportsSection) {
            var declarations = LastValue as IDeclaredSymbolTarget;
            var result = new ExportedMethodDeclaration();
            InitNode(result, exportsSection);
            result.Name = ExtractSymbolName(exportsSection.ExportName);
            result.IsResident = exportsSection.Resident;
            result.HasIndex = exportsSection.IndexParameter != null;
            result.HasName = exportsSection.NameParameter != null;
            declarations.Symbols.AddDirect(result, LogSource);

        }

        #endregion
        #region RecordItem

        public void StartVisit(RecordItem recordDeclarationItem) {
            var parentType = LastValue as StructuredType;

            if (parentType == null)
                return;

            if (recordDeclarationItem.Visibility != TokenKind.Undefined) {
                CurrentMemberVisibility[parentType] = TokenKindMapper.ForVisibility(recordDeclarationItem.Visibility, recordDeclarationItem.Strict);
            };
        }

        #endregion
        #region RecordDeclaration

        public void StartVisit(RecordDeclaration recordDeclaration) {
            var typeTarget = LastTypeDeclaration;
            var result = new StructuredType();
            InitNode(result, recordDeclaration);
            result.Kind = StructuredTypeKind.Record;
            typeTarget.TypeValue = result;
            CurrentMemberVisibility[result] = MemberVisibility.Public;

        }

        public void EndVisit(RecordDeclaration recordDeclaration) {
            var parentType = LastValue as StructuredType;
            CurrentMemberVisibility.Reset(parentType);
        }

        #endregion
        #region RecordField

        public void StartVisit(RecordField fieldDeclaration) {
            StructuredType structType = null;
            StructureVariant varFields = null;
            IList<StructureFields> fields = null;

            if (LastValue is StructureVariantFields) {
                structType = LastValue.ParentItem?.ParentItem?.ParentItem as StructuredType;
                varFields = structType.Variants;
                fields = (LastValue as StructureVariantFields)?.Fields;
            }
            else {
                structType = LastValue as StructuredType;
                fields = structType.Fields.Items;
            }

            var declItem = fieldDeclaration.ParentItem as RecordItem;
            var result = new StructureFields();
            InitNode(result, fieldDeclaration);
            result.Visibility = CurrentMemberVisibility[structType];

            if (fields != null)
                fields.Add(result);

            IList<SymbolAttribute> extractedAttributes = null;
            if (declItem != null)
                extractedAttributes = ExtractAttributes(declItem.Attributes, CurrentUnit);

            foreach (var part in fieldDeclaration.Names.Parts) {

                if (part is UserAttributes attrs) {
                    extractedAttributes = ExtractAttributes(attrs, CurrentUnit, extractedAttributes);
                    continue;
                }

                var partName = part as Identifier;
                if (partName == null)
                    continue;

                var fieldName = new StructureField();
                InitNode(fieldName, partName, result);
                fieldName.Name = ExtractSymbolName(partName);
                fieldName.Attributes = extractedAttributes;

                if (varFields == null)
                    structType.Fields.Add(fieldName, LogSource);
                else
                    varFields.Add(fieldName, LogSource);

                result.Fields.Add(fieldName);
                extractedAttributes = null;
                visitor.WorkingStack.Pop();

            }

            result.Hints = ExtractHints(fieldDeclaration.Hint);

        }


        #endregion
        #region ParseRecordVariantSection

        public void StartVisit(RecordVariantSection variantSection) {
            var structType = LastValue as StructuredType;
            var result = new StructureVariantItem();
            InitNode(result, variantSection);
            result.Name = ExtractSymbolName(variantSection.Name);
            structType.Variants.Items.Add(result);

        }

        #endregion
        #region RecordVariant

        public void StartVisit(RecordVariant variantItem) {
            var structType = LastValue as StructureVariantItem;
            var result = new StructureVariantFields();
            InitNode(result, variantItem);
            structType.Items.Add(result);

        }

        #endregion
        #region RecordHelperDefinition       

        public void StartVisit(RecordHelperDefinition recordHelper) {
            var typeTarget = LastTypeDeclaration;
            var result = new StructuredType();
            InitNode(result, recordHelper);
            result.Kind = StructuredTypeKind.RecordHelper;
            typeTarget.TypeValue = result;
            CurrentMemberVisibility[result] = MemberVisibility.Public;

        }

        public void EndVisit(RecordHelperDefinition classDeclaration) {
            var parentType = LastValue as StructuredType;
            CurrentMemberVisibility.Reset(parentType);
        }

        #endregion
        #region RecordHelperItem

        public void StartVisit(RecordHelperItem recordDeclarationItem) {
            var parentType = LastValue as StructuredType;

            if (parentType == null)
                return;

            if (recordDeclarationItem.Visibility != TokenKind.Undefined) {
                CurrentMemberVisibility[parentType] = TokenKindMapper.ForVisibility(recordDeclarationItem.Visibility, recordDeclarationItem.Strict);
            };
        }

        #endregion
        #region ObjectDeclaration       

        public void StartVisit(ObjectDeclaration objectDeclaration) {
            var typeTarget = LastTypeDeclaration;
            var result = new StructuredType();
            InitNode(result, objectDeclaration);
            result.Kind = StructuredTypeKind.Object;
            typeTarget.TypeValue = result;
            CurrentMemberVisibility[result] = MemberVisibility.Public;

        }

        public void EndVisit(ObjectDeclaration objectDeclaration) {
            var parentType = LastValue as StructuredType;
            CurrentMemberVisibility.Reset(parentType);
        }


        #endregion
        #region ObjectItem

        public void StartVisit(ObjectItem objectItem) {
            var parentType = LastValue as StructuredType;

            if (parentType == null)
                return;

            if (objectItem.Visibility != TokenKind.Undefined) {
                CurrentMemberVisibility[parentType] = TokenKindMapper.ForVisibility(objectItem.Visibility, objectItem.Strict);
            };
        }

        #endregion
        #region InterfaceDefinition

        public void StartVisit(InterfaceDefinition interfaceDeclaration) {
            var typeTarget = LastTypeDeclaration;
            var result = new StructuredType();
            InitNode(result, interfaceDeclaration);
            if (interfaceDeclaration.DisplayInterface)
                result.Kind = StructuredTypeKind.DispInterface;
            else
                result.Kind = StructuredTypeKind.Interface;

            result.ForwardDeclaration = interfaceDeclaration.ForwardDeclaration;
            typeTarget.TypeValue = result;
            CurrentMemberVisibility[result] = MemberVisibility.Public;

        }

        public void EndVisit(InterfaceDefinition interfaceDeclaration) {
            var parentType = LastValue as StructuredType;
            CurrentMemberVisibility.Reset(parentType);
        }

        #endregion
        #region InterfaceGuid

        public void StartVisit(InterfaceGuid interfaceGuid) {
            var structType = LastValue as StructuredType;

            if (interfaceGuid.IdIdentifier != null) {
                structType.GuidName = ExtractSymbolName(interfaceGuid.IdIdentifier);
            }
            else if (interfaceGuid.Id != null) {
                structType.GuidId = interfaceGuid.Id.FirstTerminalToken.ParsedValue;
            }


            ;
        }

        #endregion
        #region ClassHelper

        public void StartVisit(ClassHelperDef classHelper) {
            var typeTarget = LastTypeDeclaration;
            var result = new StructuredType();
            InitNode(result, classHelper);
            result.Kind = StructuredTypeKind.ClassHelper;
            typeTarget.TypeValue = result;
            CurrentMemberVisibility[result] = MemberVisibility.Public;

        }

        public void EndVisit(ClassHelperDef classHelper) {
            var parentType = LastValue as StructuredType;
            CurrentMemberVisibility.Reset(parentType);
        }


        #endregion
        #region ClassHelperItem

        public void StartVisit(ClassHelperItem classHelperItem) {
            var parentType = LastValue as StructuredType;

            if (parentType == null)
                return;

            if (classHelperItem.Visibility != TokenKind.Undefined) {
                CurrentMemberVisibility[parentType] = TokenKindMapper.ForVisibility(classHelperItem.Visibility, classHelperItem.Strict);
            };
        }

        #endregion
        #region ProcedureDeclaration

        public void StartVisit(ProcedureDeclaration procedure) {
            var symbolTarget = LastValue as IDeclaredSymbolTarget;
            var result = new MethodImplementation();
            InitNode(result, procedure);
            result.Name = ExtractSymbolName(procedure.Heading.Name);
            result.Kind = TokenKindMapper.MapMethodKind(procedure.Heading.Kind);
            symbolTarget.Symbols.AddDirect(result, LogSource);

        }

        #endregion
        #region MethodDeclaration

        public void StartVisit(Standard.MethodDeclaration method) {
            var unit = CurrentUnit;
            var name = ExtractSymbolName(method.Heading.Qualifiers);
            var result = new MethodImplementation();
            InitNode(result, method);
            result.Kind = TokenKindMapper.MapMethodKind(method.Heading.Kind);
            result.Name = name;

            var type = unit.InterfaceSymbols.Find(name.NamespaceParts);

            if (type == null)
                type = unit.ImplementationSymbols.Find(name.NamespaceParts);

            if (type != null) {
                var typeDecl = type as Abstract.TypeDeclaration;
                var typeStruct = typeDecl.TypeValue as StructuredType;
                if (typeStruct.Methods.Contains(name.Name)) {
                    var declaration = typeStruct.Methods[name.Name];
                    declaration.Implementation = result;
                }
            }
        }

        #endregion
        #region StatementPart

        public void StartVisit(StatementPart part) {

            if (part.DesignatorPart == null && part.Assignment == null)
                return;

            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, part);

            if (part.Assignment != null) {
                result.Kind = StructuredStatementKind.Assignment;
            }
            else {
                result.Kind = StructuredStatementKind.ExpressionStatement;
            }

            target.Statements.Add(result);

        }

        #endregion
        #region ClosureExpression

        public void StartVisit(ClosureExpression closure) {
            var expression = LastExpression;
            var result = new MethodImplementation();
            InitNode(result, closure);
            result.Name = new SimpleSymbolName(CurrentUnit.GenerateSymbolName());
            expression.Value = result;
        }

        #endregion
        #region RaiseStatement

        public void StartVisit(RaiseStatement raise) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, raise);
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

        }

        #endregion
        #region TryStatement

        public void StartVisit(TryStatement tryStatement) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, tryStatement);
            target.Statements.Add(result);

            if (tryStatement.Finally != null) {
                result.Kind = StructuredStatementKind.TryFinally;
            }
            else if (tryStatement.Handlers != null) {
                result.Kind = StructuredStatementKind.TryExcept;
            }


        }

        public void StartVisitChild(TryStatement tryStatement, ISyntaxPart child) {
            var statements = child as StatementList;

            if (statements == null)
                return;

            var target = LastValue as IStatementTarget;
            var result = new BlockOfStatements();
            InitNode(result, child);
            target.Statements.Add(result);
        }

        #endregion
        #region ExceptHandlers

        public void StartVisit(ExceptHandlers exceptHandlers) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, exceptHandlers);
            result.Kind = StructuredStatementKind.ExceptElse;
            target.Statements.Add(result);

        }

        #endregion
        #region ExceptHandler

        public void StartVisit(ExceptHandler exceptHandler) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, exceptHandler);
            result.Kind = StructuredStatementKind.ExceptOn;
            result.Name = ExtractSymbolName(exceptHandler.Name);
            target.Statements.Add(result);

        }

        #endregion
        #region WithStatement

        public void StartVisit(WithStatement withStatement) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, withStatement);
            result.Kind = StructuredStatementKind.With;
            target.Statements.Add(result);

        }

        #endregion
        #region ForStatement

        public void StartVisit(ForStatement forStatement) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, forStatement);

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

        }

        #endregion
        #region WhileStatement

        public void StartVisit(WhileStatement withStatement) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, withStatement);
            result.Kind = StructuredStatementKind.While;
            target.Statements.Add(result);

        }

        #endregion
        #region RepeatStatement

        public void StartVisit(RepeatStatement repeateStatement) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, repeateStatement);
            result.Kind = StructuredStatementKind.Repeat;
            target.Statements.Add(result);

        }

        #endregion
        #region CaseStatement

        public void StartVisit(CaseStatement caseStatement) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, caseStatement);
            result.Kind = StructuredStatementKind.Case;
            target.Statements.Add(result);

        }

        public void StartVisitChild(CaseStatement caseStatement, ISyntaxPart child) {

            if (caseStatement.Else != child)
                return;

            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, child);
            result.Kind = StructuredStatementKind.CaseElse;
            target.Statements.Add(result);
        }

        #endregion
        #region CaseItem

        public void StartVisit(CaseItem caseItem) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, caseItem);
            result.Kind = StructuredStatementKind.CaseItem;
            target.Statements.Add(result);

        }

        #endregion
        #region CaseLabel

        public void StartVisit(CaseLabel caseLabel) {
            if (caseLabel.EndExpression != null) {
                var lastExpression = LastExpression;
                var binOp = new BinaryOperator();
                InitNode(binOp, caseLabel);
                lastExpression.Value = binOp;
                binOp.Kind = ExpressionKind.RangeOperator;
            }
        }

        #endregion
        #region IfStatement

        public void StartVisit(IfStatement ifStatement) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, ifStatement);
            result.Kind = StructuredStatementKind.IfThen;
            target.Statements.Add(result);

        }

        public void StartVisitChild(IfStatement ifStatement, ISyntaxPart child) {

            if (ifStatement.ElsePart != child)
                return;

            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, child);
            result.Kind = StructuredStatementKind.IfElse;
            target.Statements.Add(result);
        }

        #endregion
        #region GoToStatement

        public void StartVisit(GoToStatement gotoStatement) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, gotoStatement);

            if (gotoStatement.Break)
                result.Kind = StructuredStatementKind.Break;
            else if (gotoStatement.Continue)
                result.Kind = StructuredStatementKind.Continue;
            else if (gotoStatement.GoToLabel != null)
                result.Kind = StructuredStatementKind.GoToLabel;
            else if (gotoStatement.Exit)
                result.Kind = StructuredStatementKind.Exit;

            target.Statements.Add(result);

        }

        #endregion
        #region AsmBlock

        public void StartVisit(AsmBlock block) {
            var statementTarget = LastValue as IStatementTarget;
            var blockTarget = LastValue as IBlockTarget;
            var result = new BlockOfAssemblerStatements();
            InitNode(result, block);
            if (statementTarget != null)
                statementTarget.Statements.Add(result);
            else if (blockTarget != null)
                blockTarget.Block = result;

        }

        #endregion
        #region AsmPseudoOp

        public void StartVisit(AsmPseudoOp op) {
            var statementTarget = LastValue as BlockOfAssemblerStatements;
            var result = new AssemblerStatement();
            InitNode(result, op);

            if (op.ParamsOperation) {
                result.Kind = AssemblerStatementKind.ParamsOperation;
                var operand = new ConstantValue();
                InitNode(operand, op.NumberOfParams);
                operand.Kind = ConstantValueKind.Integer;
                operand.LiteralValue = op.NumberOfParams.FirstTerminalToken.ParsedValue;
                result.Operands.Add(operand);
            }
            else if (op.PushEnvOperation) {
                result.Kind = AssemblerStatementKind.PushEnvOperation;
                var operand = new SymbolReference();
                InitNode(operand, op.Register);
                operand.Name = ExtractSymbolName(op.Register);
                result.Operands.Add(operand);
            }
            else if (op.SaveEnvOperation) {
                result.Kind = AssemblerStatementKind.SaveEnvOperation;
                var operand = new SymbolReference();
                InitNode(operand, op.Register);
                operand.Name = ExtractSymbolName(op.Register);
                result.Operands.Add(operand);
            }
            else if (op.NoFrame) {
                result.Kind = AssemblerStatementKind.NoFrameOperation;
            }

            statementTarget.Statements.Add(result);

        }

        #endregion
        #region LocalAsmLabel

        public void StartVisit(LocalAsmLabel label) {
            var value = string.Empty;
            foreach (var token in label.Parts) {
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
            ;
        }

        #endregion
        #region AsmStatement

        public void StartVisit(AsmStatement statement) {
            var parent = LastValue as BlockOfAssemblerStatements;
            var result = new AssemblerStatement();
            InitNode(result, statement);
            parent.Statements.Add(result);
            result.OpCode = ExtractSymbolName(statement.OpCode?.OpCode);
            result.SegmentPrefix = ExtractSymbolName(statement.Prefix?.SegmentPrefix);
            result.LockPrefix = ExtractSymbolName(statement.Prefix?.LockPrefix);


        }

        #endregion
        #region ParseAssemblyOperand

        public void StartVisit(AsmOperand statement) {

            if (statement.LeftTerm != null && statement.RightTerm != null) {
                var lastExpression = LastExpression;
                var currentExpression = new BinaryOperator();
                InitNode(currentExpression, statement);
                lastExpression.Value = currentExpression;
                currentExpression.Kind = TokenKindMapper.ForExpression(statement.Kind);
            }

            if (statement.NotExpression != null) {
                var lastExpression = LastExpression;
                var currentExpression = new UnaryOperator();
                InitNode(currentExpression, statement);
                lastExpression.Value = currentExpression;
                currentExpression.Kind = ExpressionKind.Not;
            }

            ;
        }

        #endregion
        #region AsmExpression

        public void StartVisit(AsmExpression statement) {

            if (statement.Offset != null) {
                var lastExpression = LastExpression;
                var currentExpression = new UnaryOperator();
                InitNode(currentExpression, statement);
                lastExpression.Value = currentExpression;
                currentExpression.Kind = ExpressionKind.AsmOffset;
            }

            if (statement.BytePtrKind != null) {
                var lastExpression = LastExpression;
                var currentExpression = new UnaryOperator();
                InitNode(currentExpression, statement);
                lastExpression.Value = currentExpression;
                currentExpression.Kind = TokenKindMapper.ForAsmBytePointerKind(ExtractSymbolName(statement.BytePtrKind)?.CompleteName);
            }

            if (statement.TypeExpression != null) {
                var lastExpression = LastExpression;
                var currentExpression = new UnaryOperator();
                InitNode(currentExpression, statement);
                lastExpression.Value = currentExpression;
                currentExpression.Kind = ExpressionKind.AsmType;
                return;
            }

            if (statement.RightOperand != null) {
                var lastExpression = LastExpression;
                var currentExpression = new BinaryOperator();
                InitNode(currentExpression, statement);
                lastExpression.Value = currentExpression;
                currentExpression.Kind = TokenKindMapper.ForExpression(statement.BinaryOperatorKind);
                return;
            }

            ;
        }

        #endregion
        #region AsmTerm

        public void StartVisit(AsmTerm statement) {

            if (statement.Kind != TokenKind.Undefined) {
                var lastExpression = LastExpression;
                var currentExpression = new BinaryOperator();
                InitNode(currentExpression, statement);
                lastExpression.Value = currentExpression;
                currentExpression.Kind = TokenKindMapper.ForExpression(statement.Kind);
            }

            if (statement.Subtype != null) {
                var lastExpression = LastExpression;
                var currentExpression = new BinaryOperator();
                InitNode(currentExpression, statement);
                lastExpression.Value = currentExpression;
                currentExpression.Kind = ExpressionKind.Dot;
            }

            ;
        }

        #endregion
        #region DesignatorStatement

        public void StartVisit(DesignatorStatement designator) {
            var lastExpression = LastExpression;
            var result = new SymbolReference();
            InitNode(result, designator);
            if (designator.Inherited)
                result.Inherited = true;

            lastExpression.Value = result;

        }

        #endregion
        #region DesignatorItem

        public void StartVisit(DesignatorItem designator) {
            var parent = LastValue as SymbolReference;

            if (designator.Dereference) {
                var part = new SymbolReferencePart();
                InitNode(part, designator);
                parent.AddPart(part);
                part.Kind = SymbolReferencePartKind.Dereference;
                return;
            }

            if (designator.Subitem != null) {
                var part = new SymbolReferencePart();
                InitNode(part, designator);
                parent.AddPart(part);
                part.Kind = SymbolReferencePartKind.SubItem;
                part.Name = ExtractSymbolName(designator.Subitem);
                part.GenericType = ExtractGenericDefinition(part, designator.SubitemGenericType);
                //return (AbstractSyntaxPart)part.GenericType ?? part;
                return;
            }

            if (designator.IndexExpression != null) {
                var part = new SymbolReferencePart();
                InitNode(part, designator);
                parent.AddPart(part);
                part.Kind = SymbolReferencePartKind.ArrayIndex;
                return;
            }

            if (designator.ParameterList) {
                var part = new SymbolReferencePart();
                InitNode(part, designator);
                parent.AddPart(part);
                part.Kind = SymbolReferencePartKind.CallParameters;
                return;
            }
        }

        #endregion
        #region Parameter

        public void StartVisit(Parameter param) {
            if (param.ParameterName == null)
                return;

            var lastExpression = LastExpression;
            var result = new SymbolReference();
            InitNode(result, param);
            result.NamedParameter = true;
            result.Name = ExtractSymbolName(param.ParameterName);
            lastExpression.Value = result;

        }

        #endregion
        #region FormattedExpression

        public void StartVisit(Standard.FormattedExpression expr) {
            if (expr.Width == null && expr.Decimals == null)
                return;

            var lastExpression = LastExpression;
            var result = new Abstract.FormattedExpression();
            InitNode(result, expr);
            lastExpression.Value = result;

        }

        #endregion
        #region SetSection

        public void StartVisit(SetSection expr) {
            var lastExpression = LastExpression;
            var result = new ArrayExpression();
            InitNode(result, expr);
            lastExpression.Value = result;
        }

        #endregion

        public void StartVisit(SetSectnPart part) {
            if (part.Continuation != TokenKind.DotDot) {
                var arrayExpression = LastExpression as ArrayExpression;

                if (arrayExpression == null)
                    return;

                var binOp = arrayExpression.Expressions.LastOrDefault() as BinaryOperator;

                // if (binOp != null && binOp.RightOperand == null)
                //     return binOp;

                return;
            }

            var lastExpression = LastExpression;
            var result = new BinaryOperator();
            InitNode(result, part);
            result.Kind = ExpressionKind.RangeOperator;
            lastExpression.Value = result;

        }

        #region AsmFactor

        public void StartVisit(AsmFactor factor) {
            var expression = LastExpression;

            if (factor.Number != null) {
                var value = new ConstantValue();
                InitNode(value, factor);
                value.Kind = ConstantValueKind.Integer;
                expression.Value = value;
                return;
            }

            if (factor.RealNumber != null) {
                var value = new ConstantValue();
                InitNode(value, factor);
                value.Kind = ConstantValueKind.RealNumber;
                expression.Value = value;
                return;
            }

            if (factor.HexNumber != null) {
                var value = new ConstantValue();
                InitNode(value, factor);
                value.Kind = ConstantValueKind.HexNumber;
                expression.Value = value;
                return;
            }

            if (factor.QuotedString != null) {
                var value = new ConstantValue();
                InitNode(value, factor);
                value.Kind = ConstantValueKind.QuotedString;
                expression.Value = value;
                return;
            }

            if (factor.Identifier != null) {
                var value = new SymbolReference();
                InitNode(value, factor);
                value.Name = ExtractSymbolName(factor.Identifier);
                expression.Value = value;
                return;
            }

            if (factor.SegmentPrefix != null) {
                var currentExpression = new BinaryOperator();
                InitNode(currentExpression, factor);
                expression.Value = currentExpression;
                currentExpression.Kind = ExpressionKind.AsmSegmentPrefix;
                var reference = new SymbolReference();
                InitNode(reference, factor.SegmentPrefix, currentExpression);
                reference.Name = ExtractSymbolName(factor.SegmentPrefix);
                currentExpression.LeftOperand = reference;
                return;
            }

            if (factor.MemorySubexpression != null) {
                var currentExpression = new UnaryOperator();
                InitNode(currentExpression, factor);
                expression.Value = currentExpression;
                currentExpression.Kind = ExpressionKind.AsmMemorySubexpression;
                return;
            }

            if (factor.Label != null) {
                var reference = new SymbolReference();
                InitNode(reference, factor);
                expression.Value = reference;
                return;
            }
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

            foreach (var name in qualifiers) {
                if (name.Name != null) {
                    foreach (var namePart in name.Name.Namespace)
                        if (!string.IsNullOrWhiteSpace(namePart))
                            result.AddName(namePart);
                    if (!string.IsNullOrWhiteSpace(name.Name.Name))
                        result.AddName(name.Name.Name);
                    if (name.GenericDefinition != null) {
                        foreach (var part in name.GenericDefinition.Parts) {

                            if (part is Identifier idPart) {
                                result.AddGenericPart(SyntaxPartBase.IdentifierValue(idPart));
                                continue;
                            }

                            if (part is GenericDefinitionPart genericPart) {
                                result.AddGenericPart(SyntaxPartBase.IdentifierValue(genericPart.Identifier));
                            }
                        }
                    }
                }
            }

            return result;
        }

        private static SymbolName ExtractSymbolName(Identifier name) {
            var result = new SimpleSymbolName(name?.FirstTerminalToken.Value);
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

        private GenericTypes ExtractGenericDefinition(AbstractSyntaxPartBase parent, GenericSuffix genericDefinition) {
            if (genericDefinition == null)
                return null;

            var result = new GenericTypes();
            InitNode(result, genericDefinition, parent);
            result.TypeReference = true;

            return result;
        }

        private GenericTypes ExtractGenericDefinition(AbstractSyntaxPartBase parent, ISyntaxPart node, GenericDefinition genericDefinition) {
            if (genericDefinition == null)
                return null;

            var result = new GenericTypes();
            InitNode(result, node, parent);

            foreach (var part in genericDefinition.Parts) {

                if (part is Identifier idPart) {
                    var generic = new GenericType();
                    InitNode(generic, node, parent);
                    generic.Name = ExtractSymbolName(idPart);
                    result.Add(generic, LogSource);
                    visitor.WorkingStack.Pop();
                    continue;
                }

                if (part is GenericDefinitionPart genericPart) {
                    var generic = new GenericType();
                    InitNode(generic, node, result);
                    generic.Name = ExtractSymbolName(genericPart.Identifier);
                    result.Add(generic, LogSource);

                    foreach (var constraintPart in genericPart.Parts) {
                        if (constraintPart is Standard.ConstrainedGeneric constraint) {
                            var cr = new GenericConstraint();
                            InitNode(cr, node, generic);
                            cr.Kind = TokenKindMapper.ForGenericConstraint(constraint);
                            cr.Name = ExtractSymbolName(constraint.ConstraintIdentifier);
                            generic.Add(cr, LogSource);
                            visitor.WorkingStack.Pop();
                        }
                    }

                    visitor.WorkingStack.Pop();
                }
            }

            visitor.WorkingStack.Pop();


            return result;
        }

        private static SymbolHints ExtractHints(HintingInformationList hints) {
            var result = new SymbolHints();

            if (hints == null || hints.PartList.Count < 1)
                return null;

            foreach (var part in hints.Parts) {
                var hint = part as HintingInformation;
                if (hint == null)
                    continue;
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
                return new EmptyList<SymbolAttribute>();

            if (result == null)
                result = new List<SymbolAttribute>();

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

        private void InitNode(AbstractSyntaxPartBase result, ISyntaxPart node)
            => InitNode(result, node, LastValue, null);

        private void InitNode(AbstractSyntaxPartBase result, ISyntaxPart node, AbstractSyntaxPartBase parent)
            => InitNode(result, node, parent, null);

        private void InitNode(AbstractSyntaxPartBase result, ISyntaxPart node, AbstractSyntaxPartBase parent, ISyntaxPart child) {

            if (node is AbstractSyntaxPartBase)
                throw new InvalidOperationException();

            result.ParentItem = parent;
            visitor.WorkingStack.Push(new WorkingStackEntry(node, result, child));
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
        private IDictionary<AbstractSyntaxPartBase, object> currentValues
            = new Dictionary<AbstractSyntaxPartBase, object>();

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
        public ILogManager LogManager { get; set; }

        /// <summary>
        ///     project root
        /// </summary>
        public ProjectRoot Project { get; }

        /// <summary>
        ///     current compilation unit
        /// </summary>
        public CompilationUnit CurrentUnit { get; set; }
            = null;

        /// <summary>
        ///     current unit mode
        /// </summary>
        public DictionaryIndexHelper<AbstractSyntaxPartBase, UnitMode> CurrentUnitMode { get; }

        /// <summary>
        ///     currennt member visibility
        /// </summary>
        public DictionaryIndexHelper<AbstractSyntaxPartBase, MemberVisibility> CurrentMemberVisibility { get; }

        /// <summary>
        ///     const declaration mode
        /// </summary>
        public DeclarationMode CurrentDeclarationMode { get; internal set; }

        /// <summary>
        ///     last expression
        /// </summary>
        public IExpressionTarget LastExpression {
            get {
                if (visitor.WorkingStack.Count > 0)
                    return visitor.WorkingStack.Peek().Data as IExpressionTarget;
                else
                    return null;
            }
        }

        /// <summary>
        ///     create a new options set
        /// </summary>
        public TreeTransformer(ProjectRoot projectRoot) {
            visitor = new ChildVisitor(this);
            Project = projectRoot;
            CurrentUnitMode = new DictionaryIndexHelper<AbstractSyntaxPartBase, UnitMode>(currentValues);
            CurrentMemberVisibility = new DictionaryIndexHelper<AbstractSyntaxPartBase, MemberVisibility>(currentValues);
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
                if (visitor.WorkingStack.Count > 0)
                    return visitor.WorkingStack.Peek().Data as ITypeTarget;
                else
                    return null;
            }
        }

        /// <summary>
        ///     last value from the working stack
        /// </summary>
        public AbstractSyntaxPartBase LastValue {
            get {
                if (visitor.WorkingStack.Count > 0)
                    return visitor.WorkingStack.Peek().Data as AbstractSyntaxPartBase;
                else
                    return null;
            }
        }

        private void AddToStack(object part, AbstractSyntaxPartBase result)
            => visitor.WorkingStack.Push(new WorkingStackEntry(part, result));

        public void EndVisitChild(CaseStatement element, ISyntaxPart child) {
        }

        public void EndVisitChild(TypeName element, ISyntaxPart child) {
        }

        public void EndVisitChild(SimpleType element, ISyntaxPart child) {
        }

        public void EndVisitChild(MethodDirectives element, ISyntaxPart child) {
        }

        public void EndVisitChild(FunctionDirectives element, ISyntaxPart child) {
        }

        public void EndVisitChild(TryStatement element, ISyntaxPart child) {
        }

        public void EndVisitChild(IfStatement element, ISyntaxPart child) {
        }
    }
}
