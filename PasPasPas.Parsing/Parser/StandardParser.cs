using PasPasPas.Api;
using System.Text;
using System;
using System.Collections.Generic;
using PasPasPas.Parsing.Tokenizer;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.SyntaxTree.ObjectPascal;

namespace PasPasPas.Parsing.Parser {

    /// <summary>
    ///     standard, recursive descend pascal paser
    /// </summary>
    public class StandardParser : ParserBase, IParser, IParserInformationProvider {

        /// <summary>
        ///     creates a new standard parser
        /// </summary>
        public StandardParser(ParserServices environment) :
            base(environment, new PascalTokenizerWithLookahead(environment)) { }

        private static readonly HashSet<int> reservedWords
            = new HashSet<int>() {
            PascalToken.And,
            TokenKind.Array,
            PascalToken.As,
            PascalToken.Asm,
            TokenKind.Begin,
            PascalToken.Case,
            TokenKind.Class,
            TokenKind.Const,
            TokenKind.Constructor,
            TokenKind.Destructor,
            PascalToken.DispInterface,
            PascalToken.Div,
            PascalToken.Do,
            PascalToken.DownTo,
            PascalToken.Else,
            TokenKind.End,
            PascalToken.Except,
            PascalToken.Exports,
            TokenKind.File,
            PascalToken.Finalization   ,
            PascalToken.Finally,
            PascalToken.For,
            TokenKind.Function,
            PascalToken.GoToKeyword,
            PascalToken.If,
            PascalToken.Implementation,
            TokenKind.In,
            PascalToken.Inherited,
            PascalToken.Initialization,
            TokenKind.Inline,
            PascalToken.Interface,
            PascalToken.Is,
            PascalToken.Label,
            TokenKind.Library,
            PascalToken.Mod,
            PascalToken.Nil,
            PascalToken.Not,
            PascalToken.Object,
            TokenKind.Of,
            PascalToken.Or,
            TokenKind.Packed,
            TokenKind.Procedure,
            TokenKind.Program,
            PascalToken.Property,
            PascalToken.Raise,
            TokenKind.Record,
            PascalToken.Repeat,
            TokenKind.Resourcestring,
            TokenKind.Set,
            PascalToken.Shl,
            PascalToken.Shr,
            PascalToken.String,
            PascalToken.Then,
            PascalToken.ThreadVar,
            PascalToken.To,
            PascalToken.Try,
            TokenKind.TypeKeyword,
            TokenKind.Unit,
            PascalToken.Until,
            TokenKind.Uses,
            TokenKind.Var,
            PascalToken.While,
            PascalToken.With,
            PascalToken.Xor
            };

        private bool MatchIdentifier(params int[] otherTokens) {
            if (Match(otherTokens))
                return true;

            if (Match(PascalToken.Identifier))
                return true;

            var token = CurrentToken();

            if (reservedWords.Contains(token.Kind))
                return false;

            return StandardTokenizer.Keywords.ContainsKey(token.Value);
        }


        /// <summary>
        ///     parse input
        /// </summary>
        public override ISyntaxPart Parse()
            => ParseFile();

        [Rule("File", "Program | Library | Unit | Package")]
        private ISyntaxPart ParseFile() {
            if (Match(TokenKind.Library)) {
                return ParseLibrary();
            }
            else if (Match(TokenKind.Unit)) {
                return ParseUnit();
            }
            else if (Match(TokenKind.Package)) {
                return ParsePackage();
            }

            return ParseProgram();
        }

        [Rule("Unit", "UnitHead UnitInterface UnitImplementation UnitBlock '.' ")]
        private Unit ParseUnit() {
            var result = new Unit(this);
            result.UnitHead = ParseUnitHead();
            result.UnitInterface = ParseUnitInterface();
            result.UnitImplementation = ParseUnitImplementation();
            result.UnitBlock = ParseUnitBlock();
            return result;
        }

        [Rule("UnitImplementation", "'implementation' [ UsesClause ] DeclarationSections", true)]
        private UnitImplementation ParseUnitImplementation() {
            var result = new UnitImplementation(this);
            Require(PascalToken.Implementation);
            if (Match(TokenKind.Uses)) {
                result.UsesClause = ParseUsesClause();
            }
            result.DeclarationSections = ParseDeclarationSections();
            return result;
        }

        [Rule("UsesClause", "'uses' NamespaceNameList")]
        private UsesClause ParseUsesClause() {
            var result = new UsesClause(this);
            Require(TokenKind.Uses);
            result.UsesList = ParseNamespaceNameList();
            return result;
        }

        [Rule("UnitInterface", "'interface' [ UsesClause ] InterfaceDeclaration ")]
        private UnitInterface ParseUnitInterface() {
            var result = new UnitInterface(this);
            Require(PascalToken.Interface);
            if (Match(TokenKind.Uses)) {
                result.UsesClause = ParseUsesClause();
            }

            result.InterfaceDeclaration = ParseInterfaceDeclaration();
            return result;
        }

        [Rule("InterfaceDeclarationItem", "ConstSection | TypeSection | VarSection | ExportsSection | AssemblyAttribute | ExportedProcedureHeading")]
        private SyntaxPartBase ParseInterfaceDeclarationItem() {
            if (Match(TokenKind.Const)) {
                return ParseConstSection();
            }

            if (Match(TokenKind.TypeKeyword)) {
                return ParseTypeSection();
            }

            if (Match(TokenKind.Var)) {
                return ParseVarSection();
            }

            if (Match(PascalToken.Exports)) {
                return ParseExportsSection();
            }

            if (Match(TokenKind.OpenBraces) && LookAhead(1, PascalToken.Assembly)) {
                return ParseAssemblyAttribute();
            }

            if (Match(TokenKind.Procedure, TokenKind.Function)) {
                return ParseExportedProcedureHeading();
            }

            return null;
        }

        [Rule("InterfaceDeclaration", "{ InterfaceDeclarationItem }")]
        private InterfaceDeclaration ParseInterfaceDeclaration() {
            var result = new InterfaceDeclaration(this);
            SyntaxPartBase item;

            do {
                item = ParseInterfaceDeclarationItem();
                result.Add(item);
            } while (item != null);
            return result;
        }

        [Rule("ExportedProcedureHeading", "")]
        private ExportedProcedureHeading ParseExportedProcedureHeading() {
            var result = new ExportedProcedureHeading(this);
            result.Kind = Require(TokenKind.Function, TokenKind.Procedure).Kind;
            result.Name = RequireIdentifier();
            if (Optional(TokenKind.OpenParen)) {
                result.Parameters = ParseFormalParameterSection();
            }
            if (Optional(TokenKind.Colon)) {
                result.ResultAttributes = ParseAttributes();
                result.ResultType = ParseTypeSpecification();
            }
            Require(TokenKind.Semicolon);
            result.Directives = ParseFunctionDirectives();
            return result;
        }

        [Rule("FunctionDirectives", "{ FunctionDirective } ")]
        private FunctionDirectives ParseFunctionDirectives() {
            var result = new FunctionDirectives(this);
            SyntaxPartBase directive;
            do {
                directive = ParseFunctionDirective();
                result.Add(directive);
            } while (directive != null);
            return result;
        }

        [Rule("FunctionDirective", "OverloadDirective | InlineDirective | CallConvention | OldCallConvention | Hint | ExternalDirective | UnsafeDirective")]
        private SyntaxPartBase ParseFunctionDirective() {
            if (Match(TokenKind.Overload)) {
                return ParseOverloadDirective();
            }

            if (Match(TokenKind.Inline)) {
                return ParseInlineDirective();
            }

            if (Match(TokenKind.Cdecl, TokenKind.Pascal, TokenKind.Register, TokenKind.Safecall, TokenKind.Stdcall, TokenKind.Export)) {
                return ParseCallConvention();
            }

            if (Match(TokenKind.Far, TokenKind.Local, PascalToken.Near)) {
                return ParseOldCallConvention();
            }

            if (Match(TokenKind.Deprecated, TokenKind.Library, TokenKind.Experimental, TokenKind.Platform)) {
                return ParseHint();
            }

            if (Match(PascalToken.VarArgs, PascalToken.External)) {
                return ParseExternalDirective();
            }

            if (Match(PascalToken.Unsafe)) {
                return ParseUnsafeDirective();
            }

            if (Match(PascalToken.Forward)) {
                return ParseForwardDirective();
            }

            return null;
        }

        [Rule("ForwardDirective", "'forward' ';' ")]
        private ForwardDirective ParseForwardDirective() {
            var result = new ForwardDirective(this);
            Require(PascalToken.Forward);
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("UnsafeDirective", "'unsafe' ';' ")]
        private UnsafeDirective ParseUnsafeDirective() {
            var result = new UnsafeDirective(this);
            Require(PascalToken.Unsafe);
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("ExternalDirective", "(varargs | external [ ConstExpression { ExternalSpecifier } ]) ';' ")]
        private ExternalDirective ParseExternalDirective() {
            var result = new ExternalDirective(this);
            result.Kind = Require(PascalToken.VarArgs, PascalToken.External).Kind;

            if ((result.Kind == PascalToken.External) && (!Match(TokenKind.Semicolon))) {
                result.ExternalExpression = ParseConstantExpression();
                ExternalSpecifier specifier;
                do {
                    specifier = ParseExternalSpecifier();
                    result.Add(specifier);
                } while (specifier != null);
            }
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("ExternalSpecifier", "('Name' | 'Index' ) ConstExpression")]
        private ExternalSpecifier ParseExternalSpecifier() {
            if (!Match(PascalToken.Name, PascalToken.Index))
                return null;

            var result = new ExternalSpecifier(this);
            result.Kind = Require(PascalToken.Name, PascalToken.Index).Kind;
            result.Expression = ParseConstantExpression();
            return result;
        }

        private SyntaxPartBase ParseOldCallConvention() {
            var result = new OldCallConvention(this);
            result.Kind = Require(PascalToken.Near, TokenKind.Far, TokenKind.Local).Kind;
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("UnitBlock", "( UnitInitilization 'end' ) | CompoundStatement | 'end' ")]
        private UnitBlock ParseUnitBlock() {
            var result = new UnitBlock(this);

            if (Optional(TokenKind.End))
                return result;

            if (Match(TokenKind.Begin)) {
                result.MainBlock = ParseCompoundStatement();
                return result;
            }

            result.Initialization = ParseUnitInitialization();
            return result;
        }

        [Rule("UnitInitialization", "'initialization' StatementList [ UnitFinalization ]", true)]
        private UnitInitialization ParseUnitInitialization() {
            var result = new UnitInitialization(this);
            Require(PascalToken.Initialization);

            if (Match(PascalToken.Finalization)) {
                result.Finalization = ParseFinalization();
            }

            return result;
        }

        [Rule("UnitFinalization", "'finalization' StatementList", true)]
        private UnitFinalization ParseFinalization() {
            var result = new UnitFinalization(this);
            Require(PascalToken.Finalization);
            return result;
        }

        [Rule("CompoundStatement", "'begin' [ StatementList ] 'end'")]
        private CompoundStatement ParseCompoundStatement() {
            var result = new CompoundStatement(this);
            Require(TokenKind.Begin);
            if (!Match(TokenKind.End))
                result.Statements = ParseStatementList();
            Require(TokenKind.End);
            return result;
        }

        [Rule("StatementList", "[Statement], { ';' [Statement]}")]
        private StatementList ParseStatementList() {
            var result = new StatementList(this);
            do {
                result.Add(ParseStatement());
            } while (Optional(TokenKind.Semicolon));
            return result;
        }

        [Rule("Statement", "[ Label ':' ] StatementPart")]
        private Statement ParseStatement() {
            Label label = null;
            if (MatchIdentifier(PascalToken.Integer) && LookAhead(1, TokenKind.Colon)) {
                label = ParseLabel();
            }

            StatementPart part = ParseStatementPart();

            if (label != null && part == null) {
                Unexpected();
                return null;
            }

            var result = new Statement(this);
            result.Label = label;
            result.Part = part;
            return result;
        }

        [Rule("StatementPart", "IfStatement | CaseStatement | ReapeatStatement | WhileStatment | ForStatement | WithStatement | TryStatement | RaiseStatement | AsmStatement | CompoundStatement | SimpleStatement ")]
        private StatementPart ParseStatementPart() {
            var result = new StatementPart(this);
            if (Match(PascalToken.If)) {
                result.If = ParseIfStatement();
                return result;
            }
            if (Match(PascalToken.Case)) {
                result.Case = ParseCaseStatement();
                return result;
            }
            if (Match(PascalToken.Repeat)) {
                result.Reapeat = ParseRepeatStatement();
                return result;
            }
            if (Match(PascalToken.While)) {
                result.While = ParseWhileStatement();
                return result;
            }
            if (Match(PascalToken.For)) {
                result.For = ParseForStatement();
                return result;
            }
            if (Match(PascalToken.With)) {
                result.With = ParseWithStatement();
                return result;
            }
            if (Match(PascalToken.Try)) {
                result.Try = ParseTryStatement();
                return result;
            }
            if (Match(PascalToken.Raise)) {
                result.Raise = ParseRaiseStatement();
                return result;
            }
            if (Match(PascalToken.Asm)) {
                result.Asm = ParseAsmStatement();
                return result;
            }

            if (Match(TokenKind.Begin)) {
                result.CompundStatement = ParseCompoundStatement();
                return result;
            }

            return ParseSimpleStatement();
        }

        [Rule("RaiseStatement", "'raise' [ Designator ] [ 'at' Designator ]")]
        private RaiseStatement ParseRaiseStatement() {
            var result = new RaiseStatement(this);
            Require(PascalToken.Raise);

            if ((!Match(TokenKind.At)) && MatchIdentifier(PascalToken.Inherited)) {
                result.Raise = ParseDesignator();
            }

            if (Optional(TokenKind.At)) {
                result.At = ParseDesignator();
            }

            return result;
        }

        private AsmStatement ParseAsmStatement() {
            throw new NotImplementedException();
        }

        [Rule("TryStatement", "'try' StatementList  ('except' HandlerList | 'finally' StatementList) 'end'")]
        private TryStatement ParseTryStatement() {
            var result = new TryStatement(this);
            Require(PascalToken.Try);
            result.Try = ParseStatementList();

            if (Optional(PascalToken.Except)) {
                result.Handlers = ParseExceptHandlers();
                Require(TokenKind.End);
            }
            else if (Optional(PascalToken.Finally)) {
                result.Finally = ParseStatementList();
                Require(TokenKind.End);
            }
            else {
                Unexpected();
            }

            return result;
        }

        [Rule("ExceptHandlers", "({ Handler } [ 'else' StatementList ]) | StatementList")]
        private ExceptHandlers ParseExceptHandlers() {
            var result = new ExceptHandlers(this);

            if (Match(PascalToken.On, PascalToken.Else)) {
                while (Match(PascalToken.On)) {
                    result.Add(ParseExceptHandler());
                }
                if (Optional(PascalToken.Else)) {
                    result.ElseStatements = ParseStatementList();
                }
            }
            else {
                result.Statements = ParseStatementList();
            }

            return result;
        }

        [Rule("ExceptHandler", "'on' Identifier ':' NamespaceName 'do' Statement ';'")]
        private ExceptHandler ParseExceptHandler() {
            var result = new ExceptHandler(this);
            Require(PascalToken.On);
            result.Name = RequireIdentifier();
            Require(TokenKind.Colon);
            result.HandlerType = ParseNamespaceName();
            Require(PascalToken.Do);
            result.Statement = ParseStatement();
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("WithStatement", "'with' Designator { ',' Designator }  'do' Statement")]
        private WithStatement ParseWithStatement() {
            var result = new WithStatement(this);
            Require(PascalToken.With);
            do {
                result.Add(ParseDesignator());
            } while (Optional(TokenKind.Comma));
            Require(PascalToken.Do);
            result.Statement = ParseStatement();
            return result;
        }

        [Rule("ForStatement", "('for' Designator ':=' Expression ('to' | 'downto' )  Expression 'do' Statement) | ('for' Deisgnator 'in' Expression  'do' Statement)")]
        private ForStatement ParseForStatement() {
            var result = new ForStatement(this);
            Require(PascalToken.For);
            result.Variable = ParseDesignator();
            if (Optional(PascalToken.Assignment)) {
                result.StartExpression = ParseExpression();
                result.Kind = Require(PascalToken.To, PascalToken.DownTo).Kind;
                result.EndExpression = ParseExpression();
            }
            else {
                result.Kind = Require(TokenKind.In).Kind;
                result.StartExpression = ParseExpression();
            }
            Require(PascalToken.Do);
            result.Statement = ParseStatement();
            return result;
        }

        [Rule("WhileStatement", "'while' Expression 'do' Statement")]
        private WhileStatement ParseWhileStatement() {
            var result = new WhileStatement(this);
            Require(PascalToken.While);
            result.Condition = ParseExpression();
            Require(PascalToken.Do);
            result.Statement = ParseStatement();
            return result;
        }

        [Rule("RepeatStatement", "'repeat' [ StatementList ] 'until' Expression")]
        private RepeatStatement ParseRepeatStatement() {
            var result = new RepeatStatement(this);
            Require(PascalToken.Repeat);
            if (!Match(PascalToken.Until)) {
                result.Statements = ParseStatementList();
            }
            Require(PascalToken.Until);
            result.Condition = ParseExpression();
            return result;
        }

        [Rule("CaseStatement", "'case' Expression 'of' { CaseItem } ['else' StatementList[';']] 'end' ")]
        private CaseStatement ParseCaseStatement() {
            var result = new CaseStatement(this);
            Require(PascalToken.Case);
            result.CaseExpression = ParseExpression();
            Require(TokenKind.Of);
            CaseItem item;
            do {
                item = ParseCaseItem();
                result.Add(item);
            } while (item != null);

            if (Optional(PascalToken.Else)) {
                result.Else = ParseStatementList();
                Optional(TokenKind.Semicolon);
            }
            Require(TokenKind.End);
            return result;
        }

        [Rule("CaseItem", "CaseLabel { ',' CaseLabel } ':' Statement [';']")]
        private CaseItem ParseCaseItem() {
            if (Match(PascalToken.Else, TokenKind.End))
                return null;

            var result = new CaseItem(this);
            do {
                result.Add(ParseCaseLabel());
            } while (Optional(TokenKind.Comma));
            Require(TokenKind.Colon);
            result.CaseStatement = ParseStatement();
            Optional(TokenKind.Semicolon);
            return result;
        }

        [Rule("CaseLabel", "Expression [ '..' Expression ]")]
        private CaseLabel ParseCaseLabel() {
            var result = new CaseLabel(this);
            result.StartExpression = ParseExpression();
            if (Optional(TokenKind.DotDot))
                result.EndExpression = ParseExpression();
            return result;
        }

        [Rule("IfStatement", "'if' Expression 'then' Statement [ 'else' Statement ]")]
        private IfStatement ParseIfStatement() {
            var result = new IfStatement(this);
            Require(PascalToken.If);
            result.Condition = ParseExpression();
            Require(PascalToken.Then);
            result.ThenPart = ParseStatement();
            if (Optional(PascalToken.Else)) {
                result.ElsePart = ParseStatement();
            }
            return result;
        }

        [Rule("SimpleStatement", "GoToStatement | Designator [ ':=' (Expression  | NewStatement) ] ")]
        private StatementPart ParseSimpleStatement() {
            if (Match(PascalToken.GoToKeyword, PascalToken.Exit, PascalToken.Break, PascalToken.Continue)) {
                var result = new StatementPart(this);
                result.GoTo = ParseGoToStatement();
                return result;
            }

            if (MatchIdentifier(PascalToken.Inherited)) {
                var result = new StatementPart(this);
                result.DesignatorPart = ParseDesignator();

                if (Optional(PascalToken.Assignment)) {
                    result.Assignment = ParseExpression();
                }

                return result;
            }

            return null;
        }

        [Rule("GoToStatement", "('goto' Label) | 'break' | 'continue' | 'exit' '(' Expression ')' ")]
        private GoToStatement ParseGoToStatement() {
            var result = new GoToStatement(this);
            if (Optional(PascalToken.GoToKeyword)) {
                result.GoToLabel = ParseLabel();
                return result;
            }
            if (Optional(PascalToken.Break)) {
                result.Break = true;
                return result;
            }
            if (Optional(PascalToken.Continue)) {
                result.Continue = true;
                return result;
            }
            if (Optional(PascalToken.Exit)) {
                result.Exit = true;
                if (Optional(TokenKind.OpenParen)) {
                    result.ExitExpression = ParseExpression();
                    Require(TokenKind.CloseParen);
                }
                return result;
            }

            Unexpected();
            return null;
        }

        [Rule("UnitHead", "'unit' NamespaceName { Hint } ';' ")]
        private UnitHead ParseUnitHead() {
            var result = new UnitHead(this);
            Require(TokenKind.Unit);
            result.UnitName = ParseNamespaceName();
            result.Hint = ParseHints();
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("Package", "PackageHead RequiresClause [ ContainsClause ] 'end' '.' ")]
        private Package ParsePackage() {
            var result = new Package(this);
            result.PackageHead = ParsePackageHead();
            result.RequiresClause = ParseRequiresClause();

            if (Match(PascalToken.Contains)) {
                result.ContainsClause = ParseContainsClause();
            }

            Require(TokenKind.End);
            Require(TokenKind.Dot);

            return result;
        }

        [Rule("ContainsClause", "'contains' NamespaceFileNameList")]
        private PackageContains ParseContainsClause() {
            var result = new PackageContains(this);
            Require(PascalToken.Contains);
            result.ContainsList = ParseNamespaceFileNameList();
            return result;
        }

        [Rule("NamespaceFileNameList", "NamespaceFileName { ',' NamespaceFileName }")]
        private NamespaceFileNameList ParseNamespaceFileNameList() {
            var result = new NamespaceFileNameList(this);
            do {
                result.Add(ParseNamespaceFileName());
            } while (Optional(TokenKind.Comma));
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("RequiresClause", "'requires' NamespaceNameList")]
        private PackageRequires ParseRequiresClause() {
            var result = new PackageRequires(this);
            Require(PascalToken.Requires);
            result.RequiresList = ParseNamespaceNameList();
            return result;
        }

        [Rule("NamespaceNameList", "NamespaceName { ',' NamespaceName } ';' ")]
        private NamespaceNameList ParseNamespaceNameList() {
            var result = new NamespaceNameList(this);
            do {
                result.Add(ParseNamespaceName());
            } while (Optional(TokenKind.Comma));
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("PackageHead", "'package' NamespaceName ';' ")]
        private PackageHead ParsePackageHead() {
            var result = new PackageHead(this);
            Require(TokenKind.Package);
            result.PackageName = ParseNamespaceName();
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("Library", "LibraryHead [UsesFileClause] Block '.' ")]
        private Library ParseLibrary() {
            var result = new Library(this);
            result.LibraryHead = ParseLibraryHead();

            if (Match(TokenKind.Uses))
                result.Uses = ParseUsesFileClause();

            result.MainBlock = ParseBlock();
            Require(TokenKind.Dot);
            return result;
        }

        [Rule("LibraryHead", "'library' NamespaceName Hints ';'")]
        private LibraryHead ParseLibraryHead() {
            var result = new LibraryHead(this);
            Require(TokenKind.Library);
            result.Name = ParseNamespaceName();
            result.Hints = ParseHints();
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("Program", "[ProgramHead] [UsesFileClause] Block '.'")]
        private Program ParseProgram() {
            var result = new Program(this);

            if (Match(TokenKind.Program))
                result.ProgramHead = ParseProgramHead();

            if (Match(TokenKind.Uses))
                result.Uses = ParseUsesFileClause();

            result.MainBlock = ParseBlock();
            Require(TokenKind.Dot);
            return result;
        }

        [Rule("ProgramHead", "'program' NamespaceName [ProgramParams] ';'")]
        private ProgramHead ParseProgramHead() {
            var result = new ProgramHead(this);
            Require(TokenKind.Program);
            result.Name = ParseNamespaceName();
            result.Params = ParseProgramParams();
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("ProgramParams", "'(' [ Identifier { ',' Identifier } ] ')'")]
        private ProgramParameterList ParseProgramParams() {
            var result = new ProgramParameterList(this);

            if (Optional(TokenKind.OpenParen)) {

                if (MatchIdentifier()) {
                    result.Add(RequireIdentifier());

                    while (Optional(TokenKind.Comma))
                        result.Add(RequireIdentifier());
                }

                Require(TokenKind.CloseParen);
            }

            return result;
        }

        [Rule("Block", "DeclarationSections [ BlockBody ] ")]
        private Block ParseBlock() {
            var result = new Block(this);
            result.DeclarationSections = ParseDeclarationSections();
            if (Match(PascalToken.Asm, TokenKind.Begin)) {
                result.Body = ParseBlockBody();
            }
            return result;
        }

        [Rule("BlockBody", "AssemblerBlock | CompoundStatement")]
        private BlockBody ParseBlockBody() {
            var result = new BlockBody(this);

            if (Match(PascalToken.Asm)) {
                result.AssemblerBlock = ParseAsmStatement();
            }

            if (Match(TokenKind.Begin)) {
                result.Body = ParseCompoundStatement();
            }

            return result;
        }

        [Rule("DeclarationSection", "{ LabelDeclarationSection | ConstSection | TypeSection | VarSection | ExportsSection | AssemblyAttribute | MethodDecl | ProcedureDeclaration }", true)]
        private Declarations ParseDeclarationSections() {
            var result = new Declarations(this);
            bool stop = false;

            while (!stop) {

                if (Match(PascalToken.Label)) {
                    result.Add(ParseLabelDeclarationSection());
                    continue;
                }

                if (Match(TokenKind.Const, TokenKind.Resourcestring)) {
                    result.Add(ParseConstSection());
                    continue;
                }

                if (Match(TokenKind.TypeKeyword)) {
                    result.Add(ParseTypeSection());
                    continue;
                }

                if (Match(TokenKind.Var, PascalToken.ThreadVar)) {
                    result.Add(ParseVarSection());
                    continue;
                }

                if (Match(PascalToken.Exports)) {
                    result.Add(ParseExportsSection());
                    continue;
                }

                UserAttributes attrs = null;
                if (Match(TokenKind.OpenBraces)) {
                    if (LookAhead(1, PascalToken.Assembly)) {
                        result.Add(ParseAssemblyAttribute());
                        continue;
                    }
                    else {
                        attrs = ParseAttributes();
                    }
                }
                bool useClass = Optional(TokenKind.Class);

                if (Match(TokenKind.Function, TokenKind.Procedure, TokenKind.Constructor, TokenKind.Destructor, PascalToken.Operator)) {

                    bool useMethodDeclaration = //
                        useClass ||
                        Match(TokenKind.Constructor, TokenKind.Destructor, PascalToken.Operator) ||
                        (LookAhead(1, PascalToken.Identifier) && (LookAhead(2, TokenKind.Dot)));

                    if (useMethodDeclaration) {
                        var methodDecl = ParseMethodDecl();
                        methodDecl.Class = useClass;
                        methodDecl.Attributes = attrs;
                        result.Add(methodDecl);
                        continue;
                    }

                    result.Add(ParseProcedureDeclaration(attrs));
                    continue;
                }


                stop = true;
            }

            return result;
        }

        [Rule("MethodDecl", "MethodDeclHeading ';' MethodDirectives [ Block ';' ]")]
        private MethodDecl ParseMethodDecl() {
            var result = new MethodDecl(this);
            result.Heading = ParseMethodDeclHeading();
            Require(TokenKind.Semicolon);
            result.Directives = ParseMethodDirectives();
            result.MethodBody = ParseBlock();
            if ((result.MethodBody != null) && (result.MethodBody.Body != null))
                Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("MethodDirectives", "{ MethodDirective }")]
        private MethodDirectives ParseMethodDirectives() {
            var result = new MethodDirectives(this);
            SyntaxPartBase directive;
            do {
                directive = ParseMethodDirective();
                result.Add(directive);
            } while (directive != null);
            return result;
        }

        [Rule("MethodDirective", "ReintroduceDirective | OverloadDirective | InlineDirective | BindingDirective | AbstractDirective | InlineDirective | CallConvention | HintingDirective | DispIdDirective")]
        private SyntaxPartBase ParseMethodDirective() {

            if (Match(TokenKind.Reintroduce)) {
                return ParseReintroduceDirective();
            }

            if (Match(TokenKind.Overload)) {
                return ParseOverloadDirective();
            }

            if (Match(TokenKind.Inline, TokenKind.Assembler)) {
                return ParseInlineDirective();
            }

            if (Match(TokenKind.Message, TokenKind.Static, TokenKind.Dynamic, TokenKind.Override, TokenKind.Virtual)) {
                return ParseBindingDirective();
            }

            if (Match(TokenKind.Abstract, TokenKind.Final)) {
                return ParseAbstractDirective();
            }

            if (Match(TokenKind.Cdecl, TokenKind.Pascal, TokenKind.Register, TokenKind.Safecall, TokenKind.Stdcall, TokenKind.Export)) {
                return ParseCallConvention();
            }

            if (Match(TokenKind.Deprecated, TokenKind.Library, TokenKind.Experimental, TokenKind.Platform)) {
                return ParseHint();
            }

            if (Match(PascalToken.DispId)) {
                return ParseDispIdDirective();
            }

            return null;
        }

        [Rule("InlineDirective", "('inline' | 'assembler' ) ';'")]
        private SyntaxPartBase ParseInlineDirective() {
            var result = new InlineDirective(this);
            result.Kind = Require(TokenKind.Inline, TokenKind.Assembler).Kind;
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("CallConvention", "('cdecl' | 'pascal' | 'register' | 'safecall' | 'stdcall' | 'export') ';' ")]
        private SyntaxPartBase ParseCallConvention() {
            var result = new CallConvention(this);
            result.Kind = Require(TokenKind.Cdecl, TokenKind.Pascal, TokenKind.Register, TokenKind.Safecall, TokenKind.Stdcall, TokenKind.Export).Kind;
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("AbstractDirective", "('abstract' | 'final' ) ';' ")]
        private SyntaxPartBase ParseAbstractDirective() {
            var result = new AbstractDirective(this);
            result.Kind = Require(TokenKind.Abstract, TokenKind.Final).Kind;
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("BindingDirective", " ('message' Expression ) | 'static' | 'dynamic' | 'override' | 'virtual' ")]
        private SyntaxPartBase ParseBindingDirective() {
            var result = new BindingDirective(this);
            result.Kind = Require(TokenKind.Message, TokenKind.Static, TokenKind.Dynamic, TokenKind.Override, TokenKind.Virtual).Kind;
            if (result.Kind == TokenKind.Message) {
                result.MessageExpression = ParseExpression();
            }
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("OverloadDirective", "'overload' ';' ")]
        private SyntaxPartBase ParseOverloadDirective() {
            var result = new OverloadDirective(this);
            Require(TokenKind.Overload);
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("ReintroduceDirective", "'reintroduce' ';' ")]
        private SyntaxPartBase ParseReintroduceDirective() {
            var result = new ReintroduceDirective(this);
            Require(TokenKind.Reintroduce);
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("MethodDeclHeading", " ('constructor' | 'destructor' | 'function' | 'procedure') NamespaceName [GenericDefinition] [FormalParameterSection] [':' Attributes TypeSpecification ]")]
        private MethodDeclHeading ParseMethodDeclHeading() {
            var result = new MethodDeclHeading(this);
            result.Kind = Require(TokenKind.Constructor, TokenKind.Destructor, TokenKind.Function, TokenKind.Procedure).Kind;
            result.Name = ParseNamespaceName();
            if (Match(TokenKind.AngleBracketsOpen)) {
                result.GenericDefinition = ParseGenericDefinition();
            }
            if (Match(TokenKind.OpenParen)) {
                result.Parameters = ParseFormalParameterSection();
            }
            if (Optional(TokenKind.Colon)) {
                result.ResultTypeAttributes = ParseAttributes();
                result.ResultType = ParseTypeSpecification();
            }
            return result;
        }

        [Rule("ProcedureDeclaration", "ProcedureDeclarationHeading ';' FunctionDirectives [ ProcBody ]")]
        private ProcedureDeclaration ParseProcedureDeclaration(UserAttributes attributes) {
            var result = new ProcedureDeclaration(this);
            result.Attributes = attributes;
            result.Heading = ParseProcedureDeclarationHeading();
            Require(TokenKind.Semicolon);
            result.Directives = ParseFunctionDirectives();
            result.ProcBody = ParseBlock();
            if ((result.ProcBody != null) && (result.ProcBody.Body != null))
                Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("ProcedureDeclarationHeading", "('procedure' | 'function') Identifier [FormalParameterSection][':' TypeSpecification]")]
        private ProcedureDeclarationHeading ParseProcedureDeclarationHeading() {
            var result = new ProcedureDeclarationHeading(this);
            result.Kind = Require(TokenKind.Function, TokenKind.Procedure).Kind;
            result.Name = RequireIdentifier();
            if (Match(TokenKind.OpenParen)) {
                result.Parameters = ParseFormalParameterSection();
            }
            if (Optional(TokenKind.Colon)) {
                result.ResultTypeAttributes = ParseAttributes();
                result.ResultType = ParseTypeSpecification();
            }
            return result;
        }

        [Rule("AssemblyAttribute", "'[' 'assembly' ':' ']'")]
        private AssemblyAttribute ParseAssemblyAttribute() {
            var result = new AssemblyAttribute(this);
            Require(TokenKind.OpenBraces);
            Require(PascalToken.Assembly);
            Require(TokenKind.Colon);
            result.Attribute = ParseAttribute();
            Require(TokenKind.CloseBraces);
            return result;
        }

        [Rule("ExportsSection", "'exports' Identifier ExportItem { ',' ExportItem } ';' ")]
        private ExportsSection ParseExportsSection() {
            var result = new ExportsSection(this);
            Require(PascalToken.Exports);
            result.ExportName = RequireIdentifier();

            do {
                result.Add(ParseExportItem());
            } while (Optional(TokenKind.Comma));
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("ExportItem", "[ '(' FormalParameters ')' ] [ 'index' Expression ] [ 'name' Expression ]")]
        private ExportItem ParseExportItem() {
            var result = new ExportItem(this);
            if (Optional(TokenKind.OpenParen)) {
                result.Parameters = ParseFormalParameters();
                Require(TokenKind.CloseParen);
            }

            if (Optional(PascalToken.Index)) {
                result.IndexParameter = ParseExpression();
            }

            if (Optional(PascalToken.Name)) {
                result.NameParameter = ParseExpression();
            }

            result.Resident = Optional(PascalToken.Resident);
            return result;
        }

        [Rule("VarSection", "(var | threadvar) VarDeclaration { VarDeclaration }")]
        private VarSection ParseVarSection() {
            var result = new VarSection(this);
            result.Kind = Require(TokenKind.Var, PascalToken.ThreadVar).Kind;

            do {
                result.Add(ParseVarDeclaration());
            } while (MatchIdentifier(TokenKind.OpenBraces));

            return result;
        }

        [Rule("VarDeclaration", "[ Attributes ] IdentList ':' TypeSpecification [ VarValueSpecification ] Hints ';' ")]
        private VarDeclaration ParseVarDeclaration() {
            var result = new VarDeclaration(this);
            result.Attributes = ParseAttributes();
            result.Identifiers = ParseIdentList();
            Require(TokenKind.Colon);
            result.TypeDeclaration = ParseTypeSpecification();

            if (Match(PascalToken.Absolute, TokenKind.EqualsSign)) {
                result.ValueSpecification = ParseValueSpecification();
            }

            result.Hints = ParseHints();
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("VarValueSpecification", "('absolute' ConstExpression) | ('=' ConstExpression)")]
        private VarValueSpecification ParseValueSpecification() {
            var result = new VarValueSpecification(this);
            if (Optional(PascalToken.Absolute)) {
                result.Absolute = ParseConstantExpression();
                return result;
            }

            Require(TokenKind.EqualsSign);
            result.InitialValue = ParseConstantExpression();
            return result;
        }

        [Rule("LabelSection", "'label' Label { ',' Label } ';' ")]
        private LabelDeclarationSection ParseLabelDeclarationSection() {
            var result = new LabelDeclarationSection(this);
            Require(PascalToken.Label);
            do {
                result.Add(ParseLabel());
            } while (Optional(TokenKind.Comma));
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("Label", "Identifier | Integer")]
        private Label ParseLabel() {
            var result = new Label(this);

            if (MatchIdentifier()) {
                result.LabelName = RequireIdentifier();
            }
            else if (Match(PascalToken.Integer)) {
                result.LabelName = RequireInteger();
            }
            else {
                Unexpected();
            }

            return result;
        }

        [Rule("ConstSection", "('const' | 'resourcestring') ConstDeclaration { ConstDeclaration }")]
        private ConstSection ParseConstSection() {
            var result = new ConstSection(this);
            result.Kind = Require(TokenKind.Const, TokenKind.Resourcestring).Kind;
            while (MatchIdentifier(TokenKind.OpenBraces)) {
                result.Add(ParseConstDeclaration());
            }
            return result;
        }

        [Rule("ConstDeclaration", "[Attributes] Identifier [ ':' TypeSpecification ] = ConstantExpression Hints';'")]
        private ConstDeclaration ParseConstDeclaration() {
            var result = new ConstDeclaration(this);
            result.Attributes = ParseAttributes();
            result.Identifier = RequireIdentifier();

            if (Optional(TokenKind.Colon)) {
                result.TypeSpecification = ParseTypeSpecification();
            }

            Require(TokenKind.EqualsSign);
            result.Value = ParseConstantExpression();
            result.Hint = ParseHints();
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("Hints", " { Hint }")]
        private HintingInformationList ParseHints() {
            var result = new HintingInformationList(this);
            HintingInformation hint;
            do {
                hint = ParseHint();
                result.Add(hint);
            } while (hint != null);
            return result;
        }

        [Rule("Hint", " ('deprecated' [QuotedString] | 'experimental' | 'platform' | 'library' ) ")]
        private HintingInformation ParseHint() {
            var result = new HintingInformation(this);

            if (Optional(TokenKind.Deprecated)) {
                result.Deprecated = true;
                if (Match(PascalToken.QuotedString))
                    result.DeprecatedComment = RequireString();

                return result;
            }

            if (Optional(TokenKind.Experimental)) {
                result.Experimental = true;
                return result;
            }

            if (Optional(TokenKind.Platform)) {
                result.Platform = true;
                return result;
            }

            if (Optional(TokenKind.Library)) {
                result.Library = true;
                return result;
            }

            return null;
        }

        [Rule("TypeSpecification", "StructType | PointerType | StringType | ProcedureType | SimpleType ")]
        private TypeSpecification ParseTypeSpecification() {
            var result = new TypeSpecification(this);

            if (Match(TokenKind.Packed, TokenKind.Array, TokenKind.Set, TokenKind.File, //
                TokenKind.Class, PascalToken.Interface, TokenKind.Record, PascalToken.Object)) {
                result.StructuredType = ParseStructType();
                return result;
            }

            if (Match(TokenKind.Pointer, TokenKind.Circumflex)) {
                result.PointerType = ParsePointerType();
                return result;
            }

            if (Match(PascalToken.String, PascalToken.ShortString, PascalToken.AnsiString, PascalToken.UnicodeString, PascalToken.WideString)) {
                result.StringType = ParseStringType();
                return result;
            }

            if (Match(TokenKind.Function, TokenKind.Procedure, PascalToken.Reference)) {
                result.ProcedureType = ParseProcedureType();
                return result;
            }

            result.SimpleType = ParseSimpleType();
            return result;
        }

        [Rule("SimpleType", "EnumType | (ConstExpression [ '..' ConstExpression ]) | ([ 'type' ] NamespaceName [ GenericPostix ])")]
        private SimpleType ParseSimpleType() {
            var result = new SimpleType(this);

            if (Match(TokenKind.OpenParen)) {
                result.EnumType = ParseEnumType();
                return result;
            }

            result.NewType = Optional(TokenKind.TypeKeyword);

            if (result.NewType || (MatchIdentifier() && (!LookAhead(1, TokenKind.DotDot)))) {
                result.TypeId = ParseNamespaceName();
                if (Match(TokenKind.AngleBracketsOpen)) {
                    result.GenericPostfix = ParseGenericSuffix();
                }
                return result;
            }

            result.SubrangeStart = ParseConstantExpression();
            if (Optional(TokenKind.DotDot)) {
                result.SubrangeEnd = ParseConstantExpression();
            }

            return result;
        }

        [Rule("EnumType", "'(' EnumTypeValue { ',' EnumTypeValue } ')'")]
        private EnumTypeDefinition ParseEnumType() {
            var result = new EnumTypeDefinition(this);
            Require(TokenKind.OpenParen);
            do {
                result.Add(ParseEnumTypeValue());
            } while (Optional(TokenKind.Comma));
            Require(TokenKind.CloseParen);
            return result;
        }

        [Rule("EnumTypeValue", "Identifier [ '=' Expression ]")]
        private EnumValue ParseEnumTypeValue() {
            var result = new EnumValue(this);
            result.EnumName = RequireIdentifier();
            if (Optional(TokenKind.EqualsSign)) {
                result.Value = ParseExpression();
            }
            return result;
        }

        [Rule("ProcedureType", "(ProcedureRefType [ 'of' 'object' ] ( | ProcedureReference")]
        private ProcedureType ParseProcedureType() {
            var result = new ProcedureType(this);

            if (Match(TokenKind.Procedure, TokenKind.Function)) {
                result.ProcedureRefType = ParseProcedureRefType();

                if (Optional(TokenKind.Of)) {
                    Require(PascalToken.Object);
                    result.ProcedureRefType.MethodDeclaration = true;
                }

                return result;
            }

            if (Match(PascalToken.Reference)) {
                result.ProcedureReference = ParseProcedureReference();
                return result;
            }

            Unexpected();
            return result;
        }

        [Rule("ProcedureReference", "'reference' 'to' ProcedureTypeDefinition ")]
        private ProcedureReference ParseProcedureReference() {
            var result = new ProcedureReference(this);
            Require(PascalToken.Reference);
            Require(PascalToken.To);
            result.ProcedureType = ParseProcedureRefType();
            return result;
        }

        [Rule("ProcedureTypeDefinition", "('function' | 'procedure') [ '(' FormalParameters ')' ] [ ':' TypeSpecification ] [ 'of' 'object']")]
        private ProcedureTypeDefinition ParseProcedureRefType() {
            var result = new ProcedureTypeDefinition(this);
            result.Kind = Require(TokenKind.Function, TokenKind.Procedure).Kind;
            if (Match(TokenKind.OpenParen)) {
                result.Parameters = ParseFormalParameterSection();
            };

            if (result.Kind == TokenKind.Function) {
                Require(TokenKind.Colon);
                result.ReturnTypeAttributes = ParseAttributes();
                result.ReturnType = ParseTypeSpecification();
            }

            return result;
        }

        [Rule("FormalParameterSection", "'(' [ FormalParameters ] ')'")]
        private FormalParameterSection ParseFormalParameterSection() {
            var result = new FormalParameterSection(this);
            Require(TokenKind.OpenParen);
            if (!Match(TokenKind.CloseParen)) {
                result.ParameterList = ParseFormalParameters();
            }
            Require(TokenKind.CloseParen);
            return result;
        }

        [Rule("StringType", "ShortString | WideString | UnicodeString |('string' [ '[' Expression ']'  ]) | ('AnsiString' '(' ConstExpression ')') ")]
        private StringType ParseStringType() {
            var result = new StringType(this);

            if (Optional(PascalToken.String)) {
                result.Kind = PascalToken.String;
                if (Optional(TokenKind.OpenBraces)) {
                    result.StringLength = ParseExpression();
                    Require(TokenKind.CloseBraces);
                };
                return result;
            }

            if (Optional(PascalToken.AnsiString)) {
                result.Kind = PascalToken.AnsiString;
                if (Optional(TokenKind.OpenParen)) {
                    result.CodePage = ParseConstantExpression();
                    Require(TokenKind.CloseParen);
                }
                return result;
            }

            if (Optional(PascalToken.ShortString)) {
                result.Kind = PascalToken.ShortString;
                return result;
            }

            if (Optional(PascalToken.WideString)) {
                result.Kind = PascalToken.WideString;
                return result;
            }

            if (Optional(PascalToken.UnicodeString)) {
                result.Kind = PascalToken.UnicodeString;
                return result;
            }

            Unexpected();
            return result;
        }


        [Rule("StructType", "[ 'packed' ] StructTypePart")]
        private StructType ParseStructType() {
            var result = new StructType(this);
            result.Packed = Optional(TokenKind.Packed);
            result.Part = ParseStructTypePart();
            return result;
        }

        [Rule("StructTypePart", "ArrayType | SetType | FileType | ClassDecl")]
        private StructTypePart ParseStructTypePart() {
            var result = new StructTypePart(this);

            if (Match(TokenKind.Array)) {
                result.ArrayType = ParseArrayType();
                return result;
            }

            if (Match(TokenKind.Set)) {
                result.SetType = ParseSetDefinition();
                return result;
            }

            if (Match(TokenKind.File)) {
                result.FileType = ParseFileType();
                return result;
            }

            if (Match(TokenKind.Class, PascalToken.Interface, TokenKind.Record, PascalToken.Object)) {
                result.ClassDecl = ParseClassDeclaration();
                return result;
            }

            return result;
        }

        [Rule("ClassDeclaration", "ClassOfDeclaration | ClassDefinition | ClassHelper | InterfaceDef | ObjectDecl | RecordDecl | RecordHelperDecl ")]
        private ClassTypeDeclaration ParseClassDeclaration() {
            var result = new ClassTypeDeclaration(this);

            if (Match(TokenKind.Class) && LookAhead(1, TokenKind.Of)) {
                result.ClassOf = ParseClassOfDeclaration();
                return result;
            }

            if (Match(TokenKind.Class) && LookAhead(1, PascalToken.Helper)) {
                result.ClassHelper = ParseClassHelper();
                return result;
            }

            if (Match(TokenKind.Class)) {
                result.ClassDef = ParseClassDefinition();
                return result;
            }

            if (Match(PascalToken.Interface, PascalToken.DispInterface)) {
                result.InterfaceDef = ParseInterfaceDef();
                return result;
            }

            if (Match(PascalToken.Object)) {
                result.ObjectDecl = ParseObjectDecl();
                return result;
            }

            if (Match(TokenKind.Record) && LookAhead(1, PascalToken.Helper)) {
                result.RecordHelper = ParseRecordHelper();
                return result;
            }

            if (Match(TokenKind.Record)) {
                result.RecordDecl = ParseRecordDecl();
                return result;
            }

            Unexpected();
            return result;
        }

        [Rule("RecordDecl", "'record' RecordFieldList (RecordVariantSection | RecordItems ) 'end' ")]
        private RecordDeclaration ParseRecordDecl() {
            var result = new RecordDeclaration(this);
            Require(TokenKind.Record);

            if (MatchIdentifier()) {
                result.FieldList = ParseFieldList();
            }

            if (Match(PascalToken.Case)) {
                result.VariantSection = ParseRecordVariantSection();
            }
            else {
                result.Items = ParseRecordItems();
            }
            Require(TokenKind.End);
            return result;
        }

        [Rule("RecordItems", "{ RecordItem }")]
        private RecordItems ParseRecordItems() {
            var result = new RecordItems(this);
            var unexpected = false;

            while ((!Match(TokenKind.End)) && (!unexpected)) {
                var item = ParseRecordItem(out unexpected);
                if (!unexpected)
                    result.Add(item);
                else {
                    Unexpected();
                    return result;
                }
            }

            return result;
        }

        [Rule("RecordItem", "MethodDeclaration | PropertyDeclaration | ConstSection | TypeSection | RecordField | ( ['class'] VarSection)")]
        private RecordItem ParseRecordItem(out bool unexpected) {
            var result = new RecordItem(this);
            result.Class = Optional(TokenKind.Class);
            unexpected = false;

            if (Match(TokenKind.Procedure, TokenKind.Function, TokenKind.Constructor, TokenKind.Destructor)) {
                result.MethodDeclaration = ParseMethodDeclaration();
                return result;
            }

            if (Match(PascalToken.Property)) {
                result.PropertyDeclaration = ParsePropertyDeclaration();
                return result;
            }

            if (!result.Class && Match(TokenKind.Const)) {
                result.ConstSection = ParseConstSection();
                return result;
            }

            if (!result.Class && Match(TokenKind.TypeKeyword)) {
                result.TypeSection = ParseTypeSection();
                return result;
            }

            if (Match(TokenKind.Var)) {
                result.VarSection = ParseVarSection();
                return result;
            }

            if (MatchIdentifier()) {
                result.Fields = ParseFieldList();
                return result;
            }

            unexpected = true;
            return result;
        }

        [Rule("RecordVariantSection", "'case' [ Identifier ': ' ] TypeDeclaration 'of' { RecordVariant } ")]
        private RecordVariantSection ParseRecordVariantSection() {
            var result = new RecordVariantSection(this);
            Require(PascalToken.Case);
            if (MatchIdentifier() && LookAhead(1, TokenKind.Colon)) {
                result.Name = RequireIdentifier();
                Require(TokenKind.Colon);
            }
            result.TypeDecl = ParseTypeSpecification();
            Require(TokenKind.Of);

            while (!Match(TokenKind.Undefined, TokenKind.Eof, TokenKind.End)) {
                result.Add(ParseRecordVariant());
            }

            return result;
        }

        [Rule("RecordVariant", "ConstantExpression { , ConstantExpression } ")]
        private RecordVariant ParseRecordVariant() {
            var result = new RecordVariant(this);
            do {
                result.Add(ParseConstantExpression());
            } while (Optional(TokenKind.Comma));
            Require(TokenKind.Colon);
            Require(TokenKind.OpenParen);
            result.FieldList = ParseFieldList();
            Require(TokenKind.CloseParen);
            return result;
        }

        [Rule("RecordFieldList", " { RecordField } ")]
        private RecordFieldList ParseFieldList() {
            var result = new RecordFieldList(this);
            while (MatchIdentifier()) {
                result.Add(ParseRecordField());
            }
            return result;
        }

        [Rule("RecordField", "IdentList ':' TypeSpecification Hints ';'")]
        private RecordField ParseRecordField() {
            var result = new RecordField(this);
            result.Names = ParseIdentList();
            Require(TokenKind.Colon);
            result.FieldType = ParseTypeSpecification();
            result.Hint = ParseHints();
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("RecordHelperDecl", "'record' 'helper' 'for' NamespaceName RecordHelperItems 'end'")]
        private RecordHelperDef ParseRecordHelper() {
            var result = new RecordHelperDef(this);
            Require(TokenKind.Record);
            Require(PascalToken.Helper);
            Require(PascalToken.For);
            result.Name = ParseNamespaceName();
            result.Items = ParseRecordHelperItems();
            Require(TokenKind.End);
            return result;
        }

        [Rule("RecordHelperItems", " { RecordHelperItem }")]
        private RecordHelperItems ParseRecordHelperItems() {
            var result = new RecordHelperItems(this);
            var unexpected = false;

            while ((!Match(TokenKind.End)) && (!unexpected)) {
                var item = ParseRecordHelperItem(out unexpected);
                if (!unexpected)
                    result.Add(item);
                else {
                    Unexpected();
                    return result;
                }
            }

            return result;
        }

        [Rule("RecordHelperItem", "")]
        private RecordHelperItem ParseRecordHelperItem(out bool unexpected) {
            var result = new RecordHelperItem(this);
            unexpected = false;

            if (Match(TokenKind.Procedure, TokenKind.Function, TokenKind.Constructor, TokenKind.Destructor)) {
                result.MethodDeclaration = ParseMethodDeclaration();
                return result;
            }

            if (Match(PascalToken.Property)) {
                result.PropertyDeclaration = ParsePropertyDeclaration();
                return result;
            }

            unexpected = true;
            return result;
        }

        [Rule("ObjectDecl", "'object' ClassParent ObjectItems 'end' ")]
        private ObjectDeclaration ParseObjectDecl() {
            var result = new ObjectDeclaration(this);
            Require(PascalToken.Object);
            result.ClassParent = ParseClassParent();
            result.Items = ParseObjectItems();
            Require(TokenKind.End);
            return result;
        }

        [Rule("ObjectItems", " { ObjectItem } ")]
        private ObjectItems ParseObjectItems() {
            var result = new ObjectItems(this);
            var unexpected = false;

            while ((!Match(TokenKind.End)) && (!unexpected)) {
                var item = ParseObjectItem(out unexpected);
                if (!unexpected)
                    result.Add(item);
                else {
                    Unexpected();
                    return result;
                }
            }

            return result;
        }

        [Rule("ObjectItem", "Visibility | MethodDeclaration | ClassFieldDeclaration ")]
        private ObjectItem ParseObjectItem(out bool unexpected) {
            var result = new ObjectItem(this);

            if (Match(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Strict, TokenKind.Published, TokenKind.Automated)) {
                result.Strict = Optional(TokenKind.Strict);
                result.Visibility = Require(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Published, TokenKind.Automated).Kind;
                unexpected = false;
                return result;
            }

            if (Match(TokenKind.Procedure, TokenKind.Function, TokenKind.Constructor, TokenKind.Destructor)) {
                result.MethodDeclaration = ParseMethodDeclaration();
                unexpected = false;
                return result;
            }

            if (MatchIdentifier()) {
                result.FieldDeclaration = ParseClassFieldDeclararation();
                unexpected = false;
                return result;
            }

            unexpected = true;
            return result;
        }

        [Rule("InterfaceDef", "('inteface' | 'dispinterface') ClassParent [InterfaceGuid] InterfaceDefItems 'end'")]
        private InterfaceDefinition ParseInterfaceDef() {
            var result = new InterfaceDefinition(this);
            if (!Optional(PascalToken.Interface)) {
                Require(PascalToken.DispInterface);
                result.DispInterface = true;
            }
            result.ParentInterface = ParseClassParent();
            if (Match(TokenKind.OpenBraces))
                result.Guid = ParseInterfaceGuid();
            result.Items = ParseInterfaceItems();
            if (result.Items.Count > 0)
                Require(TokenKind.End);
            else
                Optional(TokenKind.End);
            return result;
        }

        [Rule("InterfaceItems", "{ InterfaceItem }")]
        private InterfaceItems ParseInterfaceItems() {
            var result = new InterfaceItems(this);
            var unexpected = false;

            while ((!Match(TokenKind.End)) && (!unexpected)) {
                var item = ParseInterfaceItem(out unexpected);
                if (!unexpected)
                    result.Add(item);
                else if (result.Count > 0) {
                    Unexpected();
                    return result;
                }
            }

            return result;
        }

        [Rule("InterfaceItem", "MethodDeclaration | PropertyDeclaration")]
        private InterfaceItem ParseInterfaceItem(out bool unexpected) {
            var result = new InterfaceItem(this);
            unexpected = true;

            if (Match(TokenKind.Procedure, TokenKind.Function)) {
                unexpected = false;
                result.Method = ParseMethodDeclaration();
                return result;
            }

            if (Match(PascalToken.Property)) {
                unexpected = false;
                result.Property = ParsePropertyDeclaration();
                return result;
            }

            return result;
        }

        [Rule("InterfaceGuid", "'[' QuotedString ']'")]
        private InterfaceGuid ParseInterfaceGuid() {
            var result = new InterfaceGuid(this);
            Require(TokenKind.OpenBraces);
            result.Id = RequireString();
            Require(TokenKind.CloseBraces);
            return result;
        }

        [Rule("ClassHelper", "'class' 'helper' ClassParent 'for' NamespaceName ClassHelperItems 'end'")]
        private ClassHelperDef ParseClassHelper() {
            var result = new ClassHelperDef(this);
            Require(TokenKind.Class);
            Require(PascalToken.Helper);
            result.ClassParent = ParseClassParent();
            Require(PascalToken.For);
            result.HelperName = ParseNamespaceName();
            result.HelperItems = ParseClassHelperItems();
            Require(TokenKind.End);
            return result;
        }

        [Rule("ClassHelperItems", " { ClassHelperItem }")]
        private ClassHelperItems ParseClassHelperItems() {
            var result = new ClassHelperItems(this);
            while (!Match(TokenKind.End, TokenKind.Undefined, TokenKind.Eof)) {
                result.Add(ParseClassHelperItem());
            }
            return result;
        }

        [Rule("ClassHelperItem", "Visibility | MethodDeclaration | PropertyDeclaration | [ 'class' ] VarSection")]
        private ClassHelperItem ParseClassHelperItem() {
            var result = new ClassHelperItem(this);
            result.Attributes = ParseAttributes();
            result.Class = Optional(TokenKind.Class);

            if (!result.Class && (result.Attributes.Count < 1) && Match(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Strict, TokenKind.Published, TokenKind.Automated)) {
                result.Strict = Optional(TokenKind.Strict);
                result.Visibility = Require(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Published, TokenKind.Automated).Kind;
                return result;
            }

            if (Match(TokenKind.Procedure, TokenKind.Function, TokenKind.Constructor, TokenKind.Destructor)) {
                result.MethodDeclaration = ParseMethodDeclaration();
                return result;
            }

            if (Match(PascalToken.Property)) {
                result.PropertyDeclaration = ParsePropertyDeclaration();
                return result;
            }

            if (Match(TokenKind.Var)) {
                result.VarSection = ParseVarSection();
                return result;
            }

            Unexpected();
            return result;
        }

        [Rule("ClassDefinition", "'class' [( 'sealed' | 'abstract' )] [ClassParent] ClassItems 'end' ")]
        private ClassDeclaration ParseClassDefinition() {
            var result = new ClassDeclaration(this);
            Require(TokenKind.Class);
            result.Sealed = Optional(TokenKind.Sealed);
            result.Abstract = Optional(TokenKind.Abstract);
            result.ClassParent = ParseClassParent();
            result.ClassItems = ParseClassItems();
            if (result.ClassItems.Count > 0)
                Require(TokenKind.End);
            else
                Optional(TokenKind.End);
            return result;
        }

        [Rule("ClassItems", "{ ClassItem } ")]
        private ClassDeclarationItems ParseClassItems() {
            var result = new ClassDeclarationItems(this);
            var unexpected = false;

            while ((!Match(TokenKind.End)) && (!unexpected)) {
                var item = ParseClassDeclarationItem(out unexpected);
                if (!unexpected)
                    result.Add(item);
                else if (result.Count > 0) {
                    Unexpected();
                    return result;
                }
            }

            return result;
        }

        [Rule("ClassItem", "Visibility | MethodResolution | MethodDeclaration | ConstSection | TypeSection | PropertyDeclaration | [ 'class'] VarSection | FieldDeclarations ")]
        private ClassDeclarationItem ParseClassDeclarationItem(out bool unexpected) {
            var result = new ClassDeclarationItem(this);
            result.Attributes = ParseAttributes();
            result.Class = Optional(TokenKind.Class);
            unexpected = false;

            if (!result.Class && (result.Attributes.Count < 1) && Match(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Strict, TokenKind.Published, TokenKind.Automated)) {
                result.Strict = Optional(TokenKind.Strict);
                result.Visibility = Require(TokenKind.Public, TokenKind.Protected, TokenKind.Private, TokenKind.Published, TokenKind.Automated).Kind;
                return result;
            }

            if (Match(TokenKind.Procedure, TokenKind.Function) && HasTokenBeforeToken(TokenKind.EqualsSign, TokenKind.Semicolon, TokenKind.OpenParen)) {
                result.MethodResolution = ParseMethodResolution();
                return result;
            }

            if (Match(TokenKind.Procedure, TokenKind.Function, TokenKind.Constructor, TokenKind.Destructor)) {
                result.MethodDeclaration = ParseMethodDeclaration();
                return result;
            }

            if (Match(PascalToken.Property)) {
                result.PropertyDeclaration = ParsePropertyDeclaration();
                return result;
            }

            if (!result.Class && Match(TokenKind.Const)) {
                result.ConstSection = ParseConstSection();
                return result;
            }

            if (!result.Class && Match(TokenKind.TypeKeyword)) {
                result.TypeSection = ParseTypeSection();
                return result;
            }

            if (Match(TokenKind.Var)) {
                result.VarSection = ParseVarSection();
                return result;
            }

            if (MatchIdentifier()) {
                result.FieldDeclaration = ParseClassFieldDeclararation();
                return result;
            }

            unexpected = true;
            return result;
        }

        [Rule("FieldDeclaration", "IdentList ':' TypeSpecification Hints ';'")]
        private ClassField ParseClassFieldDeclararation() {
            var result = new ClassField(this);
            result.Names = ParseIdentList();
            Require(TokenKind.Colon);
            result.TypeDecl = ParseTypeSpecification();
            result.Hint = ParseHints();
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("PropertyDeclaration", "'property' Identifier [ '[' FormalParameters  ']' ] [ ':' NamespaceName ] [ 'index' Expression ]  { ClassPropertySpecifier } ';' ")]
        private ClassProperty ParsePropertyDeclaration() {
            var result = new ClassProperty(this);
            Require(PascalToken.Property);
            result.PropertyName = RequireIdentifier();
            if (Optional(TokenKind.OpenBraces)) {
                result.ArrayIndex = ParseFormalParameters();
                Require(TokenKind.CloseBraces);
            }

            if (Optional(TokenKind.Colon)) {
                result.TypeName = ParseNamespaceName();
            }

            if (Optional(PascalToken.Index)) {
                result.PropertyIndex = ParseExpression();
            }

            while (Match(PascalToken.Read, PascalToken.Write, PascalToken.Add, PascalToken.Remove, PascalToken.ReadOnly, PascalToken.WriteOnly, PascalToken.DispId)) {
                result.Add(ParseClassPropertyAccessSpecifier());
            }

            Require(TokenKind.Semicolon);

            return result;
        }

        [Rule("ClassPropertySpecifier", "ClassPropertyReadWrite | ClassPropertyDispInterface | ('stored' Expression ';') | ('default' [ Expression ] ';' ) | ('nodefault' ';') | ('implements' NamespaceName) ")]
        private ClassPropertySpecifier ParseClassPropertyAccessSpecifier() {
            var result = new ClassPropertySpecifier(this);

            if (Match(PascalToken.Read, PascalToken.Write, PascalToken.Add, PascalToken.Remove)) {
                result.PropertyReadWrite = ParseClassPropertyReadWrite();
                return result;
            }

            if (Match(PascalToken.ReadOnly, PascalToken.WriteOnly, PascalToken.DispId)) {
                result.PropertyDispInterface = ParseClassPropertyDispInterface();
                return result;
            }

            if (Optional(PascalToken.Stored)) {
                result.IsStored = true;
                result.StoredProperty = ParseExpression();
                Require(TokenKind.Semicolon);
                return result;
            }

            if (Optional(PascalToken.Default)) {
                result.IsDefault = true;
                if (!Optional(TokenKind.Semicolon)) {
                    result.DefaultProperty = ParseExpression();
                    Require(TokenKind.Semicolon);
                }
                return result;
            }

            if (Optional(PascalToken.NoDefault)) {
                result.NoDefault = true;
                Require(TokenKind.Semicolon);
                return result;
            }

            if (Optional(PascalToken.Implements)) {
                result.ImplementsTypeId = ParseNamespaceName();
                return result;
            }

            Unexpected();
            return result;
        }

        [Rule("ClassPropertyDispInterface", "( 'readonly' ';')  | ( 'writeonly' ';' ) | DispIdDirective ")]
        private ClassPropertyDispInterface ParseClassPropertyDispInterface() {
            var result = new ClassPropertyDispInterface(this);

            if (Optional(PascalToken.ReadOnly)) {
                result.ReadOnly = true;
                Require(TokenKind.Semicolon);
                return result;
            }

            if (Optional(PascalToken.WriteOnly)) {
                result.WriteOnly = true;
                Require(TokenKind.Semicolon);
                return result;
            }

            result.DispId = ParseDispIdDirective();
            return result;
        }

        [Rule("DispIdDirective", "'dispid' Expression ';'")]
        private DispIdDirective ParseDispIdDirective() {
            var result = new DispIdDirective(this);
            Require(PascalToken.DispId);
            result.DispExpression = ParseExpression();
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("ClassPropertyReadWrite", "('read' | 'write' | 'add' | 'remove' ) NamespaceName ")]
        private ClassPropertyReadWrite ParseClassPropertyReadWrite() {
            var result = new ClassPropertyReadWrite(this);

            result.Kind = Require(PascalToken.Read, PascalToken.Write, PascalToken.Add, PascalToken.Remove).Kind;
            result.Member = ParseNamespaceName();

            return result;
        }

        [Rule("TypeSection", "'type' TypeDeclaration { TypeDeclaration }")]
        private TypeSection ParseTypeSection() {
            var result = new TypeSection(this);
            Require(TokenKind.TypeKeyword);

            do {
                result.Add(ParseTypeDeclaration());
            } while (MatchIdentifier(TokenKind.OpenBraces));

            return result;
        }

        [Rule("TypeDeclaration", "[ Attributes ] GenericTypeIdent '=' TypeDeclaration Hints ';' ")]
        private TypeDeclaration ParseTypeDeclaration() {
            var result = new TypeDeclaration(this);
            result.Attributes = ParseAttributes();
            result.TypeId = ParseGenericTypeIdent();
            Require(TokenKind.EqualsSign);
            result.TypeSpecification = ParseTypeSpecification();
            result.Hint = ParseHints();
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("GenericTypeIdent", "Ident [ GenericDefintion ] ")]
        private GenericTypeIdent ParseGenericTypeIdent() {
            var result = new GenericTypeIdent(this);
            result.Ident = RequireIdentifier();
            if (Match(TokenKind.AngleBracketsOpen)) {
                result.GenericDefinition = ParseGenericDefinition();
            }
            return result;
        }

        [Rule("MethodResolution", "( 'function' | 'procedure' ) NamespaceName '=' Identifier ';' ")]
        private MethodResolution ParseMethodResolution() {
            var result = new MethodResolution(this);
            result.Kind = Require(TokenKind.Function, TokenKind.Procedure).Kind;
            result.TypeName = ParseNamespaceName();
            Require(TokenKind.EqualsSign);
            result.ResolveIdentifier = RequireIdentifier();
            Require(TokenKind.Semicolon);
            return result;
        }

        [Rule("MethodDeclaration", "( 'constructor' | 'destructor' | 'procedure' | 'function' ) Identifier [GenericDefinition] [FormalParameters] [ ':' [ Attributes ] TypeSpecification ] ';' { MethodDirective } ")]
        private ClassMethod ParseMethodDeclaration() {
            var result = new ClassMethod(this);
            result.MethodKind = Require(new[] { TokenKind.Constructor, TokenKind.Destructor, TokenKind.Function, TokenKind.Procedure }).Kind;
            result.Identifier = RequireIdentifier();

            if (Match(TokenKind.AngleBracketsOpen)) {
                result.GenericDefinition = ParseGenericDefinition();
            }

            if (Optional(TokenKind.OpenParen) && (!Optional(TokenKind.CloseParen))) {
                result.Parameters = ParseFormalParameters();
                Require(TokenKind.CloseParen);
            }

            if (Optional(TokenKind.Colon)) {
                result.ResultAttributes = ParseAttributes();
                result.ResultType = ParseTypeSpecification();
            }

            Require(TokenKind.Semicolon);
            result.Directives = ParseMethodDirectives();
            return result;
        }

        [Rule("FormalParameters", "FormalParameter { ';' FormalParameter }")]
        private FormalParameters ParseFormalParameters() {
            var result = new FormalParameters(this);
            do {
                result.Add(ParseFormalParameter());
            } while (Optional(TokenKind.Semicolon));
            return result;
        }

        [Rule("FormalParameter", "[Attributes] [( 'const' | 'var' | 'out' )] IdentList [ ':' TypeDeclaration ] [ '=' Expression ]")]
        private FormalParameter ParseFormalParameter() {
            var result = new FormalParameter(this);
            result.Attributes = ParseAttributes();

            if (Match(TokenKind.Const, TokenKind.Var, TokenKind.Out)) {
                result.ParamType = Require(TokenKind.Const, TokenKind.Var, TokenKind.Out).Kind;
            }

            result.ParamNames = ParseIdentList();

            if (Optional(TokenKind.Colon)) {
                result.TypeDeclaration = ParseTypeSpecification();
            }

            if (Optional(TokenKind.EqualsSign)) {
                result.DefaultValue = ParseExpression();
            }

            return result;
        }

        [Rule("IdentList", "Identifiert { ',' Identifier }")]
        private IdentList ParseIdentList() {
            var result = new IdentList(this);
            do {
                result.Add(RequireIdentifier());
            } while (Optional(TokenKind.Comma));
            return result;
        }

        [Rule("GenericDefinition", "SimpleGenericDefinition | ConstrainedGenericDefinition")]
        private GenericDefinition ParseGenericDefinition() {
            if (!LookAhead(2, TokenKind.Comma)) {
                return ParseConstrainedGenericDefinition();
            }

            return ParseSimpleGenericDefinition();
        }

        [Rule("SimpleGenericDefinition", "'<' Identifier { ',' Identifier } '>'")]
        private GenericDefinition ParseSimpleGenericDefinition() {
            var result = new GenericDefinition(this);
            Require(TokenKind.AngleBracketsOpen);

            do {
                var part = new GenericDefinitionPart(this);
                part.Identifier = RequireIdentifier();
                result.Add(part);
            } while (Optional(TokenKind.Comma));

            Require(TokenKind.AngleBracketsClose);
            return result;

        }

        [Rule("ConstrainedGenericDefinition", "'<' GenericDefinitionPart { ';' GenericDefinitionPart } '>'")]
        private GenericDefinition ParseConstrainedGenericDefinition() {
            var result = new GenericDefinition(this);
            Require(TokenKind.AngleBracketsOpen);

            do {
                result.Add(ParseGenericDefinitionPart());
            } while (Optional(TokenKind.Semicolon));

            Require(TokenKind.AngleBracketsClose);
            return result;

        }

        [Rule("GenericDefinitionPart", "Identifier [ ':' GenericConstraint { ',' GenericConstraint } ]")]
        private GenericDefinitionPart ParseGenericDefinitionPart() {
            var result = new GenericDefinitionPart(this);
            result.Identifier = RequireIdentifier();

            if (Optional(TokenKind.Colon)) {
                do {
                    result.Add(ParseGenericConstraint());
                } while (Optional(TokenKind.Comma));
            }

            return result;
        }

        [Rule("GenericConstraint", " 'record' | 'class' | 'constructor' | Identifier ")]
        private ConstrainedGeneric ParseGenericConstraint() {
            var result = new ConstrainedGeneric(this);

            if (Optional(TokenKind.Record)) {
                result.RecordConstraint = true;
            }
            else if (Optional(TokenKind.Class)) {
                result.ClassConstraint = true;
            }
            else if (Optional(TokenKind.Constructor)) {
                result.ConstructorConstraint = true;
            }
            else {
                result.ConstraintIdentifier = RequireIdentifier();
            }

            return result;
        }

        [Rule("ClassParent", " [ '(' NamespaceName { ',' NamespaceName } ')' ]")]
        private ParentClass ParseClassParent() {
            var result = new ParentClass(this);
            if (Optional(TokenKind.OpenParen)) {
                do {
                    result.Add(ParseNamespaceName());
                } while (Optional(TokenKind.Comma));
                Require(TokenKind.CloseParen);
            }
            return result;
        }

        [Rule("ClassOfDeclaration", "'class' 'of' NamespaceName")]
        private ClassOfDeclaration ParseClassOfDeclaration() {
            var result = new ClassOfDeclaration(this);
            Require(TokenKind.Class);
            Require(TokenKind.Of);
            result.TypeName = ParseNamespaceName();
            return result;
        }

        [Rule("FileType", "'file' [ 'of' TypeSpecification ]")]
        private FileType ParseFileType() {
            var result = new FileType(this);
            Require(TokenKind.File);
            if (Optional(TokenKind.Of)) {
                result.TypeDefinition = ParseTypeSpecification();
            }
            return result;
        }

        [Rule("SetDef", "'set' 'of' TypeSpecification")]
        private SetDef ParseSetDefinition() {
            var result = new SetDef(this);
            Require(TokenKind.Set);
            Require(TokenKind.Of);
            result.TypeDefinition = ParseTypeSpecification();
            return result;
        }

        [Rule("TypeAlias", "'type' NamespaceName")]
        private TypeAliasDefinition ParseTypeAlias() {
            var result = new TypeAliasDefinition(this);
            Require(TokenKind.TypeKeyword);
            result.TypeName = ParseNamespaceName();

            if (Match(TokenKind.AngleBracketsOpen)) {
                result.GenericSuffix = ParseGenericSuffix();
            }

            return result;
        }

        [Rule("GenericSuffix", "'<' TypeDefinition { ',' TypeDefinition '}' '>'")]
        private GenericTypesuffix ParseGenericSuffix() {
            var result = new GenericTypesuffix(this);
            Require(TokenKind.AngleBracketsOpen);

            do {
                result.Add(ParseTypeSpecification());
            } while (Optional(TokenKind.Comma));

            Require(TokenKind.AngleBracketsClose);
            return result;
        }

        [Rule("ArrayType", " 'array' [ '[' ArrayIndex { ',' ArrayIndex } ']']  'of' ( 'const' | TypeDefinition ) ")]
        private ArrayType ParseArrayType() {
            var result = new ArrayType(this);
            Require(TokenKind.Array);
            if (Optional(TokenKind.OpenBraces)) {
                do {
                    result.Add(ParseArrayIndex());
                } while (Optional(TokenKind.Comma));
                Require(TokenKind.CloseBraces);
            }
            Require(TokenKind.Of);

            if (Optional(TokenKind.Const)) {
                result.ArrayOfConst = true;
            }
            else {
                result.TypeSpecification = ParseTypeSpecification();
            }

            return result;
        }

        [Rule("ArrayIndex", "ConstantExpression [ '..' ConstantExpression ] ")]
        private ArrayIndex ParseArrayIndex() {
            var result = new ArrayIndex(this);

            result.StartIndex = ParseConstantExpression();
            if (Optional(TokenKind.DotDot)) {
                result.EndIndex = ParseConstantExpression();
            }
            return result;
        }

        [Rule("PointerType", "( 'pointer' | '^' TypeSpecification )")]
        private PointerType ParsePointerType() {
            var result = new PointerType(this);

            if (Optional(TokenKind.Pointer)) {
                result.GenericPointer = true;
                return result;
            }

            Require(TokenKind.Circumflex);
            result.TypeSpecification = ParseTypeSpecification();
            return result;
        }

        [Rule("Attributes", "{ '[' Attribute | AssemblyAttribue ']' }")]
        private UserAttributes ParseAttributes() {
            var userAttributes = new UserAttributes(this);
            while (Match(TokenKind.OpenBraces)) {
                if (LookAhead(1, PascalToken.Assembly)) {
                    userAttributes.Add(ParseAssemblyAttribute());
                }
                else {
                    Require(TokenKind.OpenBraces);
                    do {
                        userAttributes.Add(ParseAttribute());
                    } while (Optional(TokenKind.Comma));
                    Require(TokenKind.CloseBraces);
                }
            }
            return userAttributes;
        }

        [Rule("Attribute", "NamespaceName [ '(' Expressions ')' ]")]
        private UserAttribute ParseAttribute() {
            var result = new UserAttribute(this);
            result.Name = ParseNamespaceName();
            if (Optional(TokenKind.OpenParen)) {
                while (!Match(TokenKind.CloseParen)) {
                    result.Expressions = ParseExpressions();
                }
                Require(TokenKind.CloseParen);
            }
            return result;
        }

        [Rule("Expressions", "Expression { ',' Expression }")]
        private ExpressionList ParseExpressions() {
            var result = new ExpressionList(this);
            do {
                result.Add(ParseExpression());
            } while (Optional(TokenKind.Comma));
            return result;
        }

        [Rule("ConstantExpression", " '(' ( RecordConstant | ConstantExpression ) ')' | Expression")]
        private ConstantExpression ParseConstantExpression() {
            var result = new ConstantExpression(this);

            if (Optional(TokenKind.OpenParen)) {
                if (MatchIdentifier() && (LookAhead(1, TokenKind.Colon)))
                    result.RecordConstant = ParseRecordConstant();
                else do {
                        result.Add(ParseConstantExpression());
                    } while (Optional(TokenKind.Comma));
                Require(TokenKind.CloseParen);
            }
            else {
                result.Value = ParseExpression();
            }

            return result;
        }

        [Rule("RecordConstantExpression", "Identifier ':' ConstantExpression")]
        private RecordConstantExpression ParseRecordConstant() {
            var result = new RecordConstantExpression(this);
            result.Name = RequireIdentifier();
            Require(TokenKind.Colon);
            result.Value = ParseConstantExpression();
            return result;
        }

        [Rule("Expression", "SimpleExpression [ ('<'|'<='|'>'|'>='|'<>'|'='|'in'|'as') SimpleExpression ] | ClosureExpression")]
        private Expression ParseExpression() {
            var result = new Expression(this);

            if (Match(TokenKind.Function, TokenKind.Procedure)) {
                result.ClosureExpression = ParseClosureExpression();
            }
            else {
                result.LeftOperand = ParseSimpleExpression();
                if (Match(PascalToken.LessThen, PascalToken.LessThenEquals, PascalToken.GreaterThen, PascalToken.GreaterThenEquals, PascalToken.NotEquals, TokenKind.EqualsSign, TokenKind.In, PascalToken.Is)) {
                    result.Kind = Require(PascalToken.LessThen, PascalToken.LessThenEquals, PascalToken.GreaterThen, PascalToken.GreaterThenEquals, PascalToken.NotEquals, TokenKind.EqualsSign, TokenKind.In, PascalToken.Is).Kind;
                    result.RightOperand = ParseSimpleExpression();
                }
            }

            return result;
        }

        [Rule("SimpleExpression", "Term { ('+'|'-'|'or'|'xor') SimpleExpression }")]
        private SimplExpr ParseSimpleExpression() {
            var result = new SimplExpr(this);
            result.LeftOperand = ParseTerm();
            if (Match(TokenKind.Plus, TokenKind.Minus, PascalToken.Or, PascalToken.Xor)) {
                result.Kind = Require(TokenKind.Plus, TokenKind.Minus, PascalToken.Or, PascalToken.Xor).Kind;
                result.RightOperand = ParseSimpleExpression();
            }
            return result;
        }

        [Rule("Termin", "Factor [ ('*'|'/'|'div'|'mod'|'and'|'shl'|'shr'|'as') Term ]")]
        private Term ParseTerm() {
            var result = new Term(this);
            result.LeftOperand = ParseFactor();
            if (Match(TokenKind.Times, TokenKind.Slash, PascalToken.Div, PascalToken.Mod, PascalToken.And, PascalToken.Shl, PascalToken.Shr, PascalToken.As)) {
                result.Kind = Require(TokenKind.Times, TokenKind.Slash, PascalToken.Div, PascalToken.Mod, PascalToken.And, PascalToken.Shl, PascalToken.Shr, PascalToken.As).Kind;
                result.RightOperand = ParseTerm();
            }
            return result;
        }

        [Rule("Factor", "'@' Factor  | 'not' Factor | '+' Factor | '-' Factor | '^' Identifier | Integer | HexNumber | Real | 'true' | 'false' | 'nil' | '(' Expression ')' | String | SetSection | Designator | TypeCast")]
        private Factor ParseFactor() {
            var result = new Factor(this);

            if (Optional(TokenKind.At)) {
                result.AddressOf = ParseFactor();
                return result;
            }

            if (Optional(PascalToken.Not)) {
                result.Not = ParseFactor();
                return result;
            }

            if (Optional(TokenKind.Plus)) {
                result.Plus = ParseFactor();
                return result;
            }

            if (Optional(TokenKind.Minus)) {
                result.Minus = ParseFactor();
                return result;
            }

            if (Optional(TokenKind.Circumflex)) {
                result.PointerTo = RequireIdentifier();
                return result;
            }

            if (Match(PascalToken.Integer)) {
                result.IntValue = RequireInteger();
                return result;
            }

            if (Match(PascalToken.HexNumber)) {
                result.HexValue = RequireHexValue();
                return result;
            }

            if (Match(PascalToken.Real)) {
                result.RealValue = RequireRealValue();
                return result;
            }

            if (Match(PascalToken.QuotedString)) {
                result.StringValue = RequireString();
                return result;
            }

            if (Optional(PascalToken.True)) {
                result.IsTrue = true;
                return result;
            }

            if (Optional(PascalToken.False)) {
                result.IsFalse = true;
                return result;
            }

            if (Optional(PascalToken.Nil)) {
                result.IsNil = true;
                return result;
            }

            if (Optional(TokenKind.OpenParen)) {
                result.ParenExpression = ParseExpression();
                Require(TokenKind.CloseParen);
                return result;
            }

            if (Match(TokenKind.OpenBraces)) {
                result.SetSection = ParseSetSection();
                return result;
            }

            if (MatchIdentifier(PascalToken.Inherited)) {
                result.Designator = ParseDesignator();
                return result;
            }

            Unexpected();
            return null;
        }

        [Rule("Designator", "[ 'inherited' ] [ NamespaceName ] { DesignatorItem }")]
        private DesignatorStatement ParseDesignator() {
            var result = new DesignatorStatement(this);
            result.Inherited = Optional(PascalToken.Inherited);
            if (MatchIdentifier()) {
                result.Name = ParseNamespaceName();
            }

            DesignatorItem item;
            do {
                item = ParseDesignatorItem();
                result.Add(item);
            } while (item != null);

            return result;
        }

        [Rule("DesignatorItem", "'^' | '.' Ident | '[' ExpressionList ']' | '(' [ FormattedExpression  { ',' FormattedExpression } ] ')'")]
        private DesignatorItem ParseDesignatorItem() {
            if (Optional(TokenKind.Circumflex)) {
                var result = new DesignatorItem(this);
                result.Dereference = true;
                return result;
            }

            if (Optional(TokenKind.Dot)) {
                var result = new DesignatorItem(this);
                result.Subitem = RequireIdentifier();
                return result;
            }

            if (Optional(TokenKind.OpenBraces)) {
                var result = new DesignatorItem(this);
                result.IndexExpression = ParseExpressions();
                Require(TokenKind.CloseBraces);
                return result;
            }

            if (Optional(TokenKind.OpenParen)) {
                var result = new DesignatorItem(this);
                if (!Match(TokenKind.CloseParen)) {
                    do {
                        result.Add(ParseFormattedExpression());
                    } while (Optional(TokenKind.Comma));
                }
                Require(TokenKind.CloseParen);
                return result;
            }

            return null;
        }

        [Rule("FormattedExpression", "Expression [ ':' Expression [ ':' Expression ] ]")]
        private FormattedExpression ParseFormattedExpression() {
            var result = new FormattedExpression(this);
            result.Expression = ParseExpression();
            if (Optional(TokenKind.Colon)) {
                result.Width = ParseExpression();
                if (Optional(TokenKind.Colon)) {
                    result.Decimals = ParseExpression();
                }
            }
            return result;
        }

        [Rule("SetSection", "'[' [ Expression ] { (',' | '..') Expression } ']'")]
        private SetSectn ParseSetSection() {
            var result = new SetSectn(this);
            Require(TokenKind.OpenBraces);
            if (!Match(TokenKind.CloseBraces)) {
                SetSectnPart part;
                do {
                    part = new SetSectnPart(this);
                    if (Match(TokenKind.Comma, TokenKind.DotDot))
                        part.Continuation = Require(TokenKind.Comma, TokenKind.DotDot).Kind;
                    else
                        part.Continuation = TokenKind.Undefined;
                    part.SetExpression = ParseExpression();
                    result.Add(part);
                } while (Match(TokenKind.Comma, TokenKind.DotDot));
            }
            Require(TokenKind.CloseBraces);
            return result;
        }

        [Rule("ClosureExpr", "('function'|'procedure') [ FormalParameterSection ] [ ':' TypeSpecification ] Block ")]
        private ClosureExpr ParseClosureExpression() {
            var result = new ClosureExpr(this);
            result.Kind = Require(TokenKind.Function, TokenKind.Procedure).Kind;
            if (Match(TokenKind.OpenParen)) {
                result.Parameters = ParseFormalParameterSection();
            }
            if (result.Kind == TokenKind.Function) {
                Require(TokenKind.Colon);
                result.ReturnType = ParseTypeSpecification();
            }
            result.Block = ParseBlock();
            return result;
        }

        private PascalInteger RequireInteger()
            => new PascalInteger(Require(PascalToken.Integer), this);

        private PascalHexNumber RequireHexValue()
            => new PascalHexNumber(Require(PascalToken.HexNumber), this);

        private PascalRealNumber RequireRealValue()
            => new PascalRealNumber(Require(PascalToken.Real), this);

        private PascalIdentifier RequireIdentifier() {
            if (Match(PascalToken.Identifier)) {
                return new PascalIdentifier(Require(PascalToken.Identifier), this);
            };

            if (!reservedWords.Contains(CurrentToken().Kind)) {
                return new PascalIdentifier(Require(CurrentToken().Kind), this);
            }

            Unexpected();
            return new PascalIdentifier(Tokenizer.CreatePseudoToken(TokenKind.Undefined), this);
        }

        [Rule("UsesFileClause", "'uses' NamespaceFileNameList")]
        private UsesFileClause ParseUsesFileClause() {
            var result = new UsesFileClause(this);
            Require(TokenKind.Uses);
            result.Files = ParseNamespaceFileNameList();
            return result;
        }

        [Rule("NamespaceFileName", "NamespaceName [ 'in' QuotedString ]")]
        private NamespaceFileName ParseNamespaceFileName() {
            var result = new NamespaceFileName(this);
            result.NamespaceName = ParseNamespaceName();
            if (Optional(TokenKind.In))
                result.QuotedFileName = RequireString();
            return result;
        }

        private QuotedString RequireString() {
            var result = new QuotedString(this);
            result.UnquotedValue = Require(PascalToken.QuotedString).Value;
            return result;
        }

        private NamespaceName ParseNamespaceName() {
            var result = new NamespaceName(this);
            result.Add(RequireIdentifier());

            while (Optional(TokenKind.Dot)) {
                result.Add(RequireIdentifier());
            }

            return result;
        }

        /// <summary>
        ///     print the parser grammar
        /// </summary>
        /// <param name="result">Result</param>
        public static void PrintGrammar(StringBuilder result) {
            PrintGrammar(typeof(StandardParser), result);
        }



    }
}