namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     string serialization registry
    /// </summary>
    public interface IStringRegistry {

        /// <summary>
        ///     get string by id
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        string this[uint index] { get; }

        /// <summary>
        ///     get a string id by the string
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        uint this[string index] { get; }

    }
}