namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     link options
    /// </summary>
    public class LinkOptions {

        /// <summary>
        ///     create a new set of linking options
        /// </summary>
        /// <param name="baseOptions"></param>
        public LinkOptions(LinkOptions baseOptions) {
            ApplicationType = new DerivedValueOption<AppType>(baseOptions?.ApplicationType);
            ExportCppObjects = new DerivedValueOption<ExportCppObjectMode>(baseOptions?.ExportCppObjects);
            ImageBase = new DerivedValueOption<ulong>(baseOptions?.ImageBase);
            MinimumStackMemorySize = new DerivedValueOption<ulong>(baseOptions?.MinimumStackMemorySize);
            MaximumStackMemorySize = new DerivedValueOption<ulong>(baseOptions?.MaximumStackMemorySize);
            LinkAllTypes = new DerivedValueOption<StrongTypeLinking>(baseOptions?.LinkAllTypes);
            WeakPackageUnit = new DerivedValueOption<WeakPackaging>(baseOptions?.WeakPackageUnit);
            RuntimeOnlyPackage = new DerivedValueOption<RuntimePackageMode>(baseOptions?.RuntimeOnlyPackage);
            WeakLinkRtti = new DerivedValueOption<RttiLinkMode>(baseOptions?.WeakLinkRtti);
        }

        /// <summary>
        ///     Application type
        /// </summary>
        public DerivedValueOption<AppType> ApplicationType { get; }

        /// <summary>
        ///     export all cpp objects
        /// </summary>
        public DerivedValueOption<ExportCppObjectMode> ExportCppObjects { get; }

        /// <summary>
        ///     image base address
        /// </summary>
        public DerivedValueOption<ulong> ImageBase { get; }

        /// <summary>
        ///     maximum stack memory size
        /// </summary>
        public DerivedValueOption<ulong> MaximumStackMemorySize { get; }

        /// <summary>
        ///     minimum stack memory size
        /// </summary>
        public DerivedValueOption<ulong> MinimumStackMemorySize { get; }

        /// <summary>
        ///     flag to link all types
        /// </summary>
        public DerivedValueOption<StrongTypeLinking> LinkAllTypes { get; }

        /// <summary>
        ///     weak unit packaging
        /// </summary>
        public DerivedValueOption<WeakPackaging> WeakPackageUnit { get; }

        /// <summary>
        ///     switch to prohibit this package at design time
        /// </summary>
        public DerivedValueOption<RuntimePackageMode> RuntimeOnlyPackage { get; }

        /// <summary>
        ///     weak rtti linking
        /// </summary>
        public DerivedValueOption<RttiLinkMode> WeakLinkRtti { get; }

        /// <summary>
        ///     clear link options
        /// </summary>
        public void Clear() {
            ApplicationType.ResetToDefault();
            ExportCppObjects.ResetToDefault();
            ImageBase.ResetToDefault();
            MinimumStackMemorySize.ResetToDefault();
            MaximumStackMemorySize.ResetToDefault();
            LinkAllTypes.ResetToDefault();
            WeakPackageUnit.ResetToDefault();
            RuntimeOnlyPackage.ResetToDefault();
            WeakLinkRtti.ResetToDefault();
        }

        /// <summary>
        ///     reset on a new unit
        /// </summary>
        public void ResetOnNewUnit() {
            WeakPackageUnit.ResetToDefault();
            RuntimeOnlyPackage.ResetToDefault();
            WeakLinkRtti.ResetToDefault();
        }
    }
}
