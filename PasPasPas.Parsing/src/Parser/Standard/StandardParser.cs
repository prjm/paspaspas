using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.ObjectPooling;
using PasPasPas.Options.Bundles;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.SyntaxTree.Standard;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.Tokenizer;
using PasPasPas.Parsing.Tokenizer.Patterns;
using PasPasPas.Parsing.Tokenizer.TokenGroups;

namespace PasPasPas.Parsing.Parser.Standard {

    /// <summary>
    ///     standard, recursive descend parser
    /// </summary>
    public partial class StandardParser : ParserBase, IParser {

        private static InputPatterns GetPatternsFromFactory(IParserEnvironment environment)
            => environment.Patterns.StandardPatterns;

        private static TokenizerWithLookahead CreateTokenizer(IParserEnvironment environment, StackedFileReader reader, OptionSet options)
            => new TokenizerWithLookahead(environment, options, new Tokenizer.Tokenizer(environment, GetPatternsFromFactory(environment), reader), TokenizerMode.Standard);

        /// <summary>
        ///     creates a new standard parser
        /// </summary>
        public StandardParser(IParserEnvironment env, OptionSet options, StackedFileReader input) :
            base(env, options, CreateTokenizer(env, input, options)) { }

        #region Reserved Words

        private static readonly HashSet<int> reservedWords
            = new HashSet<int>() {
            TokenKind.And,
            TokenKind.Array,
            TokenKind.As,
            TokenKind.Asm,
            TokenKind.Begin,
            TokenKind.Case,
            TokenKind.Class,
            TokenKind.Const,
            TokenKind.Constructor,
            TokenKind.Destructor,
            TokenKind.DispInterface,
            TokenKind.Div,
            TokenKind.Do,
            TokenKind.DownTo,
            TokenKind.Else,
            TokenKind.End,
            TokenKind.Except,
            TokenKind.Exports,
            TokenKind.File,
            TokenKind.Finalization   ,
            TokenKind.Finally,
            TokenKind.For,
            TokenKind.Function,
            TokenKind.GoToKeyword,
            TokenKind.If,
            TokenKind.Implementation,
            TokenKind.In,
            TokenKind.Inherited,
            TokenKind.Initialization,
            TokenKind.Inline,
            TokenKind.Interface,
            TokenKind.Is,
            TokenKind.Label,
            TokenKind.Library,
            TokenKind.Mod,
            TokenKind.Nil,
            TokenKind.Not,
            TokenKind.Object,
            TokenKind.Of,
            TokenKind.Or,
            TokenKind.Packed,
            TokenKind.Procedure,
            TokenKind.Program,
            TokenKind.Property,
            TokenKind.Raise,
            TokenKind.Record,
            TokenKind.Repeat,
            TokenKind.Resourcestring,
            TokenKind.Set,
            TokenKind.Shl,
            TokenKind.Shr,
            TokenKind.String,
            TokenKind.Then,
            TokenKind.ThreadVar,
            TokenKind.To,
            TokenKind.Try,
            TokenKind.TypeKeyword,
            TokenKind.Unit,
            TokenKind.Until,
            TokenKind.Uses,
            TokenKind.Var,
            TokenKind.While,
            TokenKind.With,
            TokenKind.Xor
            };

        #endregion
        #region assembler symbols

        private HashSet<string> lockPrefixes =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
                "lock",
                "repne",
                "repnz",
                "rep",
                "repe",
                "repz" };

        private HashSet<string> segmentPrefixes =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
                "cs", "ds", "es", "fs", "gs", "ss"};

        private HashSet<string> asmPtr =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
                "byte", "word", "dword", "qword", "tbyte" };

        #endregion
        #region Parse

        /// <summary>
        ///     parse input
        /// </summary>
        public override ISyntaxPart Parse()
            => ParseFile();

        #endregion
        #region ParseFile

        /// <summary>
        ///     parse a standard file
        /// </summary>
        /// <returns></returns>
        [Rule("File", "Program | Library | Unit | Package")]
        public ISyntaxPart ParseFile() {

            if (Tokenizer.AtEof)
                throw new Exception();

            var path = Tokenizer.Input.CurrentFile;

            if (Match(TokenKind.Library))
                return ParseLibrary(path);

            if (Match(TokenKind.Unit))
                return ParseUnit(path);

            if (Match(TokenKind.Package))
                return ParsePackage(path);

            return ParseProgram(path);
        }

        #endregion

        #region ParseUnitInterface

        /// <summary>
        ///     parse a unit interface
        /// </summary>
        /// <returns></returns>

        [Rule("UnitInterface", "'interface' [ UsesClause ] InterfaceDeclaration ")]
        public UnitInterfaceSymbol ParseUnitInterface()
            => new UnitInterfaceSymbol(
                interfaceSymbol: ContinueWithOrMissing(TokenKind.Interface),
                usesClause: Match(TokenKind.Uses) ? ParseUsesClause() : default,
                interfaceDeclaration: ParseInterfaceDeclaration());

        #endregion
        #region ParseUnitImplementation

        /// <summary>
        ///     parse a unit implementation symbol
        /// </summary>
        /// <returns></returns>

        [Rule("UnitImplementation", "'implementation' [ UsesClause ] DeclarationSections", true)]
        public UnitImplementationSymbol ParseUnitImplementation() {
            var implementation = ContinueWithOrMissing(TokenKind.Implementation);
            var usesClause = default(UsesClauseSymbol);

            if (Match(TokenKind.Uses))
                usesClause = ParseUsesClause();

            var declarationSections = ParseDeclarationSections();
            return new UnitImplementationSymbol(implementation, usesClause, declarationSections);
        }

        #endregion
        #region ParseUsesClause

        /// <summary>
        ///     parse a uses clause
        /// </summary>

        [Rule("UsesClause", "'uses' NamespaceNameList")]
        public UsesClauseSymbol ParseUsesClause() {
            var usesSymbol = ContinueWithOrMissing(TokenKind.Uses);
            var usesList = ParseNamespaceNameList();
            var semicolon = ContinueWithOrMissing(TokenKind.Semicolon);
            return new UsesClauseSymbol(usesSymbol, usesList, semicolon);
        }

        #endregion
        #region ParseUsesFileClause

        /// <summary>
        ///     parse a uses file clause
        /// </summary>
        /// <returns></returns>

        [Rule("UsesFileClause", "'uses' NamespaceFileNameList ';'")]
        public UsesFileClauseSymbol ParseUsesFileClause()
            => new UsesFileClauseSymbol(
                ContinueWithOrMissing(TokenKind.Uses),
                ParseNamespaceFileNameList(),
                ContinueWithOrMissing(TokenKind.Semicolon));

        #endregion
        #region ParseNamespaceFileNameList

        /// <summary>
        ///     parse a namespace file name list
        /// </summary>
        /// <returns></returns>

        [Rule("NamespaceFileNameList", "NamespaceFileName { ',' NamespaceFileName }")]
        public NamespaceFileNameListSymbol ParseNamespaceFileNameList() {
            using (var list = GetList<NamespaceFileNameSymbol>()) {
                var item = default(NamespaceFileNameSymbol);

                do {
                    item = AddToList(list, ParseNamespaceFileName(true));
                } while (item != default && item.Comma != default);

                return new NamespaceFileNameListSymbol(GetFixedArray(list));
            }
        }

        #endregion
        #region ParseNamespaceFileName

        /// <summary>
        ///     parse a namespace file name
        /// </summary>
        /// <param name="allowComma"></param>
        /// <returns></returns>

        [Rule("NamespaceFileName", "NamespaceName [ 'in' QuotedString ]")]
        public NamespaceFileNameSymbol ParseNamespaceFileName(bool allowComma) {
            var name = ParseNamespaceName();
            var inSymbol = ContinueWith(TokenKind.In);
            var fileName = default(QuotedStringSymbol);
            var comma = default(Terminal);

            if (inSymbol != default)
                fileName = RequireString();

            if (allowComma)
                comma = ContinueWith(TokenKind.Comma);

            return new NamespaceFileNameSymbol(name, inSymbol, fileName, comma);
        }

        #endregion
        #region ParseInterfaceDeclaration

        /// <summary>
        ///     parse unit interface: declarations
        /// </summary>
        /// <returns></returns>

        [Rule("InterfaceDeclaration", "{ InterfaceDeclarationItem }")]
        public InterfaceDeclarationSymbol ParseInterfaceDeclaration() {
            var result = new InterfaceDeclarationSymbol(default);
            var item = default(SyntaxPartBase);

            using (var list = GetList<SyntaxPartBase>()) {
                do {
                    item = AddToList(list, ParseInterfaceDeclarationItem());
                } while (item != null);

                return new InterfaceDeclarationSymbol(GetFixedArray(list));
            }
        }

        #endregion
        #region ParseInterfaceDeclarationItem

        [Rule("InterfaceDeclarationItem", "ConstSection | TypeSection | VarSection | ExportsSection | AssemblyAttribute | ExportedProcedureHeading")]
        private SyntaxPartBase ParseInterfaceDeclarationItem() {

            if (Match(TokenKind.Const) || Match(TokenKind.Resourcestring))
                return ParseConstSection(false);

            if (Match(TokenKind.TypeKeyword))
                return ParseTypeSection(false);

            if (Match(TokenKind.Var))
                return ParseVarSection(false);

            if (Match(TokenKind.Exports))
                return ParseExportsSection();

            if (Match(TokenKind.OpenBraces) && LookAhead(1, TokenKind.Assembly))
                return ParseAttributes();

            if (Match(TokenKind.Procedure, TokenKind.Function))
                return ParseExportedProcedureHeading();

            return null;
        }

        #endregion
        #region ParseConstSection

        /// <summary>
        ///     parse a constant section
        /// </summary>
        /// <param name="inClassDeclaration"></param>
        /// <returns></returns>

        [Rule("ConstSection", "('const' | 'resourcestring') ConstDeclaration { ConstDeclaration }")]
        public ConstSectionSymbol ParseConstSection(bool inClassDeclaration) {
            var symbol = ContinueWith(TokenKind.Const, TokenKind.Resourcestring);

            using (var list = GetList<ConstDeclarationSymbol>()) {
                while ((!inClassDeclaration || !(Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Automated, TokenKind.Strict))) && MatchIdentifier(TokenKind.OpenBraces)) {
                    AddToList(list, ParseConstDeclaration());
                }
                return new ConstSectionSymbol(symbol, GetFixedArray(list));
            }
        }

        #endregion
        #region ParseExportedProcedureHeading

        /// <summary>
        ///     parse an exported procedure heading
        /// </summary>
        /// <returns></returns>

        [Rule("ExportedProcedureHeading", "")]
        public ExportedProcedureHeadingSymbol ParseExportedProcedureHeading() {
            var procSymbol = ContinueWith(TokenKind.Function, TokenKind.Procedure);
            var name = RequireIdentifier();
            var parameters = default(FormalParameterSection);
            var colonSymbol = default(Terminal);
            var resultAttributes = default(UserAttributesSymbol);
            var resultType = default(TypeSpecificationSymbol);
            var semicolon = default(Terminal);
            var directives = default(FunctionDirectivesSymbol);

            if (Match(TokenKind.OpenParen))
                parameters = ParseFormalParameterSection();

            colonSymbol = ContinueWith(TokenKind.Colon);

            if (colonSymbol != default) {
                resultAttributes = ParseAttributes();
                resultType = ParseTypeSpecification();
            }

            semicolon = ContinueWithOrMissing(TokenKind.Semicolon);
            directives = ParseFunctionDirectives();

            return new ExportedProcedureHeadingSymbol(procSymbol, name, parameters, colonSymbol, resultAttributes, resultType, semicolon, directives);
        }

        #endregion
        #region ParseFunctionDirectives

        /// <summary>
        ///     parse function directives
        /// </summary>
        /// <returns></returns>

        [Rule("FunctionDirectives", "{ FunctionDirective } ")]
        public FunctionDirectivesSymbol ParseFunctionDirectives() {
            using (var list = GetList<SyntaxPartBase>()) {
                var directive = default(SyntaxPartBase);
                do {
                    directive = AddToList(list, ParseFunctionDirective());
                } while (directive != default);
                return new FunctionDirectivesSymbol(GetFixedArray(list));
            }
        }

        #endregion
        #region ParseFunctionDirective

        [Rule("FunctionDirective", "OverloadDirective | InlineDirective | CallConvention | OldCallConvention | Hint | ExternalDirective | UnsafeDirective")]
        private SyntaxPartBase ParseFunctionDirective() {

            if (Match(TokenKind.Overload))
                return ParseOverloadDirective();

            if (Match(TokenKind.Inline, TokenKind.Assembler))
                return ParseInlineDirective();

            if (Match(TokenKind.Cdecl, TokenKind.Pascal, TokenKind.Register, TokenKind.Safecall, TokenKind.Stdcall, TokenKind.Export))
                return ParseCallConvention();

            if (Match(TokenKind.Far, TokenKind.Local, TokenKind.Near))
                return ParseOldCallConvention();

            if (Match(TokenKind.Deprecated, TokenKind.Library, TokenKind.Experimental, TokenKind.Platform))
                return ParseHint(true);

            if (Match(TokenKind.VarArgs, TokenKind.External))
                return ParseExternalDirective();

            if (Match(TokenKind.Unsafe))
                return ParseUnsafeDirective();

            if (Match(TokenKind.Forward))
                return ParseForwardDirective();

            return null;
        }

        #endregion
        #region ParseForwardDirective

        /// <summary>
        ///     parse a forward directive
        /// </summary>
        /// <returns></returns>

        [Rule("ForwardDirective", "'forward' ';' ")]
        public ForwardDirectiveSymbol ParseForwardDirective()
            => new ForwardDirectiveSymbol(
                ContinueWithOrMissing(TokenKind.Forward),
                ContinueWithOrMissing(TokenKind.Semicolon));

        #endregion
        #region ParseUnsafeDirective

        /// <summary>
        ///     parse a unsafe directive
        /// </summary>
        /// <returns></returns>

        [Rule("UnsafeDirective", "'unsafe' ';' ")]
        public UnsafeDirectiveSymbol ParseUnsafeDirective()
            => new UnsafeDirectiveSymbol(
                ContinueWithOrMissing(TokenKind.Unsafe),
                ContinueWithOrMissing(TokenKind.Semicolon));

        #endregion
        #region ParseExternalDirective

        /// <summary>
        ///     parse a external directive
        /// </summary>
        /// <returns></returns>

        [Rule("ExternalDirective", "(varargs | external [ ConstExpression { ExternalSpecifier } ]) ';' ")]
        public ExternalDirectiveSymbol ParseExternalDirective() {
            var directive = ContinueWithOrMissing(TokenKind.VarArgs, TokenKind.External);
            var externalExpression = default(ConstantExpressionSymbol);

            using (var list = GetList<ExternalSpecifierSymbol>()) {
                if ((directive.GetSymbolKind() == TokenKind.External) && (!Match(TokenKind.Semicolon))) {
                    externalExpression = ParseConstantExpression();
                    var specifier = default(ExternalSpecifierSymbol);
                    do {
                        specifier = AddToList(list, ParseExternalSpecifier());
                    } while (specifier != default);
                }

                var semicolon = ContinueWithOrMissing(TokenKind.Semicolon);
                return new ExternalDirectiveSymbol(directive, externalExpression, GetFixedArray(list), semicolon);
            }
        }

        #endregion
        #region ParseExternalSpecifier

        /// <summary>
        ///     parse an external specifier
        /// </summary>
        /// <returns></returns>

        [Rule("ExternalSpecifier", "(('Name' | 'Index' ) ConstExpression) |  'dependency' ConstExpression { ', ' ConstExpression } ) | delayed ")]
        public ExternalSpecifierSymbol ParseExternalSpecifier() {

            var specifier = ContinueWith(TokenKind.Name, TokenKind.Index, TokenKind.Dependency, TokenKind.Delayed);

            if (specifier == default)
                return default;

            if (specifier.GetSymbolKind() == TokenKind.Delayed)
                return new ExternalSpecifierSymbol(specifier);

            using (var list = GetList<ConstantExpressionSymbol>()) {

                if (specifier.GetSymbolKind() != TokenKind.Dependency) {
                    AddToList(list, ParseConstantExpression());
                    return new ExternalSpecifierSymbol(specifier, GetFixedArray(list));
                }

                var expression = default(ConstantExpressionSymbol);
                do {
                    expression = AddToList(list, ParseConstantExpression(false, false, true));
                } while (expression != default && expression.Comma.GetSymbolKind() == TokenKind.Comma);


                return new ExternalSpecifierSymbol(specifier, GetFixedArray(list));
            }
        }

        #endregion
        #region ParseOldCallConvention

        /// <summary>
        ///     parse old call convention symbol
        /// </summary>
        /// <returns></returns>

        [Rule("OldCallConvention", "'Near' | 'Far' | 'Local'")]
        public OldCallConventionSymbol ParseOldCallConvention() {
            var directive = ContinueWithOrMissing(TokenKind.Near, TokenKind.Far, TokenKind.Local);
            var semicolon = ContinueWithOrMissing(TokenKind.Semicolon);
            return new OldCallConventionSymbol(directive, semicolon);
        }

        #endregion
        #region ParseUnitBlock

        /// <summary>
        ///     parse a unit block
        /// </summary>
        /// <returns></returns>

        [Rule("UnitBlock", "( UnitInitilization 'end' ) | CompoundStatement | 'end' ")]
        public UnitBlockSymbol ParseUnitBlock() {
            var initialization = default(UnitInitializationSymbol);
            var mainBlock = default(CompoundStatementSymbol);
            var endSymbol = ContinueWith(TokenKind.End);

            if (endSymbol != default)
                return new UnitBlockSymbol(endSymbol);

            if (Match(TokenKind.Begin, TokenKind.Asm)) {
                mainBlock = ParseCompoundStatement();
                return new UnitBlockSymbol(mainBlock);
            }

            if (Match(TokenKind.Initialization)) {
                initialization = ParseUnitInitialization();
                endSymbol = ContinueWithOrMissing(TokenKind.End);
                return new UnitBlockSymbol(initialization, endSymbol);
            }

            Unexpected();
            return null;
        }

        #endregion
        #region ParseInitialiParseUnitInitialization

        /// <summary>
        ///     parse unit initialization
        /// </summary>
        /// <returns></returns>

        [Rule("UnitInitialization", "'initialization' StatementList [ UnitFinalization ]", true)]
        public UnitInitializationSymbol ParseUnitInitialization() {
            var initSymbol = ContinueWithOrMissing(TokenKind.Initialization);
            var statements = ParseStatementList();
            var finalization = default(UnitFinalizationSymbol);

            if (Match(TokenKind.Finalization)) {
                finalization = ParseUnitFinalization();
            }

            return new UnitInitializationSymbol(initSymbol, statements, finalization);
        }

        #endregion
        #region ParseUnitFinalization

        /// <summary>
        ///     parse unit finalization
        /// </summary>
        /// <returns></returns>

        [Rule("UnitFinalization", "'finalization' StatementList", true)]
        public UnitFinalizationSymbol ParseUnitFinalization() {
            var finalizationSymbol = ContinueWithOrMissing(TokenKind.Finalization);
            var statements = ParseStatementList();
            return new UnitFinalizationSymbol(finalizationSymbol, statements);
        }

        #endregion
        #region ParseCompoundStatement

        /// <summary>
        ///     parse a compound statement
        /// </summary>
        /// <returns></returns>

        [Rule("CompoundStatement", "(('begin' [ StatementList ] 'end' ) | AsmBlock )")]
        public CompoundStatementSymbol ParseCompoundStatement() {

            if (Match(TokenKind.Asm))
                return new CompoundStatementSymbol(ParseAsmBlock());

            var beginSymbol = ContinueWithOrMissing(TokenKind.Begin);
            var statements = default(StatementList);

            if (!Match(TokenKind.End))
                statements = ParseStatementList();

            var endSymbol = ContinueWithOrMissing(TokenKind.End);
            return new CompoundStatementSymbol(beginSymbol, statements, endSymbol);
        }

        #endregion
        #region StatementList

        [Rule("StatementList", "[Statement], { ';' [Statement]}")]
        private StatementList ParseStatementList() {
            using (var list = GetList<StatementSymbol>()) {
                var statement = default(StatementSymbol);
                do {
                    statement = AddToList(list, ParseStatement(true));
                } while (statement.Semicolon != default);

                return new StatementList(GetFixedArray(list));
            }
        }

        #endregion
        #region ParseStatement

        /// <summary>
        ///     parse a statement
        /// </summary>
        /// <param name="allowSemicolon"></param>
        /// <returns></returns>

        [Rule("Statement", "[ Label ':' ] StatementPart")]
        public StatementSymbol ParseStatement(bool allowSemicolon = false) {

            var label = default(LabelSymbol);
            var colonSymbol = default(Terminal);
            var semicolon = default(Terminal);

            if (MatchIdentifier(TokenKind.Integer, TokenKind.HexNumber) && LookAhead(1, TokenKind.Colon)) {
                label = ParseLabel();
                colonSymbol = ContinueWithOrMissing(TokenKind.Colon);
            }

            var part = ParseStatementPart();

            if (label != null && part == null) {
                Unexpected();
                return null;
            }

            if (allowSemicolon)
                semicolon = ContinueWith(TokenKind.Semicolon);


            return new StatementSymbol(label, colonSymbol, part, semicolon);
        }
        #endregion
        #region ParseStatementPart

        [Rule("StatementPart", "IfStatement | CaseStatement | ReapeatStatement | WhileStatment | ForStatement | WithStatement | TryStatement | RaiseStatement | AsmStatement | CompoundStatement | SimpleStatement ")]
        private StatementPart ParseStatementPart() {

            if (Match(TokenKind.If))
                return new StatementPart(ParseIfStatement());

            if (Match(TokenKind.Case))
                return new StatementPart(ParseCaseStatement());

            if (Match(TokenKind.Repeat))
                return new StatementPart(ParseRepeatStatement());

            if (Match(TokenKind.While))
                return new StatementPart(ParseWhileStatement());

            if (Match(TokenKind.For))
                return new StatementPart(ParseForStatement());

            if (Match(TokenKind.With))
                return new StatementPart(ParseWithStatement());

            if (Match(TokenKind.Try))
                return new StatementPart(ParseTryStatement());

            if (Match(TokenKind.Raise))
                return new StatementPart(ParseRaiseStatement());

            if (Match(TokenKind.Begin, TokenKind.Asm))
                return new StatementPart(ParseCompoundStatement());

            return ParseSimpleStatement();
        }

        #endregion
        #region ParseRaiseStatement

        /// <summary>
        ///     parse a raise statement
        /// </summary>
        /// <returns></returns>

        [Rule("RaiseStatement", "'raise' [ Expression ] [ 'at' Expression ]")]
        public RaiseStatementSymbol ParseRaiseStatement() {
            var raiseSymbol = ContinueWith(TokenKind.Raise);
            var raiseExpression = default(ExpressionSymbol);
            var atExpression = default(ExpressionSymbol);
            var atSymbol = default(Terminal);

            if ((!Match(TokenKind.AtWord)) && MatchIdentifier(TokenKind.Inherited)) {
                raiseExpression = ParseExpression();
            }

            if (Match(TokenKind.AtWord)) {
                atSymbol = ContinueWith(TokenKind.AtWord);
                atExpression = ParseExpression();
            }

            return new RaiseStatementSymbol(raiseSymbol, atSymbol, raiseExpression, atExpression);
        }

        #endregion
        #region ParseTryStatement

        /// <summary>
        ///     parse a try statement
        /// </summary>
        /// <returns></returns>

        [Rule("TryStatement", "'try' StatementList  ('except' HandlerList | 'finally' StatementList) 'end'")]
        public TryStatementSymbol ParseTryStatement() {
            var trySymbol = ContinueWithOrMissing(TokenKind.Try);
            var statements = ParseStatementList();
            var exceptSymbol = default(Terminal);
            var handlers = default(ExceptHandlersSymbol);
            var finallySymbol = default(Terminal);
            var finallyStatements = default(StatementList);
            var endSymbol = default(Terminal);

            exceptSymbol = ContinueWith(TokenKind.Except);
            finallySymbol = ContinueWith(TokenKind.Finally);

            if (exceptSymbol != default && finallySymbol == default) {
                handlers = ParseExceptHandlers();
                endSymbol = ContinueWithOrMissing(TokenKind.End);
            }
            else if (finallySymbol != default && exceptSymbol == default) {
                finallyStatements = ParseStatementList();
                endSymbol = ContinueWithOrMissing(TokenKind.End);
            }
            else {
                Unexpected();
            }

            return new TryStatementSymbol(trySymbol, statements, exceptSymbol, handlers, finallySymbol, finallyStatements, endSymbol);
        }

        #endregion
        #region ParseExceptHandlers

        /// <summary>
        ///     parse except handlers
        /// </summary>
        /// <returns></returns>

        [Rule("ExceptHandlers", "({ Handler } [ 'else' StatementList ]) | StatementList")]
        public ExceptHandlersSymbol ParseExceptHandlers() {
            using (var list = GetList<ExceptHandlerSymbol>()) {
                var elseSymbol = default(Terminal);
                var statements = default(StatementList);

                if (Match(TokenKind.On, TokenKind.Else)) {
                    while (Match(TokenKind.On)) {
                        AddToList(list, ParseExceptHandler());
                    }

                    elseSymbol = ContinueWith(TokenKind.Else);

                    if (elseSymbol != default) {
                        statements = ParseStatementList();
                    }
                }
                else {
                    statements = ParseStatementList();
                }

                return new ExceptHandlersSymbol(GetFixedArray(list), elseSymbol, statements);
            }
        }

        #endregion
        #region ParseExceptHandler

        /// <summary>
        ///     parse an except handler
        /// </summary>
        /// <returns></returns>

        [Rule("ExceptHandler", "'on' Identifier ':' NamespaceName 'do' Statement ';'")]
        public ExceptHandlerSymbol ParseExceptHandler() {
            var on = ContinueWithOrMissing(TokenKind.On);
            var name = RequireIdentifier();
            var colon = ContinueWithOrMissing(TokenKind.Colon);
            var type = ParseTypeName();
            var doSymbol = ContinueWithOrMissing(TokenKind.Do);
            var statement = ParseStatement();
            var semicolon = ContinueWithOrMissing(TokenKind.Semicolon);
            return new ExceptHandlerSymbol(on, name, colon, type, doSymbol, statement, semicolon);
        }

        #endregion
        #region ParseWithStatement

        /// <summary>
        ///     parse a with statement
        /// </summary>
        /// <returns></returns>

        [Rule("WithStatement", "'with' Expression { ',' Expression }  'do' Statement")]
        public WithStatementSymbol ParseWithStatement() {
            using (var list = GetList<ExpressionSymbol>()) {
                var withSymbol = ContinueWithOrMissing(TokenKind.With);
                var item = default(ExpressionSymbol);
                do {
                    item = AddToList(list, ParseExpression(false, true));
                } while (item != default && item.Comma != default);

                var doSymbol = ContinueWithOrMissing(TokenKind.Do);
                var statement = ParseStatement();
                return new WithStatementSymbol(withSymbol, GetFixedArray(list), doSymbol, statement);
            }
        }

        #endregion
        #region ParseForStatement

        /// <summary>
        ///     parse a for statement
        /// </summary>
        /// <returns></returns>

        [Rule("ForStatement", "('for' Designator ':=' Expression ('to' | 'downto' )  Expression 'do' Statement) | ('for' Designator 'in' Expression  'do' Statement)")]
        public ForStatementSymbol ParseForStatement() {
            var forKeyword = ContinueWithOrMissing(TokenKind.For);
            var loopVariable = RequireIdentifier();
            var assignment = ContinueWith(TokenKind.Assignment);
            var startExpression = default(ExpressionSymbol);
            var loopOperator = default(Terminal);
            var endExpression = default(ExpressionSymbol);
            var doKeyword = default(Terminal);
            var loopStatement = default(StatementSymbol);

            if (assignment != default) {
                startExpression = ParseExpression();
                loopOperator = ContinueWithOrMissing(TokenKind.To, TokenKind.DownTo);
                endExpression = ParseExpression();
            }
            else {
                loopOperator = ContinueWithOrMissing(TokenKind.In);
                endExpression = ParseExpression();
            }

            doKeyword = ContinueWithOrMissing(TokenKind.Do);
            loopStatement = ParseStatement();

            return new ForStatementSymbol(forKeyword, loopVariable, assignment, startExpression, loopOperator, endExpression, doKeyword, loopStatement);
        }

        #endregion
        #region ParseWhileStatement

        /// <summary>
        ///     parse a while statement
        /// </summary>
        /// <returns></returns>

        [Rule("WhileStatement", "'while' Expression 'do' Statement")]
        public WhileStatementSymbol ParseWhileStatement() {
            var whileSymbol = ContinueWithOrMissing(TokenKind.While);
            var condition = ParseExpression();
            var doSymbol = ContinueWithOrMissing(TokenKind.Do);
            var statement = ParseStatement();
            return new WhileStatementSymbol(whileSymbol, condition, doSymbol, statement);
        }

        #endregion
        #region ParseRepeatStatement

        /// <summary>
        ///     parse a repeat statement
        /// </summary>
        /// <returns></returns>

        [Rule("RepeatStatement", "'repeat' [ StatementList ] 'until' Expression")]
        public RepeatStatement ParseRepeatStatement() {
            var repeat = ContinueWithOrMissing(TokenKind.Repeat);
            var statements = default(StatementList);

            if (!Match(TokenKind.Until)) {
                statements = ParseStatementList();
            }

            var until = ContinueWithOrMissing(TokenKind.Until);
            var condition = ParseExpression();
            return new RepeatStatement(repeat, statements, until, condition);
        }

        #endregion
        #region ParseCaseStatement

        /// <summary>
        ///     parse a case statement
        /// </summary>
        /// <returns></returns>

        [Rule("CaseStatement", "'case' Expression 'of' { CaseItem } ['else' StatementList[';']] 'end' ")]
        public CaseStatementSymbol ParseCaseStatement() {

            var caseSymbol = ContinueWithOrMissing(TokenKind.Case);
            var caseExpression = ParseExpression();
            var ofSymbol = ContinueWithOrMissing(TokenKind.Of);
            var item = default(CaseItemSymbol);

            using (var list = GetList<CaseItemSymbol>()) {
                do {
                    item = AddToList(list, ParseCaseItem());
                } while (Tokenizer.HasNextToken && item != default);


                if (Match(TokenKind.Else)) {
                    return new CaseStatementSymbol(
                        caseSymbol, caseExpression, ofSymbol,
                        GetFixedArray(list),
                        ContinueWithOrMissing(TokenKind.Else),
                        ParseStatementList(),
                        ContinueWith(TokenKind.Semicolon),
                        ContinueWithOrMissing(TokenKind.End));
                }

                return new CaseStatementSymbol(caseSymbol, caseExpression, ofSymbol, GetFixedArray(list), ContinueWithOrMissing(TokenKind.End));
            }
        }

        #endregion
        #region ParseCaseItem

        /// <summary>
        ///     parse a case item
        /// </summary>
        /// <returns></returns>

        [Rule("CaseItem", "CaseLabel { ',' CaseLabel } ':' Statement [';']")]
        public CaseItemSymbol ParseCaseItem() {

            if (Match(TokenKind.Else, TokenKind.End))
                return default;

            if (!HasTokenBeforeToken(TokenKind.Colon, TokenKind.Semicolon, TokenKind.End, TokenKind.Begin))
                return default;

            var label = default(CaseLabelSymbol);
            using (var list = GetList<CaseLabelSymbol>()) {
                do {
                    label = AddToList(list, ParseCaseLabel());
                } while (label.Comma != default);

                return new CaseItemSymbol(
                    GetFixedArray(list),
                    ContinueWithOrMissing(TokenKind.Colon),
                    ParseStatement(),
                    ContinueWith(TokenKind.Semicolon)
                );
            }
        }

        #endregion
        #region ParseCaseLabel

        /// <summary>
        ///     parse a case label
        /// </summary>
        /// <param name="allowComma"></param>
        /// <returns></returns>

        [Rule("CaseLabel", "Expression [ '..' Expression ] [,]")]
        public CaseLabelSymbol ParseCaseLabel(bool allowComma = true) {
            var startExpression = ParseExpression();
            var dots = default(Terminal);
            var endExpression = default(ExpressionSymbol);
            var comma = default(Terminal);

            if (Match(TokenKind.DotDot)) {
                dots = ContinueWithOrMissing(TokenKind.DotDot);
                endExpression = ParseExpression();
            }

            if (allowComma)
                comma = ContinueWith(TokenKind.Comma);

            return new CaseLabelSymbol(startExpression, dots, endExpression, comma);
        }

        #endregion
        #region IfStatement

        /// <summary>
        ///     parse an if statement
        /// </summary>
        /// <returns></returns>

        [Rule("IfStatement", "'if' Expression 'then' Statement [ 'else' Statement ]")]
        public IfStatementSymbol ParseIfStatement() {
            var ifSymbol = ContinueWithOrMissing(TokenKind.If);
            var condition = ParseExpression();
            var thenSymbol = ContinueWithOrMissing(TokenKind.Then);
            var thenPart = ParseStatement();
            var elseSymbol = ContinueWith(TokenKind.Else);
            var elsePart = default(StatementSymbol);

            if (elseSymbol != default)
                elsePart = ParseStatement();

            return new IfStatementSymbol(ifSymbol, condition, thenSymbol, thenPart, elseSymbol, elsePart);
        }

        #endregion
        #region ParseSimpleStatement

        [Rule("SimpleStatement", "GoToStatement | Designator [ ':=' (Expression  | NewStatement) ] ")]
        private StatementPart ParseSimpleStatement() {
            if (!(LookAhead(1, TokenKind.Assignment, TokenKind.OpenBraces, TokenKind.OpenParen)) && Match(TokenKind.GoToKeyword, TokenKind.Exit, TokenKind.Break, TokenKind.Continue))
                return new StatementPart(ParseGoToStatement());

            if (MatchIdentifier(TokenKind.Inherited, TokenKind.Circumflex, TokenKind.OpenParen, TokenKind.At, TokenKind.AnsiString, TokenKind.UnicodeString, TokenKind.String, TokenKind.WideString, TokenKind.ShortString)) {
                var designator = ParseDesignator();
                var assignmentSymbol = ContinueWith(TokenKind.Assignment);
                var assignmentValue = default(ExpressionSymbol);

                if (assignmentSymbol != default)
                    assignmentValue = ParseExpression();

                return new StatementPart(designator, assignmentSymbol, assignmentValue);
            }

            return null;
        }

        #endregion
        #region ParseGoToStatement

        /// <summary>
        ///     parse a goto statement
        /// </summary>
        /// <returns></returns>

        [Rule("GoToStatement", "('goto' Label) | 'break' | 'continue' | 'exit' '(' Expression ')' ")]
        public GoToStatementSymbol ParseGoToStatement() {

            if (Match(TokenKind.GoToKeyword))
                return new GoToStatementSymbol(ContinueWith(TokenKind.GoToKeyword), ParseLabel());

            if (Match(TokenKind.Break, TokenKind.Continue))
                return new GoToStatementSymbol(ContinueWith(TokenKind.Break, TokenKind.Continue));

            if (Match(TokenKind.Exit)) {
                var terminal = ContinueWith(TokenKind.Exit);
                var openParen = ContinueWith(TokenKind.OpenParen);
                var exitExpression = default(ExpressionSymbol);
                var closeParen = default(Terminal);

                if (openParen != default) {
                    exitExpression = ParseExpression();
                    closeParen = ContinueWithOrMissing(TokenKind.CloseParen);
                }

                return new GoToStatementSymbol(terminal, openParen, exitExpression, closeParen);
            }

            Unexpected();
            return null;
        }

        #endregion
        #region ParseUnitHead

        /// <summary>
        ///     parse an exception handler
        /// </summary>
        /// <returns></returns>

        [Rule("UnitHead", "'unit' NamespaceName { Hint } ';' ")]
        public UnitHeadSymbol ParseUnitHead()
            => new UnitHeadSymbol(
                ContinueWithOrMissing(TokenKind.Unit),
                ParseNamespaceName(),
                ParseHints(false),
                ContinueWithOrMissing(TokenKind.Semicolon));

        #endregion
        #region ParsePackage

        /// <summary>
        ///     parse a package
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>

        [Rule("Package", "PackageHead RequiresClause [ ContainsClause ] 'end' '.' ")]
        public PackageSymbol ParsePackage(FileReference path) {
            var packageHead = ParsePackageHead();
            var requiresClause = ParseRequiresClause();
            var containsClause = default(PackageContainsSymbol);

            if (Match(TokenKind.Contains))
                containsClause = ParseContainsClause();

            var endSymbol = ContinueWithOrMissing(TokenKind.End);
            var dotSymbol = ContinueWithOrMissing(TokenKind.Dot);

            return new PackageSymbol(packageHead, path, requiresClause, containsClause, endSymbol, dotSymbol);
        }

        #endregion
        #region ParseContainsClause

        /// <summary>
        ///     parse a package contains symbol
        /// </summary>
        /// <returns></returns>

        [Rule("ContainsClause", "'contains' NamespaceFileNameList")]
        public PackageContainsSymbol ParseContainsClause()
            => new PackageContainsSymbol(
                containsSymbol: ContinueWithOrMissing(TokenKind.Contains),
                containsList: ParseNamespaceFileNameList(),
                semicolon: ContinueWithOrMissing(TokenKind.Semicolon));

        #endregion
        #region ParseRequiresClause

        /// <summary>
        ///     parse a package requires symbol
        /// </summary>
        /// <returns></returns>

        [Rule("RequiresClause", "'requires' NamespaceNameList ';'")]
        public PackageRequiresSymbol ParseRequiresClause()
            => new PackageRequiresSymbol(
                requiresSymbol: ContinueWithOrMissing(TokenKind.Requires),
                requiresList: ParseNamespaceNameList(),
                semicolon: ContinueWithOrMissing(TokenKind.Semicolon));

        #endregion
        #region ParseNamespaceNameList

        /// <summary>
        ///     parse a namespace name list
        /// </summary>
        /// <returns></returns>

        [Rule("NamespaceNameList", "NamespaceName { ',' NamespaceName }")]
        public NamespaceNameListSymbol ParseNamespaceNameList() {
            using (var list = GetList<NamespaceNameSymbol>()) {
                var item = default(NamespaceNameSymbol);

                do {
                    item = AddToList(list, ParseNamespaceName(false, false, true));
                } while (item != default && item.Comma != default);

                return new NamespaceNameListSymbol(GetFixedArray(list));
            }
        }

        #endregion
        #region ParsePackageHead

        /// <summary>
        ///     parse a package head
        /// </summary>
        /// <returns></returns>

        [Rule("PackageHead", "'package' NamespaceName ';' ")]
        public PackageHeadSymbol ParsePackageHead()
            => new PackageHeadSymbol(
                packageSymbol: ContinueWithOrMissing(TokenKind.Package),
                packageName: ParseNamespaceName(),
                semicolon: ContinueWithOrMissing(TokenKind.Semicolon));

        #endregion
        #region ParseLibrary

        /// <summary>
        ///     parse a library declaration
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>

        [Rule("Library", "LibraryHead [UsesFileClause] Block '.' ")]
        public LibrarySymbol ParseLibrary(FileReference path)
            => new LibrarySymbol(
                libraryHead: ParseLibraryHead(),
                uses: Match(TokenKind.Uses) ? ParseUsesFileClause() : null,
                mainBlock: ParseBlock(),
                dot: ContinueWithOrMissing(TokenKind.Dot),
                filePath: path);

        #endregion
        #region ParseLibraryHead

        /// <summary>
        ///     parse a library head
        /// </summary>
        /// <returns></returns>

        [Rule("LibraryHead", "'library' NamespaceName Hints ';'")]
        public LibraryHeadSymbol ParseLibraryHead()
            => new LibraryHeadSymbol(
                librarySymbol: ContinueWithOrMissing(TokenKind.Library),
                libraryName: ParseNamespaceName(),
                hints: ParseHints(false),
                semicolon: ContinueWithOrMissing(TokenKind.Semicolon));

        #endregion
        #region ParseProgram

        /// <summary>
        ///     parse a program definition
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>

        [Rule("Program", "[ProgramHead] [UsesFileClause] Block '.'")]
        public ProgramSymbol ParseProgram(FileReference path)
            => new ProgramSymbol(
                programHead: Match(TokenKind.Program) ? ParseProgramHead() : null,
                uses: Match(TokenKind.Uses) ? ParseUsesFileClause() : null,
                mainBlock: ParseBlock(),
                dot: ContinueWithOrMissing(TokenKind.Dot),
                filePath: path);

        #endregion
        #region ParseProgramHead

        /// <summary>
        ///     parse a program head
        /// </summary>
        /// <returns></returns>

        [Rule("ProgramHead", "'program' NamespaceName [ProgramParams] ';'")]
        public ProgramHeadSymbol ParseProgramHead()
            => new ProgramHeadSymbol(
                programSymbol: ContinueWithOrMissing(TokenKind.Program),
                name: ParseNamespaceName(),
                parameters: ParseProgramParams(),
                semicolon: ContinueWithOrMissing(TokenKind.Semicolon));


        #endregion
        #region ParseProgramParams

        /// <summary>
        ///     parse program parameters
        /// </summary>
        /// <returns></returns>

        [Rule("ProgramParams", "'(' [ Identifier { ',' Identifier } ] ')'")]
        public ProgramParameterListSymbol ParseProgramParams() {
            var openParen = ContinueWith(TokenKind.OpenParen);

            if (openParen == default)
                return null;

            using (var list = GetList<ProgramParameterSymbol>()) {
                if (MatchIdentifier()) {
                    var param = default(ProgramParameterSymbol);

                    do {
                        var id = RequireIdentifier();
                        var comma = ContinueWith(TokenKind.Comma);
                        param = AddToList(list, new ProgramParameterSymbol(id, comma));
                    } while (param != default && param.Comma != default);
                }

                var closeParen = ContinueWithOrMissing(TokenKind.CloseParen);
                return new ProgramParameterListSymbol(openParen, GetFixedArray(list), closeParen);
            }
        }

        #endregion
        #region ParseBlock

        /// <summary>
        ///     parse a block symbol
        /// </summary>
        /// <returns></returns>

        [Rule("Block", "DeclarationSections [ BlockBody ] ")]
        public BlockSymbol ParseBlock() => new BlockSymbol(
                declarationSections: ParseDeclarationSections(),
                body: Match(TokenKind.Asm, TokenKind.Begin) ? ParseBlockBody() : null
            );

        #endregion
        #region ParseBlockBody

        /// <summary>
        ///     parse a block body
        /// </summary>
        /// <returns></returns>

        [Rule("BlockBody", "AssemblerBlock | CompoundStatement")]
        public BlockBodySymbol ParseBlockBody() => new BlockBodySymbol(
                assemblerBlock: Match(TokenKind.Asm) ? ParseAsmBlock() : default,
                body: Match(TokenKind.Begin) ? ParseCompoundStatement() : default
            );

        #endregion
        #region ParseAsmBlock

        /// <summary>
        ///     parse an assembler block
        /// </summary>
        /// <returns></returns>

        [Rule("AsmBlock", "'asm' { AssemblyStatement | PseudoOp } 'end'")]
        public AsmBlockSymbol ParseAsmBlock() {
            var asmSymbol = ContinueWithOrMissing(TokenKind.Asm);
            var endSymbol = default(Terminal);

            using (var list = GetList<SyntaxPartBase>()) {
                while (Tokenizer.HasNextToken) {

                    if (Match(TokenKind.End)) {
                        endSymbol = ContinueWithOrMissing(TokenKind.End);
                        break;
                    }

                    if (Match(TokenKind.Dot))
                        AddToList(list, ParseAsmPseudoOp());
                    else
                        AddToList(list, ParseAsmStatement());
                }

                return new AsmBlockSymbol(asmSymbol, endSymbol, GetFixedArray(list));
            }
        }

        #endregion
        #region ParseAsmPseudoOp

        /// <summary>
        ///     parse asm pseudo code
        /// </summary>
        /// <returns></returns>

        [Rule("PseudoOp", "( '.PARAMS ' Integer | '.PUSHENV' Register | '.SAVNENV' Register | '.NOFRAME'.")]
        public AsmPseudoOpSymbol ParseAsmPseudoOp() {
            var dot = ContinueWithOrMissing(TokenKind.Dot);
            var kind = CurrentToken().Value;
            var kindSymbol = RequireIdentifier();
            var mode = AsmPrefixSymbolKind.Unknown;
            var numberOfParams = default(StandardInteger);
            var register = default(IdentifierSymbol);

            if (string.Equals(kind, "params", StringComparison.OrdinalIgnoreCase)) {
                mode = AsmPrefixSymbolKind.ParamsOperation;
                numberOfParams = RequireInteger();
            }
            else if (string.Equals(kind, "pushenv", StringComparison.OrdinalIgnoreCase)) {
                mode = AsmPrefixSymbolKind.PushEnvOperation;
                register = RequireIdentifier();
            }
            else if (string.Equals(kind, "savenv", StringComparison.OrdinalIgnoreCase)) {
                mode = AsmPrefixSymbolKind.SaveEnvOperation;
                register = RequireIdentifier();
            }
            else if (string.Equals(kind, "noframe", StringComparison.OrdinalIgnoreCase)) {
                mode = AsmPrefixSymbolKind.NoFrame;
            }
            else {
                Unexpected();
            }

            return new AsmPseudoOpSymbol(dot, kindSymbol, mode, numberOfParams, register);
        }

        #endregion
        #region ParseAsmStatement

        /// <summary>
        ///     parse an assembler statement
        /// </summary>
        /// <returns></returns>

        [Rule("AssemblyStatement", "[AssemblyLabel ':'] [AssemblyPrefix] AssemblyOpcode [AssemblyOperand {','  AssemblyOperand}]")]
        public AsmStatementSymbol ParseAsmStatement() {
            var label = default(AsmLabelSymbol);
            var colonSymbol = default(Terminal);

            if (Match(TokenKind.At) || LookAhead(1, TokenKind.Colon)) {
                label = ParseAssemblyLabel();
                colonSymbol = ContinueWithOrMissing(TokenKind.Colon);
            };

            using (var list = GetList<AsmOperandSymbol>()) {
                var prefix = default(AsmPrefixSymbol);
                var opCode = default(AsmOpCodeSymbol);


                if (!Match(TokenKind.End)) {
                    prefix = ParseAssemblyPrefix();
                    opCode = ParseAssemblyOpcode();

                    while (Tokenizer.HasNextToken && !Match(TokenKind.End) && !Match(TokenKind.Semicolon) && Tokenizer.HasNextToken && !CurrentTokenIsAfterNewline()) {
                        AddToList(list, ParseAssemblyOperand(true));
                    }
                }

                return new AsmStatementSymbol(opCode, prefix, label, colonSymbol, GetFixedArray(list));
            }
        }

        #endregion
        #region ParseAssemblyOperand

        /// <summary>
        ///     asm operand
        /// </summary>
        /// <returns></returns>

        [Rule("AssemblyOperand", " AssemblyExpression ('and' | 'or' | 'xor') AssemblyOperand | ( 'not' AssemblyOperand ']' )")]
        public AsmOperandSymbol ParseAssemblyOperand(bool allowComma = false) {

            if (Match(TokenKind.Not))
                return new AsmOperandSymbol(
                    notSymbol: ContinueWithOrMissing(TokenKind.Not),
                    notExpression: ParseAssemblyOperand(),
                    comma: allowComma ? ContinueWith(TokenKind.Comma) : null);

            var leftTerm = ParseAssemblyExpression();
            var operand = default(Terminal);
            var rightTerm = default(AsmOperandSymbol);

            if (Match(TokenKind.And, TokenKind.Or, TokenKind.Xor)) {
                operand = ContinueWithOrMissing(TokenKind.And, TokenKind.Or, TokenKind.Xor);
                rightTerm = ParseAssemblyOperand();
            }

            return new AsmOperandSymbol(leftTerm, operand, rightTerm, allowComma ? ContinueWith(TokenKind.Comma) : null);
        }

        #endregion
        #region ParseAssemblyExpression

        /// <summary>
        ///     parse an assembly expression
        /// </summary>
        /// <returns></returns>

        [Rule("AssemblyExpression", " ('OFFSET' AssemblyOperand ) | ('TYPE' AssemblyOperand) | (('BYTE' | 'WORD' | 'DWORD' | 'QWORD' | 'TBYTE' ) PTR AssemblyOperand) | AssemblyTerm ('+' | '-' ) AssemblyOperand ")]
        public AsmExpressionSymbol ParseAssemblyExpression() {
            var tokenValue = CurrentToken().Value;
            if (MatchIdentifier()) {

                if (string.Equals(tokenValue, "OFFSET", StringComparison.OrdinalIgnoreCase)) {
                    var offsetSymbol = ContinueWith(TokenKind.Identifier);
                    var offset = ParseAssemblyOperand();
                    return new AsmExpressionSymbol(offsetSymbol, offset, false);
                }

                if (asmPtr.Contains(tokenValue)) {
                    var bytePtrKind = RequireIdentifier();
                    var bytePtr = ParseAssemblyOperand();
                    return new AsmExpressionSymbol(bytePtrKind, bytePtr);
                }
            }

            if (Match(TokenKind.TypeKeyword)) {
                var typeSymbol = ContinueWithOrMissing(TokenKind.TypeKeyword);
                var typeExpression = ParseAssemblyOperand();
                return new AsmExpressionSymbol(typeSymbol, typeExpression, true);
            }

            var leftOperand = ParseAssemblyTerm();
            var rightOperand = default(AsmOperandSymbol);
            var kind = TokenKind.Undefined;
            var operand = ContinueWith(TokenKind.Plus, TokenKind.Minus);

            if (operand != null) {
                kind = operand.Kind;
                rightOperand = ParseAssemblyOperand();
            }

            return new AsmExpressionSymbol(leftOperand, operand, rightOperand, kind);
        }

        #endregion
        #region ParseAssemblyTerm

        /// <summary>
        ///     parse an assembly term symbol
        /// </summary>
        /// <returns></returns>

        [Rule("AssemblyTerm", "AssemblyFactor [( '*' | '/' | 'mod' | 'shl' | 'shr' | '.' ) AssemblyOperand ]")]
        public AsmTermSymbol ParseAssemblyTerm() {

            var leftOperand = ParseAssemblyFactor();
            var dotSymbol = ContinueWith(TokenKind.Dot);
            var subtype = default(AsmOperandSymbol);
            var oprator = default(Terminal);
            var rightOperand = default(AsmOperandSymbol);

            if (dotSymbol != null) {
                subtype = ParseAssemblyOperand();
            }
            else if (Match(TokenKind.Times, TokenKind.Slash, TokenKind.Mod, TokenKind.Shl, TokenKind.Shr)) {
                oprator = ContinueWithOrMissing(TokenKind.Times, TokenKind.Slash, TokenKind.Mod, TokenKind.Shl, TokenKind.Shr);
                rightOperand = ParseAssemblyOperand();
            }

            return new AsmTermSymbol(leftOperand, dotSymbol, subtype, oprator, rightOperand);
        }

        #endregion
        #region ParseAssemblyFactor

        /// <summary>
        ///     asm factor
        /// </summary>
        /// <returns></returns>

        [Rule("AssemblyFactor", "(SegmentPrefix ':' AssemblyOperand) | '(' AssemblyOperand ')' | '[' AssemblyOperand ']' | Identifier | QuotedString | DoubleQuotedString | Integer | HexNumber ")]
        public AsmFactorSymbol ParseAssemblyFactor() {

            if (MatchIdentifier() && segmentPrefixes.Contains(CurrentToken().Value) && LookAhead(1, TokenKind.Colon)) {
                var segmentPrefix = RequireIdentifier();
                var colonSymbol = ContinueWithOrMissing(TokenKind.Colon);
                var segmentExpression = ParseAssemblyOperand();
                return new AsmFactorSymbol(segmentPrefix, colonSymbol, segmentExpression);
            }

            if (Match(TokenKind.OpenParen)) {
                var openParen = ContinueWithOrMissing(TokenKind.OpenParen);
                var subexpression = ParseAssemblyOperand();
                var closeParen = ContinueWithOrMissing(TokenKind.CloseParen);
                return new AsmFactorSymbol(openParen, subexpression, closeParen);
            }

            if (Match(TokenKind.OpenBraces)) {
                var openBraces = ContinueWithOrMissing(TokenKind.OpenBraces);
                var memorySubexpression = ParseAssemblyOperand();
                var closeBraces = ContinueWithOrMissing(TokenKind.CloseBraces);
                return new AsmFactorSymbol(openBraces, closeBraces, memorySubexpression);
            }

            if (MatchIdentifier(true))
                return new AsmFactorSymbol(RequireIdentifier(true));

            if (Match(TokenKind.Integer))
                return new AsmFactorSymbol(RequireInteger());

            if (Match(TokenKind.Real))
                return new AsmFactorSymbol(RequireRealValue());

            if (Match(TokenKind.HexNumber))
                return new AsmFactorSymbol(RequireHexValue());

            if (Match(TokenKind.QuotedString))
                return new AsmFactorSymbol(RequireString());

            if (Match(TokenKind.DoubleQuotedString))
                return new AsmFactorSymbol(RequireDoubleQuotedString());

            if (Match(TokenKind.At))
                return new AsmFactorSymbol(ParseLocalAsmLabel());

            Unexpected();
            return null;
        }

        #endregion
        #region ParseAssemblyOpcode

        /// <summary>
        ///     parse an opcode
        /// </summary>
        /// <returns></returns>

        [Rule("AssemblyOpCode", "Identifier [AssemblyDirective] ")]
        public AsmOpCodeSymbol ParseAssemblyOpcode() {
            if (Match(TokenKind.End))
                return null;

            return new AsmOpCodeSymbol(RequireIdentifier(true));
        }

        #endregion
        #region ParseAssemblyPrefix

        /// <summary>
        ///     asm prefix
        /// </summary>
        /// <returns></returns>

        [Rule("AssemblyPrefix", "(LockPrefix | [SegmentPrefix]) | (SegmentPrefix [LockPrefix])")]
        public AsmPrefixSymbol ParseAssemblyPrefix() {

            if (!MatchIdentifier())
                return null;

            var segmentPrefix = default(IdentifierSymbol);
            var lockPrefix = default(IdentifierSymbol);

            if (lockPrefixes.Contains(CurrentToken().Value)) {

                lockPrefix = RequireIdentifier();

                if (MatchIdentifier() && segmentPrefixes.Contains(CurrentToken().Value))
                    segmentPrefix = RequireIdentifier();

                return new AsmPrefixSymbol(lockPrefix, segmentPrefix);
            }

            if (segmentPrefixes.Contains(CurrentToken().Value)) {
                segmentPrefix = RequireIdentifier();

                if (MatchIdentifier() && lockPrefixes.Contains(CurrentToken().Value))
                    lockPrefix = RequireIdentifier();

                return new AsmPrefixSymbol(lockPrefix, segmentPrefix);
            }

            return null;
        }

        #endregion
        #region ParseAssemblyLabel

        /// <summary>
        ///     parse an asm label
        /// </summary>
        /// <returns></returns>

        [Rule("AsmLabel", "(Label | LocalAsmLabel { LocalAsmLabel } )")]
        public AsmLabelSymbol ParseAssemblyLabel() {
            if (Match(TokenKind.At))
                return new AsmLabelSymbol(ParseLocalAsmLabel());

            return new AsmLabelSymbol(ParseLabel());
        }

        #endregion
        #region ParseLocalAsmLabel

        /// <summary>
        ///     parse a local asm label
        /// </summary>
        /// <returns></returns>

        [Rule("LocalAsmLabel", "'@' { '@' | Integer | Identifier | HexNumber }")]
        public LocalAsmLabelSymbol ParseLocalAsmLabel() {
            var at = ContinueWithOrMissing(TokenKind.At);
            var wasAt = false;
            using (var list = GetList<SyntaxPartBase>()) {
                do {
                    wasAt = false;

                    if (Match(TokenKind.Integer)) {
                        AddToList(list, RequireInteger());
                    }
                    else if (MatchIdentifier(true)) {
                        AddToList(list, RequireIdentifier());
                    }
                    else if (Match(TokenKind.HexNumber)) {
                        AddToList(list, RequireHexValue());
                    }
                    else if (!Match(TokenKind.At)) {
                        Unexpected();
                    }

                    if (Match(TokenKind.At)) {
                        AddToList(list, ContinueWith(TokenKind.At));
                        wasAt = true;
                    }
                }

                while ((!CurrentTokenIsAfterNewline()) && wasAt);

                return new LocalAsmLabelSymbol(at, GetFixedArray(list));
            }
        }

        #endregion
        #region ParseDeclarationSections

        /// <summary>
        ///     test declaration sections
        /// </summary>
        /// <returns></returns>

        [Rule("DeclarationSection", "{ LabelDeclarationSection | ConstSection | TypeSection | VarSection | ExportsSection | AssemblyAttribute | MethodDecl | ProcedureDeclaration }", true)]
        public DeclarationsSymbol ParseDeclarationSections() {
            var stop = false;

            using (var list = GetList<SyntaxPartBase>()) {
                while (!stop) {

                    if (Match(TokenKind.Label)) {
                        AddToList(list, ParseLabelDeclarationSection());
                        continue;
                    }

                    if (Match(TokenKind.Const, TokenKind.Resourcestring)) {
                        AddToList(list, ParseConstSection(false));
                        continue;
                    }

                    if (Match(TokenKind.TypeKeyword)) {
                        AddToList(list, ParseTypeSection(false));
                        continue;
                    }

                    if (Match(TokenKind.Var, TokenKind.ThreadVar)) {
                        AddToList(list, ParseVarSection(false));
                        continue;
                    }

                    if (Match(TokenKind.Exports)) {
                        AddToList(list, ParseExportsSection());
                        continue;
                    }

                    UserAttributesSymbol attrs = null;
                    if (Match(TokenKind.OpenBraces)) {
                        attrs = ParseAttributes();
                    }

                    var classSymbol = ContinueWith(TokenKind.Class);

                    if (Match(TokenKind.Function, TokenKind.Procedure, TokenKind.Constructor, TokenKind.Destructor, TokenKind.Operator)) {

                        var useMethodDeclaration = //
                            classSymbol != default ||
                            Match(TokenKind.Constructor, TokenKind.Destructor, TokenKind.Operator) ||
                            (LookAhead(1, TokenKind.Identifier) && (LookAhead(2, TokenKind.Dot, TokenKind.AngleBracketsOpen)) ||
                            HasTokenBeforeToken(TokenKind.Dot, TokenKind.OpenParen, TokenKind.Colon, TokenKind.Semicolon, TokenKind.Begin, TokenKind.End, TokenKind.Comma));

                        if (useMethodDeclaration) {
                            AddToList(list, ParseMethodDecl(classSymbol, attrs));
                            continue;
                        }

                        AddToList(list, ParseProcedureDeclaration(attrs as UserAttributesSymbol));
                        continue;
                    }


                    stop = true;
                }

                return new DeclarationsSymbol(GetFixedArray(list));
            }
        }

        #endregion
        #region ParseMethodDecl

        /// <summary>
        ///     parse a method declaration
        /// </summary>
        /// <param name="classSymbol"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>

        [Rule("MethodDecl", "MethodDeclHeading ';' MethodDirectives [ Block ';' ]")]
        public MethodDeclarationSymbol ParseMethodDecl(Terminal classSymbol, UserAttributesSymbol attributes) {
            var heading = ParseMethodDeclHeading();
            var semicolon = ContinueWithOrMissing(TokenKind.Semicolon);
            var directives = ParseMethodDirectives();
            var methodBody = ParseBlock();
            var semicolon2 = default(Terminal);

            if ((methodBody != default) && (methodBody.Body != default))
                semicolon2 = ContinueWithOrMissing(TokenKind.Semicolon);

            return new MethodDeclarationSymbol(classSymbol, attributes, heading, semicolon, directives, methodBody, semicolon2);
        }

        #endregion
        #region ParseMethodDirectives

        /// <summary>
        ///     parse method directives
        /// </summary>
        /// <returns></returns>

        [Rule("MethodDirectives", "{ MethodDirective }")]
        public MethodDirectivesSymbol ParseMethodDirectives() {
            using (var list = GetList<SyntaxPartBase>()) {
                SyntaxPartBase directive;
                do {
                    directive = AddToList(list, ParseMethodDirective());
                } while (directive != null);

                return new MethodDirectivesSymbol(GetFixedArray(list));
            }
        }

        #endregion
        #region ParseMethodDirective

        [Rule("MethodDirective", "ReintroduceDirective | OverloadDirective | InlineDirective | BindingDirective | AbstractDirective | InlineDirective | CallConvention | HintingDirective | DispIdDirective")]
        private SyntaxPartBase ParseMethodDirective() {

            if (Match(TokenKind.Reintroduce))
                return ParseReintroduceDirective();

            if (Match(TokenKind.Overload))
                return ParseOverloadDirective();

            if (Match(TokenKind.Inline, TokenKind.Assembler))
                return ParseInlineDirective();

            if (Match(TokenKind.Message, TokenKind.Static, TokenKind.Dynamic, TokenKind.Override, TokenKind.Virtual))
                return ParseBindingDirective();

            if (Match(TokenKind.Abstract, TokenKind.Final))
                return ParseAbstractDirective();

            if (Match(TokenKind.Cdecl, TokenKind.Pascal, TokenKind.Register, TokenKind.Safecall, TokenKind.Stdcall, TokenKind.Export))
                return ParseCallConvention();

            if (Match(TokenKind.Deprecated, TokenKind.Library, TokenKind.Experimental, TokenKind.Platform))
                return ParseHint(true);

            if (Match(TokenKind.DispId))
                return ParseDispIdDirective();

            return null;
        }

        #endregion
        #region ParseInlineDirective

        /// <summary>
        ///     parse an inline function directive
        /// </summary>
        /// <returns></returns>

        [Rule("InlineDirective", "('inline' | 'assembler' ) ';'")]
        public InlineSymbol ParseInlineDirective() => new InlineSymbol(
                directive: ContinueWithOrMissing(TokenKind.Inline, TokenKind.Assembler),
                semicolon: ContinueWithOrMissing(TokenKind.Semicolon)
            );

        #endregion
        #region ParseCallConvention

        /// <summary>
        ///     parse a calling convention
        /// </summary>
        /// <returns></returns>

        [Rule("CallConvention", "('cdecl' | 'pascal' | 'register' | 'safecall' | 'stdcall' | 'export') ';' ")]
        public CallConventionSymbol ParseCallConvention() => new CallConventionSymbol(
                directive: ContinueWithOrMissing(TokenKind.Cdecl, TokenKind.Pascal, TokenKind.Register, TokenKind.Safecall, TokenKind.Stdcall, TokenKind.Export),
                semicolon: ContinueWithOrMissing(TokenKind.Semicolon)
            );

        #endregion
        #region ParseAbstractDirective

        /// <summary>
        ///     parse an abstract directive
        /// </summary>
        /// <returns></returns>

        [Rule("AbstractDirective", "('abstract' | 'final' ) ';' ")]
        public AbstractSymbol ParseAbstractDirective() => new AbstractSymbol(
                directive: ContinueWithOrMissing(TokenKind.Abstract, TokenKind.Final),
                semicolon: ContinueWithOrMissing(TokenKind.Semicolon)
            );

        #endregion
        #region ParseBindingDirective

        /// <summary>
        ///     parse a binding directive
        /// </summary>
        /// <returns></returns>

        [Rule("BindingDirective", " ('message' Expression ) | 'static' | 'dynamic' | 'override' | 'virtual' ")]
        public BindingSymbol ParseBindingDirective() {
            var directive = ContinueWithOrMissing(TokenKind.Message, TokenKind.Static, TokenKind.Dynamic, TokenKind.Override, TokenKind.Virtual);
            return new BindingSymbol(
                directive: directive,

                messageExpression: directive.Kind == TokenKind.Message ?
                    ParseExpression() :
                    null,

                semicolon: ContinueWithOrMissing(TokenKind.Semicolon)
            );
        }

        #endregion
        #region ParseOverloadDirective

        /// <summary>
        ///     parse an overload directive
        /// </summary>
        /// <returns></returns>

        [Rule("OverloadDirective", "'overload' ';' ")]
        public OverloadSymbol ParseOverloadDirective() => new OverloadSymbol(
                directive: ContinueWithOrMissing(TokenKind.Overload),
                semicolon: ContinueWithOrMissing(TokenKind.Semicolon));

        #endregion
        #region ParseReintroduceDirective

        /// <summary>
        ///     parse a reintroduce directive
        /// </summary>
        /// <returns></returns>

        [Rule("ReintroduceDirective", "'reintroduce' ';' ")]
        public ReintroduceSymbol ParseReintroduceDirective()
            => new ReintroduceSymbol(
                directive: ContinueWithOrMissing(TokenKind.Reintroduce),
                semicolon: ContinueWithOrMissing(TokenKind.Semicolon));


        #endregion
        #region MethodDeclarationHeading

        /// <summary>
        ///     parse a method declaration name
        /// </summary>
        /// <param name="allowIn"></param>
        /// <returns></returns>

        public MethodDeclarationNameSymbol ParseMethodDeclarationName(bool allowIn) {
            var namePart = ParseNamespaceName(allowIn);
            var genericPart = default(GenericDefinitionSymbol);
            var dot = default(Terminal);

            if (Match(TokenKind.AngleBracketsOpen)) {
                genericPart = ParseGenericDefinition();
            }

            dot = ContinueWith(TokenKind.Dot);
            return new MethodDeclarationNameSymbol(namePart, genericPart, dot);
        }

        /// <summary>
        ///     parse a method declaration heading
        /// </summary>
        /// <returns></returns>

        [Rule("MethodDeclHeading", " ('constructor' | 'destructor' | 'function' | 'procedure' | 'operator') NamespaceName [GenericDefinition] [FormalParameterSection] [':' Attributes TypeSpecification ]")]
        public MethodDeclarationHeadingSymbol ParseMethodDeclHeading() {
            var kindSymbol = ContinueWithOrMissing(TokenKind.Constructor, TokenKind.Destructor, TokenKind.Function, TokenKind.Procedure, TokenKind.Operator);
            var allowIn = kindSymbol.GetSymbolKind() == TokenKind.Operator;
            var name = default(MethodDeclarationNameSymbol);
            var parameters = default(FormalParameterSection);
            var colonSymbol = default(Terminal);
            var resultTypeAttributes = default(UserAttributesSymbol);
            var resultType = default(TypeSpecificationSymbol);

            using (var list = GetList<MethodDeclarationNameSymbol>()) {
                do {
                    name = AddToList(list, ParseMethodDeclarationName(allowIn));
                } while (name != default && name.Dot.GetSymbolKind() == TokenKind.Dot);

                if (Match(TokenKind.OpenParen))
                    parameters = ParseFormalParameterSection();

                colonSymbol = ContinueWith(TokenKind.Colon);

                if (colonSymbol != default) {
                    resultTypeAttributes = ParseAttributes();
                    resultType = ParseTypeSpecification();
                }

                return new MethodDeclarationHeadingSymbol(kindSymbol, GetFixedArray(list), parameters, colonSymbol, resultTypeAttributes, resultType);
            }
        }

        #endregion
        #region ParseProcedureDeclaration

        /// <summary>
        ///     parse a procedure heading symbol
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>

        [Rule("ProcedureDeclaration", "ProcedureDeclarationHeading ';' FunctionDirectives [ ProcBody ]")]
        public ProcedureDeclarationSymbol ParseProcedureDeclaration(UserAttributesSymbol attributes) {
            var heading = ParseProcedureDeclarationHeading();
            var semicolon = ContinueWithOrMissing(TokenKind.Semicolon);
            var directives = ParseFunctionDirectives();
            var body = ParseBlock();
            var semicolon2 = default(Terminal);

            if ((body != default) && (body.Body != default))
                semicolon2 = ContinueWithOrMissing(TokenKind.Semicolon);

            return new ProcedureDeclarationSymbol(attributes, heading, semicolon, directives, body, semicolon2);
        }

        #endregion
        #region ParseProcedureDeclarationHeading

        /// <summary>
        ///     parse a procedure declaration heading
        /// </summary>
        /// <returns></returns>

        [Rule("ProcedureDeclarationHeading", "('procedure' | 'function') Identifier [FormalParameterSection][':' TypeSpecification]")]
        public ProcedureDeclarationHeadingSymbol ParseProcedureDeclarationHeading() {
            var kindSymbol = ContinueWithOrMissing(TokenKind.Function, TokenKind.Procedure);
            var name = RequireIdentifier();
            var parameters = default(FormalParameterSection);
            var colonSymbol = default(Terminal);
            var resultTypeAttributes = default(UserAttributesSymbol);
            var resultType = default(TypeSpecificationSymbol);

            if (Match(TokenKind.OpenParen)) {
                parameters = ParseFormalParameterSection();
            }

            colonSymbol = ContinueWith(TokenKind.Colon);

            if (colonSymbol != default) {
                resultTypeAttributes = ParseAttributes();
                resultType = ParseTypeSpecification();
            }

            return new ProcedureDeclarationHeadingSymbol(kindSymbol, name, parameters, colonSymbol, resultTypeAttributes, resultType);
        }

        #endregion
        #region ParseExportsSection

        /// <summary>
        ///     parse a exports section
        /// </summary>
        /// <returns></returns>

        [Rule("ExportsSection", "'exports' ExportItem { ',' ExportItem } ';' ")]
        public ExportsSectionSymbol ParseExportsSection() {
            var exports = ContinueWith(TokenKind.Exports);
            using (var list = GetList<ExportItemSymbol>()) {
                var item = default(ExportItemSymbol);
                do {
                    item = AddToList(list, ParseExportItem(true));
                } while (item != default && item.Comma.GetSymbolKind() == TokenKind.Comma);

                var semicolon = ContinueWithOrMissing(TokenKind.Semicolon);
                return new ExportsSectionSymbol(exports, GetFixedArray(list), semicolon);
            }
        }

        #endregion
        #region ParseExportItem

        /// <summary>
        ///     parse an export item
        /// </summary>
        /// <param name="allowComma"></param>
        /// <returns></returns>

        [Rule("ExportItem", " Identifier [ '(' FormalParameters ')' ] [ 'index' Expression ] [ 'name' Expression ]")]
        public ExportItemSymbol ParseExportItem(bool allowComma) {
            var exportName = RequireIdentifier();
            var openParen = default(Terminal);
            var parameters = default(FormalParametersSymbol);
            var closeParen = default(Terminal);
            var indexSymbol = default(Terminal);
            var indexParameter = default(ExpressionSymbol);
            var nameSymbol = default(Terminal);
            var nameParameter = default(ExpressionSymbol);
            var resident = default(Terminal);
            var comma = default(Terminal);

            openParen = ContinueWith(TokenKind.OpenParen);
            if (openParen != default) {
                parameters = ParseFormalParameters();
                closeParen = ContinueWithOrMissing(TokenKind.CloseParen);
            }

            indexSymbol = ContinueWith(TokenKind.Index);
            if (indexSymbol != default) {
                indexParameter = ParseExpression();
            }

            nameSymbol = ContinueWith(TokenKind.Name);
            if (nameSymbol != default) {
                nameParameter = ParseExpression();
            }

            resident = ContinueWith(TokenKind.Resident);

            if (allowComma)
                comma = ContinueWith(TokenKind.Comma);

            return new ExportItemSymbol(exportName, openParen, parameters, closeParen, indexSymbol, indexParameter, nameSymbol, nameParameter, resident, comma);
        }

        #endregion
        #region ParseVarSection

        [Rule("VarSection", "(var | threadvar) VarDeclaration { VarDeclaration }")]
        private VarSection ParseVarSection(bool inClassDeclaration) {
            var varSymbol = ContinueWithOrMissing(TokenKind.Var, TokenKind.ThreadVar);
            using (var list = GetList<VarDeclaration>()) {
                do {
                    AddToList(list, ParseVarDeclaration());
                } while ((!inClassDeclaration || !Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Automated, TokenKind.Strict)) && MatchIdentifier(TokenKind.OpenBraces));

                return new VarSection(varSymbol, GetFixedArray(list));
            }
        }

        #endregion
        #region ParseVarDeclaration

        [Rule("VarDeclaration", " IdentList ':' TypeSpecification [ VarValueSpecification ] Hints ';' ")]
        private VarDeclaration ParseVarDeclaration() {
            var attributes = ParseAttributes();
            var identifiers = ParseIdentList(false);
            var colonSymbol = ContinueWithOrMissing(TokenKind.Colon);
            var typeDeclaration = ParseTypeSpecification(false, true);
            var value = default(VarValueSpecification);

            if (Match(TokenKind.Absolute, TokenKind.EqualsSign)) {
                value = ParseValueSpecification();
            }

            var hints = ParseHints(false);
            var semicolon = ContinueWithOrMissing(TokenKind.Semicolon);
            return new VarDeclaration(attributes, identifiers, colonSymbol, typeDeclaration, value, hints, semicolon);
        }

        #endregion
        #region ParseValueSpecification

        [Rule("VarValueSpecification", "('absolute' ConstExpression) | ('=' ConstExpression)")]
        private VarValueSpecification ParseValueSpecification() {
            var symbol = ContinueWith(TokenKind.EqualsSign, TokenKind.Absolute);
            var expression = ParseConstantExpression();
            return new VarValueSpecification(symbol, expression);
        }

        #endregion
        #region ParseLabelDeclarationSection

        [Rule("LabelSection", "'label' Label { ',' Label } ';' ")]
        private LabelDeclarationSection ParseLabelDeclarationSection() {
            using (var list = GetList<LabelSymbol>()) {
                var labelSymbol = ContinueWithOrMissing(TokenKind.Label);
                var label = default(LabelSymbol);
                do {
                    label = AddToList(list, ParseLabel(true));
                } while (label != default && label.Comma.GetSymbolKind() == TokenKind.Comma);

                return new LabelDeclarationSection(labelSymbol, GetFixedArray(list), ContinueWithOrMissing(TokenKind.Semicolon));
            }
        }

        #endregion
        #region ParseLabel

        /// <summary>
        ///     parse a label symbol
        /// </summary>
        /// <param name="allowComma"></param>
        /// <returns></returns>

        [Rule("Label", "Identifier | Integer | HexNumber")]
        public LabelSymbol ParseLabel(bool allowComma = false) {

            if (MatchIdentifier())
                return new LabelSymbol(RequireIdentifier(), allowComma ? ContinueWith(TokenKind.Comma) : default);

            if (Match(TokenKind.Integer))
                return new LabelSymbol(RequireInteger(), allowComma ? ContinueWith(TokenKind.Comma) : default);

            if (Match(TokenKind.HexNumber))
                return new LabelSymbol(RequireHexValue(), allowComma ? ContinueWith(TokenKind.Comma) : default);

            Unexpected();
            return null;
        }

        #endregion
        #region ParseConstDeclaration

        /// <summary>
        ///     parse a constant declaration
        /// </summary>
        /// <returns></returns>

        [Rule("ConstDeclaration", "[Attributes] Identifier [ ':' TypeSpecification ] = ConstantExpression Hints';'")]
        public ConstDeclarationSymbol ParseConstDeclaration() {
            var attributes = ParseAttributes();
            var identifier = RequireIdentifier();
            var colon = ContinueWith(TokenKind.Colon);
            var typeSpecification = default(TypeSpecificationSymbol);

            if (colon != default) {
                typeSpecification = ParseTypeSpecification(true);
            }

            var equalsSign = ContinueWithOrMissing(TokenKind.EqualsSign);
            var value = ParseConstantExpression();
            var hint = ParseHints(false);
            var semicolon = ContinueWithOrMissing(TokenKind.Semicolon);

            return new ConstDeclarationSymbol(attributes, identifier, colon, typeSpecification, equalsSign, value, hint, semicolon);
        }

        #endregion
        #region ParseHints

        /// <summary>
        ///     parse hints
        /// </summary>
        /// <param name="requireSemicolon"></param>
        /// <returns></returns>

        [Rule("Hints", " { Hint ';' }")]
        public HintingInformationListSymbol ParseHints(bool requireSemicolon) {
            using (var list = GetList<HintSymbol>()) {
                var hint = default(HintSymbol);

                do {
                    hint = AddToList(list, ParseHint(requireSemicolon));
                } while (hint != default);

                if (list.Item.Count > 0)
                    return new HintingInformationListSymbol(GetFixedArray(list));
            }

            return null;
        }

        #endregion
        #region ParseHint

        /// <summary>
        ///     parse a hint symbol
        /// </summary>
        /// <returns></returns>

        [Rule("Hint", " ('deprecated' [QuotedString] | 'experimental' | 'platform' | 'library' ) ")]
        public HintSymbol ParseHint(bool requireSemicolon) {
            var symbol = ContinueWith(TokenKind.Deprecated);
            var semicolon = default(Terminal);

            if (symbol != default) {
                var deprecatedComment = default(QuotedStringSymbol);

                if (Match(TokenKind.QuotedString))
                    deprecatedComment = RequireString();

                if (requireSemicolon)
                    semicolon = ContinueWithOrMissing(TokenKind.Semicolon);

                return new HintSymbol(symbol, deprecatedComment, semicolon);
            }

            symbol = ContinueWith(TokenKind.Experimental, TokenKind.Platform, TokenKind.Library);
            if (symbol != default) {
                if (requireSemicolon)
                    semicolon = ContinueWithOrMissing(TokenKind.Semicolon);

                return new HintSymbol(symbol, semicolon);
            }

            return null;
        }

        #endregion
        #region ParseTypeSpecification

        /// <summary>
        ///     parse a type specification
        /// </summary>
        /// <param name="constDeclaration"></param>
        /// <param name="varDeclaration"></param>
        /// <param name="allowComma"></param>
        /// <returns></returns>

        [Rule("TypeSpecification", "StructType | PointerType | StringType | ProcedureType | SimpleType ")]
        public TypeSpecificationSymbol ParseTypeSpecification(bool constDeclaration = false, bool varDeclaration = false, bool allowComma = false) {
            var comma = default(Terminal);

            if (Match(TokenKind.Packed, TokenKind.Array, TokenKind.Set, TokenKind.File, //
                TokenKind.Class, TokenKind.Interface, TokenKind.Record, TokenKind.Object, TokenKind.DispInterface)) {
                var structuredType = ParseStructType();
                if (allowComma)
                    comma = ContinueWith(TokenKind.Comma);
                return new TypeSpecificationSymbol(structuredType, comma);
            }

            if (Match(TokenKind.Pointer, TokenKind.Circumflex)) {
                var pointerType = ParsePointerType();
                if (allowComma)
                    comma = ContinueWith(TokenKind.Comma);
                return new TypeSpecificationSymbol(pointerType, comma);
            }

            if (Match(TokenKind.String, TokenKind.ShortString, TokenKind.AnsiString, TokenKind.UnicodeString, TokenKind.WideString)) {
                var stringType = ParseStringType();
                if (allowComma)
                    comma = ContinueWith(TokenKind.Comma);
                return new TypeSpecificationSymbol(stringType, comma);
            }

            if (Match(TokenKind.Function, TokenKind.Procedure)) {
                var procedureType = ParseProcedureType();
                if (allowComma)
                    comma = ContinueWith(TokenKind.Comma);
                return new TypeSpecificationSymbol(procedureType, comma);
            }

            if (Match(TokenKind.Reference) && LookAhead(1, TokenKind.To)) {
                var procedureType = ParseProcedureType();
                if (allowComma)
                    comma = ContinueWith(TokenKind.Comma);
                return new TypeSpecificationSymbol(procedureType, comma);
            }

            var simpleType = ParseSimpleType(constDeclaration, varDeclaration);
            if (allowComma)
                comma = ContinueWith(TokenKind.Comma);

            return new TypeSpecificationSymbol(simpleType, comma);
        }

        #endregion
        #region GenericNamespaceName

        /// <summary>
        ///     parse a generic namespace name
        /// </summary>
        /// <param name="advancedCheck"></param>
        /// <param name="inDesignator"></param>
        /// <param name="allowDot"></param>
        /// <returns></returns>

        [Rule("GenericNamespaceName", "NamespaceName [ GenericSuffix ]")]
        public GenericNamespaceNameSymbol ParseGenericNamespaceName(bool advancedCheck = false, bool inDesignator = false, bool allowDot = false) {
            var name = ParseNamespaceName(false, inDesignator);
            var genericPart = default(GenericSuffixSymbol);

            if (Match(TokenKind.AngleBracketsOpen)) {
                if (!advancedCheck) {
                    genericPart = ParseGenericSuffix();
                }
                else if (LookAheadIdentifier(1, new[] { TokenKind.String, TokenKind.ShortString, TokenKind.WideString, TokenKind.UnicodeString, TokenKind.AnsiString, TokenKind.Pointer }, false)) {
                    var whereCloseBrackets = HasTokenUntilToken(new[] { TokenKind.AngleBracketsClose }, TokenKind.Identifier, TokenKind.Dot, TokenKind.Comma, TokenKind.AngleBracketsOpen, TokenKind.String, TokenKind.ShortString, TokenKind.WideString, TokenKind.UnicodeString, TokenKind.AnsiString, TokenKind.Pointer);
                    if (whereCloseBrackets.Item1 && (!LookAheadIdentifier(1 + whereCloseBrackets.Item2, new[] { TokenKind.HexNumber, TokenKind.Integer, TokenKind.Real }, false) || LookAhead(1 + whereCloseBrackets.Item2, TokenKind.Read, TokenKind.Write, TokenKind.ReadOnly, TokenKind.WriteOnly, TokenKind.Add, TokenKind.Remove, TokenKind.DispId))) {
                        genericPart = ParseGenericSuffix();
                    }
                }
            }

            var dot = default(Terminal);

            if (allowDot && (!inDesignator || !IsLastDisignatorPart()))
                dot = ContinueWith(TokenKind.Dot);

            return new GenericNamespaceNameSymbol(name, genericPart, dot);
        }

        #endregion
        #region EnumType

        /// <summary>
        ///     parse an enumeration type
        /// </summary>
        /// <returns></returns>

        [Rule("EnumType", "'(' EnumTypeValue { ',' EnumTypeValue } ')'")]
        public EnumTypeDefinitionSymbol ParseEnumType() {
            using (var list = GetList<EnumValueSymbol>()) {
                var openParen = ContinueWithOrMissing(TokenKind.OpenParen);
                var item = default(EnumValueSymbol);
                do {
                    item = AddToList(list, ParseEnumTypeValue());
                } while (item != default && item.Comma != default);

                var closeParen = ContinueWithOrMissing(TokenKind.CloseParen);
                return new EnumTypeDefinitionSymbol(openParen, GetFixedArray(list), closeParen);
            }
        }

        #endregion
        #region ParseEnumTypeValue

        /// <summary>
        ///     parse an enumeration value
        /// </summary>
        /// <returns></returns>

        [Rule("EnumTypeValue", "Identifier [ '=' Expression ]")]
        public EnumValueSymbol ParseEnumTypeValue() {
            var enumName = RequireIdentifier();
            var equals = ContinueWith(TokenKind.EqualsSign);
            var value = default(ExpressionSymbol);

            if (equals != default)
                value = ParseExpression();

            var comma = ContinueWith(TokenKind.Comma);
            return new EnumValueSymbol(enumName, equals, value, comma);
        }

        #endregion
        #region ParseProcedureType

        /// <summary>
        ///     parse a procedural type
        /// </summary>
        /// <returns></returns>

        [Rule("ProcedureType", "(ProcedureRefType [ 'of' 'object' ] ( | ProcedureReference")]
        public ProcedureTypeSymbol ParseProcedureType() {

            if (Match(TokenKind.Procedure, TokenKind.Function)) {
                var refType = ParseProcedureRefType(false);
                var ofSymbol = ContinueWith(TokenKind.Of);
                var objectSymbol = default(Terminal);

                if (ofSymbol != default)
                    objectSymbol = ContinueWithOrMissing(TokenKind.Object);

                return new ProcedureTypeSymbol(refType, ofSymbol, objectSymbol);
            }

            if (Match(TokenKind.Reference))
                return new ProcedureTypeSymbol(ParseProcedureReference());

            Unexpected();
            return null;
        }

        #endregion
        #region  ParseProcedureReference

        /// <summary>
        ///     parse a procedure reference
        /// </summary>
        /// <returns></returns>

        [Rule("ProcedureReference", "'reference' 'to' ProcedureTypeDefinition ")]
        public ProcedureReferenceSymbol ParseProcedureReference() {
            var reference = ContinueWithOrMissing(TokenKind.Reference);
            var toSymbol = ContinueWithOrMissing(TokenKind.To);
            var proceduralType = ParseProcedureRefType(true);
            return new ProcedureReferenceSymbol(reference, toSymbol, proceduralType);
        }

        #endregion
        #region ParseProcedureRefType

        /// <summary>
        ///     parse a procedure reference type
        /// </summary>
        /// <param name="allowAnonymousMethods"></param>
        /// <returns></returns>

        [Rule("ProcedureTypeDefinition", "('function' | 'procedure') [ '(' FormalParameters ')' ] [ ':' TypeSpecification ] [ 'of' 'object']")]
        public ProcedureTypeDefinitionSymbol ParseProcedureRefType(bool allowAnonymousMethods) {
            var kindSymbol = ContinueWithOrMissing(TokenKind.Function, TokenKind.Procedure);
            var parameters = default(FormalParameterSection);
            var colonSymbol = default(Terminal);
            var attributes = default(UserAttributesSymbol);
            var returnType = default(TypeSpecificationSymbol);

            if (Match(TokenKind.OpenParen))
                parameters = ParseFormalParameterSection();

            if (Match(TokenKind.Colon)) {
                colonSymbol = ContinueWithOrMissing(TokenKind.Colon);
                attributes = ParseAttributes();
                returnType = ParseTypeSpecification();
            }

            return new ProcedureTypeDefinitionSymbol(kindSymbol, parameters, colonSymbol, attributes, returnType, allowAnonymousMethods);
        }

        #endregion
        #region FormalParameterSecion

        [Rule("FormalParameterSection", "'(' [ FormalParameters ] ')'")]
        private FormalParameterSection ParseFormalParameterSection() {
            var openParen = ContinueWithOrMissing(TokenKind.OpenParen);
            var parameters = default(FormalParametersSymbol);

            if (!Match(TokenKind.CloseParen))
                parameters = ParseFormalParameters();

            var closeParen = ContinueWithOrMissing(TokenKind.CloseParen);
            return new FormalParameterSection(openParen, parameters, closeParen);
        }

        #endregion
        #region ParseStringType

        /// <summary>
        ///     parse a string type
        /// </summary>
        /// <returns></returns>

        [Rule("StringType", "ShortString | WideString | UnicodeString |('string' [ '[' Expression ']'  ]) | ('AnsiString' '(' ConstExpression ')') ")]
        public StringTypeSymbol ParseStringType() {

            if (Match(TokenKind.String)) {
                var stringSymbol = ContinueWith(TokenKind.String);
                var openBraces = ContinueWith(TokenKind.OpenBraces);
                var stringLength = default(ConstantExpressionSymbol);
                var closeBraces = default(Terminal);

                if (openBraces != default) {
                    stringLength = ParseConstantExpression();
                    closeBraces = ContinueWithOrMissing(TokenKind.CloseBraces);
                };
                return new StringTypeSymbol(stringSymbol, openBraces, stringLength, closeBraces);
            }

            if (Match(TokenKind.AnsiString)) {
                var stringSymbol = ContinueWith(TokenKind.AnsiString);
                var openParen = ContinueWith(TokenKind.OpenParen);
                var codePage = default(ConstantExpressionSymbol);
                var closeParen = default(Terminal);

                if (openParen != default) {
                    codePage = ParseConstantExpression();
                    closeParen = ContinueWithOrMissing(TokenKind.CloseParen);
                }
                return new StringTypeSymbol(stringSymbol, openParen, codePage, closeParen);
            }

            if (Match(TokenKind.ShortString, TokenKind.WideString, TokenKind.UnicodeString)) {
                return new StringTypeSymbol(ContinueWith(TokenKind.ShortString, TokenKind.WideString, TokenKind.UnicodeString));
            }

            Unexpected();
            return null;
        }

        #endregion

        #region ParseClassDeclaration

        /// <summary>
        ///     parse a class declaration
        /// </summary>
        /// <returns></returns>

        [Rule("ClassDeclaration", "ClassOfDeclaration | ClassDefinition | ClassHelper | InterfaceDef | ObjectDecl | RecordDecl | RecordHelperDecl ")]
        public ClassTypeDeclarationSymbol ParseClassDeclaration() {

            if (Match(TokenKind.Class) && LookAhead(1, TokenKind.Of))
                return new ClassTypeDeclarationSymbol(ParseClassOfDeclaration());

            if (Match(TokenKind.Class) && LookAhead(1, TokenKind.Helper))
                return new ClassTypeDeclarationSymbol(ParseClassHelper());

            if (Match(TokenKind.Class))
                return new ClassTypeDeclarationSymbol(ParseClassDefinition());

            if (Match(TokenKind.Interface, TokenKind.DispInterface))
                return new ClassTypeDeclarationSymbol(ParseInterfaceDef());

            if (Match(TokenKind.Object))
                return new ClassTypeDeclarationSymbol(ParseObjectDecl());

            if (Match(TokenKind.Record) && LookAhead(1, TokenKind.Helper))
                return new ClassTypeDeclarationSymbol(ParseRecordHelper());

            if (Match(TokenKind.Record))
                return new ClassTypeDeclarationSymbol(ParseRecordDecl());

            Unexpected();
            return null;
        }

        #endregion
        #region ParseRecordDecl

        /// <summary>
        ///     parse a record declaration
        /// </summary>
        /// <returns></returns>

        [Rule("RecordDecl", "'record' RecordFieldList (RecordVariantSection | RecordItems ) 'end' ")]
        public RecordDeclarationSymbol ParseRecordDecl() {
            var recordSymbol = ContinueWithOrMissing(TokenKind.Record);
            var fieldList = default(RecordFieldListSymbol);
            var variantSection = default(RecordVariantSectionSymbol);
            var items = default(RecordItemsSymbol);

            if (MatchIdentifier() && !Match(TokenKind.Strict, TokenKind.Protected, TokenKind.Private, TokenKind.Public, TokenKind.Published, TokenKind.Automated)) {
                fieldList = ParseRecordFieldList(true);
            }

            if (Match(TokenKind.Case)) {
                variantSection = ParseRecordVariantSection();
            }
            else {
                items = ParseRecordItems();
            }

            var endSymbol = ContinueWithOrMissing(TokenKind.End);
            return new RecordDeclarationSymbol(recordSymbol, fieldList, variantSection, items, endSymbol);
        }

        #endregion
        #region ParseRecordItems

        /// <summary>
        ///     parse record items
        /// </summary>
        /// <returns></returns>

        [Rule("RecordItems", "{ RecordItem }")]
        public RecordItemsSymbol ParseRecordItems() {
            using (var list = GetList<RecordItemSymbol>()) {
                var mode = RecordDeclarationMode.Fields;

                while ((!Match(TokenKind.End)) && (mode != RecordDeclarationMode.Undefined)) {
                    AddToList(list, ParseRecordItem(ref mode));

                    if (mode == RecordDeclarationMode.Undefined && list.Item.Count > 0) {
                        Unexpected();
                        break;
                    }
                }

                return new RecordItemsSymbol(GetFixedArray(list));
            }
        }

        #endregion
        #region ParseRecordItem

        /// <summary>
        ///     parse a record item
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>

        [Rule("RecordItem", "MethodDeclaration | PropertyDeclaration | ConstSection | TypeSection | RecordField | ( ['class'] VarSection) | Visibility ")]
        public RecordItemSymbol ParseRecordItem(ref RecordDeclarationMode mode) {

            if (Match(TokenKind.Var)) {
                mode = RecordDeclarationMode.Fields;
                return new RecordItemSymbol(ContinueWith(TokenKind.Var));
            }

            if (Match(TokenKind.Class) && LookAhead(1, TokenKind.Var)) {
                mode = RecordDeclarationMode.ClassFields;
                return new RecordItemSymbol(ContinueWith(TokenKind.Class), ContinueWith(TokenKind.Var));
            }

            var attributes1 = default(UserAttributesSymbol);
            var attributes2 = default(UserAttributesSymbol);
            var classItem = default(Terminal);

            if (Match(TokenKind.OpenBraces)) {
                attributes1 = ParseAttributes();
            }

            classItem = ContinueWith(TokenKind.Class);

            if (Match(TokenKind.OpenBraces)) {
                attributes2 = ParseAttributes();
            }

            if (Match(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Strict, TokenKind.Published, TokenKind.Automated)) {
                if (classItem != default) {
                    Unexpected();
                }

                var strict = ContinueWith(TokenKind.Strict);
                var visibility = ContinueWith(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Published, TokenKind.Automated);
                mode = RecordDeclarationMode.Fields;
                return new RecordItemSymbol(strict, visibility);
            }

            if (Match(TokenKind.Procedure, TokenKind.Function, TokenKind.Constructor, TokenKind.Destructor, TokenKind.Operator)) {
                mode = RecordDeclarationMode.Other;
                return new RecordItemSymbol(ParseMethodDeclaration(), classItem);
            }

            if (Match(TokenKind.Property)) {
                mode = RecordDeclarationMode.Other;
                return new RecordItemSymbol(ParsePropertyDeclaration());
            }

            if (classItem == default && Match(TokenKind.Case))
                return new RecordItemSymbol(ParseRecordVariantSection());

            if (classItem == default && Match(TokenKind.Const))
                return new RecordItemSymbol(ParseConstSection(true));

            if (classItem == default && Match(TokenKind.TypeKeyword))
                return new RecordItemSymbol(ParseTypeSection(true));

            if (MatchIdentifier() && (!Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Automated, TokenKind.Strict))) {
                if (mode == RecordDeclarationMode.Fields || mode == RecordDeclarationMode.ClassFields) {
                    return new RecordItemSymbol(ParseRecordFieldList(true), attributes1, attributes2);
                }

                Unexpected();
            }

            mode = RecordDeclarationMode.Undefined;
            return null;
        }

        #endregion
        #region ParseRecordVariantSection

        /// <summary>
        ///     parse a record variant section
        /// </summary>
        /// <returns></returns>

        [Rule("RecordVariantSection", "'case' [ Identifier ': ' ] TypeDeclaration 'of' { RecordVariant } ")]
        public RecordVariantSectionSymbol ParseRecordVariantSection() {
            using (var list = GetList<RecordVariantSymbol>()) {
                var caseSymbol = ContinueWith(TokenKind.Case);
                var variantName = default(IdentifierSymbol);
                var colonSymbol = default(Terminal);

                if (MatchIdentifier() && LookAhead(1, TokenKind.Colon)) {
                    variantName = RequireIdentifier();
                    colonSymbol = ContinueWithOrMissing(TokenKind.Colon);
                }
                var typeDeclaration = ParseTypeSpecification();
                var ofSymbol = ContinueWithOrMissing(TokenKind.Of);
                var item = default(RecordVariantSymbol);

                do {
                    if (Match(TokenKind.End, TokenKind.Undefined))
                        item = default;
                    else
                        item = AddToList(list, ParseRecordVariant());

                } while (item != default && item.Semicolon != default);

                return new RecordVariantSectionSymbol(caseSymbol, variantName, colonSymbol, typeDeclaration, ofSymbol, GetFixedArray(list));
            }
        }

        #endregion
        #region ParseRecordVariant

        /// <summary>
        ///     parse a record variant
        /// </summary>
        /// <returns></returns>

        [Rule("RecordVariant", "ConstantExpression { , ConstantExpression } : '(' FieldList ')' ';' ")]
        public RecordVariantSymbol ParseRecordVariant() {
            using (var list = GetList<ConstantExpressionSymbol>()) {
                var item = default(ConstantExpressionSymbol);

                do {
                    item = AddToList(list, ParseConstantExpression(false, false, true));
                } while (item != default && item.Comma != default);

                var colonSymbol = ContinueWithOrMissing(TokenKind.Colon);
                var openParen = ContinueWithOrMissing(TokenKind.OpenParen);
                var fieldList = ParseRecordFieldList(false);
                var variantSection = default(RecordVariantSectionSymbol);

                if (Match(TokenKind.Case)) {
                    variantSection = ParseRecordVariantSection();
                }

                var closeParen = ContinueWithOrMissing(TokenKind.CloseParen);
                var semicolon = ContinueWith(TokenKind.Semicolon);

                return new RecordVariantSymbol(GetFixedArray(list), colonSymbol, openParen, fieldList, variantSection, closeParen, semicolon);
            }
        }

        #endregion
        #region ParseRecordFieldList

        /// <summary>
        ///     parse record field list
        /// </summary>
        /// <param name="requireSemicolon"></param>
        /// <returns></returns>

        [Rule("RecordFieldList", " { RecordField } ")]
        public RecordFieldListSymbol ParseRecordFieldList(bool requireSemicolon) {
            using (var list = GetList<RecordFieldSymbol>()) {
                while (MatchIdentifier() && (!Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Strict))) {
                    AddToList(list, ParseRecordField(requireSemicolon));
                }
                return new RecordFieldListSymbol(GetFixedArray(list));
            }
        }

        #endregion
        #region ParseRecordField

        /// <summary>
        ///     parse a record field
        /// </summary>
        /// <param name="requireSemicolon"></param>
        /// <returns></returns>

        [Rule("RecordField", "IdentList ':' TypeSpecification Hints ';'")]
        public RecordFieldSymbol ParseRecordField(bool requireSemicolon) {
            var names = ParseIdentList(true);
            var colonSymbol = ContinueWithOrMissing(TokenKind.Colon);
            var fieldType = ParseTypeSpecification();
            var hint = ParseHints(false);
            var semicolon = default(Terminal);

            if (Match(TokenKind.Semicolon) || (requireSemicolon && (!Match(TokenKind.End))))
                semicolon = ContinueWithOrMissing(TokenKind.Semicolon);

            return new RecordFieldSymbol(names, colonSymbol, fieldType, hint, semicolon);
        }

        #endregion
        #region ParseRecordHelper

        /// <summary>
        ///     parse a record helper symbol
        /// </summary>
        /// <returns></returns>

        [Rule("RecordHelperDecl", "'record' 'helper' 'for' TypeName RecordHelperItems 'end'")]
        public RecordHelperDefinitionSymbol ParseRecordHelper() {
            var recordSymbol = ContinueWithOrMissing(TokenKind.Record);
            var helperSymbol = ContinueWithOrMissing(TokenKind.Helper);
            var forSymbol = ContinueWithOrMissing(TokenKind.For);
            var name = ParseTypeName();
            var items = ParseRecordHelperItems();
            var endSymbol = ContinueWithOrMissing(TokenKind.End);

            return new RecordHelperDefinitionSymbol(recordSymbol, helperSymbol, forSymbol, name, items, endSymbol);
        }

        #endregion
        #region ParseRecordHelperItems

        /// <summary>
        ///     parse record helper items
        /// </summary>
        /// <returns></returns>

        [Rule("RecordHelperItems", " { RecordHelperItem }")]
        public RecordHelperItemsSymbol ParseRecordHelperItems() {
            using (var list = GetList<RecordHelperItemSymbol>()) {
                var mode = RecordDeclarationMode.Fields;

                while ((!Match(TokenKind.End, TokenKind.Undefined)) && (mode != RecordDeclarationMode.Undefined)) {
                    AddToList(list, ParseRecordHelperItem(ref mode));
                    if (mode == RecordDeclarationMode.Undefined) {
                        Unexpected();
                        break;
                    }
                }

                return new RecordHelperItemsSymbol(GetFixedArray(list));
            }
        }

        #endregion
        #region ParseRecordHelperItem

        /// <summary>
        ///     parse record helper items
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>

        [Rule("RecordHelperItem", "MethodDeclaration | PropertyDeclaration | ConstSection | TypeSection | Visibility ")]
        public RecordHelperItemSymbol ParseRecordHelperItem(ref RecordDeclarationMode mode) {
            var attributes1 = default(UserAttributesSymbol);
            var attributes2 = default(UserAttributesSymbol);
            var classSymbol = default(Terminal);
            var varSymbol = ContinueWith(TokenKind.Var);

            if (varSymbol != default) {
                mode = RecordDeclarationMode.Fields;
                return new RecordHelperItemSymbol(varSymbol);
            }

            if (Match(TokenKind.Class) && LookAhead(1, TokenKind.Var)) {
                classSymbol = ContinueWith(TokenKind.Class);
                varSymbol = ContinueWith(TokenKind.Var);
                mode = RecordDeclarationMode.ClassFields;
                return new RecordHelperItemSymbol(classSymbol, varSymbol);
            }

            attributes1 = ParseAttributes();
            classSymbol = ContinueWith(TokenKind.Class);
            attributes2 = ParseAttributes();

            if (classSymbol != default && Match(TokenKind.Const)) {
                mode = RecordDeclarationMode.Other;
                return new RecordHelperItemSymbol(ParseConstSection(true));
            }

            if (classSymbol != default && Match(TokenKind.TypeKeyword)) {
                mode = RecordDeclarationMode.Other;
                return new RecordHelperItemSymbol(ParseTypeSection(true));
            }

            if (Match(TokenKind.Procedure, TokenKind.Function, TokenKind.Constructor, TokenKind.Destructor)) {
                mode = RecordDeclarationMode.Other;
                return new RecordHelperItemSymbol(ParseMethodDeclaration(), classSymbol, attributes1, attributes2);
            }

            if (Match(TokenKind.Property)) {
                mode = RecordDeclarationMode.Other;
                return new RecordHelperItemSymbol(ParsePropertyDeclaration());
            }

            if (Match(TokenKind.TypeKeyword)) {
                mode = RecordDeclarationMode.Other;
                return new RecordHelperItemSymbol(ParseTypeSection(true));
            }

            if (Match(TokenKind.Const)) {
                mode = RecordDeclarationMode.Other;
                return new RecordHelperItemSymbol(ParseConstSection(true));
            }


            if (Match(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Strict, TokenKind.Published, TokenKind.Automated)) {
                if (classSymbol != default) {
                    Unexpected();
                }

                var strict = ContinueWith(TokenKind.Strict);
                var visibility = ContinueWith(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Published, TokenKind.Automated);
                mode = RecordDeclarationMode.Fields;
                return new RecordHelperItemSymbol(strict, visibility);
            }

            if (MatchIdentifier() && (!Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Automated, TokenKind.Strict))) {

                if (mode == RecordDeclarationMode.Fields || mode == RecordDeclarationMode.ClassFields) {
                    return new RecordHelperItemSymbol(ParseClassFieldDeclararation(), attributes1, attributes2);
                }

                Unexpected();
            }

            mode = RecordDeclarationMode.Undefined;
            return null;
        }

        #endregion
        #region ParseObjectDecl

        /// <summary>
        ///     parse a object declaration
        /// </summary>
        /// <returns></returns>

        [Rule("ObjectDecl", "'object' ClassParent ObjectItems 'end' ")]
        public ObjectDeclarationSymbol ParseObjectDecl() {
            var objectSymbol = ContinueWith(TokenKind.Object);
            var classParent = ParseClassParent();
            var items = ParseObjectItems();
            var endSymbol = ContinueWithOrMissing(TokenKind.End);
            return new ObjectDeclarationSymbol(objectSymbol, classParent, items, endSymbol);
        }

        #endregion
        #region ParseObjectItems

        /// <summary>
        ///     parse object items
        /// </summary>
        /// <returns></returns>

        [Rule("ObjectItems", " { ObjectItem } ")]
        public ObjectItems ParseObjectItems() {
            using (var list = GetList<ObjectItem>()) {
                var mode = ClassDeclarationMode.Fields;
                var item = default(ObjectItem);

                while ((!Match(TokenKind.End)) && (mode != ClassDeclarationMode.Undefined)) {
                    item = AddToList(list, ParseObjectItem(ref mode));
                    if (mode == ClassDeclarationMode.Undefined) {
                        Unexpected();
                        break;
                    }
                }

                return new ObjectItems(GetFixedArray(list));
            }
        }

        #endregion
        #region ParseObjectItem

        /// <summary>
        ///     parse an object item
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>

        [Rule("ObjectItem", "Visibility | MethodDeclaration | ClassFieldDeclaration | PropertyDeclaration ")]
        public ObjectItem ParseObjectItem(ref ClassDeclarationMode mode) {
            var strictSymbol = default(Terminal);
            var visibility = default(Terminal);

            if (Match(TokenKind.Var)) {
                mode = ClassDeclarationMode.Fields;
                return new ObjectItem(ContinueWith(TokenKind.Var));
            }

            if (Match(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Strict, TokenKind.Published, TokenKind.Automated)) {
                strictSymbol = ContinueWith(TokenKind.Strict);
                visibility = ContinueWith(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Published, TokenKind.Automated);
                mode = ClassDeclarationMode.Fields;
                return new ObjectItem(strictSymbol, visibility);
            }

            if (Match(TokenKind.Procedure, TokenKind.Function, TokenKind.Constructor, TokenKind.Destructor)) {
                mode = ClassDeclarationMode.Other;
                return new ObjectItem(ParseMethodDeclaration());
            }

            if (Match(TokenKind.Property)) {
                mode = ClassDeclarationMode.Other;
                return new ObjectItem(ParsePropertyDeclaration());
            }

            if (Match(TokenKind.Const)) {
                mode = ClassDeclarationMode.Other;
                return new ObjectItem(ParseConstSection(true));
            }

            if (Match(TokenKind.TypeKeyword)) {
                mode = ClassDeclarationMode.Other;
                return new ObjectItem(ParseTypeSection(true));
            }

            if (MatchIdentifier() && (!Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Automated, TokenKind.Strict))) {
                if (mode == ClassDeclarationMode.Fields || mode == ClassDeclarationMode.ClassFields) {
                    return new ObjectItem(ParseClassFieldDeclararation());
                }
                else {
                    Unexpected();
                }
            }

            mode = ClassDeclarationMode.Undefined;
            return null;
        }

        #endregion
        #region ParseInterfaceDef

        /// <summary>
        ///     parse an interface definition
        /// </summary>
        /// <returns></returns>

        [Rule("InterfaceDef", "('interface' | 'dispinterface') ClassParent [InterfaceGuid] InterfaceDefItems 'end'")]
        public InterfaceDefinitionSymbol ParseInterfaceDef() {
            var interfaceSymbol = ContinueWithOrMissing(TokenKind.Interface, TokenKind.DispInterface);
            var parentInterface = ParseClassParent();
            var guid = default(InterfaceGuidSymbol);

            if (Match(TokenKind.OpenBraces))
                guid = ParseInterfaceGuid();

            var items = ParseInterfaceItems();
            var end = default(Terminal);

            if (items != default && items.Items != default && items.Items.Length > 0)
                end = ContinueWithOrMissing(TokenKind.End);
            else
                end = ContinueWith(TokenKind.End);

            return new InterfaceDefinitionSymbol(interfaceSymbol, parentInterface, guid, items, end);
        }

        #endregion
        #region  ParseInterfaceItems

        [Rule("InterfaceItems", "{ InterfaceItem }")]
        private InterfaceItems ParseInterfaceItems() {
            var unexpected = false;

            using (var list = GetList<InterfaceItemSymbol>()) {

                while ((!Match(TokenKind.End)) && (!unexpected)) {
                    AddToList(list, ParseInterfaceItem(out unexpected));
                    if (unexpected && list.Item.Count > 0) {
                        Unexpected();
                        break;
                    }
                }

                return new InterfaceItems(GetFixedArray(list));
            }
        }

        #endregion
        #region ParseInterfaceItem

        /// <summary>
        ///     parse an interface item
        /// </summary>
        /// <param name="unexpected"></param>
        /// <returns></returns>

        [Rule("InterfaceItem", "MethodDeclaration | PropertyDeclaration")]
        public InterfaceItemSymbol ParseInterfaceItem(out bool unexpected) {
            unexpected = true;

            if (Match(TokenKind.Procedure, TokenKind.Function)) {
                unexpected = false;
                return new InterfaceItemSymbol(ParseMethodDeclaration());
            }

            if (Match(TokenKind.Property)) {
                unexpected = false;
                return new InterfaceItemSymbol(ParsePropertyDeclaration());
            }

            return null;
        }

        #endregion
        #region ParseInterfaceGuid

        /// <summary>
        ///     parse an interface guid identifier
        /// </summary>
        /// <returns></returns>

        [Rule("InterfaceGuid", "'[' ( QuotedString ) | Identifier ']'")]
        public InterfaceGuidSymbol ParseInterfaceGuid() {
            var openBraces = ContinueWith(TokenKind.OpenBraces);
            var idIdentifier = default(IdentifierSymbol);
            var stringIdentifier = default(QuotedStringSymbol);

            if (Match(TokenKind.Identifier))
                idIdentifier = RequireIdentifier();
            else
                stringIdentifier = RequireString();

            var closeBraces = ContinueWithOrMissing(TokenKind.CloseBraces);

            return new InterfaceGuidSymbol(openBraces, idIdentifier, stringIdentifier, closeBraces);
        }

        #endregion
        #region ParseClassHelper

        /// <summary>
        ///     class helper definition
        /// </summary>
        /// <returns></returns>

        [Rule("ClassHelper", "'class' 'helper' ClassParent 'for' TypeName ClassHelperItems 'end'")]
        public ClassHelperDefSymbol ParseClassHelper()
            => new ClassHelperDefSymbol(
                classSymbol: ContinueWithOrMissing(TokenKind.Class),
                helperSymbol: ContinueWithOrMissing(TokenKind.Helper),
                classParent: ParseClassParent(),
                forSymbol: ContinueWithOrMissing(TokenKind.For),
                helperName: ParseTypeName(),
                helperItems: ParseClassHelperItems(),
                endSymbol: ContinueWithOrMissing(TokenKind.End));

        #endregion
        #region ClassHelperItems

        /// <summary>
        ///     parse class helper items
        /// </summary>
        /// <returns></returns>

        [Rule("ClassHelperItems", " { ClassHelperItem }")]
        public ClassHelperItemsSymbol ParseClassHelperItems() {
            var mode = ClassDeclarationMode.Fields;

            using (var list = GetList<ClassHelperItemSymbol>()) {
                while (Tokenizer.HasNextToken && (!Match(TokenKind.End, TokenKind.Undefined)) && (mode != ClassDeclarationMode.Undefined)) {
                    AddToList(list, ParseClassHelperItem(ref mode));
                    if (mode == ClassDeclarationMode.Undefined) {
                        Unexpected();
                        break;
                    }
                }

                return new ClassHelperItemsSymbol(GetFixedArray(list));
            }
        }

        #endregion
        #region ParseClassHelperItem

        /// <summary>
        ///     parse a class helper item
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>

        [Rule("ClassHelperItem", "Visibility | MethodDeclaration | PropertyDeclaration | [ 'class' ] VarSection")]
        public ClassHelperItemSymbol ParseClassHelperItem(ref ClassDeclarationMode mode) {

            if (Match(TokenKind.Var)) {
                mode = ClassDeclarationMode.Fields;
                return new ClassHelperItemSymbol(ContinueWith(TokenKind.Var));
            }

            if (Match(TokenKind.Class) && LookAhead(1, TokenKind.Var)) {
                mode = ClassDeclarationMode.ClassFields;
                return new ClassHelperItemSymbol(ContinueWith(TokenKind.Class), ContinueWith(TokenKind.Var));
            }

            var attributes1 = ParseAttributes();
            var classSymbol = ContinueWith(TokenKind.Class);
            var attributes2 = ParseAttributes();
            var strictSymbol = default(Terminal);
            var visibility = default(Terminal);

            if (classSymbol == default && Match(TokenKind.Const)) {
                mode = ClassDeclarationMode.Other;
                return new ClassHelperItemSymbol(ParseConstSection(true));
            }

            if (classSymbol == default && Match(TokenKind.TypeKeyword)) {
                mode = ClassDeclarationMode.Other;
                return new ClassHelperItemSymbol(ParseTypeSection(true));
            }

            if (Match(TokenKind.Procedure, TokenKind.Function, TokenKind.Constructor, TokenKind.Destructor)) {
                mode = ClassDeclarationMode.Other;
                return new ClassHelperItemSymbol(ParseMethodDeclaration(), attributes1, attributes2, classSymbol);
            }

            if (Match(TokenKind.Property)) {
                mode = ClassDeclarationMode.Other;
                return new ClassHelperItemSymbol(ParsePropertyDeclaration(), attributes1, attributes2, classSymbol);
            }

            if (Match(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Strict, TokenKind.Published)) {
                if (classSymbol != default) {
                    Unexpected();
                }

                strictSymbol = ContinueWith(TokenKind.Strict);
                visibility = ContinueWith(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Published);
                mode = ClassDeclarationMode.Fields;
                return new ClassHelperItemSymbol(attributes1, classSymbol, attributes2, strictSymbol, visibility);
            }

            if (MatchIdentifier() && (!Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Automated, TokenKind.Strict))) {

                if (mode == ClassDeclarationMode.Fields || mode == ClassDeclarationMode.ClassFields) {
                    return new ClassHelperItemSymbol(ParseClassFieldDeclararation());
                }
                else {
                    Unexpected();
                }
            }

            mode = ClassDeclarationMode.Undefined;
            return null;
        }

        #endregion
        #region ParseClassDefinition

        /// <summary>
        ///     parse a class definition
        /// </summary>
        /// <returns></returns>

        [Rule("ClassDefinition", "'class' [( 'sealed' | 'abstract' )] [ClassParent] ClassItems 'end' ")]
        public ClassDeclarationSymbol ParseClassDefinition() {
            var classSymbol = ContinueWith(TokenKind.Class);
            var sealedSymbol = ContinueWith(TokenKind.Sealed);
            var abstractSymbol = ContinueWith(TokenKind.Abstract);
            var classParent = ParseClassParent();
            var classItems = default(ClassDeclarationItemsSymbol);
            var endSymbol = default(Terminal);
            var forwardDeclaration = false;

            if (!Match(TokenKind.Semicolon))
                classItems = ParseClassDeclartionItems();

            if (classItems != default && classItems.Items != default && classItems.Items.Length > 0)
                endSymbol = ContinueWithOrMissing(TokenKind.End);
            else {
                endSymbol = ContinueWith(TokenKind.End);
                forwardDeclaration = endSymbol == default;
            }

            return new ClassDeclarationSymbol(classSymbol, sealedSymbol, abstractSymbol, classParent, classItems, forwardDeclaration, endSymbol);
        }

        #endregion
        #region ParseClassItems

        /// <summary>
        ///     parse class declaration items
        /// </summary>
        /// <returns></returns>

        [Rule("ClassItems", "{ ClassItem } ")]
        public ClassDeclarationItemsSymbol ParseClassDeclartionItems() {

            var mode = ClassDeclarationMode.Fields;
            using (var list = GetList<ClassDeclarationItemSymbol>()) {
                while ((!Match(TokenKind.End)) && (mode != ClassDeclarationMode.Undefined)) {

                    AddToList(list, ParseClassDeclarationItem(ref mode));

                    if (mode == ClassDeclarationMode.Undefined && list.Item.Count > 0) {
                        Unexpected();
                        break;
                    }
                }

                return new ClassDeclarationItemsSymbol(GetFixedArray(list));
            }
        }

        #endregion
        #region ParseClassDeclarationItem

        /// <summary>
        ///     parse a class declaration item
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>

        [Rule("ClassItem", "Visibility | MethodResolution | MethodDeclaration | ConstSection | TypeSection | PropertyDeclaration | [ 'class'] VarSection | FieldDeclarations ")]
        public ClassDeclarationItemSymbol ParseClassDeclarationItem(ref ClassDeclarationMode mode) {

            if (Match(TokenKind.Var)) {
                mode = ClassDeclarationMode.Fields;
                return new ClassDeclarationItemSymbol(ContinueWith(TokenKind.Var));
            }

            if (Match(TokenKind.Class) && LookAhead(1, TokenKind.Var)) {
                mode = ClassDeclarationMode.ClassFields;
                return new ClassDeclarationItemSymbol(ContinueWith(TokenKind.Class), ContinueWith(TokenKind.Var));
            }

            var attributes1 = ParseAttributes();
            var classSymbol = ContinueWith(TokenKind.Class);
            var attributes2 = ParseAttributes();
            var strictSymbol = default(Terminal);
            var visibility = default(Terminal);

            if (Match(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Strict, TokenKind.Published, TokenKind.Automated)) {
                if (classSymbol != default) {
                    Unexpected();
                }

                strictSymbol = ContinueWith(TokenKind.Strict);
                visibility = ContinueWith(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Published, TokenKind.Automated);
                mode = ClassDeclarationMode.Fields;
                return new ClassDeclarationItemSymbol(attributes1, classSymbol, attributes2, strictSymbol, visibility);
            }

            if (Match(TokenKind.Procedure, TokenKind.Function) && HasTokenBeforeToken(TokenKind.EqualsSign, TokenKind.Semicolon, TokenKind.OpenParen)) {
                if (classSymbol != default) {
                    Unexpected();
                }

                mode = ClassDeclarationMode.Other;
                return new ClassDeclarationItemSymbol(ParseMethodResolution());
            }

            if (Match(TokenKind.Procedure, TokenKind.Function, TokenKind.Constructor, TokenKind.Destructor)) {
                mode = ClassDeclarationMode.Other;
                return new ClassDeclarationItemSymbol(ParseMethodDeclaration(), attributes1, attributes2, classSymbol);
            }

            if (Match(TokenKind.Property)) {
                mode = ClassDeclarationMode.Other;
                return new ClassDeclarationItemSymbol(ParsePropertyDeclaration(), attributes1, attributes2, classSymbol);
            }

            if (classSymbol == default && Match(TokenKind.Const)) {
                mode = ClassDeclarationMode.Other;
                return new ClassDeclarationItemSymbol(ParseConstSection(true));
            }

            if (classSymbol == default && Match(TokenKind.TypeKeyword)) {
                mode = ClassDeclarationMode.Other;
                return new ClassDeclarationItemSymbol(ParseTypeSection(true));
            }

            if (MatchIdentifier() && (!Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Automated, TokenKind.Strict))) {
                if (mode == ClassDeclarationMode.Fields && classSymbol != default) {
                    ErrorMissingToken(TokenKind.Var);
                    mode = ClassDeclarationMode.ClassFields;
                }

                if (mode == ClassDeclarationMode.Fields || mode == ClassDeclarationMode.ClassFields) {
                    return new ClassDeclarationItemSymbol(ParseClassFieldDeclararation(), mode == ClassDeclarationMode.ClassFields, attributes1, attributes2);
                }
                else {
                    Unexpected();
                }
            }

            mode = ClassDeclarationMode.Undefined;
            return null;
        }

        #endregion
        #region ParseFieldDeclaration

        /// <summary>
        ///     parse a class field
        /// </summary>
        /// <returns></returns>

        [Rule("ClassFieldDeclaration", "IdentList ':' TypeSpecification Hints ';'")]
        public ClassFieldSymbol ParseClassFieldDeclararation()
            => new ClassFieldSymbol(
                names: ParseIdentList(true),
                colonSymbol: ContinueWithOrMissing(TokenKind.Colon),
                typeDecl: ParseTypeSpecification(),
                hint: ParseHints(false),
                semicolon: ContinueWithOrMissing(TokenKind.Semicolon));

        #endregion
        #region PropertyDeclaration

        /// <summary>
        ///     class property
        /// </summary>
        /// <returns></returns>

        [Rule("PropertyDeclaration", "'property' Identifier [ '[' FormalParameters  ']' ] [ ':' TypeName ] [ 'index' Expression ]  { ClassPropertySpecifier } ';' [ 'default' ';' ]  ")]
        public ClassPropertySymbol ParsePropertyDeclaration() {

            var propertySymbol = ContinueWithOrMissing(TokenKind.Property);
            var propertyName = RequireIdentifier();
            var openBraces = default(Terminal);
            var arrayIndex = default(FormalParametersSymbol);
            var closeBraces = default(Terminal);
            var colonSymbol = default(Terminal);
            var typeName = default(TypeNameSymbol);
            var indexSymbol = default(Terminal);
            var propertyIndex = default(ExpressionSymbol);

            if (Match(TokenKind.OpenBraces)) {
                openBraces = ContinueWith(TokenKind.OpenBraces);
                arrayIndex = ParseFormalParameters();
                closeBraces = ContinueWithOrMissing(TokenKind.CloseBraces);
            }

            if (Match(TokenKind.Colon)) {
                colonSymbol = ContinueWith(TokenKind.Colon);
                typeName = ParseTypeName();
            }

            if (Match(TokenKind.Index)) {
                indexSymbol = ContinueWith(TokenKind.Index);
                propertyIndex = ParseExpression();
            }

            using (var list = GetList<ClassPropertySpecifierSymbol>()) {
                while (Match(TokenKind.Read, TokenKind.Write, TokenKind.Add, TokenKind.Remove, TokenKind.ReadOnly, TokenKind.WriteOnly, TokenKind.DispId) ||
                    Match(TokenKind.Default, TokenKind.Stored, TokenKind.Implements, TokenKind.NoDefault)) {
                    AddToList(list, ParseClassPropertyAccessSpecifier());
                }


                var semicolon = ContinueWithOrMissing(TokenKind.Semicolon);
                var defaultSymbol = default(Terminal);
                var semicolon2 = default(Terminal);

                if (Match(TokenKind.Default)) {
                    defaultSymbol = ContinueWith(TokenKind.Default);
                    semicolon2 = ContinueWithOrMissing(TokenKind.Semicolon);
                }

                return new ClassPropertySymbol(propertySymbol, propertyName, openBraces, arrayIndex, closeBraces, colonSymbol, typeName, indexSymbol, propertyIndex, GetFixedArray(list), semicolon, defaultSymbol, semicolon2);
            }
        }

        #endregion
        #region ParseClassPropertyAccessSpecifier

        /// <summary>
        ///     parse a class property access specifier
        /// </summary>
        /// <returns></returns>

        [Rule("ClassPropertySpecifier", "ClassPropertyReadWrite | ClassPropertyDispInterface | ('stored' Expression ';') | ('default' [ Expression ] ';' ) | ('nodefault' ';') | ('implements' NamespaceName) ")]
        public ClassPropertySpecifierSymbol ParseClassPropertyAccessSpecifier() {

            if (Match(TokenKind.Read, TokenKind.Write, TokenKind.Add, TokenKind.Remove)) {
                return new ClassPropertySpecifierSymbol(ParseClassPropertyReadWrite());
            }

            if (Match(TokenKind.ReadOnly, TokenKind.WriteOnly, TokenKind.DispId)) {
                return new ClassPropertySpecifierSymbol(ParseClassPropertyDispInterface());
            }

            if (Match(TokenKind.Stored)) {
                return new ClassPropertySpecifierSymbol(ContinueWith(TokenKind.Stored), ParseExpression());
            }

            if (Match(TokenKind.Default)) {
                var defaultSymbol = ContinueWith(TokenKind.Default);
                var defaultProperty = default(ExpressionSymbol);

                if (!Match(TokenKind.Semicolon)) {
                    defaultProperty = ParseExpression();
                }
                return new ClassPropertySpecifierSymbol(defaultProperty, defaultSymbol);
            }

            if (Match(TokenKind.NoDefault)) {
                return new ClassPropertySpecifierSymbol(ContinueWith(TokenKind.NoDefault));
            }

            if (Match(TokenKind.Implements)) {
                return new ClassPropertySpecifierSymbol(ContinueWith(TokenKind.Implements), ParseNamespaceName());
            }

            Unexpected();
            return null;
        }

        #endregion
        #region ParseClassPropertyDispInterface

        /// <summary>
        ///     parse a disp interface section
        /// </summary>
        /// <returns></returns>

        [Rule("ClassPropertyDispInterface", "( 'readonly' ';')  | ( 'writeonly' ';' ) | DispIdDirective ")]
        public ClassPropertyDispInterfaceSymbols ParseClassPropertyDispInterface() {

            if (Match(TokenKind.ReadOnly)) {
                return new ClassPropertyDispInterfaceSymbols(ContinueWith(TokenKind.ReadOnly));
            }

            if (Match(TokenKind.WriteOnly)) {
                return new ClassPropertyDispInterfaceSymbols(ContinueWith(TokenKind.WriteOnly));
            }

            return new ClassPropertyDispInterfaceSymbols(ParseDispIdDirective(false));
        }

        #endregion
        #region ParseDispIdDirective

        /// <summary>
        ///     parse a dispid directive
        /// </summary>
        /// <param name="requireSemi"></param>
        /// <returns></returns>

        [Rule("DispIdDirective", "'dispid' Expression ';'")]
        public DispIdSymbol ParseDispIdDirective(bool requireSemi = true)
            => new DispIdSymbol(
                ContinueWithOrMissing(TokenKind.DispId),
                ParseExpression(),
                requireSemi ? ContinueWithOrMissing(TokenKind.Semicolon) : default);

        #endregion
        #region ParseClassPropertyReadWrite

        /// <summary>
        ///     parse a property read write modifier
        /// </summary>
        /// <returns></returns>

        [Rule("ClassPropertyReadWrite", "('read' | 'write' | 'add' | 'remove' ) NamespaceName ")]
        public ClassPropertyReadWriteSymbol ParseClassPropertyReadWrite()
            => new ClassPropertyReadWriteSymbol(
                ContinueWithOrMissing(TokenKind.Read, TokenKind.Write, TokenKind.Add, TokenKind.Remove),
                ParseNamespaceName());

        #endregion
        #region ParseTypeSection

        /// <summary>
        ///     parse a type section
        /// </summary>
        /// <param name="inClassDeclaration"></param>
        /// <returns></returns>

        [Rule("TypeSection", "'type' TypeDeclaration { TypeDeclaration }")]
        public TypeSectionSymbol ParseTypeSection(bool inClassDeclaration) {
            var typeKeyword = ContinueWithOrMissing(TokenKind.TypeKeyword);

            using (var list = GetList<TypeDeclarationSymbol>()) {
                while ((!inClassDeclaration || !Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Automated, TokenKind.Strict)) && MatchIdentifier(TokenKind.OpenBraces)) {
                    AddToList(list, ParseTypeDeclaration());
                };

                return new TypeSectionSymbol(typeKeyword, GetFixedArray(list));
            }
        }

        #endregion
        #region ParseTypeDeclaration

        /// <summary>
        ///     parse a type declaration
        /// </summary>
        /// <returns></returns>

        [Rule("TypeDeclaration", "[ Attributes ] GenericTypeIdent '=' TypeDeclaration Hints ';' ")]
        public TypeDeclarationSymbol ParseTypeDeclaration()
            => new TypeDeclarationSymbol(
                attributes: ParseAttributes(),
                genericTypeIdent: ParseGenericTypeIdent(),
                equalsSign: ContinueWithOrMissing(TokenKind.EqualsSign),
                typeSpecification: ParseTypeSpecification(),
                hints: ParseHints(false),
                semicolon: ContinueWithOrMissing(TokenKind.Semicolon));

        #endregion
        #region ParseGenericTypeIdent

        /// <summary>
        ///     parse a generic type identifier
        /// </summary>
        /// <returns></returns>

        [Rule("GenericTypeIdent", "Ident [ GenericDefintion ] ")]
        public GenericTypeIdentifierSymbol ParseGenericTypeIdent() {
            var identifier = RequireIdentifier();
            var genericDefinition = default(GenericDefinitionSymbol);

            if (Match(TokenKind.AngleBracketsOpen)) {
                genericDefinition = ParseGenericDefinition();
            }

            return new GenericTypeIdentifierSymbol(identifier, genericDefinition);
        }

        #endregion
        #region ParseMethodResolution

        /// <summary>
        ///     parse a method resolution
        /// </summary>
        /// <returns></returns>

        [Rule("MethodResolution", "( 'function' | 'procedure' ) NamespaceName '=' Identifier ';' ")]
        public MethodResolutionSymbol ParseMethodResolution() {
            var kindSymbol = ContinueWithOrMissing(TokenKind.Function, TokenKind.Procedure);
            var name = ParseTypeName();
            var equalsSign = ContinueWithOrMissing(TokenKind.EqualsSign);
            var resolveIdentifier = RequireIdentifier();
            var semicolon = ContinueWithOrMissing(TokenKind.Semicolon);
            return new MethodResolutionSymbol(kindSymbol, name, equalsSign, resolveIdentifier, semicolon);
        }

        #endregion
        #region ParseMethodDeclaration

        /// <summary>
        ///     parse a class method
        /// </summary>
        /// <returns></returns>

        [Rule("MethodDeclaration", "( 'constructor' | 'destructor' | 'procedure' | 'function' | 'operator') Identifier [GenericDefinition] [FormalParameters] [ ':' [ Attributes ] TypeSpecification ] ';' { MethodDirective } ")]
        public ClassMethodSymbol ParseMethodDeclaration() {
            var methodSymbol = ContinueWithOrMissing(TokenKind.Constructor, TokenKind.Destructor, TokenKind.Procedure, TokenKind.Function, TokenKind.Operator);
            var isInOperator = methodSymbol.Kind == TokenKind.Operator && Match(TokenKind.In);
            var identifier = RequireIdentifier(isInOperator);
            var genericDefinition = default(GenericDefinitionSymbol);
            var openParen = default(Terminal);
            var parameters = default(FormalParametersSymbol);
            var closeParen = default(Terminal);
            var colonSymbol = default(Terminal);
            var resultAttributes = default(UserAttributesSymbol);
            var resultType = default(TypeSpecificationSymbol);
            var semicolon = default(Terminal);
            var directives = default(MethodDirectivesSymbol);

            if (Match(TokenKind.AngleBracketsOpen)) {
                genericDefinition = ParseGenericDefinition();
            }

            if (Match(TokenKind.OpenParen)) {
                openParen = ContinueWith(TokenKind.OpenParen);
                parameters = ParseFormalParameters();
                closeParen = ContinueWithOrMissing(TokenKind.CloseParen);
            }

            if (Match(TokenKind.Colon)) {
                colonSymbol = ContinueWith(TokenKind.Colon);
                resultAttributes = ParseAttributes();
                resultType = ParseTypeSpecification();
            }

            semicolon = ContinueWithOrMissing(TokenKind.Semicolon);
            directives = ParseMethodDirectives();
            return new ClassMethodSymbol(methodSymbol, identifier, genericDefinition, openParen, parameters, closeParen, colonSymbol, resultAttributes, resultType, semicolon, directives);
        }

        #endregion
        #region FormalParameters

        /// <summary>
        ///     parse formal parameters
        /// </summary>
        /// <returns></returns>

        [Rule("FormalParameters", "FormalParameter { ';' FormalParameter }")]
        public FormalParametersSymbol ParseFormalParameters() {

            using (var list = GetList<FormalParameterDefinitionSymbol>()) {
                var item = default(FormalParameterDefinitionSymbol);

                do {
                    if (!Match(TokenKind.CloseParen))
                        item = AddToList(list, ParseFormalParameterDefinition(true));
                } while (item != default && item.Semicolon != default);

                return new FormalParametersSymbol(GetFixedArray(list));
            }
        }

        #endregion
        #region FormalParameter

        /// <summary>
        ///     parse a formal parameter
        /// </summary>
        /// <param name="allowComma"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        public FormalParameterSymbol ParseFormalParameter(bool allowComma, ref int kind) {

            var attributes1 = default(UserAttributesSymbol);
            var attributes2 = default(UserAttributesSymbol);
            var parameterKind = default(Terminal);

            if (Match(TokenKind.OpenBraces)) {
                attributes1 = ParseAttributes();
            }

            if (kind < TokenKind.Undefined && Match(TokenKind.Const, TokenKind.Var, TokenKind.Out)) {
                parameterKind = ContinueWith(TokenKind.Const, TokenKind.Var, TokenKind.Out);
                kind = parameterKind.GetSymbolKind();
            }
            else if (kind < TokenKind.Undefined) {
                kind = TokenKind.Undefined;
            }

            if (Match(TokenKind.OpenBraces)) {
                attributes2 = ParseAttributes();
            }

            var parameterName = RequireIdentifier(true);
            var comma = default(Terminal);

            if (allowComma)
                comma = ContinueWith(TokenKind.Comma);

            return new FormalParameterSymbol(attributes1, parameterKind, attributes2, parameterName, comma);
        }

        /// <summary>
        ///     test a formal parameter definition symbol
        /// </summary>
        /// <param name="allowSemicolon"></param>
        /// <returns></returns>

        [Rule("FormalParameter", "[Attributes] [( 'const' | 'var' | 'out' )] [Attributes] IdentList [ ':' TypeDeclaration ] [ '=' Expression ]")]
        public FormalParameterDefinitionSymbol ParseFormalParameterDefinition(bool allowSemicolon) {

            using (var list = GetList<FormalParameterSymbol>()) {
                var item = default(FormalParameterSymbol);
                var typeDef = default(TypeSpecificationSymbol);
                var colon = default(Terminal);
                var equals = default(Terminal);
                var defaultValue = default(ExpressionSymbol);
                var semicolon = default(Terminal);
                var kind = TokenKind.Undefined - 1;

                do {
                    item = AddToList(list, ParseFormalParameter(true, ref kind));
                } while (item != default && item.Comma != default);

                if (Match(TokenKind.Colon)) {
                    colon = ContinueWith(TokenKind.Colon);
                    typeDef = ParseTypeSpecification();
                }

                if (Match(TokenKind.EqualsSign)) {
                    equals = ContinueWith(TokenKind.EqualsSign);
                    defaultValue = ParseExpression();
                }

                if (allowSemicolon)
                    semicolon = ContinueWith(TokenKind.Semicolon);

                return new FormalParameterDefinitionSymbol(GetFixedArray(list), colon, typeDef, equals, defaultValue, semicolon);
            }
        }

        #endregion
        #region ParseIdentList

        /// <summary>
        ///     parse a identifier list
        /// </summary>
        /// <param name="allowAttributes"></param>
        /// <returns></returns>

        [Rule("IdentList", "Identifier { ',' Identifier }")]
        public IdentifierListSymbol ParseIdentList(bool allowAttributes) {
            var item = default(IdentifierListItem);

            using (var list = GetList<IdentifierListItem>()) {
                do {
                    var attributes = default(UserAttributesSymbol);

                    if (allowAttributes && Match(TokenKind.OpenBraces))
                        attributes = ParseAttributes();

                    var identifier = RequireIdentifier();
                    var comma = ContinueWith(TokenKind.Comma);
                    item = AddToList(list, new IdentifierListItem(attributes, identifier, comma));

                } while (item != default && item.Comma != default);

                return new IdentifierListSymbol(GetFixedArray(list));
            }
        }

        #endregion
        #region ParseGenericDefinition

        /// <summary>
        ///     parse a generic definition
        /// </summary>
        /// <returns></returns>

        [Rule("GenericDefinition", "SimpleGenericDefinition | ConstrainedGenericDefinition")]
        public GenericDefinitionSymbol ParseGenericDefinition() {
            if (!LookAhead(2, TokenKind.Comma, TokenKind.AngleBracketsClose)) {
                return ParseConstrainedGenericDefinition();
            }

            return ParseSimpleGenericDefinition();
        }

        #endregion
        #region SimpleGenericDefinition

        /// <summary>
        ///     parse a generic definition
        /// </summary>
        /// <returns></returns>

        [Rule("SimpleGenericDefinition", "'<' Identifier { ',' Identifier } '>'")]
        public GenericDefinitionSymbol ParseSimpleGenericDefinition() {
            var openBrackets = ContinueWith(TokenKind.AngleBracketsOpen);
            using (var list = GetList<GenericDefinitionPartSymbol>()) {
                var item = default(GenericDefinitionPartSymbol);

                do {
                    item = AddToList(list, new GenericDefinitionPartSymbol(RequireIdentifier(), ContinueWith(TokenKind.Comma)));
                } while (item != default && item.CommaOrSemicolon != default);

                var closeBrackets = ContinueWithOrMissing(TokenKind.AngleBracketsClose);
                return new GenericDefinitionSymbol(openBrackets, GetFixedArray(list), closeBrackets);
            }
        }

        #endregion
        #region ParseConstrainedGenericDefinition

        [Rule("ConstrainedGenericDefinition", "'<' GenericDefinitionPart { ';' GenericDefinitionPart } '>'")]
        private GenericDefinitionSymbol ParseConstrainedGenericDefinition() {
            var openBrackets = ContinueWith(TokenKind.AngleBracketsOpen);
            using (var list = GetList<GenericDefinitionPartSymbol>()) {
                var item = default(GenericDefinitionPartSymbol);

                do {
                    item = AddToList(list, new GenericDefinitionPartSymbol(ParseGenericDefinitionPart(), ContinueWith(TokenKind.Semicolon)));
                } while (item != default && item.CommaOrSemicolon != default);

                var closeBrackets = ContinueWithOrMissing(TokenKind.AngleBracketsClose);
                return new GenericDefinitionSymbol(openBrackets, GetFixedArray(list), closeBrackets);
            }
        }

        #endregion
        #region ParseGenericDefinitionPart

        /// <summary>
        ///     parse generic symbol part
        /// </summary>
        /// <returns></returns>

        [Rule("GenericDefinitionPart", "Identifier [ ':' GenericConstraint { ',' GenericConstraint } ]")]
        public GenericDefinitionPartSymbol ParseGenericDefinitionPart() {
            var identifier = RequireIdentifier();
            var colon = ContinueWith(TokenKind.Colon);

            if (colon != default) {
                using (var list = GetList<ConstrainedGenericSymbol>()) {
                    var item = default(ConstrainedGenericSymbol);

                    do {
                        item = AddToList(list, ParseGenericConstraint(true));
                    } while (item != default && item.Comma != default);

                    return new GenericDefinitionPartSymbol(identifier, colon, GetFixedArray(list));
                };
            }

            return new GenericDefinitionPartSymbol(identifier);
        }

        #endregion
        #region ParseGenericConstraint

        /// <summary>
        ///     parse a generic constraint
        /// </summary>
        /// <param name="allowComma"></param>
        /// <returns></returns>

        [Rule("GenericConstraint", " 'record' | 'class' | 'constructor' | Identifier ")]
        public ConstrainedGenericSymbol ParseGenericConstraint(bool allowComma) {
            var constraintSymbol = ContinueWith(TokenKind.Record, TokenKind.Class, TokenKind.Constructor);
            var comma = default(Terminal);
            var identifier = default(IdentifierSymbol);

            if (constraintSymbol == null)
                identifier = RequireIdentifier();

            if (allowComma)
                comma = ContinueWith(TokenKind.Comma);

            return new ConstrainedGenericSymbol(constraintSymbol, identifier, comma);
        }

        #endregion
        #region ParseClassParent

        /// <summary>
        ///     parse a parent class symbol
        /// </summary>
        /// <returns></returns>

        [Rule("ClassParent", " [ '(' TypeName { ',' TypeName } ')' ]")]
        public ParentClassSymbol ParseClassParent() {
            if (!Match(TokenKind.OpenParen))
                return null;

            using (var list = GetList<TypeNameSymbol>()) {
                var openParen = ContinueWith(TokenKind.OpenParen);
                var item = default(TypeNameSymbol);
                do {
                    item = AddToList(list, ParseTypeName(false, true));
                } while (item != default && item.Comma != default);

                var closeParen = ContinueWithOrMissing(TokenKind.CloseParen);
                return new ParentClassSymbol(openParen, GetFixedArray(list), closeParen);
            }
        }

        #endregion
        #region ParseClassOfDeclaration

        /// <summary>
        ///     class of declaration
        /// </summary>
        /// <returns></returns>

        [Rule("ClassOfDeclaration", "'class' 'of' TypeName")]
        public ClassOfDeclarationSymbol ParseClassOfDeclaration() => new ClassOfDeclarationSymbol(
                classSymbol: ContinueWithOrMissing(TokenKind.Class),
                ofSymbol: ContinueWithOrMissing(TokenKind.Of),
                typeName: ParseTypeName());

        #endregion
        #region ParseTypeName

        /// <summary>
        ///     pare a type name
        /// </summary>
        /// <param name="inDesignator"></param>
        /// <param name="allowComma"></param>
        /// <returns></returns>
        [Rule("TypeName", "'string' | 'ansistring' | 'shortstring' | 'unicodestring' | 'widestring' | (NamespaceName [ GenericSuffix ]) ")]
        public TypeNameSymbol ParseTypeName(bool inDesignator = false, bool allowComma = false) {
            var stringType = ContinueWith(TokenKind.String, TokenKind.AnsiString, TokenKind.ShortString, TokenKind.UnicodeString, TokenKind.WideString);

            if (stringType != default)
                return new TypeNameSymbol(stringType, allowComma ? ContinueWith(TokenKind.Comma) : default);

            using (var list = GetList<GenericNamespaceNameSymbol>()) {
                var name = default(GenericNamespaceNameSymbol);
                do {
                    name = AddToList(list, ParseGenericNamespaceName(true, inDesignator, true));
                } while ((!inDesignator || !IsLastDisignatorPart()) && name?.Dot != default);

                return new TypeNameSymbol(GetFixedArray(list), allowComma ? ContinueWith(TokenKind.Comma) : default);
            }
        }

        private bool IsLastDisignatorPart() {
            var angleBracesCount = 0;
            var lookaheadCount = 1;

            while (!Tokenizer.BaseTokenizer.AtEof) {
                var tokenKind = Tokenizer.LookAhead(lookaheadCount).Value.Kind;

                if ((angleBracesCount == 0) && (tokenKind == TokenKind.Dot))
                    return false;

                if ((angleBracesCount == 0) && (tokenKind == TokenKind.OpenBraces))
                    return true;

                if ((angleBracesCount == 0) && (tokenKind == TokenKind.OpenParen))
                    return true;

                if (tokenKind == TokenKind.Semicolon)
                    return false;

                if (tokenKind == TokenKind.CloseBraces)
                    return false;

                if (tokenKind == TokenKind.CloseParen)
                    return false;

                if (tokenKind == TokenKind.End)
                    return false;

                if (tokenKind == TokenKind.LessThen)
                    angleBracesCount++;

                if (tokenKind == TokenKind.GreaterThen)
                    angleBracesCount--;

                if (angleBracesCount < 0)
                    return false;

                lookaheadCount++;
            }

            return true;
        }

        #endregion
        #region ParseFileType

        /// <summary>
        ///     parse a file type
        /// </summary>
        /// <returns></returns>

        [Rule("FileType", "'file' [ 'of' TypeSpecification ]")]
        public FileTypeSymbol ParseFileType() {
            var fileSymbol = ContinueWithOrMissing(TokenKind.File);
            var ofSymbol = ContinueWith(TokenKind.Of);
            var typeDefinition = default(TypeSpecificationSymbol);

            if (ofSymbol != default)
                typeDefinition = ParseTypeSpecification();

            return new FileTypeSymbol(fileSymbol, ofSymbol, typeDefinition);
        }

        #endregion
        #region ParseGenericSuffix

        /// <summary>
        ///     parse a generic suffix symbol
        /// </summary>
        /// <returns></returns>

        [Rule("GenericSuffix", "'<' TypeDefinition { ',' TypeDefinition '}' '>'")]
        public GenericSuffixSymbol ParseGenericSuffix() {
            var openBracket = ContinueWithOrMissing(TokenKind.AngleBracketsOpen);

            using (var list = GetList<TypeSpecificationSymbol>()) {
                var typeSpecification = default(TypeSpecificationSymbol);
                do {
                    typeSpecification = AddToList(list, ParseTypeSpecification(false, false, true));
                } while ((!Tokenizer.AtEof) && typeSpecification?.Comma != default);

                var closeBracket = ContinueWithOrMissing(TokenKind.AngleBracketsClose);
                return new GenericSuffixSymbol(openBracket, GetFixedArray(list), closeBracket);
            }
        }

        #endregion
        #region ParseArrayType

        /// <summary>
        ///     parse an array type
        /// </summary>
        /// <returns></returns>

        [Rule("ArrayType", " 'array' [ '[' ArrayIndex { ',' ArrayIndex } ']']  'of' ( 'const' | TypeDefinition ) ")]
        public ArrayTypeSymbol ParseArrayType() {
            var array = ContinueWithOrMissing(TokenKind.Array);
            var openBraces = ContinueWith(TokenKind.OpenBraces);
            var closeBraces = default(Terminal);
            var items = default(ImmutableArray<ArrayIndexSymbol>);

            if (openBraces != default) {
                using (var list = GetList<ArrayIndexSymbol>()) {
                    var index = default(ArrayIndexSymbol);
                    do {
                        index = AddToList(list, ParseArrayIndex());
                    } while ((!Tokenizer.AtEof) && index?.Comma != default);

                    items = GetFixedArray(list);
                }

                closeBraces = ContinueWithOrMissing(TokenKind.CloseBraces);
            }

            var ofSymbol = ContinueWithOrMissing(TokenKind.Of);
            var constSymbol = ContinueWith(TokenKind.Const);
            var typeSpecification = default(TypeSpecificationSymbol);

            if (constSymbol == null)
                typeSpecification = ParseTypeSpecification();

            return new ArrayTypeSymbol(
                array: array,
                openBraces: openBraces,
                items: items,
                closeBraces: closeBraces,
                ofSymbol: ofSymbol,
                constSymbol: constSymbol,
                typeSpecification: typeSpecification
            );
        }

        #endregion
        #region ParseArrayIndex

        /// <summary>
        ///     parse an array index
        /// </summary>
        /// <returns></returns>

        [Rule("ArrayIndex", "ConstantExpression [ '..' ConstantExpression ] ")]
        public ArrayIndexSymbol ParseArrayIndex() => new ArrayIndexSymbol(
                startIndex: ParseConstantExpression(),
                dotDot: ContinueWith(TokenKind.DotDot, out var hasDots),
                endIndex: hasDots ? ParseConstantExpression() : null,
                comma: ContinueWith(TokenKind.Comma)
            );

        #endregion
        #region ParsePointerType

        /// <summary>
        ///     parse a pointer type
        /// </summary>
        /// <returns></returns>

        [Rule("PointerType", "( 'pointer' | '^' TypeSpecification )")]
        public PointerTypeSymbol ParsePointerType() {

            if (Match(TokenKind.Pointer))
                return new PointerTypeSymbol(ContinueWith(TokenKind.Pointer));

            var circumflex = ContinueWithOrMissing(TokenKind.Circumflex);
            var typeSpecification = ParseTypeSpecification();
            return new PointerTypeSymbol(circumflex, typeSpecification);
        }

        #endregion
        #region ParseAttributes

        /// <summary>
        ///     parse user attributes
        /// </summary>
        /// <returns></returns>

        [Rule("Attributes", "{ '[' AttributeSet | AssemblyAttribue ']' }")]
        public UserAttributesSymbol ParseAttributes() {
            using (var list = GetList<UserAttributeSetSymbol>()) {
                while (Match(TokenKind.OpenBraces)) {
                    AddToList(list, ParseAttributeSet());
                }

                return new UserAttributesSymbol(GetFixedArray(list));
            }
        }

        #endregion
        #region ParseAttributeSet

        /// <summary>
        ///     parse a set of attributes
        /// </summary>
        /// <returns></returns>

        [Rule("AttributeSet", " '[' Attribute { ',' Attribute } ']' ")]
        public UserAttributeSetSymbol ParseAttributeSet() {
            var openBraces = ContinueWith(TokenKind.OpenBraces);
            using (var list = GetList<UserAttributeDefinitionSymbol>()) {
                var item = default(UserAttributeDefinitionSymbol);

                do {
                    item = AddToList(list, ParseAttribute(true));
                } while (item != default && item.Comma != default);

                var closeBraces = ContinueWith(TokenKind.CloseBraces);

                return new UserAttributeSetSymbol(openBraces, GetFixedArray(list), closeBraces);
            }
        }

        /// <summary>
        ///     parse a user attribute
        /// </summary>
        /// <param name="allowComma"></param>
        /// <returns></returns>

        [Rule("Attribute", " [ 'Result' ':' ] NamespaceName [ '(' Expressions ')' ]")]
        public UserAttributeDefinitionSymbol ParseAttribute(bool allowComma) {

            var prefix = default(IdentifierSymbol);
            var colon = default(Terminal);

            if (LookAhead(1, TokenKind.Colon)) {
                prefix = RequireIdentifier(true);
                colon = ContinueWith(TokenKind.Colon);
            }

            var name = ParseNamespaceName();
            var openParen = ContinueWith(TokenKind.OpenParen);
            var closeParen = default(Terminal);
            var expressions = default(ExpressionList);
            var comma = default(Terminal);

            if (openParen != default) {
                expressions = ParseExpressions();
                closeParen = ContinueWithOrMissing(TokenKind.CloseParen);
            }

            if (allowComma)
                comma = ContinueWith(TokenKind.Comma);

            return new UserAttributeDefinitionSymbol(prefix, colon, name, openParen, expressions, closeParen, comma);
        }

        #endregion
        #region Expressions

        [Rule("Expressions", "Expression { ',' Expression }")]
        private ExpressionList ParseExpressions() {
            using (var list = GetList<ExpressionSymbol>()) {
                var item = default(ExpressionSymbol);

                do {
                    item = AddToList(list, ParseExpression(false, true));
                } while (item != default && item.Comma.GetSymbolKind() == TokenKind.Comma);

                return new ExpressionList(GetFixedArray(list));
            }
        }

        #endregion
        #region ParseConstantExpression

        private bool IsArrayConstant() {
            var level = 1;
            var counter = 1;

            while (level > 0 && (!Tokenizer.BaseTokenizer.AtEof)) {
                if (LookAhead(counter, TokenKind.OpenParen))
                    level++;
                else if (LookAhead(counter, TokenKind.CloseParen))
                    level--;
                if (level == 1 && LookAhead(counter, TokenKind.Comma))
                    return true;
                counter = counter + 1;
            }

            return false;
        }

        /// <summary>
        ///     parse a constant expression
        /// </summary>
        /// <param name="fromDesignator"></param>
        /// <param name="fromTypeConstExpression"></param>
        /// <param name="allowComma"></param>
        /// <returns></returns>

        [Rule("ConstantExpression", " '(' ( RecordConstant | ConstantExpression ) ')' | Expression")]
        public ConstantExpressionSymbol ParseConstantExpression(bool fromDesignator = false, bool fromTypeConstExpression = false, bool allowComma = false) {
            var expr = default(ExpressionSymbol);
            var openParen = default(Terminal);
            var closeParen = default(Terminal);
            var comma = default(Terminal);

            if (Match(TokenKind.OpenParen)) {

                if (LookAhead(1, TokenKind.CloseParen) || (LookAheadIdentifier(1, Array.Empty<int>(), true) && (LookAhead(2, TokenKind.Colon)))) {
                    openParen = ContinueWithOrMissing(TokenKind.OpenParen);
                    var record = default(RecordConstantExpressionSymbol);
                    using (var list = GetList<SyntaxPartBase>()) {
                        do {
                            if (!Match(TokenKind.CloseParen)) {
                                record = AddToList(list, ParseRecordConstant(true));
                            }
                            else {
                                record = default;
                            }
                        } while (record != default && record.Separator.GetSymbolKind() == TokenKind.Semicolon && Tokenizer.HasNextToken);

                        closeParen = ContinueWithOrMissing(TokenKind.CloseParen);
                        if (allowComma)
                            comma = ContinueWith(TokenKind.Comma);
                        return new ConstantExpressionSymbol(openParen, GetFixedArray(list), closeParen, comma);
                    }
                }

                if (IsArrayConstant() || fromDesignator) {
                    openParen = ContinueWithOrMissing(TokenKind.OpenParen);
                    var constExpr = default(ConstantExpressionSymbol);
                    using (var list = GetList<SyntaxPartBase>()) {
                        do {
                            if (!Match(TokenKind.CloseParen)) {
                                constExpr = AddToList(list, ParseConstantExpression(allowComma: true));
                            }
                            else {
                                constExpr = default;
                            }
                        } while (constExpr != default && constExpr.Comma.GetSymbolKind() == TokenKind.Comma && Tokenizer.HasNextToken);

                        closeParen = ContinueWithOrMissing(TokenKind.CloseParen);
                        if (allowComma)
                            comma = ContinueWith(TokenKind.Comma);
                        return new ConstantExpressionSymbol(openParen, closeParen, GetFixedArray(list), comma);
                    }
                }

                if (!fromDesignator) {
                    expr = ParseExpression();
                    if (allowComma)
                        comma = ContinueWith(TokenKind.Comma);
                    return new ConstantExpressionSymbol(expr, comma);
                }

                Unexpected();
                return null;
            }

            expr = ParseExpression(fromTypeConstExpression);
            if (allowComma)
                comma = ContinueWith(TokenKind.Comma);
            return new ConstantExpressionSymbol(expr, comma);
        }

        #endregion
        #region ParseRecordConstant

        /// <summary>
        ///     parse a record constant expression
        /// </summary>
        /// <param name="allowSemicolon"></param>
        /// <returns></returns>

        [Rule("RecordConstantExpression", "Identifier ':' ConstantExpression")]
        public RecordConstantExpressionSymbol ParseRecordConstant(bool allowSemicolon) {
            var name = RequireIdentifier();
            var colon = ContinueWithOrMissing(TokenKind.Colon);
            var value = ParseConstantExpression();
            var semicolon = allowSemicolon ? ContinueWith(TokenKind.Semicolon) : default;
            return new RecordConstantExpressionSymbol(name, colon, value, semicolon);
        }

        #endregion
        #region ParseExpression

        /// <summary>
        ///     parse a expression
        /// </summary>
        /// <param name="fromConstTypeDeclaration"></param>
        /// <param name="allowComma"></param>
        /// <returns></returns>

        [Rule("Expression", "SimpleExpression [ ('<'|'<='|'>'|'>='|'<>'|'='|'in'|'is') SimpleExpression ] | ClosureExpression")]
        public ExpressionSymbol ParseExpression(bool fromConstTypeDeclaration = false, bool allowComma = false) {
            var comma = default(Terminal);

            if (Match(TokenKind.Function, TokenKind.Procedure)) {
                var closureExpression = ParseClosureExpression();
                if (allowComma)
                    comma = ContinueWith(TokenKind.Comma);
                return new ExpressionSymbol(closureExpression, comma);
            }

            var leftOperand = ParseSimpleExpression();
            var rightOperand = default(SimpleExpression);

            if (fromConstTypeDeclaration && !HasTokenBeforeToken(TokenKind.EqualsSign, new[] { TokenKind.Semicolon }))
                return new ExpressionSymbol(leftOperand);

            var @operator = ContinueWith(TokenKind.LessThen, TokenKind.LessThenEquals, TokenKind.GreaterThen, TokenKind.GreaterThenEquals, TokenKind.NotEquals, TokenKind.EqualsSign, TokenKind.In, TokenKind.Is);

            if (@operator != default)
                rightOperand = ParseSimpleExpression();

            if (allowComma)
                comma = ContinueWith(TokenKind.Comma);

            return new ExpressionSymbol(leftOperand, @operator, rightOperand, comma);
        }

        #endregion
        #region ParseTerm

        /// <summary>
        ///     parse a term
        /// </summary>
        /// <returns></returns>
        [Rule("Term", "Factor [ ('*'|'/'|'div'|'mod'|'and'|'shl'|'shr'|'as') Term ]")]
        public TermSymbol ParseTerm() {
            var leftOperand = ParseFactor();
            var @operator = ContinueWith(TokenKind.Times, TokenKind.Slash, TokenKind.Div, TokenKind.Mod, TokenKind.And, TokenKind.Shl, TokenKind.Shr, TokenKind.As);
            var rightOperand = default(TermSymbol);

            if (@operator != default)
                rightOperand = ParseTerm();

            return new TermSymbol(leftOperand, @operator, rightOperand);
        }

        #endregion
        #region ParseFactor

        private bool IsDesignator() {
            var level = 1;
            var counter = 1;

            while (level > 0 && (!Tokenizer.BaseTokenizer.AtEof)) {
                if (LookAhead(counter, TokenKind.OpenParen))
                    level++;
                else if (LookAhead(counter, TokenKind.CloseParen))
                    level--;
                counter = counter + 1;
            }

            return LookAhead(counter, TokenKind.Dot, TokenKind.Circumflex);
        }

        /// <summary>
        ///     parse a factor
        /// </summary>
        /// <returns></returns>

        [Rule("Factor", "'@' Factor  | 'not' Factor | '+' Factor | '-' Factor | '^' Identifier | Integer | HexNumber | Real | 'true' | 'false' | 'nil' | '(' Expression ')' | String | SetSection | Designator | TypeCast")]
        public FactorSymbol ParseFactor() {
            var designator = default(DesignatorStatementSymbol);
            var unaryOperator = ContinueWith(TokenKind.At, TokenKind.Not, TokenKind.Plus, TokenKind.Minus);

            if (unaryOperator != default)
                return new FactorSymbol(unaryOperator, ParseFactor());

            unaryOperator = ContinueWith(TokenKind.Circumflex);

            if (unaryOperator != default)
                return new FactorSymbol(unaryOperator, RequireIdentifier());

            if (Match(TokenKind.Integer)) {
                var intValue = RequireInteger();

                if (Match(TokenKind.Dot))
                    designator = ParseDesignator();

                return new FactorSymbol(intValue, designator);
            }

            if (Match(TokenKind.HexNumber)) {
                var hexValue = RequireHexValue();

                if (Match(TokenKind.Dot))
                    designator = ParseDesignator();

                return new FactorSymbol(hexValue, designator);
            }

            if (Match(TokenKind.Real)) {
                var realValue = RequireRealValue();

                if (Match(TokenKind.Dot))
                    designator = ParseDesignator();

                return new FactorSymbol(realValue, designator);
            }

            if (Match(TokenKind.QuotedString)) {
                var stringValue = RequireString();

                if (Match(TokenKind.Dot))
                    designator = ParseDesignator();

                return new FactorSymbol(stringValue, designator);
            }

            if (Match(TokenKind.True) || Match(TokenKind.False)) {
                var booleanValue = ContinueWith(TokenKind.True, TokenKind.False);

                if (Match(TokenKind.Dot))
                    designator = ParseDesignator();

                return new FactorSymbol(booleanValue, designator);
            }

            if (Match(TokenKind.Nil))
                return new FactorSymbol(ContinueWith(TokenKind.Nil));

            if (Match(TokenKind.OpenBraces))
                return new FactorSymbol(ParseSetSection());

            if (MatchIdentifier(TokenKind.Inherited, TokenKind.ShortString, TokenKind.String, TokenKind.WideString, TokenKind.UnicodeString, TokenKind.AnsiString, TokenKind.Dot))
                return new FactorSymbol(ParseDesignator());

            if (Match(TokenKind.OpenParen) && IsDesignator())
                return new FactorSymbol(ParseDesignator());

            if (Match(TokenKind.OpenParen)) {
                var openParen = ContinueWith(TokenKind.OpenParen);
                var parenExpression = ParseConstantExpression(false, false);
                var closeParen = ContinueWithOrMissing(TokenKind.CloseParen);
                return new FactorSymbol(openParen, parenExpression, closeParen);
            }

            Unexpected();
            return null;
        }

        #endregion
        #region ParseDesignator

        /// <summary>
        ///     parse a designator statement
        /// </summary>
        /// <returns></returns>

        [Rule("Designator", "[ 'inherited' ] [ NamespaceName ] { DesignatorItem }")]
        public DesignatorStatementSymbol ParseDesignator() {
            var inherited = ContinueWith(TokenKind.Inherited);
            var name = default(TypeNameSymbol);
            var item = default(DesignatorItemSymbol);
            var hasIdentifier = false;

            if (MatchIdentifier(TokenKind.String, TokenKind.ShortString, TokenKind.AnsiString, TokenKind.WideString, TokenKind.String) && LookAhead(1, TokenKind.Dot, TokenKind.AngleBracketsOpen)) {
                name = ParseTypeName(true);
                hasIdentifier = name != default;
            }

            using (var list = GetList<DesignatorItemSymbol>()) {
                do {
                    item = AddToList(list, ParseDesignatorItem(hasIdentifier, item));
                    var di = item as DesignatorItemSymbol;
                    hasIdentifier = hasIdentifier || (di != default && di.Subitem != default);
                } while (item != default);

                return new DesignatorStatementSymbol(inherited, name, GetFixedArray(list));
            }
        }

        #endregion
        #region ParseDesignatorItem

        /// <summary>
        ///     parse a parameter
        /// </summary>
        /// <returns></returns>
        public ParameterSymbol ParseParameter() {
            var parameterName = default(IdentifierSymbol);
            var assignmentSymbol = default(Terminal);
            var expression = default(FormattedExpressionSymbol);
            var comma = default(Terminal);

            if (MatchIdentifier(true) && LookAhead(1, TokenKind.Assignment)) {
                parameterName = RequireIdentifier(true);
                assignmentSymbol = ContinueWithOrMissing(TokenKind.Assignment);
            }

            if (!Match(TokenKind.Comma))
                expression = ParseFormattedExpression();

            comma = ContinueWith(TokenKind.Comma);

            return new ParameterSymbol(parameterName, assignmentSymbol, expression, comma);
        }

        /// <summary>
        ///     parse a designator item
        /// </summary>
        /// <returns></returns>

        [Rule("DesignatorItem", "'^' | '.' Ident [GenericSuffix] | '[' ExpressionList ']' | '(' [ FormattedExpression  { ',' FormattedExpression } ] ')'")]
        public DesignatorItemSymbol ParseDesignatorItem(bool hasIdentifier, DesignatorItemSymbol prevDesignatorItem) {

            if (Match(TokenKind.Circumflex))
                return new DesignatorItemSymbol(ContinueWithOrMissing(TokenKind.Circumflex));

            var subitem = default(IdentifierSymbol);
            var dot = default(Terminal);
            var genericSuffix = default(GenericSuffixSymbol);
            var openBraces = default(Terminal);
            var closeBraces = default(Terminal);
            var indexExpression = default(ExpressionList);
            var openParen = default(Terminal);
            var closeParen = default(Terminal);

            if (Match(TokenKind.Dot) ||
                (!hasIdentifier && MatchIdentifier(TokenKind.String, TokenKind.AnsiString, TokenKind.ShortString, TokenKind.WideString, TokenKind.UnicodeString))) {
                dot = ContinueWith(TokenKind.Dot);
                subitem = RequireIdentifier(true);
            };

            if (Match(TokenKind.AngleBracketsOpen) &&
                LookAheadIdentifier(1, new[] { TokenKind.String, TokenKind.ShortString, TokenKind.WideString, TokenKind.UnicodeString, TokenKind.AnsiString, TokenKind.Pointer }, false)) {
                var whereCloseBrackets = HasTokenUntilToken(new[] { TokenKind.AngleBracketsClose }, TokenKind.Identifier, TokenKind.Dot, TokenKind.Comma, TokenKind.AngleBracketsOpen, TokenKind.String, TokenKind.ShortString, TokenKind.WideString, TokenKind.UnicodeString, TokenKind.AnsiString, TokenKind.Pointer);
                if (whereCloseBrackets.Item1 && (!LookAheadIdentifier(1 + whereCloseBrackets.Item2, new[] { TokenKind.HexNumber, TokenKind.Integer, TokenKind.Real }, false) || LookAhead(1 + whereCloseBrackets.Item2, TokenKind.Read, TokenKind.Write, TokenKind.ReadOnly, TokenKind.WriteOnly, TokenKind.Add, TokenKind.Remove, TokenKind.DispId))) {
                    genericSuffix = ParseGenericSuffix();
                }
            }

            if (Match(TokenKind.OpenBraces)) {
                openBraces = ContinueWith(TokenKind.OpenBraces);
                indexExpression = ParseExpressions();
                closeBraces = ContinueWithOrMissing(TokenKind.CloseBraces);
                return new DesignatorItemSymbol(dot, subitem, genericSuffix, openBraces, indexExpression, closeBraces);
            }

            if (Match(TokenKind.OpenParen)) {
                if (!IsDesignator() && ((prevDesignatorItem == null) || (prevDesignatorItem.Subitem == null)) && (!LookAhead(1, TokenKind.CloseParen))) {
                    openParen = ContinueWithOrMissing(TokenKind.OpenParen);
                    var children = ParseConstantExpression(true);
                    closeParen = ContinueWithOrMissing(TokenKind.CloseParen);
                    return new DesignatorItemSymbol(dot, subitem, openParen, children, closeParen);
                }

                openParen = ContinueWith(TokenKind.OpenParen);
                using (var list = GetList<ParameterSymbol>()) {
                    if (!Match(TokenKind.CloseParen)) {
                        var parameter = default(ParameterSymbol);
                        do {
                            parameter = AddToList(list, ParseParameter());
                        } while (parameter != default && parameter.Comma != default);
                    }
                    closeParen = ContinueWithOrMissing(TokenKind.CloseParen);

                    return new DesignatorItemSymbol(dot, subitem, genericSuffix, openParen, GetFixedArray(list), closeParen);
                }
            }

            if (dot != null || subitem != null || genericSuffix != null)
                return new DesignatorItemSymbol(dot, subitem, genericSuffix, openParen, ImmutableArray<ParameterSymbol>.Empty, closeParen);

            return default;
        }

        #endregion
        #region ParseFormattedExpression

        /// <summary>
        ///     parse a formatted expression
        /// </summary>
        /// <returns></returns>

        [Rule("FormattedExpression", "Expression [ ':' Expression [ ':' Expression ] ]")]
        public FormattedExpressionSymbol ParseFormattedExpression() {
            var expression = ParseExpression();
            var colon1 = ContinueWith(TokenKind.Colon);
            var colon2 = default(Terminal);
            var width = default(ExpressionSymbol);
            var decimals = default(ExpressionSymbol);

            if (colon1 != default) {
                width = ParseExpression();
                colon2 = ContinueWith(TokenKind.Colon);

                if (colon2 != default)
                    decimals = ParseExpression();
            }

            return new FormattedExpressionSymbol(expression, colon1, width, colon2, decimals);
        }

        #endregion
        #region ParseClosureExpression

        /// <summary>
        ///     closure expression (anonymous method)
        /// </summary>
        /// <returns></returns>

        [Rule("ClosureExpr", "('function'|'procedure') [ FormalParameterSection ] [ ':' TypeSpecification ] Block ")]
        public ClosureExpressionSymbol ParseClosureExpression() {

            var procSymbol = ContinueWithOrMissing(TokenKind.Function, TokenKind.Procedure);
            var parameters = default(FormalParameterSection);
            var colonSymbol = default(Terminal);
            var returnType = default(TypeSpecificationSymbol);

            if (Match(TokenKind.OpenParen))
                parameters = ParseFormalParameterSection();

            if (procSymbol.GetSymbolKind() == TokenKind.Function) {
                colonSymbol = ContinueWithOrMissing(TokenKind.Colon);
                returnType = ParseTypeSpecification();
            }

            var block = ParseBlock();
            return new ClosureExpressionSymbol(procSymbol, parameters, colonSymbol, returnType, block);
        }

        #endregion

        #region Helper Functions

        /// <summary>
        ///     require an integral value
        /// </summary>
        /// <returns></returns>
        public StandardInteger RequireInteger()
            => new StandardInteger(ContinueWithOrMissing(TokenKind.Integer));

        /// <summary>
        ///     parse a hex number
        /// </summary>
        /// <returns></returns>
        public HexNumberSymbol RequireHexValue()
            => new HexNumberSymbol(ContinueWithOrMissing(TokenKind.HexNumber));

        /// <summary>
        ///     parse a real number
        /// </summary>
        /// <returns></returns>
        public RealNumberSymbol RequireRealValue()
            => new RealNumberSymbol(ContinueWithOrMissing(TokenKind.Real));


        /// <summary>
        ///     parse an identifier
        /// </summary>
        /// <param name="allowReserverdWords"></param>
        /// <returns></returns>
        public IdentifierSymbol RequireIdentifier(bool allowReserverdWords = false) {

            if (Match(TokenKind.Identifier)) {
                return new IdentifierSymbol(ContinueWithOrMissing(TokenKind.Identifier));
            };

            var token = CurrentToken();

            if (allowReserverdWords || !reservedWords.Contains(token.Kind)) {
                return new IdentifierSymbol(ContinueWithOrMissing(token.Kind));
            }

            ErrorMissingToken(TokenKind.Identifier);
            return null;
        }

        /// <summary>
        ///     parse a quoted string symbol
        /// </summary>
        /// <returns></returns>
        public QuotedStringSymbol RequireString()
            => new QuotedStringSymbol(ContinueWithOrMissing(TokenKind.QuotedString));

        private QuotedStringSymbol RequireDoubleQuotedString()
            => new QuotedStringSymbol(ContinueWithOrMissing(TokenKind.DoubleQuotedString));

        private bool CurrentTokenIsAfterNewline() {

            if (CurrentTokenSequences().Prefix == null)
                return false;

            foreach (var invalidToken in CurrentTokenSequences().Prefix) {
                if (invalidToken.Kind == TokenKind.WhiteSpace && PatternContinuation.ContainsNewLineChar(invalidToken.Value))
                    return true;
            }

            return false;
        }

        /// <summary>
        ///     parse a namespace name
        /// </summary>
        /// <param name="allowIn"></param>
        /// <param name="inDesignator"></param>
        /// <param name="allowComma"></param>
        /// <returns></returns>
        public NamespaceNameSymbol ParseNamespaceName(bool allowIn = false, bool inDesignator = false, bool allowComma = false) {
            using (var list = GetList<SyntaxPartBase>()) {
                var name = default(SyntaxPartBase);
                var dot = default(Terminal);
                var comma = default(Terminal);

                name = ContinueWith(TokenKind.AnsiString, TokenKind.String, TokenKind.WideString, TokenKind.ShortString, TokenKind.UnicodeString);

                if (name == default && allowIn && Match(TokenKind.In))
                    name = ContinueWith(TokenKind.In);

                if (name == default)
                    name = RequireIdentifier();

                AddToList(list, name);

                if (!inDesignator || LookAhead(2, new int[] { TokenKind.Dot })) {
                    dot = ContinueWith(TokenKind.Dot);
                    AddToList(list, dot);
                }

                while (name != default && dot != default && MatchIdentifier(true) && (!inDesignator || LookAhead(1, new int[] { TokenKind.Dot }))) {
                    name = RequireIdentifier(true);
                    AddToList(list, name);

                    dot = ContinueWith(TokenKind.Dot);
                    AddToList(list, dot);
                }

                if (allowComma)
                    comma = ContinueWith(TokenKind.Comma);

                return new NamespaceNameSymbol(GetFixedArray(list), comma);
            }
        }


        private PoolItem<List<T>> GetList<T>() where T : class
            => Environment.ListPools.GetList<T>();

        private static Q AddToList<T, Q>(PoolItem<List<T>> list, Q item) where T : class where Q : T {
            if (item != default)
                list.Item.Add(item);
            return item;
        }

        private static ImmutableArray<T> GetFixedArray<T>(PoolItem<List<T>> list) where T : class {
            var builder = ListPools.GetImmutableArrayBuilder(list);
            for (var index = 0; index < list.Item.Count; index++)
                builder.Add(list.Item[index]);
            return builder.ToImmutable();
        }

        /// <summary>
        ///     print the parser grammar
        /// </summary>
        /// <param name="result">Result</param>
        public static void PrintGrammar(StringBuilder result)
            => PrintGrammar(typeof(StandardParser), result);

        private bool MatchIdentifier(params int[] otherTokens)
            => LookAheadIdentifier(0, otherTokens, false);

        private bool MatchIdentifier(bool allowReservedWords)
            => LookAheadIdentifier(0, Array.Empty<int>(), allowReservedWords);

        private bool LookAheadIdentifier(int lookAhead, int[] otherTokens, bool allowReservedWords) {
            if (LookAhead(lookAhead, otherTokens))
                return true;

            if (LookAhead(lookAhead, TokenKind.Identifier))
                return true;

            var token = Tokenizer.LookAhead(lookAhead);

            if (!allowReservedWords && reservedWords.Contains(token.Value.Kind))
                return false;

            if (string.IsNullOrWhiteSpace(token.Value.Value))
                return false;

            return Tokenizer.Keywords.ContainsKey(token.Value.Value);
        }


        #endregion

    }
}