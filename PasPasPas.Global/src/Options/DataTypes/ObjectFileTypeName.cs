namespace PasPasPas.Globals.Options.DataTypes {

    /// <summary>
    ///     object file type names
    /// </summary>
    public class ObjectFileTypeName {

        /// <summary>
        ///     create a new type alias
        /// </summary>
        /// <param name="typeName">type name</param>
        /// <param name="aliasName">alias name</param>
        public ObjectFileTypeName(string typeName, string aliasName) {
            TypeName = typeName;
            AliasName = aliasName;
        }

        /// <summary>
        ///     type alias for the linker
        /// </summary>
        public string AliasName { get; }

        /// <summary>
        ///     type name
        /// </summary>
        public string TypeName { get; }
    }
}