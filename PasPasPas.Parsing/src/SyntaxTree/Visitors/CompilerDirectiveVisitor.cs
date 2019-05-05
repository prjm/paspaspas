using System;
using PasPasPas.Globals.Log;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Options.Bundles;
using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.SyntaxTree.CompilerDirectives;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     visitor for compiler directives
    /// </summary>
    public class CompilerDirectiveVisitor :
        IStartVisitor<AlignSwitch>,
        IStartVisitor<AppTypeParameter>,
        IStartVisitor<CodeAlignParameter>,
        IStartVisitor<DefineSymbol>,
        IStartVisitor<DebugInfoSwitch>,
        IStartVisitor<UnDefineSymbol>,
        IStartVisitor<IfDef>,
        IStartVisitor<EndIf>,
        IStartVisitor<BooleanEvaluationSwitch>,
        IStartVisitor<Description>,
        IStartVisitor<ParseDenyPackageUnit>,
        IStartVisitor<AssertSwitch>,
        IStartVisitor<IfDirective>,
        IStartVisitor<DesignOnly>,
        IStartVisitor<ExtendedCompatibility>,
        IStartVisitor<Extension>,
        IStartVisitor<ExternalSymbolDeclaration>,
        IStartVisitor<HighCharUnicodeSwitch>,
        IStartVisitor<ExcessPrecision>,
        IStartVisitor<ExtSyntax>,
        IStartVisitor<HppEmit>,
        IStartVisitor<Hints>,
        IStartVisitor<ImageBase>,
        IStartVisitor<ImplicitBuild>,
        IStartVisitor<IoChecks>,
        IStartVisitor<LocalSymbols>,
        IStartVisitor<Include>,
        IStartVisitor<Link>,
        IStartVisitor<Message>,
        IStartVisitor<OpenStrings>,
        IStartVisitor<Optimization>,
        IStartVisitor<SafeDivide>,
        IStartVisitor<RangeChecks>,
        IStartVisitor<Resource>,
        IStartVisitor<StackFrames>,
        IStartVisitor<ZeroBasedStrings>,
        IStartVisitor<Overflow>,
        IStartVisitor<LongStrings>,
        IStartVisitor<WritableConsts>,
        IStartVisitor<ElseDirective>,
        IStartVisitor<VarStringChecks>,
        IStartVisitor<TypedPointers>,
        IStartVisitor<Warnings>,
        IStartVisitor<SymbolDefinitions>,
        IStartVisitor<StrongLinkTypes>,
        IStartVisitor<ScopedEnums>,
        IStartVisitor<PublishedRtti>,
        IStartVisitor<RunOnly>,
        IStartVisitor<LegacyIfEnd>,
        IStartVisitor<RealCompatibility>,
        IStartVisitor<PointerMath>,
        IStartVisitor<NoDefine>,
        IStartVisitor<OldTypeLayout>,
        IStartVisitor<ObjTypeName>,
        IStartVisitor<NoInclude>,
        IStartVisitor<MinEnumSize>,
        IStartVisitor<MethodInfo>,
        IStartVisitor<LibInfo>,
        IStartVisitor<WeakPackageUnit>,
        IStartVisitor<RttiControl>,
        IStartVisitor<IfOpt>,
        IStartVisitor<ParsedVersion>,
        IStartVisitor<Region>,
        IStartVisitor<EndRegion>,
        IStartVisitor<ImportedData>,
        IStartVisitor<StackMemorySize>,
        IStartVisitor<WarnSwitch>,
        IStartVisitor<WeakLinkRtti>,
        IStartVisitor<ObjectExport>,
        IStartVisitor<VarPropSetter> {

        private readonly Visitor visitor;
        private readonly ILogManager log;
        private OptionSet Options { get; }
        private readonly LogSource logSource;
        private readonly FileReference path;

        private readonly Guid logSourceId
             = new Guid(new byte[] { 0x67, 0x23, 0x1b, 0x2e, 0xf6, 0x4b, 0xdf, 0x40, 0xac, 0xf8, 0x2, 0xc3, 0x1d, 0x7c, 0x2e, 0xf2 });
        /* {2e1b2367-4bf6-40df-acf8-02c31d7c2ef2} */

        /// <summary>
        ///     creates a new visitor
        /// </summary>
        public CompilerDirectiveVisitor(OptionSet options, FileReference filePath, ILogManager logMgr) {
            Options = options;
            visitor = new Visitor(this);
            log = logMgr;
            path = filePath;
            logSource = new LogSource(log, logSourceId);
        }

        /// <summary>
        ///     compile options
        /// </summary>
        public CompileOptions CompilerOptions
            => Options.CompilerOptions;

        /// <summary>
        ///     conditional compilation options
        /// </summary>
        public ConditionalCompilationOptions ConditionalCompilation
            => Options.ConditionalCompilation;

        /// <summary>
        ///     warnings
        /// </summary>
        public WarningOptions Warnings
            => Options.Warnings;

        /// <summary>
        ///     log source
        /// </summary>
        public LogSource LogSource
            => logSource;

        /// <summary>
        ///     meta information
        /// </summary>
        public MetaInformation Meta
            => Options.Meta;

        /// <summary>
        ///     include reader
        /// </summary>
        public StackedFileReader IncludeInput { get; set; }

        /// <summary>
        ///     test if an item can be visited
        /// </summary>
        /// <param name="syntaxPart">syntax part to test</param>
        /// <returns></returns>
        private bool CanVisit(ISyntaxPart syntaxPart) {
            if (!ConditionalCompilation.Skip)
                return true;

            return syntaxPart is EndIf ||
                syntaxPart is IfDef ||
                syntaxPart is IfDirective ||
                syntaxPart is IfOpt ||
                syntaxPart is ElseDirective;
        }

        /// <summary>
        ///     align switch
        /// </summary>
        /// <param name="element">switch to visit</param>
        public void StartVisit(AlignSwitch element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.CodeGeneration.Align.Value = element.AlignValue;
        }


        /// <summary>
        ///     update application type
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(AppTypeParameter element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.LinkOptions.ApplicationType.Value = element.ApplicationType;
        }

        /// <summary>
        ///     update assertion mode
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(AssertSwitch element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.DebugOptions.Assertions.Value = element.Assertions;
        }

        /// <summary>
        ///     update assertion mode
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(BooleanEvaluationSwitch element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.Syntax.BoolEval.Value = element.BoolEval;
        }

        /// <summary>
        ///     update code align
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(CodeAlignParameter element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.CodeGeneration.CodeAlign.Value = element.CodeAlign;
        }

        /// <summary>
        ///     update debug info mode
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(DebugInfoSwitch element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.DebugOptions.DebugInfo.Value = element.DebugInfo;
        }

        /// <summary>
        ///     define symbol
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(DefineSymbol element) {
            if (!CanVisit(element))
                return;

            ConditionalCompilation.DefineSymbol(element.SymbolName);
        }

        /// <summary>
        ///     undefine symbol
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(UnDefineSymbol element) {
            if (!CanVisit(element))
                return;

            ConditionalCompilation.UndefineSymbol(element.SymbolName);
        }

        /// <summary>
        ///     conditional compilation ("ifdef")
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(IfDef element) {
            if (!CanVisit(element))
                return;

            if (element.Negate)
                ConditionalCompilation.AddIfNDefCondition(element.SymbolName);
            else
                ConditionalCompilation.AddIfDefCondition(element.SymbolName);
        }

        /// <summary>
        ///     conditional compilation ("endif")
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(EndIf element) {
            if (!CanVisit(element))
                return;


            if (ConditionalCompilation.HasConditions) {
                ConditionalCompilation.RemoveIfDefCondition();
            }
            else {
                LogSource.LogError(CompilerDirectiveParserErrors.EndIfWithoutIf, element);
            }
        }

        /// <summary>
        ///     deny unit in package switch
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ParseDenyPackageUnit element) {
            if (!CanVisit(element))
                return;

            ConditionalCompilation.DenyInPackages.Value = element.DenyUnit;
        }

        /// <summary>
        ///     description
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(Description element) {
            if (!CanVisit(element))
                return;

            Meta.Description.Value = element.DescriptionValue;
        }

        /// <summary>
        ///     <c>$if</c>
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(IfDirective element) {
            if (!CanVisit(element))
                return;

            ConditionalCompilation.AddIfCondition(false);
        }

        /// <summary>
        ///     conditional compilation ("else")
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ElseDirective element) {
            if (!CanVisit(element))
                return;

            if (ConditionalCompilation.HasConditions) {
                ConditionalCompilation.AddElseCondition();
            }
            else {
                LogSource.LogError(CompilerDirectiveParserErrors.ElseIfWithoutIf, element);
            }
        }

        /// <summary>
        ///     design time only
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(DesignOnly element) {
            if (!CanVisit(element))
                return;

            ConditionalCompilation.DesignOnly.Value = element.DesignTimeOnly;
        }

        /// <summary>
        ///     extension
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(Extension element) {
            if (!CanVisit(element))
                return;

            Meta.FileExtension.Value = element.ExecutableExtension;
        }


        /// <summary>
        ///     extended compatibility
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ExtendedCompatibility element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.AdditionalOptions.ExtendedCompatibility.Value = element.Mode;
        }

        /// <summary>
        ///     extended syntax
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ExtSyntax element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.Syntax.UseExtendedSyntax.Value = element.Mode;
        }

        /// <summary>
        ///     external symbol
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ExternalSymbolDeclaration element) {
            if (!CanVisit(element))
                return;

            if (element.Identifier.GetSymbolKind() != TokenKind.Identifier)
                return;

            Meta.RegisterExternalSymbol(element.IdentifierName, element.SymbolName, element.UnionName);
        }


        /// <summary>
        ///     excess precision
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ExcessPrecision element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.AdditionalOptions.ExcessPrecision.Value = element.Mode;
        }

        /// <summary>
        ///     high char unicode switch
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(HighCharUnicodeSwitch element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.AdditionalOptions.HighCharUnicode.Value = element.Mode;
        }

        /// <summary>
        ///     hint switch
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(Hints element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.HintsAndWarnings.Hints.Value = element.Mode;
        }


        /// <summary>
        ///     c++ header emit
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(HppEmit element) {
            if (!CanVisit(element))
                return;

            if (element.Mode == HppEmitMode.Undefined)
                return;

            Meta.HeaderEmit(element.Mode, element.EmitValue);
        }


        /// <summary>
        ///     image base
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ImageBase element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.LinkOptions.ImageBase.Value = element.BaseValue;
        }

        /// <summary>
        ///     implicit build
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ImplicitBuild element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.AdditionalOptions.ImplicitBuild.Value = element.Mode;
        }

        /// <summary>
        ///     io checks
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(IoChecks element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.RuntimeChecks.IoChecks.Value = element.Mode;
        }

        /// <summary>
        ///     local symbols
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(LocalSymbols element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.DebugOptions.LocalSymbols.Value = element.Mode;
        }


        /// <summary>
        ///     long strings
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(LongStrings element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.Syntax.LongStrings.Value = element.Mode;
        }

        /// <summary>
        ///     open strings
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(OpenStrings element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.Syntax.OpenStrings.Value = element.Mode;
        }


        /// <summary>
        ///        optimization
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(Optimization element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.CodeGeneration.Optimization.Value = element.Mode;
        }


        /// <summary>
        ///        overflow
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(Overflow element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.RuntimeChecks.CheckOverflows.Value = element.Mode;
        }

        /// <summary>
        ///         safe division
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(SafeDivide element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.CodeGeneration.SafeDivide.Value = element.Mode;
        }

        /// <summary>
        ///        range checks
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(RangeChecks element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.RuntimeChecks.RangeChecks.Value = element.Mode;
        }

        /// <summary>
        ///        stack frames
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(StackFrames element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.CodeGeneration.StackFrames.Value = element.Mode;
        }

        /// <summary>
        ///        zero based strings
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ZeroBasedStrings element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.Syntax.IndexOfFirstCharInString.Value = element.Mode;
        }

        /// <summary>
        ///     writable constants
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(WritableConsts element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.Syntax.WritableConstants.Value = element.Mode;
        }

        /// <summary>
        ///     weak link rtti
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(WeakLinkRtti element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.LinkOptions.WeakLinkRtti.Value = element.Mode;
        }

        /// <summary>
        ///     weak link rtti
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(Warnings element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.HintsAndWarnings.Warnings.Value = element.Mode;
        }

        /// <summary>
        ///     warn switch
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(WarnSwitch element) {
            if (!CanVisit(element))
                return;

            if (string.IsNullOrWhiteSpace(element.WarningType))
                return;

            Warnings.SetModeByIdentifier(element.WarningType, element.Mode);
        }

        /// <summary>
        ///     var string checks
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(VarStringChecks element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.Syntax.VarStringChecks.Value = element.Mode;
        }

        /// <summary>
        ///     type checked pointers
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(TypedPointers element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.Syntax.TypedPointers.Value = element.Mode;
        }

        /// <summary>
        ///     symbol definitions pointers
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(SymbolDefinitions element) {
            if (!CanVisit(element))
                return;


            if (element.Mode != SymbolDefinitionInfo.Undefined)
                CompilerOptions.DebugOptions.SymbolDefinitions.Value = element.Mode;

            if (element.ReferencesMode != SymbolReferenceInfo.Undefined)
                CompilerOptions.DebugOptions.SymbolReferences.Value = element.ReferencesMode;
        }


        /// <summary>
        ///     type chedk pointers
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(StrongLinkTypes element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.LinkOptions.LinkAllTypes.Value = element.Mode;
        }


        /// <summary>
        ///     scoped enumerations directive
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ScopedEnums element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.Syntax.ScopedEnums.Value = element.Mode;
        }

        /// <summary>
        ///     published rtti
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(PublishedRtti element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.CodeGeneration.PublishedRtti.Value = element.Mode;
        }

        /// <summary>
        ///      runtime only
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(RunOnly element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.LinkOptions.RuntimeOnlyPackage.Value = element.Mode;
        }


        /// <summary>
        ///      legacy if end
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(LegacyIfEnd element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.AdditionalOptions.LegacyIfEnd.Value = element.Mode;
        }

        /// <summary>
        ///      legacy if end
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(RealCompatibility element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.AdditionalOptions.RealCompatibility.Value = element.Mode;
        }



        /// <summary>
        ///      legacy if end
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(PointerMath element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.Syntax.PointerMath.Value = element.Mode;
        }

        /// <summary>
        ///      old type layout
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(OldTypeLayout element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.AdditionalOptions.OldTypeLayout.Value = element.Mode;
        }

        /// <summary>
        ///      nodefine
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(NoDefine element) {
            if (!CanVisit(element))
                return;

            if (!string.IsNullOrEmpty(element.TypeName))
                Meta.AddNoDefine(element.TypeName, element.TypeNameInHpp, element.TypeNameInUnion);
        }

        /// <summary>
        ///      obj type name
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ObjTypeName element) {

            if (!CanVisit(element))
                return;


            if (!string.IsNullOrEmpty(element.TypeName))
                Meta.AddObjectFileTypeName(element.TypeName, element.AliasName);
        }


        /// <summary>
        ///      noinclude
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(NoInclude element) {
            if (!CanVisit(element))
                return;

            if (!string.IsNullOrEmpty(element.UnitName))
                Meta.AddNoInclude(element.UnitName);
        }


        /// <summary>
        ///      min enum size
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(MinEnumSize element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.CodeGeneration.MinimumEnumSize.Value = element.Size;
        }


        /// <summary>
        ///      start region
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(Region element) {
            if (!CanVisit(element))
                return;

            if (!string.IsNullOrWhiteSpace(element.RegionName))
                Meta.StartRegion(element.RegionName);
        }


        /// <summary>
        ///      end region
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(EndRegion element) {
            if (!CanVisit(element))
                return;


            if (Meta.Regions.Count > 0)
                Meta.StopRegion();
            else
                LogSource.LogError(CompilerDirectiveParserErrors.EndRegionWithoutRegion, element);
        }


        /// <summary>
        ///      method info
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(MethodInfo element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.CodeGeneration.MethodInfo.Value = element.Mode;
        }

        /// <summary>
        ///      libprefix / libsuffix / libversion
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(LibInfo element) {
            if (!CanVisit(element))
                return;

            if (element.LibPrefix != null)
                Meta.LibPrefix.Value = element.LibPrefix;

            if (element.LibSuffix != null)
                Meta.LibSuffix.Value = element.LibSuffix;

            if (element.LibVersion != null)
                Meta.LibVersion.Value = element.LibVersion;

        }

        /// <summary>
        ///     pe version
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ParsedVersion element) {
            if (!CanVisit(element))
                return;

            switch (element.Kind) {
                case TokenKind.SetPEOsVersion:
                    Meta.PEOsVersion.MajorVersion.Value = element.MajorVersion;
                    Meta.PEOsVersion.MinorVersion.Value = element.MinorVersion;
                    break;

                case TokenKind.SetPESubsystemVersion:
                    Meta.PESubsystemVersion.MajorVersion.Value = element.MajorVersion;
                    Meta.PESubsystemVersion.MinorVersion.Value = element.MinorVersion;
                    break;

                case TokenKind.SetPEUserVersion:
                    Meta.PEUserVersion.MajorVersion.Value = element.MajorVersion;
                    Meta.PEUserVersion.MinorVersion.Value = element.MinorVersion;
                    break;
            }
        }

        /// <summary>
        ///      weak package unit
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(WeakPackageUnit element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.LinkOptions.WeakPackageUnit.Value = element.Mode;
        }

        /// <summary>
        ///     visit an object export directive
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ObjectExport element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.LinkOptions.ExportCppObjects.Value = element.Mode;
        }

        /// <summary>
        ///      weak package unit
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(RttiControl element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.CodeGeneration.Rtti.Mode = element.Mode;
            CompilerOptions.CodeGeneration.Rtti.AssignVisibility(element.Properties, element.Methods, element.Fields);
        }


        /// <summary>
        ///      if opt
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(IfOpt element) {
            if (!CanVisit(element))
                return;

            ConditionalCompilation.AddIfOptCondition(element.SwitchKind, element.SwitchInfo, element.SwitchState);
        }

        /// <summary>
        ///      imported data
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ImportedData element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.DebugOptions.ImportedData.Value = element.Mode;
        }

        /// <summary>
        ///      min / max stack size
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(StackMemorySize element) {
            if (!CanVisit(element))
                return;

            if (element.MinStackSize != null)
                CompilerOptions.LinkOptions.MinimumStackMemorySize.Value = element.MinStackSize.Value;
            if (element.MaxStackSize != null)
                CompilerOptions.LinkOptions.MaximumStackMemorySize.Value = element.MaxStackSize.Value;
        }

        /// <summary>
        ///      message
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(Message element) {
            if (element.MessageType == MessageSeverity.Undefined)
                return;

            LogSource.ProcessMessage(new LogMessage(element.MessageType, ParserBase.ParserLogMessage, ParserBase.UserGeneratedMessage, element.MessageType, element.MessageText));
        }

        /// <summary>
        ///      var prop setter
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(VarPropSetter element) {
            if (!CanVisit(element))
                return;

            CompilerOptions.Syntax.VarPropSetter.Value = element.Mode;
        }


        /// <summary>
        ///     link
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(Link element) {
            if (!CanVisit(element))
                return;

            var basePath = path;
            var fileName = element?.FileName;

            if (basePath == null || string.IsNullOrWhiteSpace(basePath.Path))
                return;

            if (fileName == null || string.IsNullOrWhiteSpace(fileName))
                return;

            var resolvedFile = Meta.LinkedFileResolver.ResolvePath(basePath, new FileReference(fileName));

            var linkedFile = new LinkedFile() {
                OriginalFileName = element.FileName,
                TargetPath = resolvedFile.TargetPath,
                IsResolved = resolvedFile.IsResolved
            };
            Meta.AddLinkedFile(linkedFile);
        }


        /// <summary>
        ///     resource
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(Resource element) {
            if (!CanVisit(element))
                return;

            var basePath = path;
            var fileName = element?.FileName;

            if (basePath == null || string.IsNullOrWhiteSpace(basePath.Path))
                return;

            if (fileName == null || string.IsNullOrWhiteSpace(fileName))
                return;


            var resolvedFile = Meta.ResourceFilePathResolver.ResolvePath(basePath, new FileReference(fileName));

            var resourceReference = new ResourceReference() {
                OriginalFileName = fileName,
                TargetPath = resolvedFile.TargetPath,
                RcFile = element.RcFile,
                IsResolved = resolvedFile.IsResolved
            };
            Meta.AddResourceReference(resourceReference);
        }

        /// <summary>
        ///     include
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(Include element) {
            if (!CanVisit(element))
                return;

            var basePath = path;
            var fileName = element?.FileName;

            if (basePath == null || string.IsNullOrWhiteSpace(basePath.Path))
                return;

            if (fileName == null || string.IsNullOrWhiteSpace(fileName))
                return;

            var targetPath = Meta.IncludePathResolver.ResolvePath(
                basePath, new FileReference(fileName)).TargetPath;

            if (IncludeInput != null) {
                IncludeInput.AddFileToRead(targetPath);
            }
        }

        /// <summary>
        ///     common visitor
        /// </summary>
        /// <returns></returns>
        public IStartEndVisitor AsVisitor()
            => visitor;

    }
}
