#nullable disable
using PasPasPas.Globals.Options;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     derived option for single values
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DerivedValueOption<T> : DerivedOptionBase, IOption<T> {

        private bool hasValue = false;

        /// <summary>
        ///     parent option
        /// </summary>
        public IOption<T> Parent { get; }

        /// <summary>
        ///     creates a root option without parent
        /// </summary>
        public DerivedValueOption() : this(null) {
        }

        /// <summary>
        ///     creates a derived option
        /// </summary>
        /// <param name="parentOption">parent option</param>
        public DerivedValueOption(IOption<T> parentOption) : base()
            => Parent = parentOption;

        /// <summary>
        ///     test if the parent's value is overridden
        /// </summary>
        public override bool OverwritesDefaultValue
            => hasValue;

        /// <summary>
        ///     reset to default value
        /// </summary>
        public override void ResetToDefault() {
            hasValue = false;
            optionValue = default;
        }

        private T optionValue;

        /// <summary>
        ///     option value
        /// </summary>
        public T Value {
            get {
                if (hasValue)
                    return optionValue;

                if (Parent != null)
                    return Parent.Value;

                return default;
            }
            set {
                hasValue = true;
                optionValue = value;
            }
        }
    }

}
