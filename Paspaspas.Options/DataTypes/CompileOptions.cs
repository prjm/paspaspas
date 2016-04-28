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
            ExtendedCompatibility = new DerivedValueOption<ExtendedCompatiblityMode>(baseOptions?.ExtendedCompatibility);
            UseExtendedSyntax = new DerivedValueOption<ExtendedSyntax>(baseOptions?.UseExtendedSyntax);
            ExcessPrecision = new DerivedValueOption<ExcessPrecisionForResults>(baseOptions?.ExcessPrecision);
            HighCharUnicode = new DerivedValueOption<HighCharsUnicode>(baseOptions?.HighCharUnicode);
            Hints = new DerivedValueOption<CompilerHints>(baseOptions?.Hints);
            ImageBase = new DerivedValueOption<int>(baseOptions?.ImageBase);
            ImplicitBuild = new DerivedValueOption<ImplicitBuildUnit>(baseOptions?.ImplicitBuild);
            ImportedData = new DerivedValueOption<ImportGlobalUnitData>(baseOptions?.ImportedData);
            IoChecks = new DerivedValueOption<IoCallChecks>(baseOptions?.IoChecks);
            LocalSymbols = new DerivedValueOption<LocalDebugSymbols>(baseOptions?.LocalSymbols);
            LongStrings = new DerivedValueOption<LongStringTypes>(baseOptions?.LongStrings);
            OpenStrings = new DerivedValueOption<OpenStringTypes>(baseOptions?.OpenStrings);
            Optimization = new DerivedValueOption<CompilerOptmization>(baseOptions?.Optimization);
            CheckOverflows = new DerivedValueOption<RuntimeOverflowChecks>(baseOptions?.CheckOverflows);
            SafeDivide = new DerivedValueOption<FDivSafeDivide>(baseOptions?.SafeDivide);
            RangeChecks = new DerivedValueOption<RuntimeRangeChecks>(baseOptions?.RangeChecks);
            StackFrames = new DerivedValueOption<StackFrameGeneration>(baseOptions?.StackFrames);
            IndexOfFirstCharInString = new DerivedValueOption<FirstCharIndex>(baseOptions?.IndexOfFirstCharInString);
            WriteableConstants = new DerivedValueOption<ConstantValues>(baseOptions?.WriteableConstants);
            WeakLinkRtti = new DerivedValueOption<RttiLinkMode>(baseOptions?.WeakLinkRtti);
            WeakPackageUnit = new DerivedValueOption<WeakPackaging>(baseOptions?.WeakPackageUnit);
            Warnings = new DerivedValueOption<CompilerWarnings>(baseOptions?.Warnings);
            VarStringChecks = new DerivedValueOption<ShortVarStringChecks>(baseOptions?.VarStringChecks);
            TypedPointers = new DerivedValueOption<TypeCheckedPointers>(baseOptions?.TypedPointers);
            SymbolDefinitions = new DerivedValueOption<SymbolDefinitionInfo>(baseOptions?.SymbolDefinitions);
            SymbolReferences = new DerivedValueOption<SymbolReferenceInfo>(baseOptions?.SymbolReferences);
            LinkAllTypes = new DerivedValueOption<StrongTypeLinking>(baseOptions?.LinkAllTypes);
            ScopedEnums = new DerivedValueOption<RequireScopedEnums>(baseOptions?.ScopedEnums);
            PublishedRtti = new DerivedValueOption<RttiForPublishedProperties>(baseOptions?.PublishedRtti);
            RuntimeOnlyPackage = new DerivedValueOption<RuntimePackageMode>(baseOptions?.RuntimeOnlyPackage);
        }

        /// <summary>
        ///     image base address
        /// </summary>
        public DerivedValueOption<int> ImageBase { get; }

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
        public DerivedValueOption<ExcessPrecisionForResults> ExcessPrecision { get; }

        /// <summary>
        ///     export all cpp objects
        /// </summary>
        public DerivedValueOption<ExportCppObjects> ExportCppObjects { get; }

        /// <summary>
        ///     exteded compatibility mode
        /// </summary>
        public DerivedValueOption<ExtendedCompatiblityMode> ExtendedCompatibility { get; }

        /// <summary>
        ///     high chars for unicode
        /// </summary>
        public DerivedValueOption<HighCharsUnicode> HighCharUnicode { get; }

        /// <summary>
        ///     enable or disable hints
        /// </summary>
        public DerivedValueOption<CompilerHints> Hints { get; }

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
        public DerivedValueOption<IoCallChecks> IoChecks { get; }

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
        public DerivedValueOption<CompilerOptmization> Optimization { get; }

        /// <summary>
        ///     flag to enable overflow checks
        /// </summary>
        public DerivedValueOption<RuntimeOverflowChecks> CheckOverflows { get; }

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
        public DerivedValueOption<ConstantValues> WriteableConstants { get; }

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
        public DerivedValueOption<CompilerWarnings> Warnings { get; }

        /// <summary>
        ///     var string checks
        /// </summary>
        public DerivedValueOption<ShortVarStringChecks> VarStringChecks { get; }

        /// <summary>
        ///     pointers with types
        /// </summary>
        public DerivedValueOption<TypeCheckedPointers> TypedPointers { get; }

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
            WriteableConstants.ResetToDefault();
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
            WriteableConstants.ResetToDefault();
            WeakLinkRtti.ResetToDefault();
            WeakPackageUnit.ResetToDefault();
            Warnings.ResetToDefault();
            VarStringChecks.ResetToDefault();
            ScopedEnums.ResetToDefault();
            PublishedRtti.ResetToDefault();
            RuntimeOnlyPackage.ResetToDefault();
        }
    }
}
