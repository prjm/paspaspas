using PasPasPas.Api;
using PasPasPas.Internal.Parser.Syntax;
using System.Text;
using System;
using System.Collections.Generic;
using PasPasPas.Internal.Tokenizer;

namespace PasPasPas.Internal.Parser {

    /// <summary>
    ///     standard, recursive descend pascal paser
    /// </summary>
    public class StandardParser : ParserBase, IPascalParser, IParserInformationProvider {

        private static readonly HashSet<int> reservedWords
            = new HashSet<int>() {
            PascalToken.And,
            PascalToken.Array,
            PascalToken.As,
            PascalToken.Asm,
            PascalToken.Begin,
            PascalToken.Case,
            PascalToken.Class,
            PascalToken.Const,
            PascalToken.Constructor,
            PascalToken.Destructor,
            PascalToken.DispInterface     ,
            PascalToken.Div,
            PascalToken.Do,
            PascalToken.DownTo,
            PascalToken.Else,
            PascalToken.End,
            PascalToken.Except,
            PascalToken.Exports,
            PascalToken.File,
            PascalToken.Finalization   ,
            PascalToken.Finally,
            PascalToken.For,
            PascalToken.Function,
            PascalToken.GoToKeyword,
            PascalToken.If,
            PascalToken.Implementation,
            PascalToken.In,
            PascalToken.Inherited,
            PascalToken.Initialization,
            PascalToken.Inline,
            PascalToken.Interface,
            PascalToken.Is,
            PascalToken.Label,
            PascalToken.Library,
            PascalToken.Mod,
            PascalToken.Nil,
            PascalToken.Not,
            PascalToken.Object,
            PascalToken.Of,
            PascalToken.Or,
            PascalToken.Packed,
            PascalToken.Procedure,
            PascalToken.Program,
            PascalToken.Property,
            PascalToken.Raise,
            PascalToken.Record,
            PascalToken.Repeat,
            PascalToken.Resourcestring,
            PascalToken.Set,
            PascalToken.Shl,
            PascalToken.Shr,
            PascalToken.String,
            PascalToken.Then,
            PascalToken.ThreadVar,
            PascalToken.To,
            PascalToken.Try,
            PascalToken.TypeKeyword,
            PascalToken.Unit,
            PascalToken.Until,
            PascalToken.Uses,
            PascalToken.Var,
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

            return StandardTokenizer.IsKeyword(token.Value);
        }


        /// <summary>
        ///     parse input
        /// </summary>
        public ISyntaxPart Parse()
            => ParseFile();

        [Rule("File", "Program | Library | Unit | Package")]
        private ISyntaxPart ParseFile() {
            if (Match(PascalToken.Library)) {
                return ParseLibrary();
            }
            else if (Match(PascalToken.Unit)) {
                return ParseUnit();
            }
            else if (Match(PascalToken.Package)) {
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
            if (Match(PascalToken.Uses)) {
                result.UsesClause = ParseUsesClause();
            }
            result.DeclarationSections = ParseDeclarationSections();
            return result;
        }

        [Rule("UsesClause", "'uses' NamespaceNameList")]
        private UsesClause ParseUsesClause() {
            var result = new UsesClause(this);
            Require(PascalToken.Uses);
            result.UsesList = ParseNamespaceNameList();
            return result;
        }

        [Rule("UnitInterface", "'interface' [ UsesClause ] InterfaceDeclaration ")]
        private UnitInterface ParseUnitInterface() {
            var result = new UnitInterface(this);
            Require(PascalToken.Interface);
            if (Match(PascalToken.Uses)) {
                result.UsesClause = ParseUsesClause();
            }

            result.InterfaceDeclaration = ParseInterfaceDeclaration();
            return result;
        }

        [Rule("InterfaceDeclarationItem", "ConstSection | TypeSection | VarSection | ExportsSection | AssemblyAttribute | ExportedProcedureHeading")]
        private SyntaxPartBase ParseInterfaceDeclarationItem() {
            if (Match(PascalToken.Const)) {
                return ParseConstSection();
            }

            if (Match(PascalToken.TypeKeyword)) {
                return ParseTypeSection();
            }

            if (Match(PascalToken.Var)) {
                return ParseVarSection();
            }

            if (Match(PascalToken.Exports)) {
                return ParseExportsSection();
            }

            if (Match(PascalToken.OpenBraces) && LookAhead(1, PascalToken.Assembly)) {
                return ParseAssemblyAttribute();
            }

            if (Match(PascalToken.Procedure, PascalToken.Function)) {
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
            result.Kind = Require(PascalToken.Function, PascalToken.Procedure).Kind;
            result.Name = RequireIdentifier();
            if (Optional(PascalToken.OpenParen)) {
                result.Parameters = ParseFormalParameterSection();
            }
            if (Optional(PascalToken.Colon)) {
                result.ResultAttributes = ParseAttributes();
                result.ResultType = ParseTypeSpecification();
            }
            Require(PascalToken.Semicolon);
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
            if (Match(PascalToken.Overload)) {
                return ParseOverloadDirective();
            }

            if (Match(PascalToken.Inline)) {
                return ParseInlineDirective();
            }

            if (Match(PascalToken.Cdecl, PascalToken.Pascal, PascalToken.Register, PascalToken.Safecall, PascalToken.Stdcall, PascalToken.Export)) {
                return ParseCallConvention();
            }

            if (Match(PascalToken.Far, PascalToken.Local, PascalToken.Near)) {
                return ParseOldCallConvention();
            }

            if (Match(PascalToken.Deprecated, PascalToken.Library, PascalToken.Experimental, PascalToken.Platform)) {
                return ParseHint();
            }

            if (Match(PascalToken.VarArgs, PascalToken.External)) {
                return ParseExternalDirective();
            }

            if (Match(PascalToken.Unsafe)) {
                return ParseUnsafeDirective();
            }

            return null;
        }

        [Rule("UnsafeDirective", "'unsafe' ';' ")]
        private UnsafeDirective ParseUnsafeDirective() {
            var result = new UnsafeDirective(this);
            Require(PascalToken.Unsafe);
            Require(PascalToken.Semicolon);
            return result;
        }

        [Rule("ExternalDirective", "(varargs | external [ ConstExpression { ExternalSpecifier } ]) ';' ")]
        private ExternalDirective ParseExternalDirective() {
            var result = new ExternalDirective(this);
            result.Kind = Require(PascalToken.VarArgs, PascalToken.External).Kind;

            if ((result.Kind == PascalToken.External) && (!Match(PascalToken.Semicolon))) {
                result.ExternalExpression = ParseConstantExpression();
                ExternalSpecifier specifier;
                do {
                    specifier = ParseExternalSpecifier();
                    result.Add(specifier);
                } while (specifier != null);
            }
            Require(PascalToken.Semicolon);
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
            result.Kind = Require(PascalToken.Near, PascalToken.Far, PascalToken.Local).Kind;
            Require(PascalToken.Semicolon);
            return result;
        }

        [Rule("UnitBlock", "( UnitInitilization 'end' ) | CompoundStatement | 'end' ")]
        private UnitBlock ParseUnitBlock() {
            var result = new UnitBlock(this);

            if (Optional(PascalToken.End))
                return result;

            if (Match(PascalToken.Begin)) {
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

        [Rule("CompoundStatement", "", true)]
        private CompoundStatement ParseCompoundStatement() {
            var result = new CompoundStatement(this);
            Require(PascalToken.Begin);
            Require(PascalToken.End);
            return result;
        }

        [Rule("UnitHead", "'unit' NamespaceName { Hint } ';' ")]
        private UnitHead ParseUnitHead() {
            var result = new UnitHead(this);
            Require(PascalToken.Unit);
            result.UnitName = ParseNamespaceName();
            result.Hint = ParseHints();
            Require(PascalToken.Semicolon);
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

            Require(PascalToken.End);
            Require(PascalToken.Dot);

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
            } while (Optional(PascalToken.Comma));
            Require(PascalToken.Semicolon);
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
            } while (Optional(PascalToken.Comma));
            Require(PascalToken.Semicolon);
            return result;
        }

        [Rule("PackageHead", "'package' NamespaceName ';' ")]
        private PackageHead ParsePackageHead() {
            var result = new PackageHead(this);
            Require(PascalToken.Package);
            result.PackageName = ParseNamespaceName();
            Require(PascalToken.Semicolon);
            return result;
        }

        [Rule("Library", "LibraryHead [UsesFileClause] Block '.' ")]
        private Library ParseLibrary() {
            var result = new Library(this);
            result.LibraryHead = ParseLibraryHead();

            if (Match(PascalToken.Uses))
                result.Uses = ParseUsesFileClause();

            result.MainBlock = ParseBlock();
            Require(PascalToken.Dot);
            return result;
        }

        [Rule("LibraryHead", "'library' NamespaceName Hints ';'")]
        private LibraryHead ParseLibraryHead() {
            var result = new LibraryHead(this);
            Require(PascalToken.Library);
            result.Name = ParseNamespaceName();
            result.Hints = ParseHints();
            Require(PascalToken.Semicolon);
            return result;
        }

        [Rule("Program", "[ProgramHead] [UsesFileClause] Block '.'")]
        private Program ParseProgram() {
            var result = new Program(this);

            if (Match(PascalToken.Program))
                result.ProgramHead = ParseProgramHead();

            if (Match(PascalToken.Uses))
                result.Uses = ParseUsesFileClause();

            result.MainBlock = ParseBlock();
            Require(PascalToken.Dot);
            return result;
        }

        [Rule("ProgramHead", "'program' NamespaceName [ProgramParams] ';'")]
        private ProgramHead ParseProgramHead() {
            var result = new ProgramHead(this);
            Require(PascalToken.Program);
            result.Name = ParseNamespaceName();
            result.Params = ParseProgramParams();
            Require(PascalToken.Semicolon);
            return result;
        }

        [Rule("ProgramParams", "'(' [ Identifier { ',' Identifier } ] ')'")]
        private ProgramParameterList ParseProgramParams() {
            var result = new ProgramParameterList(this);

            if (Optional(PascalToken.OpenParen)) {

                if (MatchIdentifier()) {
                    result.Add(RequireIdentifier());

                    while (Optional(PascalToken.Comma))
                        result.Add(RequireIdentifier());
                }

                Require(PascalToken.CloseParen);
            }

            return result;
        }

        [Rule("Block", "DeclarationSections [ BlockBody ] ")]
        private Block ParseBlock() {
            var result = new Block(this);
            result.DeclarationSections = ParseDeclarationSections();
            if (Match(PascalToken.Asm, PascalToken.Begin)) {
                result.Body = ParseBlockBody();
            }
            return result;
        }

        [Rule("BlockBody", "AssemblerBlock | CompoundStatement")]
        private BlockBody ParseBlockBody() {
            var result = new BlockBody(this);

            if (Match(PascalToken.Asm)) {
                result.AssemblerBlock = ParseAssemblerBlock();
            }

            if (Match(PascalToken.Begin)) {
                result.Body = ParseCompoundStatement();
            }

            return result;
        }

        private AssemblerBlock ParseAssemblerBlock() {
            throw new NotImplementedException();
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

                if (Match(PascalToken.Const, PascalToken.Resourcestring)) {
                    result.Add(ParseConstSection());
                    continue;
                }

                if (Match(PascalToken.TypeKeyword)) {
                    result.Add(ParseTypeSection());
                    continue;
                }

                if (Match(PascalToken.Var, PascalToken.ThreadVar)) {
                    result.Add(ParseVarSection());
                    continue;
                }

                if (Match(PascalToken.Exports)) {
                    result.Add(ParseExportsSection());
                    continue;
                }

                UserAttributes attrs = null;
                if (Match(PascalToken.OpenBraces)) {
                    if (LookAhead(1, PascalToken.Assembly)) {
                        result.Add(ParseAssemblyAttribute());
                        continue;
                    }
                    else {
                        attrs = ParseAttributes();
                    }
                }
                bool useClass = Optional(PascalToken.Class);

                if (Match(PascalToken.Function, PascalToken.Procedure, PascalToken.Constructor, PascalToken.Destructor, PascalToken.Operator)) {

                    bool useMethodDeclaration = //
                        useClass ||
                        Match(PascalToken.Constructor, PascalToken.Destructor, PascalToken.Operator) ||
                        (LookAhead(1, PascalToken.Identifier) && (LookAhead(2, PascalToken.Dot)));

                    if (useMethodDeclaration) {
                        result.Add(ParseMethodDecl());
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
            Require(PascalToken.Semicolon);
            result.Directives = ParseMethodDirectives();
            result.MethodBody = ParseBlock();
            if ((result.MethodBody != null) && (result.MethodBody.Body != null))
                Require(PascalToken.Semicolon);
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

            if (Match(PascalToken.Reintroduce)) {
                return ParseReintroduceDirective();
            }

            if (Match(PascalToken.Overload)) {
                return ParseOverloadDirective();
            }

            if (Match(PascalToken.Inline, PascalToken.Assembler)) {
                return ParseInlineDirective();
            }

            if (Match(PascalToken.Message, PascalToken.Static, PascalToken.Dynamic, PascalToken.Override, PascalToken.Virtual)) {
                return ParseBindingDirective();
            }

            if (Match(PascalToken.Abstract, PascalToken.Final)) {
                return ParseAbstractDirective();
            }

            if (Match(PascalToken.Cdecl, PascalToken.Pascal, PascalToken.Register, PascalToken.Safecall, PascalToken.Stdcall, PascalToken.Export)) {
                return ParseCallConvention();
            }

            if (Match(PascalToken.Deprecated, PascalToken.Library, PascalToken.Experimental, PascalToken.Platform)) {
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
            result.Kind = Require(PascalToken.Inline, PascalToken.Assembler).Kind;
            Require(PascalToken.Semicolon);
            return result;
        }

        [Rule("CallConvention", "('cdecl' | 'pascal' | 'register' | 'safecall' | 'stdcall' | 'export') ';' ")]
        private SyntaxPartBase ParseCallConvention() {
            var result = new CallConvention(this);
            result.Kind = Require(PascalToken.Cdecl, PascalToken.Pascal, PascalToken.Register, PascalToken.Safecall, PascalToken.Stdcall, PascalToken.Export).Kind;
            Require(PascalToken.Semicolon);
            return result;
        }

        [Rule("AbstractDirective", "('abstract' | 'final' ) ';' ")]
        private SyntaxPartBase ParseAbstractDirective() {
            var result = new AbstractDirective(this);
            result.Kind = Require(PascalToken.Abstract, PascalToken.Final).Kind;
            Require(PascalToken.Semicolon);
            return result;
        }

        [Rule("BindingDirective", " ('message' Expression ) | 'static' | 'dynamic' | 'override' | 'virtual' ")]
        private SyntaxPartBase ParseBindingDirective() {
            var result = new BindingDirective(this);
            result.Kind = Require(PascalToken.Message, PascalToken.Static, PascalToken.Dynamic, PascalToken.Override, PascalToken.Virtual).Kind;
            if (result.Kind == PascalToken.Message) {
                result.MessageExpression = ParseExpression();
            }
            Require(PascalToken.Semicolon);
            return result;
        }

        [Rule("OverloadDirective", "'overload' ';' ")]
        private SyntaxPartBase ParseOverloadDirective() {
            var result = new OverloadDirective(this);
            Require(PascalToken.Overload);
            Require(PascalToken.Semicolon);
            return result;
        }

        [Rule("ReintroduceDirective", "'reintroduce' ';' ")]
        private SyntaxPartBase ParseReintroduceDirective() {
            var result = new ReintroduceDirective(this);
            Require(PascalToken.Reintroduce);
            Require(PascalToken.Semicolon);
            return result;
        }

        [Rule("MethodDeclHeading", " ('constructor' | 'destructor' | 'function' | 'procedure') NamespaceName [GenericDefinition] [FormalParameterSection] [':' Attributes TypeSpecification ]")]
        private MethodDeclHeading ParseMethodDeclHeading() {
            var result = new MethodDeclHeading(this);
            result.Kind = Require(PascalToken.Constructor, PascalToken.Destructor, PascalToken.Function, PascalToken.Procedure).Kind;
            result.Name = ParseNamespaceName();
            if (Match(PascalToken.AngleBracketsOpen)) {
                result.GenericDefinition = ParseGenericDefinition();
            }
            if (Match(PascalToken.OpenParen)) {
                result.Parameters = ParseFormalParameterSection();
            }
            if (Optional(PascalToken.Colon)) {
                result.ResultTypeAttributes = ParseAttributes();
                result.ResultType = ParseTypeSpecification();
            }
            return result;
        }

        [Rule("ProcedureDeclaration", "ProcedureDeclarationHeading ';' FunctionDirectives [ ProcBody ]")]
        private ProcedureDeclaration ParseProcedureDeclaration(UserAttributes attributes) {
            var result = new ProcedureDeclaration(this);
            result.Heading = ParseProcedureDeclarationHeading();
            Require(PascalToken.Semicolon);
            result.Directives = ParseFunctionDirectives();
            result.ProcBody = ParseProcBody();
            return result;
        }

        private ProcBody ParseProcBody() {
            throw new NotImplementedException();
        }

        [Rule("ProcedureDeclarationHeading", "('procedure' | 'function') Identifier [FormalParameterSection][':' TypeSpecification]")]
        private ProcedureDeclarationHeading ParseProcedureDeclarationHeading() {
            var result = new ProcedureDeclarationHeading(this);
            result.Kind = Require(PascalToken.Function, PascalToken.Procedure).Kind;
            result.Name = RequireIdentifier();
            if (Match(PascalToken.OpenParen)) {
                result.Parameters = ParseFormalParameterSection();
            }
            if (Optional(PascalToken.Colon)) {
                result.ResultTypeAttributes = ParseAttributes();
                result.ResultType = ParseTypeSpecification();
            }
            return result;
        }

        private UserAttributes ParseAssemblyAttribute() {
            throw new NotImplementedException();
        }

        [Rule("ExportsSection", "'exports' Identifier ExportItem { ',' ExportItem } ';' ")]
        private ExportsSection ParseExportsSection() {
            var result = new ExportsSection(this);
            Require(PascalToken.Exports);
            result.ExportName = RequireIdentifier();

            do {
                result.Add(ParseExportItem());
            } while (Optional(PascalToken.Comma));
            Require(PascalToken.Semicolon);
            return result;
        }

        [Rule("ExportItem", "[ '(' FormalParameters ')' ] [ 'index' Expression ] [ 'name' Expression ]")]
        private ExportItem ParseExportItem() {
            var result = new ExportItem(this);
            if (Optional(PascalToken.OpenParen)) {
                result.Parameters = ParseFormalParameters();
                Require(PascalToken.CloseParen);
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
            result.Kind = Require(PascalToken.Var, PascalToken.ThreadVar).Kind;

            do {
                result.Add(ParseVarDeclaration());
            } while (MatchIdentifier(PascalToken.OpenBraces));

            return result;
        }

        [Rule("VarDeclaration", "[ Attributes ] IdentList ':' TypeSpecification [ VarValueSpecification ] Hints ';' ")]
        private VarDeclaration ParseVarDeclaration() {
            var result = new VarDeclaration(this);
            result.Attributes = ParseAttributes();
            result.Identifiers = ParseIdentList();
            Require(PascalToken.Colon);
            result.TypeDeclaration = ParseTypeSpecification();

            if (Match(PascalToken.Absolute, PascalToken.EqualsSign)) {
                result.ValueSpecification = ParseValueSpecification();
            }

            result.Hints = ParseHints();
            Require(PascalToken.Semicolon);
            return result;
        }

        [Rule("VarValueSpecification", "('absolute' ConstExpression) | ('=' ConstExpression)")]
        private VarValueSpecification ParseValueSpecification() {
            var result = new VarValueSpecification(this);
            if (Optional(PascalToken.Absolute)) {
                result.Absolute = ParseConstantExpression();
                return result;
            }

            Require(PascalToken.EqualsSign);
            result.InitialValue = ParseConstantExpression();
            return result;
        }

        [Rule("LabelSection", "'label' Label { ',' Label } ';' ")]
        private LabelDeclarationSection ParseLabelDeclarationSection() {
            var result = new LabelDeclarationSection(this);
            Require(PascalToken.Label);
            do {
                result.Add(ParseLabel());
            } while (Optional(PascalToken.Comma));
            Require(PascalToken.Semicolon);
            return result;
        }


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
            result.Kind = Require(PascalToken.Const, PascalToken.Resourcestring).Kind;
            while (MatchIdentifier(PascalToken.OpenBraces)) {
                result.Add(ParseConstDeclaration());
            }
            return result;
        }

        [Rule("ConstDeclaration", "[Attributes] Identifier [ ':' TypeSpecification ] = ConstantExpression Hints';'")]
        private ConstDeclaration ParseConstDeclaration() {
            var result = new ConstDeclaration(this);
            result.Attributes = ParseAttributes();
            result.Identifier = RequireIdentifier();

            if (Optional(PascalToken.Colon)) {
                result.TypeSpecification = ParseTypeSpecification();
            }

            Require(PascalToken.EqualsSign);
            result.Value = ParseConstantExpression();
            result.Hint = ParseHints();
            Require(PascalToken.Semicolon);
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

            if (Optional(PascalToken.Deprecated)) {
                result.Deprecated = true;
                if (Match(PascalToken.QuotedString))
                    result.DeprecatedComment = Require(PascalToken.QuotedString).Value;

                return result;
            }

            if (Optional(PascalToken.Experimental)) {
                result.Experimental = true;
                return result;
            }

            if (Optional(PascalToken.Platform)) {
                result.Platform = true;
                return result;
            }

            if (Optional(PascalToken.Library)) {
                result.Library = true;
                return result;
            }

            return null;
        }

        [Rule("TypeSpecification", "StructType | PointerType | StringType | ProcedureType | SimpleType ")]
        private TypeSpecification ParseTypeSpecification() {
            var result = new TypeSpecification(this);

            if (Match(PascalToken.Packed, PascalToken.Array, PascalToken.Set, PascalToken.File, //
                PascalToken.Class, PascalToken.Interface, PascalToken.Record, PascalToken.Object)) {
                result.StructuredType = ParseStructType();
                return result;
            }

            if (Match(PascalToken.Pointer, PascalToken.Circumflex)) {
                result.PointerType = ParsePointerType();
                return result;
            }

            if (Match(PascalToken.String, PascalToken.ShortString, PascalToken.AnsiString, PascalToken.UnicodeString, PascalToken.WideString)) {
                result.StringType = ParseStringType();
                return result;
            }

            if (Match(PascalToken.Function, PascalToken.Procedure, PascalToken.Reference)) {
                result.ProcedureType = ParseProcedureType();
                return result;
            }

            result.SimpleType = ParseSimpleType();
            return result;
        }

        [Rule("SimpleType", "EnumType | (ConstExpression [ '..' ConstExpression ]) | ([ 'type' ] NamespaceName [ GenericPostix ])")]
        private SimpleType ParseSimpleType() {
            var result = new SimpleType(this);

            if (Match(PascalToken.OpenParen)) {
                result.EnumType = ParseEnumType();
                return result;
            }

            result.NewType = Optional(PascalToken.TypeKeyword);

            if (result.NewType || (MatchIdentifier() && (!LookAhead(1, PascalToken.DotDot)))) {
                result.TypeId = ParseNamespaceName();
                if (Match(PascalToken.AngleBracketsOpen)) {
                    result.GenericPostfix = ParseGenericSuffix();
                }
                return result;
            }

            result.SubrangeStart = ParseConstantExpression();
            if (Optional(PascalToken.DotDot)) {
                result.SubrangeEnd = ParseConstantExpression();
            }

            return result;
        }

        [Rule("EnumType", "'(' EnumTypeValue { ',' EnumTypeValue } ')'")]
        private EnumTypeDefinition ParseEnumType() {
            var result = new EnumTypeDefinition(this);
            Require(PascalToken.OpenParen);
            do {
                result.Add(ParseEnumTypeValue());
            } while (Optional(PascalToken.Comma));
            Require(PascalToken.CloseParen);
            return result;
        }

        [Rule("EnumTypeValue", "Identifier [ '=' Expression ]")]
        private EnumValue ParseEnumTypeValue() {
            var result = new EnumValue(this);
            result.EnumName = RequireIdentifier();
            if (Optional(PascalToken.EqualsSign)) {
                result.Value = ParseExpression();
            }
            return result;
        }

        [Rule("ProcedureType", "(ProcedureRefType [ 'of' 'object' ] ( | ProcedureReference")]
        private ProcedureType ParseProcedureType() {
            var result = new ProcedureType(this);

            if (Match(PascalToken.Procedure, PascalToken.Function)) {
                result.ProcedureRefType = ParseProcedureRefType();

                if (Optional(PascalToken.Of)) {
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
            result.Kind = Require(PascalToken.Function, PascalToken.Procedure).Kind;
            if (Match(PascalToken.OpenParen)) {
                result.Parameters = ParseFormalParameterSection();
            };

            if (result.Kind == PascalToken.Function) {
                Require(PascalToken.Colon);
                result.ReturnTypeAttributes = ParseAttributes();
                result.ReturnType = ParseTypeSpecification();
            }

            return result;
        }

        [Rule("FormalParameterSection", "'(' [ FormalParameters ] ')'")]
        private FormalParameterSection ParseFormalParameterSection() {
            var result = new FormalParameterSection(this);
            Require(PascalToken.OpenParen);
            if (!Match(PascalToken.CloseParen)) {
                result.ParameterList = ParseFormalParameters();
            }
            Require(PascalToken.CloseParen);
            return result;
        }

        [Rule("StringType", "ShortString | WideString | UnicodeString |('string' [ '[' Expression ']'  ]) | ('AnsiString' '(' ConstExpression ')') ")]
        private StringType ParseStringType() {
            var result = new StringType(this);

            if (Optional(PascalToken.String)) {
                result.Kind = PascalToken.String;
                if (Optional(PascalToken.OpenBraces)) {
                    result.StringLength = ParseExpression();
                    Require(PascalToken.CloseBraces);
                };
                return result;
            }

            if (Optional(PascalToken.AnsiString)) {
                result.Kind = PascalToken.AnsiString;
                if (Optional(PascalToken.OpenParen)) {
                    result.CodePage = ParseConstantExpression();
                    Require(PascalToken.CloseParen);
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
            result.Packed = Optional(PascalToken.Packed);
            result.Part = ParseStructTypePart(this);
            return result;
        }

        [Rule("StructTypePart", "ArrayType | SetType | FileType | ClassDecl")]
        private StructTypePart ParseStructTypePart(StandardParser standardParser) {
            var result = new StructTypePart(this);

            if (Match(PascalToken.Array)) {
                result.ArrayType = ParseArrayType();
                return result;
            }

            if (Match(PascalToken.Set)) {
                result.SetType = ParseSetDefinition();
                return result;
            }

            if (Match(PascalToken.File)) {
                result.FileType = ParseFileType();
                return result;
            }

            if (Match(PascalToken.Class, PascalToken.Interface, PascalToken.Record, PascalToken.Object)) {
                result.ClassDecl = ParseClassDeclaration();
                return result;
            }

            return result;
        }

        [Rule("ClassDeclaration", "ClassOfDeclaration | ClassDefinition | ClassHelper | InterfaceDef | ObjectDecl | RecordDecl | RecordHelperDecl ")]
        private ClassTypeDeclaration ParseClassDeclaration() {
            var result = new ClassTypeDeclaration(this);

            if (Match(PascalToken.Class) && LookAhead(1, PascalToken.Of)) {
                result.ClassOf = ParseClassOfDeclaration();
                return result;
            }

            if (Match(PascalToken.Class) && LookAhead(1, PascalToken.Helper)) {
                result.ClassHelper = ParseClassHelper();
                return result;
            }

            if (Match(PascalToken.Class)) {
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

            if (Match(PascalToken.Record) && LookAhead(1, PascalToken.Helper)) {
                result.RecordHelper = ParseRecordHelper();
                return result;
            }

            if (Match(PascalToken.Record)) {
                result.RecordDecl = ParseRecordDecl();
                return result;
            }

            Unexpected();
            return result;
        }

        [Rule("RecordDecl", "'record' RecordFieldList (RecordVariantSection | RecordItems ) 'end' ")]
        private RecordDeclaration ParseRecordDecl() {
            var result = new RecordDeclaration(this);
            Require(PascalToken.Record);

            if (MatchIdentifier()) {
                result.FieldList = ParseFieldList();
            }

            if (Match(PascalToken.Case)) {
                result.VariantSection = ParseRecordVariantSection();
            }
            else {
                result.Items = ParseRecordItems();
            }
            Require(PascalToken.End);
            return result;
        }

        [Rule("RecordItems", "{ RecordItem }")]
        private RecordItems ParseRecordItems() {
            var result = new RecordItems(this);
            var unexpected = false;

            while ((!Match(PascalToken.End)) && (!unexpected)) {
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
            result.Class = Optional(PascalToken.Class);
            unexpected = false;

            if (Match(PascalToken.Procedure, PascalToken.Function, PascalToken.Constructor, PascalToken.Destructor)) {
                result.MethodDeclaration = ParseMethodDeclaration();
                return result;
            }

            if (Match(PascalToken.Property)) {
                result.PropertyDeclaration = ParsePropertyDeclaration();
                return result;
            }

            if (!result.Class && Match(PascalToken.Const)) {
                result.ConstSection = ParseConstSection();
                return result;
            }

            if (!result.Class && Match(PascalToken.TypeKeyword)) {
                result.TypeSection = ParseTypeSection();
                return result;
            }

            if (Match(PascalToken.Var)) {
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
            if (MatchIdentifier() && LookAhead(1, PascalToken.Colon)) {
                result.Name = RequireIdentifier();
                Require(PascalToken.Colon);
            }
            result.TypeDecl = ParseTypeSpecification();
            Require(PascalToken.Of);

            while (!Match(PascalToken.Undefined, PascalToken.Eof, PascalToken.End)) {
                result.Add(ParseRecordVariant());
            }

            return result;
        }

        [Rule("RecordVariant", "ConstantExpression { , ConstantExpression } ")]
        private RecordVariant ParseRecordVariant() {
            var result = new RecordVariant(this);
            do {
                result.Add(ParseConstantExpression());
            } while (Optional(PascalToken.Comma));
            Require(PascalToken.Colon);
            Require(PascalToken.OpenParen);
            result.FieldList = ParseFieldList();
            Require(PascalToken.CloseParen);
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
            Require(PascalToken.Colon);
            result.FieldType = ParseTypeSpecification();
            result.Hint = ParseHints();
            Require(PascalToken.Semicolon);
            return result;
        }

        [Rule("RecordHelperDecl", "'record' 'helper' 'for' NamespaceName RecordHelperItems 'end'")]
        private RecordHelperDef ParseRecordHelper() {
            var result = new RecordHelperDef(this);
            Require(PascalToken.Record);
            Require(PascalToken.Helper);
            Require(PascalToken.For);
            result.Name = ParseNamespaceName();
            result.Items = ParseRecordHelperItems();
            Require(PascalToken.End);
            return result;
        }

        [Rule("RecordHelperItems", " { RecordHelperItem }")]
        private RecordHelperItems ParseRecordHelperItems() {
            var result = new RecordHelperItems(this);
            var unexpected = false;

            while ((!Match(PascalToken.End)) && (!unexpected)) {
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

            if (Match(PascalToken.Procedure, PascalToken.Function, PascalToken.Constructor, PascalToken.Destructor)) {
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
            Require(PascalToken.End);
            return result;
        }

        [Rule("ObjectItems", " { ObjectItem } ")]
        private ObjectItems ParseObjectItems() {
            var result = new ObjectItems(this);
            var unexpected = false;

            while ((!Match(PascalToken.End)) && (!unexpected)) {
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

            if (Match(PascalToken.Public, PascalToken.Protected, PascalToken.Private, PascalToken.Strict, PascalToken.Published, PascalToken.Automated)) {
                result.Strict = Optional(PascalToken.Strict);
                result.Visibility = Require(PascalToken.Public, PascalToken.Protected, PascalToken.Private, PascalToken.Published, PascalToken.Automated).Kind;
                unexpected = false;
                return result;
            }

            if (Match(PascalToken.Procedure, PascalToken.Function, PascalToken.Constructor, PascalToken.Destructor)) {
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
            if (Match(PascalToken.OpenBraces))
                result.Guid = ParseInterfaceGuid();
            result.Items = ParseInterfaceItems();
            if (result.Items.Count > 0)
                Require(PascalToken.End);
            else
                Optional(PascalToken.End);
            return result;
        }

        [Rule("InterfaceItems", "{ InterfaceItem }")]
        private InterfaceItems ParseInterfaceItems() {
            var result = new InterfaceItems(this);
            var unexpected = false;

            while ((!Match(PascalToken.End)) && (!unexpected)) {
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

            if (Match(PascalToken.Procedure, PascalToken.Function)) {
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
            Require(PascalToken.OpenBraces);
            result.Id = ParseQuotedString();
            Require(PascalToken.CloseBraces);
            return result;
        }

        [Rule("ClassHelper", "'class' 'helper' ClassParent 'for' NamespaceName ClassHelperItems 'end'")]
        private ClassHelperDef ParseClassHelper() {
            var result = new ClassHelperDef(this);
            Require(PascalToken.Class);
            Require(PascalToken.Helper);
            result.ClassParent = ParseClassParent();
            Require(PascalToken.For);
            result.HelperName = ParseNamespaceName();
            result.HelperItems = ParseClassHelperItems();
            Require(PascalToken.End);
            return result;
        }

        [Rule("ClassHelperItems", " { ClassHelperItem }")]
        private ClassHelperItems ParseClassHelperItems() {
            var result = new ClassHelperItems(this);
            while (!Match(PascalToken.End, PascalToken.Undefined, PascalToken.Eof)) {
                result.Add(ParseClassHelperItem());
            }
            return result;
        }

        [Rule("ClassHelperItem", "Visibility | MethodDeclaration | PropertyDeclaration | [ 'class' ] VarSection")]
        private ClassHelperItem ParseClassHelperItem() {
            var result = new ClassHelperItem(this);
            result.Attributes = ParseAttributes();
            result.Class = Optional(PascalToken.Class);

            if (!result.Class && (result.Attributes.Count < 1) && Match(PascalToken.Public, PascalToken.Protected, PascalToken.Private, PascalToken.Strict, PascalToken.Published, PascalToken.Automated)) {
                result.Strict = Optional(PascalToken.Strict);
                result.Visibility = Require(PascalToken.Public, PascalToken.Protected, PascalToken.Private, PascalToken.Published, PascalToken.Automated).Kind;
                return result;
            }

            if (Match(PascalToken.Procedure, PascalToken.Function, PascalToken.Constructor, PascalToken.Destructor)) {
                result.MethodDeclaration = ParseMethodDeclaration();
                return result;
            }

            if (Match(PascalToken.Property)) {
                result.PropertyDeclaration = ParsePropertyDeclaration();
                return result;
            }

            if (Match(PascalToken.Var)) {
                result.VarSection = ParseVarSection();
                return result;
            }

            Unexpected();
            return result;
        }

        [Rule("ClassDefinition", "'class' [( 'sealed' | 'abstract' )] [ClassParent] ClassItems 'end' ")]
        private ClassDeclaration ParseClassDefinition() {
            var result = new ClassDeclaration(this);
            Require(PascalToken.Class);
            result.Sealed = Optional(PascalToken.Sealed);
            result.Abstract = Optional(PascalToken.Abstract);
            result.ClassParent = ParseClassParent();
            result.ClassItems = ParseClassItems();
            if (result.ClassItems.Count > 0)
                Require(PascalToken.End);
            else
                Optional(PascalToken.End);
            return result;
        }

        [Rule("ClassItems", "{ ClassItem } ")]
        private ClassDeclarationItems ParseClassItems() {
            var result = new ClassDeclarationItems(this);
            var unexpected = false;

            while ((!Match(PascalToken.End)) && (!unexpected)) {
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
            result.Class = Optional(PascalToken.Class);
            unexpected = false;

            if (!result.Class && (result.Attributes.Count < 1) && Match(PascalToken.Public, PascalToken.Protected, PascalToken.Private, PascalToken.Strict, PascalToken.Published, PascalToken.Automated)) {
                result.Strict = Optional(PascalToken.Strict);
                result.Visibility = Require(PascalToken.Public, PascalToken.Protected, PascalToken.Private, PascalToken.Published, PascalToken.Automated).Kind;
                return result;
            }

            if (Match(PascalToken.Procedure, PascalToken.Function) && HasTokenBeforeToken(PascalToken.EqualsSign, PascalToken.Semicolon, PascalToken.OpenParen)) {
                result.MethodResolution = ParseMethodResolution();
                return result;
            }

            if (Match(PascalToken.Procedure, PascalToken.Function, PascalToken.Constructor, PascalToken.Destructor)) {
                result.MethodDeclaration = ParseMethodDeclaration();
                return result;
            }

            if (Match(PascalToken.Property)) {
                result.PropertyDeclaration = ParsePropertyDeclaration();
                return result;
            }

            if (!result.Class && Match(PascalToken.Const)) {
                result.ConstSection = ParseConstSection();
                return result;
            }

            if (!result.Class && Match(PascalToken.TypeKeyword)) {
                result.TypeSection = ParseTypeSection();
                return result;
            }

            if (Match(PascalToken.Var)) {
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
            Require(PascalToken.Colon);
            result.TypeDecl = ParseTypeSpecification();
            result.Hint = ParseHints();
            Require(PascalToken.Semicolon);
            return result;
        }

        [Rule("PropertyDeclaration", "'property' Identifier [ '[' FormalParameters  ']' ] [ ':' NamespaceName ] [ 'index' Expression ]  { ClassPropertySpecifier } ';' ")]
        private ClassProperty ParsePropertyDeclaration() {
            var result = new ClassProperty(this);
            Require(PascalToken.Property);
            result.PropertyName = RequireIdentifier();
            if (Optional(PascalToken.OpenBraces)) {
                result.ArrayIndex = ParseFormalParameters();
                Require(PascalToken.CloseBraces);
            }

            if (Optional(PascalToken.Colon)) {
                result.TypeName = ParseNamespaceName();
            }

            if (Optional(PascalToken.Index)) {
                result.PropertyIndex = ParseExpression();
            }

            while (Match(PascalToken.Read, PascalToken.Write, PascalToken.Add, PascalToken.Remove, PascalToken.ReadOnly, PascalToken.WriteOnly, PascalToken.DispId)) {
                result.Add(ParseClassPropertyAccessSpecifier());
            }

            Require(PascalToken.Semicolon);

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
                Require(PascalToken.Semicolon);
                return result;
            }

            if (Optional(PascalToken.Default)) {
                result.IsDefault = true;
                if (!Optional(PascalToken.Semicolon)) {
                    result.DefaultProperty = ParseExpression();
                    Require(PascalToken.Semicolon);
                }
                return result;
            }

            if (Optional(PascalToken.NoDefault)) {
                result.NoDefault = true;
                Require(PascalToken.Semicolon);
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
                Require(PascalToken.Semicolon);
                return result;
            }

            if (Optional(PascalToken.WriteOnly)) {
                result.WriteOnly = true;
                Require(PascalToken.Semicolon);
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
            Require(PascalToken.Semicolon);
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
            Require(PascalToken.TypeKeyword);

            do {
                result.Add(ParseTypeDeclaration());
            } while (MatchIdentifier(PascalToken.OpenBraces));

            return result;
        }

        [Rule("TypeDeclaration", "[ Attributes ] GenericTypeIdent '=' TypeDeclaration Hints ';' ")]
        private TypeDeclaration ParseTypeDeclaration() {
            var result = new TypeDeclaration(this);
            result.Attributes = ParseAttributes();
            result.TypeId = ParseGenericTypeIdent();
            Require(PascalToken.EqualsSign);
            result.TypeSpecification = ParseTypeSpecification();
            result.Hint = ParseHints();
            Require(PascalToken.Semicolon);
            return result;
        }

        [Rule("GenericTypeIdent", "Ident [ GenericDefintion ] ")]
        private GenericTypeIdent ParseGenericTypeIdent() {
            var result = new GenericTypeIdent(this);
            result.Ident = RequireIdentifier();
            if (Match(PascalToken.AngleBracketsOpen)) {
                result.GenericDefinition = ParseGenericDefinition();
            }
            return result;
        }

        [Rule("MethodResolution", "( 'function' | 'procedure' ) NamespaceName '=' Identifier ';' ")]
        private MethodResolution ParseMethodResolution() {
            var result = new MethodResolution(this);
            result.Kind = Require(PascalToken.Function, PascalToken.Procedure).Kind;
            result.TypeName = ParseNamespaceName();
            Require(PascalToken.EqualsSign);
            result.ResolveIdentifier = RequireIdentifier();
            Require(PascalToken.Semicolon);
            return result;
        }

        [Rule("MethodDeclaration", "( 'constructor' | 'destructor' | 'procedure' | 'function' ) Identifier [GenericDefinition] [FormalParameters] [ ':' [ Attributes ] TypeSpecification ] ';' { MethodDirective } ")]
        private ClassMethod ParseMethodDeclaration() {
            var result = new ClassMethod(this);
            result.MethodKind = Require(new[] { PascalToken.Constructor, PascalToken.Destructor, PascalToken.Function, PascalToken.Procedure }).Kind;
            result.Identifier = RequireIdentifier();

            if (Match(PascalToken.AngleBracketsOpen)) {
                result.GenericDefinition = ParseGenericDefinition();
            }

            if (Optional(PascalToken.OpenParen) && (!Optional(PascalToken.CloseParen))) {
                result.Parameters = ParseFormalParameters();
                Require(PascalToken.CloseParen);
            }

            if (Optional(PascalToken.Colon)) {
                result.ResultAttributes = ParseAttributes();
                result.ResultType = ParseTypeSpecification();
            }

            Require(PascalToken.Semicolon);
            result.Directives = ParseMethodDirectives();
            return result;
        }

        [Rule("FormalParameters", "FormalParameter { ';' FormalParameter }")]
        private FormalParameters ParseFormalParameters() {
            var result = new FormalParameters(this);
            do {
                result.Add(ParseFormalParameter());
            } while (Optional(PascalToken.Semicolon));
            return result;
        }

        [Rule("FormalParameter", "[Attributes] [( 'const' | 'var' | 'out' )] IdentList [ ':' TypeDeclaration ] [ '=' Expression ]")]
        private FormalParameter ParseFormalParameter() {
            var result = new FormalParameter(this);
            result.Attributes = ParseAttributes();

            if (Match(PascalToken.Const, PascalToken.Var, PascalToken.Out)) {
                result.ParamType = Require(PascalToken.Const, PascalToken.Var, PascalToken.Out).Kind;
            }

            result.ParamNames = ParseIdentList();

            if (Optional(PascalToken.Colon)) {
                result.TypeDeclaration = ParseTypeSpecification();
            }

            if (Optional(PascalToken.EqualsSign)) {
                result.DefaultValue = ParseExpression();
            }

            return result;
        }

        [Rule("IdentList", "Identifiert { ',' Identifier }")]
        private IdentList ParseIdentList() {
            var result = new IdentList(this);
            do {
                result.Add(RequireIdentifier());
            } while (Optional(PascalToken.Comma));
            return result;
        }

        [Rule("GenericDefinition", "SimpleGenericDefinition | ConstrainedGenericDefinition")]
        private GenericDefinition ParseGenericDefinition() {
            if (!LookAhead(2, PascalToken.Comma)) {
                return ParseConstrainedGenericDefinition();
            }

            return ParseSimpleGenericDefinition();
        }

        [Rule("SimpleGenericDefinition", "'<' Identifier { ',' Identifier } '>'")]
        private GenericDefinition ParseSimpleGenericDefinition() {
            var result = new GenericDefinition(this);
            Require(PascalToken.AngleBracketsOpen);

            do {
                var part = new GenericDefinitionPart(this);
                part.Identifier = RequireIdentifier();
                result.Add(part);
            } while (Optional(PascalToken.Comma));

            Require(PascalToken.AngleBracketsClose);
            return result;

        }

        [Rule("ConstrainedGenericDefinition", "'<' GenericDefinitionPart { ';' GenericDefinitionPart } '>'")]
        private GenericDefinition ParseConstrainedGenericDefinition() {
            var result = new GenericDefinition(this);
            Require(PascalToken.AngleBracketsOpen);

            do {
                result.Add(ParseGenericDefinitionPart());
            } while (Optional(PascalToken.Semicolon));

            Require(PascalToken.AngleBracketsClose);
            return result;

        }

        [Rule("GenericDefinitionPart", "Identifier [ ':' GenericConstraint { ',' GenericConstraint } ]")]
        private GenericDefinitionPart ParseGenericDefinitionPart() {
            var result = new GenericDefinitionPart(this);
            result.Identifier = RequireIdentifier();

            if (Optional(PascalToken.Colon)) {
                do {
                    result.Add(ParseGenericConstraint());
                } while (Optional(PascalToken.Comma));
            }

            return result;
        }

        [Rule("GenericConstraint", " 'record' | 'class' | 'constructor' | Identifier ")]
        private ConstrainedGeneric ParseGenericConstraint() {
            var result = new ConstrainedGeneric(this);

            if (Optional(PascalToken.Record)) {
                result.RecordConstraint = true;
            }
            else if (Optional(PascalToken.Class)) {
                result.ClassConstraint = true;
            }
            else if (Optional(PascalToken.Constructor)) {
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
            if (Optional(PascalToken.OpenParen)) {
                do {
                    result.Add(ParseNamespaceName());
                } while (Optional(PascalToken.Comma));
                Require(PascalToken.CloseParen);
            }
            return result;
        }

        [Rule("ClassOfDeclaration", "'class' 'of' NamespaceName")]
        private ClassOfDeclaration ParseClassOfDeclaration() {
            var result = new ClassOfDeclaration(this);
            Require(PascalToken.Class);
            Require(PascalToken.Of);
            result.TypeName = ParseNamespaceName();
            return result;
        }

        [Rule("FileType", "'file' [ 'of' TypeSpecification ]")]
        private FileType ParseFileType() {
            var result = new FileType(this);
            Require(PascalToken.File);
            if (Optional(PascalToken.Of)) {
                result.TypeDefinition = ParseTypeSpecification();
            }
            return result;
        }

        [Rule("SetDef", "'set' 'of' TypeSpecification")]
        private SetDef ParseSetDefinition() {
            var result = new SetDef(this);
            Require(PascalToken.Set);
            Require(PascalToken.Of);
            result.TypeDefinition = ParseTypeSpecification();
            return result;
        }

        [Rule("TypeAlias", "'type' NamespaceName")]
        private TypeAliasDefinition ParseTypeAlias() {
            var result = new TypeAliasDefinition(this);
            Require(PascalToken.TypeKeyword);
            result.TypeName = ParseNamespaceName();

            if (Match(PascalToken.AngleBracketsOpen)) {
                result.GenericSuffix = ParseGenericSuffix();
            }

            return result;
        }

        [Rule("GenericSuffix", "'<' TypeDefinition { ',' TypeDefinition '}' '>'")]
        private GenericTypesuffix ParseGenericSuffix() {
            var result = new GenericTypesuffix(this);
            Require(PascalToken.AngleBracketsOpen);

            do {
                result.Add(ParseTypeSpecification());
            } while (Optional(PascalToken.Comma));

            Require(PascalToken.AngleBracketsClose);
            return result;
        }

        [Rule("ArrayType", " 'array' [ '[' ArrayIndex { ',' ArrayIndex } ']']  'of' ( 'const' | TypeDefinition ) ")]
        private ArrayType ParseArrayType() {
            var result = new ArrayType(this);
            Require(PascalToken.Array);
            if (Optional(PascalToken.OpenBraces)) {
                do {
                    result.Add(ParseArrayIndex());
                } while (Optional(PascalToken.Comma));
                Require(PascalToken.CloseBraces);
            }
            Require(PascalToken.Of);

            if (Optional(PascalToken.Const)) {
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
            if (Optional(PascalToken.DotDot)) {
                result.EndIndex = ParseConstantExpression();
            }
            return result;
        }

        [Rule("PointerType", "( 'pointer' | '^' TypeSpecification )")]
        private PointerType ParsePointerType() {
            var result = new PointerType(this);

            if (Optional(PascalToken.Pointer)) {
                result.GenericPointer = true;
                return result;
            }

            Require(PascalToken.Circumflex);
            result.TypeSpecification = ParseTypeSpecification();
            return result;
        }

        [Rule("Attributes", "{ '[' Attribute ']' }")]
        private UserAttributes ParseAttributes() {
            var userAttributes = new UserAttributes(this);
            while (Optional(PascalToken.OpenBraces)) {
                do {
                    userAttributes.Add(ParseAttribute());
                } while (Optional(PascalToken.Comma));
                Require(PascalToken.CloseBraces);
            }
            return userAttributes;
        }

        [Rule("Attribute", "NamespaceName [ '(' Expressions ')' ]")]
        private UserAttribute ParseAttribute() {
            var result = new UserAttribute(this);
            result.Name = ParseNamespaceName();
            if (Optional(PascalToken.OpenParen)) {
                while (!Match(PascalToken.CloseParen)) {
                    result.Expressions = ParseExpressions();
                }
                Require(PascalToken.CloseParen);
            }
            return result;
        }

        [Rule("Expressions", "Expression { ',' Expression }")]
        private ExpressionList ParseExpressions() {
            var result = new ExpressionList(this);
            do {
                result.Add(ParseExpression());
            } while (Optional(PascalToken.Comma));
            return result;
        }

        [Rule("ConstantExpression", " '(' ( RecordConstant | ConstantExpression ) ')' | Expression")]
        private ConstantExpression ParseConstantExpression() {
            var result = new ConstantExpression(this);

            if (Optional(PascalToken.OpenParen)) {
                if (MatchIdentifier() && (LookAhead(1, PascalToken.Colon)))
                    result.RecordConstant = ParseRecordConstant();
                else do {
                        result.Add(ParseConstantExpression());
                    } while (Optional(PascalToken.Comma));
                Require(PascalToken.CloseParen);
            }
            else {
                result.Value = ParseExpression();
            }

            return result;
        }

        private RecordConstantExpression ParseRecordConstant() {
            var result = new RecordConstantExpression(this);
            result.Name = RequireIdentifier();
            Require(PascalToken.Colon);
            result.Value = ParseConstantExpression();
            return result;
        }

        private ExpressionBase ParseExpression() => RequireInteger();

        private PascalInteger RequireInteger()
            => new PascalInteger(Require(PascalToken.Integer), this);

        private PascalIdentifier RequireIdentifier() {
            if (Match(PascalToken.Identifier)) {
                return new PascalIdentifier(Require(PascalToken.Identifier), this);
            };

            if (!reservedWords.Contains(CurrentToken().Kind)) {
                return new PascalIdentifier(Require(CurrentToken().Kind), this);
            }

            Unexpected();
            return new PascalIdentifier(new PascalToken() { Kind = PascalToken.Undefined, Value = "" }, this);
        }

        [Rule("UsesFileClause", "'uses' NamespaceFileNameList")]
        private UsesFileClause ParseUsesFileClause() {
            var result = new UsesFileClause(this);
            Require(PascalToken.Uses);
            result.Files = ParseNamespaceFileNameList();
            return result;
        }

        [Rule("NamespaceFileName", "NamespaceName [ 'in' QuotedString ]")]
        private NamespaceFileName ParseNamespaceFileName() {
            var result = new NamespaceFileName(this);
            result.NamespaceName = ParseNamespaceName();
            if (Optional(PascalToken.In))
                result.QuotedFileName = ParseQuotedString();
            return result;
        }

        private QuotedString ParseQuotedString() {
            var result = new QuotedString(this);
            result.UnquotedValue = Require(PascalToken.QuotedString).Value;
            return result;
        }

        private NamespaceName ParseNamespaceName() {
            var result = new NamespaceName(this);
            result.Add(RequireIdentifier());

            while (Optional(PascalToken.Dot)) {
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