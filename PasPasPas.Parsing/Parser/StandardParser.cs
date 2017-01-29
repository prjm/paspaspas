using System.Text;
using System.Collections.Generic;
using PasPasPas.Parsing.Tokenizer;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.SyntaxTree.Standard;
using System;

namespace PasPasPas.Parsing.Parser {

    /// <summary>
    ///     standard, recursive descend paser
    /// </summary>
    public class StandardParser : ParserBase, IParser {

        /// <summary>
        ///     creates a new standard parser
        /// </summary>
        public StandardParser(ParserServices environment) :
            base(environment, new StandardTokenizerWithLookahead(environment)) { }

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
        #region Asm symbols

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

        private HashSet<string> asmDirectives =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
                "db", "dw", "dd", "dq" };

        private HashSet<string> asmPtr =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
                "byte", "word", "dword", "qword", "tbyte" };

        #endregion
        #region Parse

        /// <summary>
        ///     parse input
        /// </summary>
        public override ISyntaxPart Parse()
            => ParseFile(null);

        #endregion
        #region ParseFile

        /// <summary>
        ///     parse a standard file
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [Rule("File", "Program | Library | Unit | Package")]
        public ISyntaxPart ParseFile(IExtendableSyntaxPart parent) {
            if (Match(TokenKind.Library)) {
                return ParseLibrary(parent);
            }
            else if (Match(TokenKind.Unit)) {
                return ParseUnit(parent);
            }
            else if (Match(TokenKind.Package)) {
                return ParsePackage(parent);
            }

            return ParseProgram(parent);
        }

        #endregion
        #region ParseUnit

        [Rule("Unit", "UnitHead UnitInterface UnitImplementation UnitBlock '.' ")]
        private Unit ParseUnit(IExtendableSyntaxPart parent) {
            Unit result = CreateChild<Unit>(parent);
            result.UnitHead = ParseUnitHead(result);
            result.UnitInterface = ParseUnitInterface(result);
            result.UnitImplementation = ParseUnitImplementation(result);
            result.UnitBlock = ParseUnitBlock(result);
            ContinueWithOrMissing(result, TokenKind.Dot);
            return result;
        }

        #endregion
        #region ParseUnitInterface

        [Rule("UnitInterface", "'interface' [ UsesClause ] InterfaceDeclaration ")]
        private UnitInterface ParseUnitInterface(IExtendableSyntaxPart parent) {
            UnitInterface result = CreateByTerminal<UnitInterface>(parent, TokenKind.Interface);

            if (Match(TokenKind.Uses)) {
                result.UsesClause = ParseUsesClause(result);
            }

            result.InterfaceDeclaration = ParseInterfaceDeclaration(result);
            return result;
        }

        #endregion
        #region ParseUnitImplementation

        [Rule("UnitImplementation", "'implementation' [ UsesClause ] DeclarationSections", true)]
        private UnitImplementation ParseUnitImplementation(IExtendableSyntaxPart parent) {
            UnitImplementation result = CreateByTerminal<UnitImplementation>(parent, TokenKind.Implementation);

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
            UsesClause result = CreateByTerminal<UsesClause>(parent, TokenKind.Uses);
            result.UsesList = ParseNamespaceNameList(result);
            return result;
        }

        #endregion
        #region ParseUsesFileClause

        [Rule("UsesFileClause", "'uses' NamespaceFileNameList")]
        private UsesFileClause ParseUsesFileClause(IExtendableSyntaxPart parent) {
            UsesFileClause result = CreateByTerminal<UsesFileClause>(parent, TokenKind.Uses);
            result.Files = ParseNamespaceFileNameList(result);
            return result;
        }

        #endregion
        #region ParseNamespaceFileNameList

        [Rule("NamespaceFileNameList", "NamespaceFileName { ',' NamespaceFileName }")]
        private NamespaceFileNameList ParseNamespaceFileNameList(IExtendableSyntaxPart parent) {
            NamespaceFileNameList result = CreateChild<NamespaceFileNameList>(parent);

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
            NamespaceFileName result = CreateChild<NamespaceFileName>(parent);

            result.NamespaceName = ParseNamespaceName(result);
            if (ContinueWith(result, TokenKind.In))
                result.QuotedFileName = RequireString(result);

            return result;
        }

        #endregion
        #region ParseInterfaceDeclaration

        [Rule("InterfaceDeclaration", "{ InterfaceDeclarationItem }")]
        private InterfaceDeclaration ParseInterfaceDeclaration(IExtendableSyntaxPart parent) {
            InterfaceDeclaration result = CreateChild<InterfaceDeclaration>(parent);
            SyntaxPartBase item;

            do {
                item = ParseInterfaceDeclarationItem(result);
            } while (item != null);

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
                return ParseAssemblyAttribute(parent);
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
            ConstSection result = CreateByTerminal<ConstSection>(parent, TokenKind.Const, TokenKind.Resourcestring);
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
            ExportedProcedureHeading result = CreateByTerminal<ExportedProcedureHeading>(parent, TokenKind.Function, TokenKind.Procedure);
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
            FunctionDirectives result = CreateChild<FunctionDirectives>(parent);

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
            if (Match(TokenKind.Overload)) {
                return ParseOverloadDirective(parent);
            }

            if (Match(TokenKind.Inline, TokenKind.Assembler)) {
                return ParseInlineDirective(parent);
            }

            if (Match(TokenKind.Cdecl, TokenKind.Pascal, TokenKind.Register, TokenKind.Safecall, TokenKind.Stdcall, TokenKind.Export)) {
                return ParseCallConvention(parent);
            }

            if (Match(TokenKind.Far, TokenKind.Local, TokenKind.Near)) {
                return ParseOldCallConvention(parent);
            }

            if (Match(TokenKind.Deprecated, TokenKind.Library, TokenKind.Experimental, TokenKind.Platform)) {
                HintingInformation result = ParseHint(parent);
                ContinueWithOrMissing(result, TokenKind.Semicolon);
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
            ForwardDirective result = CreateByTerminal<ForwardDirective>(parent, TokenKind.Forward);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseUnsafeDirective

        [Rule("UnsafeDirective", "'unsafe' ';' ")]
        private UnsafeDirective ParseUnsafeDirective(IExtendableSyntaxPart parent) {
            UnsafeDirective result = CreateByTerminal<UnsafeDirective>(parent, TokenKind.Unsafe);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseExternalDirective

        [Rule("ExternalDirective", "(varargs | external [ ConstExpression { ExternalSpecifier } ]) ';' ")]
        private ExternalDirective ParseExternalDirective(IExtendableSyntaxPart parent) {
            ExternalDirective result = CreateByTerminal<ExternalDirective>(parent, TokenKind.VarArgs, TokenKind.External);
            result.Kind = result.LastTerminalKind;

            if ((result.Kind == TokenKind.External) && (!Match(TokenKind.Semicolon))) {
                result.ExternalExpression = ParseConstantExpression(result);
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

            ExternalSpecifier result = CreateByTerminal<ExternalSpecifier>(parent, TokenKind.Name, TokenKind.Index, TokenKind.Dependency, TokenKind.Delayed);
            result.Kind = result.LastTerminalKind;

            if (result.Kind == TokenKind.Delayed)
                return result;

            if (result.Kind != TokenKind.Dependency) {
                result.Expression = ParseConstantExpression(result);
            }
            else {
                do {
                    ParseConstantExpression(result);
                } while (ContinueWith(result, TokenKind.Comma));
            }

            return result;
        }

        #endregion
        #region ParseOldCallConvention

        [Rule("OrdCallConvention", "'Near' | 'Far' | 'Local'")]
        private SyntaxPartBase ParseOldCallConvention(IExtendableSyntaxPart parent) {
            OldCallConvention result = CreateByTerminal<OldCallConvention>(parent, TokenKind.Near, TokenKind.Far, TokenKind.Local);
            result.Kind = result.LastTerminalKind;
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseUnitBlock

        [Rule("UnitBlock", "( UnitInitilization 'end' ) | CompoundStatement | 'end' ")]
        private UnitBlock ParseUnitBlock(IExtendableSyntaxPart parent) {
            UnitBlock result = CreateChild<UnitBlock>(parent);

            if (ContinueWith(result, TokenKind.End))
                return result;

            if (Match(TokenKind.Begin, TokenKind.Asm)) {
                result.MainBlock = ParseCompoundStatement(result);
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
            UnitInitialization result = CreateByTerminal<UnitInitialization>(parent, TokenKind.Initialization);

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
            UnitFinalization result = CreateByTerminal<UnitFinalization>(parent, TokenKind.Finalization);
            result.Statements = ParseStatementList(result);
            return result;
        }

        #endregion
        #region ParseCompoundStatement

        [Rule("CompoundStatement", "(('begin' [ StatementList ] 'end' ) | AsmBlock )")]
        private CompoundStatement ParseCompoundStatement(IExtendableSyntaxPart parent) {

            if (Match(TokenKind.Asm)) {
                CompoundStatement result = CreateChild<CompoundStatement>(parent);
                result.AssemblerBlock = ParseAsmBlock(result);
                return result;
            }
            else {
                CompoundStatement result = CreateByTerminal<CompoundStatement>(parent, TokenKind.Begin);

                if (!Match(TokenKind.End))
                    result.Statements = ParseStatementList(result);

                ContinueWithOrMissing(result, TokenKind.End);
                return result;
            }
        }

        #endregion
        #region StatementList

        [Rule("StatementList", "[Statement], { ';' [Statement]}")]
        private StatementList ParseStatementList(IExtendableSyntaxPart parent) {
            StatementList result = CreateChild<StatementList>(parent);

            do {
                ParseStatement(result);
            } while (ContinueWith(result, TokenKind.Semicolon));

            return result;
        }

        #endregion
        #region ParseStatement
        [Rule("Statement", "[ Label ':' ] StatementPart")]
        private Statement ParseStatement(IExtendableSyntaxPart parent) {
            Statement result = CreateChild<Statement>(parent);

            Label label = null;
            if (MatchIdentifier(TokenKind.Integer, TokenKind.HexNumber) && LookAhead(1, TokenKind.Colon)) {
                label = ParseLabel(result);
                ContinueWithOrMissing(result, TokenKind.Colon);
            }

            StatementPart part = ParseStatementPart(result);

            if (label != null && part == null) {
                Unexpected();
                return null;
            }


            result.Label = label;
            result.Part = part;
            return result;
        }
        #endregion

        [Rule("StatementPart", "IfStatement | CaseStatement | ReapeatStatement | WhileStatment | ForStatement | WithStatement | TryStatement | RaiseStatement | AsmStatement | CompoundStatement | SimpleStatement ")]
        private StatementPart ParseStatementPart(IExtendableSyntaxPart parent) {
            StatementPart result = CreateChild<StatementPart>(parent);

            if (Match(TokenKind.If)) {
                result.If = ParseIfStatement(result);
                return result;
            }
            if (Match(TokenKind.Case)) {
                result.Case = ParseCaseStatement(result);
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
                result.CompoundStatement = ParseCompoundStatement(result);
                return result;
            }

            return ParseSimpleStatement(parent);
        }

        [Rule("RaiseStatement", "'raise' [ Expression ] [ 'at' Expression ]")]
        private RaiseStatement ParseRaiseStatement(IExtendableSyntaxPart parent) {
            RaiseStatement result = CreateByTerminal<RaiseStatement>(parent, TokenKind.Raise);

            if ((!Match(TokenKind.AtWord)) && MatchIdentifier(TokenKind.Inherited)) {
                result.Raise = ParseExpression(result);
            }

            if (ContinueWith(result, TokenKind.AtWord)) {
                result.At = ParseExpression(result);
            }

            return result;
        }

        [Rule("TryStatement", "'try' StatementList  ('except' HandlerList | 'finally' StatementList) 'end'")]
        private TryStatement ParseTryStatement(IExtendableSyntaxPart parent) {
            TryStatement result = CreateByTerminal<TryStatement>(parent, TokenKind.Try);

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

        [Rule("ExceptHandlers", "({ Handler } [ 'else' StatementList ]) | StatementList")]
        private ExceptHandlers ParseExceptHandlers(IExtendableSyntaxPart parent) {
            ExceptHandlers result = CreateChild<ExceptHandlers>(parent);

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

        [Rule("ExceptHandler", "'on' Identifier ':' NamespaceName 'do' Statement ';'")]
        private ExceptHandler ParseExceptHandler(IExtendableSyntaxPart parent) {
            ExceptHandler result = CreateByTerminal<ExceptHandler>(parent, TokenKind.On);
            result.Name = RequireIdentifier(result);
            ContinueWithOrMissing(result, TokenKind.Colon);
            result.HandlerType = ParseNamespaceName(result);
            ContinueWithOrMissing(result, TokenKind.Do);
            result.Statement = ParseStatement(result);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        [Rule("WithStatement", "'with' Expression { ',' Expression }  'do' Statement")]
        private WithStatement ParseWithStatement(IExtendableSyntaxPart parent) {
            WithStatement result = CreateByTerminal<WithStatement>(parent, TokenKind.With);


            do {
                ParseExpression(result);
            } while (ContinueWith(result, TokenKind.Comma));

            ContinueWithOrMissing(result, TokenKind.Do);

            result.Statement = ParseStatement(result);
            return result;
        }

        [Rule("ForStatement", "('for' Designator ':=' Expression ('to' | 'downto' )  Expression 'do' Statement) | ('for' Designator 'in' Expression  'do' Statement)")]
        private ForStatement ParseForStatement(IExtendableSyntaxPart parent) {
            ForStatement result = CreateByTerminal<ForStatement>(parent, TokenKind.For);

            result.Variable = ParseDesignator(result);
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
            result.Statement = ParseStatement(result);
            return result;
        }

        [Rule("WhileStatement", "'while' Expression 'do' Statement")]
        private WhileStatement ParseWhileStatement(IExtendableSyntaxPart parent) {
            WhileStatement result = CreateByTerminal<WhileStatement>(parent, TokenKind.While);
            ContinueWithOrMissing(result, TokenKind.While);
            result.Condition = ParseExpression(result);
            ContinueWithOrMissing(result, TokenKind.Do);
            result.Statement = ParseStatement(result);
            return result;
        }

        [Rule("RepeatStatement", "'repeat' [ StatementList ] 'until' Expression")]
        private RepeatStatement ParseRepeatStatement(IExtendableSyntaxPart parent) {
            RepeatStatement result = CreateByTerminal<RepeatStatement>(parent, TokenKind.Repeat);

            if (!Match(TokenKind.Until)) {
                result.Statements = ParseStatementList(result);
            }
            ContinueWithOrMissing(result, TokenKind.Until);
            result.Condition = ParseExpression(result);
            return result;
        }

        [Rule("CaseStatement", "'case' Expression 'of' { CaseItem } ['else' StatementList[';']] 'end' ")]
        private CaseStatement ParseCaseStatement(IExtendableSyntaxPart parent) {
            CaseStatement result = CreateByTerminal<CaseStatement>(parent, TokenKind.Case);
            result.CaseExpression = ParseExpression(result);
            ContinueWithOrMissing(result, TokenKind.Of);

            CaseItem item = null;
            do {
                item = ParseCaseItem(result);
            } while (Tokenizer.HasNextToken() && item != null);

            if (ContinueWith(result, TokenKind.Else)) {
                result.Else = ParseStatementList(result);
                ContinueWith(result, TokenKind.Semicolon);
            }
            ContinueWithOrMissing(result, TokenKind.End);
            return result;
        }

        [Rule("CaseItem", "CaseLabel { ',' CaseLabel } ':' Statement [';']")]
        private CaseItem ParseCaseItem(IExtendableSyntaxPart parent) {
            if (Match(TokenKind.Else, TokenKind.End))
                return null;

            if (!HasTokenBeforeToken(TokenKind.Colon, TokenKind.Semicolon, TokenKind.End, TokenKind.Begin))
                return null;

            CaseItem result = CreateChild<CaseItem>(parent);

            do {
                ParseCaseLabel(result);
            } while (ContinueWith(result, TokenKind.Comma));

            ContinueWithOrMissing(result, TokenKind.Colon);
            result.CaseStatement = ParseStatement(result);
            ContinueWith(result, TokenKind.Semicolon);
            return result;
        }

        [Rule("CaseLabel", "Expression [ '..' Expression ]")]
        private CaseLabel ParseCaseLabel(IExtendableSyntaxPart parent) {
            CaseLabel result = CreateChild<CaseLabel>(parent);
            result.StartExpression = ParseExpression(result);
            if (ContinueWith(result, TokenKind.DotDot))
                result.EndExpression = ParseExpression(result);
            return result;
        }

        [Rule("IfStatement", "'if' Expression 'then' Statement [ 'else' Statement ]")]
        private IfStatement ParseIfStatement(IExtendableSyntaxPart parent) {
            IfStatement result = CreateByTerminal<IfStatement>(parent, TokenKind.If);

            result.Condition = ParseExpression(result);
            ContinueWithOrMissing(result, TokenKind.Then);
            result.ThenPart = ParseStatement(result);
            if (ContinueWith(result, TokenKind.Else)) {
                result.ElsePart = ParseStatement(result);
            }

            return result;
        }

        [Rule("SimpleStatement", "GoToStatement | Designator [ ':=' (Expression  | NewStatement) ] ")]
        private StatementPart ParseSimpleStatement(IExtendableSyntaxPart parent) {
            if (!(LookAhead(1, TokenKind.Assignment, TokenKind.OpenBraces, TokenKind.OpenParen)) && Match(TokenKind.GoToKeyword, TokenKind.Exit, TokenKind.Break, TokenKind.Continue)) {
                StatementPart result = CreateChild<StatementPart>(parent);
                result.GoTo = ParseGoToStatement(result);
                return result;
            }

            if (MatchIdentifier(TokenKind.Inherited, TokenKind.Circumflex, TokenKind.OpenParen, TokenKind.At, TokenKind.AnsiString, TokenKind.UnicodeString, TokenKind.String, TokenKind.WideString, TokenKind.ShortString)) {
                StatementPart result = CreateChild<StatementPart>(parent);
                result.DesignatorPart = ParseDesignator(result);

                if (ContinueWith(result, TokenKind.Assignment)) {
                    result.Assignment = ParseExpression(result);
                }

                return result;
            }

            return null;
        }

        [Rule("GoToStatement", "('goto' Label) | 'break' | 'continue' | 'exit' '(' Expression ')' ")]
        private GoToStatement ParseGoToStatement(IExtendableSyntaxPart parent) {
            GoToStatement result = CreateChild<GoToStatement>(parent);

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

        [Rule("UnitHead", "'unit' NamespaceName { Hint } ';' ")]
        private UnitHead ParseUnitHead(IExtendableSyntaxPart parent) {
            UnitHead result = CreateByTerminal<UnitHead>(parent, TokenKind.Unit);
            result.UnitName = ParseNamespaceName(result);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            result.Hint = ParseHints(result);
            return result;
        }

        #region ParsePackage

        [Rule("Package", "PackageHead RequiresClause [ ContainsClause ] 'end' '.' ")]
        private Package ParsePackage(IExtendableSyntaxPart parent) {
            Package result = CreateChild<Package>(parent);
            result.PackageHead = ParsePackageHead(result);
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
            PackageContains result = CreateByTerminal<PackageContains>(parent, TokenKind.Contains);
            result.ContainsList = ParseNamespaceFileNameList(result);
            return result;
        }

        #endregion
        #region ParseRequiresClause

        [Rule("RequiresClause", "'requires' NamespaceNameList")]
        private PackageRequires ParseRequiresClause(IExtendableSyntaxPart parent) {
            PackageRequires result = CreateByTerminal<PackageRequires>(parent, TokenKind.Requires);
            result.RequiresList = ParseNamespaceNameList(result);
            return result;
        }

        #endregion

        [Rule("NamespaceNameList", "NamespaceName { ',' NamespaceName } ';' ")]
        private NamespaceNameList ParseNamespaceNameList(IExtendableSyntaxPart parent) {
            NamespaceNameList result = CreateChild<NamespaceNameList>(parent);

            do {
                ParseNamespaceName(result);
            } while (ContinueWith(result, TokenKind.Comma));

            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        [Rule("PackageHead", "'package' NamespaceName ';' ")]
        private PackageHead ParsePackageHead(IExtendableSyntaxPart parent) {
            PackageHead result = CreateByTerminal<PackageHead>(parent, TokenKind.Package);
            result.PackageName = ParseNamespaceName(result);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        [Rule("Library", "LibraryHead [UsesFileClause] Block '.' ")]
        private Library ParseLibrary(IExtendableSyntaxPart parent) {
            Library result = CreateChild<Library>(parent);
            result.LibraryHead = ParseLibraryHead(result);

            if (Match(TokenKind.Uses))
                result.Uses = ParseUsesFileClause(result);

            result.MainBlock = ParseBlock(result);
            ContinueWithOrMissing(result, TokenKind.Dot);
            return result;
        }

        [Rule("LibraryHead", "'library' NamespaceName Hints ';'")]
        private LibraryHead ParseLibraryHead(IExtendableSyntaxPart parent) {
            LibraryHead result = CreateByTerminal<LibraryHead>(parent, TokenKind.Library);
            result.LibraryName = ParseNamespaceName(result);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            result.Hints = ParseHints(result);
            return result;
        }

        [Rule("Program", "[ProgramHead] [UsesFileClause] Block '.'")]
        private Program ParseProgram(IExtendableSyntaxPart parent) {
            Program result = CreateChild<Program>(parent);

            if (Match(TokenKind.Program))
                result.ProgramHead = ParseProgramHead(result);

            if (Match(TokenKind.Uses))
                result.Uses = ParseUsesFileClause(result);

            result.MainBlock = ParseBlock(result);
            ContinueWithOrMissing(result, TokenKind.Dot);
            return result;
        }

        [Rule("ProgramHead", "'program' NamespaceName [ProgramParams] ';'")]
        private ProgramHead ParseProgramHead(IExtendableSyntaxPart parent) {
            ProgramHead result = CreateByTerminal<ProgramHead>(parent, TokenKind.Program);
            result.Name = ParseNamespaceName(result);
            result.Parameters = ParseProgramParams(result);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        [Rule("ProgramParams", "'(' [ Identifier { ',' Identifier } ] ')'")]
        private ProgramParameterList ParseProgramParams(IExtendableSyntaxPart parent) {
            ProgramParameterList result = CreateChild<ProgramParameterList>(parent);

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

        [Rule("Block", "DeclarationSections [ BlockBody ] ")]
        private Block ParseBlock(IExtendableSyntaxPart parent) {
            Block result = CreateChild<Block>(parent);
            result.DeclarationSections = ParseDeclarationSections(result);
            if (Match(TokenKind.Asm, TokenKind.Begin)) {
                result.Body = ParseBlockBody(result);
            }
            return result;
        }

        [Rule("BlockBody", "AssemblerBlock | CompoundStatement")]
        private BlockBody ParseBlockBody(IExtendableSyntaxPart parent) {
            BlockBody result = CreateChild<BlockBody>(parent);

            if (Match(TokenKind.Asm)) {
                result.AssemblerBlock = ParseAsmBlock(result);
            }

            if (Match(TokenKind.Begin)) {
                result.Body = ParseCompoundStatement(result);
            }

            return result;
        }

        [Rule("AsmBlock", "'asm' { AssemblyStatement | PseudoOp } 'end'")]
        private AsmBlock ParseAsmBlock(IExtendableSyntaxPart parent) {
            AsmBlock result = CreateByTerminal<AsmBlock>(parent, TokenKind.Asm);

            while (Tokenizer.HasNextToken() && (!ContinueWith(result, TokenKind.End))) {
                if (Match(TokenKind.Dot)) {
                    ParseAsmPseudoOp(result);
                }
                else {
                    ParseAsmStatement(result);
                }
            }

            return result;
        }

        [Rule("PseudoOp", "( '.PARAMS ' Integer | '.PUSHNV' Register | '.SAVNENV' Register | '.NOFRAME'.")]
        private AsmPseudoOp ParseAsmPseudoOp(AsmBlock parent) {
            AsmPseudoOp result = CreateByTerminal<AsmPseudoOp>(parent, TokenKind.Dot);
            var kind = CurrentToken().Value;
            result.Kind = RequireIdentifier(result);

            if (string.Equals(kind, "params", StringComparison.OrdinalIgnoreCase)) {
                result.ParamsOperation = true;
                result.NumberOfParams = RequireInteger(parent);
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

        [Rule("AssemblyStatement", "[AssemblyLabel ':'] [AssemblyPrefix] AssemblyOpcode [AssemblyOperand {','  AssemblyOperand}]")]
        private AsmStatement ParseAsmStatement(IExtendableSyntaxPart parent) {
            AsmStatement result = CreateChild<AsmStatement>(parent);

            if (Match(TokenKind.At) || LookAhead(1, TokenKind.Colon)) {
                ParseAssemblyLabel(result);
                ContinueWithOrMissing(result, TokenKind.Colon);
            }

            if (Match(TokenKind.End))
                return result;

            result.Prefix = ParseAssemblyPrefix(result);
            result.OpCode = ParseAssemblyOpcode(result);

            while (Tokenizer.HasNextToken() && !Match(TokenKind.End) && !Match(TokenKind.Semicolon) && Tokenizer.HasNextToken() && !CurrentTokenIsAfterNewline()) {
                ParseAssemblyOperand(result);
                ContinueWith(result, TokenKind.Comma);
            }

            return result;
        }


        [Rule("AssemblyOperand", " AssemblyExpression ('and' | 'or' | 'xor') | ( 'not' AssemblyExpression ']' )")]
        private AsmOperand ParseAssemblyOperand(IExtendableSyntaxPart parent) {
            AsmOperand result = CreateChild<AsmOperand>(parent);

            if (ContinueWith(result, TokenKind.Not)) {
                result.NotExpression = ParseAssemblyOperand(result);
                return result;
            }

            result.LeftTerm = ParseAssemblyExpression(result);

            if (ContinueWith(result, TokenKind.And, TokenKind.Or, TokenKind.Xor)) {
                result.RightTerm = ParseAssemblyOperand(result);
            }

            return result;
        }

        [Rule("AssemblyExpression", " ('OFFSET' AssemblyOperand ) | ('TYPE' AssemblyOperand) | (('BYTE' | 'WORD' | 'DWORD' | 'QWORD' | 'TBYTE' ) PTR AssemblyOperand) | AssemblyTerm ('+' | '-' ) AssemblyOperand ")]
        private AsmExpression ParseAssemblyExpression(AsmOperand parent) {
            AsmExpression result = CreateChild<AsmExpression>(parent);
            var tokenValue = CurrentToken().Value;

            if (MatchIdentifier()) {

                if (string.Equals(tokenValue, "OFFSET", StringComparison.OrdinalIgnoreCase)) {
                    ContinueWith(result, TokenKind.Identifier);
                    result.Offset = ParseAssemblyOperand(result);
                    return result;
                }

                if (asmPtr.Contains(tokenValue)) {
                    ContinueWith(result, TokenKind.Identifier);
                    result.BytePtr = ParseAssemblyOperand(result);
                    return result;
                }

            }

            if (ContinueWith(result, TokenKind.TypeKeyword)) {
                result.TypeExpression = ParseAssemblyOperand(result);
                return result;
            }

            result.LeftOperand = ParseAssemblyTerm(result);

            if (ContinueWith(result, TokenKind.Plus, TokenKind.Minus)) {
                result.RightOperand = ParseAssemblyOperand(result);
            }

            return result;
        }

        [Rule("AssemblyTerm", "AssemblyFactor [( '*' | '/' | 'mod' | 'shl' | 'shr' ) AssemblyOperand ]")]
        private AsmTerm ParseAssemblyTerm(IExtendableSyntaxPart parent) {
            AsmTerm result = CreateChild<AsmTerm>(parent);

            result.LeftOperand = ParseAssemblyFactor(result);

            if (ContinueWith(result, TokenKind.Dot)) {
                result.Subtype = ParseAssemblyOperand(result);
            }

            if (ContinueWith(result, TokenKind.Times, TokenKind.Slash, TokenKind.Mod, TokenKind.Shl, TokenKind.Shr)) {
                result.RightOperand = ParseAssemblyOperand(result);
            }


            return result;
        }

        [Rule("AssemblyFactor", "(SegmentPrefix ':' AssemblyOperand) | '(' AssemblyOperand ')' | '[' AssemblyOperand ']' | Identifier | QuotedString | DoubleQuotedString | Integer | HexNumber ")]
        private AsmFactor ParseAssemblyFactor(AsmTerm parent) {
            AsmFactor result = CreateChild<AsmFactor>(parent);

            if (MatchIdentifier() && segmentPrefixes.Contains(CurrentToken().Value) && LookAhead(1, TokenKind.Colon)) {
                result.SegmentPrefix = RequireIdentifier(result);
                ContinueWithOrMissing(result, TokenKind.Colon);
                result.SegmentExpression = ParseAssemblyOperand(result);
                return result;
            }

            if (ContinueWith(result, TokenKind.OpenParen)) {
                result.Subexpression = ParseAssemblyOperand(result);
                ContinueWithOrMissing(result, TokenKind.CloseParen);
                return result;
            }

            if (ContinueWith(result, TokenKind.OpenBraces)) {
                result.MemorySubexpression = ParseAssemblyOperand(result);
                ContinueWithOrMissing(result, TokenKind.CloseBraces);
                return result;
            }

            if (MatchIdentifier(true)) {
                result.Identifier = RequireIdentifier(result, true);
            }
            else if (Match(TokenKind.Integer)) {
                result.Number = RequireInteger(result);
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
                ParseLocalAsmLabel(result);
            }
            else {
                Unexpected();
            }
            return result;
        }

        private bool CurrentTokenIsAfterNewline() {
            foreach (Token invalidToken in CurrentToken().InvalidTokensBefore) {
                if (invalidToken.Kind == TokenKind.WhiteSpace && LineCounter.ContainsNewLineChar(invalidToken.Value))
                    return true;
            }

            return false;
        }

        [Rule("AssemblyOpCode", "Identifier [AssemblyDirective] ")]
        private AsmOpCode ParseAssemblyOpcode(IExtendableSyntaxPart parent) {
            if (Match(TokenKind.End))
                return null;

            AsmOpCode result = CreateChild<AsmOpCode>(parent);

            result.OpCode = RequireIdentifier(result, true);

            if (MatchIdentifier(true) && asmDirectives.Contains(CurrentToken().Value)) {
                result.Directive = RequireIdentifier(result, true);
            }

            return result;
        }

        [Rule("AssemblyPrefix", "(LockPrefix | [SegmentPrefix]) | (SegmentPrefix [LockPrefix])")]
        private AsmPrefix ParseAssemblyPrefix(IExtendableSyntaxPart parent) {

            if (!MatchIdentifier())
                return null;

            if (lockPrefixes.Contains(CurrentToken().Value)) {
                AsmPrefix result = CreateChild<AsmPrefix>(parent);
                result.LockPrefix = RequireIdentifier(result);

                if (MatchIdentifier() && segmentPrefixes.Contains(CurrentToken().Value)) {
                    result.SegmentPrefix = RequireIdentifier(result);
                }

                return result;
            }

            if (segmentPrefixes.Contains(CurrentToken().Value)) {
                AsmPrefix result = CreateChild<AsmPrefix>(parent);
                result.SegmentPrefix = RequireIdentifier(result);

                if (MatchIdentifier() && lockPrefixes.Contains(CurrentToken().Value)) {
                    result.LockPrefix = RequireIdentifier(result);
                }

                return result;
            }

            return null;
        }

        [Rule("AsmLabel", "(Label | LocalAsmLabel { LocalAsmLabel } )")]
        private AsmLabel ParseAssemblyLabel(AsmStatement parent) {
            AsmLabel result = CreateChild<AsmLabel>(parent);
            if (Match(TokenKind.At)) {
                ParseLocalAsmLabel(result);
            }
            else {
                ParseLabel(result);
            }
            return result;
        }

        [Rule("LocalAsmLabel", "'@' { '@' | Integer | Identifier | HexNumber }")]
        private void ParseLocalAsmLabel(IExtendableSyntaxPart parent) {
            LocalAsmLabel result = CreateChild<LocalAsmLabel>(parent);

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
        }


        [Rule("DeclarationSection", "{ LabelDeclarationSection | ConstSection | TypeSection | VarSection | ExportsSection | AssemblyAttribute | MethodDecl | ProcedureDeclaration }", true)]
        private Declarations ParseDeclarationSections(IExtendableSyntaxPart parent) {
            Declarations result = CreateChild<Declarations>(parent);
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

                UserAttributes attrs = null;
                if (Match(TokenKind.OpenBraces)) {
                    if (LookAhead(1, TokenKind.Assembly)) {
                        ParseAssemblyAttribute(result);
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
                        MethodDeclaration methodDecl = ParseMethodDecl(result);
                        methodDecl.Class = useClass;
                        methodDecl.Attributes = attrs;
                        continue;
                    }

                    ParseProcedureDeclaration(result, attrs);
                    continue;
                }


                stop = true;
            }

            return result;
        }

        [Rule("MethodDecl", "MethodDeclHeading ';' MethodDirectives [ Block ';' ]")]
        private MethodDeclaration ParseMethodDecl(IExtendableSyntaxPart parent) {
            MethodDeclaration result = CreateChild<MethodDeclaration>(parent);
            result.Heading = ParseMethodDeclHeading(result);
            ContinueWithOrMissing(result, TokenKind.Semicolon);

            result.Directives = ParseMethodDirectives(result);
            result.MethodBody = ParseBlock(result);

            if ((result.MethodBody != null) && (result.MethodBody.Body != null))
                ContinueWithOrMissing(result, TokenKind.Semicolon);

            return result;
        }

        #region ParseMethodDirectives

        [Rule("MethodDirectives", "{ MethodDirective }")]
        private MethodDirectives ParseMethodDirectives(IExtendableSyntaxPart parent) {
            MethodDirectives result = CreateChild<MethodDirectives>(parent);

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

            if (Match(TokenKind.Reintroduce)) {
                return ParseReintroduceDirective(parent);
            }

            if (Match(TokenKind.Overload)) {
                return ParseOverloadDirective(parent);
            }

            if (Match(TokenKind.Inline, TokenKind.Assembler)) {
                return ParseInlineDirective(parent);
            }

            if (Match(TokenKind.Message, TokenKind.Static, TokenKind.Dynamic, TokenKind.Override, TokenKind.Virtual)) {
                return ParseBindingDirective(parent);
            }

            if (Match(TokenKind.Abstract, TokenKind.Final)) {
                return ParseAbstractDirective(parent);
            }

            if (Match(TokenKind.Cdecl, TokenKind.Pascal, TokenKind.Register, TokenKind.Safecall, TokenKind.Stdcall, TokenKind.Export)) {
                return ParseCallConvention(parent);
            }

            if (Match(TokenKind.Deprecated, TokenKind.Library, TokenKind.Experimental, TokenKind.Platform)) {
                HintingInformation result = ParseHint(parent);
                ContinueWithOrMissing(result, TokenKind.Semicolon);
                return result;
            }

            if (Match(TokenKind.DispId)) {
                return ParseDispIdDirective(parent);
            }

            return null;
        }

        #endregion
        #region ParseInlineDirective

        [Rule("InlineDirective", "('inline' | 'assembler' ) ';'")]
        private SyntaxPartBase ParseInlineDirective(IExtendableSyntaxPart parent) {
            InlineDirective result = CreateByTerminal<InlineDirective>(parent, TokenKind.Inline, TokenKind.Assembler);
            result.Kind = result.LastTerminalKind;
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseCallConvention

        [Rule("CallConvention", "('cdecl' | 'pascal' | 'register' | 'safecall' | 'stdcall' | 'export') ';' ")]
        private SyntaxPartBase ParseCallConvention(IExtendableSyntaxPart parent) {
            CallConvention result = CreateByTerminal<CallConvention>(parent, TokenKind.Cdecl, TokenKind.Pascal, TokenKind.Register, TokenKind.Safecall, TokenKind.Stdcall, TokenKind.Export);
            result.Kind = result.LastTerminalKind;
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseAbstractDirective

        [Rule("AbstractDirective", "('abstract' | 'final' ) ';' ")]
        private SyntaxPartBase ParseAbstractDirective(IExtendableSyntaxPart parent) {
            AbstractDirective result = CreateByTerminal<AbstractDirective>(parent, TokenKind.Abstract, TokenKind.Final);
            result.Kind = result.LastTerminalKind;
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseBindingDirective

        [Rule("BindingDirective", " ('message' Expression ) | 'static' | 'dynamic' | 'override' | 'virtual' ")]
        private SyntaxPartBase ParseBindingDirective(IExtendableSyntaxPart parent) {
            BindingDirective result = CreateByTerminal<BindingDirective>(parent, TokenKind.Message, TokenKind.Static, TokenKind.Dynamic, TokenKind.Override, TokenKind.Virtual);
            result.Kind = result.LastTerminalKind;
            if (result.Kind == TokenKind.Message) {
                result.MessageExpression = ParseExpression(result);
            }
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseOverloadDirective

        [Rule("OverloadDirective", "'overload' ';' ")]
        private SyntaxPartBase ParseOverloadDirective(IExtendableSyntaxPart parent) {
            OverloadDirective result = CreateByTerminal<OverloadDirective>(parent, TokenKind.Overload);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseReintroduceDirective

        [Rule("ReintroduceDirective", "'reintroduce' ';' ")]
        private SyntaxPartBase ParseReintroduceDirective(IExtendableSyntaxPart parent) {
            ReintroduceDirective result = CreateByTerminal<ReintroduceDirective>(parent, TokenKind.Reintroduce);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion

        [Rule("MethodDeclHeading", " ('constructor' | 'destructor' | 'function' | 'procedure' | 'operator') NamespaceName [GenericDefinition] [FormalParameterSection] [':' Attributes TypeSpecification ]")]
        private MethodDeclarationHeading ParseMethodDeclHeading(IExtendableSyntaxPart parent) {
            MethodDeclarationHeading result = CreateByTerminal<MethodDeclarationHeading>(parent, TokenKind.Constructor, TokenKind.Destructor, TokenKind.Function, TokenKind.Procedure, TokenKind.Operator);
            result.Kind = result.LastTerminalKind;

            do {
                result.Name = ParseNamespaceName(result);

                if (Match(TokenKind.AngleBracketsOpen)) {
                    result.GenericDefinition = ParseGenericDefinition(result);
                }
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

        [Rule("ProcedureDeclaration", "ProcedureDeclarationHeading ';' FunctionDirectives [ ProcBody ]")]
        private ProcedureDeclaration ParseProcedureDeclaration(IExtendableSyntaxPart parent, UserAttributes attributes) {
            ProcedureDeclaration result = CreateChild<ProcedureDeclaration>(parent);
            result.Attributes = attributes;
            result.Heading = ParseProcedureDeclarationHeading(result);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            result.Directives = ParseFunctionDirectives(result);
            result.ProcedureBody = ParseBlock(result);
            if ((result.ProcedureBody != null) && (result.ProcedureBody.Body != null))
                ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        [Rule("ProcedureDeclarationHeading", "('procedure' | 'function') Identifier [FormalParameterSection][':' TypeSpecification]")]
        private ProcedureDeclarationHeading ParseProcedureDeclarationHeading(IExtendableSyntaxPart parent) {
            ProcedureDeclarationHeading result = CreateByTerminal<ProcedureDeclarationHeading>(parent, TokenKind.Function, TokenKind.Procedure);
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

        [Rule("AssemblyAttribute", "'[' 'assembly' ':' ']'")]
        private AssemblyAttributeDeclaration ParseAssemblyAttribute(IExtendableSyntaxPart parent) {
            AssemblyAttributeDeclaration result = CreateByTerminal<AssemblyAttributeDeclaration>(parent, TokenKind.OpenBraces);
            ContinueWithOrMissing(result, TokenKind.Assembly);
            ContinueWithOrMissing(result, TokenKind.Colon);
            result.Attribute = ParseAttribute(result);
            ContinueWithOrMissing(result, TokenKind.CloseBraces);
            return result;
        }

        [Rule("ExportsSection", "'exports' Identifier ExportItem { ',' ExportItem } ';' ")]
        private ExportsSection ParseExportsSection(IExtendableSyntaxPart parent) {
            ExportsSection result = CreateByTerminal<ExportsSection>(parent, TokenKind.Exports);
            result.ExportName = RequireIdentifier(result);

            do {
                ParseExportItem(result);
            } while (ContinueWith(result, TokenKind.Comma));

            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        [Rule("ExportItem", "[ '(' FormalParameters ')' ] [ 'index' Expression ] [ 'name' Expression ]")]
        private ExportItem ParseExportItem(IExtendableSyntaxPart parent) {
            ExportItem result = CreateChild<ExportItem>(parent);

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

        [Rule("VarSection", "(var | threadvar) VarDeclaration { VarDeclaration }")]
        private VarSection ParseVarSection(IExtendableSyntaxPart parent, bool inClassDeclaration) {
            VarSection result = CreateByTerminal<VarSection>(parent, TokenKind.Var, TokenKind.ThreadVar);
            result.Kind = result.LastTerminalKind;

            do {
                ParseVarDeclaration(result);
            } while ((!inClassDeclaration || !Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Automated, TokenKind.Strict)) && MatchIdentifier(TokenKind.OpenBraces));

            return result;
        }

        #region ParseVarDeclaration

        [Rule("VarDeclaration", " IdentList ':' TypeSpecification [ VarValueSpecification ] Hints ';' ")]
        private VarDeclaration ParseVarDeclaration(IExtendableSyntaxPart parent) {
            VarDeclaration result = CreateChild<VarDeclaration>(parent);

            result.Attributes = ParseAttributes(result);
            result.Identifiers = ParseIdentList(result, false);
            ContinueWithOrMissing(result, TokenKind.Colon);
            result.TypeDeclaration = ParseTypeSpecification(result);

            if (Match(TokenKind.Absolute, TokenKind.EqualsSign)) {
                result.ValueSpecification = ParseValueSpecification(result);
            }

            result.Hints = ParseHints(result);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion

        [Rule("VarValueSpecification", "('absolute' ConstExpression) | ('=' ConstExpression)")]
        private VarValueSpecification ParseValueSpecification(IExtendableSyntaxPart parent) {
            VarValueSpecification result = CreateChild<VarValueSpecification>(parent);

            if (ContinueWith(result, TokenKind.Absolute)) {
                result.Absolute = ParseConstantExpression(result);
                return result;
            }

            ContinueWithOrMissing(result, TokenKind.EqualsSign);
            result.InitialValue = ParseConstantExpression(result);
            return result;
        }

        [Rule("LabelSection", "'label' Label { ',' Label } ';' ")]
        private LabelDeclarationSection ParseLabelDeclarationSection(IExtendableSyntaxPart parent) {
            LabelDeclarationSection result = CreateByTerminal<LabelDeclarationSection>(parent, TokenKind.Label);

            do {
                ParseLabel(result);
            } while (ContinueWith(result, TokenKind.Comma));

            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #region ParseLabel

        [Rule("Label", "Identifier | Integer")]
        private Label ParseLabel(IExtendableSyntaxPart parent) {
            Label result = CreateChild<Label>(parent);

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
            ConstDeclaration result = CreateChild<ConstDeclaration>(parent);
            result.Attributes = ParseAttributes(result);
            result.Identifier = RequireIdentifier(result);

            if (ContinueWith(result, TokenKind.Colon)) {
                result.TypeSpecification = ParseTypeSpecification(result);
            }

            ContinueWithOrMissing(result, TokenKind.EqualsSign);
            result.Value = ParseConstantExpression(result);
            result.Hint = ParseHints(result);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseHints

        [Rule("Hints", " { Hint ';' }")]
        private HintingInformationList ParseHints(IExtendableSyntaxPart parent) {
            HintingInformationList result = CreateChild<HintingInformationList>(parent);

            HintingInformation hint;
            do {
                hint = ParseHint(result);
                ContinueWithOrMissing(result, TokenKind.Semicolon);
            } while (hint != null);

            return result;
        }

        #endregion
        #region ParseHint
        [Rule("Hint", " ('deprecated' [QuotedString] | 'experimental' | 'platform' | 'library' ) ")]
        private HintingInformation ParseHint(IExtendableSyntaxPart parent) {
            HintingInformation result = CreateChild<HintingInformation>(parent);

            if (ContinueWith(result, TokenKind.Deprecated)) {
                result.Deprecated = true;
                if (Match(TokenKind.QuotedString))
                    result.DeprecatedComment = RequireString(result);

                return result;
            }

            if (ContinueWith(result, TokenKind.Experimental)) {
                result.Experimental = true;
                return result;
            }

            if (ContinueWith(result, TokenKind.Platform)) {
                result.Platform = true;
                return result;
            }

            if (ContinueWith(result, TokenKind.Library)) {
                result.Library = true;
                return result;
            }

            return null;
        }

        #endregion
        #region ParseTypeSpecification

        [Rule("TypeSpecification", "StructType | PointerType | StringType | ProcedureType | SimpleType ")]
        private TypeSpecification ParseTypeSpecification(IExtendableSyntaxPart parent) {
            TypeSpecification result = CreateChild<TypeSpecification>(parent);

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

            result.SimpleType = ParseSimpleType(result);
            return result;
        }

        #endregion
        #region ParseSimpleType

        [Rule("SimpleType", "EnumType | (ConstExpression [ '..' ConstExpression ]) | ([ 'type' ] GenericNamespaceName {'.' GenericNamespaceName })")]
        private SimpleType ParseSimpleType(IExtendableSyntaxPart parent) {
            SimpleType result = CreateChild<SimpleType>(parent);

            if (Match(TokenKind.OpenParen)) {
                result.EnumType = ParseEnumType(result);
                return result;
            }

            result.NewType = ContinueWith(result, TokenKind.TypeKeyword);

            if (result.NewType)
                result.TypeOf = ContinueWith(result, TokenKind.Of);

            if (result.NewType || (MatchIdentifier(TokenKind.ShortString, TokenKind.String, TokenKind.WideString, TokenKind.UnicodeString, TokenKind.AnsiString) && (!LookAhead(1, TokenKind.DotDot)))) {
                do {
                    ParseGenericNamespaceName(result);
                } while (ContinueWith(result, TokenKind.Dot));
                return result;
            }

            result.SubrangeStart = ParseConstantExpression(result);
            if (ContinueWith(result, TokenKind.DotDot)) {
                result.SubrangeEnd = ParseConstantExpression(result);
            }

            return result;
        }

        #endregion
        #region GenericNamespaceName

        [Rule("GenericNamespaceName", "NamespaceName [ GenericPostfix ]")]
        private GenericNamespaceName ParseGenericNamespaceName(IExtendableSyntaxPart parent, bool advancedCheck = false) {
            GenericNamespaceName result = CreateChild<GenericNamespaceName>(parent);
            result.Name = ParseNamespaceName(result);
            if (Match(TokenKind.AngleBracketsOpen)) {
                if (!advancedCheck) {
                    result.GenericPart = ParseGenericSuffix(result);
                }
                else if (LookAheadIdentifier(1, new[] { TokenKind.String, TokenKind.ShortString, TokenKind.WideString, TokenKind.UnicodeString, TokenKind.AnsiString, TokenKind.Pointer }, false)) {
                    Tuple<bool, int> whereCloseBrackets = HasTokenUntilToken(new[] { TokenKind.AngleBracketsClose }, TokenKind.Identifier, TokenKind.Dot, TokenKind.Comma, TokenKind.AngleBracketsOpen, TokenKind.String, TokenKind.ShortString, TokenKind.WideString, TokenKind.UnicodeString, TokenKind.AnsiString, TokenKind.Pointer);
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
            EnumTypeDefinition result = CreateByTerminal<EnumTypeDefinition>(parent, TokenKind.OpenParen);

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
            EnumValue result = CreateChild<EnumValue>(parent);

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
            ProcedureType result = CreateChild<ProcedureType>(parent);

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
            ProcedureReference result = CreateByTerminal<ProcedureReference>(parent, TokenKind.Reference);
            ContinueWithOrMissing(result, TokenKind.To);
            result.ProcedureType = ParseProcedureRefType(result);
            result.ProcedureType.AllowAnonymousMethods = true;
            return result;
        }

        #endregion
        #region ParseProcedureRefType

        [Rule("ProcedureTypeDefinition", "('function' | 'procedure') [ '(' FormalParameters ')' ] [ ':' TypeSpecification ] [ 'of' 'object']")]
        private ProcedureTypeDefinition ParseProcedureRefType(IExtendableSyntaxPart parent) {
            ProcedureTypeDefinition result = CreateByTerminal<ProcedureTypeDefinition>(parent, TokenKind.Function, TokenKind.Procedure);
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
            FormalParameterSection result = CreateByTerminal<FormalParameterSection>(parent, TokenKind.OpenParen);

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
            StringType result = CreateChild<StringType>(parent);

            if (ContinueWith(result, TokenKind.String)) {
                result.Kind = TokenKind.String;
                if (ContinueWith(result, TokenKind.OpenBraces)) {
                    result.StringLength = ParseExpression(result);
                    ContinueWithOrMissing(result, TokenKind.CloseBraces);
                };
                return result;
            }

            if (ContinueWith(result, TokenKind.AnsiString)) {
                result.Kind = TokenKind.AnsiString;
                if (ContinueWith(result, TokenKind.OpenParen)) {
                    result.CodePage = ParseConstantExpression(result);
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
            StructType result = CreateChild<StructType>(parent);
            result.Packed = ContinueWith(result, TokenKind.Packed);
            result.Part = ParseStructTypePart(result);
            return result;
        }

        #endregion
        #region ParseStructTypePart

        [Rule("StructTypePart", "ArrayType | SetType | FileType | ClassDecl")]
        private StructTypePart ParseStructTypePart(IExtendableSyntaxPart parent) {
            StructTypePart result = CreateChild<StructTypePart>(parent);

            if (Match(TokenKind.Array)) {
                result.ArrayType = ParseArrayType(result);
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
                result.ClassDeclaration = ParseClassDeclaration(result);
                return result;
            }

            return result;
        }

        #endregion
        #region ParseClassDeclaration

        [Rule("ClassDeclaration", "ClassOfDeclaration | ClassDefinition | ClassHelper | InterfaceDef | ObjectDecl | RecordDecl | RecordHelperDecl ")]
        private ClassTypeDeclaration ParseClassDeclaration(IExtendableSyntaxPart parent) {
            ClassTypeDeclaration result = CreateChild<ClassTypeDeclaration>(parent);

            if (Match(TokenKind.Class) && LookAhead(1, TokenKind.Of)) {
                result.ClassOf = ParseClassOfDeclaration(result);
                return result;
            }

            if (Match(TokenKind.Class) && LookAhead(1, TokenKind.Helper)) {
                result.ClassHelper = ParseClassHelper(result);
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

        [Rule("RecordDecl", "'record' RecordFieldList (RecordVariantSection | RecordItems ) 'end' ")]
        private RecordDeclaration ParseRecordDecl(IExtendableSyntaxPart parent) {
            RecordDeclaration result = CreateByTerminal<RecordDeclaration>(parent, TokenKind.Record);

            if (MatchIdentifier() && !Match(TokenKind.Strict, TokenKind.Protected, TokenKind.Private, TokenKind.Public, TokenKind.Published)) {
                result.FieldList = ParseFieldList(result);
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

        [Rule("RecordItems", "{ RecordItem }")]
        private RecordItems ParseRecordItems(IExtendableSyntaxPart parent) {
            RecordItems result = CreateChild<RecordItems>(parent);
            var unexpected = false;

            while ((!Match(TokenKind.End)) && (!unexpected)) {
                ParseRecordItem(result, out unexpected);
                if (unexpected) {
                    Unexpected();
                    return result;
                }
            }

            return result;
        }

        [Rule("RecordItem", "MethodDeclaration | PropertyDeclaration | ConstSection | TypeSection | RecordField | ( ['class'] VarSection) | Visibility ")]
        private RecordItem ParseRecordItem(IExtendableSyntaxPart parent, out bool unexpected) {
            RecordItem result = CreateChild<RecordItem>(parent);

            if (Match(TokenKind.OpenBraces)) {
                ParseAttributes(result);
            }

            result.Class = ContinueWith(result, TokenKind.Class);

            if (Match(TokenKind.OpenBraces)) {
                ParseAttributes(result);
            }

            unexpected = false;

            if (Match(TokenKind.OpenBraces)) {
                ParseAttributes(result);
            }

            if (Match(TokenKind.Procedure, TokenKind.Function, TokenKind.Constructor, TokenKind.Destructor, TokenKind.Operator)) {
                result.MethodDeclaration = ParseMethodDeclaration(result);
                return result;
            }

            if (Match(TokenKind.Property)) {
                result.PropertyDeclaration = ParsePropertyDeclaration(result);
                return result;
            }

            if (Match(TokenKind.Case)) {
                result.VariantSection = ParseRecordVariantSection(result);
                return result;
            }

            if (!result.Class && Match(TokenKind.Const)) {
                result.ConstSection = ParseConstSection(result, true);
                return result;
            }

            if (!result.Class && Match(TokenKind.TypeKeyword)) {
                result.TypeSection = ParseTypeSection(result, true);
                return result;
            }

            if (Match(TokenKind.Var)) {
                result.VarSection = ParseVarSection(result, true);
                return result;
            }

            if (Match(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Strict, TokenKind.Published)) {
                result.Strict = ContinueWith(result, TokenKind.Strict);
                ContinueWith(result, TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Published);
                result.Visibility = result.LastTerminalKind;
                unexpected = false;
                return result;
            }

            if (MatchIdentifier()) {
                result.Fields = ParseFieldList(result);
                return result;
            }

            unexpected = true;
            return result;
        }

        [Rule("RecordVariantSection", "'case' [ Identifier ': ' ] TypeDeclaration 'of' { RecordVariant } ")]
        private RecordVariantSection ParseRecordVariantSection(IExtendableSyntaxPart parent) {
            RecordVariantSection result = CreateByTerminal<RecordVariantSection>(parent, TokenKind.Case);

            if (MatchIdentifier() && LookAhead(1, TokenKind.Colon)) {
                result.Name = RequireIdentifier(result);
                ContinueWithOrMissing(result, TokenKind.Colon);
            }
            result.TypeDeclaration = ParseTypeSpecification(result);
            ContinueWithOrMissing(result, TokenKind.Of);

            while (!Match(TokenKind.Undefined, TokenKind.Eof, TokenKind.End)) {
                ParseRecordVariant(result);
            }

            return result;
        }

        [Rule("RecordVariant", "ConstantExpression { , ConstantExpression } : '(' FieldList ')' ';' ")]
        private RecordVariant ParseRecordVariant(IExtendableSyntaxPart parent) {
            RecordVariant result = CreateChild<RecordVariant>(parent);

            do {
                ParseConstantExpression(result);
            } while (ContinueWith(result, TokenKind.Comma));

            ContinueWithOrMissing(result, TokenKind.Colon);
            ContinueWithOrMissing(result, TokenKind.OpenParen);
            result.FieldList = ParseFieldList(result);
            ContinueWithOrMissing(result, TokenKind.CloseParen);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        [Rule("RecordFieldList", " { RecordField } ")]
        private RecordFieldList ParseFieldList(IExtendableSyntaxPart parent) {
            RecordFieldList result = CreateChild<RecordFieldList>(parent);
            while (MatchIdentifier() && (!Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Strict))) {
                ParseRecordField(result);
            }
            return result;
        }

        [Rule("RecordField", "IdentList ':' TypeSpecification Hints ';'")]
        private RecordField ParseRecordField(IExtendableSyntaxPart parent) {
            RecordField result = CreateChild<RecordField>(parent);
            result.Names = ParseIdentList(result, true);
            ContinueWithOrMissing(result, TokenKind.Colon);
            result.FieldType = ParseTypeSpecification(result);
            result.Hint = ParseHints(result);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        [Rule("RecordHelperDecl", "'record' 'helper' 'for' TypeName RecordHelperItems 'end'")]
        private RecordHelperDefinition ParseRecordHelper(IExtendableSyntaxPart parent) {
            RecordHelperDefinition result = CreateByTerminal<RecordHelperDefinition>(parent, TokenKind.Record);
            ContinueWithOrMissing(result, TokenKind.Helper);
            ContinueWithOrMissing(result, TokenKind.For);
            result.Name = ParseTypeName(result);
            result.Items = ParseRecordHelperItems(result);
            ContinueWithOrMissing(result, TokenKind.End);
            return result;
        }

        [Rule("RecordHelperItems", " { RecordHelperItem }")]
        private RecordHelperItems ParseRecordHelperItems(IExtendableSyntaxPart parent) {
            RecordHelperItems result = CreateChild<RecordHelperItems>(parent);
            var unexpected = false;

            while ((!Match(TokenKind.End)) && (!unexpected)) {
                ParseRecordHelperItem(result, out unexpected);
                if (unexpected) {
                    Unexpected();
                    return result;
                }
            }

            return result;
        }

        [Rule("RecordHelperItem", "")]
        private RecordHelperItem ParseRecordHelperItem(IExtendableSyntaxPart parent, out bool unexpected) {
            RecordHelperItem result = CreateChild<RecordHelperItem>(parent);
            unexpected = false;

            if (Match(TokenKind.OpenBraces)) {
                ParseAttributes(result);
            }

            result.Class = ContinueWith(result, TokenKind.Class);

            if (Match(TokenKind.OpenBraces)) {
                ParseAttributes(result);
            }

            if (Match(TokenKind.Const)) {
                result.ConstDeclaration = ParseConstSection(result, true);
                return result;
            }

            if (Match(TokenKind.Procedure, TokenKind.Function, TokenKind.Constructor, TokenKind.Destructor)) {
                result.MethodDeclaration = ParseMethodDeclaration(result);
                return result;
            }

            if (Match(TokenKind.Property)) {
                result.PropertyDeclaration = ParsePropertyDeclaration(result);
                return result;
            }

            if (Match(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Strict, TokenKind.Published)) {
                result.Strict = ContinueWith(result, TokenKind.Strict);
                ContinueWith(result, TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Published);
                result.Visibility = result.LastTerminalKind;
                unexpected = false;
                return result;
            }

            unexpected = true;
            return result;
        }

        [Rule("ObjectDecl", "'object' ClassParent ObjectItems 'end' ")]
        private ObjectDeclaration ParseObjectDecl(IExtendableSyntaxPart parent) {
            ObjectDeclaration result = CreateByTerminal<ObjectDeclaration>(parent, TokenKind.Object);

            result.ClassParent = ParseClassParent(result);
            result.Items = ParseObjectItems(result);

            ContinueWithOrMissing(result, TokenKind.End);
            return result;
        }

        [Rule("ObjectItems", " { ObjectItem } ")]
        private ObjectItems ParseObjectItems(IExtendableSyntaxPart parent) {
            ObjectItems result = CreateChild<ObjectItems>(parent);
            var unexpected = false;

            while ((!Match(TokenKind.End)) && (!unexpected)) {
                ParseObjectItem(result, out unexpected);
                if (unexpected) {
                    Unexpected();
                    return result;
                }
            }

            return result;
        }

        [Rule("ObjectItem", "Visibility | MethodDeclaration | ClassFieldDeclaration ")]
        private ObjectItem ParseObjectItem(IExtendableSyntaxPart parent, out bool unexpected) {
            ObjectItem result = CreateChild<ObjectItem>(parent);

            if (Match(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Strict, TokenKind.Published, TokenKind.Automated)) {
                result.Strict = ContinueWith(result, TokenKind.Strict);
                ContinueWith(result, TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Published, TokenKind.Automated);
                result.Visibility = result.LastTerminalKind;
                unexpected = false;
                return result;
            }

            if (Match(TokenKind.Procedure, TokenKind.Function, TokenKind.Constructor, TokenKind.Destructor)) {
                result.MethodDeclaration = ParseMethodDeclaration(result);
                unexpected = false;
                return result;
            }

            if (MatchIdentifier()) {
                result.FieldDeclaration = ParseClassFieldDeclararation(result);
                unexpected = false;
                return result;
            }

            unexpected = true;
            return result;
        }

        [Rule("InterfaceDef", "('inteface' | 'dispinterface') ClassParent [InterfaceGuid] InterfaceDefItems 'end'")]
        private InterfaceDefinition ParseInterfaceDef(IExtendableSyntaxPart parent) {
            InterfaceDefinition result = CreateChild<InterfaceDefinition>(parent);

            if (!ContinueWith(result, TokenKind.Interface)) {
                ContinueWithOrMissing(result, TokenKind.DispInterface);
                result.DisplayInterface = true;
            }
            result.ParentInterface = ParseClassParent(result);
            if (Match(TokenKind.OpenBraces))
                result.Guid = ParseInterfaceGuid(result);
            result.Items = ParseInterfaceItems(result);
            if (result.Items.PartList.Count > 0)
                ContinueWithOrMissing(result, TokenKind.End);
            else
                ContinueWith(result, TokenKind.End);

            return result;
        }

        [Rule("InterfaceItems", "{ InterfaceItem }")]
        private InterfaceItems ParseInterfaceItems(IExtendableSyntaxPart parent) {
            InterfaceItems result = CreateChild<InterfaceItems>(parent);
            var unexpected = false;

            while ((!Match(TokenKind.End)) && (!unexpected)) {
                ParseInterfaceItem(result, out unexpected);
                if (unexpected && result.PartList.Count > 0) {
                    Unexpected();
                    return result;
                }
            }

            return result;
        }

        [Rule("InterfaceItem", "MethodDeclaration | PropertyDeclaration")]
        private InterfaceItem ParseInterfaceItem(IExtendableSyntaxPart parent, out bool unexpected) {
            InterfaceItem result = CreateChild<InterfaceItem>(parent);
            unexpected = true;

            if (Match(TokenKind.Procedure, TokenKind.Function)) {
                unexpected = false;
                result.Method = ParseMethodDeclaration(result);
                return result;
            }

            if (Match(TokenKind.Property)) {
                unexpected = false;
                result.Property = ParsePropertyDeclaration(result);
                return result;
            }

            return result;
        }

        [Rule("InterfaceGuid", "'[' QuotedString ']'")]
        private InterfaceGuid ParseInterfaceGuid(IExtendableSyntaxPart parent) {
            InterfaceGuid result = CreateByTerminal<InterfaceGuid>(parent, TokenKind.OpenBraces);

            if (Match(TokenKind.Identifier))
                result.IdIdentifier = RequireIdentifier(result);
            else
                result.Id = RequireString(result);

            ContinueWithOrMissing(result, TokenKind.CloseBraces);
            return result;
        }

        [Rule("ClassHelper", "'class' 'helper' ClassParent 'for' TypeName ClassHelperItems 'end'")]
        private ClassHelperDef ParseClassHelper(IExtendableSyntaxPart parent) {
            ClassHelperDef result = CreateByTerminal<ClassHelperDef>(parent, TokenKind.Class);
            ContinueWithOrMissing(result, TokenKind.Helper);
            result.ClassParent = ParseClassParent(result);
            ContinueWithOrMissing(result, TokenKind.For);
            result.HelperName = ParseTypeName(result);
            result.HelperItems = ParseClassHelperItems(result);
            ContinueWithOrMissing(result, TokenKind.End);
            return result;
        }

        [Rule("ClassHelperItems", " { ClassHelperItem }")]
        private ClassHelperItems ParseClassHelperItems(IExtendableSyntaxPart parent) {
            ClassHelperItems result = CreateChild<ClassHelperItems>(parent);
            while (!Match(TokenKind.End, TokenKind.Undefined, TokenKind.Eof)) {
                ParseClassHelperItem(result);
            }
            return result;
        }

        [Rule("ClassHelperItem", "Visibility | MethodDeclaration | PropertyDeclaration | [ 'class' ] VarSection")]
        private ClassHelperItem ParseClassHelperItem(IExtendableSyntaxPart parent) {
            ClassHelperItem result = CreateChild<ClassHelperItem>(parent);
            result.Attributes = ParseAttributes(result);
            result.Class = ContinueWith(result, TokenKind.Class);

            if (!result.Class && (result.Attributes.PartList.Count < 1) && Match(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Strict, TokenKind.Published, TokenKind.Automated)) {
                result.Strict = ContinueWith(result, TokenKind.Strict);
                ContinueWithOrMissing(result, TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Published, TokenKind.Automated);
                result.Visibility = result.LastTerminalKind;
                return result;
            }

            if (Match(TokenKind.Procedure, TokenKind.Function, TokenKind.Constructor, TokenKind.Destructor)) {
                result.MethodDeclaration = ParseMethodDeclaration(result);
                return result;
            }

            if (Match(TokenKind.Property)) {
                result.PropertyDeclaration = ParsePropertyDeclaration(result);
                return result;
            }

            if (Match(TokenKind.Var)) {
                result.VarSection = ParseVarSection(result, true);
                return result;
            }

            Unexpected();
            return result;
        }

        #region ParseClassDefinition

        [Rule("ClassDefinition", "'class' [( 'sealed' | 'abstract' )] [ClassParent] ClassItems 'end' ")]
        private ClassDeclaration ParseClassDefinition(IExtendableSyntaxPart parent) {
            ClassDeclaration result = CreateByTerminal<ClassDeclaration>(parent, TokenKind.Class);

            result.Sealed = ContinueWith(result, TokenKind.Sealed);
            result.Abstract = ContinueWith(result, TokenKind.Abstract);

            result.ClassParent = ParseClassParent(result);

            if (!Match(TokenKind.Semicolon))
                result.ClassItems = ParseClassItems(result);

            if (result.ClassItems != null && result.ClassItems.PartList.Count > 0)
                ContinueWithOrMissing(result, TokenKind.End);
            else
                result.ForwardDeclaration = !ContinueWith(result, TokenKind.End);

            return result;
        }

        #endregion
        #region ParseClassItems

        [Rule("ClassItems", "{ ClassItem } ")]
        private ClassDeclarationItems ParseClassItems(IExtendableSyntaxPart parent) {
            ClassDeclarationItems result = CreateChild<ClassDeclarationItems>(parent);
            ClassDeclarationMode mode = ClassDeclarationMode.Fields;

            while ((!Match(TokenKind.End)) && (mode != ClassDeclarationMode.Undefined)) {
                ParseClassDeclarationItem(result, ref mode);

                if (mode == ClassDeclarationMode.Undefined && result.PartList.Count > 0) {
                    Unexpected();
                    return result;
                }
            }

            return result;
        }

        #endregion
        #region ParseClassDeclarationItem

        [Rule("ClassItem", "Visibility | MethodResolution | MethodDeclaration | ConstSection | TypeSection | PropertyDeclaration | [ 'class'] VarSection | FieldDeclarations ")]
        private ClassDeclarationItem ParseClassDeclarationItem(IExtendableSyntaxPart parent, ref ClassDeclarationMode mode) {

            if (ContinueWith(parent, TokenKind.Var)) {
                mode = ClassDeclarationMode.Fields;
                return null;
            }

            if (Match(TokenKind.Class) && LookAhead(1, TokenKind.Var)) {
                ContinueWith(parent, TokenKind.Class);
                ContinueWith(parent, TokenKind.Var);
                mode = ClassDeclarationMode.ClassFields;
                return null;
            }

            ClassDeclarationItem result = CreateChild<ClassDeclarationItem>(parent);

            if (Match(TokenKind.OpenBraces)) {
                result.Attributes = ParseAttributes(result);
            }

            result.Class = ContinueWith(result, TokenKind.Class);

            if (Match(TokenKind.OpenBraces)) {
                result.Attributes = ParseAttributes(result, result.Attributes);
            }

            if (Match(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Strict, TokenKind.Published, TokenKind.Automated)) {
                if (result.Class) {
                    Unexpected();
                }
                else {
                    result.Strict = ContinueWith(result, TokenKind.Strict);
                    ContinueWith(result, TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Published, TokenKind.Automated);
                    result.Visibility = result.LastTerminalKind;
                }
                mode = ClassDeclarationMode.Fields;
                return result;
            }

            if (Match(TokenKind.Procedure, TokenKind.Function) && HasTokenBeforeToken(TokenKind.EqualsSign, TokenKind.Semicolon, TokenKind.OpenParen)) {
                if (result.Class) {
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
                result.MethodDeclaration = ParseMethodDeclaration(result);
                return result;
            }

            if (Match(TokenKind.Property)) {
                mode = ClassDeclarationMode.Other;
                result.PropertyDeclaration = ParsePropertyDeclaration(result);
                return result;
            }

            if (!result.Class && Match(TokenKind.Const)) {
                result.ConstSection = ParseConstSection(result, true);
                return result;
            }

            if (!result.Class && Match(TokenKind.TypeKeyword)) {
                result.TypeSection = ParseTypeSection(result, true);
                return result;
            }

            if (MatchIdentifier() && (!Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Automated, TokenKind.Strict))) {

                if (mode == ClassDeclarationMode.Fields || mode == ClassDeclarationMode.ClassFields) {
                    result.FieldDeclaration = ParseClassFieldDeclararation(result);
                    result.Class = mode == ClassDeclarationMode.ClassFields;
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

        [Rule("FieldDeclaration", "IdentList ':' TypeSpecification Hints ';'")]
        private ClassField ParseClassFieldDeclararation(IExtendableSyntaxPart parent) {
            ClassField result = CreateChild<ClassField>(parent);
            result.Names = ParseIdentList(result, true);
            ContinueWithOrMissing(result, TokenKind.Colon);
            result.TypeDecl = ParseTypeSpecification(result);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            result.Hint = ParseHints(result);
            return result;
        }

        #endregion
        #region PropertyDeclaration

        [Rule("PropertyDeclaration", "'property' Identifier [ '[' FormalParameters  ']' ] [ ':' TypeName ] [ 'index' Expression ]  { ClassPropertySpecifier } ';' [ 'default' ';' ]  ")]
        private ClassProperty ParsePropertyDeclaration(IExtendableSyntaxPart parent) {
            ClassProperty result = CreateByTerminal<ClassProperty>(parent, TokenKind.Property);
            result.PropertyName = RequireIdentifier(result);
            if (ContinueWith(result, TokenKind.OpenBraces)) {
                result.ArrayIndex = ParseFormalParameters(result);
                ContinueWithOrMissing(result, TokenKind.CloseBraces);
            }

            if (ContinueWith(result, TokenKind.Colon)) {
                result.TypeName = ParseTypeName(result);
            }

            if (ContinueWith(result, TokenKind.Index)) {
                result.PropertyIndex = ParseExpression(result);
            }

            while (Match(TokenKind.Read, TokenKind.Write, TokenKind.Add, TokenKind.Remove, TokenKind.ReadOnly, TokenKind.WriteOnly, TokenKind.DispId) ||
                Match(TokenKind.Default, TokenKind.Stored, TokenKind.Implements, TokenKind.NoDefault)) {
                ParseClassPropertyAccessSpecifier(result);
            }

            ContinueWithOrMissing(result, TokenKind.Semicolon);

            if (ContinueWith(result, TokenKind.Default)) {
                result.IsDefault = true;
                ContinueWithOrMissing(result, TokenKind.Semicolon);
            }

            return result;
        }

        #endregion
        #region ParseClassPropertyAccessSpecifier

        [Rule("ClassPropertySpecifier", "ClassPropertyReadWrite | ClassPropertyDispInterface | ('stored' Expression ';') | ('default' [ Expression ] ';' ) | ('nodefault' ';') | ('implements' NamespaceName) ")]
        private ClassPropertySpecifier ParseClassPropertyAccessSpecifier(IExtendableSyntaxPart parent) {
            ClassPropertySpecifier result = CreateChild<ClassPropertySpecifier>(parent);

            if (Match(TokenKind.Read, TokenKind.Write, TokenKind.Add, TokenKind.Remove)) {
                result.PropertyReadWrite = ParseClassPropertyReadWrite(result);
                return result;
            }

            if (Match(TokenKind.ReadOnly, TokenKind.WriteOnly, TokenKind.DispId)) {
                result.PropertyDispInterface = ParseClassPropertyDispInterface(result);
                return result;
            }

            if (ContinueWith(result, TokenKind.Stored)) {
                result.IsStored = true;
                result.StoredProperty = ParseExpression(result);
                ContinueWithOrMissing(result, TokenKind.Semicolon);
                return result;
            }

            if (ContinueWith(result, TokenKind.Default)) {
                result.IsDefault = true;
                if (!ContinueWith(result, TokenKind.Semicolon)) {
                    result.DefaultProperty = ParseExpression(result);
                    ContinueWithOrMissing(result, TokenKind.Semicolon);
                }
                return result;
            }

            if (ContinueWith(result, TokenKind.NoDefault)) {
                result.NoDefault = true;
                ContinueWithOrMissing(result, TokenKind.Semicolon);
                return result;
            }

            if (ContinueWith(result, TokenKind.Implements)) {
                result.ImplementsTypeId = ParseNamespaceName(result);
                return result;
            }

            Unexpected();
            return result;
        }

        #endregion
        #region ParseClassPropertyDispInterface

        [Rule("ClassPropertyDispInterface", "( 'readonly' ';')  | ( 'writeonly' ';' ) | DispIdDirective ")]
        private ClassPropertyDispInterface ParseClassPropertyDispInterface(IExtendableSyntaxPart parent) {
            ClassPropertyDispInterface result = CreateChild<ClassPropertyDispInterface>(parent);

            if (ContinueWith(result, TokenKind.ReadOnly)) {
                result.ReadOnly = true;
                ContinueWithOrMissing(result, TokenKind.Semicolon);
                return result;
            }

            if (ContinueWith(result, TokenKind.WriteOnly)) {
                result.WriteOnly = true;
                ContinueWithOrMissing(result, TokenKind.Semicolon);
                return result;
            }

            result.DispId = ParseDispIdDirective(parent);
            return result;
        }

        #endregion
        #region ParseDispIdDirective

        [Rule("DispIdDirective", "'dispid' Expression ';'")]
        private DispIdDirective ParseDispIdDirective(IExtendableSyntaxPart parent) {
            DispIdDirective result = CreateByTerminal<DispIdDirective>(parent, TokenKind.DispId);
            result.DispExpression = ParseExpression(result);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseClassPropertyReadWrite

        [Rule("ClassPropertyReadWrite", "('read' | 'write' | 'add' | 'remove' ) NamespaceName ")]
        private ClassPropertyReadWrite ParseClassPropertyReadWrite(IExtendableSyntaxPart parent) {
            ClassPropertyReadWrite result = CreateByTerminal<ClassPropertyReadWrite>(parent, TokenKind.Read, TokenKind.Write, TokenKind.Add, TokenKind.Remove);

            result.Kind = result.LastTerminalKind;
            result.Member = ParseNamespaceName(result);

            return result;
        }

        #endregion
        #region ParseTypeSection

        [Rule("TypeSection", "'type' TypeDeclaration { TypeDeclaration }")]
        private TypeSection ParseTypeSection(IExtendableSyntaxPart parent, bool inClassDeclaration) {
            TypeSection result = CreateByTerminal<TypeSection>(parent, TokenKind.TypeKeyword);

            while ((!inClassDeclaration || !Match(TokenKind.Private, TokenKind.Protected, TokenKind.Public, TokenKind.Published, TokenKind.Automated, TokenKind.Strict)) && MatchIdentifier(TokenKind.OpenBraces)) {
                ParseTypeDeclaration(result);
            };

            return result;
        }

        #endregion
        #region ParseTypeDeclaration

        [Rule("TypeDeclaration", "[ Attributes ] GenericTypeIdent '=' TypeDeclaration Hints ';' ")]
        private TypeDeclaration ParseTypeDeclaration(IExtendableSyntaxPart parent) {
            TypeDeclaration result = CreateChild<TypeDeclaration>(parent);
            result.Attributes = ParseAttributes(result);
            result.TypeId = ParseGenericTypeIdent(result);
            ContinueWithOrMissing(result, TokenKind.EqualsSign);
            result.TypeSpecification = ParseTypeSpecification(result);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            result.Hint = ParseHints(result);
            return result;
        }

        #endregion
        #region ParseGenericTypeIdent

        [Rule("GenericTypeIdent", "Ident [ GenericDefintion ] ")]
        private GenericTypeIdentifier ParseGenericTypeIdent(IExtendableSyntaxPart parent) {
            GenericTypeIdentifier result = CreateChild<GenericTypeIdentifier>(parent);
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
            MethodResolution result = CreateByTerminal<MethodResolution>(parent, TokenKind.Function, TokenKind.Procedure);
            result.Kind = result.LastTerminalKind;
            result.Name = ParseTypeName(result);
            ContinueWithOrMissing(result, TokenKind.EqualsSign);
            result.ResolveIdentifier = RequireIdentifier(result);
            ContinueWithOrMissing(result, TokenKind.Semicolon);
            return result;
        }

        #endregion
        #region ParseMethodDeclaration

        [Rule("MethodDeclaration", "( 'constructor' | 'destructor' | 'procedure' | 'function' | 'operator') Identifier [GenericDefinition] [FormalParameters] [ ':' [ Attributes ] TypeSpecification ] ';' { MethodDirective } ")]
        private ClassMethod ParseMethodDeclaration(IExtendableSyntaxPart parent) {
            ClassMethod result = CreateByTerminal<ClassMethod>(parent, TokenKind.Constructor, TokenKind.Destructor, TokenKind.Procedure, TokenKind.Function, TokenKind.Operator);
            result.MethodKind = result.LastTerminalKind;
            result.Identifier = RequireIdentifier(result);

            if (Match(TokenKind.AngleBracketsOpen)) {
                result.GenericDefinition = ParseGenericDefinition(result);
            }

            if (ContinueWith(result, TokenKind.OpenParen) && (!ContinueWith(result, TokenKind.CloseParen))) {
                result.Parameters = ParseFormalParameters(result);
                ContinueWithOrMissing(result, TokenKind.CloseParen);
            }

            if (ContinueWith(result, TokenKind.Colon)) {
                result.ResultAttributes = ParseAttributes(result);
                result.ResultType = ParseTypeSpecification(result);
            }

            ContinueWithOrMissing(result, TokenKind.Semicolon);
            result.Directives = ParseMethodDirectives(result);
            return result;
        }

        #endregion
        #region FormalParameters

        [Rule("FormalParameters", "FormalParameter { ';' FormalParameter }")]
        private FormalParameters ParseFormalParameters(IExtendableSyntaxPart parent) {
            FormalParameters result = CreateChild<FormalParameters>(parent);

            do {
                ParseFormalParameter(result);
            } while (ContinueWith(result, TokenKind.Semicolon));

            return result;
        }

        #endregion
        #region FormalParameter

        [Rule("FormalParameter", "[Attributes] [( 'const' | 'var' | 'out' )] [Attributes] IdentList [ ':' TypeDeclaration ] [ '=' Expression ]")]
        private void ParseFormalParameter(IExtendableSyntaxPart parent) {
            var kind = TokenKind.Undefined - 1;
            FormalParameterDefinition parentDefinition = CreateChild<FormalParameterDefinition>(parent);

            do {
                FormalParameter result = CreateChild<FormalParameter>(parentDefinition);

                if (Match(TokenKind.OpenBraces)) {
                    result.Attributes = ParseAttributes(result);
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
                    result.Attributes = ParseAttributes(result, result.Attributes);
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

        [Rule("IdentList", "Identifiert { ',' Identifier }")]
        private IdentifierList ParseIdentList(IExtendableSyntaxPart parent, bool allowAttributes) {
            IdentifierList result = CreateChild<IdentifierList>(parent);

            do {
                if (allowAttributes && Match(TokenKind.OpenBraces))
                    ParseAttributes(result);
                RequireIdentifier(result);
            } while (ContinueWith(result, TokenKind.Comma));

            return result;
        }

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
            GenericDefinition result = CreateByTerminal<GenericDefinition>(parent, TokenKind.AngleBracketsOpen);

            do {
                GenericDefinitionPart part = CreateChild<GenericDefinitionPart>(result);
                part.Identifier = RequireIdentifier(result);
            } while (ContinueWith(result, TokenKind.Comma));

            ContinueWithOrMissing(result, TokenKind.AngleBracketsClose);
            return result;
        }

        #endregion
        #region ParseConstrainedGenericDefinition

        [Rule("ConstrainedGenericDefinition", "'<' GenericDefinitionPart { ';' GenericDefinitionPart } '>'")]
        private GenericDefinition ParseConstrainedGenericDefinition(IExtendableSyntaxPart parent) {
            GenericDefinition result = CreateByTerminal<GenericDefinition>(parent, TokenKind.AngleBracketsOpen);

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
            GenericDefinitionPart result = CreateChild<GenericDefinitionPart>(parent);
            result.Identifier = RequireIdentifier(result);

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
            ConstrainedGeneric result = CreateChild<ConstrainedGeneric>(parent);

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
            ParentClass result = CreateChild<ParentClass>(parent);

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

        [Rule("ClassOfDeclaration", "'class' 'of' TypeName")]
        private ClassOfDeclaration ParseClassOfDeclaration(IExtendableSyntaxPart parent) {
            ClassOfDeclaration result = CreateByTerminal<ClassOfDeclaration>(parent, TokenKind.Class);
            ContinueWithOrMissing(result, TokenKind.Of);
            result.TypeRef = ParseTypeName(result);
            return result;
        }

        #endregion
        #region ParseTypeName

        [Rule("TypeName", "'string' | 'ansistring' | 'shortstring' | 'unicodestring' | 'widestring' | (NamespaceName [ GenericSuffix ]) ")]
        private TypeName ParseTypeName(IExtendableSyntaxPart parent) {
            TypeName result = CreateChild<TypeName>(parent);

            if (ContinueWith(result, TokenKind.String, TokenKind.AnsiString, TokenKind.ShortString, TokenKind.UnicodeString, TokenKind.WideString)) {
                result.StringType = result.LastTerminalKind;
                return result;
            }

            do {
                ParseGenericNamespaceName(result, true);
            } while (ContinueWith(result, TokenKind.Dot));

            return result;
        }

        #endregion
        #region ParseFileType

        [Rule("FileType", "'file' [ 'of' TypeSpecification ]")]
        private FileType ParseFileType(IExtendableSyntaxPart parent) {
            FileType result = CreateByTerminal<FileType>(parent, TokenKind.File);

            if (ContinueWith(result, TokenKind.Of)) {
                result.TypeDefinition = ParseTypeSpecification(result);
            }

            return result;
        }

        #endregion
        #region SetDefinition

        [Rule("SetDef", "'set' 'of' TypeSpecification")]
        private SetDefinition ParseSetDefinition(IExtendableSyntaxPart parent) {
            SetDefinition result = CreateByTerminal<SetDefinition>(parent, TokenKind.Set);
            ContinueWithOrMissing(result, TokenKind.Of);
            result.TypeDefinition = ParseTypeSpecification(result);
            return result;
        }

        #endregion

        [Rule("GenericSuffix", "'<' TypeDefinition { ',' TypeDefinition '}' '>'")]
        private GenericPostfix ParseGenericSuffix(IExtendableSyntaxPart parent) {
            GenericPostfix result = CreateByTerminal<GenericPostfix>(parent, TokenKind.AngleBracketsOpen);

            do {
                ParseTypeSpecification(result);
            } while (ContinueWith(result, TokenKind.Comma));

            ContinueWithOrMissing(result, TokenKind.AngleBracketsClose);
            return result;
        }

        #region ParseArrayType

        [Rule("ArrayType", " 'array' [ '[' ArrayIndex { ',' ArrayIndex } ']']  'of' ( 'const' | TypeDefinition ) ")]
        private ArrayType ParseArrayType(IExtendableSyntaxPart parent) {
            ArrayType result = CreateByTerminal<ArrayType>(parent, TokenKind.Array);

            if (ContinueWith(result, TokenKind.OpenBraces)) {

                do {
                    ParseArrayIndex(result);
                } while (ContinueWith(result, TokenKind.Comma));

                ContinueWithOrMissing(result, TokenKind.CloseBraces);
            }

            ContinueWithOrMissing(result, TokenKind.Of);

            if (ContinueWith(result, TokenKind.Const)) {
                result.ArrayOfConst = true;
            }
            else {
                result.TypeSpecification = ParseTypeSpecification(result);
            }

            return result;
        }

        #endregion
        #region ParseArrayIndex

        [Rule("ArrayIndex", "ConstantExpression [ '..' ConstantExpression ] ")]
        private ArrayIndex ParseArrayIndex(IExtendableSyntaxPart parent) {
            ArrayIndex result = CreateChild<ArrayIndex>(parent);

            result.StartIndex = ParseConstantExpression(result);
            if (ContinueWith(result, TokenKind.DotDot)) {
                result.EndIndex = ParseConstantExpression(result);
            }
            return result;
        }

        #endregion
        #region ParsePointerType

        [Rule("PointerType", "( 'pointer' | '^' TypeSpecification )")]
        private PointerType ParsePointerType(IExtendableSyntaxPart parent) {
            PointerType result = CreateChild<PointerType>(parent);

            if (ContinueWith(result, TokenKind.Pointer)) {
                result.GenericPointer = true;
                return result;
            }

            ContinueWithOrMissing(result, TokenKind.Circumflex);
            result.TypeSpecification = ParseTypeSpecification(result);
            return result;
        }

        #endregion

        [Rule("Attributes", "{ '[' Attribute | AssemblyAttribue ']' }")]
        private UserAttributes ParseAttributes(IExtendableSyntaxPart parent, UserAttributes result = null) {
            if (result == null)
                result = CreateChild<UserAttributes>(parent);

            while (Match(TokenKind.OpenBraces)) {
                if (LookAhead(1, TokenKind.Assembly)) {
                    ParseAssemblyAttribute(result);
                }
                else {
                    ContinueWithOrMissing(result, TokenKind.OpenBraces);
                    do {
                        ParseAttribute(result);
                    } while (ContinueWith(result, TokenKind.Comma));
                    ContinueWithOrMissing(result, TokenKind.CloseBraces);
                }
            }
            return result;
        }

        [Rule("Attribute", " [ 'Result' ':' ] NamespaceName [ '(' Expressions ')' ]")]
        private UserAttributeDefinition ParseAttribute(IExtendableSyntaxPart parent) {
            UserAttributeDefinition result = CreateChild<UserAttributeDefinition>(parent);

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

        [Rule("Expressions", "Expression { ',' Expression }")]
        private ExpressionList ParseExpressions(IExtendableSyntaxPart parent) {
            ExpressionList result = CreateChild<ExpressionList>(parent);

            do {
                ParseExpression(result);
            } while (ContinueWith(result, TokenKind.Comma));

            return result;
        }

        [Rule("ConstantExpression", " '(' ( RecordConstant | ConstantExpression ) ')' | Expression")]
        private ConstantExpression ParseConstantExpression(IExtendableSyntaxPart parent) {
            ConstantExpression result = CreateChild<ConstantExpression>(parent);

            if (Match(TokenKind.OpenParen)) {

                if (LookAheadIdentifier(1, new int[0], true) && (LookAhead(2, TokenKind.Colon))) {
                    result.IsRecordConstant = true;
                    ContinueWithOrMissing(result, TokenKind.OpenParen);
                    do {
                        ParseRecordConstant(result);
                    } while (ContinueWith(result, TokenKind.Semicolon));
                    ContinueWithOrMissing(result, TokenKind.CloseParen);
                }
                else if (HasTokenBeforeToken(TokenKind.Comma, TokenKind.OpenParen, TokenKind.OpenBraces, TokenKind.CloseBraces, TokenKind.CloseParen)) {
                    result.IsArrayConstant = true;
                    ContinueWithOrMissing(result, TokenKind.OpenParen);
                    do {
                        ParseConstantExpression(result);
                    } while (ContinueWith(result, TokenKind.Comma));
                    ContinueWithOrMissing(result, TokenKind.CloseParen);
                }
                else {
                    result.Value = ParseExpression(result);
                }

            }
            else {
                result.Value = ParseExpression(result);
            }

            return result;
        }

        [Rule("RecordConstantExpression", "Identifier ':' ConstantExpression")]
        private RecordConstantExpression ParseRecordConstant(IExtendableSyntaxPart parent) {
            RecordConstantExpression result = CreateChild<RecordConstantExpression>(parent);
            result.Name = RequireIdentifier(result);
            ContinueWithOrMissing(result, TokenKind.Colon);
            result.Value = ParseConstantExpression(result);
            return result;
        }

        [Rule("Expression", "SimpleExpression [ ('<'|'<='|'>'|'>='|'<>'|'='|'in'|'as') SimpleExpression ] | ClosureExpression")]
        private Expression ParseExpression(IExtendableSyntaxPart parent) {
            Expression result = CreateChild<Expression>(parent);

            if (Match(TokenKind.Function, TokenKind.Procedure)) {
                result.ClosureExpression = ParseClosureExpression(result);
            }
            else {
                result.LeftOperand = ParseSimpleExpression(result);
                if (ContinueWith(result, TokenKind.LessThen, TokenKind.LessThenEquals, TokenKind.GreaterThen, TokenKind.GreaterThenEquals, TokenKind.NotEquals, TokenKind.EqualsSign, TokenKind.In, TokenKind.Is)) {
                    result.Kind = result.LastTerminalKind;
                    result.RightOperand = ParseSimpleExpression(result);
                }
            }

            return result;
        }

        [Rule("SimpleExpression", "Term { ('+'|'-'|'or'|'xor') SimpleExpression }")]
        private SimpleExpression ParseSimpleExpression(IExtendableSyntaxPart parent) {
            SimpleExpression result = CreateChild<SimpleExpression>(parent);

            result.LeftOperand = ParseTerm(result);
            if (ContinueWith(result, TokenKind.Plus, TokenKind.Minus, TokenKind.Or, TokenKind.Xor)) {
                result.Kind = result.LastTerminalKind;
                result.RightOperand = ParseSimpleExpression(result);
            }

            return result;
        }

        [Rule("Term", "Factor [ ('*'|'/'|'div'|'mod'|'and'|'shl'|'shr'|'as') Term ]")]
        private Term ParseTerm(IExtendableSyntaxPart parent) {
            Term result = CreateChild<Term>(parent);

            result.LeftOperand = ParseFactor(result);

            if (ContinueWith(result, TokenKind.Times, TokenKind.Slash, TokenKind.Div, TokenKind.Mod, TokenKind.And, TokenKind.Shl, TokenKind.Shr, TokenKind.As)) {
                result.Kind = result.LastTerminalKind;
                result.RightOperand = ParseTerm(result);
            }

            return result;
        }

        [Rule("Factor", "'@' Factor  | 'not' Factor | '+' Factor | '-' Factor | '^' Identifier | Integer | HexNumber | Real | 'true' | 'false' | 'nil' | '(' Expression ')' | String | SetSection | Designator | TypeCast")]
        private Factor ParseFactor(IExtendableSyntaxPart parent) {
            Factor result = CreateChild<Factor>(parent);

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

            if (Match(TokenKind.Circumflex, TokenKind.Dot, TokenKind.OpenBraces, TokenKind.OpenParen)) {
                result.Designator = ParseDesignator(result);
                return result;
            }

            Unexpected();
            return null;
        }

        [Rule("Designator", "[ 'inherited' ] [ NamespaceName ] { DesignatorItem }")]
        private DesignatorStatement ParseDesignator(IExtendableSyntaxPart parent) {
            DesignatorStatement result = CreateChild<DesignatorStatement>(parent);
            result.AddressOf = ContinueWith(result, TokenKind.At);
            result.Inherited = ContinueWith(result, TokenKind.Inherited);
            if (MatchIdentifier(TokenKind.String, TokenKind.ShortString, TokenKind.AnsiString, TokenKind.WideString, TokenKind.String)) {
                result.Name = ParseTypeName(result);
            }

            ISyntaxPart item;
            var first = true;
            do {
                item = ParseDesignatorItem(result, first && result.Name != null);
                first = false;
            } while (item != null);

            return result;
        }

        [Rule("DesignatorItem", "'^' | '.' Ident [GenericSuffix] | '[' ExpressionList ']' | '(' [ FormattedExpression  { ',' FormattedExpression } ] ')'")]
        private ISyntaxPart ParseDesignatorItem(SyntaxPartBase parent, bool hasIdentifier) {
            if (Match(TokenKind.Circumflex)) {
                DesignatorItem result = CreateByTerminal<DesignatorItem>(parent, TokenKind.Circumflex);
                result.Dereference = true;
                return result;
            }

            if (Match(TokenKind.Dot)) {
                DesignatorItem result = CreateByTerminal<DesignatorItem>(parent, TokenKind.Dot);
                result.Subitem = RequireIdentifier(result);

                if (Match(TokenKind.AngleBracketsOpen) &&
                    LookAheadIdentifier(1, new[] { TokenKind.String, TokenKind.ShortString, TokenKind.WideString, TokenKind.UnicodeString, TokenKind.AnsiString, TokenKind.Pointer }, false)) {
                    Tuple<bool, int> whereCloseBrackets = HasTokenUntilToken(new[] { TokenKind.AngleBracketsClose }, TokenKind.Identifier, TokenKind.Dot, TokenKind.Comma, TokenKind.AngleBracketsOpen, TokenKind.String, TokenKind.ShortString, TokenKind.WideString, TokenKind.UnicodeString, TokenKind.AnsiString, TokenKind.Pointer);
                    if (whereCloseBrackets.Item1 && (!LookAheadIdentifier(1 + whereCloseBrackets.Item2, new[] { TokenKind.HexNumber, TokenKind.Integer, TokenKind.Real }, false) || LookAhead(1 + whereCloseBrackets.Item2, TokenKind.Read, TokenKind.Write, TokenKind.ReadOnly, TokenKind.WriteOnly, TokenKind.Add, TokenKind.Remove, TokenKind.DispId))) {
                        result.SubitemGenericType = ParseGenericSuffix(result);
                    }
                }

                return result;
            }

            if (Match(TokenKind.OpenBraces)) {
                DesignatorItem result = CreateByTerminal<DesignatorItem>(parent, TokenKind.OpenBraces);
                result.IndexExpression = ParseExpressions(result);
                ContinueWithOrMissing(result, TokenKind.CloseBraces);
                return result;
            }

            if (Match(TokenKind.OpenParen)) {
                if (LookAheadIdentifier(1, new int[0], true) && LookAhead(2, TokenKind.Colon)) {
                    DesignatorItem prevDesignatorItem = parent.PartList.Count > 0 ? parent.PartList[parent.PartList.Count - 1] as DesignatorItem : null;
                    if (!hasIdentifier && ((prevDesignatorItem == null) || (prevDesignatorItem.Subitem == null))) {
                        return ParseConstantExpression(parent);
                    }
                }

                DesignatorItem result = CreateByTerminal<DesignatorItem>(parent, TokenKind.OpenParen);
                if (!Match(TokenKind.CloseParen)) {
                    do {
                        Parameter parameter = CreateChild<Parameter>(result);

                        if (MatchIdentifier(true) && LookAhead(1, TokenKind.Assignment)) {
                            parameter.ParameterName = RequireIdentifier(parameter, true);
                            ContinueWithOrMissing(parameter, TokenKind.Assignment);
                        }

                        if (!Match(TokenKind.Comma))
                            parameter.Expression = ParseFormattedExpression(parameter);

                    } while (ContinueWith(result, TokenKind.Comma));
                }
                ContinueWithOrMissing(result, TokenKind.CloseParen);
                return result;
            }

            return null;
        }

        [Rule("FormattedExpression", "Expression [ ':' Expression [ ':' Expression ] ]")]
        private FormattedExpression ParseFormattedExpression(IExtendableSyntaxPart parent) {
            FormattedExpression result = CreateChild<FormattedExpression>(parent);
            result.Expression = ParseExpression(result);

            if (ContinueWith(result, TokenKind.Colon)) {
                result.Width = ParseExpression(result);
                if (ContinueWith(result, TokenKind.Colon)) {
                    result.Decimals = ParseExpression(result);
                }
            }
            return result;
        }

        [Rule("SetSection", "'[' [ Expression ] { (',' | '..') Expression } ']'")]
        private SetSection ParseSetSection(IExtendableSyntaxPart parent) {
            SetSection result = CreateByTerminal<SetSection>(parent, TokenKind.OpenBraces);


            if (!Match(TokenKind.CloseBraces)) {
                SetSectnPart part;
                do {
                    part = CreateChild<SetSectnPart>(result);

                    if (ContinueWith(result, TokenKind.Comma, TokenKind.DotDot))
                        part.Continuation = result.LastTerminalKind;
                    else
                        part.Continuation = TokenKind.Undefined;

                    part.SetExpression = ParseExpression(result);

                } while (Match(TokenKind.Comma, TokenKind.DotDot));
            }

            ContinueWithOrMissing(result, TokenKind.CloseBraces);
            return result;
        }

        [Rule("ClosureExpr", "('function'|'procedure') [ FormalParameterSection ] [ ':' TypeSpecification ] Block ")]
        private ClosureExpression ParseClosureExpression(IExtendableSyntaxPart parent) {
            ClosureExpression result = CreateByTerminal<ClosureExpression>(parent, TokenKind.Function, TokenKind.Procedure);
            result.Kind = result.LastTerminalKind;

            if (Match(TokenKind.OpenParen)) {
                result.Parameters = ParseFormalParameterSection(result);
            }

            if (result.Kind == TokenKind.Function) {
                ContinueWithOrMissing(result, TokenKind.Colon);
                result.ReturnType = ParseTypeSpecification(result);
            }
            result.Block = ParseBlock(result);
            return result;
        }

        private StandardInteger RequireInteger(IExtendableSyntaxPart parent)
            => CreateByTerminal<StandardInteger>(parent, TokenKind.Integer);

        private HexNumber RequireHexValue(IExtendableSyntaxPart parent)
            => CreateByTerminal<HexNumber>(parent, TokenKind.HexNumber);

        private RealNumber RequireRealValue(IExtendableSyntaxPart parent)
            => CreateByTerminal<RealNumber>(parent, TokenKind.Real);

        private Identifier RequireIdentifier(IExtendableSyntaxPart parent, bool allowReserverdWords = false) {

            //if (CurrentToken().Value == "SQLITE_OPEN_SHAREDCACHE")
            //    System.Diagnostics.Debugger.Break();

            if (Match(TokenKind.Identifier)) {
                return CreateByTerminal<Identifier>(parent, TokenKind.Identifier);
            };

            Token token = CurrentToken();

            if (token != null && (allowReserverdWords || !reservedWords.Contains(token.Kind))) {
                return CreateByTerminal<Identifier>(parent, token.Kind);
            }

            ContinueWithOrMissing(parent, TokenKind.Identifier);
            return null;
        }

        private QuotedString RequireString(IExtendableSyntaxPart parent) {
            QuotedString result = CreateByTerminal<QuotedString>(parent, TokenKind.QuotedString);
            result.UnquotedValue = QuotedStringTokenValue.Unwrap(result.LastTerminalToken);
            return result;
        }

        private QuotedString RequireDoubleQuotedString(IExtendableSyntaxPart parent) {
            QuotedString result = CreateByTerminal<QuotedString>(parent, TokenKind.DoubleQuotedString);
            result.UnquotedValue = result.LastTerminalValue;
            return result;
        }

        private NamespaceName ParseNamespaceName(IExtendableSyntaxPart parent) {
            NamespaceName result = CreateChild<NamespaceName>(parent);

            if (!ContinueWith(result, TokenKind.AnsiString, TokenKind.String, TokenKind.WideString, TokenKind.ShortString, TokenKind.UnicodeString))
                RequireIdentifier(result);

            while (LookAheadIdentifier(1, new int[0], true) && ContinueWith(result, TokenKind.Dot)) {
                RequireIdentifier(result, true);
            }

            return result;
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
            => LookAheadIdentifier(0, new int[0], allowReservedWords);

        private bool LookAheadIdentifier(int lookAhead, int[] otherTokens, bool allowReservedWords) {
            if (LookAhead(lookAhead, otherTokens))
                return true;

            if (LookAhead(lookAhead, TokenKind.Identifier))
                return true;

            Token token = Tokenizer.LookAhead(lookAhead);

            if (token == null)
                return false;

            if (!allowReservedWords && reservedWords.Contains(token.Kind))
                return false;

            return StandardTokenizer.Keywords.ContainsKey(token.Value);
        }



    }
}