#nullable disable
namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     base class for derived options
    /// </summary>
    public abstract class DerivedOptionBase : IDerivedOption {

        /// <summary>
        ///     <c>true</c> if the default value is overriden
        /// </summary>
        public abstract bool OverwritesDefaultValue { get; }

        /// <summary>
        ///     reset to the default value
        /// </summary>
        public abstract void ResetToDefault();
    }

}
