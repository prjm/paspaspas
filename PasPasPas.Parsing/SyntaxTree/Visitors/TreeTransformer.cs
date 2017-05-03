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
        IStartVisitor<ConstDeclaration>, IEndVisitor<ConstDeclaration>,
        IStartVisitor<LabelDeclarationSection>, IEndVisitor<LabelDeclarationSection>,
        IStartVisitor<VarSection>, IEndVisitor<VarSection>,
        IStartVisitor<VarDeclaration>, IEndVisitor<VarDeclaration>,
        IStartVisitor<VarValueSpecification>, IEndVisitor<VarValueSpecification>,
        IStartVisitor<ConstantExpression>, IEndVisitor<ConstantExpression>,
        IStartVisitor<RecordConstantExpression>, IEndVisitor<RecordConstantExpression>,
        IStartVisitor<Expression>, IEndVisitor<Expression>,
        IStartVisitor<SimpleExpression>, IEndVisitor<SimpleExpression>,
        IStartVisitor<Term>, IEndVisitor<Term>,
        IStartVisitor<Factor>, IEndVisitor<Factor>,
        IStartVisitor<UsesClause>, IEndVisitor<UsesClause>,
        IStartVisitor<UsesFileClause>, IEndVisitor<UsesFileClause>,
        IStartVisitor<PackageRequires>, IEndVisitor<PackageRequires>,
        IStartVisitor<PackageContains>, IEndVisitor<PackageContains>,
        IStartVisitor<StructType>, IEndVisitor<StructType>,
        IStartVisitor<ArrayType>, IEndVisitor<ArrayType>,
        IStartVisitor<SetDefinition>, IEndVisitor<SetDefinition>,
        IStartVisitor<FileType>, IEndVisitor<FileType>,
        IStartVisitor<ClassOfDeclaration>, IEndVisitor<ClassOfDeclaration>,
        IStartVisitor<TypeName>, IEndVisitor<TypeName>,
        IStartVisitor<SimpleType>, IEndVisitor<SimpleType>,
        IStartVisitor<EnumTypeDefinition>, IEndVisitor<EnumTypeDefinition>,
        IStartVisitor<EnumValue>, IEndVisitor<EnumValue>,
        IStartVisitor<ArrayIndex>, IEndVisitor<ArrayIndex>,
        IStartVisitor<PointerType>, IEndVisitor<PointerType>,
        IStartVisitor<StringType>, IEndVisitor<StringType>,
        IStartVisitor<ProcedureTypeDefinition>, IEndVisitor<ProcedureTypeDefinition>,
        IStartVisitor<FormalParameterDefinition>, IEndVisitor<FormalParameterDefinition>,
        IStartVisitor<FormalParameter>, IEndVisitor<FormalParameter>,
        IStartVisitor<UnitInitialization>, IEndVisitor<UnitInitialization>,
        IStartVisitor<UnitFinalization>, IEndVisitor<UnitFinalization>,
        IStartVisitor<CompoundStatement>, IEndVisitor<CompoundStatement>,
        IStartVisitor<Label>, IEndVisitor<Label>,
        IStartVisitor<ClassDeclaration>, IEndVisitor<ClassDeclaration>,
        IStartVisitor<ClassDeclarationItem>, IEndVisitor<ClassDeclarationItem>,
        IStartVisitor<ClassField>, IEndVisitor<ClassField>,
        IStartVisitor<ClassProperty>, IEndVisitor<ClassProperty>,
        IStartVisitor<ClassPropertyReadWrite>, IEndVisitor<ClassPropertyReadWrite>,
        IStartVisitor<ClassPropertyDispInterface>, IEndVisitor<ClassPropertyDispInterface>,
        IStartVisitor<ClassPropertySpecifier>, IEndVisitor<ClassPropertySpecifier>,
        IStartVisitor<ClassMethod>, IEndVisitor<ClassMethod>,
        IStartVisitor<MethodResolution>, IEndVisitor<MethodResolution>,
        IStartVisitor<ReintroduceDirective>, IEndVisitor<ReintroduceDirective>,
        IStartVisitor<OverloadDirective>, IEndVisitor<OverloadDirective>,
        IStartVisitor<DispIdDirective>, IEndVisitor<DispIdDirective>,
        IStartVisitor<InlineDirective>, IEndVisitor<InlineDirective>,
        IStartVisitor<AbstractDirective>, IEndVisitor<AbstractDirective>,
        IStartVisitor<OldCallConvention>, IEndVisitor<OldCallConvention>,
        IStartVisitor<ExternalDirective>, IEndVisitor<ExternalDirective>,
        IStartVisitor<ExternalSpecifier>, IEndVisitor<ExternalSpecifier>,
        IStartVisitor<CallConvention>, IEndVisitor<CallConvention>,
        IStartVisitor<BindingDirective>, IEndVisitor<BindingDirective>,
        IStartVisitor<ExportedProcedureHeading>, IEndVisitor<ExportedProcedureHeading>,
        IStartVisitor<UnsafeDirective>, IEndVisitor<UnsafeDirective>,
        IStartVisitor<ForwardDirective>, IEndVisitor<ForwardDirective>,
        IStartVisitor<ExportsSection>, IEndVisitor<ExportsSection>,
        IStartVisitor<ExportItem>, IEndVisitor<ExportItem>,
        IStartVisitor<RecordItem>, IEndVisitor<RecordItem>,
        IStartVisitor<RecordDeclaration>, IEndVisitor<RecordDeclaration>,
        IStartVisitor<RecordField>, IEndVisitor<RecordField>,
        IStartVisitor<RecordVariantSection>, IEndVisitor<RecordVariantSection>,
        IStartVisitor<RecordVariant>, IEndVisitor<RecordVariant>,
        IStartVisitor<RecordHelperDefinition>, IEndVisitor<RecordHelperDefinition>,
        IStartVisitor<RecordHelperItem>, IEndVisitor<RecordHelperItem>,
        IStartVisitor<ObjectDeclaration>, IEndVisitor<ObjectDeclaration>,
        IStartVisitor<ObjectItem>, IEndVisitor<ObjectItem>,
        IStartVisitor<InterfaceDefinition>, IEndVisitor<InterfaceDefinition>,
        IStartVisitor<InterfaceGuid>, IEndVisitor<InterfaceGuid>,
        IStartVisitor<ClassHelperDef>, IEndVisitor<ClassHelperDef>,
        IStartVisitor<ClassHelperItem>, IEndVisitor<ClassHelperItem>,
        IStartVisitor<ProcedureDeclaration>, IEndVisitor<ProcedureDeclaration>,
        IStartVisitor<Standard.MethodDeclaration>, IEndVisitor<Standard.MethodDeclaration>,
        IStartVisitor<StatementPart>, IEndVisitor<StatementPart>,
        IStartVisitor<ClosureExpression>, IEndVisitor<ClosureExpression>,
        IStartVisitor<RaiseStatement>, IEndVisitor<RaiseStatement>,
        IStartVisitor<TryStatement>, IEndVisitor<TryStatement>,
        IStartVisitor<ExceptHandlers>, IEndVisitor<ExceptHandlers>,
        IStartVisitor<ExceptHandler>, IEndVisitor<ExceptHandler>,
        IStartVisitor<WithStatement>, IEndVisitor<WithStatement>,
        IStartVisitor<ForStatement>, IEndVisitor<ForStatement>,
        IStartVisitor<WhileStatement>, IEndVisitor<WhileStatement>,
        IStartVisitor<RepeatStatement>, IEndVisitor<RepeatStatement>,
        IStartVisitor<CaseStatement>, IEndVisitor<CaseStatement>,
        IStartVisitor<CaseItem>, IEndVisitor<CaseItem>,
        IStartVisitor<CaseLabel>, IEndVisitor<CaseLabel>,
        IStartVisitor<IfStatement>, IEndVisitor<IfStatement>,
        IStartVisitor<GoToStatement>, IEndVisitor<GoToStatement>,
        IStartVisitor<AsmBlock>, IEndVisitor<AsmBlock>,
        IStartVisitor<AsmPseudoOp>, IEndVisitor<AsmPseudoOp>,
        IStartVisitor<LocalAsmLabel>, IEndVisitor<LocalAsmLabel>,
        IStartVisitor<AsmStatement>, IEndVisitor<AsmStatement>,
        IStartVisitor<AsmOperand>, IEndVisitor<AsmOperand>,
        IStartVisitor<AsmExpression>, IEndVisitor<AsmExpression>,
        IStartVisitor<AsmTerm>, IEndVisitor<AsmTerm>,
        IStartVisitor<DesignatorStatement>, IEndVisitor<DesignatorStatement>,
        IStartVisitor<DesignatorItem>, IEndVisitor<DesignatorItem>,
        IStartVisitor<Parameter>, IEndVisitor<Parameter>,
        IStartVisitor<Standard.FormattedExpression>, IEndVisitor<Standard.FormattedExpression>,
        IStartVisitor<SetSection>, IEndVisitor<SetSection>,
        IStartVisitor<SetSectnPart>, IEndVisitor<SetSectnPart>,
        IStartVisitor<AsmFactor>, IEndVisitor<AsmFactor> {

        private Visitor visitor;

        public IStartEndVisitor AsVisitor() {
            return visitor;
        }

        #region Unit

        public void StartVisit(Unit unit) {
            CompilationUnit result = AddNode<CompilationUnit, Unit>(unit, Project);
            result.FileType = CompilationUnitType.Unit;
            result.UnitName = ExtractSymbolName(unit.UnitName);
            result.Hints = ExtractHints(unit.Hints);
            result.FilePath = unit.FilePath;
            result.InterfaceSymbols = new DeclaredSymbols() { Parent = result };
            result.ImplementationSymbols = new DeclaredSymbols() { Parent = result };
            Project.Add(result, LogSource);
            CurrentUnit = result;
        }

        public void EndVisit(Unit unit) {
            CurrentUnit = null;
            RemoveFromStack(unit);
        }

        #endregion
        #region Library

        public void StartVisit(Library library) {
            CompilationUnit result = AddNode<CompilationUnit, Library>(library, Project);
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
        }

        public void EndVisit(Library library) {
            CurrentUnitMode.Reset(CurrentUnit);
            CurrentUnit = null;
            RemoveFromStack(library);
        }

        #endregion
        #region Program

        /// <summary>
        ///     visit a program
        /// </summary>
        /// <param name="program"></param>
        /// <param name="parameter"></param>
        public void StartVisit(Program program) {
            CompilationUnit result = AddNode<CompilationUnit, Program>(program, Project);
            result.FileType = CompilationUnitType.Program;
            result.UnitName = ExtractSymbolName(program.ProgramName);
            result.FilePath = program.FilePath;
            result.InitializationBlock = new BlockOfStatements();
            result.Symbols = new DeclaredSymbols() { Parent = result };
            CurrentUnitMode[result] = UnitMode.Program;
            Project.Add(result, LogSource);
            CurrentUnit = result;
        }

        public void EndVisit(Program program) {
            CurrentUnitMode.Reset(CurrentUnit);
            CurrentUnit = null;
            RemoveFromStack(program);
        }

        #endregion
        #region Package

        public void StartVisit(Package package) {
            CompilationUnit result = AddNode<CompilationUnit, Package>(package, Project);
            result.FileType = CompilationUnitType.Package;
            result.UnitName = ExtractSymbolName(package.PackageName);
            result.FilePath = package.FilePath;
            Project.Add(result, LogSource);
            CurrentUnit = result;
        }

        public void EndVisit(Package package) {
            CurrentUnit = null;
            RemoveFromStack(package);
        }

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
            RemoveFromStack(unitInterface);
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
            RemoveFromStack(unit);
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

        public void EndVisit(ConstSection constSection) {
            CurrentDeclarationMode = DeclarationMode.Unknown;
        }

        #endregion
        #region TypeSection

        public void StartVisit(TypeSection typeSection) {
            CurrentDeclarationMode = DeclarationMode.Types;
        }

        public void EndVisit(TypeSection typeSection) {
            CurrentDeclarationMode = DeclarationMode.Unknown;
        }

        #endregion
        #region TypeDeclaration

        public void StartVisit(Standard.TypeDeclaration typeDeclaration) {
            var symbols = LastValue as IDeclaredSymbolTarget;
            Abstract.TypeDeclaration declaration = AddNode<Abstract.TypeDeclaration, Standard.TypeDeclaration>(typeDeclaration);
            declaration.Name = ExtractSymbolName(typeDeclaration.TypeId?.Identifier);
            declaration.Generics = ExtractGenericDefinition(declaration, typeDeclaration.TypeId?.GenericDefinition);
            declaration.Attributes = ExtractAttributes(typeDeclaration.Attributes, CurrentUnit);
            declaration.Hints = ExtractHints(typeDeclaration.Hint);
            symbols.Symbols.AddDirect(declaration, LogSource);
        }

        public void EndVisit(Standard.TypeDeclaration typeDeclaration) {
            RemoveFromStack(typeDeclaration);
        }

        #endregion
        #region ConstDeclaration

        public void StartVisit(ConstDeclaration constDeclaration) {
            var symbols = LastValue as IDeclaredSymbolTarget;
            ConstantDeclaration declaration = AddNode<ConstantDeclaration, ConstDeclaration>(constDeclaration);
            declaration.Name = ExtractSymbolName(constDeclaration.Identifier);
            declaration.Mode = CurrentDeclarationMode;
            declaration.Attributes = ExtractAttributes(constDeclaration.Attributes, CurrentUnit);
            declaration.Hints = ExtractHints(constDeclaration.Hint);
            symbols.Symbols.AddDirect(declaration, LogSource);
        }

        public void EndVisit(ConstDeclaration typeDeclaration) {
            RemoveFromStack(typeDeclaration);
        }

        #endregion

        #region VarSection

        public void StartVisit(LabelDeclarationSection lblSection) {
            CurrentDeclarationMode = DeclarationMode.Label;
        }

        public void EndVisit(LabelDeclarationSection lblSection) {
            CurrentDeclarationMode = DeclarationMode.Unknown;
        }

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

        public void EndVisit(VarSection varSection) {
            CurrentDeclarationMode = DeclarationMode.Unknown;
        }


        #endregion
        #region VarDeclaration

        public void StartVisit(VarDeclaration varDeclaration) {
            var symbols = LastValue as IDeclaredSymbolTarget;
            VariableDeclaration declaration = AddNode<VariableDeclaration, VarDeclaration>(varDeclaration);
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
        }

        public void EndVisit(VarDeclaration varDeclaration) {
            RemoveFromStack(varDeclaration);
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

            if (constExpression.IsSetConstant) {
                SetConstant result = AddNode<SetConstant, ConstantExpression>(constExpression);
                DefineExpressionValue(result);
            }

            if (constExpression.IsRecordConstant) {
                RecordConstant result = AddNode<RecordConstant, ConstantExpression>(constExpression);
                DefineExpressionValue(result);
            }

        }

        public void EndVisit(ConstantExpression constExpression) {
            if (constExpression.IsRecordConstant || constExpression.IsSetConstant) {
                RemoveFromStack(constExpression);
            }
        }

        #endregion
        #region RecordConstantExpression

        public void StartVisit(RecordConstantExpression constExpression) {
            RecordConstantItem expression = CreateNode<RecordConstantItem>(constExpression);
            DefineExpressionValue(expression);
            expression.Name = ExtractSymbolName(constExpression.Name);
            return expression;
        }

        public void EndVisit(RecordConstantExpression constExpression) {
        }


        #endregion
        #region Expression

        public void StartVisit(Expression expression) {
            if (expression.LeftOperand != null && expression.RightOperand != null) {
                BinaryOperator currentExpression = CreateNode<BinaryOperator>(expression);
                DefineExpressionValue(currentExpression);
                currentExpression.Kind = BinaryOperator.ConvertKind(expression.Kind);
                return currentExpression;
            }
            ;
        }

        #endregion
        #region SimpleExpression

        public void StartVisit(SimpleExpression simpleExpression) {
            if (simpleExpression.LeftOperand != null && simpleExpression.RightOperand != null) {
                BinaryOperator currentExpression = CreateNode<BinaryOperator>(simpleExpression);
                DefineExpressionValue(currentExpression);
                currentExpression.Kind = BinaryOperator.ConvertKind(simpleExpression.Kind);
                return currentExpression;
            }
            ;
        }

        #endregion       
        #region Term

        public void StartVisit(Term term) {
            if (term.LeftOperand != null && term.RightOperand != null) {
                BinaryOperator currentExpression = CreateNode<BinaryOperator>(term);
                DefineExpressionValue(currentExpression);
                currentExpression.Kind = BinaryOperator.ConvertKind(term.Kind);
                return currentExpression;
            }
            ;
        }

        #endregion
        #region Factor

        public void StartVisit(Factor factor) {

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
            ;
        }

        #endregion
        #region UsesClause

        public void StartVisit(UsesClause unit) {
            if (unit.UsesList == null)
                ;

            foreach (ISyntaxPart part in unit.UsesList.Parts) {
                var name = part as NamespaceName;
                if (name == null)
                    continue;

                RequiredUnitName unitName = CreatePartNode<RequiredUnitName>(CurrentUnit.RequiredUnits, part);
                unitName.Name = ExtractSymbolName(name);
                unitName.Mode = CurrentUnitMode[CurrentUnit];
                CurrentUnit.RequiredUnits.Add(unitName, LogSource);
            }

            ;
        }

        #endregion
        #region UsesFileClause

        public void StartVisit(UsesFileClause unit) {
            if (unit.Files == null)
                ;

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

            ;
        }

        #endregion
        #region PackageRequires

        public void StartVisit(PackageRequires requires) {
            CurrentUnitMode[CurrentUnit] = UnitMode.Requires;

            if (requires.RequiresList == null)
                ;

            foreach (ISyntaxPart part in requires.RequiresList.Parts) {
                var name = part as NamespaceName;
                if (name == null)
                    continue;

                RequiredUnitName unitName = CreatePartNode<RequiredUnitName>(CurrentUnit.RequiredUnits, part);
                unitName.Name = ExtractSymbolName(name);
                unitName.Mode = CurrentUnitMode[CurrentUnit];
                CurrentUnit.RequiredUnits.Add(unitName, LogSource);
            }

            ;
        }

        public void EndVisit(PackageRequires requires)
            => CurrentUnitMode[CurrentUnit] = UnitMode.Interface;

        #endregion
        #region PackageContains

        public void StartVisit(PackageContains contains) {
            CurrentUnitMode[CurrentUnit] = UnitMode.Contains;

            if (contains.ContainsList == null)
                ;

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

            ;
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

        public void StartVisit(SetDefinition set) {
            SetTypeDeclaration value = CreateNode<SetTypeDeclaration>(set);
            value.PackedType = CurrentStructTypeMode == StructTypeMode.Packed;
            DefineTypeValue(value);
            return value;
        }

        #endregion
        #region FileTypeDefinition

        public void StartVisit(FileType set) {
            FileTypeDeclaration value = CreateNode<FileTypeDeclaration>(set);
            value.PackedType = CurrentStructTypeMode == StructTypeMode.Packed;
            DefineTypeValue(value);
            return value;
        }

        #endregion
        #region ClassOf

        public void StartVisit(ClassOfDeclaration classOf) {
            ClassOfTypeDeclaration value = CreateNode<ClassOfTypeDeclaration>(classOf);
            value.PackedType = CurrentStructTypeMode == StructTypeMode.Packed;
            DefineTypeValue(value);
            return value;
        }

        #endregion
        #region TypeName                                       

        public void StartVisit(TypeName typeName) {
            MetaType value = CreateNode<MetaType>(typeName);
            value.Kind = typeName.MapTypeKind();
            DefineTypeValue(value);
        }

        private AbstractSyntaxPart BeginVisitChildItem(TypeName typeName, ISyntaxPart part) {
            var name = part as GenericNamespaceName;
            var value = LastValue as MetaType;

            if (name == null || value == null)
                ;

            GenericNameFragment fragment = CreatePartNode<GenericNameFragment>(value, name);
            fragment.Name = ExtractSymbolName(name.Name);
            value.AddFragment(fragment);
            return fragment;
        }


        #endregion
        #region SimpleType

        public void StartVisit(SimpleType simpleType) {
            if (simpleType.SubrangeStart != null) {
                SubrangeType subrange = CreateNode<SubrangeType>(simpleType);
                DefineTypeValue(subrange);
                return subrange;
            }

            if (simpleType.EnumType != null)
                ;

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
                ;

            GenericNameFragment fragment = CreatePartNode<GenericNameFragment>(value, name);
            fragment.Name = ExtractSymbolName(name.Name);
            value.AddFragment(fragment);
            return fragment;
        }

        #endregion
        #region EnumTypeDefinition

        public void StartVisit(EnumTypeDefinition type) {
            EnumType value = CreateNode<EnumType>(type);
            DefineTypeValue(value);
            return value;
        }


        #endregion
        #region EnumValue

        public void StartVisit(EnumValue enumValue) {
            var enumDeclaration = LastValue as EnumType;
            if (enumDeclaration != null) {
                EnumTypeValue value = CreateNode<EnumTypeValue>(enumValue);
                value.Name = ExtractSymbolName(enumValue.EnumName);
                enumDeclaration.Add(value, LogSource);
                return value;
            }
            ;
        }

        #endregion
        #region ArrayIndex

        public void StartVisit(ArrayIndex arrayIndex) {
            if (arrayIndex.EndIndex != null) {
                BinaryOperator binOp = CreateNode<BinaryOperator>(arrayIndex);
                DefineExpressionValue(binOp);
                binOp.Kind = ExpressionKind.RangeOperator;
                return binOp;
            }

            ;
        }

        #endregion
        #region PointerType

        public void StartVisit(PointerType pointer) {
            if (pointer.GenericPointer) {
                MetaType result = CreateNode<MetaType>(pointer);
                result.Kind = MetaTypeKind.Pointer;
                DefineTypeValue(result);
                AddToStack(x, result);
            }
            else {
                PointerToType result = CreateNode<PointerToType>(pointer);
                DefineTypeValue(result);
                AddToStack(x, result);
            }
        }

        #endregion
        #region StringType

        public void StartVisit(StringType stringType) {
            MetaType result = CreateNode<MetaType>(stringType);
            result.Kind = MetaType.ConvertKind(stringType.Kind);
            DefineTypeValue(result);
            AddToStack(x, result);
        }

        #endregion
        #region ProcedureTypeDefinition

        public void StartVisit(ProcedureTypeDefinition proceduralType) {
            ProceduralType result = CreateNode<ProceduralType>(proceduralType);
            DefineTypeValue(result);
            result.Kind = Abstract.MethodDeclaration.MapKind(proceduralType.Kind);
            result.MethodDeclaration = proceduralType.MethodDeclaration;
            result.AllowAnonymousMethods = proceduralType.AllowAnonymousMethods;

            if (proceduralType.ReturnTypeAttributes != null)
                result.ReturnAttributes = ExtractAttributes(proceduralType.ReturnTypeAttributes, CurrentUnit);

            AddToStack(x, result);
        }

        #endregion
        #region FormalParameterDefinition

        public void StartVisit(FormalParameterDefinition formalParameter) {
            var paramterTarget = LastValue as IParameterTarget;
            ParameterTypeDefinition result = CreatePartNode<ParameterTypeDefinition>(paramterTarget.Parameters, formalParameter);
            paramterTarget.Parameters.Items.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region FormalParameter

        public void StartVisit(FormalParameter formalParameter) {
            ParameterDefinition result = CreateNode<ParameterDefinition>(formalParameter);
            var typeDefinition = LastValue as ParameterTypeDefinition;
            var allParams = typeDefinition.Parent as ParameterDefinitions;
            result.Name = ExtractSymbolName(formalParameter.ParameterName);
            result.Attributes = ExtractAttributes(formalParameter.Attributes, CurrentUnit);
            result.ParameterKind = ParameterDefinition.MapKind(formalParameter.ParameterType);
            typeDefinition.Parameters.Add(result);
            allParams.Add(result, LogSource);
            AddToStack(x, result);
        }

        #endregion   
        #region UnitInitialization

        public void StartVisit(UnitInitialization unitBlock) {
            BlockOfStatements result = CreatePartNode<BlockOfStatements>(CurrentUnit, unitBlock);
            CurrentUnit.InitializationBlock = result;
            AddToStack(x, result);
        }

        #endregion
        #region UnitFinalization

        public void StartVisit(UnitFinalization unitBlock) {
            BlockOfStatements result = CreatePartNode<BlockOfStatements>(CurrentUnit, unitBlock);
            CurrentUnit.FinalizationBlock = result;
            AddToStack(x, result);
        }

        #endregion
        #region CompoundStatement

        public void StartVisit(CompoundStatement block) {

            if (block.AssemblerBlock != null) {
                var statementTarget = LastValue as IStatementTarget;
                var blockTarget = LastValue as IBlockTarget;
                BlockOfAssemblerStatements result = CreatePartNode<BlockOfAssemblerStatements>(LastValue, block);
                if (statementTarget != null)
                    statementTarget.Statements.Add(result);
                else if (blockTarget != null)
                    blockTarget.Block = result;
                AddToStack(x, result);
            }

            else {
                var statementTarget = LastValue as IStatementTarget;
                var blockTarget = LastValue as IBlockTarget;
                BlockOfStatements result = CreatePartNode<BlockOfStatements>(LastValue, block);
                if (statementTarget != null)
                    statementTarget.Statements.Add(result);
                else if (blockTarget != null)
                    blockTarget.Block = result;
                AddToStack(x, result);
            }

        }

        #endregion
        #region Label

        public void StartVisit(Label label) {
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
                ;

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
                ;
            parent.LabelName = name;
            ;
        }

        #endregion
        #region ClassDeclaration

        public void StartVisit(ClassDeclaration classDeclaration) {
            StructuredType result = CreateNode<StructuredType>(classDeclaration);
            result.Kind = StructuredTypeKind.Class;
            result.SealedClass = classDeclaration.Sealed;
            result.AbstractClass = classDeclaration.Abstract;
            result.ForwardDeclaration = classDeclaration.ForwardDeclaration;
            DefineTypeValue(result);
            CurrentMemberVisibility[result] = MemberVisibility.Public;
            AddToStack(x, result);
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
                ;

            if (classDeclarationItem.Visibility != TokenKind.Undefined) {
                CurrentMemberVisibility[parentType] = StructuredType.MapVisibility(classDeclarationItem.Visibility, classDeclarationItem.Strict);
            };

            ;
        }

        #endregion
        #region ClassField

        public void StartVisit(ClassField field) {
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

            AddToStack(x, result);
        }

        #endregion
        #region ClassProperty

        public void StartVisit(ClassProperty property) {
            StructureProperty result = CreateNode<StructureProperty>(property);
            var parent = LastValue as StructuredType;
            var declItem = property.Parent as IStructuredTypeMember;
            result.Name = ExtractSymbolName(property.PropertyName);
            parent.Properties.Add(result, LogSource);
            result.Visibility = CurrentMemberVisibility[parent];

            if (declItem != null) {
                result.Attributes = ExtractAttributes(declItem.Attributes, CurrentUnit);
            }

            AddToStack(x, result);
        }

        #endregion    
        #region ClassPropertyReadWrite

        public void StartVisit(ClassPropertyReadWrite property) {
            var parent = LastValue as StructureProperty;
            StructurePropertyAccessor result = CreateNode<StructurePropertyAccessor>(property);
            result.Kind = StructurePropertyAccessor.MapKind(property.Kind);
            result.Name = ExtractSymbolName(property.Member);
            parent.Accessors.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region  ClassPropertyDispInterface

        public void StartVisit(ClassPropertyDispInterface property) {
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
            AddToStack(x, result);
        }

        #endregion
        #region ParseClassPropertyAccessSpecifier

        public void StartVisit(ClassPropertySpecifier property) {
            if (property.PropertyReadWrite != null)
                ;

            if (property.PropertyDispInterface != null)
                ;

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
            AddToStack(x, result);
        }

        #endregion
        #region ClassMethod

        public void StartVisit(ClassMethod method) {
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
            AddToStack(x, result);
        }

        #endregion
        #region MethodResolution

        public void StartVisit(MethodResolution methodResolution) {
            StructureMethodResolution result = CreateNode<StructureMethodResolution>(methodResolution);
            var parent = LastValue as StructuredType;
            result.Attributes = ExtractAttributes(((ClassDeclarationItem)methodResolution.Parent).Attributes, CurrentUnit);
            result.Kind = StructureMethodResolution.MapKind(methodResolution.Kind);
            result.Target = ExtractSymbolName(methodResolution.ResolveIdentifier);
            parent.MethodResolutions.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region ReintroduceDirective

        public void StartVisit(ReintroduceDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(directive);
            result.Kind = MethodDirectiveKind.Reintroduce;
            parent.Directives.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region OverloadDirective

        public void StartVisit(OverloadDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(directive);
            result.Kind = MethodDirectiveKind.Overload;
            parent.Directives.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region DispIdDirective

        public void StartVisit(DispIdDirective directive) {
            var parent = LastValue as IDirectiveTarget;

            if (parent == null)
                ;

            MethodDirective result = CreateNode<MethodDirective>(directive);
            result.Kind = MethodDirectiveKind.DispId;
            parent.Directives.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region InlineDirective

        public void StartVisit(InlineDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(directive);

            if (directive.Kind == TokenKind.Inline) {
                result.Kind = MethodDirectiveKind.Inline;
            }
            else if (directive.Kind == TokenKind.Assembler) {
                result.Kind = MethodDirectiveKind.Assembler;
            }

            parent.Directives.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region AbstractDirective

        public void StartVisit(AbstractDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(directive);

            if (directive.Kind == TokenKind.Abstract) {
                result.Kind = MethodDirectiveKind.Abstract;
            }
            else if (directive.Kind == TokenKind.Final) {
                result.Kind = MethodDirectiveKind.Final;
            }

            parent.Directives.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region OldCallConvention

        public void StartVisit(OldCallConvention directive) {
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
            AddToStack(x, result);
        }

        #endregion
        #region ExternalDirective

        public void StartVisit(ExternalDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(directive);

            if (directive.Kind == TokenKind.VarArgs) {
                result.Kind = MethodDirectiveKind.VarArgs;
            }

            else if (directive.Kind == TokenKind.External) {
                result.Kind = MethodDirectiveKind.External;
            }

            parent.Directives.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region ExternalSpecifier

        public void StartVisit(ExternalSpecifier directive) {
            var parent = LastValue as MethodDirective;
            MethodDirectiveSpecifier result = CreateNode<MethodDirectiveSpecifier>(directive);
            result.Kind = MethodDirectiveSpecifier.MapKind(directive.Kind);
            parent.Specifiers.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region CallConvention

        public void StartVisit(CallConvention directive) {
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
            AddToStack(x, result);
        }

        #endregion
        #region BindingDirective

        public void StartVisit(BindingDirective directive) {
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
            AddToStack(x, result);
        }

        #endregion
        #region MethodDirectives

        private AbstractSyntaxPart BeginVisitChildItem(MethodDirectives parent, ISyntaxPart child) {
            var hints = child as HintingInformation;
            var lastValue = LastValue as IDirectiveTarget;

            if (hints != null && lastValue != null) {
                lastValue.Hints = ExtractHints(hints, lastValue.Hints);
            }

            ;
        }

        #endregion
        #region FunctionDirectives

        private AbstractSyntaxPart BeginVisitChildItem(FunctionDirectives parent, ISyntaxPart child) {
            var hints = child as HintingInformation;
            var lastValue = LastValue as IDirectiveTarget;

            if (hints != null && lastValue != null) {
                lastValue.Hints = ExtractHints(hints, lastValue.Hints);
            }

            ;
        }

        #endregion
        #region ExportedProcedureHeading

        public void StartVisit(ExportedProcedureHeading procHeading) {
            var symbols = LastValue as IDeclaredSymbolTarget;
            GlobalMethod result = CreateNode<GlobalMethod>(procHeading);
            result.Name = ExtractSymbolName(procHeading.Name);
            result.Kind = Abstract.MethodDeclaration.MapKind(procHeading.Kind);
            result.Attributes = ExtractAttributes(procHeading.Attributes, CurrentUnit);
            result.ReturnAttributes = ExtractAttributes(procHeading.ResultAttributes, CurrentUnit);
            symbols.Symbols.AddDirect(result, LogSource);
            AddToStack(x, result);
        }

        #endregion
        #region UnsafeDirective

        public void StartVisit(UnsafeDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(directive);
            result.Kind = MethodDirectiveKind.Unsafe;
            parent.Directives.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region ForwardDirective

        public void StartVisit(ForwardDirective directive) {
            var parent = LastValue as IDirectiveTarget;
            MethodDirective result = CreateNode<MethodDirective>(directive);
            result.Kind = MethodDirectiveKind.Forward;
            parent.Directives.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region ExportsSection

        public void StartVisit(ExportsSection exportsSection) {
            CurrentDeclarationMode = DeclarationMode.Exports;
            ;
        }

        private AbstractSyntaxPart EndVisitItem(ExportsSection exportsSection) {
            CurrentDeclarationMode = DeclarationMode.Unknown;
            ;
        }


        #endregion
        #region ExportItem

        public void StartVisit(ExportItem exportsSection) {
            var declarations = LastValue as IDeclaredSymbolTarget;
            ExportedMethodDeclaration result = CreateNode<ExportedMethodDeclaration>(exportsSection);
            result.Name = ExtractSymbolName(exportsSection.ExportName);
            result.IsResident = exportsSection.Resident;
            result.HasIndex = exportsSection.IndexParameter != null;
            result.HasName = exportsSection.NameParameter != null;
            declarations.Symbols.AddDirect(result, LogSource);
            AddToStack(x, result);
        }

        #endregion
        #region RecordItem

        public void StartVisit(RecordItem recordDeclarationItem) {
            var parentType = LastValue as StructuredType;

            if (parentType == null)
                ;

            if (recordDeclarationItem.Visibility != TokenKind.Undefined) {
                CurrentMemberVisibility[parentType] = StructuredType.MapVisibility(recordDeclarationItem.Visibility, recordDeclarationItem.Strict);
            };

            ;
        }

        #endregion
        #region RecordDeclaration

        public void StartVisit(RecordDeclaration recordDeclaration) {
            StructuredType result = CreateNode<StructuredType>(recordDeclaration);
            result.Kind = StructuredTypeKind.Record;
            DefineTypeValue(result);
            CurrentMemberVisibility[result] = MemberVisibility.Public;
            AddToStack(x, result);
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
            AddToStack(x, result);
        }


        #endregion
        #region ParseRecordVariantSection

        public void StartVisit(RecordVariantSection variantSection) {
            var structType = LastValue as StructuredType;
            StructureVariantItem result = CreateNode<StructureVariantItem>(variantSection);
            result.Name = ExtractSymbolName(variantSection.Name);
            structType.Variants.Items.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region RecordVariant

        public void StartVisit(RecordVariant variantItem) {
            var structType = LastValue as StructureVariantItem;
            StructureVariantFields result = CreateNode<StructureVariantFields>(variantItem);
            structType.Items.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region RecordHelperDefinition       

        public void StartVisit(RecordHelperDefinition recordHelper) {
            StructuredType result = CreateNode<StructuredType>(recordHelper);
            result.Kind = StructuredTypeKind.RecordHelper;
            DefineTypeValue(result);
            CurrentMemberVisibility[result] = MemberVisibility.Public;
            AddToStack(x, result);
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
                ;

            if (recordDeclarationItem.Visibility != TokenKind.Undefined) {
                CurrentMemberVisibility[parentType] = StructuredType.MapVisibility(recordDeclarationItem.Visibility, recordDeclarationItem.Strict);
            };

            ;
        }

        #endregion
        #region ObjectDeclaration       

        public void StartVisit(ObjectDeclaration objectDeclaration) {
            StructuredType result = CreateNode<StructuredType>(objectDeclaration);
            result.Kind = StructuredTypeKind.Object;
            DefineTypeValue(result);
            CurrentMemberVisibility[result] = MemberVisibility.Public;
            AddToStack(x, result);
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
                ;

            if (objectItem.Visibility != TokenKind.Undefined) {
                CurrentMemberVisibility[parentType] = StructuredType.MapVisibility(objectItem.Visibility, objectItem.Strict);
            };

            ;
        }

        #endregion
        #region InterfaceDefinition

        public void StartVisit(InterfaceDefinition interfaceDeclaration) {
            StructuredType result = CreateNode<StructuredType>(interfaceDeclaration);
            if (interfaceDeclaration.DisplayInterface)
                result.Kind = StructuredTypeKind.DispInterface;
            else
                result.Kind = StructuredTypeKind.Interface;

            result.ForwardDeclaration = interfaceDeclaration.ForwardDeclaration;
            DefineTypeValue(result);
            CurrentMemberVisibility[result] = MemberVisibility.Public;
            AddToStack(x, result);
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
                structType.GuidId = QuotedStringTokenValue.Unwrap(interfaceGuid.Id.FirstTerminalToken);
            }


            ;
        }

        #endregion
        #region ClassHelper

        public void StartVisit(ClassHelperDef classHelper) {
            StructuredType result = CreateNode<StructuredType>(classHelper);
            result.Kind = StructuredTypeKind.ClassHelper;
            DefineTypeValue(result);
            CurrentMemberVisibility[result] = MemberVisibility.Public;
            AddToStack(x, result);
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
                ;

            if (classHelperItem.Visibility != TokenKind.Undefined) {
                CurrentMemberVisibility[parentType] = StructuredType.MapVisibility(classHelperItem.Visibility, classHelperItem.Strict);
            };

            ;
        }

        #endregion
        #region ProcedureDeclaration

        public void StartVisit(ProcedureDeclaration procedure) {
            var symbolTarget = LastValue as IDeclaredSymbolTarget;
            MethodImplementation result = CreateNode<MethodImplementation>(procedure);
            result.Name = ExtractSymbolName(procedure.Heading.Name);
            result.Kind = Abstract.MethodDeclaration.MapKind(procedure.Heading.Kind);
            symbolTarget.Symbols.AddDirect(result, LogSource);
            AddToStack(x, result);
        }

        #endregion
        #region MethodDeclaration

        public void StartVisit(Standard.MethodDeclaration method) {
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

            AddToStack(x, result);
        }

        #endregion
        #region StatementPart

        public void StartVisit(StatementPart part) {

            if (part.DesignatorPart == null && part.Assignment == null)
                ;

            StructuredStatement result = CreateNode<StructuredStatement>(part);
            var target = LastValue as IStatementTarget;
            if (part.Assignment != null) {
                result.Kind = StructuredStatementKind.Assignment;
            }
            else {
                result.Kind = StructuredStatementKind.ExpressionStatement;
            }

            target.Statements.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region ClosureExpression

        public void StartVisit(ClosureExpression closure) {
            MethodImplementation result = CreateNode<MethodImplementation>(closure);

            result.Name = new SimpleSymbolName(CurrentUnit.GenerateSymbolName());
            IExpressionTarget expression = LastExpression;
            expression.Value = result;
            AddToStack(x, result);
        }

        #endregion
        #region RaiseStatement

        public void StartVisit(RaiseStatement raise) {
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
            AddToStack(x, result);
        }

        #endregion
        #region TryStatement

        public void StartVisit(TryStatement tryStatement) {
            StructuredStatement result = CreateNode<StructuredStatement>(tryStatement);
            var target = LastValue as IStatementTarget;
            target.Statements.Add(result);

            if (tryStatement.Finally != null) {
                result.Kind = StructuredStatementKind.TryFinally;
            }
            else if (tryStatement.Handlers != null) {
                result.Kind = StructuredStatementKind.TryExcept;
            }

            AddToStack(x, result);
        }

        private AbstractSyntaxPart BeginVisitChildItem(TryStatement tryStatement, ISyntaxPart child) {
            var statements = child as StatementList;
            if (statements == null)
                ;

            BlockOfStatements result = CreateNode<BlockOfStatements>(child);
            var target = LastValue as IStatementTarget;
            target.Statements.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region ExceptHandlers

        public void StartVisit(ExceptHandlers exceptHandlers) {
            StructuredStatement result = CreateNode<StructuredStatement>(exceptHandlers);
            var target = LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.ExceptElse;
            target.Statements.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region ExceptHandler

        public void StartVisit(ExceptHandler exceptHandler) {
            StructuredStatement result = CreateNode<StructuredStatement>(exceptHandler);
            var target = LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.ExceptOn;
            result.Name = ExtractSymbolName(exceptHandler.Name);
            target.Statements.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region WithStatement

        public void StartVisit(WithStatement withStatement) {
            StructuredStatement result = CreateNode<StructuredStatement>(withStatement);
            var target = LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.With;
            target.Statements.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region ForStatement

        public void StartVisit(ForStatement forStatement) {
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
            AddToStack(x, result);
        }

        #endregion
        #region WhileStatement

        public void StartVisit(WhileStatement withStatement) {
            StructuredStatement result = CreateNode<StructuredStatement>(withStatement);
            var target = LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.While;
            target.Statements.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region RepeatStatement

        public void StartVisit(RepeatStatement repeateStatement) {
            StructuredStatement result = CreateNode<StructuredStatement>(repeateStatement);
            var target = LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.Repeat;
            target.Statements.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region CaseStatement

        public void StartVisit(CaseStatement caseStatement) {
            StructuredStatement result = CreateNode<StructuredStatement>(caseStatement);
            var target = LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.Case;
            target.Statements.Add(result);
            AddToStack(x, result);
        }

        private AbstractSyntaxPart BeginVisitChildItem(CaseStatement caseStatement, ISyntaxPart child) {

            if (caseStatement.Else != child)
                ;

            StructuredStatement result = CreateNode<StructuredStatement>(caseStatement);
            var target = LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.CaseElse;
            target.Statements.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region CaseItem

        public void StartVisit(CaseItem caseItem) {
            StructuredStatement result = CreateNode<StructuredStatement>(caseItem);
            var target = LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.CaseItem;
            target.Statements.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region CaseLabel

        public void StartVisit(CaseLabel caseLabel) {
            if (caseLabel.EndExpression != null) {
                BinaryOperator binOp = CreateNode<BinaryOperator>(caseLabel);
                DefineExpressionValue(binOp);
                binOp.Kind = ExpressionKind.RangeOperator;
                return binOp;
            }

            ;
        }

        #endregion
        #region IfStatement

        public void StartVisit(IfStatement ifStatement) {
            StructuredStatement result = CreateNode<StructuredStatement>(ifStatement);
            var target = LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.IfThen;
            target.Statements.Add(result);
            AddToStack(x, result);
        }

        private AbstractSyntaxPart BeginVisitChildItem(IfStatement ifStatement, ISyntaxPart child) {

            if (ifStatement.ElsePart != child)
                ;

            StructuredStatement result = CreateNode<StructuredStatement>(ifStatement);
            var target = LastValue as IStatementTarget;
            result.Kind = StructuredStatementKind.IfElse;
            target.Statements.Add(result);
            AddToStack(x, result);
        }

        #endregion
        #region GoToStatement

        public void StartVisit(GoToStatement gotoStatement) {
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
            AddToStack(x, result);
        }

        #endregion
        #region AsmBlock

        public void StartVisit(AsmBlock block) {
            var statementTarget = LastValue as IStatementTarget;
            var blockTarget = LastValue as IBlockTarget;
            BlockOfAssemblerStatements result = CreatePartNode<BlockOfAssemblerStatements>(LastValue, block);
            if (statementTarget != null)
                statementTarget.Statements.Add(result);
            else if (blockTarget != null)
                blockTarget.Block = result;
            AddToStack(x, result);
        }

        #endregion
        #region AsmPseudoOp

        public void StartVisit(AsmPseudoOp op) {
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
            AddToStack(x, result);
        }

        #endregion
        #region LocalAsmLabel

        public void StartVisit(LocalAsmLabel label) {
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
            ;
        }

        #endregion
        #region AsmStatement

        public void StartVisit(AsmStatement statement) {
            AssemblerStatement result = CreateNode<AssemblerStatement>(statement);
            var parent = LastValue as BlockOfAssemblerStatements;
            parent.Statements.Add(result);
            result.OpCode = ExtractSymbolName(statement.OpCode?.OpCode);
            result.SegmentPrefix = ExtractSymbolName(statement.Prefix?.SegmentPrefix);
            result.LockPrefix = ExtractSymbolName(statement.Prefix?.LockPrefix);

            AddToStack(x, result);
        }

        #endregion
        #region ParseAssemblyOperand

        public void StartVisit(AsmOperand statement) {

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

            ;
        }

        #endregion
        #region AsmExpression

        public void StartVisit(AsmExpression statement) {

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

            ;
        }

        #endregion
        #region AsmTerm

        public void StartVisit(AsmTerm statement) {

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

            ;
        }

        #endregion
        #region DesignatorStatement

        public void StartVisit(DesignatorStatement designator) {
            if (!designator.Inherited && designator.Name == null)
                ;

            SymbolReference result = CreateNode<SymbolReference>(designator);
            if (designator.Inherited)
                result.Inherited = true;

            DefineExpressionValue(result);
            AddToStack(x, result);
        }

        #endregion
        #region DesignatorItem

        public void StartVisit(DesignatorItem designator) {
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

            ;
        }

        #endregion
        #region Parameter

        public void StartVisit(Parameter param) {
            if (param.ParameterName == null)
                ;

            SymbolReference result = CreateNode<SymbolReference>(param);
            result.NamedParameter = true;
            result.Name = ExtractSymbolName(param.ParameterName);
            DefineExpressionValue(result);
            AddToStack(x, result);
        }

        #endregion
        #region FormattedExpression

        public void StartVisit(Standard.FormattedExpression expr) {
            if (expr.Width == null && expr.Decimals == null)
                ;

            Abstract.FormattedExpression result = CreateNode<Abstract.FormattedExpression>(expr);
            DefineExpressionValue(result);
            AddToStack(x, result);
        }

        #endregion
        #region SetSection

        public void StartVisit(SetSection expr) {
            SetExpression result = CreateNode<SetExpression>(expr);
            DefineExpressionValue(result);
            AddToStack(x, result);
        }

        #endregion

        public void StartVisit(SetSectnPart part) {
            if (part.Continuation != TokenKind.DotDot) {
                var arrayExpression = LastExpression as SetExpression;

                if (arrayExpression == null)
                    ;

                var binOp = arrayExpression.Expressions.LastOrDefault() as BinaryOperator;

                if (binOp != null && binOp.RightOperand == null)
                    return binOp;

                ;
            }

            BinaryOperator result = CreateNode<BinaryOperator>(part);
            result.Kind = ExpressionKind.RangeOperator;
            DefineExpressionValue(result);
            AddToStack(x, result);
        }

        #region AsmFactor

        public void StartVisit(AsmFactor factor) {

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

            ;
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

            AddToStack(x, result);
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

            AddToStack(x, result);
        }

        private static SymbolName ExtractSymbolName(Identifier name) {
            var result = new SimpleSymbolName(name?.FirstTerminalToken?.Value);
            AddToStack(x, result);
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

            AddToStack(x, result);
        }

        private static GenericTypes ExtractGenericDefinition(AbstractSyntaxPart parent, GenericSuffix genericDefinition) {
            if (genericDefinition == null)
                ;

            GenericTypes result = CreatePartNode<GenericTypes>(parent, genericDefinition);
            result.TypeReference = true;

            AddToStack(x, result);
        }

        private GenericTypes ExtractGenericDefinition(AbstractSyntaxPart parent, GenericDefinition genericDefinition) {
            if (genericDefinition == null)
                ;

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


            AddToStack(x, result);
        }

        private static SymbolHints ExtractHints(HintingInformationList hints) {
            var result = new SymbolHints();

            if (hints == null || hints.PartList.Count < 1)
                AddToStack(x, result);

            foreach (ISyntaxPart part in hints.Parts) {
                var hint = part as HintingInformation;
                if (hint == null) continue;
                ExtractHints(hint, result);
            }

            AddToStack(x, result);
        }

        private static SymbolHints ExtractHints(HintingInformation hint, SymbolHints result = null) {
            if (result == null)
                result = new SymbolHints();

            result.SymbolIsDeprecated = result.SymbolIsDeprecated || hint.Deprecated;
            result.DeprecatedInformation = (result.DeprecatedInformation ?? string.Empty) + hint.DeprecatedComment?.UnquotedValue;
            result.SymbolInLibrary = result.SymbolInLibrary || hint.Library;
            result.SymbolIsPlatformSpecific = result.SymbolIsPlatformSpecific || hint.Platform;
            result.SymbolIsExperimental = result.SymbolIsExperimental || hint.Experimental;
            AddToStack(x, result);
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

            AddToStack(x, result);
        }

        #endregion
        #region Helper functions

        private ChildType AddNode<ChildType, NodeType>(NodeType node) {
            return default(ChildType);
        }

        private ChildType AddNode<ChildType, NodeType>(NodeType node, AbstractSyntaxPart paret) {
            return default(ChildType);
        }

        [Obsolete]
        private T CreateNode<T>(ISyntaxPart element) where T : AbstractSyntaxPart, new()
            => CreatePartNode<T>(LastValue, element);

        [Obsolete]
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

                ;
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
                    ;

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
                    ;
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
                    ;
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

        private void AddToStack<T>(T part, AbstractSyntaxPart result) {
            throw new NotImplementedException();
        }

        private void RemoveFromStack<T>(T part) {
            throw new NotImplementedException();
        }

    }
}
