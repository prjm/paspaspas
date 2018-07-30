using System.Text;
using System.Collections.Generic;
using PasPasPas.Parsing.Tokenizer;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.SyntaxTree.Standard;
using System;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.Tokenizer.Patterns;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Options.Bundles;
using PasPasPas.Infrastructure.ObjectPooling;
using System.Collections.Immutable;

namespace PasPasPas.Parsing.Parser {

    /// <summary>
    ///     standard, recursive descend parser
    /// </summary>
    public class StandardParser : ParserBase, IParser {

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

            var path = Tokenizer.Input.CurrentFile.File;

            if (Match(TokenKind.Library))
                return ParseLibrary(path);

            if (Match(TokenKind.Unit))
                return ParseUnit(path);

            if (Match(TokenKind.Package))
                return ParsePackage(path);

            return ParseProgram(path);
        }

        #endregion
        #region ParseUnit

        /// <summary>
        ///     parse a unit declaration
        /// </summary>
        /// <param name="path">unit path</param>
        /// <returns>parsed unit</returns>

        [Rule("Unit", "UnitHead UnitInterface UnitImplementation UnitBlock '.' ")]
        public UnitSymbol ParseUnit(IFileReference path) {
            return new UnitSymbol() {
                UnitHead = ParseUnitHead(),
                UnitInterface = ParseUnitInterface(),
                UnitImplementation = ParseUnitImplementation(),
                UnitBlock = ParseUnitBlock(),
                DotSymbol = ContinueWithOrMissing(TokenKind.Dot),
                FilePath = path,
            };
        }

        #endregion
        #region ParseUnitInterface

        /// <summary>
        ///     parse a unit interface
        /// </summary>
        /// <returns></returns>

        [Rule("UnitInterface", "'interface' [ UsesClause ] InterfaceDeclaration ")]
        public UnitInterfaceSymbol ParseUnitInterface() {
            return new UnitInterfaceSymbol() {
                InterfaceSymbol = ContinueWithOrMissing(TokenKind.Interface),
                UsesClause = Match(TokenKind.Uses) ? (ISyntaxPart)ParseUsesClause(null) : EmptyTerminal(),
                InterfaceDeclaration = ParseInterfaceDeclaration()
            };
        }

        #endregion
        #region ParseUnitImplementation

        [Rule("UnitImplementation", "'implementation' [ UsesClause ] DeclarationSections", true)]
        private UnitImplementation ParseUnitImplementation() {
            var result = new UnitImplementation();
            InitByTerminal(result, null, TokenKind.Implementation);

            if (Match(TokenKind.Uses)) {
                result.UsesClause = ParseUsesClause(result);
            }

            result.DeclarationSections = ParseDeclarationSections();
            return result;
        }

        #endregion
        #region ParseUsesClause

        [Rule("UsesClause", "'uses' NamespaceNameList")]
        private UsesClause ParseUsesClause(IExtendableSyntaxPart parent) {
            var result = new UsesClause();
            InitByTerminal(result, parent, TokenKind.Uses);
            result.UsesList = ParseNamespaceNameList(result);
            return result;
        }

        #endregion
        #region ParseUsesFileClause

        [Rule("UsesFileClause", "'uses' NamespaceFileNameList")]
        private UsesFileClause ParseUsesFileClause(IExtendableSyntaxPart parent) {
            var result = new UsesFileClause();
            InitByTerminal(result, parent, TokenKind.Uses);
            result.Files = ParseNamespaceFileNameList(result);
            return result;
        }

        #endregion
        #region ParseNamespaceFileNameList

        [Rule("NamespaceFileNameList", "NamespaceFileName { ',' NamespaceFileName }")]
        private NamespaceFileNameList ParseNamespaceFileNameList(IExtendableSyntaxPart parent) {
            var result = new NamespaceFileNameList();
            parent.Add(result);

            do {
                ParseNamespaceFileName(result);
            } while (ContinueWith(result, TokenKind.Comma));

            ContinueWithOrMissing(result, TokenKind.Semicolon);

            return result;
        }

        #endregion
        #region ParseNamespaceFileName

        [Rule("NamespaceFileName", "NamespaceName [ 'in' QuotedString ]")]
        private NamespaceFileName ParseNamespaceFileName(IExtendableSyntaxPart parent) {
            var result = new NamespaceFileName();
            parent.Add(result);

            result.NamespaceName = ParseNamespaceName();
            if (ContinueWith(result, TokenKind.In))
                result.QuotedFileName = RequireString();

            return result;
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
                    item = AddToList(list, ParseInterfaceDeclarationItem(null));
                } while (item != null);
            }

            return result;
        }

        #endregion
        #region ParseInterfaceDeclarationItem

        [Rule("InterfaceDeclarationItem", "ConstSection | TypeSection | VarSection | ExportsSection | AssemblyAttribute | ExportedProcedureHeading")]
        private SyntaxPartBase ParseInterfaceDeclarationItem(IExtendableSyntaxPart parent) {

            if (Match(TokenKind.Const) || Match(TokenKind.Resourcestring)) {
                return ParseConstSection(false);
            }

            if (Match(TokenKind.TypeKeyword)) {
                return ParseTypeSection(false);
            }

            if (Match(TokenKind.Var)) {
                return ParseVarSection(false);
            }

            if (Match(TokenKind.Exports)) {
                return ParseExportsSection();
            }

            if (Match(TokenKind.OpenBraces) && LookAhead(1, TokenKind.Assembly)) {
                return ParseAttributes();
            }

            if (Match(TokenKind.Procedure, TokenKind.Function)) {
                return ParseExportedProcedureHeading();
            }

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
            var resultAttributes = default(UserAttributes);
            var resultType = default(TypeSpecification);
            var semicolon = default(Terminal);
            var directives = default(FunctionDirectives);

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

        [Rule("FunctionDirectives", "{ FunctionDirective } ")]
        private FunctionDirectives ParseFunctionDirectives() {

            using (var list = GetList<SyntaxPartBase>()) {
                var directive = default(SyntaxPartBase);
                do {
                    directive = AddToList(list, ParseFunctionDirective());
                } while (directive != default);
                return new FunctionDirectives(GetFixedArray(list));
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

            if (Match(TokenKind.Far, TokenKind.Local, TokenKind.Near)) {
                return ParseOldCallConvention(null);
            }

            if (Match(TokenKind.Deprecated, TokenKind.Library, TokenKind.Experimental, TokenKind.Platform)) {
                var result = ParseHint();
                result.Semicolon = ContinueWithOrMissing(TokenKind.Semicolon);
                return result;
            }

            if (Match(TokenKind.VarArgs, TokenKind.External)) {
                return ParseExternalDirective(null);
            }

            if (Match(TokenKind.Unsafe)) {
                return ParseUnsafeDirective(null);
            }

            if (Match(TokenKind.Forward)) {
                return ParseForwardDirective(null);
            }

            return null;
        }

        #endregion
        #region ParseForwardDirective

        [Rule("ForwardDirective", "'forward' ';' ")]
        private ForwardDirective ParseForwardDirective(IExtendableSyntaxPart parent) {
            var result = new ForwardDirective();
            InitByTerminal(result, parent, TokenKind.Forward);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseUnsafeDirective

        [Rule("UnsafeDirective", "'unsafe' ';' ")]
        private UnsafeDirective ParseUnsafeDirective(IExtendableSyntaxPart parent) {
            var result = new UnsafeDirective();
            InitByTerminal(result, parent, TokenKind.Unsafe);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseExternalDirective

        [Rule("ExternalDirective", "(varargs | external [ ConstExpression { ExternalSpecifier } ]) ';' ")]
        private ExternalDirective ParseExternalDirective(IExtendableSyntaxPart parent) {
            var result = new ExternalDirective();
            InitByTerminal(result, parent, TokenKind.VarArgs, TokenKind.External);
            result.Kind = result.LastTerminalKind;

            if ((result.Kind == TokenKind.External) && (!Match(TokenKind.Semicolon))) {
                result.ExternalExpression = ParseConstantExpression();
                ExternalSpecifier specifier;
                do {
                    specifier = ParseExternalSpecifier(result);
                } while (specifier != null);
            }

            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseExternalSpecifier

        [Rule("ExternalSpecifier", "(('Name' | 'Index' ) ConstExpression) |  'Dependency' ConstExpression { ', ' ConstExpression } ) | delayed ")]
        private ExternalSpecifier ParseExternalSpecifier(IExtendableSyntaxPart parent) {

            if (!Match(TokenKind.Name, TokenKind.Index, TokenKind.Dependency, TokenKind.Delayed))
                return null;

            var result = new ExternalSpecifier();
            InitByTerminal(result, parent, TokenKind.Name, TokenKind.Index, TokenKind.Dependency, TokenKind.Delayed);
            result.Kind = result.LastTerminalKind;

            if (result.Kind == TokenKind.Delayed)
                return result;

            if (result.Kind != TokenKind.Dependency) {
                result.Expression = ParseConstantExpression();
            }
            else {
                do {
                    ParseConstantExpression();
                } while (ContinueWith(result, TokenKind.Comma));
            }

            return result;
        }

        #endregion
        #region ParseOldCallConvention

        [Rule("OldCallConvention", "'Near' | 'Far' | 'Local'")]
        private SyntaxPartBase ParseOldCallConvention(IExtendableSyntaxPart parent) {
            var result = new OldCallConvention();
            InitByTerminal(result, parent, TokenKind.Near, TokenKind.Far, TokenKind.Local);
            result.Kind = result.LastTerminalKind;
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseUnitBlock

        [Rule("UnitBlock", "( UnitInitilization 'end' ) | CompoundStatement | 'end' ")]
        private UnitBlock ParseUnitBlock() {
            var result = new UnitBlock();

            if (ContinueWith(result, TokenKind.End))
                return result;

            if (Match(TokenKind.Begin, TokenKind.Asm)) {
                result.MainBlock = ParseCompoundStatement();
                return result;
            }

            if (Match(TokenKind.Initialization)) {
                result.Initialization = ParseUnitInitialization(result);
                ContinueWithOrMissing(result, TokenKind.End);
                return result;
            }

            Unexpected();
            return result;
        }

        #endregion
        #region ParseInitialiParseUnitInitialization

        [Rule("UnitInitialization", "'initialization' StatementList [ UnitFinalization ]", true)]
        private UnitInitialization ParseUnitInitialization(IExtendableSyntaxPart parent) {
            var result = new UnitInitialization();
            InitByTerminal(result, parent, TokenKind.Initialization);

            result.Statements = ParseStatementList();

            if (Match(TokenKind.Finalization)) {
                result.Finalization = ParseUnitFinalization(result);
            }

            return result;
        }

        #endregion
        #region ParseUnitFinalization

        [Rule("UnitFinalization", "'finalization' StatementList", true)]
        private UnitFinalization ParseUnitFinalization(IExtendableSyntaxPart parent) {
            var result = new UnitFinalization();
            InitByTerminal(result, parent, TokenKind.Finalization);
            result.Statements = ParseStatementList();
            return result;
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
            using (var list = GetList<Statement>()) {
                var statement = default(Statement);
                do {
                    statement = AddToList(list, ParseStatement(true));
                } while (statement.Semicolon != default);

                return new StatementList(GetFixedArray(list));
            }
        }

        #endregion
        #region ParseStatement
        [Rule("Statement", "[ Label ':' ] StatementPart")]


        public Statement ParseStatement(bool allowSemicolon = false) {

            var label = default(Label);
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


            return new Statement(label, colonSymbol, part, semicolon);
        }
        #endregion
        #region ParseStatementPart

        [Rule("StatementPart", "IfStatement | CaseStatement | ReapeatStatement | WhileStatment | ForStatement | WithStatement | TryStatement | RaiseStatement | AsmStatement | CompoundStatement | SimpleStatement ")]
        private StatementPart ParseStatementPart() {
            var result = new StatementPart();

            if (Match(TokenKind.If)) {
                result.If = ParseIfStatement(result);
                return result;
            }
            if (Match(TokenKind.Case)) {
                result.Case = ParseCaseStatement();
                return result;
            }
            if (Match(TokenKind.Repeat)) {
                result.Repeat = ParseRepeatStatement(result);
                return result;
            }
            if (Match(TokenKind.While)) {
                result.While = ParseWhileStatement(result);
                return result;
            }
            if (Match(TokenKind.For)) {
                result.For = ParseForStatement(result);
                return result;
            }
            if (Match(TokenKind.With)) {
                result.With = ParseWithStatement(result);
                return result;
            }
            if (Match(TokenKind.Try)) {
                result.Try = ParseTryStatement(result);
                return result;
            }
            if (Match(TokenKind.Raise)) {
                result.Raise = ParseRaiseStatement(result);
                return result;
            }
            if (Match(TokenKind.Begin, TokenKind.Asm)) {
                result.CompoundStatement = ParseCompoundStatement();
                return result;
            }

            return ParseSimpleStatement();
        }

        #endregion
        #region ParseRaiseStatement

        [Rule("RaiseStatement", "'raise' [ Expression ] [ 'at' Expression ]")]
        private RaiseStatement ParseRaiseStatement(IExtendableSyntaxPart parent) {
            var result = new RaiseStatement();
            InitByTerminal(result, parent, TokenKind.Raise);

            if ((!Match(TokenKind.AtWord)) && MatchIdentifier(TokenKind.Inherited)) {
                result.Raise = ParseExpression();
            }

            if (ContinueWith(result, TokenKind.AtWord)) {
                result.At = ParseExpression();
            }

            return result;
        }

        #endregion
        #region ParseTryStatement

        [Rule("TryStatement", "'try' StatementList  ('except' HandlerList | 'finally' StatementList) 'end'")]
        private TryStatement ParseTryStatement(IExtendableSyntaxPart parent) {
            var result = new TryStatement();
            InitByTerminal(result, parent, TokenKind.Try);

            result.Try = ParseStatementList();

            if (ContinueWith(result, TokenKind.Except)) {
                result.Handlers = ParseExceptHandlers(result);
                ContinueWithOrMissing(result, TokenKind.End);
            }
            else if (ContinueWith(result, TokenKind.Finally)) {
                result.Finally = ParseStatementList();
                ContinueWithOrMissing(result, TokenKind.End);
            }
            else {
                Unexpected();
            }

            return result;
        }

        #endregion
        #region ParseExceptHandlers

        [Rule("ExceptHandlers", "({ Handler } [ 'else' StatementList ]) | StatementList")]
        private ExceptHandlers ParseExceptHandlers(IExtendableSyntaxPart parent) {
            var result = new ExceptHandlers();
            parent.Add(result);

            if (Match(TokenKind.On, TokenKind.Else)) {
                while (Match(TokenKind.On)) {
                    ParseExceptHandler();
                }
                if (ContinueWith(result, TokenKind.Else)) {
                    result.ElseStatements = ParseStatementList();
                }
            }
            else {
                result.Statements = ParseStatementList();
            }

            return result;
        }

        #endregion
        #region ParseExceptHandler

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

        [Rule("WithStatement", "'with' Expression { ',' Expression }  'do' Statement")]
        private WithStatement ParseWithStatement(IExtendableSyntaxPart parent) {
            var result = new WithStatement();
            InitByTerminal(result, parent, TokenKind.With);

            do {
                ParseExpression();
            } while (ContinueWith(result, TokenKind.Comma));

            ContinueWithOrMissing(result, TokenKind.Do);

            result.Statement = ParseStatement();
            return result;
        }

        #endregion
        #region ParseForStatement

        [Rule("ForStatement", "('for' Designator ':=' Expression ('to' | 'downto' )  Expression 'do' Statement) | ('for' Designator 'in' Expression  'do' Statement)")]
        private ForStatement ParseForStatement(IExtendableSyntaxPart parent) {
            var result = new ForStatement();
            InitByTerminal(result, parent, TokenKind.For);

            result.Variable = RequireIdentifier();
            if (ContinueWith(result, TokenKind.Assignment)) {
                result.StartExpression = ParseExpression();
                ContinueWithOrMissing(result, TokenKind.To, TokenKind.DownTo);
                result.Kind = result.LastTerminalKind;
                result.EndExpression = ParseExpression();
            }
            else {
                ContinueWithOrMissing(result, TokenKind.In);
                result.Kind = result.LastTerminalKind;
                result.StartExpression = ParseExpression();
            }
            ContinueWithOrMissing(result, TokenKind.Do);
            result.Statement = ParseStatement();
            return result;
        }

        #endregion
        #region ParseWhileStatement

        [Rule("WhileStatement", "'while' Expression 'do' Statement")]
        private WhileStatement ParseWhileStatement(IExtendableSyntaxPart parent) {
            var result = new WhileStatement();
            InitByTerminal(result, parent, TokenKind.While);
            result.Condition = ParseExpression();
            ContinueWithOrMissing(result, TokenKind.Do);
            result.Statement = ParseStatement();
            return result;
        }

        #endregion
        #region ParseRepeatStatement

        [Rule("RepeatStatement", "'repeat' [ StatementList ] 'until' Expression")]
        private RepeatStatement ParseRepeatStatement(IExtendableSyntaxPart parent) {
            var result = new RepeatStatement();
            InitByTerminal(result, parent, TokenKind.Repeat);

            if (!Match(TokenKind.Until)) {
                result.Statements = ParseStatementList();
            }
            ContinueWithOrMissing(result, TokenKind.Until);
            result.Condition = ParseExpression();
            return result;
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
            var endExpression = default(Expression);
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

        [Rule("IfStatement", "'if' Expression 'then' Statement [ 'else' Statement ]")]
        private IfStatement ParseIfStatement(IExtendableSyntaxPart parent) {
            var result = new IfStatement();
            InitByTerminal(result, parent, TokenKind.If);

            result.Condition = ParseExpression();
            ContinueWithOrMissing(result, TokenKind.Then);
            result.ThenPart = ParseStatement();
            if (ContinueWith(result, TokenKind.Else)) {
                result.ElsePart = ParseStatement();
            }

            return result;
        }

        #endregion
        #region ParseSimpleStatement

        [Rule("SimpleStatement", "GoToStatement | Designator [ ':=' (Expression  | NewStatement) ] ")]
        private StatementPart ParseSimpleStatement() {
            if (!(LookAhead(1, TokenKind.Assignment, TokenKind.OpenBraces, TokenKind.OpenParen)) && Match(TokenKind.GoToKeyword, TokenKind.Exit, TokenKind.Break, TokenKind.Continue)) {
                var result = new StatementPart();
                result.GoTo = ParseGoToStatement(result);
                return result;
            }

            if (MatchIdentifier(TokenKind.Inherited, TokenKind.Circumflex, TokenKind.OpenParen, TokenKind.At, TokenKind.AnsiString, TokenKind.UnicodeString, TokenKind.String, TokenKind.WideString, TokenKind.ShortString)) {
                var result = new StatementPart();
                result.DesignatorPart = ParseDesignator();

                if (ContinueWith(result, TokenKind.Assignment)) {
                    result.Assignment = ParseExpression();
                }

                return result;
            }

            return null;
        }

        #endregion
        #region ParseGoToStatement

        [Rule("GoToStatement", "('goto' Label) | 'break' | 'continue' | 'exit' '(' Expression ')' ")]
        private GoToStatement ParseGoToStatement(IExtendableSyntaxPart parent) {
            var result = new GoToStatement();
            parent.Add(result);

            if (ContinueWith(result, TokenKind.GoToKeyword)) {
                result.GoToLabel = ParseLabel();
                return result;
            }
            if (ContinueWith(result, TokenKind.Break)) {
                result.Break = true;
                return result;
            }
            if (ContinueWith(result, TokenKind.Continue)) {
                result.Continue = true;
                return result;
            }
            if (ContinueWith(result, TokenKind.Exit)) {
                result.Exit = true;
                if (ContinueWith(result, TokenKind.OpenParen)) {
                    result.ExitExpression = ParseExpression();
                    ContinueWithOrMissing(result, TokenKind.CloseParen);
                }
                return result;
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
        private UnitHeadSymbol ParseUnitHead() {
            return new UnitHeadSymbol(
                ContinueWithOrMissing(TokenKind.Unit),
                ParseNamespaceName(),
                ParseHints(false),
                ContinueWithOrMissing(TokenKind.Semicolon));
        }

        #endregion
        #region ParsePackage

        [Rule("Package", "PackageHead RequiresClause [ ContainsClause ] 'end' '.' ")]
        private Package ParsePackage(IFileReference path) {
            var result = new Package();
            result.PackageHead = ParsePackageHead(result);
            result.FilePath = path;
            result.RequiresClause = ParseRequiresClause(result);

            if (Match(TokenKind.Contains)) {
                result.ContainsClause = ParseContainsClause(result);
            }

            ContinueWithOrMissing(result, TokenKind.End);
            ContinueWithOrMissing(result, TokenKind.Dot);

            return result;
        }

        #endregion
        #region ParseContainsClause

        [Rule("ContainsClause", "'contains' NamespaceFileNameList")]
        private PackageContains ParseContainsClause(IExtendableSyntaxPart parent) {
            var result = new PackageContains();
            InitByTerminal(result, parent, TokenKind.Contains);
            result.ContainsList = ParseNamespaceFileNameList(result);
            return result;
        }

        #endregion
        #region ParseRequiresClause

        [Rule("RequiresClause", "'requires' NamespaceNameList")]
        private PackageRequires ParseRequiresClause(IExtendableSyntaxPart parent) {
            var result = new PackageRequires();
            InitByTerminal(result, parent, TokenKind.Requires);
            result.RequiresList = ParseNamespaceNameList(result);
            return result;
        }

        #endregion
        #region ParseNamespaceNameList

        [Rule("NamespaceNameList", "NamespaceName { ',' NamespaceName } ';' ")]
        private NamespaceNameList ParseNamespaceNameList(IExtendableSyntaxPart parent) {
            var result = new NamespaceNameList();
            parent.Add(result);

            do {
                ParseNamespaceName();
            } while (ContinueWith(result, TokenKind.Comma));

            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParsePackageHead

        [Rule("PackageHead", "'package' NamespaceName ';' ")]
        private PackageHead ParsePackageHead(IExtendableSyntaxPart parent) {
            var result = new PackageHead();
            InitByTerminal(result, parent, TokenKind.Package);
            result.PackageName = ParseNamespaceName();
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseLibrary

        /// <summary>
        ///     parse a library declaration
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>

        [Rule("Library", "LibraryHead [UsesFileClause] Block '.' ")]
        public LibrarySymbol ParseLibrary(IFileReference path) {
            return new LibrarySymbol() {
                LibraryHead = ParseLibraryHead(),
                Uses = Match(TokenKind.Uses) ? (ISyntaxPart)ParseUsesFileClause(null) : EmptyTerminal(),
                MainBlock = ParseBlock(),
                Dot = ContinueWithOrMissing(TokenKind.Dot),
                FilePath = path
            };
        }

        #endregion
        #region ParseLibraryHead

        /// <summary>
        ///     parse a library head
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>

        [Rule("LibraryHead", "'library' NamespaceName Hints ';'")]
        public LibraryHeadSymbol ParseLibraryHead() {
            return new LibraryHeadSymbol() {
                LibrarySymbol = ContinueWithOrMissing(TokenKind.Library),
                LibraryName = ParseNamespaceName(),
                Hints = ParseHints(false),
                Semicolon = ContinueWithOrMissing(TokenKind.Semicolon)
            };
        }

        #endregion
        #region ParseProgram

        /// <summary>
        ///     parse a program definition
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>

        [Rule("Program", "[ProgramHead] [UsesFileClause] Block '.'")]
        public Program ParseProgram(IFileReference path) {
            return new Program() {
                ProgramHead = Match(TokenKind.Program) ? ParseProgramHead() as ISyntaxPart : EmptyTerminal(),
                Uses = Match(TokenKind.Uses) ? ParseUsesFileClause(null) as ISyntaxPart : EmptyTerminal(),
                MainBlock = ParseBlock(),
                Dot = ContinueWithOrMissing(TokenKind.Dot),
                FilePath = path
            };
        }

        #endregion
        #region ParseProgramHead

        /// <summary>
        ///     parse a program head
        /// </summary>
        /// <returns></returns>

        [Rule("ProgramHead", "'program' NamespaceName [ProgramParams] ';'")]
        public ProgramHeadSymbol ParseProgramHead() {
            return new ProgramHeadSymbol() {
                ProgramSymbol = ContinueWithOrMissing(TokenKind.Program),
                Name = ParseNamespaceName(),
                Parameters = ParseProgramParams(null),
                Semicolon = ContinueWithOrMissing(TokenKind.Semicolon)
            };
        }

        #endregion
        #region ParseProgramParams

        [Rule("ProgramParams", "'(' [ Identifier { ',' Identifier } ] ')'")]
        private ProgramParameterList ParseProgramParams(IExtendableSyntaxPart parent) {
            var result = new ProgramParameterList();

            if (parent != null)
                parent.Add(result);

            if (ContinueWith(result, TokenKind.OpenParen)) {

                if (MatchIdentifier()) {
                    RequireIdentifier();

                    while (ContinueWith(result, TokenKind.Comma))
                        RequireIdentifier();
                }

                ContinueWithOrMissing(result, TokenKind.CloseParen);
            }

            return result;
        }

        #endregion
        #region ParseBlock

        /// <summary>
        ///     parse a block symbol
        /// </summary>
        /// <returns></returns>

        [Rule("Block", "DeclarationSections [ BlockBody ] ")]
        public BlockSymbol ParseBlock() {
            return new BlockSymbol(
                declarationSections: ParseDeclarationSections(),
                body: Match(TokenKind.Asm, TokenKind.Begin) ? ParseBlockBody() : null
            );
        }

        #endregion
        #region ParseBlockBody

        /// <summary>
        ///     parse a block body
        /// </summary>
        /// <returns></returns>

        [Rule("BlockBody", "AssemblerBlock | CompoundStatement")]
        public BlockBodySymbol ParseBlockBody() {
            return new BlockBodySymbol(
                assemblerBlock: Match(TokenKind.Asm) ? ParseAsmBlock() : default,
                body: Match(TokenKind.Begin) ? ParseCompoundStatement() : default
            );
        }

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
            var register = default(Identifier);

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
                kind = CurrentToken().Kind;
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

            var segmentPrefix = default(Identifier);
            var lockPrefix = default(Identifier);

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

        [Rule("LocalAsmLabel", "'@' { '@' | Integer | Identifier | HexNumber }")]
        private LocalAsmLabel ParseLocalAsmLabel() {
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
                    else if (Match(TokenKind.At)) {
                        AddToList(list, ContinueWith(TokenKind.At));
                        wasAt = true;
                    }
                    else {
                        Unexpected();
                    }
                }
                while ((!CurrentTokenIsAfterNewline()) && wasAt);

                return new LocalAsmLabel(at, GetFixedArray(list));
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

                    UserAttributes attrs = null;
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

                        AddToList(list, ParseProcedureDeclaration(attrs as UserAttributes));
                        continue;
                    }


                    stop = true;
                }

                return new DeclarationsSymbol(GetFixedArray(list));
            }
        }

        #endregion
        #region ParseMethodDecl

        [Rule("MethodDecl", "MethodDeclHeading ';' MethodDirectives [ Block ';' ]")]
        private MethodDeclaration ParseMethodDecl(Terminal classSymbol, UserAttributes attributes) {
            var heading = ParseMethodDeclHeading();
            var semicolon = ContinueWithOrMissing(TokenKind.Semicolon);
            var directives = ParseMethodDirectives();
            var methodBody = ParseBlock();
            var semicolon2 = default(Terminal);

            if ((methodBody != default) && (methodBody.Body != default))
                semicolon2 = ContinueWithOrMissing(TokenKind.Semicolon);

            return new MethodDeclaration(classSymbol, attributes, heading, semicolon, directives, methodBody, semicolon2);
        }

        #endregion
        #region ParseMethodDirectives

        [Rule("MethodDirectives", "{ MethodDirective }")]
        private MethodDirectives ParseMethodDirectives() {
            using (var list = GetList<SyntaxPartBase>()) {
                SyntaxPartBase directive;
                do {
                    directive = AddToList(list, ParseMethodDirective());
                } while (directive != null);

                return new MethodDirectives(GetFixedArray(list));
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

            if (Match(TokenKind.Deprecated, TokenKind.Library, TokenKind.Experimental, TokenKind.Platform)) {
                var result = ParseHint();
                result.Semicolon = ContinueWithOrMissing(TokenKind.Semicolon);
                return result;
            }

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
        public InlineSymbol ParseInlineDirective() {
            return new InlineSymbol(
                directive: ContinueWithOrMissing(TokenKind.Inline, TokenKind.Assembler),
                semicolon: ContinueWithOrMissing(TokenKind.Semicolon)
            );
        }

        #endregion
        #region ParseCallConvention

        /// <summary>
        ///     parse a calling convention
        /// </summary>
        /// <returns></returns>

        [Rule("CallConvention", "('cdecl' | 'pascal' | 'register' | 'safecall' | 'stdcall' | 'export') ';' ")]
        public CallConventionSymbol ParseCallConvention() {
            return new CallConventionSymbol(
                directive: ContinueWithOrMissing(TokenKind.Cdecl, TokenKind.Pascal, TokenKind.Register, TokenKind.Safecall, TokenKind.Stdcall, TokenKind.Export),
                semicolon: ContinueWithOrMissing(TokenKind.Semicolon)
            );
        }

        #endregion
        #region ParseAbstractDirective

        /// <summary>
        ///     parse an abstract directive
        /// </summary>
        /// <returns></returns>

        [Rule("AbstractDirective", "('abstract' | 'final' ) ';' ")]
        public AbstractSymbol ParseAbstractDirective() {
            return new AbstractSymbol(
                directive: ContinueWithOrMissing(TokenKind.Abstract, TokenKind.Final),
                semicolon: ContinueWithOrMissing(TokenKind.Semicolon)
            );
        }

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
        public OverloadSymbol ParseOverloadDirective() {
            return new OverloadSymbol(
                directive: ContinueWithOrMissing(TokenKind.Overload),
                semicolon: ContinueWithOrMissing(TokenKind.Semicolon)
            );
        }

        #endregion
        #region ParseReintroduceDirective

        /// <summary>
        ///     parse a reintroduce directive
        /// </summary>
        /// <returns></returns>

        [Rule("ReintroduceDirective", "'reintroduce' ';' ")]
        public ReintroduceSymbol ParseReintroduceDirective() {
            return new ReintroduceSymbol() {
                Directive = ContinueWithOrMissing(TokenKind.Reintroduce),
                Semicolon = ContinueWithOrMissing(TokenKind.Semicolon)
            };
        }

        #endregion
        #region MethodDeclarationHeading

        [Rule("MethodDeclHeading", " ('constructor' | 'destructor' | 'function' | 'procedure' | 'operator') NamespaceName [GenericDefinition] [FormalParameterSection] [':' Attributes TypeSpecification ]")]
        private MethodDeclarationHeading ParseMethodDeclHeading() {
            var kindSymbol = ContinueWithOrMissing(TokenKind.Constructor, TokenKind.Destructor, TokenKind.Function, TokenKind.Procedure, TokenKind.Operator);
            var allowIn = kindSymbol.GetSymbolKind() == TokenKind.Operator;
            var name = default(MethodDeclarationName);
            var parameters = default(FormalParameterSection);
            var colonSymbol = default(Terminal);
            var resultTypeAttributes = default(UserAttributes);
            var resultType = default(TypeSpecification);

            using (var list = GetList<MethodDeclarationName>()) {
                do {
                    var namePart = ParseNamespaceName(allowIn);
                    var genericPart = default(GenericDefinition);
                    var dot = default(Terminal);

                    if (Match(TokenKind.AngleBracketsOpen)) {
                        genericPart = ParseGenericDefinition();
                    }

                    dot = ContinueWith(TokenKind.Dot);
                    name = AddToList(list, new MethodDeclarationName(namePart, genericPart, dot));

                } while (name != default && name.Dot.GetSymbolKind() == TokenKind.Dot);

                if (Match(TokenKind.OpenParen)) {
                    parameters = ParseFormalParameterSection();
                }

                colonSymbol = ContinueWith(TokenKind.Colon);

                if (colonSymbol != default) {
                    resultTypeAttributes = ParseAttributes();
                    resultType = ParseTypeSpecification();
                }

                return new MethodDeclarationHeading(kindSymbol, GetFixedArray(list), parameters, colonSymbol, resultTypeAttributes, resultType);
            }
        }

        #endregion
        #region ParseProcedureDeclaration

        [Rule("ProcedureDeclaration", "ProcedureDeclarationHeading ';' FunctionDirectives [ ProcBody ]")]
        private ProcedureDeclaration ParseProcedureDeclaration(UserAttributes attributes) {
            var heading = ParseProcedureDeclarationHeading();
            var semicolon = ContinueWithOrMissing(TokenKind.Semicolon);
            var directives = ParseFunctionDirectives();
            var body = ParseBlock();
            var semicolon2 = default(Terminal);

            if ((body != default) && (body.Body != default))
                semicolon2 = ContinueWithOrMissing(TokenKind.Semicolon);

            return new ProcedureDeclaration(attributes, heading, semicolon, directives, body, semicolon2);
        }

        #endregion
        #region ParseProcedureDeclarationHeading

        [Rule("ProcedureDeclarationHeading", "('procedure' | 'function') Identifier [FormalParameterSection][':' TypeSpecification]")]
        private ProcedureDeclarationHeading ParseProcedureDeclarationHeading() {
            var kindSymbol = ContinueWithOrMissing(TokenKind.Function, TokenKind.Procedure);
            var name = RequireIdentifier();
            var parameters = default(FormalParameterSection);
            var colonSymbol = default(Terminal);
            var resultTypeAttributes = default(UserAttributes);
            var resultType = default(TypeSpecification);

            if (Match(TokenKind.OpenParen)) {
                parameters = ParseFormalParameterSection();
            }

            colonSymbol = ContinueWith(TokenKind.Colon);

            if (colonSymbol != default) {
                resultTypeAttributes = ParseAttributes();
                resultType = ParseTypeSpecification();
            }

            return new ProcedureDeclarationHeading(kindSymbol, name, parameters, colonSymbol, resultTypeAttributes, resultType);
        }

        #endregion
        #region ParseExportsSection

        [Rule("ExportsSection", "'exports' ExportItem { ',' ExportItem } ';' ")]
        private ExportsSection ParseExportsSection() {
            var exports = ContinueWith(TokenKind.Exports);
            using (var list = GetList<ExportItemSymbol>()) {
                var item = default(ExportItemSymbol);
                do {
                    item = AddToList(list, ParseExportItem(true));
                } while (item != default && item.Comma.GetSymbolKind() == TokenKind.Comma);

                var semicolon = ContinueWithOrMissing(TokenKind.Semicolon);
                return new ExportsSection(exports, GetFixedArray(list), semicolon);
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
            var parameters = default(FormalParameters);
            var closeParen = default(Terminal);
            var indexSymbol = default(Terminal);
            var indexParameter = default(Expression);
            var nameSymbol = default(Terminal);
            var nameParameter = default(Expression);
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
            using (var list = GetList<Label>()) {
                var labelSymbol = ContinueWithOrMissing(TokenKind.Label);
                var label = default(Label);
                do {
                    label = AddToList(list, ParseLabel(true));
                } while (label != default && label.Comma.GetSymbolKind() == TokenKind.Comma);

                return new LabelDeclarationSection(labelSymbol, GetFixedArray(list), ContinueWithOrMissing(TokenKind.Semicolon));
            }
        }

        #endregion
        #region ParseLabel

        [Rule("Label", "Identifier | Integer | HexNumber")]
        private Label ParseLabel(bool allowComma = false) {

            if (MatchIdentifier())
                return new Label(RequireIdentifier(), allowComma ? ContinueWith(TokenKind.Comma) : default);

            if (Match(TokenKind.Integer))
                return new Label(RequireInteger(), allowComma ? ContinueWith(TokenKind.Comma) : default);

            if (Match(TokenKind.HexNumber))
                return new Label(RequireHexValue(), allowComma ? ContinueWith(TokenKind.Comma) : default);

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
            var typeSpecification = default(TypeSpecification);

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
        public HintingInformationList ParseHints(bool requireSemicolon) {
            var result = default(HintingInformationList);
            var hint = default(HintSymbol);

            using (var list = GetList<HintSymbol>()) {
                do {
                    hint = ParseHint();
                    if (hint != null && requireSemicolon) {
                        hint.Semicolon = ContinueWithOrMissing(TokenKind.Semicolon);
                    }
                    else if (hint != null) {
                        hint.Semicolon = EmptyTerminal();
                    }

                    if (hint != null) {
                        AddToList(list, hint);
                    }

                } while (hint != default);

                if (list.Item.Count > 0)
                    return new HintingInformationList(GetFixedArray(list));
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
        public HintSymbol ParseHint() {
            var result = new HintSymbol {
                Symbol = ContinueWith(TokenKind.Deprecated),
                Semicolon = EmptyTerminal()
            };

            if (result.Symbol != null) {
                result.Deprecated = true;
                if (Match(TokenKind.QuotedString))
                    result.DeprecatedComment = RequireString();
                else
                    result.DeprecatedComment = EmptyTerminal();

                return result;
            }
            else {
                result.DeprecatedComment = EmptyTerminal();
            }

            result.Symbol = ContinueWith(TokenKind.Experimental);
            if (result.Symbol != null) {
                result.Experimental = true;
                return result;
            }

            result.Symbol = ContinueWith(TokenKind.Platform);
            if (result.Symbol != null) {
                result.Platform = true;
                return result;
            }

            result.Symbol = ContinueWith(TokenKind.Library);
            if (result.Symbol != null) {
                result.Library = true;
                return result;
            }

            return null;
        }

        #endregion
        #region ParseTypeSpecification

        [Rule("TypeSpecification", "StructType | PointerType | StringType | ProcedureType | SimpleType ")]
        private TypeSpecification ParseTypeSpecification(bool constDeclaration = false, bool varDeclaration = false, bool allowComma = false) {
            var comma = default(Terminal);

            if (Match(TokenKind.Packed, TokenKind.Array, TokenKind.Set, TokenKind.File, //
                TokenKind.Class, TokenKind.Interface, TokenKind.Record, TokenKind.Object, TokenKind.DispInterface)) {
                var structuredType = ParseStructType();
                if (allowComma)
                    comma = ContinueWith(TokenKind.Comma);
                return new TypeSpecification(structuredType, comma);
            }

            if (Match(TokenKind.Pointer, TokenKind.Circumflex)) {
                var pointerType = ParsePointerType();
                if (allowComma)
                    comma = ContinueWith(TokenKind.Comma);
                return new TypeSpecification(pointerType, comma);
            }

            if (Match(TokenKind.String, TokenKind.ShortString, TokenKind.AnsiString, TokenKind.UnicodeString, TokenKind.WideString)) {
                var stringType = ParseStringType();
                if (allowComma)
                    comma = ContinueWith(TokenKind.Comma);
                return new TypeSpecification(stringType, comma);
            }

            if (Match(TokenKind.Function, TokenKind.Procedure)) {
                var procedureType = ParseProcedureType();
                if (allowComma)
                    comma = ContinueWith(TokenKind.Comma);
                return new TypeSpecification(procedureType, comma);
            }

            if (Match(TokenKind.Reference) && LookAhead(1, TokenKind.To)) {
                var procedureType = ParseProcedureType();
                if (allowComma)
                    comma = ContinueWith(TokenKind.Comma);
                return new TypeSpecification(procedureType, comma);
            }

            var simpleType = ParseSimpleType(constDeclaration, varDeclaration);
            if (allowComma)
                comma = ContinueWith(TokenKind.Comma);

            return new TypeSpecification(simpleType, comma);
        }

        #endregion
        #region ParseSimpleType

        [Rule("SimpleType", "EnumType | (ConstExpression [ '..' ConstExpression ]) | ([ 'type' ] GenericNamespaceName {'.' GenericNamespaceName })")]
        private SimpleType ParseSimpleType(bool constDeclaration = false, bool varDeclaration = false) {

            if (Match(TokenKind.OpenParen)) {
                return new SimpleType(ParseEnumType());
            }

            var newType = default(Terminal);
            var typeOf = default(Terminal);

            if (!varDeclaration) {
                newType = ContinueWith(TokenKind.TypeKeyword);

                if (newType != default)
                    typeOf = ContinueWith(TokenKind.Of);
            }
            else {
                if (Match(TokenKind.TypeKeyword)) {
                    Unexpected();
                }
            }

            if (newType != default || (MatchIdentifier(TokenKind.ShortString, TokenKind.String, TokenKind.WideString, TokenKind.UnicodeString, TokenKind.AnsiString) && (!LookAhead(1, TokenKind.DotDot)))) {
                using (var list = GetList<GenericNamespaceName>()) {
                    var item = default(GenericNamespaceName);

                    do {
                        item = AddToList(list, ParseGenericNamespaceName(false, false, true));
                    } while (item != default && item.Dot != default);

                    return new SimpleType(newType, typeOf, GetFixedArray(list));
                }
            }

            var subrangeStart = ParseConstantExpression(false, constDeclaration);
            var subrangeEnd = default(ConstantExpressionSymbol);
            var dotDot = ContinueWith(TokenKind.DotDot);
            if (dotDot != default) {
                subrangeEnd = ParseConstantExpression(false, constDeclaration);
            }

            return new SimpleType(newType, typeOf, subrangeStart, dotDot, subrangeEnd);
        }

        #endregion
        #region GenericNamespaceName

        [Rule("GenericNamespaceName", "NamespaceName [ GenericSuffix ]")]
        private GenericNamespaceName ParseGenericNamespaceName(bool advancedCheck = false, bool inDesignator = false, bool allowDot = false) {
            var name = ParseNamespaceName(false, inDesignator);
            var genericPart = default(GenericSuffix);

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
            return new GenericNamespaceName(name, genericPart, allowDot ? ContinueWith(TokenKind.Dot) : null);
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
            var value = default(Expression);

            if (equals != default)
                value = ParseExpression();

            var comma = ContinueWith(TokenKind.Comma);
            return new EnumValueSymbol(enumName, equals, value, comma);
        }

        #endregion
        #region ParseProcedureType

        [Rule("ProcedureType", "(ProcedureRefType [ 'of' 'object' ] ( | ProcedureReference")]
        private ProcedureType ParseProcedureType() {
            var result = new ProcedureType();

            if (Match(TokenKind.Procedure, TokenKind.Function)) {
                result.ProcedureRefType = ParseProcedureRefType(result);
                result.ProcedureRefType.AllowAnonymousMethods = false;

                if (ContinueWith(result, TokenKind.Of)) {
                    ContinueWithOrMissing(result, TokenKind.Object);
                    result.ProcedureRefType.MethodDeclaration = true;
                }

                return result;
            }

            if (Match(TokenKind.Reference)) {
                result.ProcedureReference = ParseProcedureReference(result);
                return result;
            }

            Unexpected();
            return result;
        }

        #endregion
        #region  ParseProcedureReference

        [Rule("ProcedureReference", "'reference' 'to' ProcedureTypeDefinition ")]
        private ProcedureReference ParseProcedureReference(IExtendableSyntaxPart parent) {
            var result = new ProcedureReference();
            InitByTerminal(result, parent, TokenKind.Reference);
            ContinueWithOrMissing(result, TokenKind.To);
            result.ProcedureType = ParseProcedureRefType(result);
            result.ProcedureType.AllowAnonymousMethods = true;
            return result;
        }

        #endregion
        #region ParseProcedureRefType

        [Rule("ProcedureTypeDefinition", "('function' | 'procedure') [ '(' FormalParameters ')' ] [ ':' TypeSpecification ] [ 'of' 'object']")]
        private ProcedureTypeDefinition ParseProcedureRefType(IExtendableSyntaxPart parent) {
            var result = new ProcedureTypeDefinition();
            InitByTerminal(result, parent, TokenKind.Function, TokenKind.Procedure);
            result.Kind = result.LastTerminalKind;

            if (Match(TokenKind.OpenParen)) {
                result.Parameters = ParseFormalParameterSection();
            }

            if (result.Kind == TokenKind.Function) {
                ContinueWithOrMissing(result, TokenKind.Colon);
                result.ReturnTypeAttributes = ParseAttributes();
                result.ReturnType = ParseTypeSpecification();
            }

            return result;
        }

        #endregion
        #region FormalParameterSecion

        [Rule("FormalParameterSection", "'(' [ FormalParameters ] ')'")]
        private FormalParameterSection ParseFormalParameterSection() {
            var openParen = ContinueWithOrMissing(TokenKind.OpenParen);
            var parameters = default(FormalParameters);

            if (!Match(TokenKind.CloseParen)) {
                parameters = ParseFormalParameters();
            }

            var closeParen = ContinueWithOrMissing(TokenKind.CloseParen);
            return new FormalParameterSection(openParen, parameters, closeParen);
        }

        #endregion
        #region ParseStringType

        [Rule("StringType", "ShortString | WideString | UnicodeString |('string' [ '[' Expression ']'  ]) | ('AnsiString' '(' ConstExpression ')') ")]
        private StringType ParseStringType() {

            if (Match(TokenKind.String)) {
                var stringSymbol = ContinueWith(TokenKind.String);
                var openBraces = ContinueWith(TokenKind.OpenBraces);
                var stringLength = default(ConstantExpressionSymbol);
                var closeBraces = default(Terminal);

                if (openBraces != default) {
                    stringLength = ParseConstantExpression();
                    closeBraces = ContinueWithOrMissing(TokenKind.CloseBraces);
                };
                return new StringType(stringSymbol, openBraces, stringLength, closeBraces);
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
                return new StringType(stringSymbol, openParen, codePage, closeParen);
            }

            if (Match(TokenKind.ShortString, TokenKind.WideString, TokenKind.UnicodeString)) {
                return new StringType(ContinueWith(TokenKind.ShortString, TokenKind.WideString, TokenKind.UnicodeString));
            }

            Unexpected();
            return null;
        }

        #endregion
        #region ParseStructType

        [Rule("StructType", "[ 'packed' ] StructTypePart")]
        private StructType ParseStructType() {
            var result = new StructType();
            result.Packed = ContinueWith(result, TokenKind.Packed);
            result.Part = ParseStructTypePart(result);
            return result;
        }

        #endregion
        #region ParseStructTypePart

        [Rule("StructTypePart", "ArrayType | SetType | FileType | ClassDecl")]
        private StructTypePart ParseStructTypePart(IExtendableSyntaxPart parent) {
            var result = new StructTypePart();
            parent.Add(result);

            if (Match(TokenKind.Array)) {
                result.ArrayType = ParseArrayType();
                return result;
            }

            if (Match(TokenKind.Set)) {
                result.SetType = ParseSetDefinition(result);
                return result;
            }

            if (Match(TokenKind.File)) {
                result.FileType = ParseFileType(result);
                return result;
            }

            if (Match(TokenKind.Class, TokenKind.Interface, TokenKind.Record, TokenKind.Object, TokenKind.DispInterface)) {
                result.ClassDeclaration = ParseClassDeclaration();
                return result;
            }

            return result;
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

        [Rule("RecordDecl", "'record' RecordFieldList (RecordVariantSection | RecordItems ) 'end' ")]
        private RecordDeclaration ParseRecordDecl() {
            var recordSymbol = ContinueWithOrMissing(TokenKind.Record);
            var fieldList = default(RecordFieldList);
            var variantSection = default(RecordVariantSection);
            var items = default(RecordItems);

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
            return new RecordDeclaration(recordSymbol, fieldList, variantSection, items, endSymbol);
        }

        #endregion
        #region ParseRecordItems

        [Rule("RecordItems", "{ RecordItem }")]
        private RecordItems ParseRecordItems() {
            var result = new RecordItems();
            var mode = RecordDeclarationMode.Fields;

            while ((!Match(TokenKind.End)) && (mode != RecordDeclarationMode.Undefined)) {
                ParseRecordItem(result, ref mode);

                if (mode == RecordDeclarationMode.Undefined && result.PartList.Count > 0) {
                    Unexpected();
                    return result;
                }
            }

            return result;
        }

        #endregion
        #region ParseRecordItem

        [Rule("RecordItem", "MethodDeclaration | PropertyDeclaration | ConstSection | TypeSection | RecordField | ( ['class'] VarSection) | Visibility ")]
        private RecordItem ParseRecordItem(IExtendableSyntaxPart parent, ref RecordDeclarationMode mode) {

            if (ContinueWith(parent, TokenKind.Var)) {
                mode = RecordDeclarationMode.Fields;
                return null;
            }

            if (Match(TokenKind.Class) && LookAhead(1, TokenKind.Var)) {
                ContinueWith(parent, TokenKind.Class);
                ContinueWith(parent, TokenKind.Var);
                mode = RecordDeclarationMode.ClassFields;
                return null;
            }

            var result = new RecordItem();
            parent.Add(result);

            if (Match(TokenKind.OpenBraces)) {
                result.Attributes1 = ParseAttributes();
            }

            result.ClassItem = ContinueWith(result, TokenKind.Class);

            if (Match(TokenKind.OpenBraces)) {
                result.Attributes2 = ParseAttributes();
            }

            if (Match(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Strict, TokenKind.Published, TokenKind.Automated)) {
                if (result.ClassItem) {
                    Unexpected();
                }
                else {
                    result.Strict = ContinueWith(result, TokenKind.Strict);
                    ContinueWith(result, TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Published, TokenKind.Automated);
                    result.Visibility = result.LastTerminalKind;
                }
                mode = RecordDeclarationMode.Fields;
                return result;
            }

            if (Match(TokenKind.Procedure, TokenKind.Function, TokenKind.Constructor, TokenKind.Destructor, TokenKind.Operator)) {
                result.MethodDeclaration = ParseMethodDeclaration();
                mode = RecordDeclarationMode.Other;
                return result;
            }

            if (Match(TokenKind.Property)) {
                result.PropertyDeclaration = ParsePropertyDeclaration();
                mode = RecordDeclarationMode.Other;
                return result;
            }

            if (Match(TokenKind.Case)) {
                result.VariantSection = ParseRecordVariantSection();
                return result;
            }

            if (!result.ClassItem && Match(TokenKind.Const)) {
                result.ConstSection = ParseConstSection(true);
                return result;
            }

            if (!result.ClassItem && Match(TokenKind.TypeKeyword)) {
                result.TypeSection = ParseTypeSection(true);
                return result;
            }

            if (MatchIdentifier() && (!Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Automated, TokenKind.Strict))) {
                if (mode == RecordDeclarationMode.Fields || mode == RecordDeclarationMode.ClassFields) {
                    result.ClassItem = mode == RecordDeclarationMode.ClassFields;
                    result.Fields = ParseRecordFieldList(true);
                    return result;
                }
                else {
                    Unexpected();
                }
            }

            mode = RecordDeclarationMode.Undefined;
            return result;
        }

        #endregion
        #region ParseRecordVariantSection

        [Rule("RecordVariantSection", "'case' [ Identifier ': ' ] TypeDeclaration 'of' { RecordVariant } ")]
        private RecordVariantSection ParseRecordVariantSection() {
            var result = new RecordVariantSection();
            InitByTerminal(result, null, TokenKind.Case);

            if (MatchIdentifier() && LookAhead(1, TokenKind.Colon)) {
                result.Name = RequireIdentifier();
                ContinueWithOrMissing(result, TokenKind.Colon);
            }
            result.TypeDeclaration = ParseTypeSpecification();
            ContinueWithOrMissing(result, TokenKind.Of);

            while (!Match(TokenKind.Undefined, TokenKind.End)) {
                ParseRecordVariant(result);
                ContinueWith(result, TokenKind.Semicolon);
            }

            return result;
        }

        #endregion
        #region ParseRecordVariant

        [Rule("RecordVariant", "ConstantExpression { , ConstantExpression } : '(' FieldList ')' ';' ")]
        private RecordVariant ParseRecordVariant(IExtendableSyntaxPart parent) {
            var result = new RecordVariant();
            parent.Add(result);

            do {
                ParseConstantExpression();
            } while (ContinueWith(result, TokenKind.Comma));

            ContinueWithOrMissing(result, TokenKind.Colon);
            ContinueWithOrMissing(result, TokenKind.OpenParen);
            result.FieldList = ParseRecordFieldList(false);
            ContinueWithOrMissing(result, TokenKind.CloseParen);
            return result;
        }

        #endregion
        #region ParseRecordFieldList

        [Rule("RecordFieldList", " { RecordField } ")]
        private RecordFieldList ParseRecordFieldList(bool requireSemicolon) {
            using (var list = GetList<RecordField>()) {
                while (MatchIdentifier() && (!Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Strict))) {
                    AddToList(list, ParseRecordField(requireSemicolon));
                }
                return new RecordFieldList(GetFixedArray(list));
            }
        }

        #endregion
        #region ParseRecordField

        [Rule("RecordField", "IdentList ':' TypeSpecification Hints ';'")]
        private RecordField ParseRecordField(bool requireSemicolon) {
            var names = ParseIdentList(true);
            var colonSymbol = ContinueWithOrMissing(TokenKind.Colon);
            var fieldType = ParseTypeSpecification();
            var hint = ParseHints(false);
            var semicolon = default(Terminal);

            if (Match(TokenKind.Semicolon) || (requireSemicolon && (!Match(TokenKind.End))))
                semicolon = ContinueWithOrMissing(TokenKind.Semicolon);

            return new RecordField(names, colonSymbol, fieldType, hint, semicolon);
        }

        #endregion
        #region ParseRecordHelper

        [Rule("RecordHelperDecl", "'record' 'helper' 'for' TypeName RecordHelperItems 'end'")]
        private RecordHelperDefinition ParseRecordHelper() {
            var recordSymbol = ContinueWithOrMissing(TokenKind.Record);
            var helperSymbol = ContinueWithOrMissing(TokenKind.Helper);
            var forSymbol = ContinueWithOrMissing(TokenKind.For);
            var name = ParseTypeName();
            var items = ParseRecordHelperItems();
            var endSymbol = ContinueWithOrMissing(TokenKind.End);

            return new RecordHelperDefinition(recordSymbol, helperSymbol, forSymbol, name, items, endSymbol);
        }

        #endregion
        #region ParseRecordHelperItems

        [Rule("RecordHelperItems", " { RecordHelperItem }")]
        private RecordHelperItems ParseRecordHelperItems() {
            var result = new RecordHelperItems();
            var mode = RecordDeclarationMode.Fields;

            while ((!Match(TokenKind.End, TokenKind.Undefined)) && (mode != RecordDeclarationMode.Undefined)) {
                ParseRecordHelperItem(result, ref mode);
                if (mode == RecordDeclarationMode.Undefined) {
                    Unexpected();
                    return result;
                }
            }

            return result;
        }

        #endregion
        #region ParseRecordHelperItem

        [Rule("RecordHelperItem", "MethodDeclaration | PropertyDeclaration | ConstSection | TypeSection | Visibility ")]
        private RecordHelperItem ParseRecordHelperItem(IExtendableSyntaxPart parent, ref RecordDeclarationMode mode) {

            if (ContinueWith(parent, TokenKind.Var)) {
                mode = RecordDeclarationMode.Fields;
                return null;
            }

            if (Match(TokenKind.Class) && LookAhead(1, TokenKind.Var)) {
                ContinueWith(parent, TokenKind.Class);
                ContinueWith(parent, TokenKind.Var);
                mode = RecordDeclarationMode.ClassFields;
                return null;
            }

            var result = new RecordHelperItem();
            parent.Add(result);

            result.Attributes1 = ParseAttributes();
            result.ClassItem = ContinueWith(result, TokenKind.Class);
            result.Attributes2 = ParseAttributes();

            if (Match(TokenKind.Const)) {
                result.ConstDeclaration = ParseConstSection(true);
                mode = RecordDeclarationMode.Other;
                return result;
            }

            if (!result.ClassItem && Match(TokenKind.TypeKeyword)) {
                result.TypeSection = ParseTypeSection(true);
                return result;
            }

            if (Match(TokenKind.Procedure, TokenKind.Function, TokenKind.Constructor, TokenKind.Destructor)) {
                result.MethodDeclaration = ParseMethodDeclaration();
                mode = RecordDeclarationMode.Other;
                return result;
            }

            if (Match(TokenKind.Property)) {
                mode = RecordDeclarationMode.Other;
                result.PropertyDeclaration = ParsePropertyDeclaration();
                return result;
            }

            if (Match(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Strict, TokenKind.Published)) {
                result.Strict = ContinueWith(result, TokenKind.Strict);
                ContinueWith(result, TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Published);
                result.Visibility = result.LastTerminalKind;
                mode = RecordDeclarationMode.Other;
                return result;
            }

            if (MatchIdentifier() && (!Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Automated, TokenKind.Strict))) {

                if (mode == RecordDeclarationMode.Fields || mode == RecordDeclarationMode.ClassFields) {
                    result.FieldDeclaration = ParseClassFieldDeclararation();
                    result.ClassItem = mode == RecordDeclarationMode.ClassFields;
                    return result;
                }
                else {
                    Unexpected();
                }
            }

            mode = RecordDeclarationMode.Undefined;
            return result;
        }

        #endregion
        #region ParseObjectDecl

        [Rule("ObjectDecl", "'object' ClassParent ObjectItems 'end' ")]
        private ObjectDeclaration ParseObjectDecl() {
            var objectSymbol = ContinueWith(TokenKind.Object);
            var classParent = ParseClassParent();
            var items = ParseObjectItems();
            var endSymbol = ContinueWithOrMissing(TokenKind.End);
            return new ObjectDeclaration(objectSymbol, classParent, items, endSymbol);
        }

        #endregion
        #region ParseObjectItems

        [Rule("ObjectItems", " { ObjectItem } ")]
        private ObjectItems ParseObjectItems() {
            var result = new ObjectItems();
            var mode = ClassDeclarationMode.Fields;

            while ((!Match(TokenKind.End)) && (mode != ClassDeclarationMode.Undefined)) {
                ParseObjectItem(result, ref mode);
                if (mode == ClassDeclarationMode.Undefined) {
                    Unexpected();
                    return result;
                }
            }

            return result;
        }

        #endregion
        #region ParseObjectItem

        [Rule("ObjectItem", "Visibility | MethodDeclaration | ClassFieldDeclaration | PropertyDeclaration ")]
        private ObjectItem ParseObjectItem(IExtendableSyntaxPart parent, ref ClassDeclarationMode mode) {
            var result = new ObjectItem();
            parent.Add(result);

            if (Match(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Strict, TokenKind.Published, TokenKind.Automated)) {
                result.Strict = ContinueWith(result, TokenKind.Strict);
                ContinueWith(result, TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Published, TokenKind.Automated);
                result.Visibility = result.LastTerminalKind;
                mode = ClassDeclarationMode.Other;
                return result;
            }

            if (Match(TokenKind.Procedure, TokenKind.Function, TokenKind.Constructor, TokenKind.Destructor)) {
                result.MethodDeclaration = ParseMethodDeclaration();
                mode = ClassDeclarationMode.Other;
                return result;
            }

            if (Match(TokenKind.Property)) {
                mode = ClassDeclarationMode.Other;
                result.Property = ParsePropertyDeclaration();
                return result;
            }

            if (!result.ClassItem && Match(TokenKind.Const)) {
                result.ConstSection = ParseConstSection(true);
                return result;
            }

            if (!result.ClassItem && Match(TokenKind.TypeKeyword)) {
                result.TypeSection = ParseTypeSection(true);
                return result;
            }

            if (MatchIdentifier()) {
                result.FieldDeclaration = ParseClassFieldDeclararation();
                mode = ClassDeclarationMode.Fields;
                return result;
            }


            mode = ClassDeclarationMode.Undefined;
            return result;
        }

        #endregion
        #region ParseInterfaceDef

        [Rule("InterfaceDef", "('interface' | 'dispinterface') ClassParent [InterfaceGuid] InterfaceDefItems 'end'")]
        private InterfaceDefinition ParseInterfaceDef() {
            var interfaceSymbol = ContinueWithOrMissing(TokenKind.Interface, TokenKind.DispInterface);
            var parentInterface = ParseClassParent();
            var guid = default(InterfaceGuid);

            if (Match(TokenKind.OpenBraces))
                guid = ParseInterfaceGuid();

            var items = ParseInterfaceItems();
            var end = default(Terminal);

            if (items != default && items.Items != default && items.Items.Length > 0)
                end = ContinueWithOrMissing(TokenKind.End);
            else
                end = ContinueWith(TokenKind.End);

            return new InterfaceDefinition(interfaceSymbol, parentInterface, guid, items, end);
        }

        #endregion
        #region  ParseInterfaceItems

        [Rule("InterfaceItems", "{ InterfaceItem }")]
        private InterfaceItems ParseInterfaceItems() {
            var unexpected = false;

            using (var list = GetList<InterfaceItem>()) {

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

        [Rule("InterfaceItem", "MethodDeclaration | PropertyDeclaration")]
        private InterfaceItem ParseInterfaceItem(out bool unexpected) {
            var result = new InterfaceItem();
            unexpected = true;

            if (Match(TokenKind.Procedure, TokenKind.Function)) {
                unexpected = false;
                result.Method = ParseMethodDeclaration();
                return result;
            }

            if (Match(TokenKind.Property)) {
                unexpected = false;
                result.Property = ParsePropertyDeclaration();
                return result;
            }

            return result;
        }

        #endregion
        #region ParseInterfaceGuid

        [Rule("InterfaceGuid", "'[' ( QuotedString ) | Identifier ']'")]
        private InterfaceGuid ParseInterfaceGuid() {
            var result = new InterfaceGuid();
            InitByTerminal(result, null, TokenKind.OpenBraces);

            if (Match(TokenKind.Identifier))
                result.IdIdentifier = RequireIdentifier();
            else
                result.Id = RequireString();

            ContinueWithOrMissing(result, TokenKind.CloseBraces);
            return result;
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
                    return new ClassDeclarationItemSymbol(ParseClassFieldDeclararation());
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
            var arrayIndex = default(FormalParameters);
            var closeBraces = default(Terminal);
            var colonSymbol = default(Terminal);
            var typeName = default(TypeName);
            var indexSymbol = default(Terminal);
            var propertyIndex = default(Expression);

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
                var defaultProperty = default(Expression);

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

        [Rule("TypeSection", "'type' TypeDeclaration { TypeDeclaration }")]
        private TypeSection ParseTypeSection(bool inClassDeclaration) {
            var typeKeyword = ContinueWithOrMissing(TokenKind.TypeKeyword);

            using (var list = GetList<TypeDeclaration>()) {
                while ((!inClassDeclaration || !Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Automated, TokenKind.Strict)) && MatchIdentifier(TokenKind.OpenBraces)) {
                    AddToList(list, ParseTypeDeclaration());
                };

                return new TypeSection(typeKeyword, GetFixedArray(list));
            }
        }

        #endregion
        #region ParseTypeDeclaration

        [Rule("TypeDeclaration", "[ Attributes ] GenericTypeIdent '=' TypeDeclaration Hints ';' ")]
        private TypeDeclaration ParseTypeDeclaration()
            => new TypeDeclaration(
                attributes: ParseAttributes(),
                genericTypeIdent: ParseGenericTypeIdent(),
                equalsSign: ContinueWithOrMissing(TokenKind.EqualsSign),
                typeSpecification: ParseTypeSpecification(),
                hints: ParseHints(false),
                semicolon: ContinueWithOrMissing(TokenKind.Semicolon));

        #endregion
        #region ParseGenericTypeIdent

        [Rule("GenericTypeIdent", "Ident [ GenericDefintion ] ")]
        private GenericTypeIdentifier ParseGenericTypeIdent() {
            var identifier = RequireIdentifier();
            var genericDefinition = default(GenericDefinition);

            if (Match(TokenKind.AngleBracketsOpen)) {
                genericDefinition = ParseGenericDefinition();
            }

            return new GenericTypeIdentifier(identifier, genericDefinition);
        }

        #endregion
        #region ParseMethodResolution

        [Rule("MethodResolution", "( 'function' | 'procedure' ) NamespaceName '=' Identifier ';' ")]
        private MethodResolution ParseMethodResolution() {
            var result = new MethodResolution();
            InitByTerminal(result, null, TokenKind.Function, TokenKind.Procedure);
            result.Kind = result.LastTerminalKind;
            result.Name = ParseTypeName();
            ContinueWithOrMissing(result, TokenKind.EqualsSign);
            result.ResolveIdentifier = RequireIdentifier();
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
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
            var genericDefinition = default(GenericDefinition);
            var openParen = default(Terminal);
            var parameters = default(FormalParameters);
            var closeParen = default(Terminal);
            var colonSymbol = default(Terminal);
            var resultAttributes = default(UserAttributes);
            var resultType = default(TypeSpecification);
            var semicolon = default(Terminal);
            var directives = default(MethodDirectives);

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

        [Rule("FormalParameters", "FormalParameter { ';' FormalParameter }")]
        private FormalParameters ParseFormalParameters() {

            using (var list = GetList<FormalParameterDefinition>()) {
                var item = default(FormalParameterDefinition);

                do {
                    if (!Match(TokenKind.CloseParen))
                        item = AddToList(list, ParseFormalParameterDefinition(true));
                } while (item != default && item.Semicolon != default);

                return new FormalParameters(GetFixedArray(list));
            }
        }

        #endregion
        #region FormalParameter

        private FormalParameter ParseFormalParameter(bool allowComma, ref int kind) {

            var attributes1 = default(UserAttributes);
            var attributes2 = default(UserAttributes);
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

            return new FormalParameter(attributes1, parameterKind, attributes2, parameterName, comma);
        }

        [Rule("FormalParameter", "[Attributes] [( 'const' | 'var' | 'out' )] [Attributes] IdentList [ ':' TypeDeclaration ] [ '=' Expression ]")]
        private FormalParameterDefinition ParseFormalParameterDefinition(bool allowSemicolon) {

            using (var list = GetList<FormalParameter>()) {
                var item = default(FormalParameter);
                var typeDef = default(TypeSpecification);
                var colon = default(Terminal);
                var equals = default(Terminal);
                var defaultValue = default(Expression);
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

                return new FormalParameterDefinition(GetFixedArray(list), colon, typeDef, equals, defaultValue, semicolon);
            }
        }

        #endregion
        #region ParseIdentList

        [Rule("IdentList", "Identifier { ',' Identifier }")]
        private IdentifierList ParseIdentList(bool allowAttributes) {
            var item = default(IdentifierListItem);

            using (var list = GetList<IdentifierListItem>()) {
                do {
                    var attributes = default(UserAttributes);

                    if (allowAttributes && Match(TokenKind.OpenBraces))
                        attributes = ParseAttributes();

                    var identifier = RequireIdentifier();
                    var comma = ContinueWith(TokenKind.Comma);
                    item = AddToList(list, new IdentifierListItem(attributes, identifier, comma));

                } while (item != default && item.Comma != default);

                return new IdentifierList(GetFixedArray(list));
            }
        }

        #endregion
        #region ParseGenericDefinition

        [Rule("GenericDefinition", "SimpleGenericDefinition | ConstrainedGenericDefinition")]
        private GenericDefinition ParseGenericDefinition() {
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
        public GenericDefinition ParseSimpleGenericDefinition() {
            var openBrackets = ContinueWith(TokenKind.AngleBracketsOpen);
            using (var list = GetList<GenericDefinitionPart>()) {
                var item = default(GenericDefinitionPart);

                do {
                    item = AddToList(list, new GenericDefinitionPart(RequireIdentifier(), ContinueWith(TokenKind.Comma)));
                } while (item != default && item.CommaOrSemicolon != default);

                var closeBrackets = ContinueWithOrMissing(TokenKind.AngleBracketsClose);
                return new GenericDefinition(openBrackets, GetFixedArray(list), closeBrackets);
            }
        }

        #endregion
        #region ParseConstrainedGenericDefinition

        [Rule("ConstrainedGenericDefinition", "'<' GenericDefinitionPart { ';' GenericDefinitionPart } '>'")]
        private GenericDefinition ParseConstrainedGenericDefinition() {
            var openBrackets = ContinueWith(TokenKind.AngleBracketsOpen);
            using (var list = GetList<GenericDefinitionPart>()) {
                var item = default(GenericDefinitionPart);

                do {
                    item = AddToList(list, new GenericDefinitionPart(ParseGenericDefinitionPart(), ContinueWith(TokenKind.Semicolon)));
                } while (item != default && item.CommaOrSemicolon != default);

                var closeBrackets = ContinueWithOrMissing(TokenKind.AngleBracketsClose);
                return new GenericDefinition(openBrackets, GetFixedArray(list), closeBrackets);
            }
        }

        #endregion
        #region ParseGenericDefinitionPart

        [Rule("GenericDefinitionPart", "Identifier [ ':' GenericConstraint { ',' GenericConstraint } ]")]
        private GenericDefinitionPart ParseGenericDefinitionPart() {
            var identifier = RequireIdentifier();
            var colon = ContinueWith(TokenKind.Colon);

            if (colon != default) {
                using (var list = GetList<ConstrainedGenericSymbol>()) {
                    var item = default(ConstrainedGenericSymbol);

                    do {
                        item = AddToList(list, ParseGenericConstraint(true));
                    } while (item != default && item.Comma != default);

                    return new GenericDefinitionPart(identifier, colon, GetFixedArray(list));
                };
            }

            return new GenericDefinitionPart(identifier);
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
            var identifier = default(Identifier);

            if (constraintSymbol == null)
                identifier = RequireIdentifier();

            if (allowComma)
                comma = ContinueWith(TokenKind.Comma);

            return new ConstrainedGenericSymbol(constraintSymbol, identifier, comma);
        }

        #endregion
        #region ParseClassParent

        [Rule("ClassParent", " [ '(' TypeName { ',' TypeName } ')' ]")]
        private ParentClass ParseClassParent() {
            if (Match(TokenKind.OpenParen)) {
                using (var list = GetList<TypeName>()) {
                    var openParen = ContinueWith(TokenKind.OpenParen);
                    var item = default(TypeName);
                    do {
                        item = AddToList(list, ParseTypeName(false, true));
                    } while (item != default && item.Comma != default);

                    var closeParen = ContinueWithOrMissing(TokenKind.CloseParen);
                    return new ParentClass(openParen, GetFixedArray(list), closeParen);
                }
            }

            return null;
        }

        #endregion
        #region ParseClassOfDeclaration

        /// <summary>
        ///     class of declaration
        /// </summary>
        /// <returns></returns>

        [Rule("ClassOfDeclaration", "'class' 'of' TypeName")]
        public ClassOfDeclarationSymbol ParseClassOfDeclaration() {
            return new ClassOfDeclarationSymbol(
                classSymbol: ContinueWithOrMissing(TokenKind.Class),
                ofSymbol: ContinueWithOrMissing(TokenKind.Of),
                typeName: ParseTypeName());
        }

        #endregion
        #region ParseTypeName

        [Rule("TypeName", "'string' | 'ansistring' | 'shortstring' | 'unicodestring' | 'widestring' | (NamespaceName [ GenericSuffix ]) ")]
        private TypeName ParseTypeName(bool inDesignator = false, bool allowComma = false) {
            var stringType = ContinueWith(TokenKind.String, TokenKind.AnsiString, TokenKind.ShortString, TokenKind.UnicodeString, TokenKind.WideString);

            if (stringType != default)
                return new TypeName(stringType, allowComma ? ContinueWith(TokenKind.Comma) : default);

            using (var list = GetList<GenericNamespaceName>()) {
                var name = default(GenericNamespaceName);
                do {
                    name = AddToList(list, ParseGenericNamespaceName(true, inDesignator, true));
                } while ((!inDesignator || !IsLastDisignatorPart()) && name?.Dot != default);

                return new TypeName(GetFixedArray(list), allowComma ? ContinueWith(TokenKind.Comma) : default);
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

        [Rule("FileType", "'file' [ 'of' TypeSpecification ]")]
        private FileType ParseFileType(IExtendableSyntaxPart parent) {
            var result = new FileType();
            InitByTerminal(result, parent, TokenKind.File);

            if (ContinueWith(result, TokenKind.Of)) {
                result.TypeDefinition = ParseTypeSpecification();
            }

            return result;
        }

        #endregion
        #region SetDefinition

        [Rule("SetDef", "'set' 'of' TypeSpecification")]
        private SetDefinition ParseSetDefinition(IExtendableSyntaxPart parent) {
            var result = new SetDefinition();
            InitByTerminal(result, parent, TokenKind.Set);
            ContinueWithOrMissing(result, TokenKind.Of);
            result.TypeDefinition = ParseTypeSpecification();
            return result;
        }

        #endregion
        #region ParseGenericSuffix

        [Rule("GenericSuffix", "'<' TypeDefinition { ',' TypeDefinition '}' '>'")]
        private GenericSuffix ParseGenericSuffix() {
            var openBracket = ContinueWithOrMissing(TokenKind.AngleBracketsOpen);

            using (var list = GetList<TypeSpecification>()) {
                var typeSpecification = default(TypeSpecification);
                do {
                    typeSpecification = AddToList(list, ParseTypeSpecification(false, false, true));
                } while ((!Tokenizer.AtEof) && typeSpecification?.Comma != default);

                var closeBracket = ContinueWithOrMissing(TokenKind.AngleBracketsClose);
                return new GenericSuffix(openBracket, GetFixedArray(list), closeBracket);
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
            var typeSpecification = default(TypeSpecification);

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
        public ArrayIndexSymbol ParseArrayIndex() {
            return new ArrayIndexSymbol(
                startIndex: ParseConstantExpression(),
                dotDot: ContinueWith(TokenKind.DotDot, out var hasDots),
                endIndex: hasDots ? ParseConstantExpression() : null,
                comma: ContinueWith(TokenKind.Comma)
            );
        }

        #endregion
        #region ParsePointerType

        [Rule("PointerType", "( 'pointer' | '^' TypeSpecification )")]
        private PointerType ParsePointerType() {
            var result = new PointerType();

            if (ContinueWith(result, TokenKind.Pointer)) {
                result.GenericPointer = true;
                return result;
            }

            ContinueWithOrMissing(result, TokenKind.Circumflex);
            result.TypeSpecification = ParseTypeSpecification();
            return result;
        }

        #endregion
        #region ParseAttributes

        [Rule("Attributes", "{ '[' AttributeSet | AssemblyAttribue ']' }")]
        private UserAttributes ParseAttributes() {
            using (var list = GetList<SyntaxPartBase>()) {
                while (Match(TokenKind.OpenBraces)) {
                    AddToList(list, ParseAttributeSet());
                }

                return new UserAttributes(GetFixedArray(list));
            }
        }

        #endregion
        #region ParseAttributeSet

        [Rule("AttributeSet", " '[' Attribute { ',' Attribute } ']' ")]
        public UserAttributeSet ParseAttributeSet() {
            var openBraces = ContinueWith(TokenKind.OpenBraces);
            using (var list = GetList<UserAttributeDefinition>()) {
                var item = default(UserAttributeDefinition);

                do {
                    item = AddToList(list, ParseAttribute(true));
                } while (item != default && item.Comma != default);

                var closeBraces = ContinueWith(TokenKind.CloseBraces);

                return new UserAttributeSet(openBraces, GetFixedArray(list), closeBraces);
            }
        }

        [Rule("Attribute", " [ 'Result' ':' ] NamespaceName [ '(' Expressions ')' ]")]
        private UserAttributeDefinition ParseAttribute(bool allowComma) {

            var prefix = default(Identifier);
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

            return new UserAttributeDefinition(prefix, colon, name, openParen, expressions, closeParen, comma);
        }

        #endregion
        #region Expressions

        [Rule("Expressions", "Expression { ',' Expression }")]
        private ExpressionList ParseExpressions() {
            using (var list = GetList<Expression>()) {
                var item = default(Expression);

                do {
                    item = AddToList(list, ParseExpression());
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
            var expr = default(Expression);
            var openParen = default(Terminal);
            var closeParen = default(Terminal);
            var comma = default(Terminal);

            if (Match(TokenKind.OpenParen)) {

                if (LookAhead(1, TokenKind.CloseParen) || (LookAheadIdentifier(1, Array.Empty<int>(), true) && (LookAhead(2, TokenKind.Colon)))) {
                    openParen = ContinueWithOrMissing(TokenKind.OpenParen);
                    var record = default(RecordConstantExpression);
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

        [Rule("RecordConstantExpression", "Identifier ':' ConstantExpression")]
        private RecordConstantExpression ParseRecordConstant(bool allowSemicolon) {
            var name = RequireIdentifier();
            var colon = ContinueWithOrMissing(TokenKind.Colon);
            var value = ParseConstantExpression();
            var semicolon = allowSemicolon ? ContinueWith(TokenKind.Semicolon) : default;
            return new RecordConstantExpression(name, colon, value, semicolon);
        }

        #endregion
        #region ParseExpression

        [Rule("Expression", "SimpleExpression [ ('<'|'<='|'>'|'>='|'<>'|'='|'in'|'is') SimpleExpression ] | ClosureExpression")]
        private Expression ParseExpression(bool fromConstTypeDeclaration = false, bool allowComma = false) {
            var comma = default(Terminal);

            if (Match(TokenKind.Function, TokenKind.Procedure)) {
                var closureExpression = ParseClosureExpression();
                if (allowComma)
                    comma = ContinueWith(TokenKind.Comma);
                return new Expression(closureExpression, comma);
            }

            var leftOperand = ParseSimpleExpression();
            var rightOperand = default(SimpleExpression);

            if (fromConstTypeDeclaration && !HasTokenBeforeToken(TokenKind.EqualsSign, new[] { TokenKind.Semicolon }))
                return new Expression(leftOperand);

            var @operator = ContinueWith(TokenKind.LessThen, TokenKind.LessThenEquals, TokenKind.GreaterThen, TokenKind.GreaterThenEquals, TokenKind.NotEquals, TokenKind.EqualsSign, TokenKind.In, TokenKind.Is);

            if (@operator != default)
                rightOperand = ParseSimpleExpression();

            if (allowComma)
                comma = ContinueWith(TokenKind.Comma);

            return new Expression(leftOperand, @operator, rightOperand, comma);
        }

        #endregion
        #region ParseSimpleExpression

        [Rule("SimpleExpression", "Term { ('+'|'-'|'or'|'xor') SimpleExpression }")]
        private SimpleExpression ParseSimpleExpression() {
            var leftOperand = ParseTerm();
            var @operator = ContinueWith(TokenKind.Plus, TokenKind.Minus, TokenKind.Or, TokenKind.Xor);
            var rightOperand = default(SimpleExpression);

            if (@operator != default)
                rightOperand = ParseSimpleExpression();

            return new SimpleExpression(leftOperand, @operator, rightOperand);
        }

        #endregion
        #region ParseTerm

        [Rule("Term", "Factor [ ('*'|'/'|'div'|'mod'|'and'|'shl'|'shr'|'as') Term ]")]
        private Term ParseTerm() {
            var leftOperand = ParseFactor();
            var @operator = ContinueWith(TokenKind.Times, TokenKind.Slash, TokenKind.Div, TokenKind.Mod, TokenKind.And, TokenKind.Shl, TokenKind.Shr, TokenKind.As);
            var rightOperand = default(Term);

            if (@operator != default)
                rightOperand = ParseTerm();

            return new Term(leftOperand, @operator, rightOperand);
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

        [Rule("Factor", "'@' Factor  | 'not' Factor | '+' Factor | '-' Factor | '^' Identifier | Integer | HexNumber | Real | 'true' | 'false' | 'nil' | '(' Expression ')' | String | SetSection | Designator | TypeCast")]
        private Factor ParseFactor() {
            var designator = default(DesignatorStatementSymbol);
            var unaryOperator = ContinueWith(TokenKind.At, TokenKind.Not, TokenKind.Plus, TokenKind.Minus);

            if (unaryOperator != default)
                return new Factor(unaryOperator, ParseFactor());

            unaryOperator = ContinueWith(TokenKind.Circumflex);

            if (unaryOperator != default)
                return new Factor(unaryOperator, RequireIdentifier());

            if (Match(TokenKind.Integer)) {
                var intValue = RequireInteger();

                if (Match(TokenKind.Dot))
                    designator = ParseDesignator();

                return new Factor(intValue, designator);
            }

            if (Match(TokenKind.HexNumber)) {
                var hexValue = RequireHexValue();

                if (Match(TokenKind.Dot))
                    designator = ParseDesignator();

                return new Factor(hexValue, designator);
            }

            if (Match(TokenKind.Real)) {
                var realValue = RequireRealValue();

                if (Match(TokenKind.Dot))
                    designator = ParseDesignator();

                return new Factor(realValue, designator);
            }

            if (Match(TokenKind.QuotedString)) {
                var stringValue = RequireString();

                if (Match(TokenKind.Dot))
                    designator = ParseDesignator();

                return new Factor(stringValue, designator);
            }

            if (Match(TokenKind.True) || Match(TokenKind.False)) {
                var booleanValue = ContinueWith(TokenKind.True, TokenKind.False);

                if (Match(TokenKind.Dot))
                    designator = ParseDesignator();

                return new Factor(booleanValue, designator);
            }

            if (Match(TokenKind.Nil))
                return new Factor(ContinueWith(TokenKind.Nil));

            if (Match(TokenKind.OpenBraces))
                return new Factor(ParseSetSection());

            if (MatchIdentifier(TokenKind.Inherited, TokenKind.ShortString, TokenKind.String, TokenKind.WideString, TokenKind.UnicodeString, TokenKind.AnsiString, TokenKind.Dot, TokenKind.OpenBraces))
                return new Factor(ParseDesignator());

            if (Match(TokenKind.OpenParen) && IsDesignator())
                return new Factor(ParseDesignator());

            if (Match(TokenKind.OpenParen)) {
                var openParen = ContinueWith(TokenKind.OpenParen);
                var parenExpression = ParseConstantExpression(false, false);
                var closeParen = ContinueWithOrMissing(TokenKind.CloseParen);
                return new Factor(openParen, parenExpression, closeParen);
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
            var name = default(TypeName);
            var item = default(SyntaxPartBase);

            if (MatchIdentifier(TokenKind.String, TokenKind.ShortString, TokenKind.AnsiString, TokenKind.WideString, TokenKind.String) && LookAhead(1, TokenKind.Dot, TokenKind.AngleBracketsOpen)) {
                name = ParseTypeName(true);
            }

            using (var list = GetList<SyntaxPartBase>()) {
                do {
                    item = AddToList(list, ParseDesignatorItem());
                } while (item != default);

                return new DesignatorStatementSymbol(inherited, name, GetFixedArray(list));
            }
        }

        #endregion
        #region ParseDesignatorItem

        /// <summary>
        ///     parse a designator item
        /// </summary>
        /// <returns></returns>

        [Rule("DesignatorItem", "'^' | '.' Ident [GenericSuffix] | '[' ExpressionList ']' | '(' [ FormattedExpression  { ',' FormattedExpression } ] ')'")]
        public SyntaxPartBase ParseDesignatorItem() {

            if (Match(TokenKind.Circumflex))
                return new DesignatorItemSymbol(ContinueWithOrMissing(TokenKind.Circumflex));

            var subitem = default(Identifier);
            var dot = default(Terminal);
            var genericSuffix = default(GenericSuffix);
            var openBraces = default(Terminal);
            var closeBraces = default(Terminal);
            var indexExpression = default(ExpressionList);
            var openParen = default(Terminal);
            var closeParen = default(Terminal);

            if (MatchIdentifier(TokenKind.String, TokenKind.ShortString, TokenKind.AnsiString, TokenKind.UnicodeString)) {
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
                //var prevDesignatorItem = parent.PartList != null && parent.PartList.Count > 0 ? parent.PartList[parent.PartList.Count - 1] as DesignatorItem : null;
                var prevDesignatorItem = default(DesignatorItemSymbol);
                if (!IsDesignator() && ((prevDesignatorItem == null) || (prevDesignatorItem.Subitem == null))) {
                    ContinueWithOrMissing(TokenKind.OpenParen);
                    var children = ParseConstantExpression(true);
                    ContinueWithOrMissing(TokenKind.CloseParen);
                    return children;
                }

                openParen = ContinueWith(TokenKind.OpenParen);
                using (var list = GetList<Parameter>()) {
                    if (!Match(TokenKind.CloseParen)) {
                        do {
                            var parameter = new Parameter();
                            AddToList(list, parameter);

                            if (MatchIdentifier(true) && LookAhead(1, TokenKind.Assignment)) {
                                parameter.ParameterName = RequireIdentifier(true);
                                ContinueWithOrMissing(parameter, TokenKind.Assignment);
                            }

                            if (!Match(TokenKind.Comma))
                                parameter.Expression = ParseFormattedExpression(parameter);

                        } while (ContinueWith(TokenKind.Comma) != null);
                    }
                    closeParen = ContinueWithOrMissing(TokenKind.CloseParen);

                    return new DesignatorItemSymbol(dot, subitem, genericSuffix, openParen, GetFixedArray(list), closeParen);
                }
            }

            dot = ContinueWith(TokenKind.Dot);

            if (dot != null || subitem != null || genericSuffix != null)
                return new DesignatorItemSymbol(dot, subitem, genericSuffix, openParen, ImmutableArray<Parameter>.Empty, closeParen);
            else
                return default;
        }

        #endregion
        #region ParseFormattedExpression

        [Rule("FormattedExpression", "Expression [ ':' Expression [ ':' Expression ] ]")]
        private FormattedExpression ParseFormattedExpression(IExtendableSyntaxPart parent) {
            var result = new FormattedExpression();
            parent.Add(result);
            result.Expression = ParseExpression();

            if (ContinueWith(result, TokenKind.Colon)) {
                result.Width = ParseExpression();
                if (ContinueWith(result, TokenKind.Colon)) {
                    result.Decimals = ParseExpression();
                }
            }
            return result;
        }

        #endregion
        #region ParseSetSection

        [Rule("SetSection", "'[' [ Expression ] { (',' | '..') Expression } ']'")]
        private SetSection ParseSetSection() {
            var result = new SetSection();
            InitByTerminal(result, null, TokenKind.OpenBraces);
            SetSectnPart lastPart = null;

            if (!Match(TokenKind.CloseBraces)) {
                SetSectnPart part;
                do {

                    if (ContinueWith(result, TokenKind.Comma)) {
                        if (lastPart != null)
                            lastPart.Continuation = TokenKind.Comma;
                        else
                            Unexpected();
                    }
                    else if (ContinueWith(result, TokenKind.DotDot)) {
                        if (lastPart != null && lastPart.Continuation == TokenKind.Undefined)
                            lastPart.Continuation = TokenKind.DotDot;
                        else
                            Unexpected();
                    }
                    else {
                        if (lastPart != null)
                            Unexpected();
                    }

                    part = new SetSectnPart();
                    result.Add(part);
                    part.Continuation = TokenKind.Undefined;
                    part.SetExpression = ParseExpression();
                    lastPart = part;

                } while (Match(TokenKind.Comma, TokenKind.DotDot));
            }

            ContinueWithOrMissing(result, TokenKind.CloseBraces);
            return result;
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
            var returnType = default(TypeSpecification);

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

        private StandardInteger RequireInteger()
            => new StandardInteger(ContinueWithOrMissing(TokenKind.Integer));

        private HexNumber RequireHexValue()
            => new HexNumber(ContinueWithOrMissing(TokenKind.HexNumber));

        /// <summary>
        ///     parse a real number
        /// </summary>
        /// <returns></returns>
        public RealNumberSymbol RequireRealValue()
            => new RealNumberSymbol(ContinueWithOrMissing(TokenKind.Real));

        private Identifier RequireIdentifier(bool allowReserverdWords = false) {

            if (Match(TokenKind.Identifier)) {
                return new Identifier(ContinueWithOrMissing(TokenKind.Identifier));
            };

            var token = CurrentToken();

            if (allowReserverdWords || !reservedWords.Contains(token.Kind)) {
                return new Identifier(ContinueWithOrMissing(token.Kind));
            }

            ErrorMissingToken(TokenKind.Identifier);
            return null;
        }

        private QuotedString RequireString()
            => new QuotedString(ContinueWithOrMissing(TokenKind.QuotedString));

        private QuotedString RequireDoubleQuotedString()
            => new QuotedString(ContinueWithOrMissing(TokenKind.DoubleQuotedString));

        private bool CurrentTokenIsAfterNewline() =>
            /*
        foreach (Token invalidToken in CurrentToken().InvalidTokensBefore) {
        if (invalidToken.Kind == TokenKind.WhiteSpace && PatternContinuation.ContainsNewLineChar(invalidToken.Value))
        return true;
        }
        */
            false;

        private NamespaceName ParseNamespaceName(bool allowIn = false, bool inDesignator = false) {
            using (var list = GetList<SyntaxPartBase>()) {
                var name = default(SyntaxPartBase);
                var dot = default(Terminal);

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

                while (name != default && LookAheadIdentifier(1, Array.Empty<int>(), true) && (!inDesignator || LookAhead(2, new int[] { TokenKind.Dot })) && dot != default) {
                    name = RequireIdentifier(true);
                    AddToList(list, name);

                    dot = ContinueWith(TokenKind.Dot);
                    AddToList(list, dot);
                }

                return new NamespaceName(GetFixedArray(list));
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