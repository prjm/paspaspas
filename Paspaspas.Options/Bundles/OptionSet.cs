using System;
using PasPasPas.Options.DataTypes;
using PasPasPas.Infrastructure.Input;

namespace PasPasPas.Options.Bundles {

    /// <summary>
    ///     set of compiler options
    /// </summary>
    public class OptionSet : IOptionSet {

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
        /// <param name="fileAccess">file access</param>
        public OptionSet(IFileAccess fileAccess) : this(null, fileAccess) { }

        /// <summary>
        ///     create a new option set
        /// </summary>
        /// <param name="baseOptions"></param>
        /// <param name="fileAccess">file access</param>
        public OptionSet(OptionSet baseOptions, IFileAccess fileAccess) {
            Files = fileAccess;
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
        ///     option service id
        /// </summary>
        public static readonly Guid OptionSetServiceId
            = new Guid("E177A4B4-012F-4929-A084-B341D23BCC12");

        /// <summary>
        ///     service id
        /// </summary>
        public Guid ServiceId => OptionSetServiceId;

        /// <summary>
        ///     service name
        /// </summary>
        public string ServiceName => "CompilerOptions";

        /// <summary>
        ///     path options
        /// </summary>
        public PathOptionSet PathOptions { get; }

        /// <summary>
        ///     warning optiosn
        /// </summary>
        public WarningOptions Warnings { get; }

        /// <summary>
        ///     file access
        /// </summary>
        public IFileAccess Files { get; }

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
            ConditionalCompilation.ResetOnNewUnit();
            Meta.ResetOnNewUnit();
            PathOptions.ResetOnNewUnit();
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
                    return GetSwitchInfo(CompilerOptions.Align, Alignment.QuadWord, Alignment.Unaligned);
                case 'B':
                    return GetSwitchInfo(CompilerOptions.BoolEval, BooleanEvaluation.CompleteEvaluation, BooleanEvaluation.ShortEvaluation);
                case 'C':
                    return GetSwitchInfo(CompilerOptions.Assertions, AssertionMode.EnableAssertions, AssertionMode.DisableAssertions);
                case 'D':
                    return GetSwitchInfo(CompilerOptions.DebugInfo, DebugInformation.IncludeDebugInformation, DebugInformation.NoDebugInfo);
                case 'G':
                    return GetSwitchInfo(CompilerOptions.ImportedData, ImportGlobalUnitData.DoImport, ImportGlobalUnitData.NoImport);
                case 'I':
                    return GetSwitchInfo(CompilerOptions.IoChecks, IoCallChecks.EnableIoChecks, IoCallChecks.DisableIoChecks);
                case 'J':
                    return GetSwitchInfo(CompilerOptions.WritableConstants, ConstantValues.Writable, ConstantValues.Constant);
                case 'H':
                    return GetSwitchInfo(CompilerOptions.LongStrings, LongStringTypes.EnableLongStrings, LongStringTypes.DisableLongStrings);
                case 'L':
                    return GetSwitchInfo(CompilerOptions.LocalSymbols, LocalDebugSymbols.EnableLocalSymbols, LocalDebugSymbols.DisableLocalSymbols);
                case 'M':
                    return GetSwitchInfo(CompilerOptions.PublishedRtti, RttiForPublishedProperties.Enable, RttiForPublishedProperties.Disable);
                case 'O':
                    return GetSwitchInfo(CompilerOptions.Optimization, CompilerOptmization.EnableOptimization, CompilerOptmization.DisableOptimization);
                case 'P':
                    return GetSwitchInfo(CompilerOptions.OpenStrings, OpenStringTypes.EnableOpenStrings, OpenStringTypes.DisableOpenStrings);
                case 'Q':
                    return GetSwitchInfo(CompilerOptions.CheckOverflows, RuntimeOverflowChecks.EnableChecks, RuntimeOverflowChecks.DisableChecks);
                case 'R':
                    return GetSwitchInfo(CompilerOptions.RangeChecks, RuntimeRangeChecks.EnableRangeChecks, RuntimeRangeChecks.DisableRangeChecks);
                case 'T':
                    return GetSwitchInfo(CompilerOptions.TypedPointers, TypeCheckedPointers.Enable, TypeCheckedPointers.Disable);
                case 'U':
                    return GetSwitchInfo(CompilerOptions.SafeDivide, FDivSafeDivide.EnableSafeDivide, FDivSafeDivide.DisableSafeDivide);
                case 'V':
                    return GetSwitchInfo(CompilerOptions.VarStringChecks, ShortVarStringChecks.EnableChecks, ShortVarStringChecks.DisableChecks);
                case 'W':
                    return GetSwitchInfo(CompilerOptions.StackFrames, StackFrameGeneration.EnableFrames, StackFrameGeneration.DisableFrames);
                case 'X':
                    return GetSwitchInfo(CompilerOptions.UseExtendedSyntax, ExtendedSyntax.UseExtendedSyntax, ExtendedSyntax.NoExtendedSyntax);
                case 'Y':
                    return GetSwitchInfo(CompilerOptions.SymbolReferences, SymbolReferenceInfo.Enable, SymbolReferenceInfo.Disable);
                case 'Z':
                    return GetSwitchInfo(CompilerOptions.MinumEnumSize, EnumSize.FourByte, EnumSize.OneByte);

                default:
                    return SwitchInfo.Undefined;
            }
        }
    }
}