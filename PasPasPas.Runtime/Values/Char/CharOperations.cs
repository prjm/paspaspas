using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values.Char {

    /// <summary>
    ///     operations on characters
    /// </summary>
    public class CharOperations : ICharOperations {


        /// <summary>
        ///     get a constant wide char value
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public ITypeReference ToWideCharValue(char character)
            => new WideCharValue(character);

    }
}
