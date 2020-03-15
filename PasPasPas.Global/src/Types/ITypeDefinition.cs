namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     interface for a type definition
    /// </summary>
    public interface ITypeDefinition : ITypeSymbol {

        /// <summary>
        ///     get base type of this type definition
        /// </summary>
        BaseType BaseType { get; }

        /// <summary>
        ///     defining unit
        /// </summary>
        IUnitType DefiningUnit { get; }

        /// <summary>
        ///     type name
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     type size in bytes
        /// </summary>
        uint TypeSizeInBytes { get; }

        /// <summary>
        ///     mangled type name
        /// </summary>
        string MangledName { get; }

        /// <summary>
        ///     test if this type can be assigned from a value of another type
        /// </summary>
        /// <param name="otherType"></param>
        /// <returns></returns>
        bool CanBeAssignedFromType(ITypeDefinition otherType);
    }

    /// <summary>
    ///     type definition helper
    /// </summary>
    public static class TypeDefinitionHelper {

        /// <summary>
        ///     test if a given type definition defines a numerical type
        /// </summary>
        /// <param name="typeDefinition"></param>
        /// <returns></returns>
        public static bool IsNumericType(this ITypeDefinition typeDefinition)
            => typeDefinition.BaseType == BaseType.Integer || typeDefinition.BaseType == BaseType.Real;

        /// <summary>
        ///     test if a given type definition defines a text type
        /// </summary>
        /// <param name="typeDefinition"></param>
        /// <returns></returns>
        public static bool IsTextType(this ITypeDefinition typeDefinition)
            => typeDefinition.BaseType == BaseType.Char || typeDefinition.BaseType == BaseType.String;

        /// <summary>
        ///     test if the given type is a subrange type
        /// </summary>
        /// <param name="typeDef">type definition</param>
        /// <param name="subrangeType">subrange type</param>
        /// <returns></returns>
        public static bool IsSubrangeType(this ITypeDefinition typeDef, out ISubrangeType subrangeType) {
            if (typeDef.BaseType == BaseType.Subrange && typeDef is ISubrangeType subrange) {
                subrangeType = subrange;
                return true;
            }

            subrangeType = default;
            return false;
        }

    }

}