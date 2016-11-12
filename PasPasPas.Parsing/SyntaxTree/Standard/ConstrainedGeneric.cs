using PasPasPas.Parsing.SyntaxTree.Abstract;
using System;

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

        /// <summary>
        ///     map a constraint kind
        /// </summary>
        /// <returns></returns>
        public GenericConstraintKind MapKind() {
            if (RecordConstraint)
                return GenericConstraintKind.Record;
            else if (ClassConstraint)
                return GenericConstraintKind.Class;
            else if (ConstructorConstraint)
                return GenericConstraintKind.Constructor;
            else if (ConstraintIdentifier != null)
                return GenericConstraintKind.Identifier;
            else
                return GenericConstraintKind.Unknown;
        }
    }
}