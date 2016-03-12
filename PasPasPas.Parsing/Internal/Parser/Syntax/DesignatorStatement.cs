using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     designator
    /// </summary>
    public class DesignatorStatement : ComposedPart<DesignatorItem> {

        /// <summary>
        ///     create a new syntax element
        /// </summary>
        /// <param name="informationProvider"></param>
        public DesignatorStatement(IParserInformationProvider informationProvider) : base(informationProvider) {
        }

        /// <summary>
        ///     inherited
        /// </summary>
        public bool Inherited { get; internal set; }

        /// <summary>
        ///     name
        /// </summary>
        public NamespaceName Name { get; internal set; }

        /// <summary>
        ///     format designator statemet
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (Inherited) {
                result.Keyword("inherited").Space();
            }
            result.Part(Name);
            FlattenToPascal(result, null);
        }
    }
}