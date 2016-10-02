﻿namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     one part of a variant part of a record
    /// </summary>
    public class RecordVariant : SyntaxPartBase {

        /// <summary>
        ///     field list
        /// </summary>
        public RecordFieldList FieldList { get; set; }


    }
}