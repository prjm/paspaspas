using System;
using PasPasPas.Infrastructure.Service;
using PasPasPas.Options.DataTypes;

namespace PasPasPas.Options.Bundles {

    /// <summary>
    ///     set of compiler options
    /// </summary>
    public class OptionSet : ServiceBase, IService, IOptionSet {

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
        public OptionSet() : this(null) { }

        /// <summary>
        ///     create a new option set
        /// </summary>
        /// <param name="baseOptions"></param>
        public OptionSet(OptionSet baseOptions) {
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
        ///     return service class id
        /// </summary>
        public Guid ServiceClassId => StandardServices.CompilerConfigurationServiceClass;

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

        private static SwitchInfo GetSwitchInfo<T>(T value, T enabled, T disabled)
            where T : struct {
            if (value.Equals(enabled))
                return SwitchInfo.Enabled;
            if (value.Equals(disabled))
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
                    return GetSwitchInfo(CompilerOptions.Align.Value, Alignment.QuadWord, Alignment.Unaligned);
                default:
                    return SwitchInfo.Undefined;
            }
        }
    }
}