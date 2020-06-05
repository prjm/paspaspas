using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values.Other;

namespace PasPasPas.Runtime.Values.CharValues {

    /// <summary>
    ///     operations on characters
    /// </summary>
    public class CharOperations : ICharOperations {

        private readonly Lazy<IValue> invalidChar;

        /// <summary>
        ///     create a new registry provider for char data types
        /// </summary>
        /// <param name="provider"></param>
        public CharOperations(ITypeRegistryProvider provider) {
            registryProvider = provider;
            invalidChar = new Lazy<IValue>(() => new ErrorValue(provider.GetErrorType(), SpecialConstantKind.InvalidChar));
        }

        private readonly ITypeRegistryProvider registryProvider;

        /// <summary>
        ///     invalid char value
        /// </summary>
        public IValue Invalid
            => invalidChar.Value;

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

        /// <summary>
        ///     get an ANSI char
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public IValue ToAnsiCharValue(byte character)
            => ToAnsiCharValue(registryProvider.GetAnsiCharType(), character);
    }

}