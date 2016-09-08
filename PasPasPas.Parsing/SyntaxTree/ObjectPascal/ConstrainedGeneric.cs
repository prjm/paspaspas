namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

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
        public PascalIdentifier ConstraintIdentifier { get; set; }

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