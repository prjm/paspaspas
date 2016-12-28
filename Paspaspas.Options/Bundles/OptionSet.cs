using System;
using PasPasPas.Options.DataTypes;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Infrastructure.Log;

namespace PasPasPas.Options.Bundles {

    /// <summary>
    ///     set of compiler options
    /// </summary>
    public class OptionSet : IOptionSet {

        private static readonly Guid messageSource
            = new Guid(new byte[] { 0x21, 0x78, 0xd, 0xe5, 0x2f, 0xb2, 0xca, 0x42, 0xbf, 0x47, 0x71, 0xf1, 0x11, 0x1b, 0x31, 0xc9 });
        /* {e50d7821-b22f-42ca-bf47-71f1111b31c9} */


        /// <summary>
        ///     open ifdef / ifndef
        /// </summary>
        public static readonly Guid PendingCondition
            = new Guid(new byte[] { 0x31, 0xb0, 0xe3, 0x39, 0x3e, 0x22, 0xef, 0x42, 0xb7, 0x4f, 0x88, 0x18, 0x47, 0x17, 0x2b, 0x6d });
        /* {39e3b031-223e-42ef-b74f-881847172b6d} */

        /// <summary>
        ///     pending region
        /// </summary>
        public static readonly Guid PendingRegion
            = new Guid(new byte[] { 0xdc, 0x6e, 0x35, 0x28, 0x76, 0xcd, 0xba, 0x47, 0xbe, 0x3, 0xf5, 0x37, 0x3c, 0x5, 0x8b, 0xbf });
        /* {28356edc-cd76-47ba-be03-f5373c058bbf} */

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
        public void ResetOnNewUnit(ILogManager logManager) {
            var logSource = new LogSource(logManager, messageSource);

            CompilerOptions.ResetOnNewUnit();
            ConditionalCompilation.ResetOnNewUnit(logSource);
            Meta.ResetOnNewUnit(logSource);
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
                    return GetSwitchInfo(CompilerOptions.IoChecks, IoCallCheck.EnableIoChecks, IoCallCheck.DisableIoChecks);
                case 'J':
                    return GetSwitchInfo(CompilerOptions.WritableConstants, ConstantValue.Writable, ConstantValue.Constant);
                case 'H':
                    return GetSwitchInfo(CompilerOptions.LongStrings, LongStringTypes.EnableLongStrings, LongStringTypes.DisableLongStrings);
                case 'L':
                    return GetSwitchInfo(CompilerOptions.LocalSymbols, LocalDebugSymbols.EnableLocalSymbols, LocalDebugSymbols.DisableLocalSymbols);
                case 'M':
                    return GetSwitchInfo(CompilerOptions.PublishedRtti, RttiForPublishedProperties.Enable, RttiForPublishedProperties.Disable);
                case 'O':
                    return GetSwitchInfo(CompilerOptions.Optimization, CompilerOptimization.EnableOptimization, CompilerOptimization.DisableOptimization);
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
                    return GetSwitchInfo(CompilerOptions.MinimumEnumSize, EnumSize.FourByte, EnumSize.OneByte);

                default:
                    return SwitchInfo.Undefined;
            }
        }
    }
}