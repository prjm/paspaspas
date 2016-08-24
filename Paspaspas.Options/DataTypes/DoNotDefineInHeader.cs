namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     marker to skip a type definiion in a hpp file
    /// </summary>
    public class DoNotDefineInHeader {

        /// <summary>
        ///     create a new object to skip header generation of certain cpp types
        /// </summary>
        /// <param name="typeName">type name to skip</param>
        /// <param name="typeNameInUnion">type name in unions to skip</param>
        /// <param name="nameInHpp">header file name</param>
        public DoNotDefineInHeader(string typeName, string nameInHpp, string typeNameInUnion) {
            TypeName = typeName;
            UnionTypeName = typeNameInUnion;
            HppName = nameInHpp;
        }

        /// <summary>
        ///     header file name
        /// </summary>
        public string HppName { get; }

        /// <summary>
        ///     type alias name
        /// </summary>
        public string TypeName { get; }

        /// <summary>
        /// type alis name for unions
        /// </summary>
        public string UnionTypeName { get; }
    }
}