namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     operations on characters
    /// </summary>
    public interface ICharOperations {

        /// <summary>
        ///     convert a char to a runtime value object
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        IValue ToWideCharValue(char character);


    }
}