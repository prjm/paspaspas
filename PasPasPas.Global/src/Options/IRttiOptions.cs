#nullable disable
using PasPasPas.Globals.Options.DataTypes;

namespace PasPasPas.Globals.Options {

    /// <summary>
    ///     options for <c>rtti</c>
    /// </summary>
    public interface IRttiOptions {

        /// <summary>
        ///     rtti generation mode
        /// </summary>
        RttiGenerationMode Mode { get; set; }

        /// <summary>
        ///     properties options
        /// </summary>
        RttiForVisibility Properties { get; }

        /// <summary>
        ///     method options
        /// </summary>
        RttiForVisibility Methods { get; }

        /// <summary>
        ///     field options
        /// </summary>
        RttiForVisibility Fields { get; }

        /// <summary>
        ///     reset options
        /// </summary>
        void ResetToDefault();

        /// <summary>
        ///     assign visibility
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="methods"></param>
        /// <param name="fields"></param>
        void AssignVisibility(RttiForVisibility properties, RttiForVisibility methods, RttiForVisibility fields);
    }
}
