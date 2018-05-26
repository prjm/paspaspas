namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     interface for structural type members
    /// </summary>
    public interface IStructuredTypeMember {

        /// <summary>
        ///     static member
        /// </summary>
        bool ClassItem { get; set; }

        /// <summary>
        ///     member attributes
        /// </summary>
        SyntaxPartBase Attributes1 { get; set; }

        /// <summary>
        ///     member attributes
        /// </summary>
        SyntaxPartBase Attributes2 { get; set; }

    }
}