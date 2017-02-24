﻿namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     kind of structured type
    /// </summary>
    public enum StructuredTypeKind {

        /// <summary>
        ///     undefined type
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     class type
        /// </summary>
        Class = 1,

        /// <summary>
        ///     record type
        /// </summary>
        Record = 2,

        /// <summary>
        ///     record helper
        /// </summary>
        RecordHelper = 3,

        /// <summary>
        ///     object type
        /// </summary>
        Object = 4,

        /// <summary>
        ///     interface declaration
        /// </summary>
        Interface = 5,

        /// <summary>
        ///     display interface declaration
        /// </summary>
        DispInterface = 6,

        /// <summary>
        ///     classhelper
        /// </summary>
        ClassHelper = 7,
    }
}
