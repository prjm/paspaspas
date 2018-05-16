using System;
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
        IStartVisitor<WeakLinkRtti> {

        private readonly Visitor visitor;
        private readonly ILogManager log;
        private OptionSet Options { get; }
        private readonly LogSource logSource;
        private readonly IFileReference path;

        private readonly Guid logSourceId
             = new Guid(new byte[] { 0x67, 0x23, 0x1b, 0x2e, 0xf6, 0x4b, 0xdf, 0x40, 0xac, 0xf8, 0x2, 0xc3, 0x1d, 0x7c, 0x2e, 0xf2 });
        /* {2e1b2367-4bf6-40df-acf8-02c31d7c2ef2} */

        /// <summary>
        ///     creates a new visitor
        /// </summary>
        public CompilerDirectiveVisitor(OptionSet options, IFileReference filePath, ILogManager logMgr) {
            Options = options;
            visitor = new Visitor(this);
            log = logMgr;
            path = filePath;
            logSource = new LogSource(log, logSourceId);
        }

        private static readonly Guid messageSource
            = new Guid(new byte[] { 0xcc, 0x3b, 0xd8, 0xdd, 0xbf, 0x76, 0x5f, 0x40, 0xa2, 0xe8, 0x8a, 0xbd, 0x9f, 0xb6, 0x20, 0xc4 });
        /* {ddd83bcc-76bf-405f-a2e8-8abd9fb620c4} */


        /// <summary>
        ///     file access
        /// </summary>
        public IFileAccess FileAccess
            => Options.Files;

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
        /// <param name="syntaxPart">switch to visit</param>
        public void StartVisit(AlignSwitch syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.Align.Value = syntaxPart.AlignValue;
        }


        /// <summary>
        ///     update application type
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(AppTypeParameter syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.ApplicationType.Value = syntaxPart.ApplicationType;
        }

        /// <summary>
        ///     update assertion mode
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(AssertSwitch syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.Assertions.Value = syntaxPart.Assertions;
        }

        /// <summary>
        ///     update assertion mode
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(BooleanEvaluationSwitch syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.BoolEval.Value = syntaxPart.BoolEval;
        }

        /// <summary>
        ///     update code align
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(CodeAlignParameter syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.CodeAlign.Value = syntaxPart.CodeAlign;
        }

        /// <summary>
        ///     update debug info mode
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(DebugInfoSwitch syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.DebugInfo.Value = syntaxPart.DebugInfo;
        }

        /// <summary>
        ///     define symbol
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(DefineSymbol syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            ConditionalCompilation.DefineSymbol(syntaxPart.SymbolName);
        }

        /// <summary>
        ///     undefine symbol
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(UnDefineSymbol syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            ConditionalCompilation.UndefineSymbol(syntaxPart.SymbolName);
        }

        /// <summary>
        ///     conditional compilation ("ifdef")
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(IfDef syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            if (syntaxPart.Negate)
                ConditionalCompilation.AddIfNDefCondition(syntaxPart.SymbolName);
            else
                ConditionalCompilation.AddIfDefCondition(syntaxPart.SymbolName);
        }

        /// <summary>
        ///     conditional compilation ("endif")
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(EndIf syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;


            if (ConditionalCompilation.HasConditions) {
                ConditionalCompilation.RemoveIfDefCondition();
            }
            else {
                LogSource.Error(CompilerDirectiveParserErrors.EndIfWithoutIf, syntaxPart);
            }
        }

        /// <summary>
        ///     deny unit in package switch
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(ParseDenyPackageUnit syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            ConditionalCompilation.DenyInPackages.Value = syntaxPart.DenyUnit;
        }

        /// <summary>
        ///     description
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(Description syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            Meta.Description.Value = syntaxPart.DescriptionValue;
        }

        /// <summary>
        ///     <c>$if</c>
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(IfDirective syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            ConditionalCompilation.AddIfCondition(false);
        }

        /// <summary>
        ///     conditional compilation ("else")
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(ElseDirective syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            if (ConditionalCompilation.HasConditions) {
                ConditionalCompilation.AddElseCondition();
            }
            else {
                LogSource.Error(CompilerDirectiveParserErrors.ElseIfWithoutIf, syntaxPart);
            }
        }

        /// <summary>
        ///     design time only
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(DesignOnly syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            ConditionalCompilation.DesignOnly.Value = syntaxPart.DesignTimeOnly;
        }

        /// <summary>
        ///     extension
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(Extension syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            Meta.FileExtension.Value = syntaxPart.ExecutableExtension;
        }


        /// <summary>
        ///     extended compatibility
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(ExtendedCompatibility syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.ExtendedCompatibility.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     extended syntax
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(ExtSyntax syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.UseExtendedSyntax.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     external symbol
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(ExternalSymbolDeclaration syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            Meta.RegisterExternalSymbol(syntaxPart.IdentifierName, syntaxPart.SymbolName, syntaxPart.UnionName);
        }


        /// <summary>
        ///     excess precision
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(ExcessPrecision syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.ExcessPrecision.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     high char unicode switch
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(HighCharUnicodeSwitch syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.HighCharUnicode.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     hint switch
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(Hints syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.Hints.Value = syntaxPart.Mode;
        }


        /// <summary>
        ///     c++ header mit
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(HppEmit syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            if (syntaxPart.Mode == HppEmitMode.Undefined)
                return;

            Meta.HeaderEmit(syntaxPart.Mode, syntaxPart.EmitValue);
        }


        /// <summary>
        ///     image base
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(ImageBase syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.ImageBase.Value = syntaxPart.BaseValue;
        }

        /// <summary>
        ///     implicit build
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(ImplicitBuild syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.ImplicitBuild.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     io checks
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(IoChecks syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.IoChecks.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     local symbols
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(LocalSymbols syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.LocalSymbols.Value = syntaxPart.Mode;
        }


        /// <summary>
        ///     long strings
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(LongStrings syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.LongStrings.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     open strings
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(OpenStrings syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.OpenStrings.Value = syntaxPart.Mode;
        }


        /// <summary>
        ///        optimization
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(Optimization syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.Optimization.Value = syntaxPart.Mode;
        }


        /// <summary>
        ///        overflow
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(Overflow syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.CheckOverflows.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///         safe division
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(SafeDivide syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.SafeDivide.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///        range checks
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(RangeChecks syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.RangeChecks.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///        stack frames
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(StackFrames syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.StackFrames.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///        zero based strings
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(ZeroBasedStrings syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.IndexOfFirstCharInString.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     writable consts
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(WritableConsts syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.WritableConstants.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     weak link rtti
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(WeakLinkRtti syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.WeakLinkRtti.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     weak link rtti
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(Warnings syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.Warnings.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     warn switch
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(WarnSwitch syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            if (string.IsNullOrWhiteSpace(syntaxPart.WarningType))
                return;

            Warnings.SetModeByIdentifier(syntaxPart.WarningType, syntaxPart.Mode);
        }

        /// <summary>
        ///     var string checks
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(VarStringChecks syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.VarStringChecks.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     type chedk pointers
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(TypedPointers syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.TypedPointers.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     type chedk pointers
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(SymbolDefinitions syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;


            if (syntaxPart.Mode != SymbolDefinitionInfo.Undefined)
                CompilerOptions.SymbolDefinitions.Value = syntaxPart.Mode;

            if (syntaxPart.ReferencesMode != SymbolReferenceInfo.Undefined)
                CompilerOptions.SymbolReferences.Value = syntaxPart.ReferencesMode;
        }


        /// <summary>
        ///     type chedk pointers
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(StrongLinkTypes syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.LinkAllTypes.Value = syntaxPart.Mode;
        }


        /// <summary>
        ///     scoped enums directive
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(ScopedEnums syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.ScopedEnums.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     published rtti
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(PublishedRtti syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.PublishedRtti.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///      runtime only
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(RunOnly syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.RuntimeOnlyPackage.Value = syntaxPart.Mode;
        }


        /// <summary>
        ///      legacy if end
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(LegacyIfEnd syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.LegacyIfEnd.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///      legacy if end
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(RealCompatibility syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.RealCompatibility.Value = syntaxPart.Mode;
        }



        /// <summary>
        ///      legacy if end
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(PointerMath syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.PointerMath.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///      old type layout
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(OldTypeLayout syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.OldTypeLayout.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///      nodefine
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(NoDefine syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            if (!string.IsNullOrEmpty(syntaxPart.TypeName))
                Meta.AddNoDefine(syntaxPart.TypeName, syntaxPart.TypeNameInHpp, syntaxPart.TypeNameInUnion);
        }

        /// <summary>
        ///      obj type name
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(ObjTypeName syntaxPart) {

            if (!CanVisit(syntaxPart))
                return;


            if (!string.IsNullOrEmpty(syntaxPart.TypeName))
                Meta.AddObjectFileTypeName(syntaxPart.TypeName, syntaxPart.AliasName);
        }


        /// <summary>
        ///      nocinlude
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(NoInclude syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            if (!string.IsNullOrEmpty(syntaxPart.UnitName))
                Meta.AddNoInclude(syntaxPart.UnitName);
        }


        /// <summary>
        ///      min enum size
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(MinEnumSize syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.MinimumEnumSize.Value = syntaxPart.Size;
        }


        /// <summary>
        ///      start region
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(Region syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            if (!string.IsNullOrWhiteSpace(syntaxPart.RegionName))
                Meta.StartRegion(syntaxPart.RegionName);
        }


        /// <summary>
        ///      end region
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(EndRegion syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;


            if (Meta.Regions.Count > 0)
                Meta.StopRegion();
            else
                LogSource.Error(CompilerDirectiveParserErrors.EndRegionWithoutRegion, syntaxPart);
        }


        /// <summary>
        ///      method info
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(MethodInfo syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.MethodInfo.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///      libprefix / libsuffix / libversion
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(LibInfo syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            if (syntaxPart.LibPrefix != null)
                Meta.LibPrefix.Value = syntaxPart.LibPrefix;

            if (syntaxPart.LibSuffix != null)
                Meta.LibSuffix.Value = syntaxPart.LibSuffix;

            if (syntaxPart.LibVersion != null)
                Meta.LibVersion.Value = syntaxPart.LibVersion;

        }

        /// <summary>
        ///     pe version
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(ParsedVersion syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            switch (syntaxPart.Kind) {
                case TokenKind.SetPEOsVersion:
                    Meta.PEOsVersion.MajorVersion.Value = syntaxPart.MajorVersion;
                    Meta.PEOsVersion.MinorVersion.Value = syntaxPart.MinorVersion;
                    break;

                case TokenKind.SetPESubsystemVersion:
                    Meta.PESubsystemVersion.MajorVersion.Value = syntaxPart.MajorVersion;
                    Meta.PESubsystemVersion.MinorVersion.Value = syntaxPart.MinorVersion;
                    break;

                case TokenKind.SetPEUserVersion:
                    Meta.PEUserVersion.MajorVersion.Value = syntaxPart.MajorVersion;
                    Meta.PEUserVersion.MinorVersion.Value = syntaxPart.MinorVersion;
                    break;
            }
        }

        /// <summary>
        ///      weak package unit
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(WeakPackageUnit syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.WeakPackageUnit.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///      weak package unit
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(RttiControl syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.Rtti.Mode = syntaxPart.Mode;
            CompilerOptions.Rtti.AssignVisibility(syntaxPart.Properties, syntaxPart.Methods, syntaxPart.Fields);
        }


        /// <summary>
        ///      if opt
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(IfOpt syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            ConditionalCompilation.AddIfOptCondition(syntaxPart.SwitchKind, syntaxPart.SwitchInfo, syntaxPart.SwitchState);
        }

        /// <summary>
        ///      imported data
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(ImportedData syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            CompilerOptions.ImportedData.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///      min / max stack size
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(StackMemorySize syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            if (syntaxPart.MinStackSize != null)
                CompilerOptions.MinimumStackMemorySize.Value = syntaxPart.MinStackSize.Value;
            if (syntaxPart.MaxStackSize != null)
                CompilerOptions.MaximumStackMemorySize.Value = syntaxPart.MaxStackSize.Value;
        }

        /// <summary>
        ///      message
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(Message syntaxPart) {
            if (syntaxPart.MessageType == MessageSeverity.Undefined)
                return;

            LogSource.ProcessMessage(new LogMessage(syntaxPart.MessageType, ParserBase.ParserLogMessage, ParserBase.UserGeneratedMessage, syntaxPart.MessageType, syntaxPart.LastTerminalToken));
        }

        /// <summary>
        ///     link
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(Link syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            var basePath = path;
            var fileName = syntaxPart?.FileName;

            if (basePath == null || string.IsNullOrWhiteSpace(basePath.Path))
                return;

            if (fileName == null || string.IsNullOrWhiteSpace(fileName))
                return;

            var resolvedFile = Meta.LinkedFileResolver.ResolvePath(
                basePath,
                Meta.LinkedFileResolver.Files.ReferenceToFile(Options.Environment.StringPool, fileName));

            if (resolvedFile.IsResolved) {
                var linkedFile = new LinkedFile() {
                    OriginalFileName = syntaxPart.FileName,
                    TargetPath = resolvedFile.TargetPath
                };
                Meta.AddLinkedFile(linkedFile);
            }
        }


        /// <summary>
        ///     resource
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(Resource syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            var basePath = path;
            var fileName = syntaxPart?.FileName;

            if (basePath == null || string.IsNullOrWhiteSpace(basePath.Path))
                return;

            if (fileName == null || string.IsNullOrWhiteSpace(fileName))
                return;


            var resolvedFile = Meta.ResourceFilePathResolver.ResolvePath(
                basePath,
                Meta.ResourceFilePathResolver.Files.ReferenceToFile(Options.Environment.StringPool, fileName));

            if (resolvedFile.IsResolved) {
                var resourceReference = new ResourceReference() {
                    OriginalFileName = fileName,
                    TargetPath = resolvedFile.TargetPath,
                    RcFile = syntaxPart.RcFile
                };
                Meta.AddResourceReference(resourceReference);
            }
        }

        /// <summary>
        ///     include
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(Include syntaxPart) {
            if (!CanVisit(syntaxPart))
                return;

            var basePath = path;
            var fileName = syntaxPart?.FileName;

            if (basePath == null || string.IsNullOrWhiteSpace(basePath.Path))
                return;

            if (fileName == null || string.IsNullOrWhiteSpace(fileName))
                return;

            var targetPath = Meta.IncludePathResolver.ResolvePath(
                basePath,
                Meta.IncludePathResolver.Files.ReferenceToFile(Options.Environment.StringPool, fileName)).TargetPath;

            if (IncludeInput != null) {
                IncludeInput.Buffer.Add(targetPath, Options.Files.OpenFileForReading(targetPath));
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
