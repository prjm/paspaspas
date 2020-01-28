using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     operations on characters
    /// </summary>
    public interface ICharOperations {

        /// <summary>
        ///     convert a Unicode char to a runtime value object
        /// </summary>
        /// <param name="character">Unicode character (16 bits)</param>
        /// <param name="typeDef"></param>
        /// <returns></returns>
        IValue ToWideCharValue(ITypeDefinition typeDef, char character);

        /// <summary>
        ///     convert a ANSI char to a runtime value object
        /// </summary>
        /// <param name="character">ANSI character (8 bits)</param>
        /// <param name="typeDef"></param>
        /// <returns></returns>
        IValue ToAnsiCharValue(ITypeDefinition typeDef, byte character);


        /// <summary>
        ///     convert a Unicode char to a runtime value object
        /// </summary>
        /// <param name="character">Unicode character (16 bits)</param>
        /// <returns></returns>
        IValue ToWideCharValue(char character);
    }
}