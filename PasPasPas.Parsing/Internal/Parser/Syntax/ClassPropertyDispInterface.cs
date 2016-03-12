using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     property accessor for disp interfaces
    /// </summary>
    public class ClassPropertyDispInterface : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ClassPropertyDispInterface(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     Disp id directive
        /// </summary>
        public DispIdDirective DispId { get; internal set; }

        /// <summary>
        ///     readonly
        /// </summary>
        public bool ReadOnly { get; internal set; }

        /// <summary>
        ///    write only
        /// </summary>
        public bool WriteOnly { get; internal set; }

        /// <summary>
        ///     format 
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (ReadOnly) {
                result.Keyword("readonly");
                result.Punct(";");
                return;
            }

            if (WriteOnly) {
                result.Keyword("writeonly");
                result.Punct(";");
                return;
            }

            DispId.ToFormatter(result);
        }
    }
}