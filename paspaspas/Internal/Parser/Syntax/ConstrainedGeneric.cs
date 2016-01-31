using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     generic constraint
    /// </summary>
    public class ConstrainedGeneric : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ConstrainedGeneric(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     class constraints
        /// </summary>
        public bool ClassConstraint { get; internal set; }

        /// <summary>
        ///     constraint identifier
        /// </summary>
        public PascalIdentifier ConstraintIdentifier { get; internal set; }

        /// <summary>
        ///     constructor constraint
        /// </summary>
        public bool ConstructorConstraint { get; internal set; }

        /// <summary>
        ///     record constraint
        /// </summary>
        public bool RecordConstraint { get; internal set; }

        /// <summary>
        ///     format constraint
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {

            if (ClassConstraint)
                result.Keyword("class");
            else if (RecordConstraint)
                result.Keyword("record");
            else if (ConstructorConstraint)
                result.Keyword("constructor");
            else
                result.Identifier(ConstraintIdentifier.Value);

        }
    }
}