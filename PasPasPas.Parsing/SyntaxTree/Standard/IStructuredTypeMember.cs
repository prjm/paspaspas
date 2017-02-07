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
        UserAttributes Attributes { get; set; }
    }
}