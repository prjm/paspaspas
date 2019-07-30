using PasPasPas.Globals.Options.DataTypes;

namespace PasPasPas.Globals.Options {

    /// <summary>
    ///     linking options
    /// </summary>
    public interface ILinkOptions {

        /// <summary>
        ///     application type
        /// </summary>
        IOption<AppType> ApplicationType { get; }

        /// <summary>
        ///     image base address
        /// </summary>
        IOption<ulong> ImageBase { get; }

        /// <summary>
        ///     rtti link mode
        /// </summary>
        IOption<RttiLinkMode> WeakLinkRtti { get; }

        /// <summary>
        ///     link all types
        /// </summary>
        IOption<StrongTypeLinking> LinkAllTypes { get; }

        /// <summary>
        ///     export cpp objects
        /// </summary>
        IOption<ExportCppObjectMode> ExportCppObjects { get; }

        /// <summary>
        ///     minimum stack size
        /// </summary>
        IOption<ulong> MinimumStackMemorySize { get; }

        /// <summary>
        ///     maximum stack size
        /// </summary>
        IOption<ulong> MaximumStackMemorySize { get; }

        /// <summary>
        ///     weak package unit
        /// </summary>
        IOption<WeakPackaging> WeakPackageUnit { get; }

        /// <summary>
        ///     runtime only package option
        /// </summary>
        IOption<RuntimePackageMode> RuntimeOnlyPackage { get; }

        /// <summary>
        ///     clear options
        /// </summary>
        void Clear();

        /// <summary>
        ///     clear options on a new unit
        /// </summary>
        void ResetOnNewUnit();
    }
}