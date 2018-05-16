namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     accessor kind
    /// </summary>
    public enum StructurePropertyAccessorKind {

        /// <summary>
        ///     undefined kind
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     read access
        /// </summary>
        Read = 1,

        /// <summary>
        ///     write access
        /// </summary>
        Write = 2,

        /// <summary>
        ///     add access
        /// </summary>
        Add = 3,

        /// <summary>
        ///     remove access
        /// </summary>
        Remove = 4,

        /// <summary>
        ///     readonly 
        /// </summary>
        ReadOnly = 5,

        /// <summary>
        ///     writeonly
        /// </summary>
        WriteOnly = 6,

        /// <summary>
        ///     display id
        /// </summary>
        DispId = 7,

        /// <summary>
        ///     stored property value
        /// </summary>
        Stored = 8,

        /// <summary>
        ///     defaultr property
        /// </summary>
        Default = 9,

        /// <summary>
        ///     nodefault property
        /// </summary>
        NoDefault = 10,

        /// <summary>
        ///     implements property
        /// </summary>
        Implements = 11,
    }
}