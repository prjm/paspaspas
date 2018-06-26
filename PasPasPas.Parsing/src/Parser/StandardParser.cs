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

            result.DeclarationSections = ParseDeclarationSections(result);
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

            result.NamespaceName = ParseNamespaceName(result);
            if (ContinueWith(result, TokenKind.In))
                result.QuotedFileName = RequireString(result);

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
                    item = ParseInterfaceDeclarationItem(null);
                    if (item != null)
                        list.Item.Add(item);
                } while (item != null);
            }

            return result;
        }

        #endregion
        #region ParseInterfaceDeclarationItem

        [Rule("InterfaceDeclarationItem", "ConstSection | TypeSection | VarSection | ExportsSection | AssemblyAttribute | ExportedProcedureHeading")]
        private SyntaxPartBase ParseInterfaceDeclarationItem(IExtendableSyntaxPart parent) {

            if (Match(TokenKind.Const) || Match(TokenKind.Resourcestring)) {
                return ParseConstSection(parent, false);
            }

            if (Match(TokenKind.TypeKeyword)) {
                return ParseTypeSection(parent, false);
            }

            if (Match(TokenKind.Var)) {
                return ParseVarSection(parent, false);
            }

            if (Match(TokenKind.Exports)) {
                return ParseExportsSection(parent);
            }

            if (Match(TokenKind.OpenBraces) && LookAhead(1, TokenKind.Assembly)) {
                return ParseAssemblyAttribute();
            }

            if (Match(TokenKind.Procedure, TokenKind.Function)) {
                return ParseExportedProcedureHeading(parent);
            }

            return null;
        }

        #endregion
        #region ParseConstSection

        [Rule("ConstSection", "('const' | 'resourcestring') ConstDeclaration { ConstDeclaration }")]
        private ConstSection ParseConstSection(IExtendableSyntaxPart parent, bool inClassDeclaration) {
            var result = new ConstSection();
            InitByTerminal(result, parent, TokenKind.Const, TokenKind.Resourcestring);
            result.Kind = result.LastTerminalKind;
            while ((!inClassDeclaration || !(Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Automated, TokenKind.Strict))) && MatchIdentifier(TokenKind.OpenBraces)) {
                ParseConstDeclaration(result);
            }
            return result;
        }

        #endregion
        #region ParseExportedProcedureHeading

        [Rule("ExportedProcedureHeading", "")]
        private ExportedProcedureHeading ParseExportedProcedureHeading(IExtendableSyntaxPart parent) {
            var result = new ExportedProcedureHeading();
            InitByTerminal(result, parent, TokenKind.Function, TokenKind.Procedure);
            result.Kind = result.LastTerminalKind;
            result.Name = RequireIdentifier(result);

            if (Match(TokenKind.OpenParen)) {
                result.Parameters = ParseFormalParameterSection(result);
            }
            if (ContinueWith(result, TokenKind.Colon)) {
                result.ResultAttributes = ParseAttributes(result);
                result.ResultType = ParseTypeSpecification(result);
            }
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            result.Directives = ParseFunctionDirectives(result);
            return result;
        }

        #endregion
        #region ParseFunctionDirectives

        [Rule("FunctionDirectives", "{ FunctionDirective } ")]
        private FunctionDirectives ParseFunctionDirectives(IExtendableSyntaxPart parent) {
            var result = new FunctionDirectives();
            parent.Add(result);

            SyntaxPartBase directive;
            do {
                directive = ParseFunctionDirective(result);
            } while (directive != null);
            return result;
        }

        #endregion
        #region ParseFunctionDirective

        [Rule("FunctionDirective", "OverloadDirective | InlineDirective | CallConvention | OldCallConvention | Hint | ExternalDirective | UnsafeDirective")]
        private SyntaxPartBase ParseFunctionDirective(IExtendableSyntaxPart parent) {

            if (Match(TokenKind.Overload))
                return ParseOverloadDirective();

            if (Match(TokenKind.Inline, TokenKind.Assembler))
                return ParseInlineDirective();

            if (Match(TokenKind.Cdecl, TokenKind.Pascal, TokenKind.Register, TokenKind.Safecall, TokenKind.Stdcall, TokenKind.Export))
                return ParseCallConvention();

            if (Match(TokenKind.Far, TokenKind.Local, TokenKind.Near)) {
                return ParseOldCallConvention(parent);
            }

            if (Match(TokenKind.Deprecated, TokenKind.Library, TokenKind.Experimental, TokenKind.Platform)) {
                var result = ParseHint();
                result.Semicolon = ContinueWithOrMissing(TokenKind.Semicolon);
                return result;
            }

            if (Match(TokenKind.VarArgs, TokenKind.External)) {
                return ParseExternalDirective(parent);
            }

            if (Match(TokenKind.Unsafe)) {
                return ParseUnsafeDirective(parent);
            }

            if (Match(TokenKind.Forward)) {
                return ParseForwardDirective(parent);
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

            result.Statements = ParseStatementList(result);

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
            result.Statements = ParseStatementList(result);
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

            if (Match(TokenKind.Asm)) {
                return new CompoundStatementSymbol {
                    BeginSymbol = EmptyTerminal(),
                    AssemblerBlock = ParseAsmBlock(),
                    Statements = EmptyTerminal(),
                    EndSymbol = EmptyTerminal()
                };

            }
            else {
                var result = new CompoundStatementSymbol();
                result.BeginSymbol = ContinueWithOrMissing(TokenKind.Begin);
                result.AssemblerBlock = EmptyTerminal();

                if (!Match(TokenKind.End))
                    result.Statements = ParseStatementList(result);
                else
                    result.Statements = EmptyTerminal();


                result.EndSymbol = ContinueWithOrMissing(TokenKind.End);
                return result;
            }
        }

        #endregion
        #region StatementList

        [Rule("StatementList", "[Statement], { ';' [Statement]}")]
        private StatementList ParseStatementList(IExtendableSyntaxPart parent) {
            var result = new StatementList();
            parent.Add(result);

            do {
                ParseStatement();
            } while (ContinueWith(result, TokenKind.Semicolon));

            return result;
        }

        #endregion
        #region ParseStatement
        [Rule("Statement", "[ Label ':' ] StatementPart")]


        public Statement ParseStatement() {
            var result = new Statement {
                ColonSymbol = EmptyTerminal(),
            };

            Label label = null;
            if (MatchIdentifier(TokenKind.Integer, TokenKind.HexNumber) && LookAhead(1, TokenKind.Colon)) {
                label = ParseLabel(result);
                result.ColonSymbol = ContinueWithOrMissing(TokenKind.Colon);
            }

            var part = ParseStatementPart(result);

            if (label != null && part == null) {
                Unexpected();
                return null;
            }

            result.Label = (SyntaxPartBase)label ?? EmptyTerminal();
            result.Part = (SyntaxPartBase)part ?? EmptyTerminal();
            return result;
        }
        #endregion
        #region ParseStatementPart

        [Rule("StatementPart", "IfStatement | CaseStatement | ReapeatStatement | WhileStatment | ForStatement | WithStatement | TryStatement | RaiseStatement | AsmStatement | CompoundStatement | SimpleStatement ")]
        private StatementPart ParseStatementPart(IExtendableSyntaxPart parent) {
            var result = new StatementPart();
            parent.Add(result);

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

            return ParseSimpleStatement(parent);
        }

        #endregion
        #region ParseRaiseStatement

        [Rule("RaiseStatement", "'raise' [ Expression ] [ 'at' Expression ]")]
        private RaiseStatement ParseRaiseStatement(IExtendableSyntaxPart parent) {
            var result = new RaiseStatement();
            InitByTerminal(result, parent, TokenKind.Raise);

            if ((!Match(TokenKind.AtWord)) && MatchIdentifier(TokenKind.Inherited)) {
                result.Raise = ParseExpression(result);
            }

            if (ContinueWith(result, TokenKind.AtWord)) {
                result.At = ParseExpression(result);
            }

            return result;
        }

        #endregion
        #region ParseTryStatement

        [Rule("TryStatement", "'try' StatementList  ('except' HandlerList | 'finally' StatementList) 'end'")]
        private TryStatement ParseTryStatement(IExtendableSyntaxPart parent) {
            var result = new TryStatement();
            InitByTerminal(result, parent, TokenKind.Try);

            result.Try = ParseStatementList(result);

            if (ContinueWith(result, TokenKind.Except)) {
                result.Handlers = ParseExceptHandlers(result);
                ContinueWithOrMissing(result, TokenKind.End);
            }
            else if (ContinueWith(result, TokenKind.Finally)) {
                result.Finally = ParseStatementList(result);
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
                    ParseExceptHandler(result);
                }
                if (ContinueWith(result, TokenKind.Else)) {
                    result.ElseStatements = ParseStatementList(result);
                }
            }
            else {
                result.Statements = ParseStatementList(result);
            }

            return result;
        }

        #endregion
        #region ParseExceptHandler

        [Rule("ExceptHandler", "'on' Identifier ':' NamespaceName 'do' Statement ';'")]
        private ExceptHandler ParseExceptHandler(IExtendableSyntaxPart parent) {
            var result = new ExceptHandler();
            InitByTerminal(result, parent, TokenKind.On);
            result.Name = RequireIdentifier(result);
            ContinueWithOrMissing(result, TokenKind.Colon);
            result.HandlerType = ParseTypeName(result);
            ContinueWithOrMissing(result, TokenKind.Do);
            result.Statement = ParseStatement();
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseWithStatement

        [Rule("WithStatement", "'with' Expression { ',' Expression }  'do' Statement")]
        private WithStatement ParseWithStatement(IExtendableSyntaxPart parent) {
            var result = new WithStatement();
            InitByTerminal(result, parent, TokenKind.With);

            do {
                ParseExpression(result);
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

            result.Variable = RequireIdentifier(result);
            if (ContinueWith(result, TokenKind.Assignment)) {
                result.StartExpression = ParseExpression(result);
                ContinueWithOrMissing(result, TokenKind.To, TokenKind.DownTo);
                result.Kind = result.LastTerminalKind;
                result.EndExpression = ParseExpression(result);
            }
            else {
                ContinueWithOrMissing(result, TokenKind.In);
                result.Kind = result.LastTerminalKind;
                result.StartExpression = ParseExpression(result);
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
            result.Condition = ParseExpression(result);
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
                result.Statements = ParseStatementList(result);
            }
            ContinueWithOrMissing(result, TokenKind.Until);
            result.Condition = ParseExpression(result);
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
            var result = new CaseStatementSymbol(default) {
                CaseSymbol = ContinueWithOrMissing(TokenKind.Case),
                CaseExpression = ParseExpression(null),
                OfSymbol = ContinueWithOrMissing(TokenKind.Of)
            };

            var item = default(CaseItemSymbol);
            using (var list = GetList<CaseItemSymbol>()) {
                do {
                    item = ParseCaseItem();
                    if (item != default)
                        list.Item.Add(item);
                } while (Tokenizer.HasNextToken && item != default);
            }

            if (Match(TokenKind.Else)) {
                result.ElseSymbol = ContinueWithOrMissing(TokenKind.Else);
                result.Else = ParseStatementList(result);
                result.Semicolon = ContinueWith(TokenKind.Semicolon) ?? EmptyTerminal();
            }
            else {
                result.ElseSymbol = EmptyTerminal();
                result.Else = EmptyTerminal();
                result.Semicolon = EmptyTerminal();
            }

            result.EndSymbol = ContinueWithOrMissing(TokenKind.End);
            return result;
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
                return null;

            if (!HasTokenBeforeToken(TokenKind.Colon, TokenKind.Semicolon, TokenKind.End, TokenKind.Begin))
                return null;

            var result = new CaseItemSymbol(default);
            var label = default(CaseLabelSymbol);
            using (var list = GetList<CaseLabelSymbol>()) {
                do {
                    label = ParseCaseLabel();
                    list.Item.Add(label);
                    label.Comma = ContinueWith(TokenKind.Comma) ?? EmptyTerminal();
                } while (label.Comma.Kind == TokenKind.Comma);
            }

            result.ColonSymbol = ContinueWithOrMissing(TokenKind.Colon);
            result.CaseStatement = ParseStatement();
            result.Semicolon = ContinueWith(TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseCaseLabel

        /// <summary>
        ///     parse a case label
        /// </summary>
        /// <returns></returns>

        [Rule("CaseLabel", "Expression [ '..' Expression ]")]
        public CaseLabelSymbol ParseCaseLabel() {
            var result = new CaseLabelSymbol();
            result.StartExpression = ParseExpression(result);
            if (Match(TokenKind.DotDot)) {
                result.Dots = ContinueWithOrMissing(TokenKind.DotDot);
                result.EndExpression = ParseExpression(result);
            }
            else {
                result.Dots = EmptyTerminal();
                result.EndExpression = EmptyTerminal();
            }
            return result;
        }

        #endregion
        #region IfStatement

        [Rule("IfStatement", "'if' Expression 'then' Statement [ 'else' Statement ]")]
        private IfStatement ParseIfStatement(IExtendableSyntaxPart parent) {
            var result = new IfStatement();
            InitByTerminal(result, parent, TokenKind.If);

            result.Condition = ParseExpression(result);
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
        private StatementPart ParseSimpleStatement(IExtendableSyntaxPart parent) {
            if (!(LookAhead(1, TokenKind.Assignment, TokenKind.OpenBraces, TokenKind.OpenParen)) && Match(TokenKind.GoToKeyword, TokenKind.Exit, TokenKind.Break, TokenKind.Continue)) {
                var result = new StatementPart();
                parent.Add(result);
                result.GoTo = ParseGoToStatement(result);
                return result;
            }

            if (MatchIdentifier(TokenKind.Inherited, TokenKind.Circumflex, TokenKind.OpenParen, TokenKind.At, TokenKind.AnsiString, TokenKind.UnicodeString, TokenKind.String, TokenKind.WideString, TokenKind.ShortString)) {
                var result = new StatementPart();
                parent.Add(result);

                result.DesignatorPart = ParseDesignator(result);

                if (ContinueWith(result, TokenKind.Assignment)) {
                    result.Assignment = ParseExpression(result);
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
                result.GoToLabel = ParseLabel(result);
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
                    result.ExitExpression = ParseExpression(result);
                    ContinueWithOrMissing(result, TokenKind.CloseParen);
                }
                return result;
            }

            Unexpected();
            return null;
        }

        #endregion
        #region ParseUnitHead

        [Rule("UnitHead", "'unit' NamespaceName { Hint } ';' ")]
        private UnitHeadSymbol ParseUnitHead() {
            return new UnitHeadSymbol {
                Unit = ContinueWithOrMissing(TokenKind.Unit),
                UnitName = ParseNamespaceName(null),
                Hint = ParseHints(null, false),
                Semicolon = ContinueWithOrMissing(TokenKind.Semicolon)
            };
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
                ParseNamespaceName(result);
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
            result.PackageName = ParseNamespaceName(result);
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
                LibraryName = ParseNamespaceName(null),
                Hints = ParseHints(null, false),
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
                Name = ParseNamespaceName(null),
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
                    RequireIdentifier(result);

                    while (ContinueWith(result, TokenKind.Comma))
                        RequireIdentifier(result);
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
            var result = new BlockSymbol();

            result.DeclarationSections = ParseDeclarationSections(result);

            if (Match(TokenKind.Asm, TokenKind.Begin))
                result.Body = ParseBlockBody();
            else
                result.Body = EmptyTerminal();

            return result;
        }

        #endregion
        #region ParseBlockBody

        /// <summary>
        ///     parse a block body
        /// </summary>
        /// <returns></returns>

        [Rule("BlockBody", "AssemblerBlock | CompoundStatement")]
        public BlockBodySymbol ParseBlockBody() {
            return new BlockBodySymbol() {
                AssemblerBlock = Match(TokenKind.Asm) ? (SyntaxPartBase)ParseAsmBlock() : EmptyTerminal(),
                Body = Match(TokenKind.Begin) ? (SyntaxPartBase)ParseCompoundStatement() : EmptyTerminal(),
            };
        }

        #endregion
        #region ParseAsmBlock

        /// <summary>
        ///     parse an assembler block
        /// </summary>
        /// <returns></returns>

        [Rule("AsmBlock", "'asm' { AssemblyStatement | PseudoOp } 'end'")]
        public AsmBlockSymbol ParseAsmBlock() {
            var result = new AsmBlockSymbol(default) {
                AsmSymbol = ContinueWithOrMissing(TokenKind.Asm),
                EndSymbol = EmptyTerminal()
            };

            while (Tokenizer.HasNextToken) {

                if (Match(TokenKind.End)) {
                    result.EndSymbol = ContinueWithOrMissing(TokenKind.End);
                    break;
                }

                using (var list = GetList<SyntaxPartBase>()) {
                    if (Match(TokenKind.Dot))
                        list.Item.Add(ParseAsmPseudoOp());
                    else
                        list.Item.Add(ParseAsmStatement());
                }
            }

            return result;
        }

        #endregion
        #region ParseAsmPseudoOp

        /// <summary>
        ///     parse asm pseudo code
        /// </summary>
        /// <returns></returns>

        [Rule("PseudoOp", "( '.PARAMS ' Integer | '.PUSHNV' Register | '.SAVNENV' Register | '.NOFRAME'.")]
        public AsmPseudoOpSymbol ParseAsmPseudoOp() {
            var result = new AsmPseudoOpSymbol {
                DotSymbol = ContinueWithOrMissing(TokenKind.Dot)
            };

            var kind = CurrentToken().Value;
            result.Kind = RequireIdentifier(result);
            result.NumberOfParams = EmptyTerminal();
            result.Register = EmptyTerminal();

            if (string.Equals(kind, "params", StringComparison.OrdinalIgnoreCase)) {
                result.ParamsOperation = true;
                result.NumberOfParams = RequireInteger(null);
            }
            else if (string.Equals(kind, "pushenv", StringComparison.OrdinalIgnoreCase)) {
                result.PushEnvOperation = true;
                result.Register = RequireIdentifier(result);
            }
            else if (string.Equals(kind, "savenv", StringComparison.OrdinalIgnoreCase)) {
                result.SaveEnvOperation = true;
                result.Register = RequireIdentifier(result);
            }
            else if (string.Equals(kind, "noframe", StringComparison.OrdinalIgnoreCase)) {
                result.NoFrame = true;
            }
            else {
                Unexpected();
            }

            return result;
        }

        #endregion
        #region ParseAsmStatement

        /// <summary>
        ///     parse an assembler statement
        /// </summary>
        /// <returns></returns>

        [Rule("AssemblyStatement", "[AssemblyLabel ':'] [AssemblyPrefix] AssemblyOpcode [AssemblyOperand {','  AssemblyOperand}]")]
        public AsmStatementSymbolx ParseAsmStatement() {
            var result = new AsmStatementSymbolx(default);

            if (Match(TokenKind.At) || LookAhead(1, TokenKind.Colon)) {
                result.Label = ParseAssemblyLabel();
                result.ColonSymbol = ContinueWithOrMissing(TokenKind.Colon);
            }
            else {
                result.Label = EmptyTerminal();
                result.ColonSymbol = EmptyTerminal();
            }

            if (Match(TokenKind.End)) {
                result.Prefix = EmptyTerminal();
                result.OpCode = EmptyTerminal();
                return result;
            }

            result.Prefix = ParseAssemblyPrefix();
            result.OpCode = ParseAssemblyOpcode();

            using (var list = GetList<AsmOperandSymbol>()) {
                while (Tokenizer.HasNextToken && !Match(TokenKind.End) && !Match(TokenKind.Semicolon) && Tokenizer.HasNextToken && !CurrentTokenIsAfterNewline()) {
                    var operand = ParseAssemblyOperand();
                    list.Item.Add(operand);
                    operand.Comma = ContinueWith(TokenKind.Comma) ?? EmptyTerminal();
                }
            }

            return result;
        }

        #endregion
        #region ParseAssemblyOperand

        /// <summary>
        ///     asm operand
        /// </summary>
        /// <returns></returns>

        [Rule("AssemblyOperand", " AssemblyExpression ('and' | 'or' | 'xor') AssemblyOperand | ( 'not' AssemblyOperand ']' )")]
        public AsmOperandSymbol ParseAssemblyOperand() {
            var result = new AsmOperandSymbol() {
                Comma = EmptyTerminal()
            };

            if (Match(TokenKind.Not)) {
                result.NotSymbol = ContinueWithOrMissing(TokenKind.Not);
                result.NotExpression = ParseAssemblyOperand();
                result.LeftTerm = EmptyTerminal();
                result.Operand = EmptyTerminal();
                result.RightTerm = EmptyTerminal();
                return result;
            }

            result.NotSymbol = EmptyTerminal();
            result.NotExpression = EmptyTerminal();
            result.LeftTerm = ParseAssemblyExpression();

            if (Match(TokenKind.And, TokenKind.Or, TokenKind.Xor)) {
                result.Operand = ContinueWithOrMissing(TokenKind.And, TokenKind.Or, TokenKind.Xor);
                result.RightTerm = ParseAssemblyOperand();
            }
            else {
                result.Operand = EmptyTerminal();
                result.RightTerm = EmptyTerminal();
            }

            return result;
        }

        #endregion
        #region ParseAssemblyExpression

        /// <summary>
        ///     parse an assembly expression
        /// </summary>
        /// <returns></returns>

        [Rule("AssemblyExpression", " ('OFFSET' AssemblyOperand ) | ('TYPE' AssemblyOperand) | (('BYTE' | 'WORD' | 'DWORD' | 'QWORD' | 'TBYTE' ) PTR AssemblyOperand) | AssemblyTerm ('+' | '-' ) AssemblyOperand ")]
        public AsmExpressionSymbol ParseAssemblyExpression() {
            var result = new AsmExpressionSymbol() {
                Offset = EmptyTerminal(),
                OffsetSymbol = EmptyTerminal(),
                BytePtr = EmptyTerminal(),
                BytePtrKind = EmptyTerminal(),
                TypeSymbol = EmptyTerminal(),
                TypeExpression = EmptyTerminal(),
                LeftOperand = EmptyTerminal(),
                RightOperand = EmptyTerminal()
            };

            var tokenValue = CurrentToken().Value;
            if (MatchIdentifier()) {

                if (string.Equals(tokenValue, "OFFSET", StringComparison.OrdinalIgnoreCase)) {
                    result.OffsetSymbol = ContinueWith(TokenKind.Identifier);
                    result.Offset = ParseAssemblyOperand();
                    return result;
                }

                if (asmPtr.Contains(tokenValue)) {
                    result.BytePtrKind = RequireIdentifier(result);
                    result.BytePtr = ParseAssemblyOperand();
                    return result;
                }
            }

            if (Match(TokenKind.TypeKeyword)) {
                result.TypeSymbol = ContinueWithOrMissing(TokenKind.TypeKeyword);
                result.TypeExpression = ParseAssemblyOperand();
                return result;
            }

            result.LeftOperand = ParseAssemblyTerm();

            if (Match(TokenKind.Plus, TokenKind.Minus)) {
                result.BinaryOperatorKind = CurrentToken().Kind;
                FetchNextToken();
                result.RightOperand = ParseAssemblyOperand();
            }

            return result;
        }

        #endregion
        #region ParseAssemblyTerm

        /// <summary>
        ///     parse an assembly term symbol
        /// </summary>
        /// <returns></returns>

        [Rule("AssemblyTerm", "AssemblyFactor [( '*' | '/' | 'mod' | 'shl' | 'shr' | '.' ) AssemblyOperand ]")]
        public AsmTermSymbol ParseAssemblyTerm() {
            var result = new AsmTermSymbol {
                LeftOperand = ParseAssemblyFactor(),
                DotSymbol = ContinueWith(TokenKind.Dot)
            };

            if (result.DotSymbol != null) {
                result.Subtype = ParseAssemblyOperand();
            }
            else {
                result.DotSymbol = EmptyTerminal();
                result.Subtype = EmptyTerminal();
            }

            if (Match(TokenKind.Times, TokenKind.Slash, TokenKind.Mod, TokenKind.Shl, TokenKind.Shr)) {
                result.Operator = ContinueWithOrMissing(TokenKind.Times, TokenKind.Slash, TokenKind.Mod, TokenKind.Shl, TokenKind.Shr);
                result.RightOperand = ParseAssemblyOperand();
            }
            else {
                result.Operator = EmptyTerminal();
                result.RightOperand = EmptyTerminal();
            }

            return result;
        }

        #endregion
        #region ParseAssemblyFactor

        /// <summary>
        ///     asm factor
        /// </summary>
        /// <returns></returns>

        [Rule("AssemblyFactor", "(SegmentPrefix ':' AssemblyOperand) | '(' AssemblyOperand ')' | '[' AssemblyOperand ']' | Identifier | QuotedString | DoubleQuotedString | Integer | HexNumber ")]
        public AsmFactorSymbol ParseAssemblyFactor() {
            var result = new AsmFactorSymbol() {
                SegmentPrefix = EmptyTerminal(),
                ColonSymbol = EmptyTerminal(),
                SegmentExpression = EmptyTerminal(),
                OpenParen = EmptyTerminal(),
                Subexpression = EmptyTerminal(),
                CloseParen = EmptyTerminal(),
                OpenBraces = EmptyTerminal(),
                MemorySubexpression = EmptyTerminal(),
                CloseBraces = EmptyTerminal(),
                Identifier = EmptyTerminal(),
                Number = EmptyTerminal(),
                RealNumber = EmptyTerminal(),
                HexNumber = EmptyTerminal(),
                QuotedString = EmptyTerminal(),
                Label = EmptyTerminal(),
            };

            if (MatchIdentifier() && segmentPrefixes.Contains(CurrentToken().Value) && LookAhead(1, TokenKind.Colon)) {
                result.SegmentPrefix = RequireIdentifier(result);
                result.ColonSymbol = ContinueWithOrMissing(TokenKind.Colon);
                result.SegmentExpression = ParseAssemblyOperand();
                return result;
            }

            if (Match(TokenKind.OpenParen)) {
                result.OpenParen = ContinueWithOrMissing(TokenKind.OpenParen);
                result.Subexpression = ParseAssemblyOperand();
                result.CloseParen = ContinueWithOrMissing(TokenKind.CloseParen);
                return result;
            }

            if (Match(TokenKind.OpenBraces)) {
                result.OpenBraces = ContinueWithOrMissing(TokenKind.OpenBraces);
                result.MemorySubexpression = ParseAssemblyOperand();
                result.CloseBraces = ContinueWithOrMissing(TokenKind.CloseBraces);
                return result;
            }

            if (MatchIdentifier(true)) {
                result.Identifier = RequireIdentifier(result, true);
            }
            else if (Match(TokenKind.Integer)) {
                result.Number = RequireInteger(result);
            }
            else if (Match(TokenKind.Real)) {
                result.RealNumber = RequireRealValue(result);
            }
            else if (Match(TokenKind.HexNumber)) {
                result.HexNumber = RequireHexValue(result);
            }
            else if (Match(TokenKind.QuotedString)) {
                result.QuotedString = RequireString(result);
            }
            else if (Match(TokenKind.DoubleQuotedString)) {
                result.QuotedString = RequireDoubleQuotedString(result);
            }
            else if (Match(TokenKind.At)) {
                result.Label = ParseLocalAsmLabel(result);
            }
            else {
                Unexpected();
            }
            return result;
        }

        #endregion
        #region ParseAssemblyOpcode

        /// <summary>
        ///     parse an opcode
        /// </summary>
        /// <returns></returns>

        [Rule("AssemblyOpCode", "Identifier [AssemblyDirective] ")]
        public SyntaxPartBase ParseAssemblyOpcode() {
            if (Match(TokenKind.End))
                return EmptyTerminal();

            return new AsmOpCodeSymbol() {
                OpCode = RequireIdentifier(null, true)
            };
        }

        #endregion
        #region ParseAssemblyPrefix

        /// <summary>
        ///     asm prefix
        /// </summary>
        /// <returns></returns>

        [Rule("AssemblyPrefix", "(LockPrefix | [SegmentPrefix]) | (SegmentPrefix [LockPrefix])")]
        public SyntaxPartBase ParseAssemblyPrefix() {

            if (!MatchIdentifier())
                return EmptyTerminal();

            if (lockPrefixes.Contains(CurrentToken().Value)) {
                var result = new AsmPrefixSymbol();

                result.LockPrefix = RequireIdentifier(result);

                if (MatchIdentifier() && segmentPrefixes.Contains(CurrentToken().Value))
                    result.SegmentPrefix = RequireIdentifier(result);
                else
                    result.SegmentPrefix = EmptyTerminal();

                return result;
            }

            if (segmentPrefixes.Contains(CurrentToken().Value)) {
                var result = new AsmPrefixSymbol();

                result.SegmentPrefix = RequireIdentifier(result);

                if (MatchIdentifier() && lockPrefixes.Contains(CurrentToken().Value))
                    result.LockPrefix = RequireIdentifier(result);
                else
                    result.SegmentPrefix = EmptyTerminal();

                return result;
            }

            return EmptyTerminal();
        }

        #endregion
        #region ParseAssemblyLabel

        /// <summary>
        ///     parse an asm label
        /// </summary>
        /// <returns></returns>

        [Rule("AsmLabel", "(Label | LocalAsmLabel { LocalAsmLabel } )")]
        public AsmLabelSymbol ParseAssemblyLabel() {
            var result = new AsmLabelSymbol();

            if (Match(TokenKind.At)) {
                result.LocalLabel = ParseLocalAsmLabel(result);
                result.Label = EmptyTerminal();
            }
            else {
                result.Label = ParseLabel(result);
                result.LocalLabel = EmptyTerminal();
            }

            return result;
        }

        #endregion
        #region ParseLocalAsmLabel

        [Rule("LocalAsmLabel", "'@' { '@' | Integer | Identifier | HexNumber }")]
        private LocalAsmLabel ParseLocalAsmLabel(IExtendableSyntaxPart parent) {
            var result = new LocalAsmLabel();
            parent.Add(result);

            ContinueWithOrMissing(result, TokenKind.At);

            do {
                if (Match(TokenKind.Integer)) {
                    RequireInteger(result);
                }
                else if (MatchIdentifier(true)) {
                    RequireIdentifier(result);
                }
                else if (Match(TokenKind.HexNumber)) {
                    RequireHexValue(result);
                }
                else if (Match(TokenKind.At)) {
                    //..
                }
                else {
                    Unexpected();
                }
            }
            while ((!CurrentTokenIsAfterNewline()) && ContinueWith(result, TokenKind.At));
            return result;
        }

        #endregion
        #region ParseDeclarationSections

        [Rule("DeclarationSection", "{ LabelDeclarationSection | ConstSection | TypeSection | VarSection | ExportsSection | AssemblyAttribute | MethodDecl | ProcedureDeclaration }", true)]
        private Declarations ParseDeclarationSections(IExtendableSyntaxPart parent) {
            var result = new Declarations();
            parent.Add(result);

            var stop = false;

            while (!stop) {

                if (Match(TokenKind.Label)) {
                    ParseLabelDeclarationSection(result);
                    continue;
                }

                if (Match(TokenKind.Const, TokenKind.Resourcestring)) {
                    ParseConstSection(result, false);
                    continue;
                }

                if (Match(TokenKind.TypeKeyword)) {
                    ParseTypeSection(result, false);
                    continue;
                }

                if (Match(TokenKind.Var, TokenKind.ThreadVar)) {
                    ParseVarSection(result, false);
                    continue;
                }

                if (Match(TokenKind.Exports)) {
                    ParseExportsSection(parent);
                    continue;
                }

                SyntaxPartBase attrs = null;
                if (Match(TokenKind.OpenBraces)) {
                    if (LookAhead(1, TokenKind.Assembly)) {
                        ParseAssemblyAttribute();
                        continue;
                    }
                    else {
                        attrs = ParseAttributes(result);
                    }
                }
                var useClass = ContinueWith(result, TokenKind.Class);

                if (Match(TokenKind.Function, TokenKind.Procedure, TokenKind.Constructor, TokenKind.Destructor, TokenKind.Operator)) {

                    var useMethodDeclaration = //
                        useClass ||
                        Match(TokenKind.Constructor, TokenKind.Destructor, TokenKind.Operator) ||
                        (LookAhead(1, TokenKind.Identifier) && (LookAhead(2, TokenKind.Dot, TokenKind.AngleBracketsOpen)) ||
                        HasTokenBeforeToken(TokenKind.Dot, TokenKind.OpenParen, TokenKind.Colon, TokenKind.Semicolon, TokenKind.Begin, TokenKind.End, TokenKind.Comma));

                    if (useMethodDeclaration) {
                        var methodDecl = ParseMethodDecl(result);
                        methodDecl.Class = useClass;
                        methodDecl.Attributes = attrs as UserAttributes;
                        continue;
                    }

                    ParseProcedureDeclaration(result, attrs as UserAttributes);
                    continue;
                }


                stop = true;
            }

            return result;
        }

        #endregion
        #region ParseMethodDecl

        [Rule("MethodDecl", "MethodDeclHeading ';' MethodDirectives [ Block ';' ]")]
        private MethodDeclaration ParseMethodDecl(IExtendableSyntaxPart parent) {
            var result = new MethodDeclaration();
            parent.Add(result);
            result.Heading = ParseMethodDeclHeading(result);
            ContinueWithOrMissing(result, TokenKind.Semicolon);

            result.Directives = ParseMethodDirectives(result);
            result.MethodBody = ParseBlock();

            if ((result.MethodBody != null) && (result.MethodBody.Body != null))
                ContinueWithOrMissing(result, TokenKind.Semicolon);

            return result;
        }

        #endregion
        #region ParseMethodDirectives

        [Rule("MethodDirectives", "{ MethodDirective }")]
        private MethodDirectives ParseMethodDirectives(IExtendableSyntaxPart parent) {
            var result = new MethodDirectives();
            parent.Add(result);

            SyntaxPartBase directive;
            do {
                directive = ParseMethodDirective(result);
            } while (directive != null);

            return result;
        }

        #endregion
        #region ParseMethodDirective

        [Rule("MethodDirective", "ReintroduceDirective | OverloadDirective | InlineDirective | BindingDirective | AbstractDirective | InlineDirective | CallConvention | HintingDirective | DispIdDirective")]
        private SyntaxPartBase ParseMethodDirective(IExtendableSyntaxPart parent) {

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
        /// <param name="parent"></param>
        /// <returns></returns>

        [Rule("InlineDirective", "('inline' | 'assembler' ) ';'")]
        public InlineSymbol ParseInlineDirective() {
            return new InlineSymbol() {
                Directive = ContinueWithOrMissing(TokenKind.Inline, TokenKind.Assembler),
                Semicolon = ContinueWithOrMissing(TokenKind.Semicolon)
            };
        }

        #endregion
        #region ParseCallConvention

        /// <summary>
        ///     parse a calling convention
        /// </summary>
        /// <returns></returns>

        [Rule("CallConvention", "('cdecl' | 'pascal' | 'register' | 'safecall' | 'stdcall' | 'export') ';' ")]
        public CallConventionSymbol ParseCallConvention() {
            return new CallConventionSymbol() {
                Directive = ContinueWithOrMissing(TokenKind.Cdecl, TokenKind.Pascal, TokenKind.Register, TokenKind.Safecall, TokenKind.Stdcall, TokenKind.Export),
                Semicolon = ContinueWithOrMissing(TokenKind.Semicolon)
            };
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
            return new BindingSymbol() {
                Directive = directive,

                MessageExpression = directive.Kind == TokenKind.Message ?
                    (SyntaxPartBase)ParseExpression(null) :
                    EmptyTerminal(),

                Semicolon = ContinueWithOrMissing(TokenKind.Semicolon),
            };
        }

        #endregion
        #region ParseOverloadDirective

        /// <summary>
        ///     parse an overload directive
        /// </summary>
        /// <returns></returns>

        [Rule("OverloadDirective", "'overload' ';' ")]
        public OverloadSymbol ParseOverloadDirective() {
            return new OverloadSymbol() {
                Directive = ContinueWithOrMissing(TokenKind.Overload),
                Semicolon = ContinueWithOrMissing(TokenKind.Semicolon)
            };
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
        private MethodDeclarationHeading ParseMethodDeclHeading(IExtendableSyntaxPart parent) {
            var result = new MethodDeclarationHeading();
            InitByTerminal(result, parent, TokenKind.Constructor, TokenKind.Destructor, TokenKind.Function, TokenKind.Procedure, TokenKind.Operator);
            result.Kind = result.LastTerminalKind;
            var allowIn = result.Kind == TokenKind.Operator;

            do {
                var name = new MethodDeclarationName();
                result.Add(name);
                name.Name = ParseNamespaceName(name, allowIn);

                if (Match(TokenKind.AngleBracketsOpen)) {
                    name.GenericDefinition = ParseGenericDefinition(name);
                }

                result.Qualifiers.Add(name);

            } while (ContinueWith(result, TokenKind.Dot));


            if (Match(TokenKind.OpenParen)) {
                result.Parameters = ParseFormalParameterSection(result);
            }
            if (ContinueWith(result, TokenKind.Colon)) {
                result.ResultTypeAttributes = ParseAttributes(result);
                result.ResultType = ParseTypeSpecification(result);
            }
            return result;
        }

        #endregion
        #region ParseProcedureDeclaration

        [Rule("ProcedureDeclaration", "ProcedureDeclarationHeading ';' FunctionDirectives [ ProcBody ]")]
        private ProcedureDeclaration ParseProcedureDeclaration(IExtendableSyntaxPart parent, UserAttributes attributes) {
            var result = new ProcedureDeclaration();
            parent.Add(result);
            result.Attributes = attributes;
            result.Heading = ParseProcedureDeclarationHeading(result);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            result.Directives = ParseFunctionDirectives(result);
            result.ProcedureBody = ParseBlock();
            if ((result.ProcedureBody != null) && (result.ProcedureBody.Body != null))
                ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseProcedureDeclarationHeading

        [Rule("ProcedureDeclarationHeading", "('procedure' | 'function') Identifier [FormalParameterSection][':' TypeSpecification]")]
        private ProcedureDeclarationHeading ParseProcedureDeclarationHeading(IExtendableSyntaxPart parent) {
            var result = new ProcedureDeclarationHeading();
            InitByTerminal(result, parent, TokenKind.Function, TokenKind.Procedure);
            result.Kind = result.LastTerminalKind;
            result.Name = RequireIdentifier(result);

            if (Match(TokenKind.OpenParen)) {
                result.Parameters = ParseFormalParameterSection(result);
            }

            if (ContinueWith(result, TokenKind.Colon)) {
                result.ResultTypeAttributes = ParseAttributes(result);
                result.ResultType = ParseTypeSpecification(result);
            }
            return result;
        }

        #endregion
        #region ParseAssemblyAttribute

        /// <summary>
        ///     parse assembly attribute
        /// </summary>
        /// <returns></returns>

        [Rule("AssemblyAttribute", "'[' 'assembly' ':' ']'")]
        public AssemblyAttributeDeclaration ParseAssemblyAttribute() {
            return new AssemblyAttributeDeclaration() {
                OpenBraces = ContinueWithOrMissing(TokenKind.OpenBraces),
                AssemblySymbol = ContinueWithOrMissing(TokenKind.Assembly),
                ColonSymbol = ContinueWithOrMissing(TokenKind.Colon),
                Attribute = ParseAttribute(null),
                CloseBraces = ContinueWithOrMissing(TokenKind.CloseBraces)
            };
        }

        #endregion
        #region ParseExportsSection

        [Rule("ExportsSection", "'exports' ExportItem { ',' ExportItem } ';' ")]
        private ExportsSection ParseExportsSection(IExtendableSyntaxPart parent) {
            var result = new ExportsSection();
            InitByTerminal(result, parent, TokenKind.Exports);

            do {
                ParseExportItem(result);
            } while (ContinueWith(result, TokenKind.Comma));

            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseExportItem

        [Rule("ExportItem", " Identifier [ '(' FormalParameters ')' ] [ 'index' Expression ] [ 'name' Expression ]")]
        private ExportItem ParseExportItem(IExtendableSyntaxPart parent) {
            var result = new ExportItem();
            parent.Add(result);

            result.ExportName = RequireIdentifier(result);

            if (ContinueWith(result, TokenKind.OpenParen)) {
                result.Parameters = ParseFormalParameters(result);
                ContinueWithOrMissing(result, TokenKind.CloseParen);
            }

            if (ContinueWith(result, TokenKind.Index)) {
                result.IndexParameter = ParseExpression(result);
            }

            if (ContinueWith(result, TokenKind.Name)) {
                result.NameParameter = ParseExpression(result);
            }

            result.Resident = ContinueWith(result, TokenKind.Resident);
            return result;
        }

        #endregion
        #region ParseVarSection

        [Rule("VarSection", "(var | threadvar) VarDeclaration { VarDeclaration }")]
        private VarSection ParseVarSection(IExtendableSyntaxPart parent, bool inClassDeclaration) {
            var result = new VarSection();
            InitByTerminal(result, parent, TokenKind.Var, TokenKind.ThreadVar);
            result.Kind = result.LastTerminalKind;

            do {
                ParseVarDeclaration(result);
            } while ((!inClassDeclaration || !Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Automated, TokenKind.Strict)) && MatchIdentifier(TokenKind.OpenBraces));

            return result;
        }

        #endregion
        #region ParseVarDeclaration

        [Rule("VarDeclaration", " IdentList ':' TypeSpecification [ VarValueSpecification ] Hints ';' ")]
        private VarDeclaration ParseVarDeclaration(IExtendableSyntaxPart parent) {
            var result = new VarDeclaration();
            parent.Add(result);

            result.Attributes = ParseAttributes(result);
            result.Identifiers = ParseIdentList(result, false);
            ContinueWithOrMissing(result, TokenKind.Colon);
            result.TypeDeclaration = ParseTypeSpecification(result, false, true);

            if (Match(TokenKind.Absolute, TokenKind.EqualsSign)) {
                result.ValueSpecification = ParseValueSpecification(result);
            }

            result.Hints = ParseHints(result, false);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseValueSpecification

        [Rule("VarValueSpecification", "('absolute' ConstExpression) | ('=' ConstExpression)")]
        private VarValueSpecification ParseValueSpecification(IExtendableSyntaxPart parent) {
            var result = new VarValueSpecification();
            parent.Add(result);

            if (ContinueWith(result, TokenKind.Absolute)) {
                result.Absolute = ParseConstantExpression();
                return result;
            }

            ContinueWithOrMissing(result, TokenKind.EqualsSign);
            result.InitialValue = ParseConstantExpression();
            return result;
        }

        #endregion
        #region ParseLabelDeclarationSection

        [Rule("LabelSection", "'label' Label { ',' Label } ';' ")]
        private LabelDeclarationSection ParseLabelDeclarationSection(IExtendableSyntaxPart parent) {
            var result = new LabelDeclarationSection();
            InitByTerminal(result, parent, TokenKind.Label);

            do {
                ParseLabel(result);
            } while (ContinueWith(result, TokenKind.Comma));

            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseLabel

        [Rule("Label", "Identifier | Integer")]
        private Label ParseLabel(IExtendableSyntaxPart parent) {
            var result = new Label();
            parent.Add(result);

            if (MatchIdentifier()) {
                result.LabelName = RequireIdentifier(result);
            }
            else if (Match(TokenKind.Integer)) {
                result.LabelName = RequireInteger(result);
            }
            else if (Match(TokenKind.HexNumber)) {
                result.LabelName = RequireHexValue(result);
            }
            else {
                Unexpected();
            }

            return result;
        }

        #endregion
        #region ParseConstDeclaration

        [Rule("ConstDeclaration", "[Attributes] Identifier [ ':' TypeSpecification ] = ConstantExpression Hints';'")]
        private ConstDeclaration ParseConstDeclaration(IExtendableSyntaxPart parent) {
            var result = new ConstDeclaration();
            parent.Add(result);
            result.Attributes = ParseAttributes(result);
            result.Identifier = RequireIdentifier(result);

            if (ContinueWith(result, TokenKind.Colon)) {
                result.TypeSpecification = ParseTypeSpecification(result, true);
            }

            ContinueWithOrMissing(result, TokenKind.EqualsSign);
            result.Value = ParseConstantExpression();
            result.Hint = ParseHints(result, false);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseHints

        /// <summary>
        ///     parse hints
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="requireSemicolon"></param>
        /// <returns></returns>

        [Rule("Hints", " { Hint ';' }")]
        public ISyntaxPart ParseHints(IExtendableSyntaxPart parent, bool requireSemicolon) {
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
                        if (result == null)
                            result = new HintingInformationList(default);
                        list.Item.Add(hint);
                    }

                } while (hint != default);
            }

            return (ISyntaxPart)result ?? EmptyTerminal();
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
                    result.DeprecatedComment = RequireString(result);
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
        private TypeSpecification ParseTypeSpecification(IExtendableSyntaxPart parent, bool constDeclaration = false, bool varDeclaration = false) {
            var result = new TypeSpecification();

            if (parent != null)
                parent.Add(result);

            if (Match(TokenKind.Packed, TokenKind.Array, TokenKind.Set, TokenKind.File, //
                TokenKind.Class, TokenKind.Interface, TokenKind.Record, TokenKind.Object, TokenKind.DispInterface)) {
                result.StructuredType = ParseStructType(result);
                return result;
            }

            if (Match(TokenKind.Pointer, TokenKind.Circumflex)) {
                result.PointerType = ParsePointerType(result);
                return result;
            }

            if (Match(TokenKind.String, TokenKind.ShortString, TokenKind.AnsiString, TokenKind.UnicodeString, TokenKind.WideString)) {
                result.StringType = ParseStringType(result);
                return result;
            }

            if (Match(TokenKind.Function, TokenKind.Procedure)) {
                result.ProcedureType = ParseProcedureType(result);
                return result;
            }

            if (Match(TokenKind.Reference) && LookAhead(1, TokenKind.To)) {
                result.ProcedureType = ParseProcedureType(result);
                return result;
            }

            result.SimpleType = ParseSimpleType(result, constDeclaration, varDeclaration);
            return result;
        }

        #endregion
        #region ParseSimpleType

        [Rule("SimpleType", "EnumType | (ConstExpression [ '..' ConstExpression ]) | ([ 'type' ] GenericNamespaceName {'.' GenericNamespaceName })")]
        private SimpleType ParseSimpleType(IExtendableSyntaxPart parent, bool constDeclaration = false, bool varDeclaration = false) {
            var result = new SimpleType();
            parent.Add(result);

            if (Match(TokenKind.OpenParen)) {
                result.EnumType = ParseEnumType(result);
                return result;
            }

            if (!varDeclaration) {
                result.NewType = ContinueWith(result, TokenKind.TypeKeyword);

                if (result.NewType)
                    result.TypeOf = ContinueWith(result, TokenKind.Of);
            }
            else {
                if (Match(TokenKind.TypeKeyword)) {
                    Unexpected();
                }
                result.NewType = false;
            }

            if (result.NewType || (MatchIdentifier(TokenKind.ShortString, TokenKind.String, TokenKind.WideString, TokenKind.UnicodeString, TokenKind.AnsiString) && (!LookAhead(1, TokenKind.DotDot)))) {
                do {
                    ParseGenericNamespaceName(result);
                } while (ContinueWith(result, TokenKind.Dot));
                return result;
            }

            result.SubrangeStart = ParseConstantExpression(false, constDeclaration);
            if (ContinueWith(result, TokenKind.DotDot)) {
                result.SubrangeEnd = ParseConstantExpression(false, constDeclaration);
            }

            return result;
        }

        #endregion
        #region GenericNamespaceName

        [Rule("GenericNamespaceName", "NamespaceName [ GenericSuffix ]")]
        private GenericNamespaceName ParseGenericNamespaceName(IExtendableSyntaxPart parent, bool advancedCheck = false, bool inDesignator = false) {
            var result = new GenericNamespaceName();
            parent.Add(result);
            result.Name = ParseNamespaceName(result, false, inDesignator);
            if (Match(TokenKind.AngleBracketsOpen)) {
                if (!advancedCheck) {
                    result.GenericPart = ParseGenericSuffix(result);
                }
                else if (LookAheadIdentifier(1, new[] { TokenKind.String, TokenKind.ShortString, TokenKind.WideString, TokenKind.UnicodeString, TokenKind.AnsiString, TokenKind.Pointer }, false)) {
                    var whereCloseBrackets = HasTokenUntilToken(new[] { TokenKind.AngleBracketsClose }, TokenKind.Identifier, TokenKind.Dot, TokenKind.Comma, TokenKind.AngleBracketsOpen, TokenKind.String, TokenKind.ShortString, TokenKind.WideString, TokenKind.UnicodeString, TokenKind.AnsiString, TokenKind.Pointer);
                    if (whereCloseBrackets.Item1 && (!LookAheadIdentifier(1 + whereCloseBrackets.Item2, new[] { TokenKind.HexNumber, TokenKind.Integer, TokenKind.Real }, false) || LookAhead(1 + whereCloseBrackets.Item2, TokenKind.Read, TokenKind.Write, TokenKind.ReadOnly, TokenKind.WriteOnly, TokenKind.Add, TokenKind.Remove, TokenKind.DispId))) {
                        result.GenericPart = ParseGenericSuffix(result);
                    }
                }
            }
            return result;
        }

        #endregion
        #region EnumType

        [Rule("EnumType", "'(' EnumTypeValue { ',' EnumTypeValue } ')'")]
        private EnumTypeDefinition ParseEnumType(IExtendableSyntaxPart parent) {
            var result = new EnumTypeDefinition();
            InitByTerminal(result, parent, TokenKind.OpenParen);

            do {
                ParseEnumTypeValue(result);
            } while (ContinueWith(result, TokenKind.Comma));

            ContinueWithOrMissing(result, TokenKind.CloseParen);
            return result;
        }

        #endregion
        #region ParseEnumTypeValue

        [Rule("EnumTypeValue", "Identifier [ '=' Expression ]")]
        private EnumValue ParseEnumTypeValue(IExtendableSyntaxPart parent) {
            var result = new EnumValue();
            parent.Add(result);

            result.EnumName = RequireIdentifier(result);
            if (ContinueWith(result, TokenKind.EqualsSign)) {
                result.Value = ParseExpression(result);
            }
            return result;
        }

        #endregion
        #region ParseProcedureType

        [Rule("ProcedureType", "(ProcedureRefType [ 'of' 'object' ] ( | ProcedureReference")]
        private ProcedureType ParseProcedureType(IExtendableSyntaxPart parent) {
            var result = new ProcedureType();
            parent.Add(result);

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
                result.Parameters = ParseFormalParameterSection(result);
            }

            if (result.Kind == TokenKind.Function) {
                ContinueWithOrMissing(result, TokenKind.Colon);
                result.ReturnTypeAttributes = ParseAttributes(result);
                result.ReturnType = ParseTypeSpecification(result);
            }

            return result;
        }

        #endregion
        #region FormalParameterSecion

        [Rule("FormalParameterSection", "'(' [ FormalParameters ] ')'")]
        private FormalParameterSection ParseFormalParameterSection(IExtendableSyntaxPart parent) {
            var result = new FormalParameterSection();
            InitByTerminal(result, parent, TokenKind.OpenParen);

            if (!Match(TokenKind.CloseParen)) {
                result.ParameterList = ParseFormalParameters(result);
            }

            ContinueWithOrMissing(result, TokenKind.CloseParen);
            return result;
        }

        #endregion
        #region ParseStringType

        [Rule("StringType", "ShortString | WideString | UnicodeString |('string' [ '[' Expression ']'  ]) | ('AnsiString' '(' ConstExpression ')') ")]
        private StringType ParseStringType(IExtendableSyntaxPart parent) {
            var result = new StringType();
            parent.Add(result);

            if (ContinueWith(result, TokenKind.String)) {
                result.Kind = TokenKind.String;
                if (ContinueWith(result, TokenKind.OpenBraces)) {
                    result.Kind = TokenKind.ShortString;
                    result.StringLength = ParseExpression(result);
                    ContinueWithOrMissing(result, TokenKind.CloseBraces);
                };
                return result;
            }

            if (ContinueWith(result, TokenKind.AnsiString)) {
                result.Kind = TokenKind.AnsiString;
                if (ContinueWith(result, TokenKind.OpenParen)) {
                    result.CodePage = ParseConstantExpression();
                    ContinueWithOrMissing(result, TokenKind.CloseParen);
                }
                return result;
            }

            if (ContinueWith(result, TokenKind.ShortString)) {
                result.Kind = TokenKind.ShortString;
                return result;
            }

            if (ContinueWith(result, TokenKind.WideString)) {
                result.Kind = TokenKind.WideString;
                return result;
            }

            if (ContinueWith(result, TokenKind.UnicodeString)) {
                result.Kind = TokenKind.UnicodeString;
                return result;
            }

            Unexpected();
            return result;
        }

        #endregion
        #region ParseStructType

        [Rule("StructType", "[ 'packed' ] StructTypePart")]
        private StructType ParseStructType(IExtendableSyntaxPart parent) {
            var result = new StructType();
            parent.Add(result);
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
            var result = new ClassTypeDeclarationSymbol() {
                ClassOf = EmptyTerminal(),
                ClassHelper = EmptyTerminal(),
                ClassDef = EmptyTerminal(),
                InterfaceDef = EmptyTerminal(),
                ObjectDecl = EmptyTerminal(),
                RecordDecl = EmptyTerminal(),
                RecordHelper = EmptyTerminal()
            };

            if (Match(TokenKind.Class) && LookAhead(1, TokenKind.Of)) {
                result.ClassOf = ParseClassOfDeclaration();
                return result;
            }

            if (Match(TokenKind.Class) && LookAhead(1, TokenKind.Helper)) {
                result.ClassHelper = ParseClassHelper();
                return result;
            }

            if (Match(TokenKind.Class)) {
                result.ClassDef = ParseClassDefinition(result);
                return result;
            }

            if (Match(TokenKind.Interface, TokenKind.DispInterface)) {
                result.InterfaceDef = ParseInterfaceDef(result);
                return result;
            }

            if (Match(TokenKind.Object)) {
                result.ObjectDecl = ParseObjectDecl(result);
                return result;
            }

            if (Match(TokenKind.Record) && LookAhead(1, TokenKind.Helper)) {
                result.RecordHelper = ParseRecordHelper(result);
                return result;
            }

            if (Match(TokenKind.Record)) {
                result.RecordDecl = ParseRecordDecl(result);
                return result;
            }

            Unexpected();
            return result;
        }

        #endregion
        #region ParseRecordDecl

        [Rule("RecordDecl", "'record' RecordFieldList (RecordVariantSection | RecordItems ) 'end' ")]
        private RecordDeclaration ParseRecordDecl(IExtendableSyntaxPart parent) {
            var result = new RecordDeclaration();
            InitByTerminal(result, parent, TokenKind.Record);

            if (MatchIdentifier() && !Match(TokenKind.Strict, TokenKind.Protected, TokenKind.Private, TokenKind.Public, TokenKind.Published, TokenKind.Automated)) {
                result.FieldList = ParseRecordFieldList(result, true);
            }

            if (Match(TokenKind.Case)) {
                result.VariantSection = ParseRecordVariantSection(result);
            }
            else {
                result.Items = ParseRecordItems(result);
            }

            ContinueWithOrMissing(result, TokenKind.End);
            return result;
        }

        #endregion
        #region ParseRecordItems

        [Rule("RecordItems", "{ RecordItem }")]
        private RecordItems ParseRecordItems(IExtendableSyntaxPart parent) {
            var result = new RecordItems();
            parent.Add(result);

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
                result.Attributes1 = ParseAttributes(result);
            }

            result.ClassItem = ContinueWith(result, TokenKind.Class);

            if (Match(TokenKind.OpenBraces)) {
                result.Attributes2 = ParseAttributes(result);
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
                result.VariantSection = ParseRecordVariantSection(result);
                return result;
            }

            if (!result.ClassItem && Match(TokenKind.Const)) {
                result.ConstSection = ParseConstSection(result, true);
                return result;
            }

            if (!result.ClassItem && Match(TokenKind.TypeKeyword)) {
                result.TypeSection = ParseTypeSection(result, true);
                return result;
            }

            if (MatchIdentifier() && (!Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Automated, TokenKind.Strict))) {
                if (mode == RecordDeclarationMode.Fields || mode == RecordDeclarationMode.ClassFields) {
                    result.ClassItem = mode == RecordDeclarationMode.ClassFields;
                    result.Fields = ParseRecordFieldList(result, true);
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
        private RecordVariantSection ParseRecordVariantSection(IExtendableSyntaxPart parent) {
            var result = new RecordVariantSection();
            InitByTerminal(result, parent, TokenKind.Case);

            if (MatchIdentifier() && LookAhead(1, TokenKind.Colon)) {
                result.Name = RequireIdentifier(result);
                ContinueWithOrMissing(result, TokenKind.Colon);
            }
            result.TypeDeclaration = ParseTypeSpecification(result);
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
            result.FieldList = ParseRecordFieldList(result, false);
            ContinueWithOrMissing(result, TokenKind.CloseParen);
            return result;
        }

        #endregion
        #region ParseRecordFieldList

        [Rule("RecordFieldList", " { RecordField } ")]
        private RecordFieldList ParseRecordFieldList(IExtendableSyntaxPart parent, bool requireSemicolon) {
            var result = new RecordFieldList();
            parent.Add(result);
            while (MatchIdentifier() && (!Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Strict))) {
                ParseRecordField(result, requireSemicolon);
            }
            return result;
        }

        #endregion
        #region ParseRecordField

        [Rule("RecordField", "IdentList ':' TypeSpecification Hints ';'")]
        private RecordField ParseRecordField(IExtendableSyntaxPart parent, bool requireSemicolon) {
            var result = new RecordField();
            parent.Add(result);
            result.Names = ParseIdentList(result, true);
            ContinueWithOrMissing(result, TokenKind.Colon);
            result.FieldType = ParseTypeSpecification(result);
            result.Hint = ParseHints(result, false);
            if (Match(TokenKind.Semicolon) || (requireSemicolon && (!Match(TokenKind.End))))
                ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseRecordHelper

        [Rule("RecordHelperDecl", "'record' 'helper' 'for' TypeName RecordHelperItems 'end'")]
        private RecordHelperDefinition ParseRecordHelper(IExtendableSyntaxPart parent) {
            var result = new RecordHelperDefinition();
            InitByTerminal(result, parent, TokenKind.Record);
            ContinueWithOrMissing(result, TokenKind.Helper);
            ContinueWithOrMissing(result, TokenKind.For);
            result.Name = ParseTypeName(result);
            result.Items = ParseRecordHelperItems(result);
            ContinueWithOrMissing(result, TokenKind.End);
            return result;
        }

        #endregion
        #region ParseRecordHelperItems

        [Rule("RecordHelperItems", " { RecordHelperItem }")]
        private RecordHelperItems ParseRecordHelperItems(IExtendableSyntaxPart parent) {
            var result = new RecordHelperItems();
            parent.Add(result);
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

            result.Attributes1 = ParseAttributes(result);
            result.ClassItem = ContinueWith(result, TokenKind.Class);
            result.Attributes2 = ParseAttributes(result);

            if (Match(TokenKind.Const)) {
                result.ConstDeclaration = ParseConstSection(result, true);
                mode = RecordDeclarationMode.Other;
                return result;
            }

            if (!result.ClassItem && Match(TokenKind.TypeKeyword)) {
                result.TypeSection = ParseTypeSection(result, true);
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
        private ObjectDeclaration ParseObjectDecl(IExtendableSyntaxPart parent) {
            var result = new ObjectDeclaration();
            InitByTerminal(result, parent, TokenKind.Object);
            result.ClassParent = ParseClassParent(result);
            result.Items = ParseObjectItems(result);

            ContinueWithOrMissing(result, TokenKind.End);
            return result;
        }

        #endregion
        #region ParseObjectItems

        [Rule("ObjectItems", " { ObjectItem } ")]
        private ObjectItems ParseObjectItems(IExtendableSyntaxPart parent) {
            var result = new ObjectItems();
            parent.Add(result);
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
                result.ConstSection = ParseConstSection(result, true);
                return result;
            }

            if (!result.ClassItem && Match(TokenKind.TypeKeyword)) {
                result.TypeSection = ParseTypeSection(result, true);
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
        private InterfaceDefinition ParseInterfaceDef(IExtendableSyntaxPart parent) {
            var result = new InterfaceDefinition();
            parent.Add(result);

            if (!ContinueWith(result, TokenKind.Interface)) {
                ContinueWithOrMissing(result, TokenKind.DispInterface);
                result.DisplayInterface = true;
            }
            result.ParentInterface = ParseClassParent(result);
            if (Match(TokenKind.OpenBraces))
                result.Guid = ParseInterfaceGuid(result);
            result.Items = ParseInterfaceItems(result);

            if (result.Items != null && result.Items.PartList != null && result.Items.PartList.Count > 0)
                ContinueWithOrMissing(result, TokenKind.End);
            else
                result.ForwardDeclaration = !ContinueWith(result, TokenKind.End);

            return result;
        }

        #endregion
        #region  ParseInterfaceItems

        [Rule("InterfaceItems", "{ InterfaceItem }")]
        private InterfaceItems ParseInterfaceItems(IExtendableSyntaxPart parent) {
            var result = new InterfaceItems();
            var unexpected = false;
            parent.Add(result);

            while ((!Match(TokenKind.End)) && (!unexpected)) {
                ParseInterfaceItem(result, out unexpected);
                if (unexpected && result.PartList.Count > 0) {
                    Unexpected();
                    return result;
                }
            }

            return result;
        }

        #endregion
        #region ParseInterfaceItem

        [Rule("InterfaceItem", "MethodDeclaration | PropertyDeclaration")]
        private InterfaceItem ParseInterfaceItem(IExtendableSyntaxPart parent, out bool unexpected) {
            var result = new InterfaceItem();
            parent.Add(result);
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
        private InterfaceGuid ParseInterfaceGuid(IExtendableSyntaxPart parent) {
            var result = new InterfaceGuid();
            InitByTerminal(result, parent, TokenKind.OpenBraces);

            if (Match(TokenKind.Identifier))
                result.IdIdentifier = RequireIdentifier(result);
            else
                result.Id = RequireString(result);

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
        public ClassHelperDefSymbol ParseClassHelper() {
            return new ClassHelperDefSymbol {
                ClassSymbol = ContinueWithOrMissing(TokenKind.Class),
                HelperSymbol = ContinueWithOrMissing(TokenKind.Helper),
                ClassParent = ParseClassParent(null),
                ForSymbol = ContinueWithOrMissing(TokenKind.For),
                HelperName = ParseTypeName(null),
                HelperItems = ParseClassHelperItems(),
                EndSymbol = ContinueWithOrMissing(TokenKind.End)
            };
        }

        #endregion
        #region ClassHelperItems

        /// <summary>
        ///     parse class helper items
        /// </summary>
        /// <returns></returns>

        [Rule("ClassHelperItems", " { ClassHelperItem }")]
        public ClassHelperItemsSymbol ParseClassHelperItems() {
            var result = new ClassHelperItemsSymbol(default);
            var mode = ClassDeclarationMode.Fields;

            using (var list = GetList<ClassHelperItemSymbol>()) {
                while (Tokenizer.HasNextToken && (!Match(TokenKind.End, TokenKind.Undefined)) && (mode != ClassDeclarationMode.Undefined)) {
                    list.Item.Add(ParseClassHelperItem(ref mode));
                    if (mode == ClassDeclarationMode.Undefined) {
                        Unexpected();
                        return result;
                    }
                }
            }
            return result;
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
            var result = new ClassHelperItemSymbol() {
                VarSymbol = EmptyTerminal(),
                ClassSymbol = EmptyTerminal(),
                Attributes1 = EmptyTerminal(),
                Attributes2 = EmptyTerminal(),
                StrictSymbol = EmptyTerminal(),
                MethodDeclaration = EmptyTerminal(),
                PropertyDeclaration = EmptyTerminal(),
                ConstDeclaration = EmptyTerminal(),
                TypeSection = EmptyTerminal(),
                FieldDeclaration = EmptyTerminal(),
            };

            if (Match(TokenKind.Var)) {
                result.VarSymbol = ContinueWith(TokenKind.Var);
                mode = ClassDeclarationMode.Fields;
                return result;
            }

            if (Match(TokenKind.Class) && LookAhead(1, TokenKind.Var)) {
                result.ClassSymbol = ContinueWith(TokenKind.Var);
                result.VarSymbol = ContinueWith(TokenKind.Class);
                mode = ClassDeclarationMode.ClassFields;
                return result;
            }

            result.Attributes1 = ParseAttributes(result);
            result.ClassSymbol = ContinueWith(TokenKind.Class) ?? EmptyTerminal();
            result.ClassItem = result.ClassSymbol.Kind == TokenKind.Class;
            result.Attributes2 = ParseAttributes(result);

            if (Match(TokenKind.Const)) {
                result.ConstDeclaration = ParseConstSection(result, true);
                mode = ClassDeclarationMode.Other;
                return result;
            }

            if (!result.ClassItem && Match(TokenKind.TypeKeyword)) {
                result.TypeSection = ParseTypeSection(result, true);
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
                result.PropertyDeclaration = ParsePropertyDeclaration();
                return result;
            }

            if (Match(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Strict, TokenKind.Published)) {
                result.StrictSymbol = ContinueWith(TokenKind.Strict);
                result.Strict = result.StrictSymbol != null;
                ContinueWith(result, TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Published);
                result.Visibility = result.LastTerminalKind;
                mode = ClassDeclarationMode.Other;
                return result;
            }

            if (MatchIdentifier() && (!Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Automated, TokenKind.Strict))) {

                if (mode == ClassDeclarationMode.Fields || mode == ClassDeclarationMode.ClassFields) {
                    result.FieldDeclaration = ParseClassFieldDeclararation();
                    result.ClassItem = mode == ClassDeclarationMode.ClassFields;
                    return result;
                }
                else {
                    Unexpected();
                }
            }

            mode = ClassDeclarationMode.Undefined;
            return result;
        }

        #endregion
        #region ParseClassDefinition

        [Rule("ClassDefinition", "'class' [( 'sealed' | 'abstract' )] [ClassParent] ClassItems 'end' ")]
        private ClassDeclarationSymbol ParseClassDefinition(IExtendableSyntaxPart parent) {
            var result = new ClassDeclarationSymbol();
            InitByTerminal(result, parent, TokenKind.Class);

            result.Sealed = ContinueWith(result, TokenKind.Sealed);
            result.Abstract = ContinueWith(result, TokenKind.Abstract);

            result.ClassParent = ParseClassParent(result);

            if (!Match(TokenKind.Semicolon))
                result.ClassItems = ParseClassItems(result);

            if (result.ClassItems != null && result.ClassItems.PartList != null && result.ClassItems.PartList.Count > 0)
                ContinueWithOrMissing(result, TokenKind.End);
            else
                result.ForwardDeclaration = !ContinueWith(result, TokenKind.End);

            return result;
        }

        #endregion
        #region ParseClassItems

        [Rule("ClassItems", "{ ClassItem } ")]
        private ClassDeclarationItems ParseClassItems(IExtendableSyntaxPart parent) {
            var result = new ClassDeclarationItems();
            parent.Add(result);

            var mode = ClassDeclarationMode.Fields;

            while ((!Match(TokenKind.End)) && (mode != ClassDeclarationMode.Undefined)) {
                ParseClassDeclarationItem(ref mode);

                if (mode == ClassDeclarationMode.Undefined && result.PartList.Count > 0) {
                    Unexpected();
                    return result;
                }
            }

            return result;
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

            var result = new ClassDeclarationItemSymbol() {
                VarSymbol = EmptyTerminal(),
                ClassSymbol = EmptyTerminal(),
                Attributes1 = EmptyTerminal(),
                Attributes2 = EmptyTerminal(),
                StrictSymbol = EmptyTerminal(),
                MethodResolution = EmptyTerminal(),
                MethodDeclaration = EmptyTerminal(),
                PropertyDeclaration = EmptyTerminal(),
                ConstSection = EmptyTerminal(),
                TypeSection = EmptyTerminal(),
                FieldDeclaration = EmptyTerminal(),
            };

            if (Match(TokenKind.Var)) {
                result.VarSymbol = ContinueWith(TokenKind.Var);
                mode = ClassDeclarationMode.Fields;
                return result;
            }

            if (Match(TokenKind.Class) && LookAhead(1, TokenKind.Var)) {
                result.ClassSymbol = ContinueWith(TokenKind.Class);
                result.VarSymbol = ContinueWith(TokenKind.Var);
                result.ClassItem = true;
                mode = ClassDeclarationMode.ClassFields;
                return result;
            }

            result.Attributes1 = ParseAttributes(result);
            result.ClassSymbol = ContinueWith(TokenKind.Class) ?? EmptyTerminal();
            result.ClassItem = result.ClassSymbol.Kind == TokenKind.Class;
            result.Attributes2 = ParseAttributes(result);

            if (Match(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Strict, TokenKind.Published, TokenKind.Automated)) {
                if (result.ClassItem) {
                    Unexpected();
                }
                else {
                    result.StrictSymbol = ContinueWith(TokenKind.Strict);
                    ContinueWith(result, TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Published, TokenKind.Automated);
                    result.Visibility = result.LastTerminalKind;
                }
                mode = ClassDeclarationMode.Fields;
                return result;
            }

            if (Match(TokenKind.Procedure, TokenKind.Function) && HasTokenBeforeToken(TokenKind.EqualsSign, TokenKind.Semicolon, TokenKind.OpenParen)) {
                if (result.ClassItem) {
                    Unexpected();
                }
                else {
                    result.MethodResolution = ParseMethodResolution(result);
                }
                mode = ClassDeclarationMode.Other;
                return result;
            }

            if (Match(TokenKind.Procedure, TokenKind.Function, TokenKind.Constructor, TokenKind.Destructor)) {
                mode = ClassDeclarationMode.Other;
                result.MethodDeclaration = ParseMethodDeclaration();
                return result;
            }

            if (Match(TokenKind.Property)) {
                mode = ClassDeclarationMode.Other;
                result.PropertyDeclaration = ParsePropertyDeclaration();
                return result;
            }

            if (!result.ClassItem && Match(TokenKind.Const)) {
                result.ConstSection = ParseConstSection(result, true);
                return result;
            }

            if (!result.ClassItem && Match(TokenKind.TypeKeyword)) {
                result.TypeSection = ParseTypeSection(result, true);
                return result;
            }

            if (MatchIdentifier() && (!Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Automated, TokenKind.Strict))) {
                if (mode == ClassDeclarationMode.Fields && result.ClassItem) {
                    ErrorMissingToken(TokenKind.Var);
                    mode = ClassDeclarationMode.ClassFields;
                }

                if (mode == ClassDeclarationMode.Fields || mode == ClassDeclarationMode.ClassFields) {
                    result.FieldDeclaration = ParseClassFieldDeclararation();
                    result.ClassItem = mode == ClassDeclarationMode.ClassFields;
                    return result;
                }
                else {
                    Unexpected();
                }
            }

            mode = ClassDeclarationMode.Undefined;
            return result;
        }

        #endregion
        #region ParseFieldDeclaration

        /// <summary>
        ///     parse a class field
        /// </summary>
        /// <returns></returns>

        [Rule("ClassFieldDeclaration", "IdentList ':' TypeSpecification Hints ';'")]
        public ClassFieldSymbol ParseClassFieldDeclararation() {
            var result = new ClassFieldSymbol();
            result.Names = ParseIdentList(result, true);
            result.ColonSymbol = ContinueWithOrMissing(TokenKind.Colon);
            result.TypeDecl = ParseTypeSpecification(result);
            result.Hint = ParseHints(result, false);
            result.Semicolon = ContinueWithOrMissing(TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region PropertyDeclaration

        /// <summary>
        ///     class property
        /// </summary>
        /// <returns></returns>

        [Rule("PropertyDeclaration", "'property' Identifier [ '[' FormalParameters  ']' ] [ ':' TypeName ] [ 'index' Expression ]  { ClassPropertySpecifier } ';' [ 'default' ';' ]  ")]
        public ClassPropertySymbol ParsePropertyDeclaration() {
            var result = new ClassPropertySymbol(default) {
                PropertySymbol = ContinueWithOrMissing(TokenKind.Property),
                PropertyName = RequireIdentifier(null)
            };

            if (Match(TokenKind.OpenBraces)) {
                result.OpenBraces = ContinueWith(TokenKind.OpenBraces);
                result.ArrayIndex = ParseFormalParameters(result);
                result.CloseBraces = ContinueWithOrMissing(TokenKind.CloseBraces);
            }
            else {
                result.OpenBraces = EmptyTerminal();
                result.ArrayIndex = EmptyTerminal();
                result.CloseBraces = EmptyTerminal();
            }

            if (Match(TokenKind.Colon)) {
                result.ColonSymbol = ContinueWith(TokenKind.Colon);
                result.TypeName = ParseTypeName(result);
            }
            else {
                result.ColonSymbol = EmptyTerminal();
                result.TypeName = EmptyTerminal();
            }

            if (Match(TokenKind.Index)) {
                result.IndexSymbol = ContinueWith(TokenKind.Index);
                result.PropertyIndex = ParseExpression(result);
            }
            else {
                result.IndexSymbol = EmptyTerminal();
                result.PropertyIndex = EmptyTerminal();
            }

            using (var list = GetList<ClassPropertySpecifierSymbol>()) {
                while (Match(TokenKind.Read, TokenKind.Write, TokenKind.Add, TokenKind.Remove, TokenKind.ReadOnly, TokenKind.WriteOnly, TokenKind.DispId) ||
                    Match(TokenKind.Default, TokenKind.Stored, TokenKind.Implements, TokenKind.NoDefault)) {
                    list.Item.Add(ParseClassPropertyAccessSpecifier());
                }
            }

            result.Semicolon = ContinueWithOrMissing(TokenKind.Semicolon);

            if (Match(TokenKind.Default)) {
                result.DefaultSymbol = ContinueWith(TokenKind.Default);
                result.IsDefault = true;
                result.Semicolon2 = ContinueWithOrMissing(TokenKind.Semicolon);
            }
            else {
                result.DefaultSymbol = EmptyTerminal();
                result.Semicolon2 = EmptyTerminal();
            }

            return result;
        }

        #endregion
        #region ParseClassPropertyAccessSpecifier

        /// <summary>
        ///     parse a class property access specifier
        /// </summary>
        /// <returns></returns>

        [Rule("ClassPropertySpecifier", "ClassPropertyReadWrite | ClassPropertyDispInterface | ('stored' Expression ';') | ('default' [ Expression ] ';' ) | ('nodefault' ';') | ('implements' NamespaceName) ")]
        public ClassPropertySpecifierSymbol ParseClassPropertyAccessSpecifier() {
            var result = new ClassPropertySpecifierSymbol() {
                PropertyReadWrite = EmptyTerminal(),
                PropertyDispInterface = EmptyTerminal(),
                StoredSymbol = EmptyTerminal(),
                StoredProperty = EmptyTerminal(),
                DefaultSymbol = EmptyTerminal(),
                DefaultProperty = EmptyTerminal(),
                NoDefaultSymbol = EmptyTerminal(),
                ImplentsSymbol = EmptyTerminal(),
                ImplementsTypeId = EmptyTerminal(),
            };

            if (Match(TokenKind.Read, TokenKind.Write, TokenKind.Add, TokenKind.Remove)) {
                result.PropertyReadWrite = ParseClassPropertyReadWrite();
                return result;
            }

            if (Match(TokenKind.ReadOnly, TokenKind.WriteOnly, TokenKind.DispId)) {
                result.PropertyDispInterface = ParseClassPropertyDispInterface();
                return result;
            }

            if (Match(TokenKind.Stored)) {
                result.StoredSymbol = ContinueWith(TokenKind.Stored);
                result.IsStored = true;
                result.StoredProperty = ParseExpression(result);
                return result;
            }

            if (Match(TokenKind.Default)) {
                result.DefaultSymbol = ContinueWith(TokenKind.Default);
                result.IsDefault = true;
                if (!Match(TokenKind.Semicolon)) {
                    result.DefaultProperty = ParseExpression(result);
                }
                return result;
            }

            if (Match(TokenKind.NoDefault)) {
                result.NoDefaultSymbol = ContinueWith(TokenKind.NoDefault);
                result.NoDefault = true;
                return result;
            }

            if (Match(TokenKind.Implements)) {
                result.ImplentsSymbol = ContinueWith(TokenKind.Implements);
                result.ImplementsTypeId = ParseNamespaceName(result);
                return result;
            }

            Unexpected();
            return result;
        }

        #endregion
        #region ParseClassPropertyDispInterface

        /// <summary>
        ///     parse a disp interface section
        /// </summary>
        /// <returns></returns>

        [Rule("ClassPropertyDispInterface", "( 'readonly' ';')  | ( 'writeonly' ';' ) | DispIdDirective ")]
        public ClassPropertyDispInterfaceSymbols ParseClassPropertyDispInterface() {
            var result = new ClassPropertyDispInterfaceSymbols() {
                DispId = EmptyTerminal(),
                Modifier = EmptyTerminal()
            };

            if (Match(TokenKind.ReadOnly)) {
                result.Modifier = ContinueWith(TokenKind.ReadOnly);
                result.ReadOnly = true;
                return result;
            }

            if (Match(TokenKind.WriteOnly)) {
                result.Modifier = ContinueWith(TokenKind.WriteOnly);
                result.WriteOnly = true;
                return result;
            }

            result.DispId = ParseDispIdDirective(false);
            return result;
        }

        #endregion
        #region ParseDispIdDirective

        /// <summary>
        ///     parse a dispid directive
        /// </summary>
        /// <param name="requireSemi"></param>
        /// <returns></returns>

        [Rule("DispIdDirective", "'dispid' Expression ';'")]
        public DispIdSymbol ParseDispIdDirective(bool requireSemi = true) {
            return new DispIdSymbol() {
                DispId = ContinueWithOrMissing(TokenKind.DispId),
                DispExpression = ParseExpression(null),
                Semicolon = requireSemi ? ContinueWithOrMissing(TokenKind.Semicolon) : EmptyTerminal()
            };
        }

        #endregion
        #region ParseClassPropertyReadWrite

        /// <summary>
        ///     parse a property read write modifier
        /// </summary>
        /// <returns></returns>

        [Rule("ClassPropertyReadWrite", "('read' | 'write' | 'add' | 'remove' ) NamespaceName ")]
        public ClassPropertyReadWriteSymbol ParseClassPropertyReadWrite() {
            return new ClassPropertyReadWriteSymbol {
                Modifier = ContinueWithOrMissing(TokenKind.Read, TokenKind.Write, TokenKind.Add, TokenKind.Remove),
                Member = ParseNamespaceName(null)
            };
        }

        #endregion
        #region ParseTypeSection

        [Rule("TypeSection", "'type' TypeDeclaration { TypeDeclaration }")]
        private TypeSection ParseTypeSection(IExtendableSyntaxPart parent, bool inClassDeclaration) {
            var result = new TypeSection();
            InitByTerminal(result, parent, TokenKind.TypeKeyword);

            while ((!inClassDeclaration || !Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Automated, TokenKind.Strict)) && MatchIdentifier(TokenKind.OpenBraces)) {
                ParseTypeDeclaration(result);
            };

            return result;
        }

        #endregion
        #region ParseTypeDeclaration

        [Rule("TypeDeclaration", "[ Attributes ] GenericTypeIdent '=' TypeDeclaration Hints ';' ")]
        private TypeDeclaration ParseTypeDeclaration(IExtendableSyntaxPart parent) {
            var result = new TypeDeclaration();
            parent.Add(result);
            result.Attributes = ParseAttributes(result);
            result.TypeId = ParseGenericTypeIdent(result);
            ContinueWithOrMissing(result, TokenKind.EqualsSign);
            result.TypeSpecification = ParseTypeSpecification(result);
            result.Hint = ParseHints(result, false);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseGenericTypeIdent

        [Rule("GenericTypeIdent", "Ident [ GenericDefintion ] ")]
        private GenericTypeIdentifier ParseGenericTypeIdent(IExtendableSyntaxPart parent) {
            var result = new GenericTypeIdentifier();
            parent.Add(result);
            result.Identifier = RequireIdentifier(result);
            if (Match(TokenKind.AngleBracketsOpen)) {
                result.GenericDefinition = ParseGenericDefinition(result);
            }
            return result;
        }

        #endregion
        #region ParseMethodResolution

        [Rule("MethodResolution", "( 'function' | 'procedure' ) NamespaceName '=' Identifier ';' ")]
        private MethodResolution ParseMethodResolution(IExtendableSyntaxPart parent) {
            var result = new MethodResolution();
            InitByTerminal(result, parent, TokenKind.Function, TokenKind.Procedure);
            result.Kind = result.LastTerminalKind;
            result.Name = ParseTypeName(result);
            ContinueWithOrMissing(result, TokenKind.EqualsSign);
            result.ResolveIdentifier = RequireIdentifier(result);
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
            var result = new ClassMethodSymbol {
                MethodSymbol = ContinueWithOrMissing(TokenKind.Constructor, TokenKind.Destructor, TokenKind.Procedure, TokenKind.Function, TokenKind.Operator)
            };

            var isInOperator = result.MethodKind == TokenKind.Operator && Match(TokenKind.In);
            result.Identifier = RequireIdentifier(result, isInOperator);

            if (Match(TokenKind.AngleBracketsOpen)) {
                result.GenericDefinition = ParseGenericDefinition(result);
            }
            else {
                result.GenericDefinition = EmptyTerminal();
            }

            if (Match(TokenKind.OpenParen)) {
                result.OpenParen = ContinueWith(TokenKind.OpenParen);
                result.Parameters = ParseFormalParameters(result);
                result.CloseParen = ContinueWithOrMissing(TokenKind.CloseParen);
            }
            else {
                result.OpenParen = EmptyTerminal();
                result.Parameters = EmptyTerminal();
                result.CloseParen = EmptyTerminal();
            }

            if (Match(TokenKind.Colon)) {
                result.ColonSymbol = ContinueWith(TokenKind.Colon);
                result.ResultAttributes = ParseAttributes(result);
                result.ResultType = ParseTypeSpecification(result);
            }
            else {
                result.ColonSymbol = EmptyTerminal();
                result.ResultAttributes = EmptyTerminal();
                result.ResultType = EmptyTerminal();
            }

            result.Semicolon = ContinueWithOrMissing(TokenKind.Semicolon);
            result.Directives = ParseMethodDirectives(result);
            return result;
        }

        #endregion
        #region FormalParameters

        [Rule("FormalParameters", "FormalParameter { ';' FormalParameter }")]
        private FormalParameters ParseFormalParameters(IExtendableSyntaxPart parent) {
            var result = new FormalParameters();
            parent.Add(result);

            do {
                if (!Match(TokenKind.CloseParen))
                    ParseFormalParameter(result);
            } while (ContinueWith(result, TokenKind.Semicolon));

            return result;
        }

        #endregion
        #region FormalParameter

        [Rule("FormalParameter", "[Attributes] [( 'const' | 'var' | 'out' )] [Attributes] IdentList [ ':' TypeDeclaration ] [ '=' Expression ]")]
        private void ParseFormalParameter(IExtendableSyntaxPart parent) {
            var kind = TokenKind.Undefined - 1;
            var parentDefinition = new FormalParameterDefinition();
            parent.Add(parentDefinition);

            do {
                var result = new FormalParameter();
                parentDefinition.Add(result);

                if (Match(TokenKind.OpenBraces)) {
                    result.Attributes1 = ParseAttributes(result);
                }

                if (kind < TokenKind.Undefined && ContinueWith(result, TokenKind.Const, TokenKind.Var, TokenKind.Out)) {
                    kind = result.LastTerminalKind;
                }
                else if (kind < TokenKind.Undefined - 1) {
                    kind = TokenKind.Undefined;
                }

                if (kind > TokenKind.Undefined) {
                    result.ParameterType = kind;
                }

                if (Match(TokenKind.OpenBraces)) {
                    result.Attributes2 = ParseAttributes(result);
                }

                result.ParameterName = RequireIdentifier(result, true);


            } while (ContinueWith(parentDefinition, TokenKind.Comma));

            if (ContinueWith(parentDefinition, TokenKind.Colon)) {
                parentDefinition.TypeDeclaration = ParseTypeSpecification(parentDefinition);
            }

            if (ContinueWith(parentDefinition, TokenKind.EqualsSign)) {
                parentDefinition.DefaultValue = ParseExpression(parentDefinition);
            }

        }

        #endregion
        #region ParseIdentList

        [Rule("IdentList", "Identifier { ',' Identifier }")]
        private IdentifierList ParseIdentList(IExtendableSyntaxPart parent, bool allowAttributes) {
            var result = new IdentifierList();
            parent.Add(result);

            do {
                if (allowAttributes && Match(TokenKind.OpenBraces))
                    ParseAttributes(result);
                RequireIdentifier(result);
            } while (ContinueWith(result, TokenKind.Comma));

            return result;
        }

        #endregion
        #region ParseGenericDefinition

        [Rule("GenericDefinition", "SimpleGenericDefinition | ConstrainedGenericDefinition")]
        private GenericDefinition ParseGenericDefinition(IExtendableSyntaxPart parent) {
            if (!LookAhead(2, TokenKind.Comma, TokenKind.AngleBracketsClose)) {
                return ParseConstrainedGenericDefinition(parent);
            }

            return ParseSimpleGenericDefinition(parent);
        }

        #endregion
        #region SimpleGenericDefinition

        [Rule("SimpleGenericDefinition", "'<' Identifier { ',' Identifier } '>'")]
        private GenericDefinition ParseSimpleGenericDefinition(IExtendableSyntaxPart parent) {
            var result = new GenericDefinition();
            InitByTerminal(result, parent, TokenKind.AngleBracketsOpen);

            do {
                var part = new GenericDefinitionPart();
                result.Add(part);
                part.Identifier = RequireIdentifier(result);
            } while (ContinueWith(result, TokenKind.Comma));

            ContinueWithOrMissing(result, TokenKind.AngleBracketsClose);
            return result;
        }

        #endregion
        #region ParseConstrainedGenericDefinition

        [Rule("ConstrainedGenericDefinition", "'<' GenericDefinitionPart { ';' GenericDefinitionPart } '>'")]
        private GenericDefinition ParseConstrainedGenericDefinition(IExtendableSyntaxPart parent) {
            var result = new GenericDefinition();
            InitByTerminal(result, parent, TokenKind.AngleBracketsOpen);

            do {
                ParseGenericDefinitionPart(result);
            } while (ContinueWith(result, TokenKind.Semicolon));

            ContinueWithOrMissing(result, TokenKind.AngleBracketsClose);
            return result;

        }

        #endregion
        #region ParseGenericDefinitionPart

        [Rule("GenericDefinitionPart", "Identifier [ ':' GenericConstraint { ',' GenericConstraint } ]")]
        private GenericDefinitionPart ParseGenericDefinitionPart(IExtendableSyntaxPart parent) {

            var result = new GenericDefinitionPart();
            result.Identifier = RequireIdentifier(result);
            parent.Add(result);

            if (ContinueWith(result, TokenKind.Colon)) {
                do {
                    ParseGenericConstraint(result);
                } while (ContinueWith(result, TokenKind.Comma));
            }

            return result;
        }

        #endregion
        #region ParseGenericConstraint

        [Rule("GenericConstraint", " 'record' | 'class' | 'constructor' | Identifier ")]
        private ConstrainedGeneric ParseGenericConstraint(IExtendableSyntaxPart parent) {
            var result = new ConstrainedGeneric();
            parent.Add(result);

            if (ContinueWith(result, TokenKind.Record)) {
                result.RecordConstraint = true;
            }
            else if (ContinueWith(result, TokenKind.Class)) {
                result.ClassConstraint = true;
            }
            else if (ContinueWith(result, TokenKind.Constructor)) {
                result.ConstructorConstraint = true;
            }
            else {
                result.ConstraintIdentifier = RequireIdentifier(result);
            }

            return result;
        }

        #endregion
        #region ParseClassParent

        [Rule("ClassParent", " [ '(' TypeName { ',' TypeName } ')' ]")]
        private ParentClass ParseClassParent(IExtendableSyntaxPart parent) {
            var result = new ParentClass();

            if (parent != null)
                parent.Add(result);

            if (ContinueWith(result, TokenKind.OpenParen)) {
                do {
                    ParseTypeName(result);
                } while (ContinueWith(result, TokenKind.Comma));
                ContinueWithOrMissing(result, TokenKind.CloseParen);
            }

            return result;
        }

        #endregion
        #region ParseClassOfDeclaration

        /// <summary>
        ///     class of declaration
        /// </summary>
        /// <returns></returns>

        [Rule("ClassOfDeclaration", "'class' 'of' TypeName")]
        public ClassOfDeclarationSymbol ParseClassOfDeclaration() {
            return new ClassOfDeclarationSymbol {
                ClassSymbol = ContinueWithOrMissing(TokenKind.Class),
                OfSymbol = ContinueWithOrMissing(TokenKind.Of),
                TypeRef = ParseTypeName(null)
            };
        }

        #endregion
        #region ParseTypeName

        [Rule("TypeName", "'string' | 'ansistring' | 'shortstring' | 'unicodestring' | 'widestring' | (NamespaceName [ GenericSuffix ]) ")]
        private TypeName ParseTypeName(IExtendableSyntaxPart parent, bool inDesignator = false) {
            var result = new TypeName();

            if (parent != null)
                parent.Add(result);

            if (ContinueWith(result, TokenKind.String, TokenKind.AnsiString, TokenKind.ShortString, TokenKind.UnicodeString, TokenKind.WideString)) {
                result.StringType = result.LastTerminalKind;
                return result;
            }

            do {
                ParseGenericNamespaceName(result, true, inDesignator);
            } while ((!inDesignator || !IsLastDisignatorPart()) && ContinueWith(result, TokenKind.Dot));

            return result;
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
                result.TypeDefinition = ParseTypeSpecification(result);
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
            result.TypeDefinition = ParseTypeSpecification(result);
            return result;
        }

        #endregion
        #region ParseGenericSuffix

        [Rule("GenericSuffix", "'<' TypeDefinition { ',' TypeDefinition '}' '>'")]
        private GenericSuffix ParseGenericSuffix(IExtendableSyntaxPart parent) {
            var result = new GenericSuffix();
            InitByTerminal(result, parent, TokenKind.AngleBracketsOpen);

            do {
                ParseTypeSpecification(result);
            } while (ContinueWith(result, TokenKind.Comma));

            ContinueWithOrMissing(result, TokenKind.AngleBracketsClose);
            return result;
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

            if (openBraces != null) {
                using (var list = GetList<ArrayIndexSymbol>()) {
                    var index = default(ArrayIndexSymbol);
                    do {
                        index = ParseArrayIndex();
                        list.Item.Add(index);
                    } while (index.Comma != null);

                    items = list.Item.ToImmutableArray();
                }

                closeBraces = ContinueWithOrMissing(TokenKind.CloseBraces);
            }

            var ofSymbol = ContinueWithOrMissing(TokenKind.Of);
            var constSymbol = ContinueWith(TokenKind.Const);
            var typeSpecification = default(TypeSpecification);

            if (constSymbol == null)
                typeSpecification = ParseTypeSpecification(null);

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
        private PointerType ParsePointerType(IExtendableSyntaxPart parent) {
            var result = new PointerType();
            parent.Add(result);

            if (ContinueWith(result, TokenKind.Pointer)) {
                result.GenericPointer = true;
                return result;
            }

            ContinueWithOrMissing(result, TokenKind.Circumflex);
            result.TypeSpecification = ParseTypeSpecification(result);
            return result;
        }

        #endregion
        #region ParseAttributes

        [Rule("Attributes", "{ '[' Attribute | AssemblyAttribue ']' }")]
        private SyntaxPartBase ParseAttributes(IExtendableSyntaxPart parent, UserAttributes result = null) {
            while (Match(TokenKind.OpenBraces)) {

                if (result == null) {
                    result = new UserAttributes();
                    parent.Add(result);
                }

                if (LookAhead(1, TokenKind.Assembly)) {
                    ParseAssemblyAttribute();
                }
                else {
                    ContinueWithOrMissing(result, TokenKind.OpenBraces);
                    do {
                        ParseAttribute(result);
                    } while (ContinueWith(result, TokenKind.Comma));
                    ContinueWithOrMissing(result, TokenKind.CloseBraces);
                }
            }
            return (SyntaxPartBase)result ?? EmptyTerminal();
        }

        #endregion
        #region ParseAttribute

        [Rule("Attribute", " [ 'Result' ':' ] NamespaceName [ '(' Expressions ')' ]")]
        private UserAttributeDefinition ParseAttribute(IExtendableSyntaxPart parent) {
            var result = new UserAttributeDefinition();

            if (parent != null)
                parent.Add(result);

            if (LookAhead(1, TokenKind.Colon)) {
                result.Prefix = RequireIdentifier(result, true);
                ContinueWith(result, TokenKind.Colon);
            }

            result.Name = ParseNamespaceName(result);

            if (ContinueWith(result, TokenKind.OpenParen)) {
                while (!Match(TokenKind.CloseParen)) {
                    result.Expressions = ParseExpressions(result);
                }
                ContinueWithOrMissing(result, TokenKind.CloseParen);
            }

            return result;
        }

        #endregion
        #region Expressions

        [Rule("Expressions", "Expression { ',' Expression }")]
        private ExpressionList ParseExpressions(IExtendableSyntaxPart parent) {
            var result = new ExpressionList();
            parent.Add(result);

            do {
                ParseExpression(result);
            } while (ContinueWith(result, TokenKind.Comma));

            return result;
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
        /// <returns></returns>

        [Rule("ConstantExpression", " '(' ( RecordConstant | ConstantExpression ) ')' | Expression")]
        public ConstantExpressionSymbol ParseConstantExpression(bool fromDesignator = false, bool fromTypeConstExpression = false) {
            var result = new ConstantExpressionSymbol(default) {
                OpenParen = EmptyTerminal(),
                CloseParen = EmptyTerminal(),
                Value = null,
            };

            if (Match(TokenKind.OpenParen)) {

                if (LookAhead(1, TokenKind.CloseParen) || (LookAheadIdentifier(1, new int[0], true) && (LookAhead(2, TokenKind.Colon)))) {
                    result.IsRecordConstant = true;
                    result.OpenParen = ContinueWithOrMissing(TokenKind.OpenParen);
                    var record = default(RecordConstantExpression);
                    using (var list = GetList<RecordConstantExpression>()) {
                        do {
                            if (!Match(TokenKind.CloseParen)) {
                                record = ParseRecordConstant(result);
                                record.Separator = ContinueWith(TokenKind.Semicolon) ?? EmptyTerminal();
                                list.Item.Add(record);
                            }
                            else {
                                record = default;
                            }
                        } while (record != default && record.Separator.Kind == TokenKind.Semicolon && Tokenizer.HasNextToken);
                    }
                    result.CloseParen = ContinueWithOrMissing(TokenKind.CloseParen);
                    return result;
                }

                if (IsArrayConstant() || fromDesignator) {
                    result.IsArrayConstant = true;
                    result.OpenParen = ContinueWithOrMissing(TokenKind.OpenParen);
                    var expr = default(ConstantExpressionSymbol);
                    using (var list = GetList<ConstantExpressionSymbol>()) {
                        do {
                            if (!Match(TokenKind.CloseParen)) {
                                expr = ParseConstantExpression();
                                expr.Separator = ContinueWith(TokenKind.Comma) ?? EmptyTerminal();
                                list.Item.Add(expr);
                            }
                            else {
                                expr = default;
                            }
                        } while (expr != default && expr.Separator.Kind == TokenKind.Comma && Tokenizer.HasNextToken);
                    }
                    result.CloseParen = ContinueWithOrMissing(TokenKind.CloseParen);
                    return result;
                }

                if (!fromDesignator) {
                    result.Value = ParseExpression(result);
                    return result;
                }

                Unexpected();
                return result;
            }

            result.Value = ParseExpression(result, fromTypeConstExpression);
            return result;
        }

        #endregion
        #region ParseRecordConstant

        [Rule("RecordConstantExpression", "Identifier ':' ConstantExpression")]
        private RecordConstantExpression ParseRecordConstant(IExtendableSyntaxPart parent) {
            var result = new RecordConstantExpression();
            parent.Add(result);
            result.Name = RequireIdentifier(result);
            ContinueWithOrMissing(result, TokenKind.Colon);
            result.Value = ParseConstantExpression();
            return result;
        }

        #endregion
        #region ParseExpression

        [Rule("Expression", "SimpleExpression [ ('<'|'<='|'>'|'>='|'<>'|'='|'in'|'is') SimpleExpression ] | ClosureExpression")]
        private Expression ParseExpression(IExtendableSyntaxPart parent, bool fromConstTypeDeclaration = false) {
            var result = new Expression();

            if (parent != null)
                parent.Add(result);

            if (Match(TokenKind.Function, TokenKind.Procedure)) {
                result.ClosureExpression = ParseClosureExpression();
            }
            else {
                result.LeftOperand = ParseSimpleExpression(result);

                if (fromConstTypeDeclaration && !HasTokenBeforeToken(TokenKind.EqualsSign, new[] { TokenKind.Semicolon }))
                    return result;

                if (ContinueWith(result, TokenKind.LessThen, TokenKind.LessThenEquals, TokenKind.GreaterThen, TokenKind.GreaterThenEquals, TokenKind.NotEquals, TokenKind.EqualsSign, TokenKind.In, TokenKind.Is)) {
                    result.Kind = result.LastTerminalKind;
                    result.RightOperand = ParseSimpleExpression(result);
                }
            }

            return result;
        }

        #endregion
        #region ParseSimpleExpression

        [Rule("SimpleExpression", "Term { ('+'|'-'|'or'|'xor') SimpleExpression }")]
        private SimpleExpression ParseSimpleExpression(IExtendableSyntaxPart parent) {
            var result = new SimpleExpression();
            parent.Add(result);

            result.LeftOperand = ParseTerm(result);
            if (ContinueWith(result, TokenKind.Plus, TokenKind.Minus, TokenKind.Or, TokenKind.Xor)) {
                result.Kind = result.LastTerminalKind;
                result.RightOperand = ParseSimpleExpression(result);
            }

            return result;
        }

        #endregion
        #region ParseTerm

        [Rule("Term", "Factor [ ('*'|'/'|'div'|'mod'|'and'|'shl'|'shr'|'as') Term ]")]
        private Term ParseTerm(IExtendableSyntaxPart parent) {
            var result = new Term();
            parent.Add(result);

            result.LeftOperand = ParseFactor(result);

            if (ContinueWith(result, TokenKind.Times, TokenKind.Slash, TokenKind.Div, TokenKind.Mod, TokenKind.And, TokenKind.Shl, TokenKind.Shr, TokenKind.As)) {
                result.Kind = result.LastTerminalKind;
                result.RightOperand = ParseTerm(result);
            }

            return result;
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
        private Factor ParseFactor(IExtendableSyntaxPart parent) {
            var result = new Factor();
            parent.Add(result);

            if (ContinueWith(result, TokenKind.At)) {
                result.AddressOf = ParseFactor(result);
                return result;
            }

            if (ContinueWith(result, TokenKind.Not)) {
                result.Not = ParseFactor(result);
                return result;
            }

            if (ContinueWith(result, TokenKind.Plus)) {
                result.Plus = ParseFactor(result);
                return result;
            }

            if (ContinueWith(result, TokenKind.Minus)) {
                result.Minus = ParseFactor(result);
                return result;
            }

            if (ContinueWith(result, TokenKind.Circumflex)) {
                result.PointerTo = RequireIdentifier(result);
                return result;
            }

            if (Match(TokenKind.Integer)) {
                result.IntValue = RequireInteger(result);

                if (Match(TokenKind.Dot))
                    result.RecordHelper = ParseDesignator(result);

                return result;
            }

            if (Match(TokenKind.HexNumber)) {
                result.HexValue = RequireHexValue(result);

                if (Match(TokenKind.Dot))
                    result.RecordHelper = ParseDesignator(result);

                return result;
            }

            if (Match(TokenKind.Real)) {
                result.RealValue = RequireRealValue(result);

                if (Match(TokenKind.Dot))
                    result.RecordHelper = ParseDesignator(result);

                return result;
            }

            if (Match(TokenKind.QuotedString)) {
                result.StringValue = RequireString(result);

                if (Match(TokenKind.Dot))
                    result.RecordHelper = ParseDesignator(result);

                return result;
            }

            if (ContinueWith(result, TokenKind.True)) {
                result.IsTrue = true;

                if (Match(TokenKind.Dot))
                    result.RecordHelper = ParseDesignator(result);

                return result;
            }

            if (ContinueWith(result, TokenKind.False)) {
                result.IsFalse = true;

                if (Match(TokenKind.Dot))
                    result.RecordHelper = ParseDesignator(result);

                return result;
            }

            if (ContinueWith(result, TokenKind.Nil)) {
                result.IsNil = true;
                return result;
            }

            if (Match(TokenKind.OpenBraces)) {
                result.SetSection = ParseSetSection(result);
                return result;
            }

            if (MatchIdentifier(TokenKind.Inherited, TokenKind.ShortString, TokenKind.String, TokenKind.WideString, TokenKind.UnicodeString, TokenKind.AnsiString)) {
                result.Designator = ParseDesignator(result);
                return result;
            }

            if (Match(TokenKind.Dot, TokenKind.OpenBraces)) {
                result.Designator = ParseDesignator(result);
                return result;
            }

            if (Match(TokenKind.OpenParen) && IsDesignator()) {
                result.Designator = ParseDesignator(result);
                return result;
            }

            if (ContinueWith(result, TokenKind.OpenParen)) {
                result.ParenExpression = ParseConstantExpression(false, false);
                ContinueWithOrMissing(result, TokenKind.CloseParen);
                return result;
            }

            Unexpected();
            return null;
        }

        #endregion
        #region ParseDesignator

        [Rule("Designator", "[ 'inherited' ] [ NamespaceName ] { DesignatorItem }")]
        private DesignatorStatement ParseDesignator(IExtendableSyntaxPart parent) {
            var result = new DesignatorStatement();
            parent.Add(result);
            result.Inherited = ContinueWith(result, TokenKind.Inherited);
            if (MatchIdentifier(TokenKind.String, TokenKind.ShortString, TokenKind.AnsiString, TokenKind.WideString, TokenKind.String) && LookAhead(1, TokenKind.Dot, TokenKind.AngleBracketsOpen)) {
                result.Name = ParseTypeName(result, true);
            }

            ISyntaxPart item;
            var hasId = result.Name != null;
            do {
                item = ParseDesignatorItem(result, hasId);
                hasId = hasId || (item is DesignatorItem di && di.Subitem != null);
            } while (item != null);

            return result;
        }

        #endregion
        #region ParseDesignatorItem

        [Rule("DesignatorItem", "'^' | '.' Ident [GenericSuffix] | '[' ExpressionList ']' | '(' [ FormattedExpression  { ',' FormattedExpression } ] ')'")]
        private ISyntaxPart ParseDesignatorItem(SyntaxPartBase parent, bool hasIdentifier) {
            if (Match(TokenKind.Circumflex)) {
                var refResult = new DesignatorItem();
                InitByTerminal(refResult, parent, TokenKind.Circumflex);
                refResult.Dereference = true;
                return refResult;
            }

            DesignatorItem result = null;

            if (!hasIdentifier && MatchIdentifier(TokenKind.String, TokenKind.ShortString, TokenKind.AnsiString, TokenKind.UnicodeString)) {
                result = new DesignatorItem();
                parent.Add(result);
                result.Subitem = RequireIdentifier(result, true);
            }

            if (Match(TokenKind.Dot)) {
                if (result == null) {
                    result = new DesignatorItem();
                    InitByTerminal(result, parent, TokenKind.Dot);
                    result.Subitem = RequireIdentifier(result, true);
                }
            }

            if (Match(TokenKind.AngleBracketsOpen) &&
                LookAheadIdentifier(1, new[] { TokenKind.String, TokenKind.ShortString, TokenKind.WideString, TokenKind.UnicodeString, TokenKind.AnsiString, TokenKind.Pointer }, false)) {
                var whereCloseBrackets = HasTokenUntilToken(new[] { TokenKind.AngleBracketsClose }, TokenKind.Identifier, TokenKind.Dot, TokenKind.Comma, TokenKind.AngleBracketsOpen, TokenKind.String, TokenKind.ShortString, TokenKind.WideString, TokenKind.UnicodeString, TokenKind.AnsiString, TokenKind.Pointer);
                if (whereCloseBrackets.Item1 && (!LookAheadIdentifier(1 + whereCloseBrackets.Item2, new[] { TokenKind.HexNumber, TokenKind.Integer, TokenKind.Real }, false) || LookAhead(1 + whereCloseBrackets.Item2, TokenKind.Read, TokenKind.Write, TokenKind.ReadOnly, TokenKind.WriteOnly, TokenKind.Add, TokenKind.Remove, TokenKind.DispId))) {
                    result.SubitemGenericType = ParseGenericSuffix(result);
                }
            }


            if (Match(TokenKind.OpenBraces)) {
                if (result == null) {
                    result = new DesignatorItem();
                    InitByTerminal(result, parent, TokenKind.OpenBraces);
                }
                else {
                    ContinueWith(result, TokenKind.OpenBraces);
                }

                result.IndexExpression = ParseExpressions(result);
                ContinueWithOrMissing(result, TokenKind.CloseBraces);
                return result;
            }

            if (Match(TokenKind.OpenParen)) {
                var prevDesignatorItem = parent.PartList != null && parent.PartList.Count > 0 ? parent.PartList[parent.PartList.Count - 1] as DesignatorItem : null;
                if (!hasIdentifier && (!IsDesignator()) && ((prevDesignatorItem == null) || (prevDesignatorItem.Subitem == null))) {
                    ContinueWithOrMissing(parent, TokenKind.OpenParen);
                    var children = ParseConstantExpression(true);
                    ContinueWithOrMissing(parent, TokenKind.CloseParen);
                    return children;
                }

                if (result == null) {
                    result = new DesignatorItem();
                    InitByTerminal(result, parent, TokenKind.OpenParen);
                }
                else {
                    ContinueWith(result, TokenKind.OpenParen);
                }

                if (!Match(TokenKind.CloseParen)) {
                    do {
                        var parameter = new Parameter();
                        result.Add(parameter);

                        if (MatchIdentifier(true) && LookAhead(1, TokenKind.Assignment)) {
                            parameter.ParameterName = RequireIdentifier(parameter, true);
                            ContinueWithOrMissing(parameter, TokenKind.Assignment);
                        }

                        if (!Match(TokenKind.Comma))
                            parameter.Expression = ParseFormattedExpression(parameter);

                    } while (ContinueWith(result, TokenKind.Comma));
                }
                ContinueWithOrMissing(result, TokenKind.CloseParen);
                result.ParameterList = true;
                return result;
            }

            return result;
        }

        #endregion
        #region ParseFormattedExpression

        [Rule("FormattedExpression", "Expression [ ':' Expression [ ':' Expression ] ]")]
        private FormattedExpression ParseFormattedExpression(IExtendableSyntaxPart parent) {
            var result = new FormattedExpression();
            parent.Add(result);
            result.Expression = ParseExpression(result);

            if (ContinueWith(result, TokenKind.Colon)) {
                result.Width = ParseExpression(result);
                if (ContinueWith(result, TokenKind.Colon)) {
                    result.Decimals = ParseExpression(result);
                }
            }
            return result;
        }

        #endregion
        #region ParseSetSection

        [Rule("SetSection", "'[' [ Expression ] { (',' | '..') Expression } ']'")]
        private SetSection ParseSetSection(IExtendableSyntaxPart parent) {
            var result = new SetSection();
            InitByTerminal(result, parent, TokenKind.OpenBraces);
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
                    part.SetExpression = ParseExpression(part);
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
            var result = new ClosureExpressionSymbol();
            result.ProcSymbol = ContinueWithOrMissing(TokenKind.Function, TokenKind.Procedure);

            if (Match(TokenKind.OpenParen)) {
                result.Parameters = ParseFormalParameterSection(result);
            }
            else {
                result.Parameters = EmptyTerminal();
            }

            if (result.Kind == TokenKind.Function) {
                result.ColonSymbol = ContinueWithOrMissing(TokenKind.Colon);
                result.ReturnType = ParseTypeSpecification(result);
            }
            else {
                result.ColonSymbol = EmptyTerminal();
                result.ReturnType = EmptyTerminal();
            }

            result.Block = ParseBlock();
            return result;
        }

        #endregion

        #region Helper Functions

        private StandardInteger RequireInteger(IExtendableSyntaxPart parent) {
            return new StandardInteger(ContinueWithOrMissing(TokenKind.Integer));
        }

        private HexNumber RequireHexValue(IExtendableSyntaxPart parent) {
            var result = new HexNumber();
            InitByTerminal(result, parent, TokenKind.HexNumber);
            return result;
        }

        private RealNumber RequireRealValue(IExtendableSyntaxPart parent) {
            var result = new RealNumber();
            InitByTerminal(result, parent, TokenKind.Real);
            return result;
        }

        private Identifier RequireIdentifier(IExtendableSyntaxPart parent, bool allowReserverdWords = false) {

            //if (CurrentToken().Value == "SQLITE_OPEN_SHAREDCACHE")
            //    System.Diagnostics.Debugger.Break();

            if (Match(TokenKind.Identifier)) {
                var result = new Identifier();
                InitByTerminal(result, parent, TokenKind.Identifier);
                return result;
            };

            var token = CurrentToken();

            if (allowReserverdWords || !reservedWords.Contains(token.Kind)) {
                var result = new Identifier();
                InitByTerminal(result, parent, token.Kind);
                return result;
            }

            ContinueWithOrMissing(parent, TokenKind.Identifier);
            return null;
        }

        private QuotedString RequireString(IExtendableSyntaxPart parent) {
            var result = new QuotedString();
            InitByTerminal(result, parent, TokenKind.QuotedString);
            result.UnquotedValue = result.LastTerminalToken.ParsedValue;
            return result;
        }

        private QuotedString RequireDoubleQuotedString(IExtendableSyntaxPart parent) {
            var result = new QuotedString();
            InitByTerminal(result, parent, TokenKind.DoubleQuotedString);
            result.UnquotedValue = result.LastTerminalValue;
            return result;
        }

        private bool CurrentTokenIsAfterNewline() =>
            /*
        foreach (Token invalidToken in CurrentToken().InvalidTokensBefore) {
        if (invalidToken.Kind == TokenKind.WhiteSpace && PatternContinuation.ContainsNewLineChar(invalidToken.Value))
        return true;
        }
        */
            false;

        private NamespaceName ParseNamespaceName(IExtendableSyntaxPart parent, bool allowIn = false, bool inDesignator = false) {
            var result = new NamespaceName();

            if (parent != null)
                parent.Add(result);

            if (!ContinueWith(result, TokenKind.AnsiString, TokenKind.String, TokenKind.WideString, TokenKind.ShortString, TokenKind.UnicodeString))
                if (!allowIn || !ContinueWith(result, TokenKind.In))
                    RequireIdentifier(result);

            while (LookAheadIdentifier(1, Array.Empty<int>(), true) && (!inDesignator || LookAhead(2, new int[] { TokenKind.Dot })) && ContinueWith(result, TokenKind.Dot)) {
                RequireIdentifier(result, true);
            }

            return result;
        }


        private PoolItem<List<T>> GetList<T>() where T : class
            => Environment.ListPools.GetList<T>();

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