using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.CharValues {

    /// <summary>
    ///     operations on characters
    /// </summary>
    public class CharOperations : ICharOperations {

        /// <summary>
        ///     create a new registry provider for char data types
        /// </summary>
        /// <param name="registryProvider"></param>
        public CharOperations(ITypeRegistryProvider registryProvider)
            => registryProvider = registryProvider;

        private ITypeRegistryProvider registryProvider;

        /// <summary>
        ///     get a constant ANSI char value
        /// </summary>
        /// <param name="character"></param>
        /// <param name="typeDef">type id</param>
        /// <returns></returns>
        public IValue ToAnsiCharValue(ITypeDefinition typeDef, byte character)
            => new AnsiCharValue(typeDef, character);

        /// <summary>
        ///     get a constant wide char value
        /// </summary>
        /// <param name="character"></param>
        /// <param name="typeDef">type id</param>
        /// <returns></returns>
        public IValue ToWideCharValue(ITypeDefinition typeDef, char character)
            => new WideCharValue(typeDef, character);

        /// <summary>
        ///     get a wide char
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public IValue ToWideCharValue(char character)
            => ToWideCharValue(registryProvider.GetWideCharType(), character);
    }

}