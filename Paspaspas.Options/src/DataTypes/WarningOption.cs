using PasPasPas.Globals.Options.DataTypes;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     specific warning option
    /// </summary>
    public class WarningOption : DerivedOptionBase {

        /// <summary>
        ///     create a new warning opton
        /// </summary>
        /// <param name="number">warning number</param>
        /// <param name="ident">warning ident</param>
        /// <param name="warningOptions">parent options</param>
        public WarningOption(WarningOptions warningOptions, string number, string ident)
            : this(warningOptions, new WarningKey(number, ident)) { }

        /// <summary>
        ///     create a new warning option
        /// </summary>
        /// <param name="forKey">warning ky</param>
        /// <param name="warningOptions">parent options</param>
        public WarningOption(WarningOptions warningOptions, WarningKey forKey) {
            Key = forKey;
            ParentOptions = warningOptions;
        }

        /// <summary>
        ///     warning key
        /// </summary>
        public WarningKey Key { get; }


        /// <summary>
        ///     parent options
        /// </summary>
        public WarningOptions ParentOptions { get; }

        private WarningMode mode = WarningMode.Default;

        /// <summary>
        ///     warning mode
        /// </summary>
        public WarningMode Mode {

            get {
                if (mode == WarningMode.Default) {
                    if (ParentOptions.ParentOptions != null)
                        return ParentOptions.ParentOptions.GetModeByKey(Key);
                    else
                        return WarningMode.Undefined;
                }

                return mode;
            }

            set => mode = value;
        }

        /// <summary>
        ///     checks if this setting overwrites the default setting
        /// </summary>
        public override bool OverwritesDefaultValue
            => Mode != WarningMode.Default;

        /// <summary>
        ///     reset warning mode
        /// </summary>
        public override void ResetToDefault() => Mode = WarningMode.Default;
    }
}
