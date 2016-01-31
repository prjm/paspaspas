using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     interface definition
    /// </summary>
    public class InterfaceDefinition : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public InterfaceDefinition(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     <c>true</c> if dispinterface
        /// </summary>
        public bool DispInterface { get; internal set; }

        /// <summary>
        ///     guid declaration
        /// </summary>
        public InterfaceGuid Guid { get; internal set; }

        /// <summary>
        ///     interface items
        /// </summary>
        public InterfaceItems Items { get; internal set; }

        /// <summary>
        ///     parent interface
        /// </summary>
        public ParentClass ParentInterface { get; internal set; }

        /// <summary>
        ///     format interface
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (DispInterface) {
                result.Keyword("dispinterface");
            }
            else {
                result.Keyword("interface");
            }
            result.Space();
            result.Part(ParentInterface);
            result.StartIndent();
            result.NewLine();
            result.Part(Guid);
            result.NewLine();
            result.Part(Items);
            result.EndIndent();
            result.Keyword("end");
        }
    }
}