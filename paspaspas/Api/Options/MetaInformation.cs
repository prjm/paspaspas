namespace PasPasPas.Api.Options {

    /// <summary>
    ///     metainformation about the compiled project
    /// </summary>
    public class MetaInformation {

        /// <summary>
        ///     creates a new meta information option object
        /// </summary>
        /// <param name="baseOption"></param>
        public MetaInformation(MetaInformation baseOption) {
            Description = new DerivedValueOption<string>(baseOption?.Description);
        }

        /// <summary>
        /// 
        /// </summary>
        public DerivedValueOption<string> Description { get; }


        /// <summary>
        ///     reset on new unit
        /// </summary>
        public void ResetOnNewUnit() {
            //..
        }

        /// <summary>
        ///     reset on new file
        /// </summary>
        internal void Clear() {
            Description.ResetToDefault();
        }
    }
}