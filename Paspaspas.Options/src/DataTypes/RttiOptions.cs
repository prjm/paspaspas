#nullable disable
using PasPasPas.Globals.Options;
using PasPasPas.Globals.Options.DataTypes;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     options for extended rtti generation
    /// </summary>
    public class RttiOptions : DerivedOptionBase, IRttiOptions {

        /// <summary>
        ///     generate a new set of rtti options
        /// </summary>
        /// <param name="baseOptions"></param>
        public RttiOptions(IRttiOptions baseOptions)
            => BaseOptions = baseOptions;

        /// <summary>
        ///     generate a new set of rtti options
        /// </summary>
        public RttiOptions() : this(null) { }

        /// <summary>
        ///     base options
        /// </summary>
        public IRttiOptions BaseOptions { get; }


        /// <summary>
        ///     rtti generation mode
        /// </summary>
        public RttiGenerationMode Mode { get; set; }

        /// <summary>
        ///     check if a custom rtti mode is set
        /// </summary>
        public override bool OverwritesDefaultValue
            => Mode != RttiGenerationMode.Undefined;

        /// <summary>
        ///     reset to default node
        /// </summary>
        public override void ResetToDefault() {
            Mode = RttiGenerationMode.Undefined;
            Properties.ResetToDefault();
            Methods.ResetToDefault();
            Fields.ResetToDefault();
        }

        /// <summary>
        ///     rtti settings for methods
        /// </summary>
        public RttiForVisibility Methods { get; }
            = new RttiForVisibility();

        /// <summary>
        ///     rtti settings for properties
        /// </summary>
        public RttiForVisibility Properties { get; }
        = new RttiForVisibility();

        /// <summary>
        ///     rtti settings for fields
        /// </summary>
        public RttiForVisibility Fields { get; }
        = new RttiForVisibility();

        /// <summary>
        ///     assign a visibility
        /// </summary>
        /// <param name="properties">properties</param>
        /// <param name="methods">methods</param>
        /// <param name="fields">fields</param>
        public void AssignVisibility(RttiForVisibility properties, RttiForVisibility methods, RttiForVisibility fields) {
            if (properties != null)
                Properties.Assign(properties);

            if (methods != null)
                Methods.Assign(methods);

            if (fields != null)
                Fields.Assign(fields);
        }
    }
}
