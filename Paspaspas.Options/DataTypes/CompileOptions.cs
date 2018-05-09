namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     compile-specific options
    /// </summary>
    public class CompileOptions {

        /// <summary>
        ///     compiler-specific options
        /// </summary>
        /// <param name="baseOptions"></param>
        public CompileOptions(CompileOptions baseOptions) {
            Align = new DerivedValueOption<Alignment>(baseOptions?.Align);
            ApplicationType = new DerivedValueOption<AppType>(baseOptions?.ApplicationType);
            BoolEval = new DerivedValueOption<BooleanEvaluation>(baseOptions?.BoolEval);
            CodeAlign = new DerivedValueOption<CodeAlignment>(baseOptions?.CodeAlign);
            Assertions = new DerivedValueOption<AssertionMode>(baseOptions?.Assertions);
            DebugInfo = new DerivedValueOption<DebugInformation>(baseOptions?.DebugInfo);
            ExportCppObjects = new DerivedValueOption<ExportCppObjects>(baseOptions?.ExportCppObjects);
            ExtendedCompatibility = new DerivedValueOption<ExtendedCompatibilityMode>(baseOptions?.ExtendedCompatibility);
            UseExtendedSyntax = new DerivedValueOption<ExtendedSyntax>(baseOptions?.UseExtendedSyntax);
            ExcessPrecision = new DerivedValueOption<ExcessPrecisionForResult>(baseOptions?.ExcessPrecision);
            HighCharUnicode = new DerivedValueOption<HighCharsUnicode>(baseOptions?.HighCharUnicode);
            Hints = new DerivedValueOption<CompilerHint>(baseOptions?.Hints);
            ImageBase = new DerivedValueOption<ulong>(baseOptions?.ImageBase);
            ImplicitBuild = new DerivedValueOption<ImplicitBuildUnit>(baseOptions?.ImplicitBuild);
            ImportedData = new DerivedValueOption<ImportGlobalUnitData>(baseOptions?.ImportedData);
            IoChecks = new DerivedValueOption<IoCallCheck>(baseOptions?.IoChecks);
            LocalSymbols = new DerivedValueOption<LocalDebugSymbols>(baseOptions?.LocalSymbols);
            LongStrings = new DerivedValueOption<LongStringTypes>(baseOptions?.LongStrings);
            OpenStrings = new DerivedValueOption<OpenStringTypes>(baseOptions?.OpenStrings);
            Optimization = new DerivedValueOption<CompilerOptimization>(baseOptions?.Optimization);
            CheckOverflows = new DerivedValueOption<RuntimeOverflowCheck>(baseOptions?.CheckOverflows);
            SafeDivide = new DerivedValueOption<FDivSafeDivide>(baseOptions?.SafeDivide);
            RangeChecks = new DerivedValueOption<RuntimeRangeChecks>(baseOptions?.RangeChecks);
            StackFrames = new DerivedValueOption<StackFrameGeneration>(baseOptions?.StackFrames);
            IndexOfFirstCharInString = new DerivedValueOption<FirstCharIndex>(baseOptions?.IndexOfFirstCharInString);
            WritableConstants = new DerivedValueOption<ConstantValue>(baseOptions?.WritableConstants);
            WeakLinkRtti = new DerivedValueOption<RttiLinkMode>(baseOptions?.WeakLinkRtti);
            WeakPackageUnit = new DerivedValueOption<WeakPackaging>(baseOptions?.WeakPackageUnit);
            Warnings = new DerivedValueOption<CompilerWarning>(baseOptions?.Warnings);
            VarStringChecks = new DerivedValueOption<ShortVarStringCheck>(baseOptions?.VarStringChecks);
            TypedPointers = new DerivedValueOption<UsePointersWithTypeChecking>(baseOptions?.TypedPointers);
            SymbolDefinitions = new DerivedValueOption<SymbolDefinitionInfo>(baseOptions?.SymbolDefinitions);
            SymbolReferences = new DerivedValueOption<SymbolReferenceInfo>(baseOptions?.SymbolReferences);
            LinkAllTypes = new DerivedValueOption<StrongTypeLinking>(baseOptions?.LinkAllTypes);
            ScopedEnums = new DerivedValueOption<RequireScopedEnums>(baseOptions?.ScopedEnums);
            PublishedRtti = new DerivedValueOption<RttiForPublishedProperties>(baseOptions?.PublishedRtti);
            RuntimeOnlyPackage = new DerivedValueOption<RuntimePackageMode>(baseOptions?.RuntimeOnlyPackage);
            Rtti = new RttiOptions(baseOptions?.Rtti);
            RealCompatibility = new DerivedValueOption<Real48>(baseOptions?.RealCompatibility);
            PointerMath = new DerivedValueOption<PointerManipulation>(baseOptions?.PointerMath);
            OldTypeLayout = new DerivedValueOption<OldRecordTypes>(baseOptions?.OldTypeLayout);
            MinimumEnumSize = new DerivedValueOption<EnumSize>(baseOptions?.MinimumEnumSize);
            MethodInfo = new DerivedValueOption<MethodInfoRtti>(baseOptions?.MethodInfo);
            MinimumStackMemorySize = new DerivedValueOption<ulong>(baseOptions?.MinimumStackMemorySize);
            MaximumStackMemorySize = new DerivedValueOption<ulong>(baseOptions?.MaximumStackMemorySize);
            LegacyIfEnd = new DerivedValueOption<EndIfMode>(baseOptions?.LegacyIfEnd);
        }

        /// <summary>
        ///     image base address
        /// </summary>
        public DerivedValueOption<ulong> ImageBase { get; }

        /// <summary>
        ///     value alignment
        /// </summary>
        public DerivedValueOption<Alignment> Align { get; }

        /// <summary>
        ///     Application type
        /// </summary>
        public DerivedValueOption<AppType> ApplicationType { get; }

        /// <summary>
        ///     Assertion mode
        /// </summary>
        public DerivedValueOption<AssertionMode> Assertions { get; }

        /// <summary>
        ///     boolean evaluation style
        /// </summary>
        public DerivedValueOption<BooleanEvaluation> BoolEval { get; }

        /// <summary>
        ///     code alignment
        /// </summary>
        public DerivedValueOption<CodeAlignment> CodeAlign { get; }

        /// <summary>
        ///     debug info
        /// </summary>
        public DerivedValueOption<DebugInformation> DebugInfo { get; }

        /// <summary>
        ///     excess precision on x64
        /// </summary>
        public DerivedValueOption<ExcessPrecisionForResult> ExcessPrecision { get; }

        /// <summary>
        ///     export all cpp objects
        /// </summary>
        public DerivedValueOption<ExportCppObjects> ExportCppObjects { get; }

        /// <summary>
        ///     exteded compatibility mode
        /// </summary>
        public DerivedValueOption<ExtendedCompatibilityMode> ExtendedCompatibility { get; }

        /// <summary>
        ///     high chars for unicode
        /// </summary>
        public DerivedValueOption<HighCharsUnicode> HighCharUnicode { get; }

        /// <summary>
        ///     enable or disable hints
        /// </summary>
        public DerivedValueOption<CompilerHint> Hints { get; }

        /// <summary>
        ///     switch to enable extended syntax
        /// </summary>
        public DerivedValueOption<ExtendedSyntax> UseExtendedSyntax { get; }

        /// <summary>
        ///     implicit build setting
        /// </summary>
        public DerivedValueOption<ImplicitBuildUnit> ImplicitBuild { get; }

        /// <summary>
        ///     option for global unit access
        /// </summary>
        public DerivedValueOption<ImportGlobalUnitData> ImportedData { get; }

        /// <summary>
        ///     io checks flag
        /// </summary>
        public DerivedValueOption<IoCallCheck> IoChecks { get; }

        /// <summary>
        ///     local symbols flag
        /// </summary>
        public DerivedValueOption<LocalDebugSymbols> LocalSymbols { get; }

        /// <summary>
        ///     flag for long strings
        /// </summary>
        public DerivedValueOption<LongStringTypes> LongStrings { get; }

        /// <summary>
        ///     flag for open strings
        /// </summary>
        public DerivedValueOption<OpenStringTypes> OpenStrings { get; }

        /// <summary>
        ///     flag to enable optimization
        /// </summary>
        public DerivedValueOption<CompilerOptimization> Optimization { get; }

        /// <summary>
        ///     flag to enable overflow checks
        /// </summary>
        public DerivedValueOption<RuntimeOverflowCheck> CheckOverflows { get; }

        /// <summary>
        ///     save divide option
        /// </summary>
        public DerivedValueOption<FDivSafeDivide> SafeDivide { get; }

        /// <summary>
        ///     generate runtime range checks
        /// </summary>
        public DerivedValueOption<RuntimeRangeChecks> RangeChecks { get; }

        /// <summary>
        ///     generate all stack frames
        /// </summary>
        public DerivedValueOption<StackFrameGeneration> StackFrames { get; }

        /// <summary>
        ///     index of first char in a string
        /// </summary>
        public DerivedValueOption<FirstCharIndex> IndexOfFirstCharInString { get; }

        /// <summary>
        ///     writeable constants
        /// </summary>
        public DerivedValueOption<ConstantValue> WritableConstants { get; }

        /// <summary>
        ///     weak rtti linking
        /// </summary>
        public DerivedValueOption<RttiLinkMode> WeakLinkRtti { get; }

        /// <summary>
        ///     weak unit packaging
        /// </summary>
        public DerivedValueOption<WeakPackaging> WeakPackageUnit { get; }

        /// <summary>
        ///     compiler warnings
        /// </summary>
        public DerivedValueOption<CompilerWarning> Warnings { get; }

        /// <summary>
        ///     var string checks
        /// </summary>
        public DerivedValueOption<ShortVarStringCheck> VarStringChecks { get; }

        /// <summary>
        ///     pointers with types
        /// </summary>
        public DerivedValueOption<UsePointersWithTypeChecking> TypedPointers { get; }

        /// <summary>
        ///     flag go generate symbol reference information
        /// </summary>
        public DerivedValueOption<SymbolReferenceInfo> SymbolReferences { get; }

        /// <summary>
        ///     flag to generate symbol definition information
        /// </summary>
        public DerivedValueOption<SymbolDefinitionInfo> SymbolDefinitions { get; }

        /// <summary>
        ///     flag to link all types
        /// </summary>
        public DerivedValueOption<StrongTypeLinking> LinkAllTypes { get; }

        /// <summary>
        ///     flag to scoped enums
        /// </summary>
        public DerivedValueOption<RequireScopedEnums> ScopedEnums { get; }

        /// <summary>
        ///     flag to generate rtti for published fields
        /// </summary>
        public DerivedValueOption<RttiForPublishedProperties> PublishedRtti { get; }

        /// <summary>
        ///     swithc to prohibit this package at desing time
        /// </summary>
        public DerivedValueOption<RuntimePackageMode> RuntimeOnlyPackage { get; }

        /// <summary>
        ///     rtti options
        /// </summary>
        public RttiOptions Rtti { get; }

        /// <summary>
        ///     switch for 48-bit doubles
        /// </summary>
        public DerivedValueOption<Real48> RealCompatibility { get; }

        /// <summary>
        ///     switch for pointer math
        /// </summary>
        public DerivedValueOption<PointerManipulation> PointerMath { get; }

        /// <summary>
        ///     switch for old record types
        /// </summary>
        public DerivedValueOption<OldRecordTypes> OldTypeLayout { get; }

        /// <summary>
        ///     minimum enum size
        /// </summary>
        public DerivedValueOption<EnumSize> MinimumEnumSize { get; }

        /// <summary>
        ///     enable or disable method info generation
        /// </summary>
        public DerivedValueOption<MethodInfoRtti> MethodInfo { get; }

        /// <summary>
        ///     maximum stack memory size
        /// </summary>
        public DerivedValueOption<ulong> MaximumStackMemorySize { get; }

        /// <summary>
        ///     mininun stack memory size
        /// </summary>
        public DerivedValueOption<ulong> MinimumStackMemorySize { get; }

        /// <summary>
        ///     legacy if / endif mode
        /// </summary>
        public DerivedValueOption<EndIfMode> LegacyIfEnd { get; }

        /// <summary>
        ///     clear options
        /// </summary>
        public void Clear() {
            Align.ResetToDefault();
            ApplicationType.ResetToDefault();
            BoolEval.ResetToDefault();
            CodeAlign.ResetToDefault();
            Assertions.ResetToDefault();
            DebugInfo.ResetToDefault();
            ExportCppObjects.ResetToDefault();
            ExtendedCompatibility.ResetToDefault();
            UseExtendedSyntax.ResetToDefault();
            ExcessPrecision.ResetToDefault();
            HighCharUnicode.ResetToDefault();
            Hints.ResetToDefault();
            ImageBase.ResetToDefault();
            ImplicitBuild.ResetToDefault();
            ImportedData.ResetToDefault();
            IoChecks.ResetToDefault();
            LocalSymbols.ResetToDefault();
            LongStrings.ResetToDefault();
            OpenStrings.ResetToDefault();
            Optimization.ResetToDefault();
            CheckOverflows.ResetToDefault();
            SafeDivide.ResetToDefault();
            RangeChecks.ResetToDefault();
            StackFrames.ResetToDefault();
            IndexOfFirstCharInString.ResetToDefault();
            WritableConstants.ResetToDefault();
            WeakLinkRtti.ResetToDefault();
            WeakPackageUnit.ResetToDefault();
            Warnings.ResetToDefault();
            VarStringChecks.ResetToDefault();
            TypedPointers.ResetToDefault();
            SymbolReferences.ResetToDefault();
            SymbolDefinitions.ResetToDefault();
            LinkAllTypes.ResetToDefault();
            ScopedEnums.ResetToDefault();
            PublishedRtti.ResetToDefault();
            RuntimeOnlyPackage.ResetToDefault();
            Rtti.ResetToDefault();
            RealCompatibility.ResetToDefault();
            PointerMath.ResetToDefault();
            OldTypeLayout.ResetToDefault();
            MinimumEnumSize.ResetToDefault();
            MethodInfo.ResetToDefault();
            MinimumStackMemorySize.ResetToDefault();
            MaximumStackMemorySize.ResetToDefault();
            LegacyIfEnd.ResetToDefault();
        }

        /// <summary>
        ///     reset compile options for a new unit
        /// </summary>
        public void ResetOnNewUnit() {
            Align.ResetToDefault();
            BoolEval.ResetToDefault();
            CodeAlign.ResetToDefault();
            Assertions.ResetToDefault();
            ExtendedCompatibility.ResetToDefault();
            ExcessPrecision.ResetToDefault();
            HighCharUnicode.ResetToDefault();
            Hints.ResetToDefault();
            ImportedData.ResetToDefault();
            IoChecks.ResetToDefault();
            LongStrings.ResetToDefault();
            OpenStrings.ResetToDefault();
            Optimization.ResetToDefault();
            CheckOverflows.ResetToDefault();
            SafeDivide.ResetToDefault();
            RangeChecks.ResetToDefault();
            StackFrames.ResetToDefault();
            IndexOfFirstCharInString.ResetToDefault();
            WritableConstants.ResetToDefault();
            WeakLinkRtti.ResetToDefault();
            WeakPackageUnit.ResetToDefault();
            Warnings.ResetToDefault();
            VarStringChecks.ResetToDefault();
            ScopedEnums.ResetToDefault();
            PublishedRtti.ResetToDefault();
            RuntimeOnlyPackage.ResetToDefault();
            Rtti.ResetToDefault();
            RealCompatibility.ResetToDefault();
            PointerMath.ResetToDefault();
            OldTypeLayout.ResetToDefault();
            MinimumEnumSize.ResetToDefault();
            MethodInfo.ResetToDefault();
            LegacyIfEnd.ResetToDefault();
        }
    }
}
