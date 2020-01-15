using System;
using System.Collections.Generic;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Standard;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     convert a concrete syntax tree to an abstract one
    ///     using a working stack and a generic visitor pattern
    /// </summary>
    public class TreeTransformer :

        IStartVisitor<UnitSymbol>, IEndVisitor<UnitSymbol>,
        IStartVisitor<LibrarySymbol>, IEndVisitor<LibrarySymbol>,
        IStartVisitor<ProgramSymbol>, IEndVisitor<ProgramSymbol>,
        IStartVisitor<PackageSymbol>, IEndVisitor<PackageSymbol>,
        IStartVisitor<UnitInterfaceSymbol>, IEndVisitor<UnitInterfaceSymbol>,
        IStartVisitor<UnitImplementationSymbol>, IEndVisitor<UnitImplementationSymbol>,
        IStartVisitor<ConstSectionSymbol>, IEndVisitor<ConstSectionSymbol>,
        IStartVisitor<TypeSectionSymbol>, IEndVisitor<TypeSectionSymbol>,
        IStartVisitor<Standard.TypeDeclarationSymbol>, IEndVisitor<Standard.TypeDeclarationSymbol>,
        IStartVisitor<ConstDeclarationSymbol>,
        IStartVisitor<LabelDeclarationSection>, IEndVisitor<LabelDeclarationSection>,
        IStartVisitor<VarSection>, IEndVisitor<VarSection>,
        IStartVisitor<VarDeclaration>,
        IStartVisitor<VarValueSpecification>, IEndVisitor<VarValueSpecification>,
        IStartVisitor<ConstantExpressionSymbol>,
        IStartVisitor<RecordConstantExpressionSymbol>,
        IStartVisitor<ExpressionSymbol>,
        IStartVisitor<SimpleExpression>,
        IStartVisitor<TermSymbol>,
        IStartVisitor<FactorSymbol>,
        IStartVisitor<UsesClauseSymbol>,
        IStartVisitor<UsesFileClauseSymbol>,
        IStartVisitor<PackageRequiresSymbol>, IEndVisitor<PackageRequiresSymbol>,
        IStartVisitor<PackageContainsSymbol>, IEndVisitor<PackageContainsSymbol>,
        IStartVisitor<StructTypeSymbol>, IEndVisitor<StructTypeSymbol>,
        IStartVisitor<ArrayTypeSymbol>,
        IStartVisitor<SetDefinitionSymbol>,
        IStartVisitor<FileTypeSymbol>,
        IStartVisitor<ClassOfDeclarationSymbol>,
        IStartVisitor<TypeNameSymbol>,
        IStartVisitor<SimpleTypeSymbol>,
        IStartVisitor<EnumTypeDefinitionSymbol>,
        IStartVisitor<EnumValueSymbol>,
        IStartVisitor<ArrayIndexSymbol>,
        IStartVisitor<PointerTypeSymbol>,
        IStartVisitor<StringTypeSymbol>,
        IStartVisitor<ProcedureTypeDefinitionSymbol>,
        IStartVisitor<ProcedureReferenceSymbol>,
        IStartVisitor<FormalParameterDefinitionSymbol>, IEndVisitor<FormalParameterDefinitionSymbol>,
        IStartVisitor<FormalParameterSymbol>,
        IStartVisitor<UnitInitializationSymbol>,
        IStartVisitor<UnitFinalizationSymbol>,
        IStartVisitor<CompoundStatementSymbol>, IEndVisitor<CompoundStatementSymbol>,
        IStartVisitor<LabelSymbol>, IEndVisitor<LabelSymbol>,
        IStartVisitor<ClassDeclarationSymbol>, IEndVisitor<ClassDeclarationSymbol>,
        IStartVisitor<ClassDeclarationItemSymbol>,
        IStartVisitor<ClassFieldSymbol>,
        IStartVisitor<ClassPropertySymbol>,
        IStartVisitor<ClassPropertyReadWriteSymbol>,
        IStartVisitor<ClassPropertyDispInterfaceSymbols>,
        IStartVisitor<ClassPropertySpecifierSymbol>,
        IStartVisitor<ClassMethodSymbol>,
        IStartVisitor<MethodResolutionSymbol>,
        IStartVisitor<ReintroduceSymbol>,
        IStartVisitor<OverloadSymbol>,
        IStartVisitor<DispIdSymbol>,
        IStartVisitor<InlineSymbol>,
        IStartVisitor<AbstractSymbol>,
        IStartVisitor<OldCallConventionSymbol>,
        IStartVisitor<ExternalDirectiveSymbol>,
        IStartVisitor<ExternalSpecifierSymbol>,
        IStartVisitor<CallConventionSymbol>,
        IStartVisitor<BindingSymbol>,
        IStartVisitor<ExportedProcedureHeadingSymbol>,
        IStartVisitor<UnsafeDirectiveSymbol>,
        IStartVisitor<ForwardDirectiveSymbol>,
        IStartVisitor<ExportsSectionSymbol>,
        IStartVisitor<ExportItemSymbol>,
        IStartVisitor<RecordItemSymbol>,
        IStartVisitor<RecordDeclarationSymbol>,
        IStartVisitor<RecordFieldSymbol>,
        IStartVisitor<RecordVariantSectionSymbol>,
        IStartVisitor<RecordVariantSymbol>,
        IStartVisitor<RecordHelperDefinitionSymbol>,
        IStartVisitor<RecordHelperItemSymbol>,
        IStartVisitor<ObjectDeclarationSymbol>,
        IStartVisitor<ObjectItem>,
        IStartVisitor<InterfaceDefinitionSymbol>,
        IStartVisitor<InterfaceGuidSymbol>,
        IStartVisitor<ClassHelperDefSymbol>,
        IStartVisitor<ClassHelperItemSymbol>,
        IStartVisitor<ProcedureDeclarationSymbol>,
        IStartVisitor<MethodDeclarationSymbol>,
        IStartVisitor<StatementPart>,
        IStartVisitor<ClosureExpressionSymbol>,
        IStartVisitor<RaiseStatementSymbol>,
        IStartVisitor<TryStatementSymbol>,
        IStartVisitor<ExceptHandlersSymbol>,
        IStartVisitor<ExceptHandlerSymbol>,
        IStartVisitor<WithStatementSymbol>,
        IStartVisitor<ForStatementSymbol>,
        IStartVisitor<WhileStatementSymbol>,
        IStartVisitor<RepeatStatement>,
        IStartVisitor<CaseStatementSymbol>,
        IStartVisitor<CaseItemSymbol>,
        IStartVisitor<CaseLabelSymbol>,
        IStartVisitor<IfStatementSymbol>,
        IStartVisitor<GoToStatementSymbol>,
        IStartVisitor<AsmBlockSymbol>,
        IStartVisitor<AsmPseudoOpSymbol>,
        IStartVisitor<LocalAsmLabelSymbol>,
        IStartVisitor<AsmStatementSymbol>,
        IStartVisitor<AsmOperandSymbol>,
        IStartVisitor<AsmExpressionSymbol>,
        IStartVisitor<AsmTermSymbol>,
        IStartVisitor<DesignatorStatementSymbol>,
        IStartVisitor<DesignatorItemSymbol>,
        IStartVisitor<ParameterSymbol>,
        IStartVisitor<FormattedExpressionSymbol>,
        IStartVisitor<SetSectionSymbol>,
        IStartVisitor<SetSectionPartSymbol>,
        IStartVisitor<AsmFactorSymbol>,
        IChildVisitor<CaseStatementSymbol>,
        IChildVisitor<TypeNameSymbol>,
        IChildVisitor<SimpleTypeSymbol>,
        IChildVisitor<MethodDirectivesSymbol>,
        IChildVisitor<FunctionDirectivesSymbol>,
        IChildVisitor<TryStatementSymbol>,
        IChildVisitor<IfStatementSymbol> {

        private readonly Visitor visitor;
        private readonly IParserEnvironment environment;

        /// <summary>
        ///     use as common visitor
        /// </summary>
        /// <returns></returns>
        public IStartEndVisitor AsVisitor()
            => visitor;

        #region Unit

        /// <summary>
        ///     visit a unit
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(UnitSymbol element) {
            var result = new CompilationUnit();
            InitNode(result, element);
            result.FileType = CompilationUnitType.Unit;
            result.UnitName = ExtractSymbolName(element.UnitName);
            result.Hints = ExtractHints(element.Hints);
            result.InterfaceSymbols = new DeclaredSymbolCollection();
            result.ImplementationSymbols = new DeclaredSymbolCollection();
            result.FilePath = element.FilePath;
            Project.Add(result, LogSource);
            CurrentUnit = result;
        }

        /// <summary>
        ///     end visiting a compilation unit
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(UnitSymbol element)
            => CurrentUnit = null;

        #endregion
        #region Library

        /// <summary>
        ///     visit a library
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(LibrarySymbol element) {
            var result = new CompilationUnit();
            InitNode(result, element);
            result.FileType = CompilationUnitType.Library;
            result.UnitName = ExtractSymbolName(element.LibraryName);
            result.Hints = ExtractHints(element.Hints);
            result.FilePath = element.FilePath;
            if ((element.MainBlock.Body as BlockBodySymbol)?.AssemblerBlock != null)
                result.InitializationBlock = new BlockOfAssemblerStatements();
            else
                result.InitializationBlock = new BlockOfStatements();
            result.Symbols = new DeclaredSymbolCollection();
            Project.Add(result, LogSource);
            CurrentUnitMode[result] = UnitMode.Library;
            CurrentUnit = result;
        }

        /// <summary>
        ///     end visiting a library
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(LibrarySymbol element) {
            CurrentUnitMode.Reset(CurrentUnit);
            CurrentUnit = null;
        }

        #endregion
        #region Program

        /// <summary>
        ///     visit a program
        /// </summary>
        /// <param name="element">program to visit</param>
        public void StartVisit(ProgramSymbol element) {
            var result = new CompilationUnit();
            InitNode(result, element);
            result.FileType = CompilationUnitType.Program;
            result.UnitName = ExtractSymbolName(element.ProgramName);
            result.InitializationBlock = new BlockOfStatements();
            result.Symbols = new DeclaredSymbolCollection();
            result.FilePath = element.FilePath;
            CurrentUnitMode[result] = UnitMode.Program;
            Project.Add(result, LogSource);
            CurrentUnit = result;
        }

        /// <summary>
        ///     finish a program
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ProgramSymbol element) {
            CurrentUnitMode.Reset(CurrentUnit);
            CurrentUnit = null;
        }

        #endregion
        #region Package

        /// <summary>
        ///     start visiting a package
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(PackageSymbol element) {
            var result = new CompilationUnit();
            InitNode(result, element);
            result.FilePath = element.FilePath;
            result.FileType = CompilationUnitType.Package;
            result.UnitName = ExtractSymbolName(element.PackageName);
            Project.Add(result, LogSource);
            CurrentUnit = result;
        }

        /// <summary>
        ///     finish a package
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(PackageSymbol element)
            => CurrentUnit = null;

        #endregion
        #region UnitInterface

        /// <summary>
        ///     start visiting an unit interface
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(UnitInterfaceSymbol element) {
            CurrentUnitMode[CurrentUnit] = UnitMode.Interface;
            CurrentUnit.Symbols = CurrentUnit.InterfaceSymbols;
            AddToStack(element, CurrentUnit.InterfaceSymbols);
        }

        /// <summary>
        ///     finish an interface
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(UnitInterfaceSymbol element) {
            CurrentUnitMode.Reset(CurrentUnit);
            CurrentUnit.Symbols = null;
        }

        #endregion
        #region UnitImplementation

        /// <summary>
        ///     start visiting an implementation
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(UnitImplementationSymbol element) {
            CurrentUnitMode[CurrentUnit] = UnitMode.Implementation;
            CurrentUnit.Symbols = CurrentUnit.ImplementationSymbols;
            AddToStack(element, CurrentUnit.ImplementationSymbols);
        }

        /// <summary>
        ///     finish the implementation part
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(UnitImplementationSymbol element) {
            CurrentUnit.Symbols = null;
            CurrentUnitMode.Reset(CurrentUnit);
        }


        #endregion
        #region ConstSection

        /// <summary>
        ///     start visiting a const section
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ConstSectionSymbol element) {
            if (element.Kind == TokenKind.Const) {
                CurrentDeclarationMode = DeclarationMode.Const;
            }
            else if (element.Kind == TokenKind.Resourcestring) {
                CurrentDeclarationMode = DeclarationMode.ResourceString;
            }
        }

        /// <summary>
        ///     finish visiting a constant section
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ConstSectionSymbol element)
            => CurrentDeclarationMode = DeclarationMode.Unknown;

        #endregion
        #region TypeSection

        /// <summary>
        ///     start visiting a type section
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(TypeSectionSymbol element)
            => CurrentDeclarationMode = DeclarationMode.Types;

        /// <summary>
        ///     finish visiting a type section
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(TypeSectionSymbol element)
            => CurrentDeclarationMode = DeclarationMode.Unknown;

        #endregion
        #region TypeDeclaration

        /// <summary>
        ///     start visiting a type declaration
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(Standard.TypeDeclarationSymbol element) {
            var symbols = LastValue as IDeclaredSymbolTarget;
            var declaration = new Abstract.TypeDeclaration();
            InitNode(declaration, element);
            declaration.Name = ExtractSymbolName(element.TypeId?.Identifier);
            declaration.Generics = ExtractGenericDefinition(element, element.TypeId?.GenericDefinition);
            ExtractAttributes(element.Attributes as UserAttributesSymbol, CurrentUnit, declaration.Attributes);
            declaration.Hints = ExtractHints(element.Hint as HintingInformationListSymbol);
            symbols.Symbols.Items.Add(new SingleDeclaredSymbol(declaration));
            symbols.Symbols.Add(declaration, LogSource, declaration.Generics?.Count ?? 0);
        }

        /// <summary>
        ///     finish visiting a type declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(Standard.TypeDeclarationSymbol element) {
        }

        #endregion
        #region ConstDeclaration

        /// <summary>
        ///     start visiting a constant declaration
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ConstDeclarationSymbol element) {
            var symbols = LastValue as IDeclaredSymbolTarget;
            var declaration = new ConstantDeclaration();
            InitNode(declaration, element);
            declaration.Name = ExtractSymbolName(element.Identifier);
            declaration.Mode = CurrentDeclarationMode;
            ExtractAttributes(element.Attributes as UserAttributesSymbol, CurrentUnit, declaration.Attributes);
            declaration.Hints = ExtractHints(element.Hint as HintingInformationListSymbol);
            symbols.Symbols.Items.Add(new SingleDeclaredSymbol(declaration));
            symbols.Symbols.Add(declaration, LogSource);
        }

        #endregion

        #region VarSection

        /// <summary>
        ///     finish visit a label declaration
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(LabelDeclarationSection element)
            => CurrentDeclarationMode = DeclarationMode.Label;

        /// <summary>
        ///     end visiting a label
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(LabelDeclarationSection element)
            => CurrentDeclarationMode = DeclarationMode.Unknown;

        #endregion
        #region VarSection

        /// <summary>
        ///     start visiting a variable section
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(VarSection element) {
            if (element.Kind == TokenKind.Var)
                CurrentDeclarationMode = DeclarationMode.Var;
            else if (element.Kind == TokenKind.ThreadVar)
                CurrentDeclarationMode = DeclarationMode.ThreadVar;
            else
                CurrentDeclarationMode = DeclarationMode.Unknown;
        }

        /// <summary>
        ///     end visiting a var section
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(VarSection element)
            => CurrentDeclarationMode = DeclarationMode.Unknown;


        #endregion
        #region VarDeclaration

        /// <summary>
        ///     start visiting a variable declaration
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(VarDeclaration element) {
            var symbols = LastValue as IDeclaredSymbolTarget;
            var declaration = new VariableDeclaration();
            InitNode(declaration, element);
            declaration.Mode = CurrentDeclarationMode;
            declaration.Hints = ExtractHints(element.Hints as HintingInformationListSymbol);

            foreach (var child in element.Identifiers.Items) {
                var ident = child.Identifier;

                if (ident != default) {
                    var name = new VariableName();
                    InitNode(name, child);
                    name.Name = ExtractSymbolName(ident);
                    name.Declaration = declaration;
                    declaration.Names.Add(name);
                    symbols.Symbols.Add(name, LogSource);
                    visitor.WorkingStack.Pop();
                }
            }

            ExtractAttributes(element.Attributes as UserAttributesSymbol, CurrentUnit, declaration.Attributes);
            symbols.Symbols.Items.Add(declaration);
        }

        #endregion
        #region VarValueSpecification

        /// <summary>
        ///     start visiting a variable value specification
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(VarValueSpecification element) {
            var varDeclaration = LastValue as VariableDeclaration;

            if (element.ValueSymbol.GetSymbolKind() == TokenKind.Absolute)
                varDeclaration.ValueKind = VariableValueKind.Absolute;
            else if (element.ValueSymbol.GetSymbolKind() == TokenKind.EqualsSign)
                varDeclaration.ValueKind = VariableValueKind.InitialValue;
        }

        /// <summary>
        ///     end visiting a variable value specification
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(VarValueSpecification element) {
        }

        #endregion
        #region ConstantExpression

        /// <summary>
        ///     start visiting a constant expression
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ConstantExpressionSymbol element) {

            if (element.IsArrayConstant) {
                var lastExpression = LastExpression;
                var result = new ArrayConstant();
                InitNode(result, element);
                lastExpression.Value = result;
            }

            if (element.IsRecordConstant) {
                var lastExpression = LastExpression;
                var result = new RecordConstant();
                InitNode(result, element);
                lastExpression.Value = result;
            }

        }

        #endregion
        #region RecordConstantExpression

        /// <summary>
        ///     start visiting a constant expression
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(RecordConstantExpressionSymbol element) {
            var lastExpression = LastExpression;
            var expression = new RecordConstantItem();
            InitNode(expression, element);
            lastExpression.Value = expression;
            expression.Name = ExtractSymbolName(element.Name);
        }

        #endregion
        #region Expression

        /// <summary>
        ///     start visiting an expression
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ExpressionSymbol element) {
            if (element.LeftOperand != null && element.RightOperand != null) {
                var lastExpression = LastExpression;
                var currentExpression = new BinaryOperator();
                InitNode(currentExpression, element);
                lastExpression.Value = currentExpression;
                currentExpression.Kind = TokenKindMapper.ForExpression(element.Kind);
            }
        }

        #endregion
        #region SimpleExpression

        /// <summary>
        ///     start visiting a simple expression
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(SimpleExpression element) {
            if (element.LeftOperand != null && element.RightOperand != null) {
                var lastExpression = LastExpression;
                var currentExpression = new BinaryOperator();
                InitNode(currentExpression, element);
                lastExpression.Value = currentExpression;
                currentExpression.Kind = TokenKindMapper.ForExpression(element.Kind);
            }
        }

        #endregion
        #region Term

        /// <summary>
        ///     visit a term
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(TermSymbol element) {
            if (element.LeftOperand != null && element.RightOperand != null) {
                var lastExpression = LastExpression;
                var currentExpression = new BinaryOperator();
                InitNode(currentExpression, element);
                lastExpression.Value = currentExpression;
                currentExpression.Kind = TokenKindMapper.ForExpression(element.Kind);
            }
        }

        #endregion
        #region Factor

        /// <summary>
        ///     start visiting a  a factor
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(FactorSymbol element) {

            if (LastExpression == default)
                return;


            // unary operators
            if (element.UnaryOperand != default) {
                var lastExpression = LastExpression;
                var value = new UnaryOperator();
                var kind = element.UnaryOperator.GetSymbolKind();
                InitNode(value, element);
                lastExpression.Value = value;

                if (kind == TokenKind.At)
                    value.Kind = ExpressionKind.AddressOf;
                else if (kind == TokenKind.Not)
                    value.Kind = ExpressionKind.Not;
                else if (kind == TokenKind.Plus)
                    value.Kind = ExpressionKind.UnaryPlus;
                else if (kind == TokenKind.Minus)
                    value.Kind = ExpressionKind.UnaryMinus;
                return;
            }

            // constant values
            if (element.IsNil) {
                var lastExpression = LastExpression;
                var value = new ConstantValue();
                InitNode(value, element);
                value.Kind = ConstantValueKind.Nil;
                lastExpression.Value = value;
                return;
            }

            if (element.PointerTo != null) {
                var lastExpression = LastExpression;
                var value = new SymbolReference();
                InitNode(value, element);
                value.Name = ExtractSymbolName(element.PointerTo);
                value.PointerTo = true;
                lastExpression.Value = value;
                return;
            }

            if (element.IsFalse) {
                var lastExpression = LastExpression;
                var value = new ConstantValue();
                InitNode(value, element);
                value.Kind = ConstantValueKind.False;
                lastExpression.Value = value;
                return;
            }

            if (element.IsTrue) {
                var lastExpression = LastExpression;
                var value = new ConstantValue();
                InitNode(value, element);
                value.Kind = ConstantValueKind.True;
                lastExpression.Value = value;
                return;
            }

            if (element.IntValue != null) {
                var lastExpression = LastExpression;
                var value = new ConstantValue();
                InitNode(value, element);
                value.Kind = ConstantValueKind.IntegralNumber;
                value.TypeInfo = element.IntValue.Value.Token.ParsedValue;
                lastExpression.Value = value;
                return;
            }

            if (element.RealValue != null) {
                var lastExpression = LastExpression;
                var value = new ConstantValue();
                InitNode(value, element);
                value.Kind = ConstantValueKind.RealNumber;
                value.TypeInfo = element.RealValue.Symbol.Token.ParsedValue;
                lastExpression.Value = value;
                return;
            }

            if (element.StringValue != null) {
                var lastExpression = LastExpression;
                var value = new ConstantValue {
                    Kind = ConstantValueKind.QuotedString,
                    TypeInfo = element.StringValue.Symbol.Token.ParsedValue
                };
                lastExpression.Value = value;
                return;
            }

            if (element.HexValue != null) {
                var lastExpression = LastExpression;
                var value = new ConstantValue();
                InitNode(value, element);
                value.Kind = ConstantValueKind.HexNumber;
                value.TypeInfo = element.HexValue.Symbol.Token.ParsedValue;
                lastExpression.Value = value;
                return;
            }
        }

        #endregion
        #region UsesClause

        /// <summary>
        ///     visit a uses clause
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(UsesClauseSymbol element) {
            if (element.UsesList == null)
                return;

            foreach (var part in element.UsesList.Items) {
                if (!(part is NamespaceNameSymbol name))
                    continue;

                var unitName = new RequiredUnitName();
                InitNode(unitName, part);
                unitName.Name = ExtractSymbolName(name);
                unitName.Mode = CurrentUnitMode[CurrentUnit];
                CurrentUnit.RequiredUnits.Add(unitName, LogSource);
                visitor.WorkingStack.Pop();
            }
        }


        #endregion
        #region UsesFileClause

        /// <summary>
        ///     start visiting a uses file clause
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(UsesFileClauseSymbol element) {
            if (element.Files == null)
                return;

            foreach (var part in element.Files.Items) {

                if (!(part is NamespaceFileNameSymbol name))
                    continue;

                var unitName = new RequiredUnitName();
                InitNode(unitName, part);
                unitName.Name = ExtractSymbolName(name);
                unitName.Mode = CurrentUnitMode[CurrentUnit];

                if (name.QuotedFileName != null && name.QuotedFileName.Symbol.Token.ParsedValue is IStringValue fileName)
                    unitName.FileName = fileName.AsUnicodeString;

                CurrentUnit.RequiredUnits.Add(unitName, LogSource);
            }
        }

        #endregion
        #region PackageRequires

        /// <summary>
        ///     visit a package requires list
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(PackageRequiresSymbol element) {
            CurrentUnitMode[CurrentUnit] = UnitMode.Requires;

            if (element.RequiresList == null)
                return;

            foreach (var part in element.RequiresList.Items) {
                if (!(part is NamespaceNameSymbol name))
                    continue;

                var unitName = new RequiredUnitName();
                InitNode(unitName, name);
                unitName.Name = ExtractSymbolName(name);
                unitName.Mode = CurrentUnitMode[CurrentUnit];
                CurrentUnit.RequiredUnits.Add(unitName, LogSource);
            }
        }

        /// <summary>
        ///     finish visiting a requires section
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(PackageRequiresSymbol element)
            => CurrentUnitMode[CurrentUnit] = UnitMode.Interface;

        #endregion
        #region PackageContains

        /// <summary>
        ///     visit a package contains list
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(PackageContainsSymbol element) {
            CurrentUnitMode[CurrentUnit] = UnitMode.Contains;

            if (element.ContainsList == null)
                return;

            foreach (var part in element.ContainsList.Items) {
                if (!(part is NamespaceFileNameSymbol name))
                    continue;

                var unitName = new RequiredUnitName();
                InitNode(unitName, name);
                unitName.Name = ExtractSymbolName(name);
                unitName.Mode = CurrentUnitMode[CurrentUnit];

                if (name?.QuotedFileName?.Symbol?.Token.ParsedValue is IStringValue fileName)
                    unitName.FileName = fileName.AsUnicodeString;

                CurrentUnit.RequiredUnits.Add(unitName, LogSource);
            }
        }

        /// <summary>
        ///     finish visiting a package contains clause
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(PackageContainsSymbol element)
            => CurrentUnitMode.Reset(CurrentUnit);

        #endregion
        #region StructType

        /// <summary>
        ///     visit a structured type
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(StructTypeSymbol element) {
            if (element.Packed)
                CurrentStructTypeMode = StructTypeMode.Packed;
            else
                CurrentStructTypeMode = StructTypeMode.Unpacked;
            ;
        }

        /// <summary>
        ///     finish visiting a structured type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(StructTypeSymbol element)
            => CurrentStructTypeMode = StructTypeMode.Undefined;

        #endregion
        #region ArrayType

        /// <summary>
        ///    visit an array type declaration
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ArrayTypeSymbol element) {
            var target = LastTypeDeclaration;
            var value = new ArrayTypeDeclaration();
            InitNode(value, element);
            value.PackedType = CurrentStructTypeMode == StructTypeMode.Packed;
            target.TypeValue = value;

            if (element.ArrayOfConst) {
                var metaType = new MetaType();
                InitNode(metaType, element);
                metaType.Kind = MetaTypeKind.Const;
                value.TypeValue = metaType;
                visitor.WorkingStack.Pop();
            }
        }

        #endregion
        #region SetDefinition

        /// <summary>
        ///     start visiting a set
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(SetDefinitionSymbol element) {
            var typeTarget = LastTypeDeclaration;
            var value = new SetTypeDeclaration();
            InitNode(value, element);
            value.PackedType = CurrentStructTypeMode == StructTypeMode.Packed;
            typeTarget.TypeValue = value;
        }

        #endregion
        #region FileTypeDefinition

        /// <summary>
        ///     visit a file type
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(FileTypeSymbol element) {
            var typeTarget = LastTypeDeclaration;
            var value = new FileTypeDeclaration();
            InitNode(value, element);
            value.PackedType = CurrentStructTypeMode == StructTypeMode.Packed;
            typeTarget.TypeValue = value;
        }

        #endregion
        #region ClassOf

        /// <summary>
        ///     start visit a class of declaration
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ClassOfDeclarationSymbol element) {
            var typeTarget = LastTypeDeclaration;
            var value = new ClassOfTypeDeclaration();
            InitNode(value, element);
            value.PackedType = CurrentStructTypeMode == StructTypeMode.Packed;
            typeTarget.TypeValue = value;
        }

        #endregion
        #region TypeName

        /// <summary>
        ///     visit a type name
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(TypeNameSymbol element) {
            var typeTarget = LastTypeDeclaration;
            var value = new MetaType();
            InitNode(value, element);
            value.Kind = element.MapTypeKind();
            typeTarget.TypeValue = value;
        }

        /// <summary>
        ///     start visit a child of a type name
        /// </summary>
        /// <param name="element"></param>
        /// <param name="child"></param>
        public void StartVisitChild(TypeNameSymbol element, ISyntaxPart child) {
            var value = LastValue as MetaType;

            if (!(child is GenericNamespaceNameSymbol name) || value == null)
                return;

            foreach (var nspace in name.Name.Namespace) {
                var nameFragment = new GenericNameFragment();
                InitNode(nameFragment, name);
                nameFragment.Name = nspace;
                value.AddFragment(nameFragment);
            }

            var fragment = new GenericNameFragment();
            InitNode(fragment, name);
            fragment.Name = name.Name.Name;
            value.AddFragment(fragment);
        }


        #endregion
        #region SimpleType

        /// <summary>
        ///     visit a simple type
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(SimpleTypeSymbol element) {
            var typeTarget = LastTypeDeclaration;

            if (element.SubrangeStart != null) {
                var subrange = new SubrangeType();
                InitNode(subrange, element);
                typeTarget.TypeValue = subrange;
                return;
            }

            if (element.EnumType != null)
                return;

            var value = new TypeAlias();
            InitNode(value, element);
            value.IsNewType = element.NewType != default;

            if (element.TypeOf != default)
                LogSource.LogWarning(MessageNumbers.UnsupportedTypeOfConstruct, element);

            typeTarget.TypeValue = value;
        }

        /// <summary>
        ///     start visit a child node of a simple type
        /// </summary>
        /// <param name="element"></param>
        /// <param name="child"></param>
        public void StartVisitChild(SimpleTypeSymbol element, ISyntaxPart child) {
            var value = LastValue as TypeAlias;

            if (!(child is GenericNamespaceNameSymbol name) || value == null)
                return;

            foreach (var nspace in name.Name.Namespace) {
                var nameFragment = new GenericNameFragment();
                InitNode(nameFragment, name);
                nameFragment.Name = nspace;
                value.AddFragment(nameFragment);
            }

            var fragment = new GenericNameFragment();
            InitNode(fragment, name);
            fragment.Name = name.Name.Name;
            value.AddFragment(fragment);
        }

        #endregion
        #region EnumTypeDefinition

        /// <summary>
        ///     start visit an enum type definition
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(EnumTypeDefinitionSymbol element) {
            var typeTarget = LastTypeDeclaration;
            var value = new EnumTypeCollection();
            InitNode(value, element);
            typeTarget.TypeValue = value;
        }

        #endregion
        #region EnumValue

        /// <summary>
        ///     visit an enum value
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(EnumValueSymbol element) {
            if (LastValue is EnumTypeCollection enumDeclaration) {
                var value = new EnumTypeValue();
                InitNode(value, element);
                value.Name = ExtractSymbolName(element.EnumName);
                enumDeclaration.Add(value, LogSource);
            }
        }

        #endregion
        #region ArrayIndex

        /// <summary>
        ///     start visit an array index
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ArrayIndexSymbol element) {
            if (element.EndIndex != null) {
                var lastExpression = LastExpression;
                var binOp = new BinaryOperator();
                InitNode(binOp, element);
                lastExpression.Value = binOp;
                binOp.Kind = ExpressionKind.RangeOperator;
            }
        }

        #endregion
        #region PointerType

        /// <summary>
        ///     visit a pointer type
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(PointerTypeSymbol element) {
            var typeTarget = LastTypeDeclaration;

            if (element.GenericPointer) {
                var result = new MetaType();
                InitNode(result, element);
                result.Kind = MetaTypeKind.PointerType;
                typeTarget.TypeValue = result;
            }
            else {
                var result = new PointerToType();
                InitNode(result, element);
                typeTarget.TypeValue = result;
            }
        }

        #endregion
        #region StringType

        /// <summary>
        ///     start visit a string type
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(StringTypeSymbol element) {
            var typeTarget = LastTypeDeclaration;
            var exprTarget = LastExpression;

            if (exprTarget is SymbolReferencePart srp && srp.Kind == SymbolReferencePartKind.StringCast)
                return;

            var result = new MetaType();
            InitNode(result, element);
            result.Kind = TokenKindMapper.ForMetaType(element.Kind);

            if (typeTarget != default)
                typeTarget.TypeValue = result;
            else if (exprTarget != default)
                exprTarget.Value = result;

            if (result.Kind == MetaTypeKind.StringType && element.StringLength != null)
                result.Kind = MetaTypeKind.ShortString;

            if (result.Kind == MetaTypeKind.ShortString && element.StringLength == null)
                result.Kind = MetaTypeKind.ShortStringDefault;

        }

        #endregion
        #region ProcedureTypeDefinition

        /// <summary>
        ///     visit a procedure type definition
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ProcedureTypeDefinitionSymbol element) {

            if (element.AllowAnonymousMethods)
                return;

            var typeTarget = LastTypeDeclaration;
            var result = new ProceduralType();
            InitNode(result, element);
            typeTarget.TypeValue = result;
            result.Kind = TokenKindMapper.MapMethodKind(element.Kind);
            result.AllowAnonymousMethods = element.AllowAnonymousMethods;
            result.MethodDeclaration = element.OfSymbol != default && element.ObjectSymbol != default;

            if (element.ReturnTypeAttributes != null)
                ExtractAttributes(element.ReturnTypeAttributes as UserAttributesSymbol, CurrentUnit, result.ReturnAttributes);
        }

        #endregion
        #region ProcedureReferenceSymbol

        /// <summary>
        ///     visit a procedure reference type definition
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ProcedureReferenceSymbol element) {
            var typeTarget = LastTypeDeclaration;
            var result = new ProceduralType();
            InitNode(result, element);
            typeTarget.TypeValue = result;
            result.Kind = TokenKindMapper.MapMethodKind(element.ProcedureType.Kind);
            result.AllowAnonymousMethods = true;
            result.MethodDeclaration = element.ProcedureType.OfSymbol != default && element.ProcedureType.ObjectSymbol != default;

            if (element.ProcedureType.ReturnTypeAttributes != null)
                ExtractAttributes(element.ProcedureType.ReturnTypeAttributes as UserAttributesSymbol, CurrentUnit, result.ReturnAttributes);
        }

        #endregion
        #region FormalParameterDefinition

        /// <summary>
        ///     start visit a formal parameter declaration
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(FormalParameterDefinitionSymbol element) {
            var parameterTarget = LastValue as IParameterTarget;
            var result = new ParameterTypeDefinition();
            InitNode(result, element);
            parameterTarget.Parameters.Items.Add(result);
            CurrentParameterList[result] = parameterTarget;
        }

        /// <summary>
        ///     end visiting a parameter list
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(FormalParameterDefinitionSymbol element) {
            var paramterTarget = LastValue as ParameterTypeDefinition;
            CurrentParameterList.Reset(paramterTarget);
        }

        #endregion
        #region FormalParameter

        /// <summary>
        ///     visit a formal parameter
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(FormalParameterSymbol element) {
            var typeDefinition = LastValue as ParameterTypeDefinition;
            var result = new ParameterDefinition();
            InitNode(result, element);
            var allParams = CurrentParameterList[typeDefinition];
            result.Name = ExtractSymbolName(element.ParameterName);
            result.ParameterType = typeDefinition;
            ExtractAttributes(element.Attributes1 as UserAttributesSymbol, CurrentUnit, result.Attributes);
            ExtractAttributes(element.Attributes2 as UserAttributesSymbol, CurrentUnit, result.Attributes);
            result.ParameterKind = TokenKindMapper.ForParameterReferenceKind(element.Kind);
            typeDefinition.Parameters.Add(result);
            allParams.Parameters.Add(result, LogSource);
        }

        #endregion
        #region UnitInitialization

        /// <summary>
        ///     start visit an unit initialization block
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(UnitInitializationSymbol element) {
            var result = new BlockOfStatements();
            InitNode(result, element);
            CurrentUnit.InitializationBlock = result;
        }

        #endregion
        #region UnitFinalization

        /// <summary>
        ///     visit a finalization section
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(UnitFinalizationSymbol element) {
            var result = new BlockOfStatements();
            InitNode(result, element);
            CurrentUnit.FinalizationBlock = result;
        }


        #endregion
        #region CompoundStatement

        /// <summary>
        ///     start visiting a compound statement
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(CompoundStatementSymbol element) {
            var lastValue = LastValue;

            if (element.AssemblerBlock != null) {
                var result = new BlockOfAssemblerStatements();
                InitNode(result, element);
                if (lastValue is IStatementTarget statements)
                    statements.Statements.Add(result);
                else if (lastValue is IBlockTarget blockTarget)
                    blockTarget.Block = result;

            }
            else {
                var result = new BlockOfStatements();
                InitNode(result, element);
                if (lastValue is IStatementTarget statements)
                    statements.Statements.Add(result);
                else if (lastValue is IBlockTarget blockTarget)
                    blockTarget.Block = result;
            }
        }

        /// <summary>
        ///     end visiting a compund statement
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(CompoundStatementSymbol element) {
        }


        #endregion
        #region Label

        /// <summary>
        ///     start visiting a label
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(LabelSymbol element) {
            SymbolName name = null;

            if (element.LabelName is IdentifierSymbol standardLabel) {
                name = ExtractSymbolName(standardLabel);
            }

            var intLabel = (element.LabelName as StandardInteger)?.Value?.Token;
            if (intLabel != null) {
                name = new SimpleSymbolName(intLabel.Value.ParsedValue.ToString());
            }

            var hexLabel = (element.LabelName as HexNumberSymbol)?.Symbol?.Token;
            if (hexLabel != null) {
                name = new SimpleSymbolName(hexLabel.Value.ParsedValue.ToString());
            }

            if (name == null)
                return;

            if (CurrentDeclarationMode == DeclarationMode.Label) {
                var symbols = LastValue as IDeclaredSymbolTarget;
                var declaration = new ConstantDeclaration();
                InitNode(declaration, element);
                declaration.Name = name;
                declaration.Mode = CurrentDeclarationMode;
                symbols.Symbols.Items.Add(new SingleDeclaredSymbol(declaration));
                symbols.Symbols.Add(declaration, LogSource);
            }

            if (!(LastValue is ILabelTarget parent))
                return;

            parent.LabelName = name;
        }

        /// <summary>
        ///     end visiting a label
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(LabelSymbol element) {
        }


        #endregion
        #region ClassDeclaration

        /// <summary>
        ///     start visiting a class declaration
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ClassDeclarationSymbol element) {
            var typeTarget = LastTypeDeclaration;
            var result = new StructuredType();
            InitNode(result, element);
            result.Kind = StructuredTypeKind.Class;
            result.SealedClass = element.SealedSymbol != default;
            result.AbstractClass = element.AbstractSymbol != default;
            result.ForwardDeclaration = element.ForwardDeclaration;
            typeTarget.TypeValue = result;
            CurrentMemberVisibility[result] = new MemberStatus() {
                Visibility = MemberVisibility.Public,
                ClassItem = false
            };
        }

        /// <summary>
        ///     end visiting a class declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ClassDeclarationSymbol element) {
            var parentType = LastValue as StructuredType;
            CurrentMemberVisibility.Reset(parentType);
        }

        #endregion
        #region ClassDeclarationItem

        /// <summary>
        ///     start visiting a class declaration item
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ClassDeclarationItemSymbol element) {

            if (!(LastValue is StructuredType parentType))
                return;

            if (element.Visibility != TokenKind.Undefined) {
                CurrentMemberVisibility[parentType].Visibility = TokenKindMapper.ForVisibility(element.Visibility, element.Strict);
            }

            CurrentMemberVisibility[parentType].ClassItem = element.ClassItem;
            ExtractAttributes(element.Attributes1, CurrentUnit, CurrentMemberVisibility[parentType].Attributes);
            ExtractAttributes(element.Attributes2, CurrentUnit, CurrentMemberVisibility[parentType].Attributes);
        }

        #endregion
        #region ClassField

        /// <summary>
        ///     start visiting a class field
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ClassFieldSymbol element) {
            var structType = LastValue as StructuredType;
            var result = new StructureFields();
            InitNode(result, element);
            result.Visibility = CurrentMemberVisibility[structType].Visibility;
            result.ClassItem = CurrentMemberVisibility[structType].ClassItem;
            structType.Fields.Items.Add(result);

            var extractedAttributes = CurrentMemberVisibility[structType].Attributes;

            foreach (var part in element.Names.Items) {

                var attrs = part.Attributes;

                if (attrs != null) {
                    ExtractAttributes(attrs, CurrentUnit, extractedAttributes);
                }

                var partName = part.Identifier;
                if (partName == null)
                    continue;

                var fieldName = new StructureField();
                InitNode(fieldName, part);
                fieldName.Name = ExtractSymbolName(partName);
                fieldName.Attributes.AddRange(extractedAttributes);
                structType.Fields.Add(fieldName, LogSource);
                result.Fields.Add(fieldName);
                extractedAttributes = new List<SymbolAttributeItem>();
                visitor.WorkingStack.Pop();
            }

            result.Hints = ExtractHints(element.Hint as HintingInformationListSymbol);
        }

        #endregion
        #region ClassProperty

        /// <summary>
        ///     start visiting a property declaration
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ClassPropertySymbol element) {
            var parent = LastValue as StructuredType;
            var result = new StructureProperty();
            InitNode(result, element);
            result.Name = ExtractSymbolName(element.PropertyName);
            parent.Properties.Add(result, LogSource);
            result.Visibility = CurrentMemberVisibility[parent].Visibility;
            result.Attributes.AddRange(CurrentMemberVisibility[parent].Attributes);
        }

        #endregion
        #region ClassPropertyReadWrite

        /// <summary>
        ///     start visiting a class propertery/read definition
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ClassPropertyReadWriteSymbol element) {
            var parent = LastValue as StructureProperty;
            var result = new StructurePropertyAccessor();
            InitNode(result, element);
            result.Kind = TokenKindMapper.ForPropertyAccessor(element.Kind);
            result.Name = ExtractSymbolName(element.Member);
            parent.Accessors.Add(result);

        }

        #endregion
        #region  ClassPropertyDispInterface

        /// <summary>
        ///     start visiting a class property
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ClassPropertyDispInterfaceSymbols element) {
            var parent = LastValue as StructureProperty;
            var result = new StructurePropertyAccessor();
            InitNode(result, element);

            if (element.ReadOnly) {
                result.Kind = StructurePropertyAccessorKind.ReadOnly;
            }
            else if (element.WriteOnly) {
                result.Kind = StructurePropertyAccessorKind.WriteOnly;
            }
            else if (element.DispId != null) {
                result.Kind = StructurePropertyAccessorKind.DispId;
            }
            else {
                result.Kind = StructurePropertyAccessorKind.Undefined;
            }

            parent.Accessors.Add(result);

        }

        #endregion
        #region ParseClassPropertyAccessSpecifier

        /// <summary>
        ///     start visiting a class property specifier
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ClassPropertySpecifierSymbol element) {
            if (element.PropertyReadWrite != null)
                return;

            if (element.PropertyDispInterface != null)
                return;

            var parent = LastValue as StructureProperty;
            var result = new StructurePropertyAccessor();
            InitNode(result, element);

            if (element.IsStored) {
                result.Kind = StructurePropertyAccessorKind.Stored;
            }
            else if (element.IsDefault) {
                result.Kind = StructurePropertyAccessorKind.Default;
            }
            else if (element.NoDefault) {
                result.Kind = StructurePropertyAccessorKind.NoDefault;
            }
            else if (element.ImplementsTypeId != null) {
                result.Kind = StructurePropertyAccessorKind.Implements;
                result.Name = ExtractSymbolName(element.ImplementsTypeId as NamespaceNameSymbol);
            }

            parent.Accessors.Add(result);

        }

        #endregion
        #region ClassMethod

        /// <summary>
        ///     start visiting a class method
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ClassMethodSymbol element) {
            var parent = LastValue as StructuredType;
            var result = new StructureMethod();
            InitNode(result, element);
            result.Visibility = CurrentMemberVisibility[parent].Visibility;
            result.ClassItem = CurrentMemberVisibility[parent].ClassItem;
            result.Attributes.AddRange(CurrentMemberVisibility[parent].Attributes);
            result.Name = ExtractSymbolName(element.Identifier);
            result.Kind = TokenKindMapper.MapMethodKind(element.MethodKind);
            result.Generics = ExtractGenericDefinition(element, element.GenericDefinition as GenericDefinitionSymbol);
            result.DefiningType = parent;
            parent.Methods.Add(result, LogSource);
        }

        #endregion
        #region MethodResolution

        /// <summary>
        ///     start visiting a method resolution
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(MethodResolutionSymbol element) {
            var parent = LastValue as StructuredType;
            var result = new StructureMethodResolution();
            InitNode(result, element);
            result.Attributes.AddRange(CurrentMemberVisibility[parent].Attributes);
            result.Kind = TokenKindMapper.ForMethodResolutionKind(element.Kind);
            result.Target = ExtractSymbolName(element.ResolveIdentifier);
            parent.MethodResolutions.Add(result);

        }

        #endregion
        #region ReintroduceDirective

        /// <summary>
        ///     visit a method directive
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ReintroduceSymbol element) {
            var parent = LastValue as IDirectiveTarget;
            var result = new MethodDirective();
            InitNode(result, element);
            result.Kind = MethodDirectiveKind.Reintroduce;
            parent.Directives.Add(result);

        }

        #endregion
        #region OverloadDirective

        /// <summary>
        ///     start visiting an overload directive
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(OverloadSymbol element) {
            var parent = LastValue as IDirectiveTarget;
            var result = new MethodDirective();
            InitNode(result, element);
            result.Kind = MethodDirectiveKind.Overload;
            parent.Directives.Add(result);

        }

        #endregion
        #region DispIdDirective

        /// <summary>
        ///     start visiting a dispid directive
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(DispIdSymbol element) {

            if (!(LastValue is IDirectiveTarget parent))
                return;

            var result = new MethodDirective();
            InitNode(result, element);
            result.Kind = MethodDirectiveKind.DispId;
            parent.Directives.Add(result);

        }

        #endregion
        #region InlineDirective

        /// <summary>
        ///     start visiting a directive
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(InlineSymbol element) {
            var parent = LastValue as IDirectiveTarget;
            var result = new MethodDirective();
            InitNode(result, element);

            if (element.Kind == TokenKind.Inline) {
                result.Kind = MethodDirectiveKind.Inline;
            }
            else if (element.Kind == TokenKind.Assembler) {
                result.Kind = MethodDirectiveKind.Assembler;
            }

            parent.Directives.Add(result);

        }

        #endregion
        #region AbstractDirective

        /// <summary>
        ///     start visit an abstract directive
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(AbstractSymbol element) {
            var parent = LastValue as IDirectiveTarget;
            var result = new MethodDirective();
            InitNode(result, element);

            if (element.Kind == TokenKind.Abstract) {
                result.Kind = MethodDirectiveKind.Abstract;
            }
            else if (element.Kind == TokenKind.Final) {
                result.Kind = MethodDirectiveKind.Final;
            }

            parent.Directives.Add(result);

        }

        #endregion
        #region OldCallConvention

        /// <summary>
        ///     start visiting a historic directive
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(OldCallConventionSymbol element) {
            var parent = LastValue as IDirectiveTarget;
            var result = new MethodDirective();
            InitNode(result, element);

            if (element.Kind == TokenKind.Far) {
                result.Kind = MethodDirectiveKind.Far;
            }

            else if (element.Kind == TokenKind.Local) {
                result.Kind = MethodDirectiveKind.Local;
            }

            else if (element.Kind == TokenKind.Near) {
                result.Kind = MethodDirectiveKind.Near;
            }

            parent.Directives.Add(result);

        }

        #endregion
        #region ExternalDirective

        /// <summary>
        ///     start visit an external directive
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ExternalDirectiveSymbol element) {
            var parent = LastValue as IDirectiveTarget;
            var result = new MethodDirective();
            InitNode(result, element);

            if (element.Kind == TokenKind.VarArgs) {
                result.Kind = MethodDirectiveKind.VarArgs;
            }

            else if (element.Kind == TokenKind.External) {
                result.Kind = MethodDirectiveKind.External;
            }

            parent.Directives.Add(result);

        }

        #endregion
        #region ExternalSpecifier

        /// <summary>
        ///     start visiting a external specifier
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ExternalSpecifierSymbol element) {
            var parent = LastValue as MethodDirective;
            var result = new MethodDirectiveSpecifier();
            InitNode(result, element);
            result.Kind = TokenKindMapper.ForMethodDirective(element.Kind);
            parent.Specifiers.Add(result);

        }

        #endregion
        #region CallConvention

        /// <summary>
        ///     start visit a calling convention directive
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(CallConventionSymbol element) {
            var parent = LastValue as IDirectiveTarget;
            var result = new MethodDirective();
            InitNode(result, element);

            if (element.Kind == TokenKind.Cdecl) {
                result.Kind = MethodDirectiveKind.Cdecl;
            }
            else if (element.Kind == TokenKind.Pascal) {
                result.Kind = MethodDirectiveKind.Pascal;
            }
            else if (element.Kind == TokenKind.Register) {
                result.Kind = MethodDirectiveKind.Register;
            }
            else if (element.Kind == TokenKind.Safecall) {
                result.Kind = MethodDirectiveKind.Safecall;
            }
            else if (element.Kind == TokenKind.Stdcall) {
                result.Kind = MethodDirectiveKind.StdCall;
            }
            else if (element.Kind == TokenKind.Export) {
                result.Kind = MethodDirectiveKind.Export;
            }

            parent.Directives.Add(result);

        }

        #endregion
        #region BindingDirective

        /// <summary>
        ///     start visiting a binding directive
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(BindingSymbol element) {
            var parent = LastValue as IDirectiveTarget;
            var result = new MethodDirective();
            InitNode(result, element);

            if (element.Kind == TokenKind.Static) {
                result.Kind = MethodDirectiveKind.Static;
            }
            else if (element.Kind == TokenKind.Dynamic) {
                result.Kind = MethodDirectiveKind.Dynamic;
            }
            else if (element.Kind == TokenKind.Virtual) {
                result.Kind = MethodDirectiveKind.Virtual;
            }
            else if (element.Kind == TokenKind.Override) {
                result.Kind = MethodDirectiveKind.Override;
            }
            else if (element.Kind == TokenKind.Message) {
                result.Kind = MethodDirectiveKind.Message;
            }

            parent.Directives.Add(result);

        }

        #endregion
        #region MethodDirectives

        /// <summary>
        ///     start visiting method directives
        /// </summary>
        /// <param name="element"></param>
        /// <param name="child"></param>
        public void StartVisitChild(MethodDirectivesSymbol element, ISyntaxPart child) {
            var lastValue = LastValue as IDirectiveTarget;

            if (child is HintSymbol hints && lastValue != null) {
                lastValue.Hints = ExtractHints(hints, lastValue.Hints);
            }
        }

        #endregion
        #region FunctionDirectives

        /// <summary>
        ///     start visiting a function directive
        /// </summary>
        /// <param name="element"></param>
        /// <param name="child"></param>
        public void StartVisitChild(FunctionDirectivesSymbol element, ISyntaxPart child) {
            var lastValue = LastValue as IDirectiveTarget;

            if (child is HintSymbol hints && lastValue != null) {
                lastValue.Hints = ExtractHints(hints, lastValue.Hints);
            }
        }

        #endregion
        #region ExportedProcedureHeading

        /// <summary>
        ///     start visiting exported procedure headings
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ExportedProcedureHeadingSymbol element) {
            var symbols = LastValue as IDeclaredSymbolTarget;
            var result = new GlobalMethod();
            var anchor = new SingleDeclaredSymbol(result);
            InitNode(result, element);
            result.Name = ExtractSymbolName(element.Name);
            result.Kind = TokenKindMapper.MapMethodKind(element.Kind);
            result.Anchor = anchor;

            ExtractAttributes(element.Attributes, CurrentUnit, result.Attributes);
            ExtractAttributes(element.ResultAttributes as UserAttributesSymbol, CurrentUnit, result.ReturnAttributes);

            symbols.Symbols.Add(result, LogSource);
            if (result.Anchor != default) {
                symbols.Symbols.Items.Add(result.Anchor);
            }
            else {
                anchor.Symbol = default;
            }
        }

        #endregion
        #region UnsafeDirective

        /// <summary>
        ///     start visiting an unsafe directive
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(UnsafeDirectiveSymbol element) {
            var parent = LastValue as IDirectiveTarget;
            var result = new MethodDirective();
            InitNode(result, element);
            result.Kind = MethodDirectiveKind.Unsafe;
            parent.Directives.Add(result);

        }

        #endregion
        #region ForwardDirective

        /// <summary>
        ///     start visiting a forward directive
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ForwardDirectiveSymbol element) {
            var parent = LastValue as IDirectiveTarget;
            var result = new MethodDirective();
            InitNode(result, element);
            result.Kind = MethodDirectiveKind.Forward;
            parent.Directives.Add(result);

        }

        #endregion
        #region ExportsSection

        /// <summary>
        ///     start visiting an exports section
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ExportsSectionSymbol element) {
            CurrentDeclarationMode = DeclarationMode.Exports;
            ;
        }

        private AbstractSyntaxPartBase EndVisitItem(ExportsSectionSymbol exportsSection) {
            CurrentDeclarationMode = DeclarationMode.Unknown;
            return null;
        }


        #endregion
        #region ExportItem

        /// <summary>
        ///     start visiting an export section
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ExportItemSymbol element) {
            var declarations = LastValue as IDeclaredSymbolTarget;
            var result = new ExportedMethodDeclaration();
            var anchor = new SingleDeclaredSymbol(result);
            InitNode(result, element);
            result.Name = ExtractSymbolName(element.ExportName);
            result.IsResident = element.Resident.GetSymbolKind() == TokenKind.Resident;
            result.HasIndex = element.IndexParameter != null;
            result.HasName = element.NameParameter != null;
            result.Anchor = anchor;

            declarations.Symbols.Add(result, LogSource);
            if (result.Anchor != default) {
                declarations.Symbols.Items.Add(result.Anchor);
            }
            else {
                anchor.Symbol = default;
            }
        }

        #endregion
        #region RecordItem

        /// <summary>
        ///     start visiting a record declaration item
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(RecordItemSymbol element) {

            if (!(LastValue is StructuredType parentType))
                return;

            if (element.Visibility != TokenKind.Undefined) {
                CurrentMemberVisibility[parentType].Visibility = TokenKindMapper.ForVisibility(element.Visibility, element.Strict);
            };

            ExtractAttributes(element.Attributes1, CurrentUnit, CurrentMemberVisibility[parentType].Attributes);
            ExtractAttributes(element.Attributes2, CurrentUnit, CurrentMemberVisibility[parentType].Attributes);
        }

        #endregion
        #region RecordDeclaration

        /// <summary>
        ///     start visiting a record declaration
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(RecordDeclarationSymbol element) {
            var typeTarget = LastTypeDeclaration;
            var result = new StructuredType();
            InitNode(result, element);
            result.Kind = StructuredTypeKind.Record;
            typeTarget.TypeValue = result;
            CurrentMemberVisibility[result] = new MemberStatus() {
                Visibility = MemberVisibility.Public,
                ClassItem = false
            };
        }

        /// <summary>
        ///     end visiting a record declaration
        /// </summary>
        /// <param name="recordDeclaration"></param>
        public void EndVisit(RecordDeclarationSymbol recordDeclaration) {
            var parentType = LastValue as StructuredType;
            CurrentMemberVisibility.Reset(parentType);
        }

        #endregion
        #region RecordField

        /// <summary>
        ///     start visiting a field declaration
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(RecordFieldSymbol element) {
            StructuredType structType = null;
            StructureVariantCollection varFields = null;
            IList<StructureFields> fields = null;

            if (LastValue is StructureVariantFields) {
                structType = LastTypeDeclaration.TypeValue as StructuredType;
                varFields = structType.Variants;
                fields = (LastValue as StructureVariantFields)?.Fields;
            }
            else {
                structType = LastValue as StructuredType;
                fields = structType.Fields.Items;
            }

            var result = new StructureFields();
            InitNode(result, element);
            result.Visibility = CurrentMemberVisibility[structType].Visibility;

            if (fields != null)
                fields.Add(result);

            var extractedAttributes = new List<SymbolAttributeItem>();
            extractedAttributes.AddRange(CurrentMemberVisibility[structType].Attributes);

            foreach (var part in element.Names.Items) {

                var attrs = part.Attributes;

                if (attrs != null) {
                    ExtractAttributes(attrs, CurrentUnit, extractedAttributes);
                }

                var partName = part.Identifier;
                if (partName == null)
                    continue;

                var fieldName = new StructureField();
                InitNode(fieldName, partName);
                fieldName.Name = ExtractSymbolName(partName);
                fieldName.Attributes.AddRange(extractedAttributes);

                if (varFields == null)
                    structType.Fields.Add(fieldName, LogSource);
                else
                    varFields.Add(fieldName, LogSource);

                result.Fields.Add(fieldName);
                extractedAttributes = null;
                visitor.WorkingStack.Pop();

            }

            result.Hints = ExtractHints(element.Hint as HintingInformationListSymbol);

        }


        #endregion
        #region ParseRecordVariantSection

        /// <summary>
        ///     start visiting a variant section of a record definition
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(RecordVariantSectionSymbol element) {
            var structType = LastValue as StructuredType;
            var result = new StructureVariantItem();
            InitNode(result, element);
            result.Name = ExtractSymbolName(element.Name);
            structType.Variants.Items.Add(result);

        }

        #endregion
        #region RecordVariant

        /// <summary>
        ///     start visiting a record variant
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(RecordVariantSymbol element) {
            var structType = LastValue as StructureVariantItem;
            var result = new StructureVariantFields();
            InitNode(result, element);
            structType.Items.Add(result);

        }

        #endregion
        #region RecordHelperDefinition

        /// <summary>
        ///     start visiting a record helper definition
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(RecordHelperDefinitionSymbol element) {
            var typeTarget = LastTypeDeclaration;
            var result = new StructuredType();
            InitNode(result, element);
            result.Kind = StructuredTypeKind.RecordHelper;
            typeTarget.TypeValue = result;
            CurrentMemberVisibility[result] = new MemberStatus() {
                Visibility = MemberVisibility.Public,
                ClassItem = false,
            };
        }

        /// <summary>
        ///     end visiting a record helper declaration
        /// </summary>
        /// <param name="classDeclaration"></param>
        public void EndVisit(RecordHelperDefinitionSymbol classDeclaration) {
            var parentType = LastValue as StructuredType;
            CurrentMemberVisibility.Reset(parentType);
        }

        #endregion
        #region RecordHelperItem

        /// <summary>
        ///     start visiting a record declaration item
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(RecordHelperItemSymbol element) {

            if (!(LastValue is StructuredType parentType))
                return;

            if (element.Visibility != TokenKind.Undefined) {
                CurrentMemberVisibility[parentType].Visibility = TokenKindMapper.ForVisibility(element.Visibility, element.Strict);
            };
        }

        #endregion
        #region ObjectDeclaration

        /// <summary>
        ///     start visiting an object declaration
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ObjectDeclarationSymbol element) {
            var typeTarget = LastTypeDeclaration;
            var result = new StructuredType();
            InitNode(result, element);
            result.Kind = StructuredTypeKind.ObjectType;
            typeTarget.TypeValue = result;
            CurrentMemberVisibility[result] = new MemberStatus() {
                Visibility = MemberVisibility.Public,
                ClassItem = false,
            };
        }

        /// <summary>
        ///     end visiting an object declaration
        /// </summary>
        /// <param name="objectDeclaration"></param>
        public void EndVisit(ObjectDeclarationSymbol objectDeclaration) {
            var parentType = LastValue as StructuredType;
            CurrentMemberVisibility.Reset(parentType);
        }


        #endregion
        #region ObjectItem

        /// <summary>
        ///     start visiting an object item
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ObjectItem element) {

            if (!(LastValue is StructuredType parentType))
                return;

            if (element.Visibility.GetSymbolKind() != TokenKind.Undefined) {
                CurrentMemberVisibility[parentType].Visibility = TokenKindMapper.ForVisibility(element.Visibility.GetSymbolKind(), element.Strict.GetSymbolKind() == TokenKind.Strict);
            };
        }

        #endregion
        #region InterfaceDefinition

        /// <summary>
        ///     end visiting an interface declaration
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(InterfaceDefinitionSymbol element) {
            var typeTarget = LastTypeDeclaration;
            var result = new StructuredType();
            InitNode(result, element);
            if (element.DisplayInterface)
                result.Kind = StructuredTypeKind.DispInterface;
            else
                result.Kind = StructuredTypeKind.Interface;

            result.ForwardDeclaration = element.ForwardDeclaration;
            typeTarget.TypeValue = result;
            CurrentMemberVisibility[result] = new MemberStatus() {
                Visibility = MemberVisibility.Public,
                ClassItem = false,
            };
        }

        /// <summary>
        ///     end visiting an interface declaration
        /// </summary>
        /// <param name="interfaceDeclaration"></param>
        public void EndVisit(InterfaceDefinitionSymbol interfaceDeclaration) {
            var parentType = LastValue as StructuredType;
            CurrentMemberVisibility.Reset(parentType);
        }

        #endregion
        #region InterfaceGuid

        /// <summary>
        ///     start visiting an interface guid definiton
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(InterfaceGuidSymbol element) {
            var structType = LastValue as StructuredType;

            if (element.IdIdentifier != null) {
                structType.GuidName = ExtractSymbolName(element.IdIdentifier);
            }
            else if (element.Id != null) {
                structType.GuidId = element.Id.Symbol.Token.ParsedValue;
            }


            ;
        }

        #endregion
        #region ClassHelper

        /// <summary>
        ///     start visiting a class helper definition
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ClassHelperDefSymbol element) {
            var typeTarget = LastTypeDeclaration;
            var result = new StructuredType();
            InitNode(result, element);
            result.Kind = StructuredTypeKind.ClassHelper;
            typeTarget.TypeValue = result;
            CurrentMemberVisibility[result] = new MemberStatus() {
                Visibility = MemberVisibility.Public,
                ClassItem = false,
            };
        }

        /// <summary>
        ///     end visiting a class helper definition
        /// </summary>
        /// <param name="classHelper"></param>
        public void EndVisit(ClassHelperDefSymbol classHelper) {
            var parentType = LastValue as StructuredType;
            CurrentMemberVisibility.Reset(parentType);
        }


        #endregion
        #region ClassHelperItem

        /// <summary>
        ///     start visiting a class helper item
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ClassHelperItemSymbol element) {

            if (!(LastValue is StructuredType parentType))
                return;

            if (element.Visibility != TokenKind.Undefined) {
                CurrentMemberVisibility[parentType].Visibility = TokenKindMapper.ForVisibility(element.Visibility, element.Strict);
            };
        }

        #endregion
        #region ProcedureDeclaration

        /// <summary>
        ///     start visiting a procedure declaration
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ProcedureDeclarationSymbol element) {
            var symbolTarget = LastValue as IDeclaredSymbolTarget;
            var result = new MethodImplementation();
            var anchor = new SingleDeclaredSymbol(result);
            InitNode(result, element);
            result.Name = ExtractSymbolName(element.Heading.Name);
            result.Kind = TokenKindMapper.MapMethodKind(element.Heading.Kind);
            result.Flags = MethodImplementationFlags.None;
            result.Anchor = anchor;

            if (element.Directives != default && element.Directives.IsForwardDeclaration)
                result.Flags |= MethodImplementationFlags.ForwardDeclaration;

            symbolTarget.Symbols.Add(result, LogSource);
            if (result.Anchor != default) {
                symbolTarget.Symbols.Items.Add(result.Anchor);
            }
            else {
                anchor.Symbol = default;
            }

            DefineResultVariables(result);
        }

        private void DefineResultVariables(MethodImplementation result) {
            if (result.Kind == RoutineKind.Function) {
                var declaration = new VariableDeclaration();
                var functionResult = new FunctionResult() {
                    Name = new SimpleSymbolName("Result"),
                    Declaration = declaration,
                    Method = result
                };
                var functionName = new FunctionResult() {
                    Name = new SimpleSymbolName(result.Name.Name),
                    Declaration = declaration,
                    Method = result
                };
                declaration.Names.Add(functionResult);
                declaration.Names.Add(functionName);
                result.Symbols.Add(functionResult, LogSource);
                result.Symbols.Items.Add(declaration);
            }
        }

        #endregion
        #region MethodDeclaration

        /// <summary>
        ///     start visiting a method declaration
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(MethodDeclarationSymbol element) {
            var unit = CurrentUnit;
            var name = ExtractSymbolName(element.Heading.Items);
            var symbolTarget = LastValue as IDeclaredSymbolTarget;
            var result = new MethodImplementation();
            InitNode(result, element);
            result.Kind = TokenKindMapper.MapMethodKind(element.Heading.Kind);
            result.Name = name;
            result.DefaultParameters = element.Heading.Parameters == default;

            var type = default(DeclaredSymbol);

            if (unit.InterfaceSymbols != default)
                type = unit.InterfaceSymbols.Find(name.NamespaceParts);

            if (type == default && unit.ImplementationSymbols != default)
                type = unit.ImplementationSymbols.Find(name.NamespaceParts);

            if (type == default && unit.Symbols != default)
                type = unit.Symbols.Find(name.NamespaceParts);

            if (type != default) {
                var typeDecl = type as Abstract.TypeDeclaration;
                if (typeDecl.TypeValue is StructuredType typeStruct && typeStruct.Methods.Contains(name.Name)) {
                    var declaration = typeStruct.Methods[name.Name];
                    declaration.Implementation = result;
                    result.Declaration = declaration;
                }
            }

            DefineResultVariables(result);
            symbolTarget.Symbols.Items.Add(new SingleDeclaredSymbol(result));
            symbolTarget.Symbols.Add(result, LogSource);
        }

        #endregion
        #region StatementPart

        /// <summary>
        ///     start visiting a statement part
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(StatementPart element) {

            if (element.DesignatorPart == null && element.Assignment == null)
                return;

            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, element);

            if (element.Assignment != null) {
                result.Kind = StructuredStatementKind.Assignment;
            }
            else {
                result.Kind = StructuredStatementKind.ExpressionStatement;
            }

            target.Statements.Add(result);

        }

        #endregion
        #region ClosureExpression

        /// <summary>
        ///     start visiting a closure expression
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ClosureExpressionSymbol element) {
            var expression = LastExpression;
            var result = new MethodImplementation();
            InitNode(result, element);
            result.Name = new SimpleSymbolName(CurrentUnit.GenerateSymbolName());
            expression.Value = result;
        }

        #endregion
        #region RaiseStatement

        /// <summary>
        ///     start visiting a raise statement
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(RaiseStatementSymbol element) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, element);
            target.Statements.Add(result);

            if (element.RaiseExpression != null && element.AtExpression == null) {
                result.Kind = StructuredStatementKind.Raise;
            }
            else if (element.RaiseExpression != null && element.AtExpression != null) {
                result.Kind = StructuredStatementKind.RaiseAt;
            }
            else if (element.RaiseExpression == null && element.AtExpression != null) {
                result.Kind = StructuredStatementKind.RaiseAtOnly;
            }

        }

        #endregion
        #region TryStatement

        /// <summary>
        ///     start visiting a try statement
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(TryStatementSymbol element) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, element);
            target.Statements.Add(result);

            if (element.Finally != null) {
                result.Kind = StructuredStatementKind.TryFinally;
            }
            else if (element.Handlers != null) {
                result.Kind = StructuredStatementKind.TryExcept;
            }


        }

        /// <summary>
        ///     start visiting a try statement child
        /// </summary>
        /// <param name="element"></param>
        /// <param name="child"></param>
        public void StartVisitChild(TryStatementSymbol element, ISyntaxPart child) {

            if (!(child is StatementList statements))
                return;

            var target = LastValue as IStatementTarget;
            var result = new BlockOfStatements();
            InitNode(result, child);
            target.Statements.Add(result);
        }

        #endregion
        #region ExceptHandlers

        /// <summary>
        ///     start visiting exception handlers
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ExceptHandlersSymbol element) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, element);
            result.Kind = StructuredStatementKind.ExceptElse;
            target.Statements.Add(result);

        }

        #endregion
        #region ExceptHandler

        /// <summary>
        ///     start visiting an exception handler
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ExceptHandlerSymbol element) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, element);
            result.Kind = StructuredStatementKind.ExceptOn;
            result.Name = ExtractSymbolName(element.Name);
            target.Statements.Add(result);

        }

        #endregion
        #region WithStatement

        /// <summary>
        ///     start visiting a with statement
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(WithStatementSymbol element) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, element);
            result.Kind = StructuredStatementKind.With;
            target.Statements.Add(result);

        }

        #endregion
        #region ForStatement

        /// <summary>
        ///     start visiting a for statement
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ForStatementSymbol element) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, element);

            switch (element.Kind) {
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

            result.Name = ExtractSymbolName(element.Variable);
            target.Statements.Add(result);

        }

        #endregion
        #region WhileStatement

        /// <summary>
        ///     start a with statement
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(WhileStatementSymbol element) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, element);
            result.Kind = StructuredStatementKind.While;
            target.Statements.Add(result);

        }

        #endregion
        #region RepeatStatement

        /// <summary>
        ///     start visiting a repeat statement
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(RepeatStatement element) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, element);
            result.Kind = StructuredStatementKind.Repeat;
            target.Statements.Add(result);

        }

        #endregion
        #region CaseStatement

        /// <summary>
        ///     start visiting a case statement
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(CaseStatementSymbol element) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, element);
            result.Kind = StructuredStatementKind.Case;
            target.Statements.Add(result);

        }

        /// <summary>
        ///     start visiting a case statement child
        /// </summary>
        /// <param name="element"></param>
        /// <param name="child"></param>
        public void StartVisitChild(CaseStatementSymbol element, ISyntaxPart child) {

            if (element.Else != child)
                return;

            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, child);
            result.Kind = StructuredStatementKind.CaseElse;
            target.Statements.Add(result);
        }

        #endregion
        #region CaseItem

        /// <summary>
        ///     start visiting a case item
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(CaseItemSymbol element) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, element);
            result.Kind = StructuredStatementKind.CaseItem;
            target.Statements.Add(result);

        }

        #endregion
        #region CaseLabel

        /// <summary>
        ///     start visiting a case label
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(CaseLabelSymbol element) {
            if (element.EndExpression != null) {
                var lastExpression = LastExpression;
                var binOp = new BinaryOperator();
                InitNode(binOp, element);
                lastExpression.Value = binOp;
                binOp.Kind = ExpressionKind.RangeOperator;
            }
        }

        #endregion
        #region IfStatement

        /// <summary>
        ///     start visiting an if statement
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(IfStatementSymbol element) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, element);
            result.Kind = StructuredStatementKind.IfThen;
            target.Statements.Add(result);

        }

        /// <summary>
        ///     start visiting an if statement child
        /// </summary>
        /// <param name="element"></param>
        /// <param name="child"></param>
        public void StartVisitChild(IfStatementSymbol element, ISyntaxPart child) {

            if (element.ElsePart != child)
                return;

            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, child);
            result.Kind = StructuredStatementKind.IfElse;
            target.Statements.Add(result);
        }

        #endregion
        #region GoToStatement

        /// <summary>
        ///     start visiting a gotot statement
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(GoToStatementSymbol element) {
            var target = LastValue as IStatementTarget;
            var result = new StructuredStatement();
            InitNode(result, element);

            if (element.Break)
                result.Kind = StructuredStatementKind.Break;
            else if (element.Continue)
                result.Kind = StructuredStatementKind.Continue;
            else if (element.GoToLabel != null)
                result.Kind = StructuredStatementKind.GoToLabel;
            else if (element.Exit)
                result.Kind = StructuredStatementKind.Exit;

            target.Statements.Add(result);

        }

        #endregion
        #region AsmBlock

        /// <summary>
        ///     start visiting an assembler block
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(AsmBlockSymbol element) {
            var blockTarget = LastValue as IBlockTarget;
            var result = new BlockOfAssemblerStatements();
            InitNode(result, element);
            if (blockTarget is IStatementTarget statementTarget)
                statementTarget.Statements.Add(result);
            else if (blockTarget != null)
                blockTarget.Block = result;

        }

        #endregion
        #region AsmPseudoOp

        /// <summary>
        ///     start visiting an pseudo operator
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(AsmPseudoOpSymbol element) {
            var statementTarget = LastValue as BlockOfAssemblerStatements;
            var result = new AssemblerStatement();
            InitNode(result, element);

            if (element.Mode == AsmPrefixSymbolKind.ParamsOperation) {
                result.Kind = AssemblerStatementKind.ParamsOperation;
                var operand = new ConstantValue();
                InitNode(operand, element.NumberOfParams);
                operand.Kind = ConstantValueKind.IntegralNumber;
                operand.TypeInfo = element.NumberOfParams.Value.Token.ParsedValue;
                result.Operands.Add(operand);
            }
            else if (element.Mode == AsmPrefixSymbolKind.PushEnvOperation) {
                result.Kind = AssemblerStatementKind.PushEnvOperation;
                var operand = new SymbolReference();
                InitNode(operand, element.Register);
                operand.Name = ExtractSymbolName(element.Register as IdentifierSymbol);
                result.Operands.Add(operand);
            }
            else if (element.Mode == AsmPrefixSymbolKind.SaveEnvOperation) {
                result.Kind = AssemblerStatementKind.SaveEnvOperation;
                var operand = new SymbolReference();
                InitNode(operand, element.Register);
                operand.Name = ExtractSymbolName(element.Register as IdentifierSymbol);
                result.Operands.Add(operand);
            }
            else if (element.Mode == AsmPrefixSymbolKind.NoFrame) {
                result.Kind = AssemblerStatementKind.NoFrameOperation;
            }

            statementTarget.Statements.Add(result);

        }

        #endregion
        #region LocalAsmLabel

        /// <summary>
        ///     start visiting an assembly label
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(LocalAsmLabelSymbol element) {
            var value = element.AtSymbol.Value;
            foreach (var token in element.Items) {

                if (token is Terminal terminal)
                    value = string.Concat(value, terminal.Value);

                if (token is StandardInteger integer)
                    value = string.Concat(value, integer.Value.Token.Value);

                if (token is IdentifierSymbol ident)
                    value = string.Concat(value, ident.Symbol.Token.Value);

            }

            var parent = LastValue as ILabelTarget;
            parent.LabelName = new SimpleSymbolName(value);
        }

        #endregion
        #region AsmStatement

        /// <summary>
        ///     start visiting an assembler statement
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(AsmStatementSymbol element) {
            var parent = LastValue as BlockOfAssemblerStatements;
            var result = new AssemblerStatement();
            InitNode(result, element);
            parent.Statements.Add(result);
            result.OpCode = ExtractSymbolName((element.OpCode as AsmOpCodeSymbol)?.OpCode);
            result.SegmentPrefix = ExtractSymbolName((element.Prefix as AsmPrefixSymbol)?.SegmentPrefix as IdentifierSymbol);
            result.LockPrefix = ExtractSymbolName((element.Prefix as AsmPrefixSymbol)?.LockPrefix);


        }

        #endregion
        #region ParseAssemblyOperand

        /// <summary>
        ///     start visiting an assembly operand
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(AsmOperandSymbol element) {

            if (element.LeftTerm != null && element.RightTerm != null) {
                var lastExpression = LastExpression;
                var currentExpression = new BinaryOperator();
                InitNode(currentExpression, element);
                lastExpression.Value = currentExpression;
                currentExpression.Kind = TokenKindMapper.ForExpression(element.Kind);
            }

            if (element.NotExpression != null) {
                var lastExpression = LastExpression;
                var currentExpression = new UnaryOperator();
                InitNode(currentExpression, element);
                lastExpression.Value = currentExpression;
                currentExpression.Kind = ExpressionKind.Not;
            }

            ;
        }

        #endregion
        #region AsmExpression

        /// <summary>
        ///     start visiting an assembler expression
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(AsmExpressionSymbol element) {

            if (element.Offset != null) {
                var lastExpression = LastExpression;
                var currentExpression = new UnaryOperator();
                InitNode(currentExpression, element);
                lastExpression.Value = currentExpression;
                currentExpression.Kind = ExpressionKind.AsmOffset;
            }

            if (element.BytePtrKind != null) {
                var lastExpression = LastExpression;
                var currentExpression = new UnaryOperator();
                InitNode(currentExpression, element);
                lastExpression.Value = currentExpression;
                currentExpression.Kind = TokenKindMapper.ForAsmBytePointerKind(ExtractSymbolName(element.BytePtrKind as IdentifierSymbol)?.CompleteName);
            }

            if (element.TypeExpression != null) {
                var lastExpression = LastExpression;
                var currentExpression = new UnaryOperator();
                InitNode(currentExpression, element);
                lastExpression.Value = currentExpression;
                currentExpression.Kind = ExpressionKind.AsmType;
                return;
            }

            if (element.RightOperand != null) {
                var lastExpression = LastExpression;
                var currentExpression = new BinaryOperator();
                InitNode(currentExpression, element);
                lastExpression.Value = currentExpression;
                currentExpression.Kind = TokenKindMapper.ForExpression(element.BinaryOperatorKind);
                return;
            }

            ;
        }

        #endregion
        #region AsmTerm

        /// <summary>
        ///     start visiting an assembler term
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(AsmTermSymbol element) {

            if (element.Kind != TokenKind.Undefined) {
                var lastExpression = LastExpression;
                var currentExpression = new BinaryOperator();
                InitNode(currentExpression, element);
                lastExpression.Value = currentExpression;
                currentExpression.Kind = TokenKindMapper.ForExpression(element.Kind);
            }

            if (element.Subtype != null) {
                var lastExpression = LastExpression;
                var currentExpression = new BinaryOperator();
                InitNode(currentExpression, element);
                lastExpression.Value = currentExpression;
                currentExpression.Kind = ExpressionKind.Dot;
            }
        }

        #endregion
        #region DesignatorStatement

        /// <summary>
        ///     start visiting a designator statmenet
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(DesignatorStatementSymbol element) {
            var lastExpression = LastExpression;

            if (lastExpression == null)
                return;

            var result = new SymbolReference();
            InitNode(result, element);
            if (element.Inherited != default)
                result.Inherited = true;

            lastExpression.Value = result;

        }

        #endregion
        #region DesignatorItem

        /// <summary>
        ///     start visiting a designator item
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(DesignatorItemSymbol element) {

            if (!(LastValue is SymbolReference parent))
                return;

            if (element.Dereference != default) {
                var part = new SymbolReferencePart();
                InitNode(part, element);
                parent.AddPart(part);
                part.Kind = SymbolReferencePartKind.Dereference;
                return;
            }

            if (element.Subitem != default) {
                var part = new SymbolReferencePart();
                var ident = element.Subitem as IdentifierSymbol;
                var strType = element.Subitem as StringTypeSymbol;

                InitNode(part, element);
                parent.AddPart(part);
                part.Kind = SymbolReferencePartKind.SubItem;

                if (ident != default)
                    part.Name = ExtractSymbolName(ident)?.CompleteName;

                if (strType != default)
                    part.Name = strType.StringSymbol.Token.Value;

                part.GenericType = ExtractGenericDefinition(element.SubitemGenericType);

                if (element.IndexExpression != default && strType == default)
                    part.Kind = SymbolReferencePartKind.ArrayIndex;
                else if (element.ParameterList && strType == default)
                    part.Kind = SymbolReferencePartKind.CallParameters;
                else if (element.ParameterList && strType != default)
                    part.Kind = SymbolReferencePartKind.StringCast;
                else if (strType != default)
                    part.Kind = SymbolReferencePartKind.StringType;

                return;
            }

            if (element.IndexExpression != null) {
                var part = new SymbolReferencePart();
                InitNode(part, element);
                parent.AddPart(part);
                part.Kind = SymbolReferencePartKind.ArrayIndex;
                return;
            }

            if (element.ParameterList) {
                var part = new SymbolReferencePart();
                InitNode(part, element);
                parent.AddPart(part);
                part.Kind = SymbolReferencePartKind.CallParameters;
                return;
            }
        }

        #endregion
        #region Parameter

        /// <summary>
        ///     start visiting a parameter definition
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ParameterSymbol element) {
            if (element.ParameterName == null)
                return;

            var lastExpression = LastExpression;
            var result = new SymbolReference();
            InitNode(result, element);
            result.NamedParameter = true;
            result.Name = ExtractSymbolName(element.ParameterName);
            lastExpression.Value = result;

        }

        #endregion
        #region FormattedExpression

        /// <summary>
        ///     start visiting an formatted expression
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(Standard.FormattedExpressionSymbol element) {
            if (element.Width == null && element.Decimals == null)
                return;

            var lastExpression = LastExpression;
            var result = new Abstract.FormattedExpression();
            InitNode(result, element);
            lastExpression.Value = result;

        }

        #endregion
        #region SetSection

        /// <summary>
        ///     start visiting a set expression
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(SetSectionSymbol element) {
            var lastExpression = LastExpression;
            var result = new SetExpression();
            InitNode(result, element);
            lastExpression.Value = result;
        }


        #endregion
        #region SetSectionPart

        /// <summary>
        ///     start visiting a set section
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(SetSectionPartSymbol element) {
            if (element.Continuation.GetSymbolKind() != TokenKind.DotDot) {

                if (!(LastExpression is SetExpression arrayExpression))
                    return;

                if (arrayExpression.Expressions.LastOrDefault() is BinaryOperator binOp && binOp.RightOperand == null) {
                    visitor.WorkingStack.Push(new WorkingStackEntry(element, binOp));
                }

                return;
            }

            var lastExpression = LastExpression;
            var result = new BinaryOperator();
            InitNode(result, element);
            result.Kind = ExpressionKind.RangeOperator;
            lastExpression.Value = result;
        }

        #endregion
        #region AsmFactor

        /// <summary>
        ///     start visiting an asm factor
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(AsmFactorSymbol element) {
            var expression = LastExpression;

            if (element.Number != null) {
                var value = new ConstantValue();
                InitNode(value, element);
                value.Kind = ConstantValueKind.IntegralNumber;
                expression.Value = value;
                return;
            }

            if (element.RealNumber != null) {
                var value = new ConstantValue();
                InitNode(value, element);
                value.Kind = ConstantValueKind.RealNumber;
                expression.Value = value;
                return;
            }

            if (element.HexNumber != null) {
                var value = new ConstantValue();
                InitNode(value, element);
                value.Kind = ConstantValueKind.HexNumber;
                expression.Value = value;
                return;
            }

            if (element.QuotedString != null) {
                var value = new ConstantValue();
                InitNode(value, element);
                value.Kind = ConstantValueKind.QuotedString;
                expression.Value = value;
                return;
            }

            if (element.Identifier != null) {
                var value = new SymbolReference();
                InitNode(value, element);
                value.Name = ExtractSymbolName(element.Identifier as IdentifierSymbol);
                expression.Value = value;
                return;
            }

            if (element.SegmentPrefix != null) {
                var currentExpression = new BinaryOperator();
                InitNode(currentExpression, element);
                expression.Value = currentExpression;
                currentExpression.Kind = ExpressionKind.AsmSegmentPrefix;
                var reference = new SymbolReference();
                InitNode(reference, element.SegmentPrefix);
                reference.Name = ExtractSymbolName(element.SegmentPrefix as IdentifierSymbol);
                currentExpression.LeftOperand = reference;
                return;
            }

            if (element.MemorySubexpression != null) {
                var currentExpression = new UnaryOperator();
                InitNode(currentExpression, element);
                expression.Value = currentExpression;
                currentExpression.Kind = ExpressionKind.AsmMemorySubexpression;
                return;
            }

            if (element.Label != null) {
                var reference = new SymbolReference();
                InitNode(reference, element);
                expression.Value = reference;
                return;
            }
        }

        #endregion

        #region Extractors

        private static SymbolName ExtractSymbolName(NamespaceNameSymbol name) {
            var result = new NamespacedSymbolName();

            if (name == null)
                return result;

            foreach (var nspace in name.Namespace)
                if (!string.IsNullOrWhiteSpace(nspace))
                    result.Append(nspace);

            if (!string.IsNullOrWhiteSpace(name.Name))
                result.Append(name.Name);


            return result;
        }

        private static GenericSymbolName ExtractSymbolName(IList<MethodDeclarationNameSymbol> qualifiers) {
            var result = new GenericSymbolName();

            foreach (var name in qualifiers) {
                if (name.Name != null) {
                    foreach (var namePart in name.Name.Namespace)
                        if (!string.IsNullOrWhiteSpace(namePart))
                            result.AddName(namePart);
                    if (!string.IsNullOrWhiteSpace(name.Name.Name))
                        result.AddName(name.Name.Name);
                    if (name.GenericDefinition != null) {
                        foreach (var part in name.GenericDefinition.Items) {

                            var idPart = part.Identifier;
                            var genPart = part.DefinitionPart;

                            if (idPart != null) {
                                result.AddGenericPart(SyntaxPartBase.IdentifierValue(idPart));
                                continue;
                            }

                            if (genPart != null) {
                                result.AddGenericPart(SyntaxPartBase.IdentifierValue(genPart.Identifier));
                            }
                        }
                    }
                }
            }

            return result;
        }

        private static SymbolName ExtractSymbolName(IdentifierSymbol name) {
            var result = new SimpleSymbolName(name?.Symbol.Value);
            return result;
        }

        private static SymbolName ExtractSymbolName(NamespaceFileNameSymbol name) {
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

        private GenericTypeCollection ExtractGenericDefinition(GenericSuffixSymbol genericDefinition) {
            if (genericDefinition == null)
                return null;

            var result = new GenericTypeCollection();
            InitNode(result, genericDefinition);
            result.TypeReference = true;

            return result;
        }

        private GenericTypeCollection ExtractGenericDefinition(ISyntaxPart node, GenericDefinitionSymbol genericDefinition) {
            if (genericDefinition == null)
                return null;

            var result = new GenericTypeCollection();
            InitNode(result, node);

            foreach (var part in genericDefinition.Items) {
                var idPart = part.Identifier;
                var genPart = part.DefinitionPart;

                if (idPart != null) {
                    var generic = new GenericTypeNameCollection();
                    InitNode(generic, node);
                    generic.Name = ExtractSymbolName(idPart);
                    result.Add(generic, LogSource);
                    visitor.WorkingStack.Pop();
                    continue;
                }

                if (genPart != null) {
                    var generic = new GenericTypeNameCollection();
                    InitNode(generic, node);
                    generic.Name = ExtractSymbolName(genPart.Identifier);
                    result.Add(generic, LogSource);

                    foreach (var constraintPart in genPart.Items) {
                        if (constraintPart is Standard.ConstrainedGenericSymbol constraint) {
                            var cr = new GenericConstraint();
                            InitNode(cr, node);
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

        private static SymbolHints ExtractHints(HintingInformationListSymbol hints) {
            var result = new SymbolHints();

            if (hints == null || hints.Items == null || hints.Items.Length < 1)
                return null;

            foreach (var part in hints.Items) {
                if (!(part is HintSymbol hint))
                    continue;
                ExtractHints(hint, result);
            }

            return result;
        }

        private static SymbolHints ExtractHints(HintSymbol hint, SymbolHints result = null) {
            if (result == null)
                result = new SymbolHints();

            result.SymbolIsDeprecated = result.SymbolIsDeprecated || hint.Deprecated;
            result.DeprecatedInformation = hint.DeprecatedComment?.Symbol?.Token.ParsedValue;
            result.SymbolInLibrary = result.SymbolInLibrary || hint.Library;
            result.SymbolIsPlatformSpecific = result.SymbolIsPlatformSpecific || hint.Platform;
            result.SymbolIsExperimental = result.SymbolIsExperimental || hint.Experimental;
            return result;
        }

        private void ExtractAttributes(UserAttributesSymbol attributes, CompilationUnit parentUnit, IList<SymbolAttributeItem> result) {
            if (attributes == null || attributes.Items == null || attributes.Items.Length < 1)
                return;

            foreach (var set in attributes.Items) {
                foreach (var attribute in set.Items) {

                    var isAssemblyAttribute = false;

                    var userAttribute = new SymbolAttributeItem() {
                        Name = ExtractSymbolName(attribute.Name)
                    };
                    if (!isAssemblyAttribute) {
                        result.Add(userAttribute);
                    }
                    else
                        parentUnit.AddAssemblyAttribute(userAttribute);
                }
            }
        }

        #endregion
        #region Helper functions

        private void InitNode(AbstractSyntaxPartBase result, ISyntaxPart node)
            => InitNode(result, node, null);

        private void InitNode(AbstractSyntaxPartBase result, ISyntaxPart node, ISyntaxPart child) {

            if (node is AbstractSyntaxPartBase)
                throw new InvalidOperationException();

            visitor.WorkingStack.Push(new WorkingStackEntry(node, result, child));
        }

        #endregion

        private ILogSource logSource;

        /// <summary>
        ///     tree states
        /// </summary>
        private readonly IDictionary<AbstractSyntaxPartBase, object> currentValues
            = new Dictionary<AbstractSyntaxPartBase, object>();

        /// <summary>
        ///     log source
        /// </summary>
        public ILogSource LogSource {
            get {
                if (logSource != null)
                    return logSource;

                if (environment.Log != null) {
                    logSource = environment.Log.CreateLogSource(MessageGroups.TreeTransformer);
                    return logSource;
                }

                return null;
            }
        }


        /// <summary>
        ///     project root
        /// </summary>
        public ProjectItemCollection Project { get; }

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
        ///     current member visibility
        /// </summary>
        public DictionaryIndexHelper<AbstractSyntaxPartBase, MemberStatus> CurrentMemberVisibility { get; }

        /// <summary>
        ///     current parameter list
        /// </summary>
        public DictionaryIndexHelper<AbstractSyntaxPartBase, IParameterTarget> CurrentParameterList { get; }

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
        public TreeTransformer(IParserEnvironment env, ProjectItemCollection projectRoot) {
            visitor = new ChildVisitor(this);
            environment = env;
            Project = projectRoot;
            CurrentUnitMode = new DictionaryIndexHelper<AbstractSyntaxPartBase, UnitMode>(currentValues);
            CurrentMemberVisibility = new DictionaryIndexHelper<AbstractSyntaxPartBase, MemberStatus>(currentValues);
            CurrentParameterList = new DictionaryIndexHelper<AbstractSyntaxPartBase, IParameterTarget>(currentValues);
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

        /// <summary>
        ///     end visit a case statement
        /// </summary>
        /// <param name="element"></param>
        /// <param name="child"></param>
        public void EndVisitChild(CaseStatementSymbol element, ISyntaxPart child) {
        }

        /// <summary>
        ///     end visiting a type name
        /// </summary>
        /// <param name="element"></param>
        /// <param name="child"></param>
        public void EndVisitChild(TypeNameSymbol element, ISyntaxPart child) {
        }

        /// <summary>
        ///     end visiting a simple element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="child"></param>
        public void EndVisitChild(SimpleTypeSymbol element, ISyntaxPart child) {
        }

        /// <summary>
        ///     end visiting a method directive
        /// </summary>
        /// <param name="element"></param>
        /// <param name="child"></param>
        public void EndVisitChild(MethodDirectivesSymbol element, ISyntaxPart child) {
        }

        /// <summary>
        ///     end visit a function directive
        /// </summary>
        /// <param name="element"></param>
        /// <param name="child"></param>
        public void EndVisitChild(FunctionDirectivesSymbol element, ISyntaxPart child) {
        }

        /// <summary>
        ///     end visiting a try statement
        /// </summary>
        /// <param name="element"></param>
        /// <param name="child"></param>
        public void EndVisitChild(TryStatementSymbol element, ISyntaxPart child) {
        }

        /// <summary>
        ///     end visiting an if statement
        /// </summary>
        /// <param name="element"></param>
        /// <param name="child"></param>
        public void EndVisitChild(IfStatementSymbol element, ISyntaxPart child) {
        }
    }
}
