using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     provides logical operations and relational operations for boolean
    ///     values
    /// </summary>
    public interface IBooleanOperations : ILogicalOperations, IRelationalOperations {

        /// <summary>
        ///     value of <c>true</c>
        /// </summary>
        IBooleanValue TrueValue { get; }

        /// <summary>
        ///     value of <c>false</c>
        /// </summary>
        IBooleanValue FalseValue { get; }

        /// <summary>
        ///     invalid <c>boolean</c> value
        /// </summary>
        IValue Invalid { get; }

        /// <summary>
        ///     convert a boolean constant value to a value object
        /// </summary>
        /// <param name="value">boolean value</param>
        /// <param name="typeDef">type def</param>
        /// <returns><c>TrueValue</c> or <c>FalseValue</c></returns>
        IBooleanValue ToBoolean(bool value, ITypeDefinition typeDef);


    }
}
