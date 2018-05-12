namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     operations on characters
    /// </summary>
    public interface ICharOperations {

        /// <summary>
        ///     convert a char to a runtime value object
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        ITypeReference ToWideCharValue(char character);


    }
}