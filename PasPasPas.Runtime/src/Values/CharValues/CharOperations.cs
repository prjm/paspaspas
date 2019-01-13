using PasPasPas.Globals.Runtime;

namespace PasPasPas.Runtime.Values.CharValues {

    /// <summary>
    ///     operations on characters
    /// </summary>
    public class CharOperations : ICharOperations {

        /// <summary>
        ///     get a constant ANSI char value
        /// </summary>
        /// <param name="character"></param>
        /// <param name="typeId">type id</param>
        /// <returns></returns>
        public ITypeReference ToAnsiCharValue(int typeId, byte character)
            => new AnsiCharValue(typeId, character);

        /// <summary>
        ///     get a constant wide char value
        /// </summary>
        /// <param name="character"></param>
        /// <param name="typeId">type id</param>
        /// <returns></returns>
        public ITypeReference ToWideCharValue(int typeId, char character)
            => new WideCharValue(typeId, character);

    }
}
