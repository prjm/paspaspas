using PasPasPas.Parsing.SyntaxTree.CompilerDirectives;
using PasPasPas.Parsing.Parser;
using PasPasPas.Options.DataTypes;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Infrastructure.Input;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     visitor to interpret compiler directives
    /// </summary>
    public class CompilerDirectiveVisitor : SyntaxPartVisitorBase<CompilerDirectiveVisitorOptions> {

        /// <summary>
        ///     test if an item can be visited
        /// </summary>
        /// <param name="syntaxPart">syntax part to test</param>
        /// <param name="parameter">options</param>
        /// <returns></returns>
        private bool CanVisit(ISyntaxPart syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            if (!parameter.ConditionalCompilation.Skip)
                return true;

            return syntaxPart is EndIf || syntaxPart is IfDef || syntaxPart is ElseDirective;
        }

        /// <summary>
        ///     visit a syntax node
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public override bool BeginVisit(ISyntaxPart syntaxPart, CompilerDirectiveVisitorOptions parameter) {

            if (!CanVisit(syntaxPart, parameter))
                return true;

            dynamic part = syntaxPart;
            BeginVisitItem(part, parameter);
            return true;
        }

        /// <summary>
        ///     other tree nodes
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(ISyntaxPart syntaxPart, CompilerDirectiveVisitorOptions parameter) { }

        /// <summary>
        ///     update alignment
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(AlignSwitch syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.Align.Value = syntaxPart.AlignValue;
        }

        /// <summary>
        ///     update application type
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(AppTypeParameter syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.ApplicationType.Value = syntaxPart.ApplicationType;
        }

        /// <summary>
        ///     update assertion mode
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(AssertSwitch syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.Assertions.Value = syntaxPart.Assertions;
        }

        /// <summary>
        ///     update assertion mode
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(BooleanEvaluationSwitch syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.BoolEval.Value = syntaxPart.BoolEval;
        }

        /// <summary>
        ///     update code align
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(CodeAlignParameter syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.CodeAlign.Value = syntaxPart.CodeAlign;
        }

        /// <summary>
        ///     update debug info mode
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(DebugInfoSwitch syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.DebugInfo.Value = syntaxPart.DebugInfo;
        }

        /// <summary>
        ///     define symbol
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(DefineSymbol syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.ConditionalCompilation.DefineSymbol(syntaxPart.SymbolName);
        }

        /// <summary>
        ///     undefine symbol
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(UnDefineSymbol syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.ConditionalCompilation.UndefineSymbol(syntaxPart.SymbolName);
        }

        /// <summary>
        ///     conditional compilation ("ifdef")
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(IfDef syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            if (syntaxPart.Negate)
                parameter.ConditionalCompilation.AddIfNDefCondition(syntaxPart.SymbolName);
            else
                parameter.ConditionalCompilation.AddIfDefCondition(syntaxPart.SymbolName);
        }

        /// <summary>
        ///     conditional compilation ("ifdef")
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(EndIf syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            if (parameter.ConditionalCompilation.HasConditions) {
                parameter.ConditionalCompilation.RemoveIfDefCondition();
            }
            else {
                parameter.LogSource.Error(CompilerDirectiveParserErrors.EndIfWithoutIf, syntaxPart);
            }
        }

        /// <summary>
        ///     deny unit in package switch
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(ParseDenyPackageUnit syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.ConditionalCompilation.DenyInPackages.Value = syntaxPart.DenyUnit;
        }

        /// <summary>
        ///     description
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(Description syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.Meta.Description.Value = syntaxPart.DescriptionValue;
        }

        /// <summary>
        ///     conditional compilation ("else")
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(ElseDirective syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            if (parameter.ConditionalCompilation.HasConditions) {
                parameter.ConditionalCompilation.AddElseCondition();
            }
            else {
                parameter.LogSource.Error(CompilerDirectiveParserErrors.ElseIfWithoutIf, syntaxPart);
            }
        }

        /// <summary>
        ///     design time only
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(DesignOnly syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.ConditionalCompilation.DesignOnly.Value = syntaxPart.DesignTimeOnly;
        }

        /// <summary>
        ///     extension
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(Extension syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.Meta.FileExtension.Value = syntaxPart.ExecutableExtension;
        }


        /// <summary>
        ///     extended compatibility
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(ExtendedCompatibility syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.ExtendedCompatibility.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     extended syntax
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(ExtSyntax syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.UseExtendedSyntax.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     external symbol
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(ExternalSymbolDeclaration syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.Meta.RegisterExternalSymbol(syntaxPart.IdentifierName, syntaxPart.SymbolName, syntaxPart.UnionName);
        }


        /// <summary>
        ///     excess precision
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(ExcessPrecision syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.ExcessPrecision.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     high char unicode switch
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(HighCharUnicodeSwitch syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.HighCharUnicode.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     hint switch
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(Hints syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.Hints.Value = syntaxPart.Mode;
        }


        /// <summary>
        ///     c++ header mit
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(HppEmit syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            if (syntaxPart.Mode == HppEmitMode.Undefined)
                return;
            parameter.Meta.HeaderEmit(syntaxPart.Mode, syntaxPart.EmitValue);
        }


        /// <summary>
        ///     image base
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(ImageBase syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.ImageBase.Value = syntaxPart.BaseValue;
        }

        /// <summary>
        ///     implicit build
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(ImplicitBuild syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.ImplicitBuild.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     io checks
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(IoChecks syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.IoChecks.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     local symbols
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(LocalSymbols syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.LocalSymbols.Value = syntaxPart.Mode;
        }


        /// <summary>
        ///     long strings
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(LongStrings syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.LongStrings.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     open strings
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(OpenStrings syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.OpenStrings.Value = syntaxPart.Mode;
        }


        /// <summary>
        ///        optimization
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(Optimization syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.Optimization.Value = syntaxPart.Mode;
        }


        /// <summary>
        ///        overflow
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(Overflow syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.CheckOverflows.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///         safe division
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(SafeDivide syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.SafeDivide.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///        range checks
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(RangeChecks syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.RangeChecks.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///        stack frames
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(StackFrames syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.StackFrames.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///        zero based strings
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(ZeroBasedStrings syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.IndexOfFirstCharInString.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     writable consts
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(WritableConsts syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.WritableConstants.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     weak link rtti
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(WeakLinkRtti syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.WeakLinkRtti.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     weak link rtti
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(Warnings syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.Warnings.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     warn switch
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(WarnSwitch syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            if (string.IsNullOrWhiteSpace(syntaxPart.WarningType))
                return;

            parameter.Warnings.SetModeByIdentifier(syntaxPart.WarningType, syntaxPart.Mode);
        }

        /// <summary>
        ///     var string checks
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(VarStringChecks syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.VarStringChecks.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     type chedk pointers
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(TypedPointers syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.TypedPointers.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     type chedk pointers
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(SymbolDefinitions syntaxPart, CompilerDirectiveVisitorOptions parameter) {

            if (syntaxPart.Mode != SymbolDefinitionInfo.Undefined)
                parameter.CompilerOptions.SymbolDefinitions.Value = syntaxPart.Mode;

            if (syntaxPart.ReferencesMode != SymbolReferenceInfo.Undefined)
                parameter.CompilerOptions.SymbolReferences.Value = syntaxPart.ReferencesMode;
        }


        /// <summary>
        ///     type chedk pointers
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(StrongLinkTypes syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.LinkAllTypes.Value = syntaxPart.Mode;
        }


        /// <summary>
        ///     scoped enums directive
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(ScopedEnums syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.ScopedEnums.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///     published rtti
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(PublishedRtti syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.PublishedRtti.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///      runtime only
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(RunOnly syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.RuntimeOnlyPackage.Value = syntaxPart.Mode;
        }


        /// <summary>
        ///      legacy if end
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(LegacyIfEnd syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.LegacyIfEnd.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///      legacy if end
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(RealCompatibility syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.RealCompatiblity.Value = syntaxPart.Mode;
        }



        /// <summary>
        ///      legacy if end
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(PointerMath syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.PointerMath.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///      old type layout
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(OldTypeLayout syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.OldTypeLayout.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///      nodefine
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(NoDefine syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            if (!string.IsNullOrEmpty(syntaxPart.TypeName))
                parameter.Meta.AddNoDefine(syntaxPart.TypeName, syntaxPart.TypeNameInHpp, syntaxPart.TypeNameInUnion);
        }

        /// <summary>
        ///      obj type name
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(ObjTypeName syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            if (!string.IsNullOrEmpty(syntaxPart.TypeName))
                parameter.Meta.AddObjectFileTypeName(syntaxPart.TypeName, syntaxPart.AliasName);
        }


        /// <summary>
        ///      nocinlude
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(NoInclude syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            if (!string.IsNullOrEmpty(syntaxPart.UnitName))
                parameter.Meta.AddNoInclude(syntaxPart.UnitName);
        }


        /// <summary>
        ///      min enum size
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(MinEnumSize syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.MinumEnumSize.Value = syntaxPart.Size;
        }


        /// <summary>
        ///      start region
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(Region syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            if (!string.IsNullOrWhiteSpace(syntaxPart.RegionName))
                parameter.Meta.StartRegion(syntaxPart.RegionName);
        }


        /// <summary>
        ///      end region
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(EndRegion syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            if (parameter.Meta.Regions.Count > 0)
                parameter.Meta.StopRegion();
            else
                parameter.LogSource.Error(CompilerDirectiveParserErrors.EndRegionWithoutRegion, syntaxPart);
        }


        /// <summary>
        ///      method info
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(MethodInfo syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.MethodInfo.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///      libprefix / libsuffix / libversion
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(LibInfo syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            if (syntaxPart.LibPrefix != null)
                parameter.Meta.LibPrefix.Value = syntaxPart.LibPrefix;

            if (syntaxPart.LibSuffix != null)
                parameter.Meta.LibSuffix.Value = syntaxPart.LibSuffix;

            if (syntaxPart.LibVersion != null)
                parameter.Meta.LibVersion.Value = syntaxPart.LibVersion;

        }

        /// <summary>
        ///     pe version
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(ParsedVersion syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            switch (syntaxPart.Kind) {
                case TokenKind.SetPEOsVersion:
                    parameter.Meta.PEOsVersion.MajorVersion.Value = syntaxPart.MajorVersion;
                    parameter.Meta.PEOsVersion.MinorVersion.Value = syntaxPart.MinorVersion;
                    break;

                case TokenKind.SetPESubsystemVersion:
                    parameter.Meta.PESubsystemVersion.MajorVersion.Value = syntaxPart.MajorVersion;
                    parameter.Meta.PESubsystemVersion.MinorVersion.Value = syntaxPart.MinorVersion;
                    break;

                case TokenKind.SetPEUserVersion:
                    parameter.Meta.PEUserVersion.MajorVersion.Value = syntaxPart.MajorVersion;
                    parameter.Meta.PEUserVersion.MinorVersion.Value = syntaxPart.MinorVersion;
                    break;
            }
        }

        /// <summary>
        ///      weak package unit
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(WeakPackageUnit syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.WeakPackageUnit.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///      weak package unit
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(RttiControl syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.Rtti.Mode = syntaxPart.Mode;
            parameter.CompilerOptions.Rtti.AssignVisibility(syntaxPart.Properties, syntaxPart.Methods, syntaxPart.Fields);
        }


        /// <summary>
        ///      if opt
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(IfOpt syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.ConditionalCompilation.AddIfOptCondition(syntaxPart.SwitchKind, syntaxPart.SwitchInfo, syntaxPart.SwitchState);
        }

        /// <summary>
        ///      imported data
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(ImportedData syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.ImportedData.Value = syntaxPart.Mode;
        }

        /// <summary>
        ///      min / max stack size
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(StackMemorySize syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            if (syntaxPart.MinStackSize != null)
                parameter.CompilerOptions.MinimumStackMemSize.Value = syntaxPart.MinStackSize.Value;
            if (syntaxPart.MaxStackSize != null)
                parameter.CompilerOptions.MaximumStackMemSize.Value = syntaxPart.MaxStackSize.Value;
        }

        /// <summary>
        ///      message
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(Message syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            if (syntaxPart.MessageType == MessageSeverity.Undefined)
                return;

            parameter.LogSource.ProcessMessage(new LogMessage(syntaxPart.MessageType, ParserBase.ParserLogMessage, ParserBase.UserGeneratedMessage, syntaxPart.MessageType, syntaxPart.LastTerminalToken));
        }

        /// <summary>
        ///     link
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(Link syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            var basePath = syntaxPart?.LastTerminalToken?.FilePath;
            var fileName = syntaxPart?.FileName;

            if (basePath == null || string.IsNullOrWhiteSpace(basePath.Path))
                return;

            if (fileName == null || string.IsNullOrWhiteSpace(fileName))
                return;


            var resolvedFile = parameter.Meta.LinkedFileResolver.ResolvePath(basePath, new FileReference(fileName));

            if (resolvedFile.IsResolved) {
                var linkedFile = new LinkedFile();
                linkedFile.OriginalFileName = syntaxPart.FileName;
                linkedFile.TargetPath = resolvedFile.TargetPath;
                parameter.Meta.AddLinkedFile(linkedFile);
            }
        }


        /// <summary>
        ///     resource
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(Resource syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            var basePath = syntaxPart?.LastTerminalToken?.FilePath;
            var fileName = syntaxPart?.FileName;

            if (basePath == null || string.IsNullOrWhiteSpace(basePath.Path))
                return;

            if (fileName == null || string.IsNullOrWhiteSpace(fileName))
                return;


            var resolvedFile = parameter.Meta.ResourceFilePathResolver.ResolvePath(basePath, new FileReference(fileName));

            if (resolvedFile.IsResolved) {
                var resourceReference = new ResourceReference();
                resourceReference.OriginalFileName = fileName;
                resourceReference.TargetPath = resolvedFile.TargetPath;
                resourceReference.RcFile = syntaxPart.RcFile;
                parameter.Meta.AddResourceReference(resourceReference);
            }
        }

        /// <summary>
        ///     include
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(Include syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            var basePath = syntaxPart?.LastTerminalToken?.FilePath;
            var fileName = syntaxPart?.FileName;

            if (basePath == null || string.IsNullOrWhiteSpace(basePath.Path))
                return;

            if (fileName == null || string.IsNullOrWhiteSpace(fileName))
                return;


            var targetPath = parameter.Meta.IncludePathResolver.ResolvePath(basePath, new FileReference(fileName)).TargetPath;

            IFileAccess fileAccess = parameter.FileAccess;
            if (parameter.IncludeInput != null)
                parameter.IncludeInput.AddFile(fileAccess.OpenFileForReading(targetPath));

        }

    }
}
