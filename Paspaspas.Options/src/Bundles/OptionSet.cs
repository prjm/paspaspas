using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Log;
using PasPasPas.Options.DataTypes;

namespace PasPasPas.Options.Bundles {

    /// <summary>
    ///     set of compiler options
    /// </summary>
    public class OptionSet : IOptionSet {

        /// <summary>
        ///     open ifdef / ifndef
        /// </summary>
        public const uint PendingCondition
            = 0x0301;

        /// <summary>
        ///     pending region
        /// </summary>
        public const uint PendingRegion
            = 0x0302;

        /// <summary>
        ///     debug configuration
        /// </summary>
        public const string DebugConfigurationName = "Debug";

        /// <summary>
        ///     release configuration
        /// </summary>
        public const string ReleaseConfigurationName = "Release";

        /// <summary>
        ///     creates a new option set
        /// </summary>
        /// <param name="environment">environment</param>
        public OptionSet(IEnvironment environment) : this(null, environment) { }

        /// <summary>
        ///     create a new option set
        /// </summary>
        public OptionSet(OptionSet baseOptions, IEnvironment environment) {
            Environment = environment;
            LogSource = environment.Log.CreateLogSource(MessageGroups.OptionSet);
            CompilerOptions = new CompileOptions(baseOptions?.CompilerOptions);
            ConditionalCompilation = new ConditionalCompilationOptions(baseOptions?.ConditionalCompilation);
            Meta = new MetaInformation(this, baseOptions?.Meta);
            PathOptions = new PathOptionSet(baseOptions?.PathOptions);
            Warnings = new WarningOptions(baseOptions?.Warnings);
        }

        /// <summary>
        ///     compiler-related options
        /// </summary>
        public CompileOptions CompilerOptions { get; }

        /// <summary>
        ///     conditional compilation options
        /// </summary>
        public ConditionalCompilationOptions ConditionalCompilation { get; }

        /// <summary>
        ///     meta information
        /// </summary>
        public MetaInformation Meta { get; }

        /// <summary>
        ///     path options
        /// </summary>
        public PathOptionSet PathOptions { get; }

        /// <summary>
        ///     warning options
        /// </summary>
        public WarningOptions Warnings { get; }

        /// <summary>
        ///     used environment
        /// </summary>
        public IEnvironment Environment { get; }

        /// <summary>
        ///     log source
        /// </summary>
        public ILogSource LogSource { get; }

        /// <summary>
        ///     clear all option values
        /// </summary>
        public void Clear() {
            CompilerOptions.Clear();
            ConditionalCompilation.Clear();
            Meta.Clear();
            PathOptions.Clear();
            Warnings.Clear();
        }

        /// <summary>
        ///     reset definitions for a new unit
        /// </summary>
        public void ResetOnNewUnit() {
            CompilerOptions.ResetOnNewUnit();
            ConditionalCompilation.ResetOnNewUnit(LogSource);
            Meta.ResetOnNewUnit(LogSource);
            Warnings.ResetOnNewUnit();
        }

        private static SwitchInfo GetSwitchInfo<T>(DerivedValueOption<T> value, T enabled, T disabled)
            where T : struct {
            if (value.Value.Equals(enabled))
                return SwitchInfo.Enabled;
            if (value.Value.Equals(disabled))
                return SwitchInfo.Disabled;
            return SwitchInfo.Undefined;
        }


        /// <summary>
        ///     get switch information
        /// </summary>
        /// <param name="switchKind"></param>
        /// <returns></returns>
        public SwitchInfo GetSwitchInfo(string switchKind) {
            if (string.IsNullOrEmpty(switchKind) || switchKind.Length != 1)
                return SwitchInfo.Undefined;

            var toggle = switchKind.ToUpperInvariant()[0];

            switch (toggle) {
                case 'A':
                    return GetSwitchInfo(CompilerOptions.CodeGeneration.Align, Alignment.QuadWord, Alignment.Unaligned);
                case 'B':
                    return GetSwitchInfo(CompilerOptions.Syntax.BoolEval, BooleanEvaluation.CompleteEvaluation, BooleanEvaluation.ShortEvaluation);
                case 'C':
                    return GetSwitchInfo(CompilerOptions.DebugOptions.Assertions, AssertionMode.EnableAssertions, AssertionMode.DisableAssertions);
                case 'D':
                    return GetSwitchInfo(CompilerOptions.DebugOptions.DebugInfo, DebugInformation.IncludeDebugInformation, DebugInformation.NoDebugInfo);
                case 'G':
                    return GetSwitchInfo(CompilerOptions.DebugOptions.ImportedData, ImportGlobalUnitData.DoImport, ImportGlobalUnitData.NoImport);
                case 'I':
                    return GetSwitchInfo(CompilerOptions.RuntimeChecks.IoChecks, IoCallCheck.EnableIoChecks, IoCallCheck.DisableIoChecks);
                case 'J':
                    return GetSwitchInfo(CompilerOptions.Syntax.WritableConstants, ConstantValue.Writable, ConstantValue.Constant);
                case 'H':
                    return GetSwitchInfo(CompilerOptions.Syntax.LongStrings, LongStringMode.EnableLongStrings, LongStringMode.DisableLongStrings);
                case 'L':
                    return GetSwitchInfo(CompilerOptions.DebugOptions.LocalSymbols, LocalDebugSymbolMode.EnableLocalSymbols, LocalDebugSymbolMode.DisableLocalSymbols);
                case 'M':
                    return GetSwitchInfo(CompilerOptions.CodeGeneration.PublishedRtti, RttiForPublishedPropertieMode.Enable, RttiForPublishedPropertieMode.Disable);
                case 'O':
                    return GetSwitchInfo(CompilerOptions.CodeGeneration.Optimization, CompilerOptimization.EnableOptimization, CompilerOptimization.DisableOptimization);
                case 'P':
                    return GetSwitchInfo(CompilerOptions.Syntax.OpenStrings, OpenStringTypeMode.EnableOpenStrings, OpenStringTypeMode.DisableOpenStrings);
                case 'Q':
                    return GetSwitchInfo(CompilerOptions.RuntimeChecks.CheckOverflows, RuntimeOverflowCheck.EnableChecks, RuntimeOverflowCheck.DisableChecks);
                case 'R':
                    return GetSwitchInfo(CompilerOptions.RuntimeChecks.RangeChecks, RuntimeRangeCheckMode.EnableRangeChecks, RuntimeRangeCheckMode.DisableRangeChecks);
                case 'T':
                    return GetSwitchInfo(CompilerOptions.Syntax.TypedPointers, UsePointersWithTypeChecking.Enable, UsePointersWithTypeChecking.Disable);
                case 'U':
                    return GetSwitchInfo(CompilerOptions.CodeGeneration.SafeDivide, FDivSafeDivide.EnableSafeDivide, FDivSafeDivide.DisableSafeDivide);
                case 'V':
                    return GetSwitchInfo(CompilerOptions.Syntax.VarStringChecks, ShortVarStringCheck.EnableChecks, ShortVarStringCheck.DisableChecks);
                case 'W':
                    return GetSwitchInfo(CompilerOptions.CodeGeneration.StackFrames, StackFrameGeneration.EnableFrames, StackFrameGeneration.DisableFrames);
                case 'X':
                    return GetSwitchInfo(CompilerOptions.Syntax.UseExtendedSyntax, ExtendedSyntax.UseExtendedSyntax, ExtendedSyntax.NoExtendedSyntax);
                case 'Y':
                    return GetSwitchInfo(CompilerOptions.DebugOptions.SymbolReferences, SymbolReferenceInfo.Enable, SymbolReferenceInfo.Disable);
                case 'Z':
                    return GetSwitchInfo(CompilerOptions.CodeGeneration.MinimumEnumSize, EnumSize.FourByte, EnumSize.OneByte);

                default:
                    return SwitchInfo.Undefined;
            }
        }
    }
}