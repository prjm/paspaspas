using System;
using PasPasPas.Globals.Log;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Options.DataTypes;

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
        /// <param name="environment">environment</param>
        public OptionSet(IBasicEnvironment environment) : this(null, environment) { }

        /// <summary>
        ///     create a new option set
        /// </summary>
        public OptionSet(OptionSet baseOptions, IBasicEnvironment environment) {
            Environment = environment;
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
        public IBasicEnvironment Environment { get; private set; }

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