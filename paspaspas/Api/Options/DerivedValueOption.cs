namespace PasPasPas.Api.Options {

    /// <summary>
    ///     derived option for single values
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DerivedValueOption<T> : DerivedOptionBase {

        private bool hasValue = false;

        /// <summary>
        ///     parent option
        /// </summary>
        public DerivedValueOption<T> Parent;

        /// <summary>
        ///     creates a root option without parent
        /// </summary>
        public DerivedValueOption() : this(null) {
        }

        /// <summary>
        ///     creates a derived option
        /// </summary>
        /// <param name="parentOption">parent option</param>
        public DerivedValueOption(DerivedValueOption<T> parentOption) : base() {
            Parent = parentOption;
        }

        /// <summary>
        ///     test if the parent's value is overriden
        /// </summary>
        public override bool OverwritesDefaultValue
            => hasValue;

        /// <summary>
        ///     reset to default value
        /// </summary>
        public override void ResetToDefault() {
            hasValue = false;
            optionValue = default(T);
        }

        private T optionValue;

        /// <summary>
        ///     option value
        /// </summary>
        public T Value
        {
            get
            {
                if (hasValue)
                    return optionValue;

                if (Parent != null)
                    return Parent.Value;

                return default(T);
            }
            set
            {
                hasValue = true;
                optionValue = value;
            }
        }
    }
}
