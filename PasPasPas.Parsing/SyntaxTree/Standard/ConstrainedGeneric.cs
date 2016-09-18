namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     generic constraint
    /// </summary>
    public class ConstrainedGeneric : SyntaxPartBase {

        /// <summary>
        ///     class constraints
        /// </summary>
        public bool ClassConstraint { get; set; }

        /// <summary>
        ///     constraint identifier
        /// </summary>
        public Identifier ConstraintIdentifier { get; set; }

        /// <summary>
        ///     constructor constraint
        /// </summary>
        public bool ConstructorConstraint { get; set; }

        /// <summary>
        ///     record constraint
        /// </summary>
        public bool RecordConstraint { get; set; }

    }
}